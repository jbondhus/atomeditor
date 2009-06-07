using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Kirishima16.Libraries.MP4Box
{
	public class BoxNodeBase
	{
		uint length;

		/// <summary>
		/// Box�̒������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public uint Length
		{
			get { return length; }
			set { length = value; }
		}

		string name;

		/// <summary>
		/// Box�̖��O���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string BoxName
		{
			get { return name; }
			set { name = value; }
		}

		BoxNode parent;

		/// <summary>
		/// �e�{�b�N�X���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public BoxNode Parent
		{
			get { return parent; }
			set { parent = value; }
		}

		List<BoxNode> children;

		/// <summary>
		/// �q�{�b�N�X�̃R���N�V�������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public List<BoxNode> Children
		{
			get { return children; }
			set { children = value; }
		}

		public BoxNodeBase()
		{
			children = new List<BoxNode>();
		}

		/// <summary>
		/// byte�z�񂩂�f�[�^��ǂݍ��݂܂��B
		/// </summary>
		/// <param name="data">�ǂݍ���byte�z��</param>
		public virtual void LoadBinary(byte[] data)
		{
			if (data.Length < 8) {
				throw new ArgumentException("Box�f�[�^�̒���������܂���B", "data");
			}
			//0x0000-0x0003 Length
			Array.Reverse(data, 0, 4);
			length = BitConverter.ToUInt32(data, 0);
			//0x0004-0x0007 Name
			name = Encoding.ASCII.GetString(data, 4, 3);
		}

		public virtual byte[] SaveBinary()
		{
			byte[] data = new byte[8];
			//0x0000-0x0003 Length
			byte[] tmp = BitConverter.GetBytes(length);
			Array.Reverse(tmp);
			Array.Copy(tmp, 0, data, 0, 4);
			//0x0004-0x0007 Name
			Array.Copy(Encoding.ASCII.GetBytes(name), 0, data, 4, 4);

			return data;
		}
	}

	public class BoxNode : BoxNodeBase
	{
		string dumpFile;

		/// <summary>
		/// Box�̓��e���ۑ�����Ă���e���|�����t�@�C�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public string DumpFile
		{
			get { return dumpFile; }
			set { dumpFile = value; }
		}

		long sourcePosition;

		/// <summary>
		/// Box�̓ǂݍ��ݎ��̐�Έʒu���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public long SourcePosition
		{
			get { return sourcePosition; }
			set { sourcePosition = value; }
		}

		long position;

		/// <summary>
		/// Box�̌��݂̐�Έʒu���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public long Position
		{
			get { return position; }
			set { position = value; }
		}

		TreeNode treeNode;

		public TreeNode TreeNode
		{
			get { return treeNode; }
			set { treeNode = value; }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Name:" + BoxName);
			sb.AppendLine("Position:" + Position.ToString());
			sb.AppendLine("Length:" + Length.ToString());
			return sb.ToString().Trim();
		}
	}
}
