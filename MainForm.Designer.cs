namespace Kirishima16.Applications.AtomEditor.V3
{
	partial class MainForm
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
			this.dpMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.SuspendLayout();
			// 
			// dpMain
			// 
			this.dpMain.ActiveAutoHideContent = null;
			this.dpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dpMain.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.dpMain.Location = new System.Drawing.Point(0, 0);
			this.dpMain.Name = "dpMain";
			this.dpMain.ShowDocumentIcon = true;
			this.dpMain.Size = new System.Drawing.Size(292, 266);
			this.dpMain.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.dpMain);
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private WeifenLuo.WinFormsUI.Docking.DockPanel dpMain;
	}
}

