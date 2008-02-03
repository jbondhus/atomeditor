using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// Boxエディタの製品情報を宣言します。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class BoxEditorAttribute : Attribute
	{
		private string name;

		/// <summary>
		/// 表示名を取得します。
		/// </summary>
		public string Name
		{
			get { return name; }
		}

		private string author;

		/// <summary>
		/// 製作者名を取得します。
		/// </summary>
		public string Author
		{
			get { return author; }
		}

		private string description;

		/// <summary>
		/// 説明を取得します。
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		/// <summary>
		/// 表示名・製作者名・説明を指定してBoxEditorAttributeを初期化します。
		/// </summary>
		/// <param name="name">表示名</param>
		/// <param name="author">製作者名</param>
		/// <param name="desc">説明</param>
		public BoxEditorAttribute(string name, string author, string desc)
		{
			this.name = name;
			this.author = author;
			this.description = desc;
		}
	}
}
