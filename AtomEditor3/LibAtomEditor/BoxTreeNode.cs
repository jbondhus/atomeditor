using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// IBoxTree�C���^�[�t�F�C�X��TreeNode���p�����������ł��B
	/// </summary>
	public class BoxTreeNode : TreeNode, IBoxNode
	{
		/// <summary>
		/// ���e���w�肹����BoxTreeNode�����������܂��B
		/// </summary>
		public BoxTreeNode()
			: base()
		{
			boxName = "";
			subName = "";
			dumpFile = "";
		}

		/// <summary>
		/// ������IBoxNode�I�u�W�F�N�g�̓��e����BoxTreeNode�����������܂��B
		/// </summary>
		/// <param name="box">�Q�Ƃ���IBoxNode�I�u�W�F�N�g</param>
		public BoxTreeNode(IBoxNode box)
			: base(box.BoxName)
		{
			this.boxLength = box.BoxLength;
			this.boxName = box.BoxName;
			this.subName = box.SubName;
			this.dumpFile = box.DumpFile;
			this.position = box.Position;
			this.sourcePosition = box.SourcePotision;
		}

		/// <summary>
		/// �w�肳�ꂽ�o�C�i���f�[�^����BoxTreeNode���쐬���܂��B
		/// </summary>
		/// <param name="binary"></param>
		/// <returns></returns>
		public static BoxTreeNode FromBinary(byte[] binary)
		{
			throw new Exception("Not Impled.");
		}

		#region IBoxNode �����o

		private uint boxLength;

		/// <summary>
		/// Box�̃T�C�Y���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public uint BoxLength
		{
			get { return boxLength; }
			set { boxLength = value; }
		}

		private string boxName;

		/// <summary>
		/// Box�̌^�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string BoxName
		{
			get { return boxName; }
			set { boxName = value; }
		}

		private string subName;

		/// <summary>
		/// Box�̕������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string SubName
		{
			get { return subName; }
			set { subName = value; }
		}

		private string dumpFile;

		/// <summary>
		/// Box�Ɋ֘A�t����ꂽ�t�@�C�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string DumpFile
		{
			get { return dumpFile; }
			set { dumpFile = value; }
		}

		private long position;

		/// <summary>
		/// Box�̌��݂̈ʒu���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public long Position
		{
			get { return position; }
			set { position = value; }
		}

		private long sourcePosition;

		/// <summary>
		/// Box�̃\�[�X�t�@�C���ł̈ʒu���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public long SourcePotision
		{
			get { return sourcePosition; }
			set { sourcePosition = value; }
		}

		/// <summary>
		/// �eBox���擾���܂��B
		/// </summary>
		IBoxNode IBoxNode.Parent
		{
			get { return base.Parent as IBoxNode; }
		}

		/// <summary>
		/// �qBox�̃��X�g���Q�Ƃ��܂��B
		/// </summary>
		/// <remarks>
		/// �qBox�̃��X�g��ύX����ɂ�BoxTreeNode.Nodes�v���p�e�B���擾���Ă��������B
		/// </remarks>
		List<IBoxNode> IBoxNode.Nodes
		{
			get
			{
				int cnt = base.Nodes.Count;
				List<IBoxNode> nodes = new List<IBoxNode>(cnt);
				for (int i = 0; i < cnt; i++) {
					nodes[i] = base.Nodes[i] as IBoxNode;
				}
				return nodes;
			}
		}

		/// <summary>
		/// �w�肳�ꂽ�o�C�i������T�C�Y�ƌ^����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="binary"></param>
		public void LoadBinary(byte[] binary)
		{
		}

		#endregion
	}
}
