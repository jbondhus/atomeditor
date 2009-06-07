using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Kirishima16.Forms
{
	internal class HexViewDrawer : DrawerControl
	{
		static SolidBrush brushRO = new SolidBrush(SystemColors.ButtonFace);

		/// <summary>
		/// �\������o�C�i���f�[�^
		/// </summary>
		byte[] data = new byte[64];

		/// <summary>
		/// �\������o�C�i���f�[�^���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public byte[] Data
		{
			get { return data; }
			set { data = value; }
		}

		/// <summary>
		/// �ǂݎ���p�Ȃ��True�A�����Ȃ����False
		/// </summary>
		bool readOnly;

		/// <summary>
		/// �ǂݎ���p���ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool ReadOnly
		{
			get { return readOnly; }
			set { readOnly = value; }
		}

		public HexViewDrawer()
		{
			SetStyle(ControlStyles.Selectable | ControlStyles.UserMouse, true);
			RenderSurface();
		}

		StringBuilder sb;
		public override void RenderSurface()
		{
			if (graphics == null) {
				return;
			}
			graphics.FillRectangle(!readOnly ? backBrush : brushRO, 0, 0, Width, Height);
			if (data.Length > 0) {
				sb = new StringBuilder(data.Length * 3);
				for (int i = 0; i < data.Length; i += 0x10) {
					if (i + 0x10 < data.Length)
						sb.Append(BitConverter.ToString(data, i, 0x10).Replace('-', ' ') + "\n");
					else {
						sb.Append(BitConverter.ToString(data, i, data.Length - i).Replace('-', ' ') + "\n");
						break;
					}
				}
				TextRenderer.DrawText(graphics, sb.ToString(), Font, new Point(fontWidth, 0), !readOnly ? ForeColor : SystemColors.ControlText);
			}
		}
	}
}
