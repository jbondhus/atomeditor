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
		/// Boxの長さを取得または設定します。
		/// </summary>
		public uint Length
		{
			get { return length; }
			set { length = value; }
		}

		string name;

		/// <summary>
		/// Boxの名前を取得または設定します。
		/// </summary>
		public string BoxName
		{
			get { return name; }
			set { name = value; }
		}

		BoxNode parent;

		/// <summary>
		/// 親ボックスを取得または設定します。
		/// </summary>
		public BoxNode Parent
		{
			get { return parent; }
			set { parent = value; }
		}

		List<BoxNode> children;

		/// <summary>
		/// 子ボックスのコレクションを取得または設定します。
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
		/// byte配列からデータを読み込みます。
		/// </summary>
		/// <param name="data">読み込むbyte配列</param>
		public virtual void LoadBinary(byte[] data)
		{
			if (data.Length < 8) {
				throw new ArgumentException("Boxデータの長さが足りません。", "data");
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
		/// Boxの内容が保存されているテンポラリファイルを取得または設定します。
		/// </summary>
		public string DumpFile
		{
			get { return dumpFile; }
			set { dumpFile = value; }
		}

		long sourcePosition;

		/// <summary>
		/// Boxの読み込み時の絶対位置を取得または設定します。
		/// </summary>
		public long SourcePosition
		{
			get { return sourcePosition; }
			set { sourcePosition = value; }
		}

		long position;

		/// <summary>
		/// Boxの現在の絶対位置を取得または設定します。
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
