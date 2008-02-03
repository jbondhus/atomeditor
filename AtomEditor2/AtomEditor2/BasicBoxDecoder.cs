using System;
using System.Collections.Generic;
using System.Text;
using Kirishima16.Libraries.AtomEditor;
using Kirishima16.Libraries.MP4Box;

namespace Kirishima16.Applications.AtomEditor2
{
	class BasicBoxDecoder : IBoxDetailReader
	{
		byte[] tmpbuf4 = new byte[4];
		byte[] tmpbuf8 = new byte[8];
		byte[] tmpbuf12 = new byte[12];
		byte[] tmpbuf128 = new byte[128];
		uint timetmp;
		DateTime appleday = new DateTime(1904, 1, 1);


		#region IBoxDecoder ÉÅÉìÉo
		public bool IsDecodable(BoxNode node)
		{
			switch (node.BoxName) {
			case "ftyp":
			case "uuid":
				return true;
			default:
				return false;
			}
		}

		public BoxDetail[] Decode(BoxNode node, byte[] data)
		{
			List<BoxDetail> details = new List<BoxDetail>();
			switch (node.BoxName) {
			case "ftyp":
				Array.Copy(data, 8, tmpbuf4, 0, 4);
				details.Add(new BoxDetail("Major brand", Encoding.ASCII.GetString(tmpbuf4), ""));
				Array.Copy(data, 12, tmpbuf4, 0, 4);
				details.Add(new BoxDetail("Version", BitConverter.ToString(tmpbuf4), ""));
				for (int i = 0; i * 4 + 16 < node.Length; i++) {
					Array.Copy(data, 16 + (i * 4), tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Conpatible brand#" + i.ToString(), Encoding.ASCII.GetString(tmpbuf4), ""));
				}
				break;
			case "uuid":
				Array.Copy(data, 8, tmpbuf4, 0, 4);
				string uuidname = Encoding.ASCII.GetString(tmpbuf4);
				details.Add(new BoxDetail("Sub Name", uuidname, ""));
				Array.Copy(data, 12, tmpbuf12, 0, 12);
				details.Add(new BoxDetail("Uuid", BitConverter.ToString(tmpbuf12), ""));
				switch (uuidname) {
				case "mvml":
					Array.Copy(data, 24, tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Reserved?", BitConverter.ToString(tmpbuf4), ""));
					Array.Copy(data, 28, tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Flag", BitConverter.ToString(tmpbuf4), ""));
					Array.Copy(data, 32, tmpbuf4, 0, 4);
					Array.Reverse(tmpbuf4);
					timetmp = BitConverter.ToUInt32(tmpbuf4, 0);
					details.Add(new BoxDetail("Timestamp", (appleday.AddSeconds(timetmp)).ToString(), ""));
					break;
				case "enci":
					Array.Copy(data, 24, tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Reserved?", BitConverter.ToString(tmpbuf4), ""));
					Array.Copy(data, 28, tmpbuf8, 0, 8);
					details.Add(new BoxDetail("Hardware Vendor?", Encoding.ASCII.GetString(tmpbuf8), ""));
					Array.Copy(data, 36, tmpbuf8, 0, 8);
					details.Add(new BoxDetail("Hardware Name", Encoding.ASCII.GetString(tmpbuf8), ""));
					Array.Copy(data, 44, tmpbuf8, 0, 8);
					details.Add(new BoxDetail("Encoder Version?", Encoding.ASCII.GetString(tmpbuf8), ""));
					Array.Copy(data, 52, tmpbuf8, 0, 8);
					details.Add(new BoxDetail("Encoder Type", Encoding.ASCII.GetString(tmpbuf8), ""));
					break;
				case "cpgd":
					Array.Copy(data, 24, tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Flag1", BitConverter.ToString(tmpbuf4), ""));
					Array.Copy(data, 28, tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Flag2", BitConverter.ToString(tmpbuf4), ""));
					Array.Copy(data, 32, tmpbuf4, 0, 4);
					Array.Reverse(tmpbuf4);
					timetmp = BitConverter.ToUInt32(tmpbuf4, 0);
					details.Add(new BoxDetail("Expiration Date", (appleday.AddDays(timetmp)).ToString(), ""));
					Array.Copy(data, 36, tmpbuf4, 0, 4);
					Array.Reverse(tmpbuf4);
					timetmp = BitConverter.ToUInt32(tmpbuf4, 0);
					details.Add(new BoxDetail("Expiration Date", new TimeSpan(0, 0, (int)timetmp).ToString(), ""));
					Array.Copy(data, 40, tmpbuf4, 0, 4);
					Array.Reverse(tmpbuf4);
					details.Add(new BoxDetail("Expiration Count", BitConverter.ToUInt32(tmpbuf4, 0).ToString(), ""));
					break;
				case "chku":
					Array.Copy(data, 24, tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Reserved?", BitConverter.ToString(tmpbuf4), ""));
					Array.Copy(data, 28, tmpbuf128, 0, 128);
					details.Add(new BoxDetail("DigitalSignature", BitConverter.ToString(tmpbuf128), ""));
					break;
				case "prop":
					Array.Copy(data, 24, tmpbuf4, 0, 4);
					details.Add(new BoxDetail("Reserved?", BitConverter.ToString(tmpbuf4), ""));
					break;
				}
				break;
			}

			return details.ToArray();
		}
		#endregion
	}
}
