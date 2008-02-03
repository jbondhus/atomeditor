using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Kirishima16.Forms
{
	internal class LineNoDrawer : DrawerControl
	{
		/// <summary>
		/// 開始アドレス
		/// </summary>
		private long startAddress;

		/// <summary>
		/// 開始アドレスを取得または設定します。
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public long StartAddress
		{
			get { return startAddress; }
			set
			{
				if (value < 0 || maxAddress < value) {
					throw new ArgumentOutOfRangeException("StartAddressには0以上MaxAddress以下の値のみ指定できます。");
				}
				startAddress = value;
			}
		}

		/// <summary>
		/// 最大アドレス
		/// </summary>
		private long maxAddress = 0x40;

		/// <summary>
		/// 最大アドレスを取得または設定します。
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public long MaxAddress
		{
			get { return maxAddress; }
			set
			{
				if (value < startAddress) {
					throw new ArgumentOutOfRangeException("MaxAddressにはStartAddress以上の値のみ指定できます。");
				}
				maxAddress = value;
			}
		}

		/// <summary>
		/// 行数
		/// </summary>
		private int lineCount = 5;

		/// <summary>
		/// 行数を取得または設定します。
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public int LineCount
		{
			get { return lineCount; }
			set
			{
				if (value < 0) {
					throw new ArgumentOutOfRangeException("LineCountには0以上の値のみ指定できます。");
				}
				lineCount = value;
			}
		}

		StringBuilder sbln;

		public LineNoDrawer()
		{
			RenderSurface();
		}

		public override void RenderSurface()
		{
			if (graphics == null) {
				return;
			}
			graphics.FillRectangle(backBrush, 0, 0, Width, Height);
			sbln = new StringBuilder(lineCount * 9);
			for (long i = startAddress; i < startAddress + lineCount * 0x10 && i <= maxAddress; i += 0x10) {
				sbln.Append(i.ToString("X10") + "\n");
			}
			TextRenderer.DrawText(graphics, sbln.ToString(), Font, new Point(fontWidth, 0), ForeColor);
		}
	}
}
