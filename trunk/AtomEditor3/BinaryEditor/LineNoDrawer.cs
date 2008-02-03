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
		/// �J�n�A�h���X
		/// </summary>
		private long startAddress;

		/// <summary>
		/// �J�n�A�h���X���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public long StartAddress
		{
			get { return startAddress; }
			set
			{
				if (value < 0 || maxAddress < value) {
					throw new ArgumentOutOfRangeException("StartAddress�ɂ�0�ȏ�MaxAddress�ȉ��̒l�̂ݎw��ł��܂��B");
				}
				startAddress = value;
			}
		}

		/// <summary>
		/// �ő�A�h���X
		/// </summary>
		private long maxAddress = 0x40;

		/// <summary>
		/// �ő�A�h���X���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public long MaxAddress
		{
			get { return maxAddress; }
			set
			{
				if (value < startAddress) {
					throw new ArgumentOutOfRangeException("MaxAddress�ɂ�StartAddress�ȏ�̒l�̂ݎw��ł��܂��B");
				}
				maxAddress = value;
			}
		}

		/// <summary>
		/// �s��
		/// </summary>
		private int lineCount = 5;

		/// <summary>
		/// �s�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public int LineCount
		{
			get { return lineCount; }
			set
			{
				if (value < 0) {
					throw new ArgumentOutOfRangeException("LineCount�ɂ�0�ȏ�̒l�̂ݎw��ł��܂��B");
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
