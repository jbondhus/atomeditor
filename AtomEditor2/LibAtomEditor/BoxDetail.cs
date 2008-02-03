using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.AtomEditor
{
	public struct BoxDetail
	{
		public BoxDetail(string name, string data, string desc)
		{
			this.name = name;
			this.data = data;
			this.description = desc;
		}

		string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		string data;

		public string Data
		{
			get { return data; }
			set { data = value; }
		}
		string description;

		public string Description
		{
			get { return description; }
			set { description = value; }
		}
	}
}
