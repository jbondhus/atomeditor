using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Forms
{
	/// <summary>
	/// 検索結果を格納するEventArgsです。
	/// </summary>
	public class FindEventArgs : EventArgs
	{
		long position;

		/// <summary>
		/// 検索対象が見つかった位置を取得します。
		/// </summary>
		public long Position
		{
			get { return position; }
		}

		/// <summary>
		/// 検索対象が見つかったかどうか取得します。
		/// </summary>
		public bool Found
		{
			get { return position >= 0; }
		}

		/// <summary>
		/// 検索対象が見つかった位置を指定してFindEventArgsを初期化します。
		/// </summary>
		/// <param name="pos">検索対象が見つかった位置</param>
		public FindEventArgs(long pos)
		{
			this.position = pos;
		}
	}
}
