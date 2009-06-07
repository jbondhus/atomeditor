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
	/// バイナリデータを16進表示およびShift JIS文字列として表示で表示および編集できるコントロールを表示します。
	/// </summary>
	[ToolboxBitmap(typeof(System.Drawing.Bitmap), "BinaryEditor.bmp")]
	public partial class BinaryEditor : UserControl
	{
		#region 内部定数
		/// <summary>
		/// バイナリデータのデータフォーマット
		/// </summary>
		const string DF_PUREBINARY = "BinaryEditor.PureBinary";
		#endregion
		#region 内部変数
		/// <summary>
		/// カーソル表示用のColorMap
		/// </summary>
		ColorMap[]
			cmHexView = new ColorMap[] { new ColorMap(), new ColorMap() },
			cmTextView = new ColorMap[] { new ColorMap(), new ColorMap() },
			cmReadOnly = new ColorMap[] { new ColorMap(), new ColorMap() };

		/// <summary>
		/// カーソル表示用のImageAttribute
		/// </summary>
		ImageAttributes
			iaHexView = new ImageAttributes(),
			iaTextView = new ImageAttributes(),
			iaReadOnly = new ImageAttributes();

		/// <summary>
		/// 16進表示にフォーカスがあればTrue、文字列表示ならばFalse
		/// </summary>
		bool bHexFocused
		{
			get { return drwHexView.Focused; }
		}

		/// <summary>
		/// 下位4ビットを編集中ならばTrue、さもなければFalse
		/// </summary>
		bool bLesserBits;

		/// <summary>
		/// OnLoad実行後ならばTrue、さもなければFalse
		/// </summary>
		bool bLoaded;

		/// <summary>
		/// 文字の幅
		/// </summary>
		int charWidth;

		/// <summary>
		/// 文字の高さ
		/// </summary>
		int charHeight;

		/// <summary>
		/// 表示を行うべき行数
		/// </summary>
		int linecnt;

		/// <summary>
		/// 自動スクロールでスクロールされる行数
		/// </summary>
		int autoScrSpeed;

		/// <summary>
		/// 自動スクロール時に参照されるマウス座標
		/// </summary>
		int lastx, lasty;

		/// <summary>
		/// 表示範囲内のバイト配列
		/// </summary>
		byte[] dispbuf = new byte[4000];

		long selectionStartBk;
		long selectionLengthBk;
		#endregion
		#region イベント
		/// <summary>
		/// バイナリデータの編集状態が変更されたときに発生します。
		/// </summary>
		[Browsable(true)]
		[Category("動作")]
		[Description("バイナリデータの編集状態が変更されたときに発生します。")]
		public event EventHandler ModifiedChanged;

		/// <summary>
		/// 編集モードが変更されたときに発生します。
		/// </summary>
		[Browsable(true)]
		[Category("プロパティ変更")]
		[Description("編集モードが変更されたときに発生します。")]
		public event EventHandler InsertModeChanged;

		/// <summary>
		/// ReadOnlyプロパティが変更されたときに発生します。
		/// </summary>
		[Browsable(true)]
		[Category("プロパティ変更")]
		public event EventHandler ReadOnlyChanged;

		/// <summary>
		/// 選択開始位置が変更されたときに発生します。
		/// </summary>
		[Browsable(true)]
		[Category("動作")]
		public event EventHandler SelectionStartChanged;

		/// <summary>
		/// 選択長が変更されたときに発生します。
		/// </summary>
		[Browsable(true)]
		[Category("動作")]
		public event EventHandler SelectionLengthChanged;

		/// <summary>
		/// <see cref="BeginFind"/>メソッドが完了したときに発生します。
		/// </summary>
		[Browsable(true)]
		[Category("動作")]
		public event EventHandler<FindEventArgs> FindCompleted;
		#endregion
		#region オーバーライド
		/// <summary>
		/// コントロールによって表示されるテキストのフォントを取得または設定します。
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
		#region 内容系プロパティ
		BinaryBuffer binaryBuf;

		/// <summary>
		/// 表示および編集対象のバイナリデータが格納されたStreamを取得または設定します。
		/// </summary>
		/// <exception cref="System.ArgumentException">
		/// 読み込みまたはシークができないストリームを設定するとスローされます。
		/// </exception>
		[Browsable(false)]
		[ReadOnly(true)]
		public Stream BinaryStream
		{
			get { return binaryBuf.Stream; }
			set
			{
				if (!(value.CanRead && value.CanSeek)) {
					throw new ArgumentException("読み込みおよびシークが可能なストリームのみ指定できます。");
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
		/// 編集中のバイナリデータを取得します。
		/// </summary>
		public byte[] BinaryData
		{
			get { return binaryBuf.GetBytesAt(0, binaryBuf.Length); }
		}

		#endregion
		#region 動作系プロパティ
		/// <summary>
		/// 【取得専用】エディタが読み取り専用ならばTrue、編集可能ならばFalse。
		/// </summary>
		bool bReadOnly = false;

		/// <summary>
		/// エディタのバイナリデータを変更できるかどうかを取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("動作")]
		[DefaultValue(false)]
		[Description("エディタのバイナリデータを変更できるかどうかを設定します。")]
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
		/// 表示開始位置のアドレス
		/// </summary>
		long viewStart = 0;

		/// <summary>
		/// 【取得専用】選択開始位置のアドレス
		/// </summary>
		long selectionStart = 0;

		/// <summary>
		/// 選択開始位置のアドレスを取得または設定します。
		/// </summary>
		[Browsable(false)]
		public long SelectionStart
		{
			get { return selectionStart; }
			set
			{
				SetSelectionStart(value);
				//外部からの呼び出しを考慮すると再描画せざるを得ない。
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
		/// 【取得専用】選択範囲の長さおよび方向
		/// </summary>
		long selectionLength;

		/// <summary>
		/// 選択範囲の長さおよび方向を取得または設定します。
		/// マイナスの値が設定された場合後方に向かって選択されます。
		/// 0で選択なし、±1で一文字選択となります。
		/// </summary>
		//TODO:範囲制限が半端
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
		/// バイナリデータの全長を取得します。
		/// </summary>
		[Browsable(false)]
		public long BufferLength
		{
			get { return binaryBuf.Length; }
		}

		/// <summary>
		/// 【取得専用】バイナリデータが最後の読み込みまたは保存後に変更されているならばTrue、さもなければFalse。
		/// </summary>
		bool bModified;

		/// <summary>
		/// バイナリデータが最後の読み込みまたは保存後に変更されているかどうかを取得します。
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
		/// 直前のタブオーダーを持つコントロール
		/// </summary>
		Control beforeControl;

		/// <summary>
		/// 直前のタブオーダーを持つコントロールを取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("動作")]
		[Description("直前のタブオーダーを持つコントロールを設定します。")]
		public Control BeforeControl
		{
			get { return beforeControl; }
			set { beforeControl = value; }
		}

		/// <summary>
		/// 【取得専用】編集モードが挿入モードならTrue、上書きモードならばFalse
		/// </summary>
		bool bInsertMode;

		/// <summary>
		/// 編集モードを取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("動作")]
		[Description("編集モードを設定します。")]
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
		/// 貼り付け操作を実行可能かどうかを取得します。
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		public bool CanPaste
		{
			get { return !bReadOnly && Clipboard.ContainsData(DF_PUREBINARY); }
		}

		/// <summary>
		/// アンドゥ操作が実行可能かどうかを取得します。
		/// </summary>
		public bool CanUndo
		{
			get { return binaryBuf.CanUndo; }
		}

		bool finding;

		/// <summary>
		/// 非同期検索が行われているかどうかを取得します。
		/// </summary>
		public bool Finding
		{
			get { return finding; }
		}
		#endregion
		#region 表示系プロパティ
		/// <summary>
		/// ヘッダの前景色を設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("ヘッダの前景色を取得または設定します。")]
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
		/// ヘッダの背景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("ヘッダの背景色を設定します。")]
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
		/// 行番号の前景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("行番号の前景色を設定します。")]
		[DefaultValue(typeof(Color), "White")]
		public Color LineNoForeColor
		{
			get { return drwLineNo.ForeColor; }
			set { drwLineNo.ForeColor = value; }
		}

		/// <summary>
		/// 行番号の背景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("行番号の背景色を設定します。")]
		[DefaultValue(typeof(Color), "Gray")]
		public Color LineNoBackgrountColor
		{
			get { return drwLineNo.BackColor; }
			set { drwLineNo.BackColor = value; }
		}

		/// <summary>
		/// 16進表示の前景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("16進表示の前景色を設定します。")]
		[DefaultValue(typeof(Color), "Black")]
		public Color HexViewForeColor
		{
			get { return drwHexView.ForeColor; }
			set { drwHexView.ForeColor = value; }
		}

		/// <summary>
		/// 16進表示の背景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("16進表示の背景色を設定します。")]
		[DefaultValue(typeof(Color), "White")]
		public Color HexViewBackColor
		{
			get { return drwHexView.BackColor; }
			set { drwHexView.BackColor = value; }
		}

		/// <summary>
		/// 文字列表示の前景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("文字列表示の前景色を設定します。")]
		[DefaultValue(typeof(Color), "Black")]
		public Color TextViewForeColor
		{
			get { return drwTextView.ForeColor; }
			set { drwTextView.ForeColor = value; }
		}

		/// <summary>
		/// 文字列表示の背景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("文字列表示の背景色を設定します。")]
		[DefaultValue(typeof(Color), "White")]
		public Color TextViewBackColor
		{
			get { return drwTextView.BackColor; }
			set { drwTextView.BackColor = value; }
		}

		Color colorSelectionF = SystemColors.HighlightText;
		/// <summary>
		/// 選択範囲の前景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("選択範囲の前景色を設定します。")]
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
		/// 選択範囲の背景色を取得または設定します。
		/// </summary>
		[Browsable(true)]
		[Category("表示")]
		[Description("選択範囲の背景色を設定します。")]
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
		#region 初期化系メソッド・イベントハンドラ
		/// <summary>
		/// 既定のバイナリを表示するバイナリエディタコントロールを初期化します。
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
		/// 主要変数設定後の初期化・再描画処理を行います。
		/// </summary>
		/// <remarks>
		/// このイベントが呼び出される以前のプロパティ設定時の描画処理は抑止されなければならない。
		/// ただしデザインモード時は除く。
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
		#region 調整系メソッド・イベントハンドラ

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
		/// スクロールバーを調整します。
		/// </summary>
		private void AjustScrollBar()
		{
			vscrMain.Maximum = (int)(binaryBuf.Length >> 4) + linecnt - ((binaryBuf.Length % 16 == 0) ? 3 : 2);
			vscrMain.LargeChange = Math.Max(0, linecnt - 1);
		}

		/// <summary>
		/// フォントサイズ変更時のサイズ調整と再描画を行います。
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
		/// 表示領域のbyte配列を更新します。
		/// </summary>
		private void SetDispBuf()
		{
			int displen = Math.Min(16 * linecnt, (int)(binaryBuf.Length - viewStart));
			dispbuf = binaryBuf.GetBytesAt(viewStart, displen);
		}

		/// <summary>
		/// アンドゥ後に選択範囲を復帰します。
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
		#region セカンダリサーフェス書き換えメソッド
		/// <summary>
		/// ヘッダのセカンダリサーフェスを書き換えます。
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
		/// 行番号のセカンダリサーフェスを書き換えます。
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
		/// 16進表示のセカンダリサーフェスを書き換えます。
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
		/// テキスト表示のセカンダリサーフェスを書き換えます。
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
		#region Paintイベントハンドラ
		/// <summary>
		/// 16進表示のカーソルまたは選択を描画します。
		/// </summary>
		private void drwHexView_Paint(object sender, PaintEventArgs e)
		{
			if (!Visible) {
				return;
			}
			Bitmap bmp = drwHexView.Surface;
			//カレットの表示
			long len = binaryBuf.Length;
			int displen = Math.Min(linecnt << 4, (int)(len - viewStart));
			int disppos = (int)(selectionStart - viewStart);
			if (binaryBuf.Length >= 0) {
				if (selectionLength == 0 && viewStart <= selectionStart && selectionStart < viewStart + len + 1) {
					//選択なし
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
					//選択あり
					long bgn = 0, end = 0;
					if (selectionLength > 0) {
						bgn = selectionStart;
						end = selectionStart + selectionLength - 1;
					} else {
						bgn = selectionStart + selectionLength + 1;
						end = selectionStart;
					}

					//表示範囲内のみ描画
					if (end >= viewStart && viewStart + displen >= bgn) {
						bgn = Math.Max(bgn - viewStart, 0);
						end = Math.Min(end - viewStart, displen - 1);
					} else {
						return;
					}

					//行内
					if ((end >> 4) == (bgn >> 4)) {
						Point p = new Point((int)((bgn % 16) * charWidth * 3 + charWidth), (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, (int)(((end % 16 + 1) * charWidth * 3) - p.X), charHeight);
						r.Offset(1, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaHexView : iaReadOnly);
						return;
					}

					//開始行
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

					//終了行
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

					//中間行
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
		/// 文字列表示のセカンダリサーフェスを描画します。
		/// </summary>
		private void drwTextView_Paint(object sender, PaintEventArgs e)
		{
			if (!Visible) {
				return;
			}
			Bitmap bmp = drwTextView.Surface;

			//カレットの表示
			long len = binaryBuf.Length;
			int displen = Math.Min(linecnt << 4, (int)(len - viewStart));
			int disppos = (int)(selectionStart - viewStart);
			if (binaryBuf.Length >= 0) {
				if (selectionLength == 0 && viewStart <= selectionStart && selectionStart < viewStart + len + 1) {
					//選択なし
					Rectangle r = drwTextView.Focused || txtTextInput.Focused ?
						!bInsertMode ?
							new Rectangle((disppos % 16 + 1) * charWidth, (disppos >> 4) * charHeight, charWidth, charHeight) :
							new Rectangle((disppos % 16 + 1) * charWidth, (disppos >> 4) * charHeight, 2, charHeight) :
						new Rectangle((disppos % 16 + 1) * charWidth, ((disppos >> 4) + 1) * charHeight - 2, charWidth, 2);
					r.Offset(1, 0);
					e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaTextView : iaReadOnly);
				} else {
					//選択あり
					long bgn = 0, end = 0;
					if (selectionLength > 0) {
						bgn = selectionStart;
						end = selectionStart + selectionLength - 1;
					} else {
						bgn = selectionStart + selectionLength + 1;
						end = selectionStart;
					}

					//選択範囲内のみ描画
					if (end >= viewStart || viewStart + displen >= bgn) {
						bgn = Math.Max(bgn - viewStart, 0);
						end = Math.Min(end - viewStart, displen - 1);
					} else {
						return;
					}

					//行内
					if ((end >> 4) == (bgn >> 4)) {
						Point p = new Point((int)((bgn % 16) * charWidth + charWidth), (int)((bgn >> 4) * charHeight));
						Rectangle r = new Rectangle(p.X, p.Y, (int)(((end % 16 + 2) * charWidth) - p.X), charHeight);
						r.Offset(1, 0);
						e.Graphics.DrawImage(bmp, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, !bReadOnly ? iaTextView : iaReadOnly);
						return;
					}

					//開始行
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

					//最終行
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

					//中間行
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
		#region マウス操作系メソッド・イベントハンドラ
		private void vscrMain_Scroll(object sender, ScrollEventArgs e)
		{
			//多重描画防止
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
				//上方向
				vscrMain.Value = Math.Max(vscrMain.Value - e.Delta / 20, vscrMain.Minimum);
			} else {
				//下方向
				vscrMain.Value = Math.Min(vscrMain.Value - e.Delta / 20, vscrMain.Maximum - vscrMain.LargeChange + 1);
			}
			ScrollToPosition(vscrMain.Value << 4);
		}

		private void drwHexView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift) {
					//選択開始位置
					bLesserBits = false;
					SelectionStart = HexPointToPos(e.X, e.Y, false);
					SelectionLength = 0;
					drwHexView.Refresh();
					drwTextView.Refresh();
				} else {
					//選択終了位置
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
					//選択開始位置
					bLesserBits = false;
					SelectionStart = TextPointToPos(e.X, e.Y, false);
					SelectionLength = 0;
					drwHexView.Refresh();
					drwTextView.Refresh();
				} else {
					//選択終了位置
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
		#region キー操作系メソッド・イベントハンドラ

		//移動後のスクロール判定は位置が移動後であることに注意
		//TODO:多重描画・レンダリングを極力減らす。
		private void BinaryEditor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			selectionStartBk = selectionStart;
			selectionLengthBk = selectionLength;

			//Control・Alt・ファンクションキーは処理しない
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

		//挿入モードか上書きモードか？
		//上位ビットか下位ビットか
		//HEXとTEXTのどちらにフォーカスがあるか？
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
		#region 編集操作系メソッド・イベントハンドラ
		/// <summary>
		/// 選択範囲をクリップボードに切り取ります。
		/// </summary>
		/// <remarks>
		/// <para><see cref="SelectionStart"/>から<see cref="SelectionLength"/>の範囲をクリップボードに切り取ります。</para>
		/// <para>
		/// データはバイナリ、ANSIテキスト、Unicodeテキストの3形式でクリップボードに配置され、
		/// それぞれ選択範囲1Byteあたり、1Byte、3Byte、6Byteで合計10Byteのメモリ領域が消費されます。
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
		/// 選択範囲をクリップボードにコピーします。
		/// </summary>
		/// <remarks>
		/// <para><see cref="SelectionStart"/>から<see cref="SelectionLength"/>の範囲をクリップボードにコピーします。</para>
		/// <para>
		/// データはバイナリ、ANSIテキスト、Unicodeテキストの3形式でクリップボードに配置され、
		/// それぞれ選択範囲1Byteあたり、1Byte、3Byte、6Byteで合計10Byteのメモリ領域が消費されます。
		/// </para>
		/// </remarks>
		public void Copy()
		{
			if (selectionLength != 0) {
				CopySelected();
			}
		}

		/// <summary>
		/// 選択範囲を削除し、クリップボードのデータを選択位置に挿入します。
		/// </summary>
		/// <remarks>
		/// <para>
		/// クリップボードにあるバイナリデータを選択位置に挿入します。
		/// 範囲選択されている場合削除してから挿入されます。
		/// </para>
		/// <para>
		/// 貼り付け可能なデータはBinaryEditor.PureBinary形式のbyte配列です。
		/// </para>
		/// </remarks>
		/// <returns>貼り付けに成功すればTrue、失敗すればFalse</returns>
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
		/// 選択範囲を削除します。
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
		/// 現在のデータを指定されたファイルに書き込みます。
		/// </summary>
		/// <param name="file">書き込み先のファイル</param>
		public void Save(string file)
		{
			FileStream fs = File.OpenWrite(file);
			binaryBuf.Save(fs);
		}

		/// <summary>
		/// 現在のデータを指定されたストリームに書き込みストリームを閉じます。
		/// </summary>
		/// <param name="s">書き込み先のストリーム</param>
		public void Save(Stream s)
		{
			binaryBuf.Save(s);
		}

		/// <summary>
		/// 指定されたバイナリを検索します。
		/// </summary>
		/// <param name="target">検索するバイナリ</param>
		/// <param name="orientation">検索方向</param>
		/// <returns>最初に見つかった位置</returns>
		public long Find(byte[] target, bool orientation)
		{
			if (orientation && selectionStart >= BufferLength - 1 || !orientation && selectionStart < 1) {
				return -1;
			}
			return binaryBuf.FindBinary(target, selectionStart + (orientation ? 1 : -1), orientation);
		}

		delegate long FindDelegate(byte[] target, bool orientation);
		/// <summary>
		/// 指定されたバイナリを非同期で検索します。
		/// </summary>
		/// <param name="target">検索するバイナリ</param>
		/// <param name="orientation">検索方向</param>
		/// <remarks>
		/// <para>
		/// 非同期でバイナリ検索を行います。
		/// 検索が完了すると<see cref="FindCompleted"/>イベントが発生します。
		/// 非同期検索中に呼び出されるか<see cref="FindCompleted"/>イベントが設定されていないと<see cref="System.InvalidOperationException"/>例外がスローされます。
		/// </para>
		/// </remarks>
		/// <exception cref="System.InvalidOperationException">
		/// 非同期検索がすでに開始されているか、<see cref="FindCompleted"/>イベントが設定されてない場合にスローされます。
		/// </exception>
		public void BeginFind(byte[] target, bool orientation)
		{
			if (finding) {
				throw new InvalidOperationException("非同期検索はすでに開始されています。");
			}
			if (FindCompleted == null) {
				throw new InvalidOperationException("通知イベントが設定されていません。");
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
		/// バイナリの非同期検索を中止します。
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="BeginFind"/>で開始された非同期検索を中止します。
		/// 非同期検索が開始されていない時に呼び出されると<see cref="FindCompleted"/>がスローされます。
		/// </para>
		/// </remarks>
		/// <exception cref="System.InvalidOperationException">
		/// 非同期検索が開始されていない場合にスローされます。
		/// </exception>
		public void CancelFind()
		{
			if (!finding) {
				throw new InvalidOperationException("非同期検索が開始されていません。");
			}
			binaryBuf.CancelFinding();
		}

		/// <summary>
		/// 直前の操作を取り消し、選択範囲を復元します。
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