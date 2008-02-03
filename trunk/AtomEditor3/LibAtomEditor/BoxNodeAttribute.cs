using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// IBoxNode�h���N���X�̑Ή�����Box��錾���܂��B
	/// </summary>
	class BoxNodeAttribute : Attribute
	{
		string supportedName;

		/// <summary>
		/// �Ή�����Box�̖��O���擾���܂��B
		/// </summary>
		public string SupportedName
		{
			get { return supportedName; }
		}

		string[] supportedSubNames;

		/// <summary>
		/// �Ή�����Box�̕����̔z����擾���܂��B
		/// </summary>
		public string[] SupportedSubNames
		{
			get { return supportedSubNames; }
		}

		/// <summary>
		/// �Ή�����Box�̖��O�ƕ������w�肵��BoxNodeAttribute�����������܂��B
		/// </summary>
		/// <param name="name">�Ή�����Box�̖��O</param>
		/// <param name="subnames">�Ή�����Box�̕���(�����w���)</param>
		public BoxNodeAttribute(string name, params string[] subnames)
		{
			this.supportedName = name;
			this.supportedSubNames = subnames;
		}
	}
}
