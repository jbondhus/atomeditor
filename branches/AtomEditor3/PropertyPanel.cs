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
	public partial class PropertyPanel : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public PropertyPanel()
		{
			InitializeComponent();
		}

		public void UpdateBox(BoxTreeNode box)
		{
			pgProperty.SelectedObject = box;
		}
	}
}

