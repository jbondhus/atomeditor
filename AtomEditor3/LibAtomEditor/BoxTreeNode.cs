using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// IBoxTreeインターフェイスのTreeNodeを継承した実装です。
	/// </summary>
	public class BoxTreeNode : TreeNode, IBoxNode
	{
		/// <summary>
		/// 内容を指定せずにBoxTreeNodeを初期化します。
		/// </summary>
		public BoxTreeNode()
			: base()
		{
			boxName = "";
			subName = "";
			dumpFile = "";
		}

		/// <summary>
		/// 既存のIBoxNodeオブジェクトの内容からBoxTreeNodeを初期化します。
		/// </summary>
		/// <param name="box">参照するIBoxNodeオブジェクト</param>
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
		/// 指定されたバイナリデータからBoxTreeNodeを作成します。
		/// </summary>
		/// <param name="binary"></param>
		/// <returns></returns>
		public static BoxTreeNode FromBinary(byte[] binary)
		{
			throw new Exception("Not Impled.");
		}

		#region IBoxNode メンバ

		private uint boxLength;

		/// <summary>
		/// Boxのサイズを取得または設定します。
		/// </summary>
		public uint BoxLength
		{
			get { return boxLength; }
			set { boxLength = value; }
		}

		private string boxName;

		/// <summary>
		/// Boxの型名を取得または設定します。
		/// </summary>
		public string BoxName
		{
			get { return boxName; }
			set { boxName = value; }
		}

		private string subName;

		/// <summary>
		/// Boxの副名を取得または設定します。
		/// </summary>
		public string SubName
		{
			get { return subName; }
			set { subName = value; }
		}

		private string dumpFile;

		/// <summary>
		/// Boxに関連付けられたファイルを取得または設定します。
		/// </summary>
		public string DumpFile
		{
			get { return dumpFile; }
			set { dumpFile = value; }
		}

		private long position;

		/// <summary>
		/// Boxの現在の位置を取得または設定します。
		/// </summary>
		public long Position
		{
			get { return position; }
			set { position = value; }
		}

		private long sourcePosition;

		/// <summary>
		/// Boxのソースファイルでの位置を取得または設定します。
		/// </summary>
		public long SourcePotision
		{
			get { return sourcePosition; }
			set { sourcePosition = value; }
		}

		/// <summary>
		/// 親Boxを取得します。
		/// </summary>
		IBoxNode IBoxNode.Parent
		{
			get { return base.Parent as IBoxNode; }
		}

		/// <summary>
		/// 子Boxのリストを参照します。
		/// </summary>
		/// <remarks>
		/// 子Boxのリストを変更するにはBoxTreeNode.Nodesプロパティを取得してください。
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
		/// 指定されたバイナリからサイズと型名を読み込みます。
		/// </summary>
		/// <param name="binary"></param>
		public void LoadBinary(byte[] binary)
		{
		}

		#endregion
	}
}
