using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.MP4Box
{
	class FtypBoxNode : BoxNode
	{
		string majorBrand;

		public string MajorBrand
		{
			get { return majorBrand; }
			set { majorBrand = value; }
		}

		byte[] version = new byte[4];

		public byte[] Version
		{
			get { return version; }
			set { version = value; }
		}

		string[] compatibleBrands = new string[0];

		public string[] CompatibleBrands
		{
			get { return compatibleBrands; }
			set { compatibleBrands = value; }
		}

		public override void LoadBinary(byte[] data)
		{
			if (data.Length < 16) {
				throw new ArgumentException("Boxデータの長さが足りません。", "data");
			}
			base.LoadBinary(data);
			//0x0008-0x000B Major Brand
			majorBrand = Encoding.ASCII.GetString(data, 0x08, 4);
			//0x000C-0x000F Version
			Array.Copy(data, 0x0C, version, 0, 4);
			//0x0010-0x0000 Compatible Brands
			compatibleBrands = new string[(data.Length - 0x10) >> 2];
			for (int i = 0; i < compatibleBrands.Length; i++) {
				compatibleBrands[i] = Encoding.ASCII.GetString(data, 0x10 + i * 4, 4);
			}
		}

		public override byte[] SaveBinary()
		{
			byte[] data = new byte[0x10 + compatibleBrands.Length * 4];
			Array.Copy(base.SaveBinary(), 0, data, 0, 8);
			//0x0008-0x000B Major Brand
			Array.Copy(Encoding.ASCII.GetBytes(majorBrand), 0, data, 0x08, 4);
			//0x000C-0x000F Version
			Array.Copy(version, 0, data, 0x0C, 4);
			//0x0010-0x0000 Compatible Brands
			for (int i = 0; i < compatibleBrands.Length; i++) {
				Array.Copy(Encoding.ASCII.GetBytes(compatibleBrands[i]), 0, data, 0x10 + i * 4, 4);
			}

			return data;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(base.ToString());
			sb.AppendLine("Major Brand:" + majorBrand);
			sb.AppendLine("Version:" + BitConverter.ToString(version));
			for (int i = 0; i < compatibleBrands.Length; i++) {
				sb.AppendLine("Compatible Brands#" + i.ToString() + ":" + compatibleBrands[i]);
			}
			return sb.ToString();
		}
	}
}
