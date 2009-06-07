using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Forms
{
	internal class UndoEventArgs : EventArgs
	{
		UndoDelegate additionalOperation;

		internal UndoDelegate AdditionalOperation
		{
			get { return additionalOperation; }
			set { additionalOperation = value; }
		}

		public UndoEventArgs() { }

		public UndoEventArgs(UndoDelegate ao)
			: base()
		{
			additionalOperation = ao;
		}
	}
}
