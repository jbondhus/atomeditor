using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Kirishima16.Applications.AtomEditor.V3
{
	public partial class ExplorerPanel : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public TreeView TreeView
		{
			get { return tvExplorer; }
		}

		public ExplorerPanel()
		{
			InitializeComponent();
		}
	}
}

