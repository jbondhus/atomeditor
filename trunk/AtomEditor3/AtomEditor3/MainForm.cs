using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kirishima16.Libraries.AtomEditor;

namespace Kirishima16.Applications.AtomEditor.V3
{
	public partial class MainForm : Form
	{
		ExplorerPanel pnlExplorer;
		PropertyPanel pnlProperty;

		public MainForm()
		{
			InitializeComponent();
			pnlExplorer = new ExplorerPanel();
			pnlExplorer.Show(dpMain);
			pnlProperty = new PropertyPanel();
			pnlProperty.Show(dpMain);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			BinaryBoxEditor bbe = new BinaryBoxEditor();
			BoxEditorPanel bep = new BoxEditorPanel(bbe);
			bep.Show(dpMain);
		}
	}
}