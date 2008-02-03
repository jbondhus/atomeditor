namespace Kirishima16.Applications.AtomEditor2
{
	partial class NewBoxForm
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
			this.txtName = new System.Windows.Forms.MaskedTextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.nudLength = new System.Windows.Forms.NumericUpDown();
			this.btnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLength)).BeginInit();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(6, 30);
			this.txtName.Mask = "<LLLL";
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(52, 19);
			this.txtName.TabIndex = 0;
			this.txtName.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtName_MaskInputRejected);
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.txtName);
			this.groupBox2.Controls.Add(this.nudLength);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(170, 67);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Boxの設定";
			// 
			// nudLength
			// 
			this.nudLength.Location = new System.Drawing.Point(64, 30);
			this.nudLength.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
			this.nudLength.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.nudLength.Name = "nudLength";
			this.nudLength.Size = new System.Drawing.Size(100, 19);
			this.nudLength.TabIndex = 1;
			this.nudLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudLength.ThousandsSeparator = true;
			this.nudLength.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			// 
			// btnAccept
			// 
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Location = new System.Drawing.Point(26, 85);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 1;
			this.btnAccept.Text = "追加";
			this.btnAccept.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(107, 85);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(63, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "Length";
			// 
			// NewBoxForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(194, 120);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.groupBox2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewBoxForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Boxの追加";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLength)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MaskedTextBox txtName;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown nudLength;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Button btnCancel;
	}
}