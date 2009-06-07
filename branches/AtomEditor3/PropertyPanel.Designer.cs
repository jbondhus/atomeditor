namespace Kirishima16.Applications.AtomEditor.V3
{
	partial class PropertyPanel
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
			this.pgProperty = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// pgProperty
			// 
			this.pgProperty.CommandsVisibleIfAvailable = false;
			this.pgProperty.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgProperty.Location = new System.Drawing.Point(0, 0);
			this.pgProperty.Name = "pgProperty";
			this.pgProperty.Size = new System.Drawing.Size(192, 266);
			this.pgProperty.TabIndex = 0;
			this.pgProperty.ToolbarVisible = false;
			// 
			// PropertyPanel
			// 
			this.ClientSize = new System.Drawing.Size(192, 266);
			this.Controls.Add(this.pgProperty);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.HideOnClose = true;
			this.Name = "PropertyPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Property";
			this.Text = "Property";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PropertyGrid pgProperty;
	}
}
