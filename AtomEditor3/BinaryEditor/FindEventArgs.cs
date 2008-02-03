using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Forms
{
	/// <summary>
	/// �������ʂ��i�[����EventArgs�ł��B
	/// </summary>
	public class FindEventArgs : EventArgs
	{
		long position;

		/// <summary>
		/// �����Ώۂ����������ʒu���擾���܂��B
		/// </summary>
		public long Position
		{
			get { return position; }
		}

		/// <summary>
		/// �����Ώۂ������������ǂ����擾���܂��B
		/// </summary>
		public bool Found
		{
			get { return position >= 0; }
		}

		/// <summary>
		/// �����Ώۂ����������ʒu���w�肵��FindEventArgs�����������܂��B
		/// </summary>
		/// <param name="pos">�����Ώۂ����������ʒu</param>
		public FindEventArgs(long pos)
		{
			this.position = pos;
		}
	}
}
