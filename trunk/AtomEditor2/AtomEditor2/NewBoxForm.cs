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
	/// Boxを新規作成するための情報を入力させます。
	/// </summary>
	public partial class NewBoxForm : Form
	{
		/// <summary>
		/// フォームに入力されたBoxの名前を取得します。
		/// </summary>
		public string BoxName
		{
			get { return txtName.Text; }
		}

		/// <summary>
		/// フォームに入力されたBoxの長さを取得します。
		/// </summary>
		public uint Length
		{
			get { return (uint)nudLength.Value; }
		}

		/// <summary>
		/// NewBoxFormを初期化します。
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