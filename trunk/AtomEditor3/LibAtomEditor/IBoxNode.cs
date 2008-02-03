using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// Box�̊�{�I�����I���������ŊǗ�����@�\���`���܂��B
	/// </summary>
	public interface IBoxNode
	{
		/// <summary>
		/// �h���N���X�Ŏ���������Box�̃T�C�Y���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		uint BoxLength { get; set; }

		/// <summary>
		/// �h���N���X�Ŏ���������Box�̌^�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		string BoxName { get; set; }

		/// <summary>
		/// �h���N���X�Ŏ���������Box�̕������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		string SubName { get; set; }

		/// <summary>
		/// �h���N���X�Ŏ���������Box�Ɋ֘A�t����ꂽ�t�@�C�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		string DumpFile { get; set; }

		/// <summary>
		/// �h���N���X�Ŏ���������Box�̌��݂̈ʒu���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		long Position { get; set; }

		/// <summary>
		/// �h���N���X�Ŏ���������Box�̃\�[�X�t�@�C���ł̈ʒu���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		long SourcePotision { get; set; }

		/// <summary>
		/// �h���N���X�Ŏ��������ƐeBox���擾���܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		IBoxNode Parent { get; /*set;*/ }

		/// <summary>
		/// �h���N���X�Ŏ��������ƎqBox�̃��X�g���擾���܂��B
		/// </summary>
		[ReadOnly(true)]
		[Category("Box")]
		List<IBoxNode> Nodes { get; }

		/// <summary>
		/// �h���N���X�Ŏ��������ƃo�C�i���f�[�^����Box�̒l��ҏW���܂��B
		/// </summary>
		/// <param name="binary"></param>
		void LoadBinary(byte[] binary);
	}
}
