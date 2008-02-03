using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Kirishima16.Libraries.MP4Box;

namespace Kirishima16.Applications.AtomEditor2
{
	struct BoxLongPair
	{
		public BoxLongPair(BoxNode node1, long num1, BoxNode node2, long num2)
		{
			this.node1 = node1;
			this.number1 = num1;
			this.node2 = node2;
			this.number2 = num2;
		}

		BoxNode node1;

		public BoxNode Node1
		{
			get { return node1; }
			set { node1 = value; }
		}

		long number1;

		public long Number1
		{
			get { return number1; }
			set { number1 = value; }
		}

		BoxNode node2;

		public BoxNode Node2
		{
			get { return node2; }
			set { node2 = value; }
		}

		long number2;

		public long Number2
		{
			get { return number2; }
			set { number2 = value; }
		}
	}
}
