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
	public partial class BoxEditorPanel : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		private IBoxEditor editor;

		public IBoxEditor Editor
		{
			get { return editor; }
		}

		private BoxEditorPanel()
		{
			InitializeComponent();
		}

		public BoxEditorPanel(IBoxEditor editor)
			: this()
		{
			if (!(editor is Control)) {
				throw new ArgumentException("boxÇÕControlÇåpè≥ÇµÇƒÇ¢ÇÈïKóvÇ™Ç†ÇËÇ‹Ç∑ÅB");
			}
			this.editor = editor;
			Control eac = editor as Control;
			eac.Dock = DockStyle.Fill;
			this.Controls.Add(eac);
		}
	}
}

