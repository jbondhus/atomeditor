using System;
using System.Collections.Generic;
using System.Text;
using Kirishima16.Libraries.MP4Box;
using Kirishima16.Libraries.AtomEditor;

namespace UUIDPropBoxDetailReader
{
	public class UuidPropBoxDetailReader
	{
		byte[] tmpbuf4 = new byte[4];
		byte[] tmpbuf8 = new byte[8];
		byte[] tmpbuf12 = new byte[12];
		byte[] tmpbuf128 = new byte[128];
		uint timetmp;
		DateTime appleday = new DateTime(1904, 1, 1);

		#region IBoxDetailReader ÉÅÉìÉo
		public bool IsDecodable(BoxNode node)
		{
			return node.BoxName == "uuid";
		}

		public BoxDetail[] Decode(BoxNode node, byte[] data)
		{
			Array.Copy(data, 8, tmpbuf4, 0, 4);
			string uuidname = Encoding.ASCII.GetString(tmpbuf4);
			//details.Add(new BoxDetail("Sub Name", uuidname, ""));
			Array.Copy(data, 12, tmpbuf12, 0, 12);
			//details.Add(new BoxDetail("Uuid", BitConverter.ToString(tmpbuf12), ""));
			switch (uuidname) {
			case "titl":
				break;
			case "auth":
				break;
			case "lght":
				break;
			case "memo":
				break;
			case "vrsn":
				break;
			}
			throw new Exception("Not implemented.");
		}
		#endregion
	}
}
