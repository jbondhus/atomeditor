using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Forms
{
	internal class BinaryPage
	{
		public BinaryPage(long pos, int len)
		{
			this.position = pos;
			this.sourcePosition = pos;
			this.length = len;
		}

		bool removed;

		public bool Removed
		{
			get { return removed; }
		}

		public void SetRemoved()
		{
			removed = true;
			//buffer = null;
		}

		public void ResetRemoved()
		{
			removed = false;
		}

		long position;

		public long Position
		{
			get { return position; }
			set { position = value; }
		}

		long sourcePosition;

		public long SourcePosition
		{
			get { return sourcePosition; }
		}

		int length;

		public int Length
		{
			get
			{
				if (Modified) {
					return buffer.Count;
				} else {
					return length;
				}
			}
		}

		public bool Modified
		{
			get { return buffer != null; }
		}

		List<byte> buffer;

		public List<byte> Buffer
		{
			get { return buffer; }
			set { buffer = value; }
		}
	}
}
