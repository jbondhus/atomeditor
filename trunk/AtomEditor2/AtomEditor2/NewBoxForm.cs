using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Kirishima16.Applications.AtomEditor2
{
	/// <summary>
	/// Box��V�K�쐬���邽�߂̏�����͂����܂��B
	/// </summary>
	public partial class NewBoxForm : Form
	{
		/// <summary>
		/// �t�H�[���ɓ��͂��ꂽBox�̖��O���擾���܂��B
		/// </summary>
		public string BoxName
		{
			get { return txtName.Text; }
		}

		/// <summary>
		/// �t�H�[���ɓ��͂��ꂽBox�̒������擾���܂��B
		/// </summary>
		public uint Length
		{
			get { return (uint)nudLength.Value; }
		}

		/// <summary>
		/// NewBoxForm�����������܂��B
		/// </summary>
		public NewBoxForm()
		{
			InitializeComponent();
		}

		private void txtName_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			btnAccept.Enabled = true;
		}
	}
}