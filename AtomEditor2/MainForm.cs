#define LOGGING

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Kirishima16.Libraries.AtomEditor;
using Kirishima16.Libraries.MP4Box;

namespace Kirishima16.Applications.AtomEditor2
{
	using Res = Properties.Resources;
	using System.Drawing;

	public partial class MainForm : Form
	{
		#region 内部変数
		Regex asciiregex = new Regex(@"^[\w\d]{4}$");
		Encoding encAscii = Encoding.ASCII;
		byte[] bufMain = new byte[1024];
		FileStream fsSave;
		BoxNode boxRoot;
		BoxNode boxCurrent = null;
		int cntTempFile;
		string tmpdir;
		string sourceFile;
		uint sourceFileLength;
		readonly byte[] openbuf = new byte[4];
		readonly string PLUGIN_DIR = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AtomEditor\Plugins");
		List<BoxLongPair> offsetBackup = new List<BoxLongPair>();
		List<IBoxDetailReader> boxDetailReaders = new List<IBoxDetailReader>();
		List<Type> boxEditorTypes = new List<Type>();
		List<IBoxEditor> openedBoxEditors = new List<IBoxEditor>();
		List<BoxNode> openedBoxes = new List<BoxNode>();
		bool tviewBoxDoubleClicked;

#if LOGGING
		StreamWriter swLog;
#endif
		#endregion
		#region 初期化系メソッド・イベントハンドラ
		/// <summary>
		/// MainFormを初期化します。
		/// </summary>
		public MainForm()
		{
#if LOGGING
			try {
				FileStream fs = File.Open(Path.Combine(Application.StartupPath, "AtomEditor.log"), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
				swLog = new StreamWriter(fs);
			} catch {
				MessageBox.Show(Res.msgOpenLogFileFailed);
			}
#endif
			InitializeComponent();
			tmpdir = Path.GetTempFileName();
			File.Delete(tmpdir);
			Directory.CreateDirectory(tmpdir);

			//組み込みプラグインを読み込み
			boxDetailReaders.Add(new BasicBoxDecoder());
			boxEditorTypes.Add(typeof(BinaryBoxEditor));

			//外部プラグインを読み込み
			LoadPlugins();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			imglTree.Images.AddRange(new Image[]{
				Res.project_bl_b, Res.fol_cl_y, Res.fol_op_y,
				Res.file_bl_b, Res.file_pen_b
			});
		}

		/// <summary>
		/// プラグインファイルを読み込みます。
		/// </summary>
		private void LoadPlugins()
		{
			List<Assembly> pasms = new List<Assembly>();
			string[] files;
			if (!Directory.Exists(PLUGIN_DIR)) {
				Directory.CreateDirectory(PLUGIN_DIR);
			} else {
				string ibdrname = typeof(IBoxDetailReader).FullName;
				string ibename = typeof(IBoxEditor).FullName;
				string ctrlname = typeof(Control).FullName;
				files = Directory.GetFiles(PLUGIN_DIR, "*.dll");
				foreach (string file in files) {
					Assembly asm = Assembly.LoadFile(file);
					Type[] types = asm.GetTypes();
					bool loaded = false;
					foreach (Type t in types) {
						if (!t.IsClass || !t.IsPublic || t.IsAbstract) {
							continue;
						}
						Object obj = Activator.CreateInstance(t);
						if (obj is IBoxDetailReader) {
							boxDetailReaders.Add(obj as IBoxDetailReader);
							loaded = true;
						} else if (obj is Control && obj is IBoxEditor) {
							boxEditorTypes.Add(t);
							loaded = true;
						}
					}
					if (loaded) {
						pasms.Add(asm);
					}
				}
			}
			Program.PluginedAssemblies = pasms.ToArray();
		}
		#endregion
		#region 内部処理メソッド
		#region ファイルを開く関連
		private void OpenFile(string file)
		{
			//TODO:保存確認する
			tabcMain.TabPages.Clear();
			openedBoxes.Clear();
			openedBoxEditors.Clear();

			sourceFile = file;
			string fname = Path.GetFileName(file);
			this.Text = string.Format(Res.tplFormTextLoading, fname);
			prgStatus.Value = 0;

			//一時ファイル削除
			UpdateStatus(Res.msgRemoveTmpFiles);
			RemoveTempFiles();

			//ファイル形式確認
			UpdateStatus(Res.msgOpeningFile + sourceFile);
			UpdateStatus(Res.msgCheckingFile);
			if (!CheckFileType(file)) {
				return;
			}

			//Box解析
			UpdateStatus(Res.msgParsingFile);
			if (!ParseBoxFromRoot()) {
				return;
			}

			//オフセット情報バックアップ
			UpdateStatus(Res.msgBackingUpOffset);
			BackupOffsetData();

			//ツリービューに表示
			UpdateStatus(Res.msgRenderingNodeToTree);
			tviewBox.Nodes.Clear();
			RenderAtomTree(boxRoot, tviewBox.Nodes);
			tviewBox.ExpandAll();
			tviewBox.SelectedNode = tviewBox.Nodes[0];

			OpenBox<BinaryBoxEditor>(boxRoot);

			this.Text = string.Format(Res.tplFormText, Path.GetFileName(sourceFile));
		}

		private void RemoveTempFiles()
		{
			prgStatus.Value = 0;
			string[] files = Directory.GetFiles(tmpdir);
			int cnt = 0, all = files.Length;
			foreach (string f in files) {
				File.Delete(f);
				cnt += 100;
				prgStatus.Value = cnt / all;
			}
		}

		private bool CheckFileType(string file)
		{
			FileStream fsCheckFile = File.OpenRead(file);
			fsCheckFile.Read(bufMain, 0, bufMain.Length);
			if (encAscii.GetString(bufMain, 4, 4) == "ftyp") {
				sourceFileLength = (uint)fsCheckFile.Length;
				return true;
			}
			UpdateStatus(Res.msgNotMp4File);
			MessageBox.Show(Res.msgNotMp4File, Res.opeOpenFile);
			return false;
		}

		private bool ParseBoxFromRoot()
		{
			boxRoot = new BoxNode();
			boxRoot.BoxName = Res.sysRootNode;
			boxRoot.Length = sourceFileLength;
			boxRoot.SourcePosition = 0;
			boxRoot.DumpFile = sourceFile;
			try {
				ParseBox(boxRoot);
			} catch {
				UpdateStatus(Res.msgOpenReadFileFailed);
				MessageBox.Show(Res.msgOpenReadFileFailed, Res.opeOpenFile);
				return false;
			}
			return true;
		}

		private void BackupOffsetData()
		{
			offsetBackup.Clear();

			List<BoxNode> stcoList = new List<BoxNode>();
			List<BoxNode> trunList = new List<BoxNode>();
			List<BoxNode> tfraList = new List<BoxNode>();
			List<BoxNode> mdatList = new List<BoxNode>();
			Dictionary<string, List<BoxNode>> cdic = new Dictionary<string, List<BoxNode>>();
			cdic.Add("stco", stcoList);
			cdic.Add("trun", trunList);
			cdic.Add("tfra", tfraList);
			cdic.Add("mdat", mdatList);
			EnumNodeByName(boxRoot, cdic);
			byte[] tmp = new byte[4];
			foreach (BoxNode stco in stcoList) {
				byte[] buf = File.ReadAllBytes(stco.DumpFile);
				for (int i = 0x10; i < stco.Length; i += 0x04) {
					Array.Copy(buf, i, tmp, 0, 4);
					Array.Reverse(tmp);
					uint pos = BitConverter.ToUInt32(tmp, 0);
					foreach (BoxNode mdat in mdatList) {
						if (mdat.SourcePosition <= pos && pos < mdat.SourcePosition + mdat.Length) {
							offsetBackup.Add(new BoxLongPair(stco, i, mdat, pos - mdat.SourcePosition));
							break;
						}
					}
				}
			}
			foreach (BoxNode trun in trunList) {
				byte[] buf = File.ReadAllBytes(trun.DumpFile);
				if ((buf[0x0B] & 0x01) == 0x01) {
					Array.Copy(buf, 0x10, tmp, 0, 4);
					Array.Reverse(tmp);
					uint pos = BitConverter.ToUInt32(tmp, 0);
					foreach (BoxNode mdat in mdatList) {
						if (mdat.SourcePosition <= pos && pos < mdat.SourcePosition + mdat.Length) {
							offsetBackup.Add(new BoxLongPair(trun, 0x10, mdat, pos - mdat.SourcePosition));
						}
					}
				}
			}
			foreach (BoxNode tfra in tfraList) {
				byte[] buf = File.ReadAllBytes(tfra.DumpFile);
				Array.Copy(buf, 0x10, tmp, 0, 4);
				Array.Reverse(tmp);
				uint n = BitConverter.ToUInt32(tmp, 0);
				uint n1 = ((n >> 4) & 3) + 1;
				uint n2 = ((n >> 2) & 3) + 1;
				uint n3 = (n & 3) + 1;
				Array.Copy(buf, 0x14, tmp, 0, 4);
				Array.Reverse(tmp);
				n = BitConverter.ToUInt32(tmp, 0);
				for (int i = 0; i < n; i++) {
					int num = (int)(0x18 + i * (8 + n1 + n2 + n3));
					Array.Copy(buf, num, tmp, 0, 4);
					Array.Reverse(tmp);
					uint pos = BitConverter.ToUInt32(tmp, 0);
					foreach (BoxNode mdat in mdatList) {
						if (mdat.SourcePosition <= pos && pos < mdat.SourcePosition + mdat.Length) {
							offsetBackup.Add(new BoxLongPair(tfra, num, mdat, pos - mdat.SourcePosition));
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// 指定されたBoxを再帰的に解析します。
		/// </summary>
		/// <param name="node">ソースファイルを参照するルートBox、もしくは子Box</param>
		private void ParseBox(BoxNode node)
		{
			using (FileStream fs = File.OpenRead(node.DumpFile)) {
				if (node.BoxName != Res.sysRootNode)
					fs.Seek(8, SeekOrigin.Current);

				while (fs.Position < fs.Length) {
					Application.DoEvents();
					//ファイルを開く
					string fnc = Path.Combine(tmpdir, cntTempFile.ToString("D8"));
					uint len;
					long pos = fs.Position + node.SourcePosition;
					string name;
					BoxNode anc;
					using (FileStream fsc = File.OpenWrite(fnc)) {
						cntTempFile++;

						//ビッグエンディアンで長さを取得
						fs.Read(openbuf, 0, 4);
						fsc.Write(openbuf, 0, 4);
						Array.Reverse(openbuf);
						len = BitConverter.ToUInt32(openbuf, 0);
						if (len > fs.Length - (fs.Position - 4)) {
							return;
						} else if (len == 0) {
							len = (uint)(fs.Length - (fs.Position - 4));
						}

						//名前を取得
						fs.Read(openbuf, 0, 4);
						fsc.Write(openbuf, 0, 4);
						name = Encoding.ASCII.GetString(openbuf, 0, 4);
						//半角英数4文字でなければ親Boxへもどる
						if (name.IndexOf('\0') >= 0 || !asciiregex.IsMatch(name)) {
							return;
						}

						//ファイルに保存(本体部分)
						for (long i = 8; i < len; i += bufMain.Length) {
							int l = (int)((i + bufMain.Length < len) ? bufMain.Length : len - i);
							fs.Read(bufMain, 0, l);
							fsc.Write(bufMain, 0, l);
							prgStatus.Value = (int)((node.Position + fs.Position) * 100 / boxRoot.Length);
						}
						fsc.Close();
					}

					//子Box追加
					anc = new BoxNode();
					anc.DumpFile = fnc;
					anc.BoxName = name;
					anc.Length = len;
					anc.SourcePosition = pos;
					anc.Position = pos;
					anc.Parent = node;
					node.Children.Add(anc);

					//再帰してみる
					ParseBox(anc);
				}
			}

			if (node.BoxName != Res.sysRootNode && node.Children.Count > 0) {
				using (FileStream fs = File.OpenWrite(node.DumpFile)) {
					fs.SetLength(8);
					fs.Close();
				}
			}
		}
		#endregion
		#region Box検索関連
		/// <summary>
		/// 指定された名前のBoxを再帰的に列挙します。
		/// </summary>
		/// <param name="node">検索を開始するBox</param>
		/// <param name="cdic">名前と格納先のコレクションのディクショナリ</param>
		private void EnumNodeByName(BoxNode node, Dictionary<string, List<BoxNode>> cdic)
		{
			if (cdic.ContainsKey(node.BoxName)) {
				cdic[node.BoxName].Add(node);
			}
			foreach (BoxNode n in node.Children) {
				EnumNodeByName(n, cdic);
			}
		}

		/// <summary>
		/// 指定されたノードより後方のBoxを再帰的に列挙します。
		/// </summary>
		/// <param name="node">検索を開始するBox</param>
		/// <param name="targetNode">検索するBox</param>
		/// <param name="nl">見つかったBoxを格納するBoxNodeコレクション</param>
		private void EnumNodeAfterNode(BoxNode node, BoxNode targetNode, List<BoxNode> nl)
		{
			bool flg = false;
			EnumNodeAfterNode(node, targetNode, nl, ref flg);
		}

		private void EnumNodeAfterNode(BoxNode node, BoxNode targetNode, List<BoxNode> nl, ref bool flg)
		{
			if (flg) {
				nl.Add(node);
			} else if (node == targetNode) {
				flg = true;
			}
			foreach (BoxNode n in node.Children) {
				EnumNodeAfterNode(n, targetNode, nl, ref flg);
			}
		}
		#endregion

		/// <summary>
		/// 指定されたBoxを再帰的にTreeViewに表示します。
		/// </summary>
		/// <param name="box">ルートBox、もしくは子Box</param>
		/// <param name="tnodes">格納先のTreeNodeCollection</param>
		private void RenderAtomTree(BoxNode box, TreeNodeCollection tnodes)
		{
			TreeNode tn = new TreeNode();
			tn.Text = string.Format("{0}", box.BoxName, box.Length);
			tn.Tag = box;
			tn.ToolTipText = box.ToString();
			if (box.BoxName == Res.sysRootNode) {
				tn.ImageIndex = 0;
				tn.SelectedImageIndex = 0;
			} else if (box.Children.Count > 0) {
				tn.ImageIndex = 1;
				tn.SelectedImageIndex = 1;
			} else {
				tn.ImageIndex = 3;
				tn.SelectedImageIndex = 3;
			}
			box.TreeNode = tn;
			tnodes.Add(tn);
			foreach (BoxNode child in box.Children) {
				RenderAtomTree(child, tn.Nodes);
			}
		}

		/// <summary>
		/// ステータスバーの文字列を更新し、ログに追記します。
		/// </summary>
		/// <param name="status">表示および追記する文字列</param>
		private void UpdateStatus(string status)
		{
			UpdateStatus(status, true);
		}

		/// <summary>
		/// ステータスバーの文字列を更新します。
		/// </summary>
		/// <param name="status">表示および追記する文字列</param>
		/// <param name="logged">ログに追記するなら真、しないなら偽</param>
		private void UpdateStatus(string status, bool logged)
		{
			lblStatus.Text = status;
			Application.DoEvents();
#if LOGGING
			if (swLog != null && logged) {
				swLog.WriteLine(status);
				swLog.Flush();
			}
#endif
		}

		private bool SaveOpenedBox(int index)
		{
			IBoxEditor be = openedBoxEditors[index];
			BoxNode box = be.Box;
			if (box.BoxName == Res.sysRootNode || !be.Modified) {
				return false;
			}
			be.SaveBox();
			int diff = (int)(be.BoxDataLength - box.Length);
			if (diff != 0) {
				BoxNode bn = boxCurrent;
				do {
					bn.Length = (uint)(bn.Length + diff);
					if (bn.Parent != null) {
						bn = bn.Parent;
					} else {
						break;
					}
				} while (true);
				List<BoxNode> afterList = new List<BoxNode>();
				EnumNodeAfterNode(boxRoot, box, afterList);
				foreach (BoxNode after in afterList) {
					after.Position += diff;
				}
			}
			return true;
		}

		private void SaveFile(string file)
		{
			if (boxRoot == null) {
				return;
			}
			this.Enabled = false;
			UpdateStatus(Res.msgSavingFile + file);
			do {
				UpdateStatus(Res.msgWritingOffset);
				prgStatus.Value = 0;
				CorrectOffset();
				UpdateStatus(Res.msgWritingFile);
				prgStatus.Value = 0;
				try {
					fsSave = File.Open(file, FileMode.Create, FileAccess.Write);
				} catch {
					UpdateStatus(Res.msgOpenWriteFileFailed);
					MessageBox.Show(Res.msgOpenWriteFileFailed, Res.opeSaveFile);
					this.Enabled = true;
					break;
				}
				fsSave.SetLength(boxRoot.Length);
				SaveBox(boxRoot);
				fsSave.Close();

				UpdateStatus(Res.msgSaveFileCompleted);
			} while (false);
			this.Enabled = true;
		}

		private void CorrectOffset()
		{
			FileStream fs = null;
			BoxNode lastbox = null;
			byte[] tmp = new byte[4];
			int cnt = 0;
			foreach (BoxLongPair blp in offsetBackup) {
				if (blp.Node1 != lastbox) {
					if (fs != null) {
						fs.Close();
					}
					fs = File.OpenWrite(blp.Node1.DumpFile);
					lastbox = blp.Node1;
				}
				fs.Seek(blp.Number1, SeekOrigin.Begin);
				uint value = (uint)(blp.Node2.Position + blp.Number2);
				tmp = BitConverter.GetBytes(value);
				Array.Reverse(tmp);
				fs.Write(tmp, 0, 4);
				cnt += 100;
				prgStatus.Value = cnt / offsetBackup.Count;
			}
			fs.Close();
		}

		byte[] buf;
		private void SaveBox(BoxNode box)
		{
			if (box.BoxName != Res.sysRootNode) {
				buf = BitConverter.GetBytes(box.Length);
				Array.Reverse(buf);
				fsSave.Write(buf, 0, 4);
				fsSave.Write(Encoding.ASCII.GetBytes(box.BoxName), 0, 4);
				if (box.Children.Count == 0 && box.Length > 8) {
					FileStream fsRead = File.OpenRead(box.DumpFile);
					fsRead.Seek(8, SeekOrigin.Begin);
					while (fsRead.Position < fsRead.Length) {
						int len = fsRead.Read(bufMain, 0, bufMain.Length);
						fsSave.Write(bufMain, 0, len);
						prgStatus.Value = (int)(fsSave.Position * 100 / boxRoot.Length);
					}
					fsRead.Close();
				}
			}
			foreach (BoxNode cbox in box.Children) {
				SaveBox(cbox);
			}
		}

		private void ShowDetail(BoxNode node, byte[] buf)
		{
			lviewDetail.Items.Clear();
			AddDetail("Box Name", node.BoxName.ToString(), "ボックスの名前");
			AddDetail("Box Length", node.Length.ToString(), "ボックスの長さ");

			try {
				foreach (IBoxDetailReader dec in boxDetailReaders) {
					if (dec.IsDecodable(node)) {
						BoxDetail[] details = dec.Decode(node, buf);
						foreach (BoxDetail det in details) {
							AddDetail(det.Name, det.Data, det.Description);
						}
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(Res.msgParseNodeFailed + ex.ToString());
			}
		}

		private void AddDetail(string name, string value, string desc)
		{
			ListViewItem item = new ListViewItem(name);
			item.SubItems.AddRange(new string[] { value, desc });
			lviewDetail.Items.Add(item);

		}

		private void OpenBox<T>(BoxNode box) where T : Control, IBoxEditor, new()
		{
			OpenBox(box, new T());
		}

		private void OpenBox(BoxNode box, IBoxEditor be)
		{
			for (int i = 0; i < tabcMain.TabCount; i++) {
				if (openedBoxEditors[i].Box == box) {
					tabcMain.SelectTab(i);
					return;
				}
			}
			UpdateStatus(Res.msgParsingNode);
			if (!be.CanOpen(box)) {
				return;
			}
			SuspendLayout();
			try {
				byte[] buf = File.ReadAllBytes(box.DumpFile);
				(be as Control).Visible = false;
				(be as Control).Dock = DockStyle.Fill;
				(be as Control).TextChanged += new EventHandler(be_TextChanged);
				TabPage tpage = new TabPage(box.BoxName);
				tpage.Padding = new Padding(0, 0, 0, 0);
				tpage.Margin = new Padding(0, 0, 0, 0);
				tpage.ImageIndex = box.TreeNode.SelectedImageIndex;
				tpage.Controls.Add(be as Control);
				tabcMain.TabPages.Add(tpage);
				openedBoxes.Add(box);
				openedBoxEditors.Add(be);
				be.OpenBox(box);
				(be as Control).Visible = true;

				boxCurrent = box;
				tabcMain.SelectTab(tpage);
				if (tabcMain.SelectedIndex == 0) {
					SelectOpenedBox();
				}
			} catch (Exception ex) {
				MessageBox.Show(Res.msgOpenNodeFailed + ex.ToString(), Res.opeOpenNode);
			}
			ResumeLayout();
		}

		void be_TextChanged(object sender, EventArgs e)
		{
			int index = openedBoxEditors.IndexOf(sender as IBoxEditor);
			string text = (openedBoxEditors[index] as Control).Text;
			tabcMain.TabPages[index].Text = text;
			if (text.IndexOf('*') > -1) {
				tabcMain.TabPages[index].ImageIndex = 4;
			} else {
				tabcMain.TabPages[index].ImageIndex = 3;
			}
		}

		private void CloseBox(int index)
		{
			if (tabcMain.TabCount < 1) {
				return;
			}
			if (openedBoxEditors[index].Modified) {
				DialogResult dr = MessageBox.Show(Res.qesSaveNode, Res.opeEditNode, MessageBoxButtons.YesNoCancel);
				switch (dr) {
				case DialogResult.Yes:
					SaveOpenedBox(index);
					break;
				case DialogResult.Cancel:
					return;
				}
			}
			tabcMain.SelectedIndex = -1;
			tabcMain.SuspendLayout();
			tabcMain.TabPages.RemoveAt(index);
			openedBoxes.RemoveAt(index);
			openedBoxEditors.RemoveAt(index);
			if (tabcMain.TabCount > 0) {
				tabcMain.SelectedIndex = Math.Min(index, tabcMain.TabCount - 1);
				tabcMain.ResumeLayout();
			}
			if (tabcMain.SelectedIndex == 0) {
				SelectOpenedBox();
			}
		}

		private void SaveAllOpenedBox()
		{
			for (int i = 0; i < tabcMain.TabCount; i++) {
				IBoxEditor be = openedBoxEditors[i];
				if (be.Modified && MessageBox.Show(string.Format(Res.qesSaveAllNode, be.Box.BoxName), Res.opeSaveFile, MessageBoxButtons.YesNo) == DialogResult.Yes) {
					SaveOpenedBox(i);
				}
			}
		}

		private void SelectOpenedBox()
		{
			if (tabcMain.SelectedIndex >= 0) {
				IBoxEditor be = openedBoxEditors[tabcMain.SelectedIndex];
				boxCurrent = be.Box;
				//propertyGrid1.SelectedObject = be.Box;
				ShowDetail(be.Box, be.BoxData);
				UpdateStatus(Res.msgOpenNodeCompleted + be.Box.BoxName);
			} else {
				boxCurrent = null;
			}
		}

		private void AjustParentOrAfterBoxes(int diff, BoxNode box, bool fromParent)
		{
			if (diff != 0) {
				BoxNode bn = fromParent ? box.Parent : box;
				do {
					bn.Length = checked((uint)(bn.Length + diff));
					if (bn.Parent != null) {
						bn = bn.Parent;
					} else {
						break;
					}
				} while (true);
				List<BoxNode> afterList = new List<BoxNode>();
				EnumNodeAfterNode(boxRoot, box, afterList);
				foreach (BoxNode after in afterList) {
					after.Position = checked(after.Position + diff);
				}
			}
		}

		bool GetTreeNodeByBox(BoxNode box, ref TreeNode tn)
		{
			bool found = false;
			GetTreeNodeByBox(box, ref tn, tviewBox.Nodes[0], found);
			return found;
		}

		bool GetTreeNodeByBox(BoxNode box, ref TreeNode tn, TreeNode ftn, bool found)
		{
			foreach (TreeNode ctn in ftn.Nodes) {
				if (ctn.Tag == box) {
					tn = ctn;
					found = true;
					return found;
				}
				foreach (TreeNode cctn in ctn.Nodes) {
					if (GetTreeNodeByBox(box, ref tn, cctn, found)) {
						return found;
					}
				}
			}
			return false;
		}
		#endregion
		#region イベントハンドラ
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Enabled = false;
			try {
				Properties.Settings.Default.Save();
			} catch {
			}
			try {
				UpdateStatus(Res.msgRemovingTmpDir);
				RemoveTempFiles();
				Directory.Delete(tmpdir, true);
			} catch (Exception ex) {
				MessageBox.Show(string.Format(Res.msgRemoveTmpDirFailed, tmpdir, ex.ToString()));
			}
#if LOGGING
			if (swLog != null) {
				swLog.Close();
			}
#endif
		}

		#region メニュー関連
		private void mmsMain_MenuActivate(object sender, EventArgs e)
		{
			bool bFileOpened = boxRoot != null;
			mmiSaveFile.Enabled = bFileOpened;
			mmiSaveFileAs.Enabled = bFileOpened;
			bool bTabAvabable = tabcMain.TabCount > 0;
			foreach (ToolStripItem tsi in mmrTab.DropDownItems) {
				tsi.Enabled = bTabAvabable;
			}
			bool bBineditorSelected = bTabAvabable && boxCurrent.BoxName != Res.sysRootNode &&
				openedBoxEditors[tabcMain.SelectedIndex].EditorName == "バイナリエディタ";
			mmiExportBoxBinary.Enabled = bBineditorSelected;
			mmiImportBoxBinary.Enabled = bBineditorSelected;
		}

		private void mmsMain_LayoutCompleted(object sender, EventArgs e)
		{
			//TODO:設定で切り替え可能にする
			//foreach (ToolStripMenuItem tsi in mmsMain.Items) {
			//    AddPaintHanderToTSMI(tsi.DropDownItems, tsi.DropDown);
			//}
		}

		private void AddPaintHanderToTSMI(ToolStripItemCollection tsmic, ToolStrip ptsmi)
		{
			ptsmi.ShowItemToolTips = false;
			foreach (ToolStripItem tsi in tsmic) {
				ToolStripMenuItem tsmi = tsi as ToolStripMenuItem;
				if (tsmi == null) {
					continue;
				}
				tsmi.Paint += new PaintEventHandler(tsmi_Paint);
				AddPaintHanderToTSMI(tsmi.DropDownItems, tsmi.DropDown);
			}
		}

		void tsmi_Paint(object sender, PaintEventArgs e)
		{
			ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
			if (tsmi == null || !tsmi.Selected) {
				return;
			}
			UpdateStatus(tsmi.ToolTipText, false);
		}
		#region ファイルメニュー
		private void mmiOpenFile_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			if (ofdMain.ShowDialog() == DialogResult.OK) {
				OpenFile(ofdMain.FileName);
			}
			this.Enabled = true;
		}

		private void mmiSaveFile_Click(object sender, EventArgs e)
		{
			SaveAllOpenedBox();
			if (MessageBox.Show(string.Format(Res.qesSaveFile, sourceFile), Res.opeSaveFile, MessageBoxButtons.YesNo) == DialogResult.Yes) {
				SaveFile(sourceFile);
			}
		}

		private void mmiSaveFileAs_Click(object sender, EventArgs e)
		{
			SaveAllOpenedBox();
			if (sfdMain.ShowDialog() == DialogResult.OK) {
				sourceFile = sfdMain.FileName;
				SaveFile(sourceFile);
			}
		}

		private void mmiExit_Click(object sender, EventArgs e)
		{
			Close();
		}
		#endregion
		#region タブメニュー
		private void mmiSaveTab_Click(object sender, EventArgs e)
		{
			SaveOpenedBox(tabcMain.SelectedIndex);
		}

		private void mmiCloseTab_Click(object sender, EventArgs e)
		{
			CloseBox(tabcMain.SelectedIndex);
		}

		private void mmiSaveAllTabs_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < tabcMain.TabCount; i++) {
				SaveOpenedBox(i);
			}
		}

		private void mmiCloseAllTabs_Click(object sender, EventArgs e)
		{
			SaveAllOpenedBox();
			tabcMain.TabPages.Clear();
		}

		private void mmiExportBoxBinary_Click(object sender, EventArgs e)
		{
			int index = tabcMain.SelectedIndex;
			if (index >= 0) {
				if (openedBoxEditors[index].Modified && !SaveOpenedBox(index)) {
					MessageBox.Show(Res.msgCanotExportWithoutSave, Res.opeExport);
					return;
				}
				BoxNode box = openedBoxes[index];
				sfdBox.FileName = string.Format("{0}_{1}({2:X8}+{3:X8}).dat",
					Path.GetFileNameWithoutExtension(sourceFile), box.BoxName, box.Position, box.Length);
				if (sfdBox.ShowDialog() == DialogResult.OK) {
					fsSave = File.OpenWrite(sfdBox.FileName);
					SaveBox(openedBoxes[index]);
					fsSave.Close();
				}
			}
		}

		private void mmiImportBoxBinary_Click(object sender, EventArgs e)
		{
			if (boxCurrent.BoxName == Res.sysRootNode) {
				return;
			}
			if (MessageBox.Show(Res.msgInportWarning, Res.opeImport, MessageBoxButtons.YesNo) == DialogResult.No) {
				return;
			}
			if (ofdBox.ShowDialog() == DialogResult.OK) {
				string file = ofdBox.FileName;

				int idxtab = tabcMain.SelectedIndex;
				IBoxEditor be = openedBoxEditors[idxtab];
				BoxNode box = openedBoxes[idxtab];
				uint lenbackup = (uint)be.BoxDataLength;

				TreeNode tn = box.TreeNode;
				TreeNode ptn = tn.Parent;
				int idxtn = ptn.Nodes.IndexOf(tn);
				BoxNode pbox = box.Parent;
				int idxbox = pbox.Children.IndexOf(box);

				BoxNode nbox = new BoxNode();
				nbox.BoxName = Res.sysRootNode;
				nbox.DumpFile = file;
				nbox.Parent = null;
				nbox.Position = box.Position;
				nbox.SourcePosition = box.Position;
				ParseBox(nbox);
				if (nbox.Children.Count == 0) {
					MessageBox.Show(Res.msgInvalidExportedBox, Res.opeImport);
					return;
				}
				nbox = nbox.Children[0];
				if (nbox.BoxName != box.BoxName) {
					MessageBox.Show(Res.msgDifferentNameExportedBox, Res.opeImport);
					return;
				}
				nbox.Parent = pbox;
				pbox.Children[idxbox] = nbox;
				openedBoxes[idxtab] = nbox;

				tviewBox.Nodes.Add(Res.sysRootNode);
				TreeNode ntn = tviewBox.Nodes[1];
				RenderAtomTree(nbox, ntn.Nodes);
				ntn = ntn.Nodes[0];
				tviewBox.Nodes.RemoveAt(1);
				ptn.Nodes.RemoveAt(idxtn);
				ptn.Nodes.Insert(idxtn, ntn);
				be.OpenBox(nbox);

				int diff = (int)(be.BoxDataLength - lenbackup);
				AjustParentOrAfterBoxes(diff, nbox, true);
			}
		}
		#endregion
		#region ヘルプメニュー
		private void mmiOpenReadme_Click(object sender, EventArgs e)
		{
			try {
				Process.Start(Path.Combine(Application.StartupPath, @"Documents\Readme.txt"));
			} catch {
				MessageBox.Show(Res.msgReadmeNotFount);
			}
		}

		private void mmiOpenInstallDir_Click(object sender, EventArgs e)
		{
			Process.Start(Application.StartupPath);
		}

		private void mmiOpenPluginDir_Click(object sender, EventArgs e)
		{
			Process.Start(PLUGIN_DIR);
		}

		private void mmiAbout_Click(object sender, EventArgs e)
		{
			new AboutBox().ShowDialog();
		}
		#endregion
		#region タブコンテキストメニュー
		private void cmsTab_Opening(object sender, CancelEventArgs e)
		{
			BoxNode box = openedBoxes[tabcMain.SelectedIndex];
			cmiSaveTab.Enabled = box.BoxName != Res.sysRootNode;
			cmiSaveTab.Text = string.Format("{0}の保存(&S)", box.BoxName);
		}

		private void cmiSaveTab_Click(object sender, EventArgs e)
		{
			SaveOpenedBox(tabcMain.SelectedIndex);
		}

		private void cmiCloseTab_Click(object sender, EventArgs e)
		{
			CloseBox(tabcMain.SelectedIndex);
		}
		#endregion
		#region TreeViewコンテキストメニュー
		private void cmiOpenBox_Click(object sender, EventArgs e)
		{
			OpenBox<BinaryBoxEditor>(tviewBox.SelectedNode.Tag as BoxNode);
		}

		private void cmsTree_Opening(object sender, CancelEventArgs e)
		{
			BoxNode box = tviewBox.SelectedNode.Tag as BoxNode;
			cmiOpenBoxBy.DropDownItems.Clear();
			foreach (Type t in boxEditorTypes) {
				IBoxEditor be = Activator.CreateInstance(t) as IBoxEditor;
				if (be.CanOpen(box)) {
					ToolStripMenuItem tsmi = new ToolStripMenuItem(be.EditorName);
					tsmi.Tag = be;
					tsmi.Click += new EventHandler(tsmi_Click);
					cmiOpenBoxBy.DropDownItems.Add(tsmi);
				}
			}
		}
		void tsmi_Click(object sender, EventArgs e)
		{
			BoxNode box = tviewBox.SelectedNode.Tag as BoxNode;
			OpenBox(box, (sender as ToolStripMenuItem).Tag as IBoxEditor);
		}
		#endregion
		#region ListViewコンテキストメニュー
		private void cmiCopyDetail_Click(object sender, EventArgs e)
		{
			//StringBuilder sbtxt = new StringBuilder();
			//foreach (ListViewItem item in lviewDetail.SelectedItems) {
			//    if (item.SubItems.Count < 3) {
			//        continue;
			//    }
			//    sbtxt.AppendLine(string.Format("{0}\t{1}\t{2}", item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text));
			//}
			//DataObject dobj = new DataObject();
			//dobj.SetData(DataFormats.Text, sbtxt.ToString());
			//Clipboard.SetDataObject(dobj, true);
		}

		private void cmiSelectAllDetails_Click(object sender, EventArgs e)
		{
			//foreach (ListViewItem item in lviewDetail.Items) {
			//    item.Selected = true;
			//}
		}
		#endregion
		#endregion
		#region TabControl・BinaryEditor関連
		private void tabcMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (movingTabIndex >= 0) {
				return;
			}
			SelectOpenedBox();
		}

		private void tabcMain_MouseClick(object sender, MouseEventArgs e)
		{
			if (movingTabIndex >= 0) {
				return;
			}
			for (int i = 0; i < tabcMain.TabCount; i++) {
				//タブがクリックされた場合
				if (tabcMain.GetTabRect(i).Contains(e.X, e.Y)) {
					tabcMain.SelectTab(i);
					if (e.Button == MouseButtons.Middle) {
						CloseBox(i);
					} else if (e.Button == MouseButtons.Right) {
						cmsTab.Show(tabcMain, e.Location);
					}
				}
			}
		}

		int movingTabIndex = -1;
		private void tabcMain_MouseDown(object sender, MouseEventArgs e)
		{
			for (int i = 0; i < tabcMain.TabCount; i++) {
				if (!tabcMain.GetTabRect(i).Contains(e.X, e.Y)) {
					continue;
				}
				if (e.Button == MouseButtons.Left) {
					movingTabIndex = i;
				}
			}
		}

		private void tabcMain_MouseMove(object sender, MouseEventArgs e)
		{
			if (tabcMain.TabCount > openedBoxes.Count) {
				return;
			}
			if (movingTabIndex < 0) {
				return;
			}
			if (e.Button == MouseButtons.Left) {
				tabcMain.TabPages.Add("末尾に移動");
				UpdateStatus("他のタブの上でマウスボタンを離すとそのタブの前に移動できます。", false);
			}
		}

		private void tabcMain_MouseUp(object sender, MouseEventArgs e)
		{
			if (movingTabIndex < 0) {
				return;
			}
			for (int i = 0; i < tabcMain.TabCount; i++) {
				if (!tabcMain.GetTabRect(i).Contains(e.X, e.Y)) {
					continue;
				}
				if (movingTabIndex == i) {
					break;
				}
				if (e.Button == MouseButtons.Left) {
					tabcMain.SelectedIndex = -1;
					TabPage tp = tabcMain.TabPages[movingTabIndex];
					tabcMain.TabPages.Remove(tp);
					if (movingTabIndex < i) {
						i--;
					}
					if (i < tabcMain.TabCount) {
						tabcMain.TabPages.Insert(i, tp);
					} else {
						tabcMain.TabPages.Add(tp);
					}
					tabcMain.SelectedIndex = i;
					SelectOpenedBox();
					break;
				}
			}
			if (tabcMain.TabCount > openedBoxes.Count) {
				tabcMain.TabPages.RemoveAt(tabcMain.TabCount - 1);
			}
			movingTabIndex = -1;
		}
		#endregion
		#region TreeView関連
		private void tviewBox_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				OpenBox<BinaryBoxEditor>(tviewBox.SelectedNode.Tag as BoxNode);
			}
		}

		private void tviewBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (tviewBox.SelectedNode != null) {
				if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) {
					OpenBox<BinaryBoxEditor>(tviewBox.SelectedNode.Tag as BoxNode);
					e.IsInputKey = true;
				}
			}
		}

		private void tviewBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) {
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		TreeNode lasttn = null;
		private void tviewBox_MouseMove(object sender, MouseEventArgs e)
		{
			TreeNode tn = tviewBox.GetNodeAt(e.Location);
			if (tn != null && tn != lasttn) {
				tn.ToolTipText = (tn.Tag as BoxNode).ToString();
				lasttn = tn;
			}
		}

		private void tviewBox_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			//選択＋コンテキストメニュー表示
			if (e.Button == MouseButtons.Right) {
				tviewBox.SelectedNode = e.Node;
				cmsTree.Show(tviewBox, e.Location);
			}
		}
		#region TreeViewダブルクリック相殺用
		private void tviewBox_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (tviewBoxDoubleClicked) {
				e.Cancel = true;
				tviewBoxDoubleClicked = false;
			}
		}

		private void tviewBox_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			if (tviewBoxDoubleClicked) {
				e.Cancel = true;
				tviewBoxDoubleClicked = false;
			}
		}

		private void tviewBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && e.Clicks == 2) {
				tviewBoxDoubleClicked = true;
			} else {
				tviewBoxDoubleClicked = false;
			}
		}
		#endregion
		#endregion

		private void cmiNewBefore_Click(object sender, EventArgs e)
		{
			//BoxNode box = tviewBox.SelectedNode.Tag as BoxNode;
			//if (box.Name == Res.sysRootNode) {
			//    return;
			//}
			//NewBoxForm nbf = new NewBoxForm();
			//if (nbf.ShowDialog() == DialogResult.OK) {
			//    BoxNode nbox = new BoxNode();
			//    string fnc = Path.Combine(tmpdir, cntTempFile.ToString("D8"));
			//    using (FileStream fsc = File.OpenWrite(fnc)) {
			//        nbox.Name = nbf.BoxName;
			//        nbox.Length = nbf.Length;
			//        nbox.Position = box.Position;
			//        byte[] tmp = BitConverter.GetBytes(nbox.Name);
			//        Array.Reverse(tmp);
			//        fsc.Write(tmp, 0, 4);
			//        fsc.Write(Encoding.ASCII.GetBytes(nbox.Name), 0, 4);
			//        fsc.SetLength(nbox.Length);
			//        fsc.Close();
			//    }
			//    box.Parent.Children.Insert(box.Parent.Children.IndexOf(box), nbox);
			//    uint diff = nbox.Length;
			//    if (diff != 0) {
			//        BoxNode bn = nbox;
			//        do {
			//            bn.Length += diff;
			//            if (bn.Parent != null) {
			//                bn = bn.Parent;
			//            } else {
			//                break;
			//            }
			//        } while (true);
			//        boxCurrent.Length = (uint)be.BoxDataLength;
			//        List<BoxNode> afterList = new List<BoxNode>();
			//        EnumNodeAfterNode(boxRoot, nbox, afterList);
			//        foreach (BoxNode after in afterList) {
			//            after.Position += diff;
			//        }
			//    }
			//}
		}
		#endregion

		private void tviewBox_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			if ((e.Node.Tag as BoxNode).BoxName == Res.sysRootNode) {
				return;
			}
			e.Node.ImageIndex = 1;
			e.Node.SelectedImageIndex = 1;
		}

		private void tviewBox_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if ((e.Node.Tag as BoxNode).BoxName == Res.sysRootNode) {
				return;
			}
			e.Node.ImageIndex = 2;
			e.Node.SelectedImageIndex = 2;
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.All;
			}
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
				if (files.Length > 1) {
					return;
				}
				string file = files[0];
				OpenFile(file);
			}
		}

		private void mmiRunNgen_Click(object sender, EventArgs e)
		{
			try {
				Process proc = Process.Start(Path.Combine(Application.StartupPath, @"Exe\NgenExecWin.exe"));
				Application.DoEvents();
				proc.WaitForExit();
			}catch(Exception ex){
				MessageBox.Show(string.Format(Res.msgNgenFailed, ex.ToString()), Res.opeNgen);
			}
		}
	}
}