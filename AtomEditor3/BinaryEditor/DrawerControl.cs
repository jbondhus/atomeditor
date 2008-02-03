using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Kirishima16.Forms
{
	/// <summary>
	/// BinaryEditorの各部コントロールの抽象基底クラスです。
	/// </summary>
	internal class DrawerControl : Control
	{
		/// <summary>
		/// フォントの幅
		/// </summary>
		protected int fontWidth;

		/// <summary>
		/// フォントの高さ
		/// </summary>
		protected int fontHeight;

		/// <summary>
		/// セカンダリサーフェスのBitmapオブジェクト
		/// </summary>
		private Bitmap surface;

		/// <summary>
		/// セカンダリサーフェスのBitmapオブジェクトを取得します。
		/// このBitmapからGraphicsオブジェクトを生成しないでください。
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public Bitmap Surface
		{
			get { return surface; }
		}

		#region Overrided Properties
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set
			{
				base.ForeColor = value;
				RenderSurface();
				Refresh();
			}
		}

		protected SolidBrush backBrush;

		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				backBrush = new SolidBrush(value);
				base.BackColor = value;
				RenderSurface();
				Refresh();
			}
		}

		protected override Size DefaultSize
		{
			get { return new Size(10, 10); }
		}

		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				fontHeight = value.Height;
				fontWidth = value.Height >> 1;
				RenderSurface();
			}
		}
		#endregion

		/// <summary>
		/// セカンダリサーフェスのGraphicsオブジェクトです。
		/// </summary>
		protected Graphics graphics;

		/// <summary>
		/// 派生クラスから呼び出されるとコントロールを初期化します。
		/// </summary>
		protected DrawerControl()
		{
			this.SetStyle(
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserMouse |
				ControlStyles.UserPaint, true);
			this.SetStyle(
				ControlStyles.ResizeRedraw |
				ControlStyles.Selectable, false);

			this.SizeChanged += new EventHandler(DrawerControl_SizeChanged);
			this.Paint += new PaintEventHandler(DrawerControl_Paint);

			BackColor = BackColor;
			Font = Font;

			CreateSurface();
		}

		/// <summary>
		/// セカンダリサーフェスを作成します。
		/// </summary>
		private void CreateSurface()
		{
			if (graphics != null) {
				graphics.Dispose();
			}
			surface = new Bitmap(Width, Height);
			graphics = Graphics.FromImage(surface);
		}

		/// <summary>
		/// 派生クラスでオーバーライドされるとセカンダリサーフェスを更新します。
		/// </summary>
		public virtual void RenderSurface()
		{
			if (graphics == null) {
				return;
			}
			graphics.FillRectangle(backBrush, 0, 0, Width, Height);
		}

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		/// <summary>
		/// 【SizeChanged】
		/// セカンダリサーフェスを更新します。
		/// </summary>
		private void DrawerControl_SizeChanged(object sender, EventArgs e)
		{
			CreateSurface();
			RenderSurface();
			Refresh();
		}

		/// <summary>
		/// 【Paint】
		/// セカンダリサーフェスを描画します。
		/// </summary>
		private void DrawerControl_Paint(object sender, PaintEventArgs e)
		{
			if (!Visible) {
				return;
			}
			e.Graphics.DrawImage(surface, 0, 0);
		}
	}
}
