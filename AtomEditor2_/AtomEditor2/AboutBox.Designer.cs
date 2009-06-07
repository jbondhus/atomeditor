namespace Kirishima16.Applications.AtomEditor2 {
	partial class AboutBox {
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("システム", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("アプリケーション", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("プラグイン", System.Windows.Forms.HorizontalAlignment.Left);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lvRefAsms = new System.Windows.Forms.ListView();
			this.chName = new System.Windows.Forms.ColumnHeader();
			this.chVersion = new System.Windows.Forms.ColumnHeader();
			this.chAuthor = new System.Windows.Forms.ColumnHeader();
			this.chCopyright = new System.Windows.Forms.ColumnHeader();
			this.chLocation = new System.Windows.Forms.ColumnHeader();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.pbxBanner = new System.Windows.Forms.PictureBox();
			this.lblExeAsm = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCopy = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbxBanner)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.lvRefAsms);
			this.groupBox1.Location = new System.Drawing.Point(3, 115);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(588, 221);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "読み込まれているアセンブリ";
			// 
			// lvRefAsms
			// 
			this.lvRefAsms.BackColor = System.Drawing.Color.White;
			this.lvRefAsms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chVersion,
            this.chAuthor,
            this.chCopyright,
            this.chLocation});
			this.lvRefAsms.Dock = System.Windows.Forms.DockStyle.Fill;
			listViewGroup1.Header = "システム";
			listViewGroup1.Name = "lvgSystem";
			listViewGroup2.Header = "アプリケーション";
			listViewGroup2.Name = "lvgApplication";
			listViewGroup3.Header = "プラグイン";
			listViewGroup3.Name = "lvgPlugin";
			this.lvRefAsms.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
			this.lvRefAsms.Location = new System.Drawing.Point(3, 15);
			this.lvRefAsms.MultiSelect = false;
			this.lvRefAsms.Name = "lvRefAsms";
			this.lvRefAsms.Size = new System.Drawing.Size(582, 203);
			this.lvRefAsms.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvRefAsms.TabIndex = 0;
			this.lvRefAsms.UseCompatibleStateImageBehavior = false;
			this.lvRefAsms.View = System.Windows.Forms.View.Details;
			// 
			// chName
			// 
			this.chName.Text = "アセンブリ名";
			this.chName.Width = 150;
			// 
			// chVersion
			// 
			this.chVersion.Text = "バージョン";
			this.chVersion.Width = 90;
			// 
			// chAuthor
			// 
			this.chAuthor.Text = "製作者名";
			this.chAuthor.Width = 150;
			// 
			// chCopyright
			// 
			this.chCopyright.Text = "著作権";
			// 
			// chLocation
			// 
			this.chLocation.Text = "場所";
			this.chLocation.Width = 200;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.pbxBanner, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblExeAsm, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(594, 339);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// pbxBanner
			// 
			this.pbxBanner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pbxBanner.BackColor = System.Drawing.Color.White;
			this.pbxBanner.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.ATOMEditorLogo;
			this.pbxBanner.Location = new System.Drawing.Point(0, 0);
			this.pbxBanner.Margin = new System.Windows.Forms.Padding(0);
			this.pbxBanner.Name = "pbxBanner";
			this.pbxBanner.Size = new System.Drawing.Size(594, 52);
			this.pbxBanner.TabIndex = 1;
			this.pbxBanner.TabStop = false;
			// 
			// lblExeAsm
			// 
			this.lblExeAsm.AutoSize = true;
			this.lblExeAsm.Location = new System.Drawing.Point(3, 58);
			this.lblExeAsm.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblExeAsm.Name = "lblExeAsm";
			this.lblExeAsm.Size = new System.Drawing.Size(95, 48);
			this.lblExeAsm.TabIndex = 1;
			this.lblExeAsm.Text = "Company Product\r\nAssembly\r\nVersion\t1.0.0.0\r\nCopyright ";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.flowLayoutPanel1.Controls.Add(this.btnOK);
			this.flowLayoutPanel1.Controls.Add(this.btnCopy);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 339);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(594, 29);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(516, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCopy
			// 
			this.btnCopy.Location = new System.Drawing.Point(410, 3);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(100, 23);
			this.btnCopy.TabIndex = 1;
			this.btnCopy.Text = "情報のコピー(&C)";
			this.btnCopy.UseVisualStyleBackColor = false;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// AboutBox
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(594, 368);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.DoubleBuffered = true;
			this.MinimizeBox = false;
			this.Name = "AboutBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TransparencyKey = System.Drawing.Color.Fuchsia;
			this.Load += new System.EventHandler(this.AboutBox_Load);
			this.SizeChanged += new System.EventHandler(this.AboutBox_SizeChanged);
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbxBanner)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView lvRefAsms;
		private System.Windows.Forms.ColumnHeader chName;
		private System.Windows.Forms.ColumnHeader chVersion;
		private System.Windows.Forms.ColumnHeader chAuthor;
		private System.Windows.Forms.ColumnHeader chCopyright;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ColumnHeader chLocation;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.PictureBox pbxBanner;
		private System.Windows.Forms.Label lblExeAsm;

	}
}
