using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// IBoxNode派生クラスの対応するBoxを宣言します。
	/// </summary>
	class BoxNodeAttribute : Attribute
	{
		string supportedName;

		/// <summary>
		/// 対応するBoxの名前を取得します。
		/// </summary>
		public string SupportedName
		{
			get { return supportedName; }
		}

		string[] supportedSubNames;

		/// <summary>
		/// 対応するBoxの副名の配列を取得します。
		/// </summary>
		public string[] SupportedSubNames
		{
			get { return supportedSubNames; }
		}

		/// <summary>
		/// 対応するBoxの名前と副名を指定してBoxNodeAttributeを初期化します。
		/// </summary>
		/// <param name="name">対応するBoxの名前</param>
		/// <param name="subnames">対応するBoxの副名(複数指定可)</param>
		public BoxNodeAttribute(string name, params string[] subnames)
		{
			this.supportedName = name;
			this.supportedSubNames = subnames;
		}
	}
}
