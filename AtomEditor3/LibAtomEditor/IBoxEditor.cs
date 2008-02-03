using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// Boxを編集するための機能を定義します。
	/// </summary>
	public interface IBoxEditor
	{
		/// <summary>
		/// 派生クラスで実装されると指定されたBoxが開けるかどうかを返します。
		/// </summary>
		/// <param name="box">確認するBox</param>
		/// <returns>開けるならTrue、さもなくばFalse</returns>
		bool CanOpen(BoxTreeNode box);

		/// <summary>
		/// 派生クラスで実装されるとBoxが編集されているかどうかを返します。
		/// </summary>
		bool Modified { get; }

		/// <summary>
		/// 派生クラスで実装されると編集中のBoxを参照します。
		/// </summary>
		BoxTreeNode Box { get; }

		/// <summary>
		/// 派生クラスで実装されると関連付けられたBoxを参照します。
		/// </summary>
		BoxTreeNode SourceBox { get; }

		/// <summary>
		/// 派生クラスで実装されると編集中のBoxのbyte配列を取得します。
		/// </summary>
		byte[] BoxData { get; }

		/// <summary>
		/// 派生クラスで実装されると編集中のBoxのサイズを取得します。
		/// </summary>
		long BoxDataLength { get; }

		/// <summary>
		/// 派生クラスで実装されると指定したBoxを開きます。
		/// </summary>
		/// <param name="box">開くBox</param>
		/// <returns>開けたならTrue、さもなくばFalse</returns>
		bool OpenBox(BoxTreeNode box);

		/// <summary>
		/// 派生クラスで実装されると開かれているBoxを保存します。
		/// </summary>
		void SaveBox();
	}
}
