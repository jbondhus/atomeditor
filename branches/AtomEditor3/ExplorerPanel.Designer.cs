namespace Kirishima16.Applications.AtomEditor.V3
{
	partial class ExplorerPanel
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
			this.tvExplorer = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// tvExplorer
			// 
			this.tvExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvExplorer.Location = new System.Drawing.Point(0, 0);
			this.tvExplorer.Name = "tvExplorer";
			this.tvExplorer.Size = new System.Drawing.Size(192, 266);
			this.tvExplorer.TabIndex = 0;
			// 
			// ExplorerPanel
			// 
			this.ClientSize = new System.Drawing.Size(192, 266);
			this.Controls.Add(this.tvExplorer);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
			this.HideOnClose = true;
			this.Name = "ExplorerPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Explorer";
			this.Text = "Explorer";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tvExplorer;
	}
}
