using System;
using System.Collections.Generic;
using System.Text;
using Kirishima16.Libraries.MP4Box;

namespace Kirishima16.Libraries.AtomEditor
{
	public interface IBoxEditor
	{
		/// <summary>
		/// 派生クラスで実装されるとエディタの表示名を返します。
		/// </summary>
		string EditorName { get; }

		/// <summary>
		/// 派生クラスで実装されると指定されたBoxが開けるかどうかを返します。
		/// </summary>
		/// <param name="box">確認するBox</param>
		/// <returns>開けるなら真、さもなくば偽</returns>
		bool CanOpen(BoxNode box);

		/// <summary>
		/// 派生クラスで実装されるとBoxが編集されているかを返します。
		/// </summary>
		bool Modified { get; }

		/// <summary>
		/// 派生クラスで実装されると関連付けられているBoxを返します。
		/// </summary>
		BoxNode Box { get; }

		/// <summary>
		/// 派生クラスで実装されてると現在の内容でbyte配列を返します。
		/// </summary>
		byte[] BoxData { get; }

		long BoxDataLength { get; }

		/// <summary>
		/// 派生クラスで実装されると指定されたBoxを開きます。
		/// </summary>
		/// <param name="box">開くBox</param>
		/// <returns>開けたなら真、さもなくば偽</returns>
		bool OpenBox(BoxNode box);

		/// <summary>
		/// 派生クラスで実装されると開かれているBoxを保存します。
		/// </summary>
		void SaveBox();
	}
}
