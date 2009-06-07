using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Kirishima16.Libraries.AtomEditor;
using Kirishima16.Libraries.MP4Box;

namespace Kirishima16.Applications.AtomEditor2
{
	/// <summary>
	/// バイナリエディタでBoxを確認および編集できるようにします。
	/// </summary>
	public partial class BinaryBoxEditor : UserControl, IBoxEditor
	{
		private BoxNode box;

		/// <summary>
		/// BinaryBoxEditorを初期化します。
		/// </summary>
		public BinaryBoxEditor()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンテナを指定してBinaryBoxEditorを初期化します。
		/// </summary>
		/// <param name="container"></param>
		public BinaryBoxEditor(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		#region IBoxEditor メンバ

		/// <summary>
		/// BinaryBoxEditorの表示名を取得します。
		/// </summary>
		public string EditorName
		{
			get { return "バイナリエディタ"; }
		}

		/// <summary>
		/// 指定されたBoxに対応しているかどうかを取得します。
		/// </summary>
		/// <param name="box">確認するBoxNode</param>
		/// <returns>対応していれば真、さもなければ偽</returns>
		public bool CanOpen(BoxNode box)
		{
			return true;
		}

		/// <summary>
		/// Boxが編集されているかどうかを取得します。
		/// </summary>
		public bool Modified
		{
			get { return bineditMain.Modified; }
		}

		/// <summary>
		/// 編集中のBoxをBoxNodeの派生クラスとして取得します。
		/// </summary>
		public BoxNode Box
		{
			get { return box; }
		}

		/// <summary>
		/// 編集中のBoxをバイナリデータとして取得します。
		/// </summary>
		public byte[] BoxData
		{
			get { return bineditMain.BinaryData; }
		}

		/// <summary>
		/// 編集中のBoxの長さを取得します。
		/// </summary>
		public long BoxDataLength
		{
			get { return bineditMain.BufferLength; }
		}

		/// <summary>
		/// 指定されたBoxを読み込みます。
		/// </summary>
		/// <param name="box">読み込むBox</param>
		/// <returns></returns>
		public bool OpenBox(BoxNode box)
		{
			byte[] bin = File.ReadAllBytes(box.DumpFile);
			MemoryStream ms = new MemoryStream(bin);
			bineditMain.BinaryStream = ms;
			if (box.Children.Count > 0) {
				bineditMain.ReadOnly = true;
			}
			this.box = box;
			SetText();
			return true;
		}

		/// <summary>
		/// 編集中のBoxを関連付けられたファイルに保存します。
		/// </summary>
		public void SaveBox()
		{
			bineditMain.Save(box.DumpFile);
			bineditMain.Modified = false;
		}

		#endregion

		/// <summary>
		/// 状態を表す文字列を取得します。
		/// </summary>
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

		private void bineditMain_ModifiedChanged(object sender, EventArgs e)
		{
			SetText();
		}

		private void SetText()
		{
			if (box != null) {
				this.Text = box.BoxName + (bineditMain.Modified ? "*" : "");
			} else {
				this.Text = "*";
			}
		}

		private void BinaryBoxEditor_ParentChanged(object sender, EventArgs e)
		{
			if (Parent != null) {
				if (Parent.Parent != null) {
					bineditMain.BeforeControl = Parent.Parent;
				} else {
					bineditMain.BeforeControl = Parent;
					Parent.ParentChanged += new EventHandler(Parent_ParentChanged);
				}
			}
		}

		void Parent_ParentChanged(object sender, EventArgs e)
		{
			if (Parent.Parent != null) {
				bineditMain.BeforeControl = Parent.Parent;
			}
		}
	}
}
