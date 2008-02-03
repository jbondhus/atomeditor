using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// Boxの基本的情報をオンメモリで管理する機能を定義します。
	/// </summary>
	public interface IBoxNode
	{
		/// <summary>
		/// 派生クラスで実装されるとBoxのサイズを取得または設定します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		uint BoxLength { get; set; }

		/// <summary>
		/// 派生クラスで実装されるとBoxの型名を取得または設定します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		string BoxName { get; set; }

		/// <summary>
		/// 派生クラスで実装されるとBoxの副名を取得または設定します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		string SubName { get; set; }

		/// <summary>
		/// 派生クラスで実装されるとBoxに関連付けられたファイルを取得または設定します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		string DumpFile { get; set; }

		/// <summary>
		/// 派生クラスで実装されるとBoxの現在の位置を取得または設定します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		long Position { get; set; }

		/// <summary>
		/// 派生クラスで実装されるとBoxのソースファイルでの位置を取得または設定します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		long SourcePotision { get; set; }

		/// <summary>
		/// 派生クラスで実装されると親Boxを取得します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		IBoxNode Parent { get; /*set;*/ }

		/// <summary>
		/// 派生クラスで実装されると子Boxのリストを取得します。
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		List<IBoxNode> Nodes { get; }

		/// <summary>
		/// 派生クラスで実装されるとバイナリデータからBoxの値を編集します。
		/// </summary>
		/// <param name="binary"></param>
		void LoadBinary(byte[] binary);
	}
}
