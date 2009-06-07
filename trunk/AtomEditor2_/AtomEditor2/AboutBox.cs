using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Kirishima16.Applications.AtomEditor2
{
	partial class AboutBox : Form
	{
		public AboutBox()
		{
			InitializeComponent();
			SuspendLayout();
			Assembly exeasm = Assembly.GetExecutingAssembly();
			AssemblyName[] refasmnames = exeasm.GetReferencedAssemblies();
			this.Text = string.Format("{0} のバージョン情報", GetAssemblyProduct(exeasm));
			lblExeAsm.Text = string.Format("{0} {1}\r\n{2} {3}\r\n{4}",
				GetAssemblyCompany(exeasm),
				GetAssemblyProduct(exeasm),
				GetAssemblyTitle(exeasm),
				GetAssemblyVersion(exeasm),
				GetAssemblyCopyright(exeasm));
			foreach (AssemblyName asmname in refasmnames) {
				Assembly asm = Assembly.Load(asmname.FullName);
				if (asm.Location.StartsWith(Application.StartupPath)) {
					AddAssemblyToList(asm, 1);
				} else {
					AddAssemblyToList(asm, 0);
				}
			}
			foreach (Assembly asm in Program.PluginedAssemblies) {
				AddAssemblyToList(asm, 2);
			}
			lvRefAsms.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			ResumeLayout();
		}

		private void AddAssemblyToList(Assembly asm, int groupIndex)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text = GetAssemblyTitle(asm);
			lvi.SubItems.Add(GetAssemblyVersion(asm));
			lvi.SubItems.Add(GetAssemblyCompany(asm));
			lvi.SubItems.Add(GetAssemblyCopyright(asm));
			lvi.SubItems.Add(asm.Location);
			lvi.Group = lvRefAsms.Groups[groupIndex];
			lvRefAsms.Items.Add(lvi);
		}

		#region アセンブリ属性アクセサ

		public string GetAssemblyTitle(Assembly asm)
		{
			object[] attributes = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
			if (attributes.Length > 0) {
				AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
				if (titleAttribute.Title != "") {
					return titleAttribute.Title;
				}
			}
			return System.IO.Path.GetFileNameWithoutExtension(asm.CodeBase);
		}

		public string GetAssemblyVersion(Assembly asm)
		{
			return asm.GetName().Version.ToString();
		}

		public string GetAssemblyDescription(Assembly asm)
		{
			object[] attributes = asm.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
			if (attributes.Length == 0) {
				return "";
			}
			return ((AssemblyDescriptionAttribute)attributes[0]).Description;
		}

		public string GetAssemblyProduct(Assembly asm)
		{
			object[] attributes = asm.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
			if (attributes.Length == 0) {
				return "";
			}
			return ((AssemblyProductAttribute)attributes[0]).Product;
		}

		public string GetAssemblyCopyright(Assembly asm)
		{
			object[] attributes = asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			if (attributes.Length == 0) {
				return "";
			}
			return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
		}

		public string GetAssemblyCompany(Assembly asm)
		{
			object[] attributes = asm.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
			if (attributes.Length == 0) {
				return "";
			}
			return ((AssemblyCompanyAttribute)attributes[0]).Company;
		}
		#endregion

		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(this.Text + "\r\n");
			sb.AppendLine(lblExeAsm.Text);
			foreach (ListViewGroup lvg in lvRefAsms.Groups) {
				sb.AppendLine("\r\n" + lvg.Header + "\r\n--------");
				foreach (ListViewItem lvi in lvg.Items) {
					foreach (ListViewItem.ListViewSubItem lvsi in lvi.SubItems) {
						sb.Append(lvsi.Text + "\t");
					}
					sb.AppendLine();
				}
			}
			string txt = sb.ToString();
			try {
				Clipboard.SetText(txt);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void AboutBox_Load(object sender, EventArgs e)
		{
			SetBackgroundImage();
		}

		private void AboutBox_SizeChanged(object sender, EventArgs e)
		{
			SetBackgroundImage();
		}

		private void SetBackgroundImage()
		{
			Bitmap bmp = new Bitmap(ClientSize.Width, 100);
			Graphics g = Graphics.FromImage(bmp);
			g.DrawImage(Properties.Resources.AboutBG, new Rectangle(0, 0, ClientSize.Width, 100), new Rectangle(0, 0, 1, 100), GraphicsUnit.Pixel);
			g.Dispose();
			this.BackgroundImage = bmp;
		}
	}
}
