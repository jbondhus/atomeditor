using System;
using System.Collections.Generic;
using System.Text;
using Kirishima16.Libraries.MP4Box;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBoxDetailReader
	{
		/// <summary>
		/// 派生クラスで実装されると指定されたノードが解析可能かどうかを返します。
		/// </summary>
		bool IsDecodable(BoxNode node);
		
		/// <summary>
		/// 派生クラスで実装されるとBoxデータを解析します。
		/// </summary>
		/// <param name="data">Boxデータのバイト配列</param>
		/// <returns>解析されたデータの配列</returns>
		BoxDetail[] Decode(BoxNode node, byte[] data);
	}
}
