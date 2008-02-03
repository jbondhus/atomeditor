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
	/// �o�C�i���G�f�B�^��Box���m�F����ѕҏW�ł���悤�ɂ��܂��B
	/// </summary>
	public partial class BinaryBoxEditor : UserControl, IBoxEditor
	{
		private BoxNode box;

		/// <summary>
		/// BinaryBoxEditor�����������܂��B
		/// </summary>
		public BinaryBoxEditor()
		{
			InitializeComponent();
		}

		/// <summary>
		/// �R���e�i���w�肵��BinaryBoxEditor�����������܂��B
		/// </summary>
		/// <param name="container"></param>
		public BinaryBoxEditor(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		#region IBoxEditor �����o

		/// <summary>
		/// BinaryBoxEditor�̕\�������擾���܂��B
		/// </summary>
		public string EditorName
		{
			get { return "�o�C�i���G�f�B�^"; }
		}

		/// <summary>
		/// �w�肳�ꂽBox�ɑΉ����Ă��邩�ǂ������擾���܂��B
		/// </summary>
		/// <param name="box">�m�F����BoxNode</param>
		/// <returns>�Ή����Ă���ΐ^�A�����Ȃ���΋U</returns>
		public bool CanOpen(BoxNode box)
		{
			return true;
		}

		/// <summary>
		/// Box���ҏW����Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool Modified
		{
			get { return bineditMain.Modified; }
		}

		/// <summary>
		/// �ҏW����Box��BoxNode�̔h���N���X�Ƃ��Ď擾���܂��B
		/// </summary>
		public BoxNode Box
		{
			get { return box; }
		}

		/// <summary>
		/// �ҏW����Box���o�C�i���f�[�^�Ƃ��Ď擾���܂��B
		/// </summary>
		public byte[] BoxData
		{
			get { return bineditMain.BinaryData; }
		}

		/// <summary>
		/// �ҏW����Box�̒������擾���܂��B
		/// </summary>
		public long BoxDataLength
		{
			get { return bineditMain.BufferLength; }
		}

		/// <summary>
		/// �w�肳�ꂽBox��ǂݍ��݂܂��B
		/// </summary>
		/// <param name="box">�ǂݍ���Box</param>
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
		/// �ҏW����Box���֘A�t����ꂽ�t�@�C���ɕۑ����܂��B
		/// </summary>
		public void SaveBox()
		{
			bineditMain.Save(box.DumpFile);
			bineditMain.Modified = false;
		}

		#endregion

		/// <summary>
		/// ��Ԃ�\����������擾���܂��B
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
