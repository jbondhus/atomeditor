using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Kirishima16.Forms
{
	/// <summary>
	/// BinaryEditor�̊e���R���g���[���̒��ۊ��N���X�ł��B
	/// </summary>
	internal class DrawerControl : Control
	{
		/// <summary>
		/// �t�H���g�̕�
		/// </summary>
		protected int fontWidth;

		/// <summary>
		/// �t�H���g�̍���
		/// </summary>
		protected int fontHeight;

		/// <summary>
		/// �Z�J���_���T�[�t�F�X��Bitmap�I�u�W�F�N�g
		/// </summary>
		private Bitmap surface;

		/// <summary>
		/// �Z�J���_���T�[�t�F�X��Bitmap�I�u�W�F�N�g���擾���܂��B
		/// ����Bitmap����Graphics�I�u�W�F�N�g�𐶐����Ȃ��ł��������B
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
		/// �Z�J���_���T�[�t�F�X��Graphics�I�u�W�F�N�g�ł��B
		/// </summary>
		protected Graphics graphics;

		/// <summary>
		/// �h���N���X����Ăяo�����ƃR���g���[�������������܂��B
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
		/// �Z�J���_���T�[�t�F�X���쐬���܂��B
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
		/// �h���N���X�ŃI�[�o�[���C�h�����ƃZ�J���_���T�[�t�F�X���X�V���܂��B
		/// </summary>
		public virtual void RenderSurface()
		{
			if (graphics == null) {
				return;
			}
			graphics.FillRectangle(backBrush, 0, 0, Width, Height);
		}

		/// <summary>
		/// �g�p���̃��\�[�X�����ׂăN���[���A�b�v���܂��B
		/// </summary>
		/// <param name="disposing">�}�l�[�W ���\�[�X���j�������ꍇ true�A�j������Ȃ��ꍇ�� false �ł��B</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		/// <summary>
		/// �ySizeChanged�z
		/// �Z�J���_���T�[�t�F�X���X�V���܂��B
		/// </summary>
		private void DrawerControl_SizeChanged(object sender, EventArgs e)
		{
			CreateSurface();
			RenderSurface();
			Refresh();
		}

		/// <summary>
		/// �yPaint�z
		/// �Z�J���_���T�[�t�F�X��`�悵�܂��B
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
