using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Kirishima16.Forms
{
	internal class TextViewDrawer : DrawerControl
	{
		static SolidBrush brushRO = new SolidBrush(SystemColors.ButtonFace);

		/// <summary>
		/// 表示するバイナリデータ
		/// </summary>
		byte[] data = new byte[64];

		/// <summary>
		/// 表示するバイナリデータを取得または設定します。
		/// </summary>
		public byte[] Data
		{
			get { return data; }
			set { data = value; }
		}

		/// <summary>
		/// 読み取り専用ならばTrue、さもなければFalse
		/// </summary>
		bool readOnly;

		/// <summary>
		/// 読み取り専用かどうかを取得または設定します。
		/// </summary>
		public bool ReadOnly
		{
			get { return readOnly; }
			set { readOnly = value; }
		}

		public TextViewDrawer()
		{
			SetStyle(ControlStyles.Selectable | ControlStyles.UserMouse, true);
			RenderSurface();
		}

		Encoding enc = Encoding.GetEncoding(932);
		StringBuilder sb;
		public override void RenderSurface()
		{
			if (graphics == null) {
				return;
			}
			graphics.FillRectangle(!readOnly ? backBrush : brushRO, 0, 0, Width, Height);
			if (data.Length >= 0) {
				sb = new StringBuilder();
				byte curBin, nexBin;
				for (int i = 0; i < data.Length; i++) {
					curBin = data[i];
					nexBin = 0;
					if (i + 1 < data.Length) {
						nexBin = data[i + 1];
					}
					//ASCII+半角カナ
					if ((0x20 <= curBin && curBin <= 0x7F)
						|| (0xA1 <= curBin && curBin <= 0xDF)) {
						sb.Append(enc.GetString(new Byte[] { curBin }));
						//漢字
					} else if ((0x81 <= curBin && curBin <= 0x9F)
						|| (0xE0 <= curBin && curBin <= 0xFC)) {
						if (0x40 <= nexBin && nexBin <= 0xFC && nexBin != 7F) {
							if (i % 16 == 15) {
								sb.Append(".\n.");
							} else {
								sb.Append(enc.GetString(new Byte[] { curBin, nexBin }));
							}
							i++;
						} else {
							sb.Append(".");
						}
						//どっちでもない
					} else {
						sb.Append(".");
					}
					if (i % 16 == 15) {
						sb.Append("\n");
					}
				}
			}
			TextRenderer.DrawText(graphics, sb.ToString(), Font, new Point(fontWidth, 0), !readOnly ? ForeColor : SystemColors.ControlText);
		}
	}
}
