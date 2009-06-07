using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;

namespace Kirishima16.Forms
{
	/// <summary>
	/// �o�C�i���f�[�^��16�i�\�������Shift JIS������Ƃ��ĕ\���ŕ\������ѕҏW�ł���R���g���[����\�����܂��B
	/// </summary>
	[ToolboxBitmap(typeof(System.Drawing.Bitmap), "BinaryEditor.bmp")]
	public partial class BinaryEditor : UserControl
	{
		#region �����萔
		/// <summary>
		/// �o�C�i���f�[�^�̃f�[�^�t�H�[�}�b�g
		/// </summary>
		const string DF_PUREBINARY = "BinaryEditor.PureBinary";
		#endregion
		#region �����ϐ�
		/// <summary>
		/// �J�[�\���\���p��ColorMap
		/// </summary>
		ColorMap[]
			cmHexView = new ColorMap[] { new ColorMap(), new ColorMap() },
			cmTextView = new ColorMap[] { new ColorMap(), new ColorMap() },
			cmReadOnly = new ColorMap[] { new ColorMap(), new ColorMap() };

		/// <summary>
		/// �J�[�\���\���p��ImageAttribute
		/// </summary>
		ImageAttributes
			iaHexView = new ImageAttributes(),
			iaTextView = new ImageAttributes(),
			iaReadOnly = new ImageAttributes();

		/// <summary>
		/// 16�i�\���Ƀt�H�[�J�X�������True�A������\���Ȃ��False
		/// </summary>
		bool bHexFocused
		{
			get { return drwHexView.Focused; }
		}

		/// <summary>
		/// ����4�r�b�g��ҏW���Ȃ��True�A�����Ȃ����False
		/// </summary>
		bool bLesserBits;

		/// <summary>
		/// OnLoad���s��Ȃ��True�A�����Ȃ����False
		/// </summary>
		bool bLoaded;

		/// <summary>
		/// �����̕�
		/// </summary>
		int charWidth;

		/// <summary>
		/// �����̍���
		/// </summary>
		int charHeight;

		/// <summary>
		/// �\�����s���ׂ��s��
		/// </summary>
		int linecnt;

		/// <summary>
		/// �����X�N���[���ŃX�N���[�������s��
		/// </summary>
		int autoScrSpeed;

		/// <summary>
		/// �����X�N���[�����ɎQ�Ƃ����}�E�X���W
		/// </summary>
		int lastx, lasty;

		/// <summary>
		/// �\���͈͓��̃o�C�g�z��
		/// </summary>
		byte[] dispbuf = new byte[4000];

		long selectionStartBk;
		long selectionLengthBk;
		#endregion
		#region �C�x���g
		/// <summary>
		/// �o�C�i���f�[�^�̕ҏW��Ԃ��ύX���ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		[Browsable(true)]
		[Category("����")]
		[Description("�o�C�i���f�[�^�̕ҏW��Ԃ��ύX���ꂽ�Ƃ��ɔ������܂��B")]
		public event EventHandler ModifiedChanged;

		/// <summary>
		/// �ҏW���[�h���ύX���ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�v���p�e�B�ύX")]
		[Description("�ҏW���[�h���ύX���ꂽ�Ƃ��ɔ������܂��B")]
		public event EventHandler InsertModeChanged;

		/// <summary>
		/// ReadOnly�v���p�e�B���ύX���ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�v���p�e�B�ύX")]
		public event EventHandler ReadOnlyChanged;

		/// <summary>
		/// �I���J�n�ʒu���ύX���ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		[Browsable(true)]
		[Category("����")]
		public event EventHandler SelectionStartChanged;

		/// <summary>
		/// �I�𒷂��ύX���ꂽ�Ƃ��ɔ������܂��B
		/// </summary>
		[Browsable(true)]
		[Category("����")]
		public event EventHandler SelectionLengthChanged;

		/// <summary>
		/// <see cref="BeginFind"/>���\�b�h�����������Ƃ��ɔ������܂��B
		/// </summary>
		[Browsable(true)]
		[Category("����")]
		public event EventHandler<FindEventArgs> FindCompleted;
		#endregion
		#region �I�[�o�[���C�h
		/// <summary>
		/// �R���g���[���ɂ���ĕ\�������e�L�X�g�̃t�H���g���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				AjustFromFontSize();
			}
		}
		#endregion
		#region ���e�n�v���p�e�B
		BinaryBuffer binaryBuf;

		/// <summary>
		/// �\������ѕҏW�Ώۂ̃o�C�i���f�[�^���i�[���ꂽStream���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		/// <exception cref="System.ArgumentException">
		/// �ǂݍ��݂܂��̓V�[�N���ł��Ȃ��X�g���[����ݒ肷��ƃX���[����܂��B
		/// </exception>
		[Browsable(false)]
		[ReadOnly(true)]
		public Stream BinaryStream
		{
			get { return binaryBuf.Stream; }
			set
			{
				if (!(value.CanRead && value.CanSeek)) {
					throw new ArgumentException("�ǂݍ��݂���уV�[�N���\�ȃX�g���[���̂ݎw��ł��܂��B");
				}
				if (binaryBuf != null) {
					binaryBuf.Dispose();
				}
				binaryBuf = new BinaryBuffer(value);
				binaryBuf.BeforeStackUndo += new EventHandler<UndoEventArgs>(binaryBuf_BeforeStackUndo);
				linecnt = drwHexView.Height / charHeight + 1;
				AjustScrollBar();
				SelectionStart = 0;
				SelectionLength = 0;
				vscrMain.Value = 0;
				vscrMain_Scroll(vscrMain, new ScrollEventArgs(ScrollEventType.First, 0, vscrMain.Value));
				Modified = false;
			}
		}

		/// <summary>
		/// �ҏW���̃o�C�i���f�[�^���擾���܂��B
		/// </summary>
		public byte[] BinaryData
		{
			get { return binaryBuf.GetBytesAt(0, binaryBuf.Length); }
		}

		#endregion
		#region ����n�v���p�e�B
		/// <summary>
		/// �y�擾��p�z�G�f�B�^���ǂݎ���p�Ȃ��True�A�ҏW�\�Ȃ��False�B
		/// </summary>
		bool bReadOnly = false;

		/// <summary>
		/// �G�f�B�^�̃o�C�i���f�[�^��ύX�ł��邩�ǂ������擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("����")]
		[DefaultValue(false)]
		[Description("�G�f�B�^�̃o�C�i���f�[�^��ύX�ł��邩�ǂ�����ݒ肵�܂��B")]
		public bool ReadOnly
		{
			get { return bReadOnly; }
			set
			{
				bReadOnly = value;
				txtTextInput.ReadOnly = value;
				txtTextInput.Text = "";
				RenderHexView();
				RenderTextView();
				drwHexView.Refresh();
				drwTextView.Refresh();
				if (ReadOnlyChanged != null) {
					ReadOnlyChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// �\���J�n�ʒu�̃A�h���X
		/// </summary>
		long viewStart = 0;

		/// <summary>
		/// �y�擾��p�z�I���J�n�ʒu�̃A�h���X
		/// </summary>
		long selectionStart = 0;

		/// <summary>
		/// �I���J�n�ʒu�̃A�h���X���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(false)]
		public long SelectionStart
		{
			get { return selectionStart; }
			set
			{
				SetSelectionStart(value);
				//�O������̌Ăяo�����l������ƍĕ`�悹����𓾂Ȃ��B
				if (selectionStart < viewStart || viewStart + ((linecnt - 1) << 4) < selectionStart) {
					vscrMain.Value = checked((int)(selectionStart >> 4));
					ScrollToPosition(vscrMain.Value << 4);
				} else {
					drwHexView.Refresh();
					drwTextView.Refresh();
				}
			}
		}

		private void SetSelectionStart(long ss)
		{
			if (0 <= ss && ss <= binaryBuf.Length) {
				selectionStart = ss;
				if (SelectionStartChanged != null) {
					SelectionStartChanged(this, EventArgs.Empty);
				}
				SelectionLength = 0;
			} else {
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// �y�擾��p�z�I��͈͂̒�������ѕ���
		/// </summary>
		long selectionLength;

		/// <summary>
		/// �I��͈͂̒�������ѕ������擾�܂��͐ݒ肵�܂��B
		/// �}�C�i�X�̒l���ݒ肳�ꂽ�ꍇ����Ɍ������đI������܂��B
		/// 0�őI���Ȃ��A�}1�ňꕶ���I���ƂȂ�܂��B
		/// </summary>
		//TODO:�͈͐��������[
		[Browsable(false)]
		public long SelectionLength
		{
			get { return selectionLength; }
			set
			{
				if (value == 0 || selectionStart + value <= binaryBuf.Length) {
					selectionLength = value;
					if (SelectionLengthChanged != null) {
						SelectionLengthChanged(this, EventArgs.Empty);
					}
				} else {
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		/// <summary>
		/// �o�C�i���f�[�^�̑S�����擾���܂��B
		/// </summary>
		[Browsable(false)]
		public long BufferLength
		{
			get { return binaryBuf.Length; }
		}

		/// <summary>
		/// �y�擾��p�z�o�C�i���f�[�^���Ō�̓ǂݍ��݂܂��͕ۑ���ɕύX����Ă���Ȃ��True�A�����Ȃ����False�B
		/// </summary>
		bool bModified;

		/// <summary>
		/// �o�C�i���f�[�^���Ō�̓ǂݍ��݂܂��͕ۑ���ɕύX����Ă��邩�ǂ������擾���܂��B
		/// </summary>
		[Browsable(false)]
		public bool Modified
		{
			get { return bModified; }
			set
			{
				bool changed = bModified != value;
				bModified = value;
				if (changed && ModifiedChanged != null) {
					ModifiedChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// ���O�̃^�u�I�[�_�[�����R���g���[��
		/// </summary>
		Control beforeControl;

		/// <summary>
		/// ���O�̃^�u�I�[�_�[�����R���g���[�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("����")]
		[Description("���O�̃^�u�I�[�_�[�����R���g���[����ݒ肵�܂��B")]
		public Control BeforeControl
		{
			get { return beforeControl; }
			set { beforeControl = value; }
		}

		/// <summary>
		/// �y�擾��p�z�ҏW���[�h���}�����[�h�Ȃ�True�A�㏑�����[�h�Ȃ��False
		/// </summary>
		bool bInsertMode;

		/// <summary>
		/// �ҏW���[�h���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("����")]
		[Description("�ҏW���[�h��ݒ肵�܂��B")]
		[DefaultValue(false)]
		public bool InsertMode
		{
			get { return bInsertMode; }
			set
			{
				bInsertMode = value;
				if (InsertModeChanged != null) {
					InsertModeChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// �\��t����������s�\���ǂ������擾���܂��B
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public bool CanPaste
		{
			get { return !bReadOnly && Clipboard.ContainsData(DF_PUREBINARY); }
		}

		/// <summary>
		/// �A���h�D���삪���s�\���ǂ������擾���܂��B
		/// </summary>
		public bool CanUndo
		{
			get { return binaryBuf.CanUndo; }
		}

		bool finding;

		/// <summary>
		/// �񓯊��������s���Ă��邩�ǂ������擾���܂��B
		/// </summary>
		public bool Finding
		{
			get { return finding; }
		}
		#endregion
		#region �\���n�v���p�e�B
		/// <summary>
		/// �w�b�_�̑O�i�F��ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("�w�b�_�̑O�i�F���擾�܂��͐ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "Gray")]
		public Color HeaderForeColor
		{
			get { return drwHeader1.ForeColor; }
			set
			{
				drwHeader1.ForeColor = value;
				drwHeader2.ForeColor = value;
				drwHeader3.ForeColor = value;
			}
		}

		/// <summary>
		/// �w�b�_�̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("�w�b�_�̔w�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "Yellow")]
		public Color HeaderBackgroundColor
		{
			get { return drwHeader1.BackColor; }
			set
			{
				drwHeader1.BackColor = value;
				drwHeader2.BackColor = value;
				drwHeader3.BackColor = value;
			}
		}

		/// <summary>
		/// �s�ԍ��̑O�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("�s�ԍ��̑O�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "White")]
		public Color LineNoForeColor
		{
			get { return drwLineNo.ForeColor; }
			set { drwLineNo.ForeColor = value; }
		}

		/// <summary>
		/// �s�ԍ��̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("�s�ԍ��̔w�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "Gray")]
		public Color LineNoBackgrountColor
		{
			get { return drwLineNo.BackColor; }
			set { drwLineNo.BackColor = value; }
		}

		/// <summary>
		/// 16�i�\���̑O�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("16�i�\���̑O�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "Black")]
		public Color HexViewForeColor
		{
			get { return drwHexView.ForeColor; }
			set { drwHexView.ForeColor = value; }
		}

		/// <summary>
		/// 16�i�\���̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("16�i�\���̔w�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "White")]
		public Color HexViewBackColor
		{
			get { return drwHexView.BackColor; }
			set { drwHexView.BackColor = value; }
		}

		/// <summary>
		/// ������\���̑O�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("������\���̑O�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "Black")]
		public Color TextViewForeColor
		{
			get { return drwTextView.ForeColor; }
			set { drwTextView.ForeColor = value; }
		}

		/// <summary>
		/// ������\���̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("������\���̔w�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "White")]
		public Color TextViewBackColor
		{
			get { return drwTextView.BackColor; }
			set { drwTextView.BackColor = value; }
		}

		Color colorSelectionF = SystemColors.HighlightText;
		/// <summary>
		/// �I��͈͂̑O�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("�I��͈͂̑O�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "HighlightText")]
		public Color SelectionForeColor
		{
			get { return colorSelectionF; }
			set
			{
				colorSelectionF = value;
				cmHexView[0].NewColor = value;
				cmTextView[0].NewColor = value;
				cmReadOnly[0].NewColor = value;
				iaHexView.SetRemapTable(cmHexView);
				iaTextView.SetRemapTable(cmTextView);
				iaReadOnly.SetBrushRemapTable(cmReadOnly);
				if (DesignMode || bLoaded) {
					drwHexView.Refresh();
					drwTextView.Refresh();
				}
			}
		}

		Color colorSelectionB = SystemColors.Highlight;
		/// <summary>
		/// �I��͈͂̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[Browsable(true)]
		[Category("�\��")]
		[Description("�I��͈͂̔w�i�F��ݒ肵�܂��B")]
		[DefaultValue(typeof(Color), "Highlight")]
		public Color SelectionBackColor
		{
			get { return colorSelectionB; }
			set
			{
				colorSelectionB = value;
				cmHexView[1].NewColor = value;
				cmTextView[1].NewColor = value;
				cmReadOnly[1].NewColor = value;
				iaHexView.SetRemapTable(cmHexView);
				iaTextView.SetRemapTable(cmTextView);
				iaReadOnly.SetBrushRemapTable(cmReadOnly);
				if (DesignMode || bLoaded) {
					drwHexView.Refresh();
					drwTextView.Refresh();
				}
			}
		}
		#endregion
		#region �������n���\�b�h�E�C�x���g�n���h��
		/// <summary>
		/// ����̃o�C�i����\������o�C�i���G�f�B�^�R���g���[�������������܂��B
		/// </summary>
		public BinaryEditor()
		{
			SetStyle(
				ControlStyles.ResizeRedraw |
				ControlStyles.OptimizedDoubleBuffer, false);

			byte[] buf = new byte[0x40];
			binaryBuf = new BinaryBuffer(new MemoryStream(buf));
			binaryBuf.BeforeStackUndo += new EventHandler<UndoEventArgs>(binaryBuf_BeforeStackUndo);

			InitializeComponent();

			if (beforeControl == null) {
				beforeControl = txtTextInput;
			}

			cmHexView[0].OldColor = drwHexView.ForeColor;
			cmHexView[0].NewColor = colorSelectionF;
			cmHexView[1].OldColor = drwHexView.BackColor;
			cmHexView[1].NewColor = colorSelectionB;
			cmTextView[0].OldColor = drwTextView.ForeColor;
			cmTextView[0].NewColor = colorSelectionF;
			cmTextView[1].OldColor = drwTextView.BackColor;
			cmTextView[1].NewColor = colorSelectionB;
			cmReadOnly[0].OldColor = SystemColors.ControlText;
			cmReadOnly[0].NewColor = colorSelectionF;
			cmReadOnly[1].OldColor = SystemColors.ButtonFace;
			cmReadOnly[1].NewColor = colorSelectionB;

			iaHexView.SetRemapTable(cmHexView);
			iaTextView.SetRemapTable(cmTextView);
			iaReadOnly.SetRemapTable(cmReadOnly);

			this.MouseWheel += new MouseEventHandler(BinaryEditor_MouseWheel);
			drwHexView.GotFocus += new EventHandler(drwBothView_GotFocus);
			drwTextView.GotFocus += new EventHandler(drwBothView_GotFocus);
			drwHexView.LostFocus += new EventHandler(drwBothView_LostFocus);
			drwTextView.LostFocus += new EventHandler(drwBothView_LostFocus);
		}

		void binaryBuf_BeforeStackUndo(object sender, UndoEventArgs e)
		{
			long ss = selectionStartBk,
				sl = selectionLengthBk;
			e.AdditionalOperation = new UndoDelegate(delegate() {
				long ss2 = ss, sl2 = sl;
				UndoSelection(ss, sl);
				drwHexView.Refresh();
				drwTextView.Refresh();
			});
		}

		void drwBothView_GotFocus(object sender, EventArgs e)
		{
			drwHexView.Refresh();
			drwTextView.Refresh();
		}

		void drwBothView_LostFocus(object sender, EventArgs e)
		{
			drwHexView.Refresh();
			drwTextView.Refresh();
		}

		/// <summary>
		/// ��v�ϐ��ݒ��̏������E�ĕ`�揈�����s���܂��B
		/// </summary>
		/// <remarks>
		/// ���̃C�x���g���Ăяo�����ȑO�̃v���p�e�B�ݒ莞�̕`�揈���͗}�~����Ȃ���΂Ȃ�Ȃ��B
		/// �������f�U�C�����[�h���͏����B
		/// </remarks>
		private void BinaryEditor_Load(object sender, EventArgs e)
		{
			bLoaded = true;
			AjustScrollBar();
			AjustFromFontSize();
			SetDispBuf();
			RenderHeader();
			RenderLineNo();
			RenderHexView();
			RenderTextView();
			RefreshAllViewes();
		}
		#endregion
		#region �����n���\�b�h�E�C�x���g�n���h��

		private void AjustRectangles()
		{
			linecnt = drwHexView.Height / charHeight + 1;
		}

		private void BinaryEditor_SizeChanged(object sender, EventArgs e)
		{
			AjustRectangles();
			SetDispBuf();
			RenderHeader();
			RenderLineNo();
			RenderHexView();
			RenderTextView();
			drwHeader1.Refresh();
			drwHeader2.Refresh();
			drwHeader3.Refresh();
			RefreshAllViewes();
			if (binaryBuf.Length > 0) {
				AjustScrollBar();
			} else {
				vscrMain.Maximum = 0;
			}
		}

		/// <summary>
		/// �X�N���[���o�[�𒲐����܂��B
		/// </summary>
		private void AjustScrollBar()
		{
			vscrMain.Maximum = (int)(binaryBuf.Length >> 4) + linecnt - ((binaryBuf.Length % 16 == 0) ? 3 : 2);
			vscrMain.LargeChange = Math.Max(0, linecnt - 1);
		}

		/// <summary>
		/// �t�H���g�T�C�Y�ύX���̃T�C�Y�����ƍĕ`����s���܂��B
		/// </summary>
		private void AjustFromFontSize()
		{
			charHeight = Font.Height;
			charWidth = charHeight >> 1;

			SuspendLayout();
			drwHeader1.Height =
			drwHeader2.Height =
			drwHeader3.Height = charHeight;
			drwHeader1.Width =
			drwLineNo.Width = charWidth * 12 + 3;
			drwHeader2.Width =
			drwHexView.Width = charWidth * 49 + 3;
			ResumeLayout();

			AjustRectangles();
			RenderAllViewes();
			RefreshAllViewes();
		}

		/// <summary>
		/// �\���̈��byte�z����X�V���܂��B
		/// </summary>
		private void SetDispBuf()
		{
			int displen = Math.Min(16 * linecnt, (int)(binaryBuf.Length - viewStart));
			dispbuf = binaryBuf.GetBytesAt(viewStart, displen);
		}

		/// <summary>
		/// �A���h�D��ɑI��͈͂𕜋A���܂��B
		/// </summary>
		/// <param name="ss"></param>
		/// <param name="sl"></param>
		private void UndoSelection(long ss, long sl)
		{
			SelectionStart = ss;
			SelectionLength = sl;
		}

		private void RenderAllDrawers()
		{
			RenderHeader();
			RenderAllViewes();
		}

		private void RenderAllViewes()
		{
			SetDispBuf();
			AjustScrollBar();
			RenderLineNo();
			RenderHexView();
			RenderTextView();
		}

		private void RefreshAllDraweres()
		{
			drwHeader1.Refresh();
			drwHeader2.Refresh();
			drwHeader3.Refresh();
			RefreshAllViewes();
		}

		private void RefreshAllViewes()
		{
			drwLineNo.Refresh();
			drwHexView.Refresh();
			drwTextView.Refresh();
		}
		#endregion
		#region �Z�J���_���T�[�t�F�X�����������\�b�h
		/// <summary>
		/// �w�b�_�̃Z�J���_���T�[�t�F�X�����������܂��B
		/// </summary>
		private void RenderHeader()
		{
			if (!bLoaded && !DesignMode)
				return;

			drwHeader1.RenderSurface();
			drwHeader2.RenderSurface();
			drwHeader3.RenderSurface();
		}

		/// <summary>
		/// �s�ԍ��̃Z�J���_���T�[�t�F�X�����������܂��B
		/// </summary>
		private void RenderLineNo()
		{
			if (!bLoaded && !DesignMode)
				return;

			drwLineNo.StartAddress = viewStart;
			drwLineNo.MaxAddress = binaryBuf.Length;
			drwLineNo.LineCount = linecnt;
			drwLineNo.RenderSurface();
		}

		/// <summary>
		/// 16�i�\���̃Z�J���_���T�[�t�F�X�����������܂��B
		/// </summary>
		private void RenderHexView()
		{
			if (!bLoaded && !DesignMode)
				return;

			drwHexView.Data = dispbuf;
			drwHexView.ReadOnly = bReadOnly;
			drwHexView.RenderSurface();
		}

		Encoding enc = Encoding.GetEncoding(932);
		/// <summary>
		/// �e�L�X�g�\���̃Z�J���_���T�[�t�F�X�����������܂��B
		/// </summary>
		private void RenderTextView()
		{
			if (!bLoaded && !DesignMode)
				return;

			drwTextView.Data = dispbuf;
			drwTextView.ReadOnly = bReadOnly;
			drwTextView.RenderSurface();
		}
		#endregion
		#region Paint�C�x���g�n���h��
		/// <summary>
		/// 16�i�\���̃J�[�\���܂��͑I����`�悵�܂��B
		/// </summary>
		private void drwHexView_Paint(object sender, PaintEventArgs e)
		{
			if (!Visible) {
				return;
			}
			Bitmap bmp = drwHexView.Surface;
			//�J���b�g�̕\��
			long len = binaryBuf.Length;
			int displen = Math.Min(linecnt << 4, (int)(len - viewStart));
			int disppos = (int)(selectionStart - viewStart);
			if (binaryBuf.Length >= 0) {
				if (selectionLength == 0 && viewStart <= selectionStart && selectionStart < viewStart + len + 1) {
					//�I���Ȃ�
					Rectangle r = drwHexView.Focused ?
						!bInsertMode ?
							new Rectangle((disppos % 16) * charWidth * 3 + charWidth, (disppos >> 4) * charHeight, charWidth, charHeight) :
								new Rectangle((disppos % 16) * charWidth * 3 + charWidth, (disppos >> 4) * charHeight, 2, charHeight) :
								new Rectangle((disppos % 16) * charWidth * 3 + charWidth, ((disppos >> 4) + 1) * charHeight - 2, charWidth, 2);
					r.Offset(1, 0);
					if (bLesserBits)
						r.Offset(charWidth, 0);
					e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaHexView : iaReadOnly);
				} else {
					//�I������
					long bgn = 0, end = 0;
					if (selectionLength > 0) {
						bgn = selectionStart;
						end = selectionStart + selectionLength - 1;
					} else {
						bgn = selectionStart + selectionLength + 1;
						end = selectionStart;
					}

					//�\���͈͓��̂ݕ`��
					if (end >= viewStart && viewStart + displen >= bgn) {
						bgn = Math.Max(bgn - viewStart, 0);
						end = Math.Min(end - viewStart, displen - 1);
					} else {
						return;
					}

					//�s��
					if ((end >> 4) == (bgn >> 4)) {
						Point p = new Point((int)((bgn % 16) * charWidth * 3 + charWidth), (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, (int)(((end % 16 + 1) * charWidth * 3) - p.X), charHeight);
						r.Offset(1, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaHexView : iaReadOnly);
						return;
					}

					//�J�n�s
					if (bgn % 16 != 0) {
						int tlen = (int)((((bgn >> 4) + 1) << 4) - bgn);
						Point p = new Point((int)((bgn % 16) * charWidth * 3 + charWidth), (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, (tlen * 3 - 1) * charWidth, charHeight);
						r.Offset(1, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaHexView : iaReadOnly);
						bgn += tlen;
					}
					if (end - bgn < 0) {
						return;
					}

					//�I���s
					if (end % 16 != 15) {
						int tlen = (int)(end - ((end >> 4) << 4) + 1);
						Point p = new Point(charWidth, (int)((end >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, (tlen * 3 - 1) * charWidth, charHeight);
						r.Offset(1, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaHexView : iaReadOnly);
						end -= tlen;
					}
					if (end - bgn < 0) {
						return;
					}

					//���ԍs
					{
						int tlen = (int)(end - bgn + 1);
						Point p = new Point(charWidth, (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, charWidth * 47, charHeight * (tlen >> 4));
						r.Offset(1, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaHexView : iaReadOnly);
					}
				}
			}
		}

		/// <summary>
		/// ������\���̃Z�J���_���T�[�t�F�X��`�悵�܂��B
		/// </summary>
		private void drwTextView_Paint(object sender, PaintEventArgs e)
		{
			if (!Visible) {
				return;
			}
			Bitmap bmp = drwTextView.Surface;

			//�J���b�g�̕\��
			long len = binaryBuf.Length;
			int displen = Math.Min(linecnt << 4, (int)(len - viewStart));
			int disppos = (int)(selectionStart - viewStart);
			if (binaryBuf.Length >= 0) {
				if (selectionLength == 0 && viewStart <= selectionStart && selectionStart < viewStart + len + 1) {
					//�I���Ȃ�
					Rectangle r = drwTextView.Focused || txtTextInput.Focused ?
						!bInsertMode ?
							new Rectangle((disppos % 16 + 1) * charWidth, (disppos >> 4) * charHeight, charWidth, charHeight) :
							new Rectangle((disppos % 16 + 1) * charWidth, (disppos >> 4) * charHeight, 2, charHeight) :
						new Rectangle((disppos % 16 + 1) * charWidth, ((disppos >> 4) + 1) * charHeight - 2, charWidth, 2);
					r.Offset(1, 0);
					e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaTextView : iaReadOnly);
				} else {
					//�I������
					long bgn = 0, end = 0;
					if (selectionLength > 0) {
						bgn = selectionStart;
						end = selectionStart + selectionLength - 1;
					} else {
						bgn = selectionStart + selectionLength + 1;
						end = selectionStart;
					}

					//�I��͈͓��̂ݕ`��
					if (end >= viewStart || viewStart + displen >= bgn) {
						bgn = Math.Max(bgn - viewStart, 0);
						end = Math.Min(end - viewStart, displen - 1);
					} else {
						return;
					}

					//�s��
					if ((end >> 4) == (bgn >> 4)) {
						Point p = new Point((int)((bgn % 16) * charWidth + charWidth), (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, (int)(((end % 16 + 2) * charWidth) - p.X), charHeight);
						r.Offset(1, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaTextView : iaReadOnly);
						return;
					}

					//�J�n�s
					if (bgn % 16 != 0) {
						int tlen = (int)((((bgn >> 4) + 1) << 4) - bgn);
						Point p = new Point((int)((bgn % 16) * charWidth + charWidth), (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, tlen * charWidth, charHeight);
						r.Offset(2, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaTextView : iaReadOnly);
						bgn += tlen;
					}
					if (end - bgn < 0) {
						return;
					}

					//�ŏI�s
					if (end % 16 != 15) {
						int tlen = (int)(end - ((end >> 4) << 4) + 1);
						Point p = new Point(charWidth, (int)((end >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, tlen * charWidth, charHeight);
						r.Offset(2, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaTextView : iaReadOnly);
						end -= tlen;
					}
					if (end - bgn < 0) {
						return;
					}

					//���ԍs
					{
						int tlen = (int)(end - bgn + 1);
						Point p = new Point(charWidth, (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, charWidth * 16, charHeight * (tlen >> 4));
						r.Offset(2, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaTextView : iaReadOnly);
					}
				}
			}
		}
		#endregion
		#region �}�E�X����n���\�b�h�E�C�x���g�n���h��
		private void vscrMain_Scroll(object sender, ScrollEventArgs e)
		{
			//���d�`��h�~
			if (e.Type == ScrollEventType.EndScroll)
				return;

			ScrollToPosition(e.NewValue * 16);
		}

		private void ScrollToPosition(long pos)
		{
			ScrollToPosition(pos, true);
		}

		private void ScrollToPosition(long pos, bool refresh)
		{
			viewStart = pos;
			if (refresh) {
				RenderAllViewes();
				RefreshAllViewes();
			}
		}

		void BinaryEditor_MouseWheel(object sender, MouseEventArgs e)
		{
			int old = vscrMain.Value;
			if (e.Delta > 0) {
				//�����
				vscrMain.Value = Math.Max(vscrMain.Value - e.Delta / 20, vscrMain.Minimum);
			} else {
				//������
				vscrMain.Value = Math.Min(vscrMain.Value - e.Delta / 20, vscrMain.Maximum - vscrMain.LargeChange + 1);
			}
			ScrollToPosition(vscrMain.Value << 4);
		}

		private void drwHexView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift) {
					//�I���J�n�ʒu
					bLesserBits = false;
					SelectionStart = HexPointToPos(e.X, e.Y, false);
					SelectionLength = 0;
					drwHexView.Refresh();
					drwTextView.Refresh();
				} else {
					//�I���I���ʒu
					SelectHexTo(e);
				}
			}
		}

		private void drwHexView_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				StartAutoScr(e);
				SelectHexTo(e);
			}
		}

		private void SelectHexTo(MouseEventArgs e)
		{
			SelectHexTo(e.X, e.Y);
		}

		private void SelectHexTo(int x, int y)
		{
			long end = HexPointToPos(x, y, true);
			long len = binaryBuf.Length;
			do {
				if (len == 0) {
					selectionLength = 0;
					break;
				}
				if (selectionStart == len) {
					SelectionStart = len - 1;
				}
				if (end == len) {
					end = len - 1;
				}
				SelectionLength = end - selectionStart;
				SelectionLength += (selectionLength > 0 ? 1 : -1);
			} while (false);
			drwHexView.Refresh();
			drwTextView.Refresh();
		}

		private void drwHexView_MouseUp(object sender, MouseEventArgs e)
		{
			if (tmrAutoScr.Enabled == true) {
				tmrAutoScr.Stop();
			}
		}

		private long HexPointToPos(int x, int y, bool isend)
		{
			x = Math.Min(charWidth * 47, Math.Max(0, x));
			y = Math.Min(charHeight * linecnt, Math.Max(0, y));
			long pos = Math.Min(viewStart + (x / (charWidth * 3)) + ((y / charHeight) << 4),
				isend ? binaryBuf.Length - 1 : binaryBuf.Length);
			return pos;
		}

		private void drwTextView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift) {
					//�I���J�n�ʒu
					bLesserBits = false;
					SelectionStart = TextPointToPos(e.X, e.Y, false);
					SelectionLength = 0;
					drwHexView.Refresh();
					drwTextView.Refresh();
				} else {
					//�I���I���ʒu
					SelectTextTo(e);
				}
			}
		}

		private void drwTextView_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				StartAutoScr(e);
				SelectTextTo(e);
			}
		}

		private void SelectTextTo(MouseEventArgs e)
		{
			SelectTextTo(e.X, e.Y);
		}

		private void SelectTextTo(int x, int y)
		{
			long end = TextPointToPos(x, y, true);
			long len = binaryBuf.Length;
			do {
				if (len == 0) {
					selectionLength = 0;
					break;
				}
				if (selectionStart == len) {
					SelectionStart = len - 1;
				}
				if (end == len) {
					end = len - 1;
				}
				SelectionLength = end - selectionStart;
				SelectionLength += (selectionLength > 0 ? 1 : -1);
			} while (false);
			drwHexView.Refresh();
			drwTextView.Refresh();
		}

		private void drwTextView_MouseUp(object sender, MouseEventArgs e)
		{
			if (tmrAutoScr.Enabled == true) {
				tmrAutoScr.Stop();
			}
		}

		private long TextPointToPos(int x, int y, bool isend)
		{
			x = Math.Min(charWidth * 16, Math.Max(0, x));
			y = Math.Min(charHeight * linecnt, Math.Max(0, y));
			long pos = Math.Min(viewStart + ((x / charWidth) - 1) + ((y / charHeight) << 4),
				isend ? binaryBuf.Length - 1 : binaryBuf.Length);
			return pos;
		}

		private void StartAutoScr(MouseEventArgs e)
		{
			lastx = e.X;
			lasty = e.Y;
			if (e.Y < charHeight) {
				autoScrSpeed = e.Y / charHeight - 2;
				if (!tmrAutoScr.Enabled) {
					tmrAutoScr.Start();
				}
			} else if (e.Y > drwTextView.Height - charHeight) {
				autoScrSpeed = (e.Y - drwTextView.Height) / charHeight + 1;
				if (!tmrAutoScr.Enabled) {
					tmrAutoScr.Start();
				}
			} else {
				tmrAutoScr.Stop();
			}
		}

		private void tmrAutoScr_Tick(object sender, EventArgs e)
		{
			int old = vscrMain.Value;
			if (autoScrSpeed < 0) {
				vscrMain.Value = Math.Max(vscrMain.Value + autoScrSpeed, vscrMain.Minimum);
			} else {
				vscrMain.Value = Math.Max(0, Math.Min(vscrMain.Value + autoScrSpeed, vscrMain.Maximum - (vscrMain.LargeChange << 1) + 2));
			}
			ScrollToPosition(vscrMain.Value << 4, false);
			if (bHexFocused) {
				SelectHexTo(lastx, lasty);
			} else {
				SelectTextTo(lastx, lasty);
			}
		}
		#endregion
		#region �L�[����n���\�b�h�E�C�x���g�n���h��

		//�ړ���̃X�N���[������͈ʒu���ړ���ł��邱�Ƃɒ���
		//TODO:���d�`��E�����_�����O���ɗ͌��炷�B
		private void BinaryEditor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			selectionStartBk = selectionStart;
			selectionLengthBk = selectionLength;

			//Control�EAlt�E�t�@���N�V�����L�[�͏������Ȃ�
			if (e.Control || e.Alt || (Keys.F1 <= e.KeyCode && e.KeyCode <= Keys.F24)) {
				e.IsInputKey = false;
				return;
			}

			e.IsInputKey = true;

			switch (e.KeyCode) {
			case Keys.Up:
				bLesserBits = false;
				if (!e.Shift) {
					SelectionLength = 0;
					if (selectionStart > 0x0F) {
						SetSelectionStart(selectionStart - 0x10);
						if (selectionStart - viewStart >> 4 == -1 && viewStart > 0x0F) {
							vscrMain.Value--;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				} else {
					long se = selectionStart + selectionLength + ((selectionLength > 0) ? -1 : 1);
					if (se > 0x0F) {
						SelectionLength -= 0x10;
						se -= 0x10;
						if (se - viewStart >> 4 == -1 && viewStart > 0x0F) {
							vscrMain.Value--;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				}
				break;
			case Keys.Down:
				bLesserBits = false;
				if (!e.Shift) {
					SelectionLength = 0;
					if (selectionStart < binaryBuf.Length - 0x0F) {
						SetSelectionStart(selectionStart + 0x10);
						if ((selectionStart - viewStart >> 4) + 1 == linecnt && viewStart < binaryBuf.Length - 0x0F) {
							vscrMain.Value++;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				} else {
					long se = selectionStart + selectionLength + ((selectionLength > 0) ? -1 : 1);
					if (se < binaryBuf.Length - 0x10) {
						SelectionLength += 0x10;
						se += 0x10;
						if ((se - viewStart >> 4) + 1 == linecnt && viewStart < binaryBuf.Length - 0x0F) {
							vscrMain.Value++;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				}
				break;
			case Keys.Left:
				if (txtTextInput.Focused && e.Control) {
					break;
				}
				bLesserBits = false;
				if (!e.Shift) {
					SelectionLength = 0;
					if (selectionStart > 0) {
						SetSelectionStart(selectionStart - 1);
						if (((selectionStart - viewStart + 1) >> 4) == 0 && selectionStart != 0 && selectionStart % 0x10 == 15) {
							vscrMain.Value--;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				} else {
					long se = selectionStart + selectionLength + ((selectionLength > 0) ? -1 : 1);
					if (se > 0) {
						if (binaryBuf.Length != 0) {
							SelectionLength--;
							se--;
						} else {
							SelectionLength = 0;
							se = 0;
						}
						if (((se - viewStart + 1) >> 4) == 0 && se != 0 && se % 0x10 == 15) {
							vscrMain.Value--;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				}
				break;
			case Keys.Right:
				if (txtTextInput.Focused && e.Control) {
					break;
				}
				bLesserBits = false;
				if (!e.Shift) {
					SelectionLength = 0;
					if (selectionStart < binaryBuf.Length) {
						SetSelectionStart(selectionStart + 1);
						if (((selectionStart - viewStart) >> 4) + 1 == linecnt && viewStart < binaryBuf.Length - 0x0F && selectionStart % 0x10 == 0) {
							vscrMain.Value++;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				} else {
					long se = selectionStart + selectionLength + ((selectionLength > 0) ? -1 : 1);
					if (se < binaryBuf.Length - 1) {
						if (binaryBuf.Length != 0) {
							SelectionLength++;
							se++;
						} else {
							SelectionLength = 0;
							se = 0;
						}
						if (((se - viewStart) >> 4) + 1 == linecnt && viewStart < binaryBuf.Length - 0x0F && se % 0x10 == 0) {
							vscrMain.Value++;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							drwHexView.Refresh();
							drwTextView.Refresh();
						}
					}
				}
				break;
			case Keys.PageUp:
				vscrMain.Value = Math.Max(vscrMain.Value - (linecnt - 1), 0);
				SetSelectionStart(Math.Max(selectionStart - ((linecnt - 1) << 4), 0));
				ScrollToPosition(vscrMain.Value << 4);
				break;
			case Keys.PageDown:
				vscrMain.Value = (int)Math.Min(vscrMain.Value + (linecnt - 1), binaryBuf.Length >> 4);
				SetSelectionStart(Math.Min(selectionStart + ((linecnt - 1) << 4), ((binaryBuf.Length >> 4) << 4)));
				ScrollToPosition(vscrMain.Value << 4);
				break;
			case Keys.Back:
				if (bReadOnly) {
					break;
				}
				if (txtTextInput.Focused) {
					break;
				}
				bLesserBits = false;
				if (selectionLength == 0) {
					if (selectionStart > 0) {
						binaryBuf.Remove(selectionStart - 1);
						SetSelectionStart(selectionStart - 1);
						if (((selectionStart - viewStart + 1) >> 4) == 0 && selectionStart != 0 && selectionStart % 0x10 == 15) {
							vscrMain.Value--;
							ScrollToPosition(vscrMain.Value << 4);
						} else {
							RenderAllViewes();
							RefreshAllViewes();
						}
					}
					Modified = true;
				} else {
					goto case Keys.Delete;
				}
				break;
			case Keys.Delete:
				if (bReadOnly) {
					break;
				}
				if (txtTextInput.Focused) {
					break;
				}
				bLesserBits = false;
				if (selectionLength == 0) {
					if (selectionStart < binaryBuf.Length) {
						binaryBuf.Remove(selectionStart);
						Modified = true;
					}
				} else {
					RemoveSelected();
					ScrollToCursor();
					Modified = true;
				}
				RenderAllViewes();
				RefreshAllViewes();
				break;
			case Keys.Insert:
				bLesserBits = false;
				InsertMode = !bInsertMode;
				if (bHexFocused) {
					drwHexView.Refresh();
				} else {
					drwTextView.Refresh();
				}
				break;
			case Keys.Tab:
				if (drwHexView.Focused) {
					if (!e.Shift) {
						drwTextView.Select();
					} else {
						if (beforeControl != null) {
							beforeControl.Select();
						}
					}
				} else if (drwTextView.Focused) {
					if (!e.Shift) {
						txtTextInput.Select();
					} else {
						drwHexView.Select();
					}
				} else {
					e.IsInputKey = false;
					break;
				}
				drwHexView.Refresh();
				drwTextView.Refresh();
				break;
			case Keys.Enter:
				if (bReadOnly) {
					return;
				}
				if (!txtTextInput.Focused || txtTextInput.TextLength == 0) {
					return;
				}
				if (selectionLength > 0) {
					RemoveSelected();
					if (!bInsertMode) {
						binaryBuf.Insert(0, selectionStart);
					}
				}
				byte[] data = enc.GetBytes(txtTextInput.Text);
				txtTextInput.Text = "";
				if (!bInsertMode) {
					binaryBuf.OverWriteRange(data, selectionStart, data.Length);
				} else {
					binaryBuf.InsertRange(data, selectionStart, data.Length);
				}
				SelectionStart += data.Length;
				RenderAllViewes();
				RefreshAllViewes();

				Modified = true;
				break;
			}
		}

		//�}�����[�h���㏑�����[�h���H
		//��ʃr�b�g�����ʃr�b�g��
		//HEX��TEXT�̂ǂ���Ƀt�H�[�J�X�����邩�H
		private void drwHexView_KeyDown(object sender, KeyEventArgs e)
		{
			if (bReadOnly) {
				return;
			}
			if (e.Control || e.Alt) {
				e.SuppressKeyPress = false;
				return;
			}

			byte hc;
			switch (e.KeyCode) {
			case Keys.D0:
			case Keys.D1:
			case Keys.D2:
			case Keys.D3:
			case Keys.D4:
			case Keys.D5:
			case Keys.D6:
			case Keys.D7:
			case Keys.D8:
			case Keys.D9:
				hc = (byte)(0x00 + (e.KeyCode - Keys.D0));
				break;
			case Keys.NumPad0:
			case Keys.NumPad1:
			case Keys.NumPad2:
			case Keys.NumPad3:
			case Keys.NumPad4:
			case Keys.NumPad5:
			case Keys.NumPad6:
			case Keys.NumPad7:
			case Keys.NumPad8:
			case Keys.NumPad9:
				hc = (byte)(0x00 + (e.KeyCode - Keys.NumPad0));
				break;
			case Keys.A:
			case Keys.B:
			case Keys.C:
			case Keys.D:
			case Keys.E:
			case Keys.F:
				hc = (byte)(0x0A + (e.KeyCode - Keys.A));
				break;
			default:
				return;
			}
			if (selectionLength > 0) {
				RemoveSelected();
				if (!bInsertMode) {
					binaryBuf.Insert(0, selectionStart);
				}
			}
			if (!bInsertMode) {
				if (!bLesserBits) {
					binaryBuf.OverWrite((byte)(hc << 4), SelectionStart);
					bLesserBits = true;
				} else {
					binaryBuf.OverWrite((byte)(hc | (binaryBuf.GetByteAt(selectionStart) & 0xF0)), SelectionStart);
					bLesserBits = false;
					SetSelectionStart(selectionStart + 1);
				}
			} else {
				if (!bLesserBits) {
					binaryBuf.Insert((byte)(hc << 4), selectionStart);
					bLesserBits = true;
				} else {
					binaryBuf.OverWrite((byte)(hc | (binaryBuf.GetByteAt(selectionStart) & 0xF0)), SelectionStart);
					bLesserBits = false;
					SetSelectionStart(selectionStart + 1);
				}
			}
			RenderAllViewes();
			RefreshAllViewes();
			Modified = true;
		}

		private void txtTextInput_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode) {
			case Keys.Up:
			case Keys.Down:
			case Keys.Enter:
				e.SuppressKeyPress = true;
				break;
			case Keys.Left:
			case Keys.Right:
				if (!e.Control) {
					e.SuppressKeyPress = true;
				}
				break;
			}
		}

		private void ScrollToCursor()
		{
			if (((selectionStart >> 4) - (viewStart >> 4)) < 0) {
				vscrMain.Value = (int)(selectionStart >> 4);
				viewStart = (selectionStart >> 4) << 4;
			}
		}
		#endregion
		#region �ҏW����n���\�b�h�E�C�x���g�n���h��
		/// <summary>
		/// �I��͈͂��N���b�v�{�[�h�ɐ؂���܂��B
		/// </summary>
		/// <remarks>
		/// <para><see cref="SelectionStart"/>����<see cref="SelectionLength"/>�͈̔͂��N���b�v�{�[�h�ɐ؂���܂��B</para>
		/// <para>
		/// �f�[�^�̓o�C�i���AANSI�e�L�X�g�AUnicode�e�L�X�g��3�`���ŃN���b�v�{�[�h�ɔz�u����A
		/// ���ꂼ��I��͈�1Byte������A1Byte�A3Byte�A6Byte�ō��v10Byte�̃������̈悪�����܂��B
		/// </para>
		/// </remarks>
		public void Cut()
		{
			if (bReadOnly) {
				return;
			}
			if (selectionLength != 0) {
				CopySelected();
				RemoveSelected();
				SetDispBuf();
				AjustScrollBar();
				RenderLineNo();
				RenderHexView();
				RenderTextView();
				drwLineNo.Refresh();
				drwHexView.Refresh();
				drwTextView.Refresh();
			}
			Modified = true;
		}

		/// <summary>
		/// �I��͈͂��N���b�v�{�[�h�ɃR�s�[���܂��B
		/// </summary>
		/// <remarks>
		/// <para><see cref="SelectionStart"/>����<see cref="SelectionLength"/>�͈̔͂��N���b�v�{�[�h�ɃR�s�[���܂��B</para>
		/// <para>
		/// �f�[�^�̓o�C�i���AANSI�e�L�X�g�AUnicode�e�L�X�g��3�`���ŃN���b�v�{�[�h�ɔz�u����A
		/// ���ꂼ��I��͈�1Byte������A1Byte�A3Byte�A6Byte�ō��v10Byte�̃������̈悪�����܂��B
		/// </para>
		/// </remarks>
		public void Copy()
		{
			if (selectionLength != 0) {
				CopySelected();
			}
		}

		/// <summary>
		/// �I��͈͂��폜���A�N���b�v�{�[�h�̃f�[�^��I���ʒu�ɑ}�����܂��B
		/// </summary>
		/// <remarks>
		/// <para>
		/// �N���b�v�{�[�h�ɂ���o�C�i���f�[�^��I���ʒu�ɑ}�����܂��B
		/// �͈͑I������Ă���ꍇ�폜���Ă���}������܂��B
		/// </para>
		/// <para>
		/// �\��t���\�ȃf�[�^��BinaryEditor.PureBinary�`����byte�z��ł��B
		/// </para>
		/// </remarks>
		/// <returns>�\��t���ɐ��������True�A���s�����False</returns>
		public bool Paste()
		{
			if (bReadOnly) {
				return false;
			}
			byte[] data = (byte[])Clipboard.GetData(DF_PUREBINARY);
			if (data != null) {
				if (selectionLength > 0) {
					RemoveSelected();
				}
				binaryBuf.InsertRange(data, selectionStart, data.Length);
				SelectionStart += data.Length;
				SetDispBuf();
				AjustScrollBar();
				RenderLineNo();
				RenderHexView();
				RenderTextView();
				drwLineNo.Refresh();
				drwHexView.Refresh();
				drwTextView.Refresh();
				Modified = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// �I��͈͂��폜���܂��B
		/// </summary>
		public void Delete()
		{
			if (bReadOnly) {
				return;
			}
			RemoveSelected();
			SetDispBuf();
			AjustScrollBar();
			RenderLineNo();
			RenderHexView();
			RenderTextView();
			drwLineNo.Refresh();
			drwHexView.Refresh();
			drwTextView.Refresh();
			Modified = true;
		}/// <summary>
		/// ���݂̃f�[�^���w�肳�ꂽ�t�@�C���ɏ������݂܂��B
		/// </summary>
		/// <param name="file">�������ݐ�̃t�@�C��</param>
		public void Save(string file)
		{
			FileStream fs = File.OpenWrite(file);
			binaryBuf.Save(fs);
		}

		/// <summary>
		/// ���݂̃f�[�^���w�肳�ꂽ�X�g���[���ɏ������݃X�g���[������܂��B
		/// </summary>
		/// <param name="s">�������ݐ�̃X�g���[��</param>
		public void Save(Stream s)
		{
			binaryBuf.Save(s);
		}

		/// <summary>
		/// �w�肳�ꂽ�o�C�i�����������܂��B
		/// </summary>
		/// <param name="target">��������o�C�i��</param>
		/// <param name="orientation">��������</param>
		/// <returns>�ŏ��Ɍ��������ʒu</returns>
		public long Find(byte[] target, bool orientation)
		{
			if (orientation && selectionStart >= BufferLength - 1 || !orientation && selectionStart < 1) {
				return -1;
			}
			return binaryBuf.FindBinary(target, selectionStart + (orientation ? 1 : -1), orientation);
		}

		delegate long FindDelegate(byte[] target, bool orientation);
		/// <summary>
		/// �w�肳�ꂽ�o�C�i����񓯊��Ō������܂��B
		/// </summary>
		/// <param name="target">��������o�C�i��</param>
		/// <param name="orientation">��������</param>
		/// <remarks>
		/// <para>
		/// �񓯊��Ńo�C�i���������s���܂��B
		/// ���������������<see cref="FindCompleted"/>�C�x���g���������܂��B
		/// �񓯊��������ɌĂяo����邩<see cref="FindCompleted"/>�C�x���g���ݒ肳��Ă��Ȃ���<see cref="System.InvalidOperationException"/>��O���X���[����܂��B
		/// </para>
		/// </remarks>
		/// <exception cref="System.InvalidOperationException">
		/// �񓯊����������łɊJ�n����Ă��邩�A<see cref="FindCompleted"/>�C�x���g���ݒ肳��ĂȂ��ꍇ�ɃX���[����܂��B
		/// </exception>
		public void BeginFind(byte[] target, bool orientation)
		{
			if (finding) {
				throw new InvalidOperationException("�񓯊������͂��łɊJ�n����Ă��܂��B");
			}
			if (FindCompleted == null) {
				throw new InvalidOperationException("�ʒm�C�x���g���ݒ肳��Ă��܂���B");
			}
			IAsyncResult ar = BeginInvoke(new FindDelegate(Find), target, orientation, null);
			finding = true;
		}

		private void FindingCallback(IAsyncResult ar)
		{
			finding = false;
			long pos = (long)EndInvoke(ar);
			if (FindCompleted != null) {
				FindCompleted(this, new FindEventArgs(pos));
			}
		}

		/// <summary>
		/// �o�C�i���̔񓯊������𒆎~���܂��B
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="BeginFind"/>�ŊJ�n���ꂽ�񓯊������𒆎~���܂��B
		/// �񓯊��������J�n����Ă��Ȃ����ɌĂяo������<see cref="FindCompleted"/>���X���[����܂��B
		/// </para>
		/// </remarks>
		/// <exception cref="System.InvalidOperationException">
		/// �񓯊��������J�n����Ă��Ȃ��ꍇ�ɃX���[����܂��B
		/// </exception>
		public void CancelFind()
		{
			if (!finding) {
				throw new InvalidOperationException("�񓯊��������J�n����Ă��܂���B");
			}
			binaryBuf.CancelFinding();
		}

		/// <summary>
		/// ���O�̑�����������A�I��͈͂𕜌����܂��B
		/// </summary>
		public void Undo()
		{
			binaryBuf.Undo();
			RenderAllViewes();
			RefreshAllViewes();
		}

		private void cmenuCut_Click(object sender, EventArgs e)
		{
			Cut();
		}

		private void cmenuCopy_Click(object sender, EventArgs e)
		{
			Copy();
		}

		private void cmenuPaste_Click(object sender, EventArgs e)
		{
			Paste();
		}

		private void cmenuDelete_Click(object sender, EventArgs e)
		{
			Delete();
		}

		private void CopySelected()
		{
			long bgn;
			if (selectionLength > 0) {
				bgn = selectionStart;
			} else {
				bgn = selectionStart + selectionLength + 1;
			}
			byte[] sbin = binaryBuf.GetBytesAt(bgn, (int)Math.Abs(selectionLength));
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < sbin.Length; i += 0x10) {
				sb.AppendLine(BitConverter.ToString(sbin, i, Math.Min(sbin.Length - i, 0x10)).Replace('-', ' '));
			}
			DataObject dobj = new DataObject();
			dobj.SetData(DF_PUREBINARY, sbin);
			dobj.SetText(sb.ToString());
			Clipboard.SetDataObject(dobj, true);
		}

		private void RemoveSelected()
		{
			long bgn;
			if (selectionLength > 0) {
				bgn = selectionStart;
			} else {
				bgn = selectionStart + selectionLength + 1;
			}
			binaryBuf.RemoveRange(bgn, Math.Abs(selectionLength));
			if (((bgn - viewStart) >> 4) < 0) {
				vscrMain.Value = (int)(viewStart >> 4);
			}
			SelectionStart = Math.Min(binaryBuf.Length, bgn);
			SelectionLength = 0;
		}



		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			cmenuUndo.Enabled = CanUndo;
			cmenuRedo.Enabled = false;
			cmenuCut.Enabled = !bReadOnly;
			cmenuPaste.Enabled = CanPaste;
			cmenuDelete.Enabled = !bReadOnly;
		}
		#endregion

		private void drwAll_Click(object sender, EventArgs e)
		{
			drwHexView.Select();
		}

		private void cmenuUndo_Click(object sender, EventArgs e)
		{
			Undo();
		}
	}
}