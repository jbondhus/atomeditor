using System;
using System.Collections.Generic;
using System.Text;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// Box��ҏW���邽�߂̋@�\���`���܂��B
	/// </summary>
	public interface IBoxEditor
	{
		/// <summary>
		/// �h���N���X�Ŏ��������Ǝw�肳�ꂽBox���J���邩�ǂ�����Ԃ��܂��B
		/// </summary>
		/// <param name="box">�m�F����Box</param>
		/// <returns>�J����Ȃ�True�A�����Ȃ���False</returns>
		bool CanOpen(BoxTreeNode box);

		/// <summary>
		/// �h���N���X�Ŏ���������Box���ҏW����Ă��邩�ǂ�����Ԃ��܂��B
		/// </summary>
		bool Modified { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������ƕҏW����Box���Q�Ƃ��܂��B
		/// </summary>
		BoxTreeNode Box { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������Ɗ֘A�t����ꂽBox���Q�Ƃ��܂��B
		/// </summary>
		BoxTreeNode SourceBox { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������ƕҏW����Box��byte�z����擾���܂��B
		/// </summary>
		byte[] BoxData { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������ƕҏW����Box�̃T�C�Y���擾���܂��B
		/// </summary>
		long BoxDataLength { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������Ǝw�肵��Box���J���܂��B
		/// </summary>
		/// <param name="box">�J��Box</param>
		/// <returns>�J�����Ȃ�True�A�����Ȃ���False</returns>
		bool OpenBox(BoxTreeNode box);

		/// <summary>
		/// �h���N���X�Ŏ��������ƊJ����Ă���Box��ۑ����܂��B
		/// </summary>
		void SaveBox();
	}
}
