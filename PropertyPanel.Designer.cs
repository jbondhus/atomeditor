namespace Kirishima16.Applications.AtomEditor.V3
{
	partial class PropertyPanel
	{
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// �g�p���̃��\�[�X�����ׂăN���[���A�b�v���܂��B
		/// </summary>
		/// <param name="disposing">�}�l�[�W ���\�[�X���j�������ꍇ true�A�j������Ȃ��ꍇ�� false �ł��B</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h

		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
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
