namespace Kirishima16.Forms
{
    partial class BinaryEditor
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.vscrMain = new System.Windows.Forms.VScrollBar();
			this.txtTextInput = new System.Windows.Forms.TextBox();
			this.tmrAutoScr = new System.Windows.Forms.Timer(this.components);
			this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmenuUndo = new System.Windows.Forms.ToolStripMenuItem();
			this.cmenuRedo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmenuCut = new System.Windows.Forms.ToolStripMenuItem();
			this.cmenuCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.cmenuPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.cmenuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.drwHeader1 = new Kirishima16.Forms.HeaderDrawer();
			this.drwLineNo = new Kirishima16.Forms.LineNoDrawer();
			this.drwTextView = new Kirishima16.Forms.TextViewDrawer();
			this.drwHexView = new Kirishima16.Forms.HexViewDrawer();
			this.drwHeader2 = new Kirishima16.Forms.HeaderDrawer();
			this.drwHeader3 = new Kirishima16.Forms.HeaderDrawer();
			this.tlpMain.SuspendLayout();
			this.cmsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpMain
			// 
			this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
			this.tlpMain.ColumnCount = 4;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.Controls.Add(this.drwHeader1, 0, 0);
			this.tlpMain.Controls.Add(this.vscrMain, 3, 0);
			this.tlpMain.Controls.Add(this.drwLineNo, 0, 1);
			this.tlpMain.Controls.Add(this.drwTextView, 2, 1);
			this.tlpMain.Controls.Add(this.drwHexView, 1, 1);
			this.tlpMain.Controls.Add(this.drwHeader2, 1, 0);
			this.tlpMain.Controls.Add(this.drwHeader3, 2, 0);
			this.tlpMain.Controls.Add(this.txtTextInput, 0, 2);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 3;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.Size = new System.Drawing.Size(493, 341);
			this.tlpMain.TabIndex = 0;
			// 
			// vscrMain
			// 
			this.vscrMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.vscrMain.LargeChange = 1;
			this.vscrMain.Location = new System.Drawing.Point(475, 2);
			this.vscrMain.Maximum = 0;
			this.vscrMain.Name = "vscrMain";
			this.tlpMain.SetRowSpan(this.vscrMain, 3);
			this.vscrMain.Size = new System.Drawing.Size(16, 337);
			this.vscrMain.TabIndex = 0;
			this.vscrMain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vscrMain_Scroll);
			// 
			// txtTextInput
			// 
			this.txtTextInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTextInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tlpMain.SetColumnSpan(this.txtTextInput, 3);
			this.txtTextInput.Location = new System.Drawing.Point(2, 327);
			this.txtTextInput.Margin = new System.Windows.Forms.Padding(0);
			this.txtTextInput.Name = "txtTextInput";
			this.txtTextInput.Size = new System.Drawing.Size(471, 12);
			this.txtTextInput.TabIndex = 3;
			this.txtTextInput.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BinaryEditor_PreviewKeyDown);
			this.txtTextInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTextInput_KeyDown);
			// 
			// tmrAutoScr
			// 
			this.tmrAutoScr.Interval = 25;
			this.tmrAutoScr.Tick += new System.EventHandler(this.tmrAutoScr_Tick);
			// 
			// cmsMain
			// 
			this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuUndo,
            this.cmenuRedo,
            this.toolStripSeparator1,
            this.cmenuCut,
            this.cmenuCopy,
            this.cmenuPaste,
            this.cmenuDelete});
			this.cmsMain.Name = "contextMenuStrip1";
			this.cmsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.cmsMain.ShowImageMargin = false;
			this.cmsMain.Size = new System.Drawing.Size(142, 142);
			this.cmsMain.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// cmenuUndo
			// 
			this.cmenuUndo.Name = "cmenuUndo";
			this.cmenuUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.cmenuUndo.Size = new System.Drawing.Size(141, 22);
			this.cmenuUndo.Text = "元に戻す(&U)";
			this.cmenuUndo.Click += new System.EventHandler(this.cmenuUndo_Click);
			// 
			// cmenuRedo
			// 
			this.cmenuRedo.Name = "cmenuRedo";
			this.cmenuRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.cmenuRedo.Size = new System.Drawing.Size(141, 22);
			this.cmenuRedo.Text = "やり直し(&R)";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(138, 6);
			// 
			// cmenuCut
			// 
			this.cmenuCut.Name = "cmenuCut";
			this.cmenuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cmenuCut.Size = new System.Drawing.Size(141, 22);
			this.cmenuCut.Text = "切り取り(&X)";
			this.cmenuCut.Click += new System.EventHandler(this.cmenuCut_Click);
			// 
			// cmenuCopy
			// 
			this.cmenuCopy.Name = "cmenuCopy";
			this.cmenuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cmenuCopy.Size = new System.Drawing.Size(141, 22);
			this.cmenuCopy.Text = "コピー(&C)";
			this.cmenuCopy.Click += new System.EventHandler(this.cmenuCopy_Click);
			// 
			// cmenuPaste
			// 
			this.cmenuPaste.Name = "cmenuPaste";
			this.cmenuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.cmenuPaste.Size = new System.Drawing.Size(141, 22);
			this.cmenuPaste.Text = "貼り付け(&P)";
			this.cmenuPaste.Click += new System.EventHandler(this.cmenuPaste_Click);
			// 
			// cmenuDelete
			// 
			this.cmenuDelete.Name = "cmenuDelete";
			this.cmenuDelete.Size = new System.Drawing.Size(141, 22);
			this.cmenuDelete.Text = "削除(&D)";
			this.cmenuDelete.Click += new System.EventHandler(this.cmenuDelete_Click);
			// 
			// drwHeader1
			// 
			this.drwHeader1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.drwHeader1.BackColor = System.Drawing.Color.Yellow;
			this.drwHeader1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.drwHeader1.Location = new System.Drawing.Point(2, 2);
			this.drwHeader1.Margin = new System.Windows.Forms.Padding(0);
			this.drwHeader1.Name = "drwHeader1";
			this.drwHeader1.Size = new System.Drawing.Size(75, 12);
			this.drwHeader1.TabIndex = 0;
			this.drwHeader1.TabStop = false;
			this.drwHeader1.Text = "   ADDRESS+";
			this.drwHeader1.Click += new System.EventHandler(this.drwAll_Click);
			// 
			// drwLineNo
			// 
			this.drwLineNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.drwLineNo.BackColor = System.Drawing.Color.Gray;
			this.drwLineNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.drwLineNo.ForeColor = System.Drawing.Color.White;
			this.drwLineNo.Location = new System.Drawing.Point(2, 16);
			this.drwLineNo.Margin = new System.Windows.Forms.Padding(0);
			this.drwLineNo.Name = "drwLineNo";
			this.drwLineNo.Size = new System.Drawing.Size(75, 309);
			this.drwLineNo.TabIndex = 0;
			this.drwLineNo.TabStop = false;
			this.drwLineNo.Text = "lineNoDrawer1";
			this.drwLineNo.Click += new System.EventHandler(this.drwAll_Click);
			// 
			// drwTextView
			// 
			this.drwTextView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.drwTextView.BackColor = System.Drawing.Color.AliceBlue;
			this.drwTextView.Data = new byte[] {
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0))};
			this.drwTextView.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.drwTextView.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.drwTextView.Location = new System.Drawing.Point(377, 16);
			this.drwTextView.Margin = new System.Windows.Forms.Padding(0);
			this.drwTextView.Name = "drwTextView";
			this.drwTextView.ReadOnly = false;
			this.drwTextView.Size = new System.Drawing.Size(96, 309);
			this.drwTextView.TabIndex = 2;
			this.drwTextView.Text = "textViewDrawer1";
			this.drwTextView.Paint += new System.Windows.Forms.PaintEventHandler(this.drwTextView_Paint);
			this.drwTextView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BinaryEditor_PreviewKeyDown);
			this.drwTextView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drwTextView_MouseMove);
			this.drwTextView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drwTextView_MouseDown);
			this.drwTextView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drwTextView_MouseUp);
			// 
			// drwHexView
			// 
			this.drwHexView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.drwHexView.BackColor = System.Drawing.Color.White;
			this.drwHexView.Data = new byte[] {
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0))};
			this.drwHexView.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.drwHexView.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.drwHexView.Location = new System.Drawing.Point(79, 16);
			this.drwHexView.Margin = new System.Windows.Forms.Padding(0);
			this.drwHexView.Name = "drwHexView";
			this.drwHexView.ReadOnly = false;
			this.drwHexView.Size = new System.Drawing.Size(296, 309);
			this.drwHexView.TabIndex = 1;
			this.drwHexView.Text = "hexViewDrawer1";
			this.drwHexView.Paint += new System.Windows.Forms.PaintEventHandler(this.drwHexView_Paint);
			this.drwHexView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BinaryEditor_PreviewKeyDown);
			this.drwHexView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drwHexView_MouseMove);
			this.drwHexView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drwHexView_MouseDown);
			this.drwHexView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drwHexView_MouseUp);
			this.drwHexView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.drwHexView_KeyDown);
			// 
			// drwHeader2
			// 
			this.drwHeader2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.drwHeader2.BackColor = System.Drawing.Color.Yellow;
			this.drwHeader2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.drwHeader2.Location = new System.Drawing.Point(79, 2);
			this.drwHeader2.Margin = new System.Windows.Forms.Padding(0);
			this.drwHeader2.Name = "drwHeader2";
			this.drwHeader2.Size = new System.Drawing.Size(296, 12);
			this.drwHeader2.TabIndex = 0;
			this.drwHeader2.TabStop = false;
			this.drwHeader2.Text = " 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F";
			this.drwHeader2.Click += new System.EventHandler(this.drwAll_Click);
			// 
			// drwHeader3
			// 
			this.drwHeader3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.drwHeader3.BackColor = System.Drawing.Color.Yellow;
			this.drwHeader3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.drwHeader3.Location = new System.Drawing.Point(377, 2);
			this.drwHeader3.Margin = new System.Windows.Forms.Padding(0);
			this.drwHeader3.Name = "drwHeader3";
			this.drwHeader3.Size = new System.Drawing.Size(96, 12);
			this.drwHeader3.TabIndex = 0;
			this.drwHeader3.TabStop = false;
			this.drwHeader3.Text = " 0123456789ABCDEF";
			this.drwHeader3.Click += new System.EventHandler(this.drwAll_Click);
			// 
			// BinaryEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ContextMenuStrip = this.cmsMain;
			this.Controls.Add(this.tlpMain);
			this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.MinimumSize = new System.Drawing.Size(485, 24);
			this.Name = "BinaryEditor";
			this.Size = new System.Drawing.Size(493, 341);
			this.Load += new System.EventHandler(this.BinaryEditor_Load);
			this.SizeChanged += new System.EventHandler(this.BinaryEditor_SizeChanged);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.cmsMain.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.VScrollBar vscrMain;
		private System.Windows.Forms.Timer tmrAutoScr;
		private System.Windows.Forms.ContextMenuStrip cmsMain;
		private System.Windows.Forms.ToolStripMenuItem cmenuCut;
		private System.Windows.Forms.ToolStripMenuItem cmenuCopy;
		private System.Windows.Forms.ToolStripMenuItem cmenuPaste;
		private System.Windows.Forms.ToolStripMenuItem cmenuUndo;
		private System.Windows.Forms.ToolStripMenuItem cmenuRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem cmenuDelete;
		private HeaderDrawer drwHeader1;
		private LineNoDrawer drwLineNo;
		private HexViewDrawer drwHexView;
		private TextViewDrawer drwTextView;
		private System.Windows.Forms.TextBox txtTextInput;
		private HeaderDrawer drwHeader2;
		private HeaderDrawer drwHeader3;

    }
}
