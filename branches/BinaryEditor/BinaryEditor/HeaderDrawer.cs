using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Kirishima16.Forms
{
	internal class HeaderDrawer : DrawerControl
	{
		public override string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = value;
				RenderSurface();
				Refresh();
			}
		}

		public HeaderDrawer()
		{
			RenderSurface();
		}

		public override void RenderSurface()
		{
			if (graphics == null) {
				return;
			}
			graphics.FillRectangle(backBrush, 0, 0, Width, Height);
			TextRenderer.DrawText(graphics, Text, Font, new Point(0, 0), ForeColor);
		}
	}
}
