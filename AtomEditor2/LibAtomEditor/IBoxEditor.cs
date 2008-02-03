using System;
using System.Collections.Generic;
using System.Text;
using Kirishima16.Libraries.MP4Box;

namespace Kirishima16.Libraries.AtomEditor
{
	public interface IBoxEditor
	{
		/// <summary>
		/// �h���N���X�Ŏ��������ƃG�f�B�^�̕\������Ԃ��܂��B
		/// </summary>
		string EditorName { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������Ǝw�肳�ꂽBox���J���邩�ǂ�����Ԃ��܂��B
		/// </summary>
		/// <param name="box">�m�F����Box</param>
		/// <returns>�J����Ȃ�^�A�����Ȃ��΋U</returns>
		bool CanOpen(BoxNode box);

		/// <summary>
		/// �h���N���X�Ŏ���������Box���ҏW����Ă��邩��Ԃ��܂��B
		/// </summary>
		bool Modified { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������Ɗ֘A�t�����Ă���Box��Ԃ��܂��B
		/// </summary>
		BoxNode Box { get; }

		/// <summary>
		/// �h���N���X�Ŏ�������Ă�ƌ��݂̓��e��byte�z���Ԃ��܂��B
		/// </summary>
		byte[] BoxData { get; }

		long BoxDataLength { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������Ǝw�肳�ꂽBox���J���܂��B
		/// </summary>
		/// <param name="box">�J��Box</param>
		/// <returns>�J�����Ȃ�^�A�����Ȃ��΋U</returns>
		bool OpenBox(BoxNode box);

		/// <summary>
		/// �h���N���X�Ŏ��������ƊJ����Ă���Box��ۑ����܂��B
		/// </summary>
		void SaveBox();
	}
}
