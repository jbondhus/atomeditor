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
	/// �o�C�i���G�f�B�^�ŕ\������ѕҏW�ł���BoxEditor�ł��B
	/// </summary>
	public partial class BinaryBoxEditor : UserControl, IBoxEditor
	{
		BoxTreeNode box = new BoxTreeNode();

		/// <summary>
		/// BinaryBoxEditor�����������܂��B
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

		#region IBoxEditor �����o

		/// <summary>
		/// �w�肵��Box���J���邩�ǂ�����Ԃ��܂��B
		/// </summary>
		/// <param name="box">�m�F����Box</param>
		/// <returns>�J����Ȃ�True�A�����Ȃ����False</returns>
		public bool CanOpen(BoxTreeNode box)
		{
			return true;
		}

		/// <summary>
		/// Box���ҏW����Ă��邩�ǂ�����Ԃ��܂��B
		/// </summary>
		public bool Modified
		{
			get { return bineditMain.Modified; }
		}

		/// <summary>
		/// �ҏW����Box���Q�Ƃ��܂��B
		/// </summary>
		/// <remarks>
		/// <para>�ҏW�̃o�C�i���̃T�C�Y���K�p���ꂽBoxTreeNode��V�K�쐬���ĕԂ��܂��B</para>
		/// <para>�֘A�t����ꂽBox���擾����ɂ�<see cref="SourceBox"/>�v���p�e�B���Q�Ƃ��Ă��������B</para>
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
		/// �֘A�t����ꂽBox���Q�Ƃ��܂��B
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="OpenBox"/>�Ŏw�肳�ꂽBox���Ԃ���܂��B
		/// </para>
		/// </remarks>
		public BoxTreeNode SourceBox
		{
			get { return box; }
		}

		/// <summary>
		/// �ҏW���̃o�C�i�����i�[����byte�z����擾���܂��B
		/// </summary>
		/// <remarks>
		/// <para>�o�C�i���G�f�B�^�ŕҏW����Ă���o�C�i����byte�z��Ƃ��Ď擾���܂��B</para>
		/// </remarks>
		public byte[] BoxData
		{
			get { return bineditMain.BinaryData; }
		}

		/// <summary>
		/// �ҏW����Box�̃T�C�Y��Ԃ��܂��B
		/// </summary>
		public long BoxDataLength
		{
			get { return bineditMain.BufferLength; }
		}

		/// <summary>
		/// �w�肵��Box���J���܂��B
		/// </summary>
		/// <param name="box">�J��Box</param>
		/// <returns>�J�����Ȃ�True�A�����Ȃ���False</returns>
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
		/// �ҏW����Box��ۑ����܂��B
		/// </summary>
		/// <remarks>
		/// <para>
		/// �ҏW����Box��<see cref="IBoxNode.DumpFile"/>�Ɋi�[����Ă���t�@�C���ɕۑ����A
		/// �eBox�̃T�C�Y����шȍ~��Box�̈ʒu�����C�����܂��B
		/// </para>
		/// </remarks>
		public void SaveBox()
		{
			bineditMain.Save(box.DumpFile);
		}

		#endregion
	}
}
