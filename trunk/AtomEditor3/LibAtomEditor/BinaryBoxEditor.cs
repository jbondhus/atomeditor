using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// バイナリエディタで表示および編集できるBoxEditorです。
	/// </summary>
	public partial class BinaryBoxEditor : UserControl, IBoxEditor
	{
		BoxTreeNode box = new BoxTreeNode();

		/// <summary>
		/// BinaryBoxEditorを初期化します。
		/// </summary>
		public BinaryBoxEditor()
		{
			InitializeComponent();
			bineditMain.ModifiedChanged += new EventHandler(bineditMain_ModifiedChanged);
		}

		void bineditMain_ModifiedChanged(object sender, EventArgs e)
		{
			SetEditorText();
		}

		private void SetEditorText()
		{

			this.ParentForm.Text = box.Name + "(" + box.SubName + ")" + (bineditMain.Modified ? "*" : "");
		}

		#region IBoxEditor メンバ

		/// <summary>
		/// 指定したBoxが開けるかどうかを返します。
		/// </summary>
		/// <param name="box">確認するBox</param>
		/// <returns>開けるならTrue、さもなければFalse</returns>
		public bool CanOpen(BoxTreeNode box)
		{
			return true;
		}

		/// <summary>
		/// Boxが編集されているかどうかを返します。
		/// </summary>
		public bool Modified
		{
			get { return bineditMain.Modified; }
		}

		/// <summary>
		/// 編集中のBoxを参照します。
		/// </summary>
		/// <remarks>
		/// <para>編集のバイナリのサイズが適用されたBoxTreeNodeを新規作成して返します。</para>
		/// <para>関連付けられたBoxを取得するには<see cref="SourceBox"/>プロパティを参照してください。</para>
		/// </remarks>
		public BoxTreeNode Box
		{
			get
			{
				BoxTreeNode nbox = new BoxTreeNode(box);
				nbox.BoxLength = (uint)bineditMain.BufferLength;
				return nbox;
			}
		}

		/// <summary>
		/// 関連付けられたBoxを参照します。
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="OpenBox"/>で指定されたBoxが返されます。
		/// </para>
		/// </remarks>
		public BoxTreeNode SourceBox
		{
			get { return box; }
		}

		/// <summary>
		/// 編集中のバイナリを格納したbyte配列を取得します。
		/// </summary>
		/// <remarks>
		/// <para>バイナリエディタで編集されているバイナリをbyte配列として取得します。</para>
		/// </remarks>
		public byte[] BoxData
		{
			get { return bineditMain.BinaryData; }
		}

		/// <summary>
		/// 編集中のBoxのサイズを返します。
		/// </summary>
		public long BoxDataLength
		{
			get { return bineditMain.BufferLength; }
		}

		/// <summary>
		/// 指定したBoxを開きます。
		/// </summary>
		/// <param name="box">開くBox</param>
		/// <returns>開けたならTrue、さもなくばFalse</returns>
		public bool OpenBox(BoxTreeNode box)
		{
			byte[] bin = File.ReadAllBytes(box.DumpFile);
			MemoryStream ms = new MemoryStream(bin);
			bineditMain.BinaryStream = ms;
			this.box = box;
			SetEditorText();
			return true;
		}

		/// <summary>
		/// 編集中のBoxを保存します。
		/// </summary>
		/// <remarks>
		/// <para>
		/// 編集中のBoxを<see cref="IBoxNode.DumpFile"/>に格納されているファイルに保存し、
		/// 親Boxのサイズおよび以降のBoxの位置情報を修正します。
		/// </para>
		/// </remarks>
		public void SaveBox()
		{
			bineditMain.Save(box.DumpFile);
		}

		#endregion
	}
}
