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
		/// �h���N���X�Ŏ��������Ǝw�肳�ꂽ�m�[�h����͉\���ǂ�����Ԃ��܂��B
		/// </summary>
		bool IsDecodable(BoxNode node);
		
		/// <summary>
		/// �h���N���X�Ŏ���������Box�f�[�^����͂��܂��B
		/// </summary>
		/// <param name="data">Box�f�[�^�̃o�C�g�z��</param>
		/// <returns>��͂��ꂽ�f�[�^�̔z��</returns>
		BoxDetail[] Decode(BoxNode node, byte[] data);
	}
}
