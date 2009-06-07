namespace Kirishima16.Applications.AtomEditor2
{
	/// <summary>
	/// ATOMEditor2のメインフォームです。
	/// </summary>
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
            if (disposing && (components != null))
            {
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
			this.components = new System.ComponentModel.Container();
			this.tviewBox = new System.Windows.Forms.TreeView();
			this.imglTree = new System.Windows.Forms.ImageList(this.components);
			this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiOpenBox = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiOpenBoxBy = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsTreeOpenBy = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiAddBox = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsTreeAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiNewBefore = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiNewAfter = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiNewChild = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiLoadBefore = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiLoadAfter = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiLoadChild = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.cmiPasteBefore = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiPasteAfter = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiPasteChild = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCutBox = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCopyBox = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiRemoveBox = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsListView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiCopyDetail = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiSelectAllDetails = new System.Windows.Forms.ToolStripMenuItem();
			this.mmsMain = new System.Windows.Forms.MenuStrip();
			this.mmrFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiOpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiSaveFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiSaveFileAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mmiExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mmrTab = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiSaveTab = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiCloseTab = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiSaveAllTabs = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiCloseAllTabs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.mmiExportBoxBinary = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiImportBoxBinary = new System.Windows.Forms.ToolStripMenuItem();
			this.mmrHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiOpenHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiOpenReadme = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.mmiOpenDirectory = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiOpenInstallDir = new System.Windows.Forms.ToolStripMenuItem();
			this.mmiOpenPluginDir = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mmiRunNgen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.mmiAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.ofdMain = new System.Windows.Forms.OpenFileDialog();
			this.ssMain = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.prgStatus = new System.Windows.Forms.ToolStripProgressBar();
			this.sfdMain = new System.Windows.Forms.SaveFileDialog();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tabcMain = new System.Windows.Forms.TabControl();
			this.sfdBox = new System.Windows.Forms.SaveFileDialog();
			this.cmsTab = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiSaveTab = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCloseTab = new System.Windows.Forms.ToolStripMenuItem();
			this.ttipMain = new System.Windows.Forms.ToolTip(this.components);
			this.ofdBox = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lviewDetail = new System.Windows.Forms.ListView();
			this.名前 = new System.Windows.Forms.ColumnHeader();
			this.値 = new System.Windows.Forms.ColumnHeader();
			this.説明 = new System.Windows.Forms.ColumnHeader();
			this.cmsTree.SuspendLayout();
			this.cmsTreeAdd.SuspendLayout();
			this.cmsListView.SuspendLayout();
			this.mmsMain.SuspendLayout();
			this.ssMain.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.cmsTab.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tviewBox
			// 
			this.tviewBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tviewBox.ImageIndex = 0;
			this.tviewBox.ImageList = this.imglTree;
			this.tviewBox.Location = new System.Drawing.Point(3, 3);
			this.tviewBox.Name = "tviewBox";
			this.tviewBox.SelectedImageIndex = 0;
			this.tviewBox.ShowNodeToolTips = true;
			this.tviewBox.Size = new System.Drawing.Size(186, 387);
			this.tviewBox.TabIndex = 1;
			this.tviewBox.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tviewBox_NodeMouseDoubleClick);
			this.tviewBox.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tviewBox_AfterCollapse);
			this.tviewBox.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tviewBox_BeforeExpand);
			this.tviewBox.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tviewBox_BeforeCollapse);
			this.tviewBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.tviewBox_PreviewKeyDown);
			this.tviewBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tviewBox_MouseMove);
			this.tviewBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tviewBox_MouseDown);
			this.tviewBox.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tviewBox_NodeMouseClick);
			this.tviewBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tviewBox_KeyDown);
			this.tviewBox.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tviewBox_AfterExpand);
			// 
			// imglTree
			// 
			this.imglTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imglTree.ImageSize = new System.Drawing.Size(16, 16);
			this.imglTree.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// cmsTree
			// 
			this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiOpenBox,
            this.cmiOpenBoxBy,
            this.toolStripSeparator4,
            this.cmiAddBox,
            this.cmiCutBox,
            this.cmiCopyBox,
            this.cmiRemoveBox});
			this.cmsTree.Name = "cmsTree";
			this.cmsTree.Size = new System.Drawing.Size(189, 142);
			this.cmsTree.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTree_Opening);
			// 
			// cmiOpenBox
			// 
			this.cmiOpenBox.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.tab_new_b;
			this.cmiOpenBox.Name = "cmiOpenBox";
			this.cmiOpenBox.Size = new System.Drawing.Size(188, 22);
			this.cmiOpenBox.Text = "開く(&O)";
			this.cmiOpenBox.Click += new System.EventHandler(this.cmiOpenBox_Click);
			// 
			// cmiOpenBoxBy
			// 
			this.cmiOpenBoxBy.DropDown = this.cmsTreeOpenBy;
			this.cmiOpenBoxBy.Name = "cmiOpenBoxBy";
			this.cmiOpenBoxBy.Size = new System.Drawing.Size(188, 22);
			this.cmiOpenBoxBy.Text = "エディタを選択して開く(&E)";
			// 
			// cmsTreeOpenBy
			// 
			this.cmsTreeOpenBy.Name = "cmsTreeOpenBy";
			this.cmsTreeOpenBy.OwnerItem = this.cmiOpenBoxBy;
			this.cmsTreeOpenBy.ShowImageMargin = false;
			this.cmsTreeOpenBy.Size = new System.Drawing.Size(36, 4);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(185, 6);
			// 
			// cmiAddBox
			// 
			this.cmiAddBox.DropDown = this.cmsTreeAdd;
			this.cmiAddBox.Enabled = false;
			this.cmiAddBox.Name = "cmiAddBox";
			this.cmiAddBox.Size = new System.Drawing.Size(188, 22);
			this.cmiAddBox.Text = "追加(&A)";
			// 
			// cmsTreeAdd
			// 
			this.cmsTreeAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNewBefore,
            this.cmiNewAfter,
            this.cmiNewChild,
            this.toolStripSeparator3,
            this.cmiLoadBefore,
            this.cmiLoadAfter,
            this.cmiLoadChild,
            this.toolStripSeparator5,
            this.cmiPasteBefore,
            this.cmiPasteAfter,
            this.cmiPasteChild});
			this.cmsTreeAdd.Name = "cmsTreeNew";
			this.cmsTreeAdd.OwnerItem = this.cmiAddBox;
			this.cmsTreeAdd.Size = new System.Drawing.Size(198, 214);
			// 
			// cmiNewBefore
			// 
			this.cmiNewBefore.Name = "cmiNewBefore";
			this.cmiNewBefore.Size = new System.Drawing.Size(197, 22);
			this.cmiNewBefore.Text = "この上に新規作成";
			this.cmiNewBefore.Click += new System.EventHandler(this.cmiNewBefore_Click);
			// 
			// cmiNewAfter
			// 
			this.cmiNewAfter.Name = "cmiNewAfter";
			this.cmiNewAfter.Size = new System.Drawing.Size(197, 22);
			this.cmiNewAfter.Text = "この下に新規作成";
			// 
			// cmiNewChild
			// 
			this.cmiNewChild.Name = "cmiNewChild";
			this.cmiNewChild.Size = new System.Drawing.Size(197, 22);
			this.cmiNewChild.Text = "子要素の末尾に新規作成";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(194, 6);
			// 
			// cmiLoadBefore
			// 
			this.cmiLoadBefore.Name = "cmiLoadBefore";
			this.cmiLoadBefore.Size = new System.Drawing.Size(197, 22);
			this.cmiLoadBefore.Text = "この上に取り込み";
			// 
			// cmiLoadAfter
			// 
			this.cmiLoadAfter.Name = "cmiLoadAfter";
			this.cmiLoadAfter.Size = new System.Drawing.Size(197, 22);
			this.cmiLoadAfter.Text = "この下に取り込み";
			// 
			// cmiLoadChild
			// 
			this.cmiLoadChild.Name = "cmiLoadChild";
			this.cmiLoadChild.Size = new System.Drawing.Size(197, 22);
			this.cmiLoadChild.Text = "子要素の末尾に取り込み";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(194, 6);
			// 
			// cmiPasteBefore
			// 
			this.cmiPasteBefore.Name = "cmiPasteBefore";
			this.cmiPasteBefore.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.cmiPasteBefore.Size = new System.Drawing.Size(197, 22);
			this.cmiPasteBefore.Text = "この上に貼り付け";
			// 
			// cmiPasteAfter
			// 
			this.cmiPasteAfter.Name = "cmiPasteAfter";
			this.cmiPasteAfter.Size = new System.Drawing.Size(197, 22);
			this.cmiPasteAfter.Text = "この下に貼り付け";
			// 
			// cmiPasteChild
			// 
			this.cmiPasteChild.Name = "cmiPasteChild";
			this.cmiPasteChild.Size = new System.Drawing.Size(197, 22);
			this.cmiPasteChild.Text = "子要素の末尾に貼り付け";
			// 
			// cmiCutBox
			// 
			this.cmiCutBox.Enabled = false;
			this.cmiCutBox.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.cut_b;
			this.cmiCutBox.Name = "cmiCutBox";
			this.cmiCutBox.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cmiCutBox.Size = new System.Drawing.Size(188, 22);
			this.cmiCutBox.Text = "切り取り(&X)";
			// 
			// cmiCopyBox
			// 
			this.cmiCopyBox.Enabled = false;
			this.cmiCopyBox.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.copy_b;
			this.cmiCopyBox.Name = "cmiCopyBox";
			this.cmiCopyBox.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cmiCopyBox.Size = new System.Drawing.Size(188, 22);
			this.cmiCopyBox.Text = "コピー(&C)";
			// 
			// cmiRemoveBox
			// 
			this.cmiRemoveBox.Enabled = false;
			this.cmiRemoveBox.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.item_del_simple;
			this.cmiRemoveBox.Name = "cmiRemoveBox";
			this.cmiRemoveBox.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.cmiRemoveBox.Size = new System.Drawing.Size(188, 22);
			this.cmiRemoveBox.Text = "削除(&D)";
			// 
			// cmsListView
			// 
			this.cmsListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiCopyDetail,
            this.cmiSelectAllDetails});
			this.cmsListView.Name = "cmsListView";
			this.cmsListView.Size = new System.Drawing.Size(178, 48);
			// 
			// cmiCopyDetail
			// 
			this.cmiCopyDetail.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.copy_b;
			this.cmiCopyDetail.Name = "cmiCopyDetail";
			this.cmiCopyDetail.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cmiCopyDetail.Size = new System.Drawing.Size(177, 22);
			this.cmiCopyDetail.Text = "コピー(&C)";
			this.cmiCopyDetail.Click += new System.EventHandler(this.cmiCopyDetail_Click);
			// 
			// cmiSelectAllDetails
			// 
			this.cmiSelectAllDetails.Name = "cmiSelectAllDetails";
			this.cmiSelectAllDetails.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.cmiSelectAllDetails.Size = new System.Drawing.Size(177, 22);
			this.cmiSelectAllDetails.Text = "すべて選択(&A)";
			this.cmiSelectAllDetails.Click += new System.EventHandler(this.cmiSelectAllDetails_Click);
			// 
			// mmsMain
			// 
			this.mmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmrFile,
            this.mmrTab,
            this.mmrHelp});
			this.mmsMain.Location = new System.Drawing.Point(0, 0);
			this.mmsMain.Name = "mmsMain";
			this.mmsMain.Size = new System.Drawing.Size(692, 24);
			this.mmsMain.TabIndex = 1;
			this.mmsMain.Text = "mmsMain";
			this.mmsMain.MenuActivate += new System.EventHandler(this.mmsMain_MenuActivate);
			this.mmsMain.LayoutCompleted += new System.EventHandler(this.mmsMain_LayoutCompleted);
			// 
			// mmrFile
			// 
			this.mmrFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiOpenFile,
            this.mmiSaveFile,
            this.mmiSaveFileAs,
            this.toolStripSeparator1,
            this.mmiExit});
			this.mmrFile.Name = "mmrFile";
			this.mmrFile.Size = new System.Drawing.Size(66, 20);
			this.mmrFile.Text = "ファイル(&F)";
			this.mmrFile.ToolTipText = "ファイルメニュー";
			// 
			// mmiOpenFile
			// 
			this.mmiOpenFile.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.fol_text_yb;
			this.mmiOpenFile.Name = "mmiOpenFile";
			this.mmiOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mmiOpenFile.Size = new System.Drawing.Size(179, 22);
			this.mmiOpenFile.Text = "開く(&O)";
			this.mmiOpenFile.ToolTipText = "MP4ファイルを開きます";
			this.mmiOpenFile.Click += new System.EventHandler(this.mmiOpenFile_Click);
			// 
			// mmiSaveFile
			// 
			this.mmiSaveFile.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.save_b;
			this.mmiSaveFile.Name = "mmiSaveFile";
			this.mmiSaveFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mmiSaveFile.Size = new System.Drawing.Size(179, 22);
			this.mmiSaveFile.Text = "上書き保存(&S)";
			this.mmiSaveFile.ToolTipText = "現在のファイルに上書き保存します";
			this.mmiSaveFile.Click += new System.EventHandler(this.mmiSaveFile_Click);
			// 
			// mmiSaveFileAs
			// 
			this.mmiSaveFileAs.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.saveAs_b;
			this.mmiSaveFileAs.Name = "mmiSaveFileAs";
			this.mmiSaveFileAs.Size = new System.Drawing.Size(179, 22);
			this.mmiSaveFileAs.Text = "名前をつけて保存(&A)";
			this.mmiSaveFileAs.ToolTipText = "指定されたファイルに保存します";
			this.mmiSaveFileAs.Click += new System.EventHandler(this.mmiSaveFileAs_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
			// 
			// mmiExit
			// 
			this.mmiExit.Name = "mmiExit";
			this.mmiExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.mmiExit.Size = new System.Drawing.Size(179, 22);
			this.mmiExit.Text = "終了(&X)";
			this.mmiExit.ToolTipText = "ATOMEditorを終了します";
			this.mmiExit.Click += new System.EventHandler(this.mmiExit_Click);
			// 
			// mmrTab
			// 
			this.mmrTab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiSaveTab,
            this.mmiCloseTab,
            this.mmiSaveAllTabs,
            this.mmiCloseAllTabs,
            this.toolStripSeparator6,
            this.mmiExportBoxBinary,
            this.mmiImportBoxBinary});
			this.mmrTab.Name = "mmrTab";
			this.mmrTab.Size = new System.Drawing.Size(49, 20);
			this.mmrTab.Text = "タブ(&T)";
			this.mmrTab.ToolTipText = "タブメニュー";
			// 
			// mmiSaveTab
			// 
			this.mmiSaveTab.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.tab_save_b;
			this.mmiSaveTab.Name = "mmiSaveTab";
			this.mmiSaveTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.mmiSaveTab.Size = new System.Drawing.Size(252, 22);
			this.mmiSaveTab.Text = "保存(&S)";
			this.mmiSaveTab.ToolTipText = "現在選択されているタブを保存します";
			this.mmiSaveTab.Click += new System.EventHandler(this.mmiSaveTab_Click);
			// 
			// mmiCloseTab
			// 
			this.mmiCloseTab.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.tab_del_b;
			this.mmiCloseTab.Name = "mmiCloseTab";
			this.mmiCloseTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
			this.mmiCloseTab.Size = new System.Drawing.Size(252, 22);
			this.mmiCloseTab.Text = "閉じる(&C)";
			this.mmiCloseTab.ToolTipText = "現在選択されているタブを閉じます";
			this.mmiCloseTab.Click += new System.EventHandler(this.mmiCloseTab_Click);
			// 
			// mmiSaveAllTabs
			// 
			this.mmiSaveAllTabs.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.tab_many_save_b;
			this.mmiSaveAllTabs.Name = "mmiSaveAllTabs";
			this.mmiSaveAllTabs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.D)));
			this.mmiSaveAllTabs.Size = new System.Drawing.Size(252, 22);
			this.mmiSaveAllTabs.Text = "すべてのタブを保存(&A)";
			this.mmiSaveAllTabs.ToolTipText = "現在開かれているすべてのタブを保存します";
			this.mmiSaveAllTabs.Click += new System.EventHandler(this.mmiSaveAllTabs_Click);
			// 
			// mmiCloseAllTabs
			// 
			this.mmiCloseAllTabs.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.tab_many_del_b;
			this.mmiCloseAllTabs.Name = "mmiCloseAllTabs";
			this.mmiCloseAllTabs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.F4)));
			this.mmiCloseAllTabs.Size = new System.Drawing.Size(252, 22);
			this.mmiCloseAllTabs.Text = "すべてのタブを閉じる(&L)";
			this.mmiCloseAllTabs.ToolTipText = "現在開かれているすべてのタブを閉じます";
			this.mmiCloseAllTabs.Click += new System.EventHandler(this.mmiCloseAllTabs_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(249, 6);
			// 
			// mmiExportBoxBinary
			// 
			this.mmiExportBoxBinary.Name = "mmiExportBoxBinary";
			this.mmiExportBoxBinary.Size = new System.Drawing.Size(252, 22);
			this.mmiExportBoxBinary.Text = "バイナリをファイルに保存(&E)";
			this.mmiExportBoxBinary.ToolTipText = "現在選択されているタブの内容をファイルに保存します";
			this.mmiExportBoxBinary.Click += new System.EventHandler(this.mmiExportBoxBinary_Click);
			// 
			// mmiImportBoxBinary
			// 
			this.mmiImportBoxBinary.Name = "mmiImportBoxBinary";
			this.mmiImportBoxBinary.Size = new System.Drawing.Size(252, 22);
			this.mmiImportBoxBinary.Text = "バイナリをファイルから読み込み(&I)";
			this.mmiImportBoxBinary.ToolTipText = "現在選択されているタブの内容をファイルから読み込みます";
			this.mmiImportBoxBinary.Click += new System.EventHandler(this.mmiImportBoxBinary_Click);
			// 
			// mmrHelp
			// 
			this.mmrHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiOpenHelp,
            this.mmiOpenReadme,
            this.toolStripSeparator8,
            this.mmiOpenDirectory,
            this.toolStripSeparator2,
            this.mmiRunNgen,
            this.toolStripSeparator7,
            this.mmiAbout});
			this.mmrHelp.Name = "mmrHelp";
			this.mmrHelp.Size = new System.Drawing.Size(64, 20);
			this.mmrHelp.Text = "ヘルプ\'(&H)";
			this.mmrHelp.ToolTipText = "ヘルプメニュー";
			// 
			// mmiOpenHelp
			// 
			this.mmiOpenHelp.Enabled = false;
			this.mmiOpenHelp.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.book_close_b;
			this.mmiOpenHelp.Name = "mmiOpenHelp";
			this.mmiOpenHelp.Size = new System.Drawing.Size(188, 22);
			this.mmiOpenHelp.Text = "ヘルプファイルを開く(&H)";
			this.mmiOpenHelp.ToolTipText = "ヘルプファイルを開きます";
			// 
			// mmiOpenReadme
			// 
			this.mmiOpenReadme.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.file_text1_b;
			this.mmiOpenReadme.Name = "mmiOpenReadme";
			this.mmiOpenReadme.Size = new System.Drawing.Size(188, 22);
			this.mmiOpenReadme.Text = "Readmeファイルを開く(&R)";
			this.mmiOpenReadme.ToolTipText = "Readmeファイルを開きます";
			this.mmiOpenReadme.Click += new System.EventHandler(this.mmiOpenReadme_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(185, 6);
			// 
			// mmiOpenDirectory
			// 
			this.mmiOpenDirectory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiOpenInstallDir,
            this.mmiOpenPluginDir});
			this.mmiOpenDirectory.Name = "mmiOpenDirectory";
			this.mmiOpenDirectory.Size = new System.Drawing.Size(188, 22);
			this.mmiOpenDirectory.Text = "フォルダを開く(&D)";
			this.mmiOpenDirectory.ToolTipText = "ATOMEditorが使用しているフォルダを開きます";
			// 
			// mmiOpenInstallDir
			// 
			this.mmiOpenInstallDir.Name = "mmiOpenInstallDir";
			this.mmiOpenInstallDir.Size = new System.Drawing.Size(198, 22);
			this.mmiOpenInstallDir.Text = "インストールフォルダを開く(&I)";
			this.mmiOpenInstallDir.ToolTipText = "ATOMEditor本体のあるフォルダを開きます";
			this.mmiOpenInstallDir.Click += new System.EventHandler(this.mmiOpenInstallDir_Click);
			// 
			// mmiOpenPluginDir
			// 
			this.mmiOpenPluginDir.Name = "mmiOpenPluginDir";
			this.mmiOpenPluginDir.Size = new System.Drawing.Size(198, 22);
			this.mmiOpenPluginDir.Text = "プラグインフォルダを開く(&P)";
			this.mmiOpenPluginDir.ToolTipText = "ATOMEditorのプラグインを設置するフォルダを開きます";
			this.mmiOpenPluginDir.Click += new System.EventHandler(this.mmiOpenPluginDir_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
			// 
			// mmiRunNgen
			// 
			this.mmiRunNgen.Name = "mmiRunNgen";
			this.mmiRunNgen.Size = new System.Drawing.Size(188, 22);
			this.mmiRunNgen.Text = "最適化処理(&N)";
			this.mmiRunNgen.Click += new System.EventHandler(this.mmiRunNgen_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(185, 6);
			// 
			// mmiAbout
			// 
			this.mmiAbout.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.win_iframe;
			this.mmiAbout.Name = "mmiAbout";
			this.mmiAbout.Size = new System.Drawing.Size(188, 22);
			this.mmiAbout.Text = "バージョン情報(&A)";
			this.mmiAbout.ToolTipText = "ATOMEditorのバージョン情報を表示します";
			this.mmiAbout.Click += new System.EventHandler(this.mmiAbout_Click);
			// 
			// ofdMain
			// 
			this.ofdMain.Filter = "3GPP2 Files|*.3g2|3GPP Files|*.3gp|MP4 Files|*.mp4|All Files|*.*";
			// 
			// ssMain
			// 
			this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.prgStatus});
			this.ssMain.Location = new System.Drawing.Point(0, 544);
			this.ssMain.Name = "ssMain";
			this.ssMain.Size = new System.Drawing.Size(692, 22);
			this.ssMain.SizingGrip = false;
			this.ssMain.TabIndex = 2;
			this.ssMain.Text = "statusStrip1";
			// 
			// lblStatus
			// 
			this.lblStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(575, 17);
			this.lblStatus.Spring = true;
			this.lblStatus.Text = "コマンド";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// prgStatus
			// 
			this.prgStatus.Name = "prgStatus";
			this.prgStatus.Size = new System.Drawing.Size(100, 16);
			this.prgStatus.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// sfdMain
			// 
			this.sfdMain.DefaultExt = "3g2";
			this.sfdMain.Filter = "3GPP2 Files|*.3g2|3GPP Files|*.3gp|MP4 Files|*.mp4|All Files|*.*";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoScroll = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
			this.tableLayoutPanel1.Controls.Add(this.tviewBox, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tabcMain, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(692, 393);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// tabcMain
			// 
			this.tabcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabcMain.ImageList = this.imglTree;
			this.tabcMain.Location = new System.Drawing.Point(192, 0);
			this.tabcMain.Margin = new System.Windows.Forms.Padding(0);
			this.tabcMain.Name = "tabcMain";
			this.tabcMain.Padding = new System.Drawing.Point(0, 0);
			this.tabcMain.SelectedIndex = 0;
			this.tabcMain.Size = new System.Drawing.Size(500, 393);
			this.tabcMain.TabIndex = 2;
			this.tabcMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tabcMain_MouseMove);
			this.tabcMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabcMain_MouseClick);
			this.tabcMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabcMain_MouseDown);
			this.tabcMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabcMain_MouseUp);
			this.tabcMain.SelectedIndexChanged += new System.EventHandler(this.tabcMain_SelectedIndexChanged);
			// 
			// sfdBox
			// 
			this.sfdBox.DefaultExt = "dat";
			// 
			// cmsTab
			// 
			this.cmsTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiSaveTab,
            this.cmiCloseTab});
			this.cmsTab.Name = "cmsTab";
			this.cmsTab.Size = new System.Drawing.Size(117, 48);
			this.cmsTab.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTab_Opening);
			// 
			// cmiSaveTab
			// 
			this.cmiSaveTab.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.tab_save_b;
			this.cmiSaveTab.Name = "cmiSaveTab";
			this.cmiSaveTab.Size = new System.Drawing.Size(116, 22);
			this.cmiSaveTab.Text = "保存(&S)";
			this.cmiSaveTab.Click += new System.EventHandler(this.cmiSaveTab_Click);
			// 
			// cmiCloseTab
			// 
			this.cmiCloseTab.Image = global::Kirishima16.Applications.AtomEditor2.Properties.Resources.tab_del_b;
			this.cmiCloseTab.Name = "cmiCloseTab";
			this.cmiCloseTab.Size = new System.Drawing.Size(116, 22);
			this.cmiCloseTab.Text = "閉じる(&C)";
			this.cmiCloseTab.Click += new System.EventHandler(this.cmiCloseTab_Click);
			// 
			// ofdBox
			// 
			this.ofdBox.DefaultExt = "dat";
			this.ofdBox.Filter = "Boxエクスポートファイル|*.dat|すべてのファイル|*.*";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lviewDetail);
			this.splitContainer1.Size = new System.Drawing.Size(692, 520);
			this.splitContainer1.SplitterDistance = 393;
			this.splitContainer1.TabIndex = 6;
			// 
			// lviewDetail
			// 
			this.lviewDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.名前,
            this.値,
            this.説明});
			this.lviewDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lviewDetail.Location = new System.Drawing.Point(0, 0);
			this.lviewDetail.Name = "lviewDetail";
			this.lviewDetail.Size = new System.Drawing.Size(692, 123);
			this.lviewDetail.TabIndex = 0;
			this.lviewDetail.UseCompatibleStateImageBehavior = false;
			this.lviewDetail.View = System.Windows.Forms.View.Details;
			// 
			// 名前
			// 
			this.名前.Text = "名前";
			// 
			// 値
			// 
			this.値.Text = "値";
			// 
			// 説明
			// 
			this.説明.Text = "説明";
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(692, 566);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.ssMain);
			this.Controls.Add(this.mmsMain);
			this.MainMenuStrip = this.mmsMain;
			this.Name = "MainForm";
			this.Text = "ATOMEditor";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.cmsTree.ResumeLayout(false);
			this.cmsTreeAdd.ResumeLayout(false);
			this.cmsListView.ResumeLayout(false);
			this.mmsMain.ResumeLayout(false);
			this.mmsMain.PerformLayout();
			this.ssMain.ResumeLayout(false);
			this.ssMain.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.cmsTab.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.TreeView tviewBox;
        private System.Windows.Forms.MenuStrip mmsMain;
        private System.Windows.Forms.ToolStripMenuItem mmrFile;
        private System.Windows.Forms.ToolStripMenuItem mmiOpenFile;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveFile;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveFileAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mmiExit;
        private System.Windows.Forms.ToolStripMenuItem mmrHelp;
        private System.Windows.Forms.ToolStripMenuItem mmiOpenHelp;
        private System.Windows.Forms.OpenFileDialog ofdMain;
        private System.Windows.Forms.StatusStrip ssMain;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.SaveFileDialog sfdMain;
		private System.Windows.Forms.SaveFileDialog sfdBox;
		private System.Windows.Forms.ContextMenuStrip cmsTree;
		private System.Windows.Forms.ToolStripMenuItem cmiCutBox;
		private System.Windows.Forms.ToolStripMenuItem cmiRemoveBox;
        private System.Windows.Forms.ContextMenuStrip cmsTreeAdd;
		private System.Windows.Forms.ToolStripMenuItem mmiOpenReadme;
		private System.Windows.Forms.TabControl tabcMain;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ContextMenuStrip cmsTab;
		private System.Windows.Forms.ToolStripMenuItem cmiSaveTab;
		private System.Windows.Forms.ToolStripMenuItem cmiCloseTab;
		private System.Windows.Forms.ToolStripProgressBar prgStatus;
		private System.Windows.Forms.ToolStripMenuItem cmiOpenBox;
		private System.Windows.Forms.ToolStripMenuItem cmiOpenBoxBy;
		private System.Windows.Forms.ContextMenuStrip cmsTreeOpenBy;
		private System.Windows.Forms.ToolStripMenuItem cmiAddBox;
		private System.Windows.Forms.ToolStripMenuItem cmiNewBefore;
		private System.Windows.Forms.ToolStripMenuItem cmiNewAfter;
		private System.Windows.Forms.ToolStripMenuItem cmiNewChild;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem cmiLoadBefore;
		private System.Windows.Forms.ToolStripMenuItem cmiLoadAfter;
		private System.Windows.Forms.ToolStripMenuItem cmiLoadChild;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem cmiPasteBefore;
		private System.Windows.Forms.ToolStripMenuItem cmiPasteAfter;
		private System.Windows.Forms.ToolStripMenuItem cmiPasteChild;
		private System.Windows.Forms.ToolStripMenuItem cmiCopyBox;
		private System.Windows.Forms.ToolTip ttipMain;
		private System.Windows.Forms.ContextMenuStrip cmsListView;
		private System.Windows.Forms.ToolStripMenuItem cmiCopyDetail;
		private System.Windows.Forms.ToolStripMenuItem cmiSelectAllDetails;
        private System.Windows.Forms.ToolStripMenuItem mmrTab;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveTab;
        private System.Windows.Forms.ToolStripMenuItem mmiCloseTab;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveAllTabs;
        private System.Windows.Forms.ToolStripMenuItem mmiCloseAllTabs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mmiExportBoxBinary;
        private System.Windows.Forms.ToolStripMenuItem mmiImportBoxBinary;
        private System.Windows.Forms.ToolStripMenuItem mmiOpenDirectory;
        private System.Windows.Forms.ToolStripMenuItem mmiOpenInstallDir;
        private System.Windows.Forms.ToolStripMenuItem mmiOpenPluginDir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem mmiAbout;
		private System.Windows.Forms.OpenFileDialog ofdBox;
		private System.Windows.Forms.ImageList imglTree;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem mmiRunNgen;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListView lviewDetail;
		private System.Windows.Forms.ColumnHeader 名前;
		private System.Windows.Forms.ColumnHeader 値;
		private System.Windows.Forms.ColumnHeader 説明;
    }
}

