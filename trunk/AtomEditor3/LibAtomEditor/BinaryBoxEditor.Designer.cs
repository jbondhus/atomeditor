﻿namespace Kirishima16.Libraries.AtomEditor
{
	partial class BinaryBoxEditor
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

		#region コンポーネント デザイナで生成されたコード

		/// <summary> 
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.bineditMain = new Kirishima16.Forms.BinaryEditor();
			this.SuspendLayout();
			// 
			// bineditMain
			// 
			this.bineditMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bineditMain.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.bineditMain.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.bineditMain.HexViewForeColor = System.Drawing.SystemColors.ControlText;
			this.bineditMain.Location = new System.Drawing.Point(0, 0);
			this.bineditMain.MinimumSize = new System.Drawing.Size(485, 24);
			this.bineditMain.Modified = false;
			this.bineditMain.Name = "bineditMain";
			this.bineditMain.SelectionLength = ((long)(0));
			this.bineditMain.SelectionStart = ((long)(0));
			this.bineditMain.Size = new System.Drawing.Size(507, 100);
			this.bineditMain.TabIndex = 0;
			this.bineditMain.TextViewBackColor = System.Drawing.Color.AliceBlue;
			this.bineditMain.TextViewForeColor = System.Drawing.SystemColors.ControlText;
			// 
			// BinaryBoxEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.bineditMain);
			this.MinimumSize = new System.Drawing.Size(507, 30);
			this.Name = "BinaryBoxEditor";
			this.Size = new System.Drawing.Size(507, 100);
			this.ResumeLayout(false);

		}

		#endregion

		private Kirishima16.Forms.BinaryEditor bineditMain;
	}
}
