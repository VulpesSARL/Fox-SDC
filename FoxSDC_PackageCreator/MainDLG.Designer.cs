namespace FoxSDC_PackageCreator
{
    partial class MainDLG
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDLG));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.testCompilePackageScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cmdDT = new System.Windows.Forms.Button();
            this.cmdPlus1 = new System.Windows.Forms.Button();
            this.chkNoReceipt = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdSelectOutputFilename = new System.Windows.Forms.Button();
            this.txtOutputFilename = new System.Windows.Forms.TextBox();
            this.cmdNewGUID = new System.Windows.Forms.Button();
            this.txtPackageID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPackageDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPackageTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeFileFolders = new System.Windows.Forms.TreeView();
            this.cntxFileFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rootFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderFromSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lstFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cntxFileFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addmultipleFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtScript = new System.Windows.Forms.TextBox();
            this.cmdDT2 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cntxFileFolder.SuspendLayout();
            this.cntxFileFiles.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(621, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.saveasToolStripMenuItem,
            this.toolStripMenuItem2,
            this.testCompilePackageScriptToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(296, 24);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(296, 24);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(293, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(296, 24);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveasToolStripMenuItem
            // 
            this.saveasToolStripMenuItem.Name = "saveasToolStripMenuItem";
            this.saveasToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveasToolStripMenuItem.Size = new System.Drawing.Size(296, 24);
            this.saveasToolStripMenuItem.Text = "Save &as";
            this.saveasToolStripMenuItem.Click += new System.EventHandler(this.saveasToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(293, 6);
            // 
            // testCompilePackageScriptToolStripMenuItem
            // 
            this.testCompilePackageScriptToolStripMenuItem.Name = "testCompilePackageScriptToolStripMenuItem";
            this.testCompilePackageScriptToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.testCompilePackageScriptToolStripMenuItem.Size = new System.Drawing.Size(296, 24);
            this.testCompilePackageScriptToolStripMenuItem.Text = "&Test compile package script";
            this.testCompilePackageScriptToolStripMenuItem.Click += new System.EventHandler(this.testCompilePackageScriptToolStripMenuItem_Click);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(296, 24);
            this.compileToolStripMenuItem.Text = "&Compile package";
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(293, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(296, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(621, 460);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cmdDT2);
            this.tabPage3.Controls.Add(this.cmdDT);
            this.tabPage3.Controls.Add(this.cmdPlus1);
            this.tabPage3.Controls.Add(this.chkNoReceipt);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.txtVersion);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.cmdSelectOutputFilename);
            this.tabPage3.Controls.Add(this.txtOutputFilename);
            this.tabPage3.Controls.Add(this.cmdNewGUID);
            this.tabPage3.Controls.Add(this.txtPackageID);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.txtPackageDescription);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.txtPackageTitle);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(613, 434);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "General options";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cmdDT
            // 
            this.cmdDT.Location = new System.Drawing.Point(459, 203);
            this.cmdDT.Name = "cmdDT";
            this.cmdDT.Size = new System.Drawing.Size(34, 23);
            this.cmdDT.TabIndex = 8;
            this.cmdDT.Text = "DT";
            this.cmdDT.UseVisualStyleBackColor = true;
            this.cmdDT.Click += new System.EventHandler(this.cmdDT_Click);
            // 
            // cmdPlus1
            // 
            this.cmdPlus1.Location = new System.Drawing.Point(418, 203);
            this.cmdPlus1.Name = "cmdPlus1";
            this.cmdPlus1.Size = new System.Drawing.Size(34, 23);
            this.cmdPlus1.TabIndex = 7;
            this.cmdPlus1.Text = "+1";
            this.cmdPlus1.UseVisualStyleBackColor = true;
            this.cmdPlus1.Click += new System.EventHandler(this.cmdPlus1_Click);
            // 
            // chkNoReceipt
            // 
            this.chkNoReceipt.AutoSize = true;
            this.chkNoReceipt.Location = new System.Drawing.Point(111, 257);
            this.chkNoReceipt.Name = "chkNoReceipt";
            this.chkNoReceipt.Size = new System.Drawing.Size(349, 17);
            this.chkNoReceipt.TabIndex = 10;
            this.chkNoReceipt.Text = "Do not create an installation receipt (Package cannot be uninstalled)";
            this.chkNoReceipt.UseVisualStyleBackColor = true;
            this.chkNoReceipt.CheckedChanged += new System.EventHandler(this.chkNoReceipt_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Version ID:";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(111, 205);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(301, 20);
            this.txtVersion.TabIndex = 6;
            this.txtVersion.TextChanged += new System.EventHandler(this.txtVersion_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Output filename:";
            // 
            // cmdSelectOutputFilename
            // 
            this.cmdSelectOutputFilename.Location = new System.Drawing.Point(418, 177);
            this.cmdSelectOutputFilename.Name = "cmdSelectOutputFilename";
            this.cmdSelectOutputFilename.Size = new System.Drawing.Size(75, 23);
            this.cmdSelectOutputFilename.TabIndex = 5;
            this.cmdSelectOutputFilename.Text = "Browse";
            this.cmdSelectOutputFilename.UseVisualStyleBackColor = true;
            this.cmdSelectOutputFilename.Click += new System.EventHandler(this.cmdSelectOutputFilename_Click);
            // 
            // txtOutputFilename
            // 
            this.txtOutputFilename.Location = new System.Drawing.Point(111, 179);
            this.txtOutputFilename.Name = "txtOutputFilename";
            this.txtOutputFilename.Size = new System.Drawing.Size(301, 20);
            this.txtOutputFilename.TabIndex = 4;
            this.txtOutputFilename.TextChanged += new System.EventHandler(this.txtMULTI_TextChanged);
            // 
            // cmdNewGUID
            // 
            this.cmdNewGUID.Location = new System.Drawing.Point(418, 151);
            this.cmdNewGUID.Name = "cmdNewGUID";
            this.cmdNewGUID.Size = new System.Drawing.Size(75, 23);
            this.cmdNewGUID.TabIndex = 3;
            this.cmdNewGUID.Text = "New ID";
            this.cmdNewGUID.UseVisualStyleBackColor = true;
            this.cmdNewGUID.Click += new System.EventHandler(this.cmdNewGUID_Click);
            // 
            // txtPackageID
            // 
            this.txtPackageID.Location = new System.Drawing.Point(111, 153);
            this.txtPackageID.Name = "txtPackageID";
            this.txtPackageID.Size = new System.Drawing.Size(301, 20);
            this.txtPackageID.TabIndex = 2;
            this.txtPackageID.TextChanged += new System.EventHandler(this.txtMULTI_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Package ID:";
            // 
            // txtPackageDescription
            // 
            this.txtPackageDescription.AcceptsReturn = true;
            this.txtPackageDescription.Location = new System.Drawing.Point(111, 38);
            this.txtPackageDescription.Multiline = true;
            this.txtPackageDescription.Name = "txtPackageDescription";
            this.txtPackageDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPackageDescription.Size = new System.Drawing.Size(301, 109);
            this.txtPackageDescription.TabIndex = 1;
            this.txtPackageDescription.TextChanged += new System.EventHandler(this.txtMULTI_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description:";
            // 
            // txtPackageTitle
            // 
            this.txtPackageTitle.Location = new System.Drawing.Point(111, 12);
            this.txtPackageTitle.Name = "txtPackageTitle";
            this.txtPackageTitle.Size = new System.Drawing.Size(301, 20);
            this.txtPackageTitle.TabIndex = 0;
            this.txtPackageTitle.TextChanged += new System.EventHandler(this.txtMULTI_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(613, 434);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Files";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeFileFolders);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstFiles);
            this.splitContainer1.Size = new System.Drawing.Size(607, 428);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeFileFolders
            // 
            this.treeFileFolders.ContextMenuStrip = this.cntxFileFolder;
            this.treeFileFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFileFolders.FullRowSelect = true;
            this.treeFileFolders.HideSelection = false;
            this.treeFileFolders.ImageIndex = 0;
            this.treeFileFolders.ImageList = this.imageList1;
            this.treeFileFolders.Location = new System.Drawing.Point(0, 0);
            this.treeFileFolders.Name = "treeFileFolders";
            this.treeFileFolders.SelectedImageIndex = 0;
            this.treeFileFolders.Size = new System.Drawing.Size(202, 428);
            this.treeFileFolders.TabIndex = 0;
            this.treeFileFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFileFolders_AfterSelect);
            // 
            // cntxFileFolder
            // 
            this.cntxFileFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFolderToolStripMenuItem,
            this.addFolderFromSystemToolStripMenuItem,
            this.deleteFolderToolStripMenuItem,
            this.renameFolderToolStripMenuItem,
            this.propertiesToolStripMenuItem1});
            this.cntxFileFolder.Name = "cntxFileFolder";
            this.cntxFileFolder.Size = new System.Drawing.Size(304, 124);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rootFolderToolStripMenuItem,
            this.currentFolderToolStripMenuItem});
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(303, 24);
            this.newFolderToolStripMenuItem.Text = "&New folder";
            // 
            // rootFolderToolStripMenuItem
            // 
            this.rootFolderToolStripMenuItem.Name = "rootFolderToolStripMenuItem";
            this.rootFolderToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.rootFolderToolStripMenuItem.Text = "&root folder";
            this.rootFolderToolStripMenuItem.Click += new System.EventHandler(this.rootFolderToolStripMenuItem_Click);
            // 
            // currentFolderToolStripMenuItem
            // 
            this.currentFolderToolStripMenuItem.Name = "currentFolderToolStripMenuItem";
            this.currentFolderToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.currentFolderToolStripMenuItem.Text = "&current folder";
            this.currentFolderToolStripMenuItem.Click += new System.EventHandler(this.currentFolderToolStripMenuItem_Click);
            // 
            // addFolderFromSystemToolStripMenuItem
            // 
            this.addFolderFromSystemToolStripMenuItem.Name = "addFolderFromSystemToolStripMenuItem";
            this.addFolderFromSystemToolStripMenuItem.Size = new System.Drawing.Size(303, 24);
            this.addFolderFromSystemToolStripMenuItem.Text = "&Add files && folders from system";
            this.addFolderFromSystemToolStripMenuItem.Click += new System.EventHandler(this.addFolderFromSystemToolStripMenuItem_Click);
            // 
            // deleteFolderToolStripMenuItem
            // 
            this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
            this.deleteFolderToolStripMenuItem.Size = new System.Drawing.Size(303, 24);
            this.deleteFolderToolStripMenuItem.Text = "&Delete folder";
            this.deleteFolderToolStripMenuItem.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // renameFolderToolStripMenuItem
            // 
            this.renameFolderToolStripMenuItem.Name = "renameFolderToolStripMenuItem";
            this.renameFolderToolStripMenuItem.Size = new System.Drawing.Size(303, 24);
            this.renameFolderToolStripMenuItem.Text = "&Rename folder";
            this.renameFolderToolStripMenuItem.Click += new System.EventHandler(this.renameFolderToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem1
            // 
            this.propertiesToolStripMenuItem1.Name = "propertiesToolStripMenuItem1";
            this.propertiesToolStripMenuItem1.Size = new System.Drawing.Size(303, 24);
            this.propertiesToolStripMenuItem1.Text = "&Properties";
            this.propertiesToolStripMenuItem1.Click += new System.EventHandler(this.propertiesToolStripMenuItem1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lstFiles
            // 
            this.lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstFiles.ContextMenuStrip = this.cntxFileFiles;
            this.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.HideSelection = false;
            this.lstFiles.Location = new System.Drawing.Point(0, 0);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(401, 428);
            this.lstFiles.TabIndex = 0;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Source";
            this.columnHeader2.Width = 200;
            // 
            // cntxFileFiles
            // 
            this.cntxFileFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileToolStripMenuItem,
            this.addmultipleFilesToolStripMenuItem,
            this.deleteFileToolStripMenuItem,
            this.propertiesToolStripMenuItem});
            this.cntxFileFiles.Name = "cntxFileFiles";
            this.cntxFileFiles.Size = new System.Drawing.Size(203, 100);
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.addFileToolStripMenuItem.Text = "&Add file";
            this.addFileToolStripMenuItem.Click += new System.EventHandler(this.addFileToolStripMenuItem_Click);
            // 
            // addmultipleFilesToolStripMenuItem
            // 
            this.addmultipleFilesToolStripMenuItem.Name = "addmultipleFilesToolStripMenuItem";
            this.addmultipleFilesToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.addmultipleFilesToolStripMenuItem.Text = "Add &multiple files";
            this.addmultipleFilesToolStripMenuItem.Click += new System.EventHandler(this.addmultipleFilesToolStripMenuItem_Click);
            // 
            // deleteFileToolStripMenuItem
            // 
            this.deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            this.deleteFileToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.deleteFileToolStripMenuItem.Text = "&Delete file";
            this.deleteFileToolStripMenuItem.Click += new System.EventHandler(this.deleteFileToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.propertiesToolStripMenuItem.Text = "&Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtScript);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(613, 434);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Script";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtScript
            // 
            this.txtScript.AcceptsReturn = true;
            this.txtScript.AcceptsTab = true;
            this.txtScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScript.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScript.Location = new System.Drawing.Point(3, 3);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtScript.Size = new System.Drawing.Size(607, 428);
            this.txtScript.TabIndex = 0;
            this.txtScript.WordWrap = false;
            this.txtScript.TextChanged += new System.EventHandler(this.txtMULTI_TextChanged);
            // 
            // cmdDT2
            // 
            this.cmdDT2.Location = new System.Drawing.Point(499, 203);
            this.cmdDT2.Name = "cmdDT2";
            this.cmdDT2.Size = new System.Drawing.Size(34, 23);
            this.cmdDT2.TabIndex = 9;
            this.cmdDT2.Text = "dt";
            this.cmdDT2.UseVisualStyleBackColor = true;
            this.cmdDT2.Click += new System.EventHandler(this.cmdDT2_Click);
            // 
            // MainDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 488);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainDLG";
            this.Text = "Package Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDLG_FormClosing);
            this.Load += new System.EventHandler(this.MainDLG_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cntxFileFolder.ResumeLayout(false);
            this.cntxFileFiles.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveasToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeFileFolders;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip cntxFileFolder;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderFromSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameFolderToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cntxFileFiles;
        private System.Windows.Forms.ToolStripMenuItem addFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addmultipleFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtPackageDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPackageTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem rootFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentFolderToolStripMenuItem;
        private System.Windows.Forms.TextBox txtPackageID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdNewGUID;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdSelectOutputFilename;
        private System.Windows.Forms.TextBox txtOutputFilename;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.CheckBox chkNoReceipt;
        private System.Windows.Forms.ToolStripMenuItem testCompilePackageScriptToolStripMenuItem;
        private System.Windows.Forms.Button cmdDT;
        private System.Windows.Forms.Button cmdPlus1;
        private System.Windows.Forms.Button cmdDT2;
    }
}

