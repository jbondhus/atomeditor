using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// Box�G�f�B�^�̐��i����錾���܂��B
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class BoxEditorAttribute : Attribute
	{
		private string name;

		/// <summary>
		/// �\�������擾���܂��B
		/// </summary>
		public string Name
		{
			get { return name; }
		}

		private string author;

		/// <summary>
		/// ����Җ����擾���܂��B
		/// </summary>
		public string Author
		{
			get { return author; }
		}

		private string description;

		/// <summary>
		/// �������擾���܂��B
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		/// <summary>
		/// �\�����E����Җ��E�������w�肵��BoxEditorAttribute�����������܂��B
		/// </summary>
		/// <param name="name">�\����</param>
		/// <param name="author">����Җ�</param>
		/// <param name="desc">����</param>
		public BoxEditorAttribute(string name, string author, string desc)
		{
			this.name = name;
			this.author = author;
			this.description = desc;
		}
	}
}
