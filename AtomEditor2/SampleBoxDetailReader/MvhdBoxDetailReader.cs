using System;
using System.Collections.Generic;
using System.Text;
using Kirishima16.Libraries.AtomEditor;
using Kirishima16.Libraries.MP4Box;

namespace SampleBoxDetailReader
{
	public class MvhdBoxDetailReader : IBoxDetailReader
	{
		DateTime appleday = new DateTime(1904, 1, 1);

		#region IBoxDetailReader ÉÅÉìÉo

		public bool IsDecodable(BoxNode node)
		{
			if (node.BoxName == "mvhd") {
				return true;
			}
			return false;
		}

		public BoxDetail[] Decode(BoxNode xnode, byte[] data)
		{
			List<BoxDetail> details = new List<BoxDetail>();

			details.Add(new BoxDetail("Version", BitConverter.ToString(data, 0x08, 1), ""));
			details.Add(new BoxDetail("Flags", BitConverter.ToString(data, 0x09, 3), ""));
			Array.Reverse(data, 0x0C, 4);
			details.Add(new BoxDetail("Creation Date",
				appleday.AddSeconds((double)BitConverter.ToUInt32(data, 0x0C)).ToString(), ""));
			Array.Reverse(data, 0x10, 4);
			details.Add(new BoxDetail("Modification Date",
				appleday.AddSeconds((double)BitConverter.ToUInt32(data, 0x10)).ToString(), ""));
			Array.Reverse(data, 0x14, 4);
			uint tscale = BitConverter.ToUInt32(data, 0x14);
			details.Add(new BoxDetail("Time Scale", tscale.ToString(), ""));
			Array.Reverse(data, 0x18, 4);
			uint duration = BitConverter.ToUInt32(data, 0x18);
			details.Add(new BoxDetail("Duration", duration.ToString(), ""));
			details.Add(new BoxDetail("Duration", new TimeSpan((long)((duration / tscale) * Math.Pow(10, 7))).ToString(), ""));
			Array.Reverse(data, 0x1C, 2);
			Array.Reverse(data, 0x1E, 2);
			ushort rate1 = BitConverter.ToUInt16(data, 0x1C);
			ushort rate2 = BitConverter.ToUInt16(data, 0x1E);
			details.Add(new BoxDetail("Preferred Rate", (rate1 + (Single)(rate2 / 0x10000)).ToString(), ""));
			byte vol1 = data[0x20];
			byte vol2 = data[0x21];
			details.Add(new BoxDetail("Preferred Volume", (vol1 + (Single)(vol2 / 0x100)).ToString(), ""));
			details.Add(new BoxDetail("Reserved", BitConverter.ToString(data, 0x22, 0x2B), ""));
			Array.Reverse(data, 0x2C, 4);
			Array.Reverse(data, 0x30, 4);
			Array.Reverse(data, 0x34, 4);
			Array.Reverse(data, 0x38, 4);
			Array.Reverse(data, 0x3C, 4);
			Array.Reverse(data, 0x40, 4);
			Array.Reverse(data, 0x48, 4);
			Array.Reverse(data, 0x4C, 4);
			details.Add(new BoxDetail("Movie Matrix[A]", BitConverter.ToUInt32(data, 0x2C).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[B]", BitConverter.ToUInt32(data, 0x30).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[U]", BitConverter.ToUInt32(data, 0x34).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[C]", BitConverter.ToUInt32(data, 0x38).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[D]", BitConverter.ToUInt32(data, 0x3C).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[V]", BitConverter.ToUInt32(data, 0x40).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[X]", BitConverter.ToUInt32(data, 0x44).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[Y]", BitConverter.ToUInt32(data, 0x48).ToString(), ""));
			details.Add(new BoxDetail("Movie Matrix[W]", BitConverter.ToUInt32(data, 0x4C).ToString(), ""));
			Array.Reverse(data, 0x50, 4);
			Array.Reverse(data, 0x54, 4);
			Array.Reverse(data, 0x58, 4);
			Array.Reverse(data, 0x5C, 4);
			Array.Reverse(data, 0x60, 4);
			Array.Reverse(data, 0x64, 4);
			uint prevt = BitConverter.ToUInt32(data, 0x50);
			details.Add(new BoxDetail("Preview Time", prevt.ToString(), ""));
			details.Add(new BoxDetail("Preview Time", new TimeSpan((long)((prevt / tscale) * Math.Pow(10, 7))).ToString(), ""));
			uint prevd = BitConverter.ToUInt32(data, 0x54);
			details.Add(new BoxDetail("Preview Duration", prevt.ToString(), ""));
			details.Add(new BoxDetail("Preview Duration", new TimeSpan((long)((prevd / tscale) * Math.Pow(10, 7))).ToString(), ""));
			uint postert = BitConverter.ToUInt32(data, 0x58);
			details.Add(new BoxDetail("Poster Time", postert.ToString(), ""));
			details.Add(new BoxDetail("Poster Time", new TimeSpan((long)((postert / tscale) * Math.Pow(10, 7))).ToString(), ""));
			uint selectiont = BitConverter.ToUInt32(data, 0x5C);
			details.Add(new BoxDetail("Selection Time", selectiont.ToString(), ""));
			details.Add(new BoxDetail("Selection Time", new TimeSpan((long)((selectiont / tscale) * Math.Pow(10, 7))).ToString(), ""));
			uint selectiond = BitConverter.ToUInt32(data, 0x60);
			details.Add(new BoxDetail("Selection Duration", selectiond.ToString(), ""));
			details.Add(new BoxDetail("Selection Duration", new TimeSpan((long)((selectiond / tscale) * Math.Pow(10, 7))).ToString(), ""));
			uint currentt = BitConverter.ToUInt32(data, 0x64);
			details.Add(new BoxDetail("Current Time", currentt.ToString(), ""));
			details.Add(new BoxDetail("Current Time", new TimeSpan((long)((currentt / tscale) * Math.Pow(10, 7))).ToString(), ""));
			Array.Reverse(data, 0x68, 4);
			details.Add(new BoxDetail("Next Track ID", BitConverter.ToUInt32(data, 0x68).ToString(), ""));

			return details.ToArray();
		}

		#endregion
	}
}
