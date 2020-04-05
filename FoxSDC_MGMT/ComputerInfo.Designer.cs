namespace FoxSDC_MGMT
{
    partial class frmComputerInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmComputerInfo));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PropertiesG = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cmdRemoteScreen = new System.Windows.Forms.Button();
            this.lblPing = new System.Windows.Forms.Label();
            this.picStatus = new System.Windows.Forms.PictureBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.Splitty = new System.Windows.Forms.SplitContainer();
            this.TVPolicies = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createpolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.policyEnabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowLevelEditPolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TVImgList = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstTasks = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdFRestart = new System.Windows.Forms.Button();
            this.cmdRestart = new System.Windows.Forms.Button();
            this.cmdRun = new System.Windows.Forms.Button();
            this.cmdKill = new System.Windows.Forms.Button();
            this.chkAutoRefreshTasks = new System.Windows.Forms.CheckBox();
            this.cmdRefreshTasks = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRemoteServer = new System.Windows.Forms.TextBox();
            this.lstRemotePort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lstWUUpdates = new System.Windows.Forms.ListView();
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdWUQuery = new System.Windows.Forms.Button();
            this.cmdWUGetList = new System.Windows.Forms.Button();
            this.lblWUStatus = new System.Windows.Forms.Label();
            this.chkWUAutoCheck = new System.Windows.Forms.CheckBox();
            this.lblWUReboot = new System.Windows.Forms.Label();
            this.cmdWUCheckUpdates = new System.Windows.Forms.Button();
            this.cmdWUInstallUpdates = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtRecvText = new System.Windows.Forms.TextBox();
            this.txtSendText = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cmdSend = new System.Windows.Forms.Button();
            this.bgwPinger = new System.ComponentModel.BackgroundWorker();
            this.timerPinger = new System.Windows.Forms.Timer(this.components);
            this.bgwListTasks = new System.ComponentModel.BackgroundWorker();
            this.timerTaskList = new System.Windows.Forms.Timer(this.components);
            this.timerWU = new System.Windows.Forms.Timer(this.components);
            this.bgwWUQuery = new System.ComponentModel.BackgroundWorker();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cmdClose = new System.Windows.Forms.Button();
            this.timChat = new System.Windows.Forms.Timer(this.components);
            this.cmdBootNext = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Splitty)).BeginInit();
            this.Splitty.Panel1.SuspendLayout();
            this.Splitty.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(749, 506);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PropertiesG);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(741, 480);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // PropertiesG
            // 
            this.PropertiesG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesG.HelpVisible = false;
            this.PropertiesG.LineColor = System.Drawing.SystemColors.ControlDark;
            this.PropertiesG.Location = new System.Drawing.Point(3, 39);
            this.PropertiesG.Name = "PropertiesG";
            this.PropertiesG.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertiesG.Size = new System.Drawing.Size(735, 438);
            this.PropertiesG.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.lblPing);
            this.panel1.Controls.Add(this.picStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 36);
            this.panel1.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cmdRemoteScreen);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(614, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(121, 36);
            this.panel6.TabIndex = 2;
            // 
            // cmdRemoteScreen
            // 
            this.cmdRemoteScreen.Location = new System.Drawing.Point(3, 4);
            this.cmdRemoteScreen.Name = "cmdRemoteScreen";
            this.cmdRemoteScreen.Size = new System.Drawing.Size(113, 23);
            this.cmdRemoteScreen.TabIndex = 0;
            this.cmdRemoteScreen.Text = "Connect to screen";
            this.cmdRemoteScreen.UseVisualStyleBackColor = true;
            this.cmdRemoteScreen.Click += new System.EventHandler(this.cmdRemoteScreen_Click);
            // 
            // lblPing
            // 
            this.lblPing.AutoSize = true;
            this.lblPing.Location = new System.Drawing.Point(37, 9);
            this.lblPing.Name = "lblPing";
            this.lblPing.Size = new System.Drawing.Size(19, 13);
            this.lblPing.TabIndex = 1;
            this.lblPing.Text = "----";
            // 
            // picStatus
            // 
            this.picStatus.Location = new System.Drawing.Point(2, 0);
            this.picStatus.Margin = new System.Windows.Forms.Padding(0);
            this.picStatus.Name = "picStatus";
            this.picStatus.Size = new System.Drawing.Size(32, 32);
            this.picStatus.TabIndex = 0;
            this.picStatus.TabStop = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.Splitty);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(741, 480);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Policies";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // Splitty
            // 
            this.Splitty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Splitty.Location = new System.Drawing.Point(3, 3);
            this.Splitty.Name = "Splitty";
            // 
            // Splitty.Panel1
            // 
            this.Splitty.Panel1.Controls.Add(this.TVPolicies);
            this.Splitty.Size = new System.Drawing.Size(735, 474);
            this.Splitty.SplitterDistance = 245;
            this.Splitty.TabIndex = 0;
            // 
            // TVPolicies
            // 
            this.TVPolicies.ContextMenuStrip = this.contextMenuStrip1;
            this.TVPolicies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TVPolicies.FullRowSelect = true;
            this.TVPolicies.ImageIndex = 0;
            this.TVPolicies.ImageList = this.TVImgList;
            this.TVPolicies.Location = new System.Drawing.Point(0, 0);
            this.TVPolicies.Name = "TVPolicies";
            this.TVPolicies.SelectedImageIndex = 0;
            this.TVPolicies.Size = new System.Drawing.Size(245, 474);
            this.TVPolicies.StateImageList = this.TVImgList;
            this.TVPolicies.TabIndex = 0;
            this.TVPolicies.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TVPolicies_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createpolicyToolStripMenuItem,
            this.policyEnabledToolStripMenuItem,
            this.deletePolicyToolStripMenuItem,
            this.lowLevelEditPolicyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(228, 100);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // createpolicyToolStripMenuItem
            // 
            this.createpolicyToolStripMenuItem.Name = "createpolicyToolStripMenuItem";
            this.createpolicyToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.createpolicyToolStripMenuItem.Text = "Create &policy";
            this.createpolicyToolStripMenuItem.Click += new System.EventHandler(this.createpolicyToolStripMenuItem_Click);
            // 
            // policyEnabledToolStripMenuItem
            // 
            this.policyEnabledToolStripMenuItem.Name = "policyEnabledToolStripMenuItem";
            this.policyEnabledToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.policyEnabledToolStripMenuItem.Text = "Enable/Disable policy";
            this.policyEnabledToolStripMenuItem.Click += new System.EventHandler(this.policyEnabledToolStripMenuItem_Click);
            // 
            // deletePolicyToolStripMenuItem
            // 
            this.deletePolicyToolStripMenuItem.Name = "deletePolicyToolStripMenuItem";
            this.deletePolicyToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.deletePolicyToolStripMenuItem.Text = "Delete poli&cy";
            this.deletePolicyToolStripMenuItem.Click += new System.EventHandler(this.deletePolicyToolStripMenuItem_Click);
            // 
            // lowLevelEditPolicyToolStripMenuItem
            // 
            this.lowLevelEditPolicyToolStripMenuItem.Name = "lowLevelEditPolicyToolStripMenuItem";
            this.lowLevelEditPolicyToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.lowLevelEditPolicyToolStripMenuItem.Text = "Lo&w level edit policy";
            this.lowLevelEditPolicyToolStripMenuItem.Click += new System.EventHandler(this.lowLevelEditPolicyToolStripMenuItem_Click);
            // 
            // TVImgList
            // 
            this.TVImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.TVImgList.ImageSize = new System.Drawing.Size(16, 16);
            this.TVImgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstTasks);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(741, 480);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Task Manager";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstTasks
            // 
            this.lstTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
            this.lstTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTasks.FullRowSelect = true;
            this.lstTasks.HideSelection = false;
            this.lstTasks.Location = new System.Drawing.Point(3, 39);
            this.lstTasks.MultiSelect = false;
            this.lstTasks.Name = "lstTasks";
            this.lstTasks.Size = new System.Drawing.Size(735, 438);
            this.lstTasks.TabIndex = 4;
            this.lstTasks.UseCompatibleStateImageBehavior = false;
            this.lstTasks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "32/64 Bit";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "PID";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Private bytes";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Working set";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Description";
            this.columnHeader6.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Company";
            this.columnHeader7.Width = 150;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Username";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Filename";
            this.columnHeader9.Width = 150;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Arguments";
            this.columnHeader10.Width = 150;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "CPU Total Time";
            this.columnHeader11.Width = 100;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "CPU User Time";
            this.columnHeader12.Width = 100;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmdBootNext);
            this.panel2.Controls.Add(this.cmdFRestart);
            this.panel2.Controls.Add(this.cmdRestart);
            this.panel2.Controls.Add(this.cmdRun);
            this.panel2.Controls.Add(this.cmdKill);
            this.panel2.Controls.Add(this.chkAutoRefreshTasks);
            this.panel2.Controls.Add(this.cmdRefreshTasks);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(735, 36);
            this.panel2.TabIndex = 3;
            // 
            // cmdFRestart
            // 
            this.cmdFRestart.Location = new System.Drawing.Point(646, 3);
            this.cmdFRestart.Name = "cmdFRestart";
            this.cmdFRestart.Size = new System.Drawing.Size(75, 23);
            this.cmdFRestart.TabIndex = 6;
            this.cmdFRestart.Text = "FRestart";
            this.cmdFRestart.UseVisualStyleBackColor = true;
            this.cmdFRestart.Click += new System.EventHandler(this.cmdFRestart_Click);
            // 
            // cmdRestart
            // 
            this.cmdRestart.Location = new System.Drawing.Point(565, 3);
            this.cmdRestart.Name = "cmdRestart";
            this.cmdRestart.Size = new System.Drawing.Size(75, 23);
            this.cmdRestart.TabIndex = 5;
            this.cmdRestart.Text = "Restart";
            this.cmdRestart.UseVisualStyleBackColor = true;
            this.cmdRestart.Click += new System.EventHandler(this.cmdRestart_Click);
            // 
            // cmdRun
            // 
            this.cmdRun.Location = new System.Drawing.Point(356, 3);
            this.cmdRun.Name = "cmdRun";
            this.cmdRun.Size = new System.Drawing.Size(75, 23);
            this.cmdRun.TabIndex = 3;
            this.cmdRun.Text = "&Run";
            this.cmdRun.UseVisualStyleBackColor = true;
            this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
            // 
            // cmdKill
            // 
            this.cmdKill.Location = new System.Drawing.Point(275, 3);
            this.cmdKill.Name = "cmdKill";
            this.cmdKill.Size = new System.Drawing.Size(75, 23);
            this.cmdKill.TabIndex = 2;
            this.cmdKill.Text = "&Kill";
            this.cmdKill.UseVisualStyleBackColor = true;
            this.cmdKill.Click += new System.EventHandler(this.cmdKill_Click);
            // 
            // chkAutoRefreshTasks
            // 
            this.chkAutoRefreshTasks.AutoSize = true;
            this.chkAutoRefreshTasks.Location = new System.Drawing.Point(113, 7);
            this.chkAutoRefreshTasks.Name = "chkAutoRefreshTasks";
            this.chkAutoRefreshTasks.Size = new System.Drawing.Size(108, 17);
            this.chkAutoRefreshTasks.TabIndex = 1;
            this.chkAutoRefreshTasks.Text = "&Automatic refresh";
            this.chkAutoRefreshTasks.UseVisualStyleBackColor = true;
            this.chkAutoRefreshTasks.CheckedChanged += new System.EventHandler(this.chkAutoRefreshTasks_CheckedChanged);
            // 
            // cmdRefreshTasks
            // 
            this.cmdRefreshTasks.Location = new System.Drawing.Point(5, 3);
            this.cmdRefreshTasks.Name = "cmdRefreshTasks";
            this.cmdRefreshTasks.Size = new System.Drawing.Size(75, 23);
            this.cmdRefreshTasks.TabIndex = 0;
            this.cmdRefreshTasks.Text = "&Refresh";
            this.cmdRefreshTasks.UseVisualStyleBackColor = true;
            this.cmdRefreshTasks.Click += new System.EventHandler(this.cmdRefreshTasks_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.txtRemoteServer);
            this.tabPage3.Controls.Add(this.lstRemotePort);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.cmdConnect);
            this.tabPage3.Controls.Add(this.txtLocalPort);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(741, 480);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Remote Connection";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Remote Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Remote Destination:";
            // 
            // txtRemoteServer
            // 
            this.txtRemoteServer.Location = new System.Drawing.Point(137, 74);
            this.txtRemoteServer.Name = "txtRemoteServer";
            this.txtRemoteServer.Size = new System.Drawing.Size(209, 20);
            this.txtRemoteServer.TabIndex = 1;
            // 
            // lstRemotePort
            // 
            this.lstRemotePort.FormattingEnabled = true;
            this.lstRemotePort.Location = new System.Drawing.Point(137, 100);
            this.lstRemotePort.Name = "lstRemotePort";
            this.lstRemotePort.Size = new System.Drawing.Size(209, 21);
            this.lstRemotePort.Sorted = true;
            this.lstRemotePort.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Local Port:";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(271, 127);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(75, 23);
            this.cmdConnect.TabIndex = 3;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(137, 22);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(209, 20);
            this.txtLocalPort.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lstWUUpdates);
            this.tabPage4.Controls.Add(this.panel3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(741, 480);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Windows Update";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lstWUUpdates
            // 
            this.lstWUUpdates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16});
            this.lstWUUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstWUUpdates.FullRowSelect = true;
            this.lstWUUpdates.HideSelection = false;
            this.lstWUUpdates.Location = new System.Drawing.Point(3, 137);
            this.lstWUUpdates.MultiSelect = false;
            this.lstWUUpdates.Name = "lstWUUpdates";
            this.lstWUUpdates.Size = new System.Drawing.Size(735, 340);
            this.lstWUUpdates.TabIndex = 5;
            this.lstWUUpdates.UseCompatibleStateImageBehavior = false;
            this.lstWUUpdates.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "ID";
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Title";
            this.columnHeader14.Width = 200;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Description";
            this.columnHeader15.Width = 100;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Link";
            this.columnHeader16.Width = 100;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.cmdWUQuery);
            this.panel3.Controls.Add(this.cmdWUGetList);
            this.panel3.Controls.Add(this.lblWUStatus);
            this.panel3.Controls.Add(this.chkWUAutoCheck);
            this.panel3.Controls.Add(this.lblWUReboot);
            this.panel3.Controls.Add(this.cmdWUCheckUpdates);
            this.panel3.Controls.Add(this.cmdWUInstallUpdates);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(735, 134);
            this.panel3.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Status:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Reboot:";
            // 
            // cmdWUQuery
            // 
            this.cmdWUQuery.Location = new System.Drawing.Point(517, 4);
            this.cmdWUQuery.Name = "cmdWUQuery";
            this.cmdWUQuery.Size = new System.Drawing.Size(75, 23);
            this.cmdWUQuery.TabIndex = 0;
            this.cmdWUQuery.Text = "Refresh";
            this.cmdWUQuery.UseVisualStyleBackColor = true;
            this.cmdWUQuery.Click += new System.EventHandler(this.cmdWUQuery_Click);
            // 
            // cmdWUGetList
            // 
            this.cmdWUGetList.Location = new System.Drawing.Point(200, 66);
            this.cmdWUGetList.Name = "cmdWUGetList";
            this.cmdWUGetList.Size = new System.Drawing.Size(133, 23);
            this.cmdWUGetList.TabIndex = 3;
            this.cmdWUGetList.Text = "Get List";
            this.cmdWUGetList.UseVisualStyleBackColor = true;
            this.cmdWUGetList.Click += new System.EventHandler(this.cmdWUGetList_Click);
            // 
            // lblWUStatus
            // 
            this.lblWUStatus.Location = new System.Drawing.Point(58, 9);
            this.lblWUStatus.Name = "lblWUStatus";
            this.lblWUStatus.Size = new System.Drawing.Size(453, 41);
            this.lblWUStatus.TabIndex = 2;
            this.lblWUStatus.Text = "-----";
            // 
            // chkWUAutoCheck
            // 
            this.chkWUAutoCheck.AutoSize = true;
            this.chkWUAutoCheck.Location = new System.Drawing.Point(517, 33);
            this.chkWUAutoCheck.Name = "chkWUAutoCheck";
            this.chkWUAutoCheck.Size = new System.Drawing.Size(108, 17);
            this.chkWUAutoCheck.TabIndex = 1;
            this.chkWUAutoCheck.Text = "Automatic refresh";
            this.chkWUAutoCheck.UseVisualStyleBackColor = true;
            this.chkWUAutoCheck.CheckedChanged += new System.EventHandler(this.chkWUAutoCheck_CheckedChanged);
            // 
            // lblWUReboot
            // 
            this.lblWUReboot.AutoSize = true;
            this.lblWUReboot.Location = new System.Drawing.Point(58, 50);
            this.lblWUReboot.Name = "lblWUReboot";
            this.lblWUReboot.Size = new System.Drawing.Size(19, 13);
            this.lblWUReboot.TabIndex = 6;
            this.lblWUReboot.Text = "----";
            // 
            // cmdWUCheckUpdates
            // 
            this.cmdWUCheckUpdates.Location = new System.Drawing.Point(61, 66);
            this.cmdWUCheckUpdates.Name = "cmdWUCheckUpdates";
            this.cmdWUCheckUpdates.Size = new System.Drawing.Size(133, 23);
            this.cmdWUCheckUpdates.TabIndex = 2;
            this.cmdWUCheckUpdates.Text = "Check Updates";
            this.cmdWUCheckUpdates.UseVisualStyleBackColor = true;
            this.cmdWUCheckUpdates.Click += new System.EventHandler(this.cmdWUCheckUpdates_Click);
            // 
            // cmdWUInstallUpdates
            // 
            this.cmdWUInstallUpdates.Location = new System.Drawing.Point(61, 95);
            this.cmdWUInstallUpdates.Name = "cmdWUInstallUpdates";
            this.cmdWUInstallUpdates.Size = new System.Drawing.Size(133, 23);
            this.cmdWUInstallUpdates.TabIndex = 4;
            this.cmdWUInstallUpdates.Text = "Install Updates";
            this.cmdWUInstallUpdates.UseVisualStyleBackColor = true;
            this.cmdWUInstallUpdates.Click += new System.EventHandler(this.cmdWUInstallUpdates_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage6.Controls.Add(this.splitContainer1);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(741, 480);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Chat";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtRecvText);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtSendText);
            this.splitContainer1.Panel2.Controls.Add(this.panel7);
            this.splitContainer1.Size = new System.Drawing.Size(735, 474);
            this.splitContainer1.SplitterDistance = 397;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtRecvText
            // 
            this.txtRecvText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecvText.Location = new System.Drawing.Point(0, 0);
            this.txtRecvText.Multiline = true;
            this.txtRecvText.Name = "txtRecvText";
            this.txtRecvText.ReadOnly = true;
            this.txtRecvText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecvText.Size = new System.Drawing.Size(735, 397);
            this.txtRecvText.TabIndex = 0;
            // 
            // txtSendText
            // 
            this.txtSendText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendText.Location = new System.Drawing.Point(0, 0);
            this.txtSendText.Multiline = true;
            this.txtSendText.Name = "txtSendText";
            this.txtSendText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSendText.Size = new System.Drawing.Size(670, 73);
            this.txtSendText.TabIndex = 0;
            this.txtSendText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSendText_KeyDown);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.cmdSend);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(670, 0);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(10);
            this.panel7.Size = new System.Drawing.Size(65, 73);
            this.panel7.TabIndex = 1;
            // 
            // cmdSend
            // 
            this.cmdSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdSend.Location = new System.Drawing.Point(10, 10);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(45, 53);
            this.cmdSend.TabIndex = 0;
            this.cmdSend.Text = "&Send";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // bgwPinger
            // 
            this.bgwPinger.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwPinger_DoWork);
            // 
            // timerPinger
            // 
            this.timerPinger.Enabled = true;
            this.timerPinger.Interval = 5000;
            this.timerPinger.Tick += new System.EventHandler(this.timerPinger_Tick);
            // 
            // bgwListTasks
            // 
            this.bgwListTasks.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwListTasks_DoWork);
            // 
            // timerTaskList
            // 
            this.timerTaskList.Interval = 2000;
            this.timerTaskList.Tick += new System.EventHandler(this.timerTaskList_Tick);
            // 
            // timerWU
            // 
            this.timerWU.Interval = 2000;
            this.timerWU.Tick += new System.EventHandler(this.timerWU_Tick);
            // 
            // bgwWUQuery
            // 
            this.bgwWUQuery.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwWUQuery_DoWork);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 506);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(749, 25);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cmdClose);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(671, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(78, 25);
            this.panel5.TabIndex = 0;
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(0, 0);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 0;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // timChat
            // 
            this.timChat.Enabled = true;
            this.timChat.Interval = 1000;
            this.timChat.Tick += new System.EventHandler(this.timChat_Tick);
            // 
            // cmdBootNext
            // 
            this.cmdBootNext.Location = new System.Drawing.Point(484, 3);
            this.cmdBootNext.Name = "cmdBootNext";
            this.cmdBootNext.Size = new System.Drawing.Size(75, 23);
            this.cmdBootNext.TabIndex = 4;
            this.cmdBootNext.Text = "BootNext";
            this.cmdBootNext.UseVisualStyleBackColor = true;
            this.cmdBootNext.Click += new System.EventHandler(this.cmdBootNext_Click);
            // 
            // frmComputerInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(749, 531);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmComputerInfo";
            this.Text = "ComputerInfo";
            this.Load += new System.EventHandler(this.frmComputerInfo_Load);
            this.Shown += new System.EventHandler(this.frmComputerInfo_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.Splitty.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Splitty)).EndInit();
            this.Splitty.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PropertyGrid PropertiesG;
        private System.Windows.Forms.TabPage tabPage2;
        private System.ComponentModel.BackgroundWorker bgwPinger;
        private System.Windows.Forms.Timer timerPinger;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picStatus;
        private System.Windows.Forms.Label lblPing;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lstTasks;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.CheckBox chkAutoRefreshTasks;
        private System.Windows.Forms.Button cmdRefreshTasks;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.ComponentModel.BackgroundWorker bgwListTasks;
        private System.Windows.Forms.Timer timerTaskList;
        private System.Windows.Forms.Button cmdKill;
        private System.Windows.Forms.Button cmdRun;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRemoteServer;
        private System.Windows.Forms.ComboBox lstRemotePort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.TextBox txtLocalPort;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label lblWUStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdWUQuery;
        private System.Windows.Forms.CheckBox chkWUAutoCheck;
        private System.Windows.Forms.Timer timerWU;
        private System.Windows.Forms.Label lblWUReboot;
        private System.Windows.Forms.Button cmdWUInstallUpdates;
        private System.Windows.Forms.Button cmdWUCheckUpdates;
        private System.Windows.Forms.ListView lstWUUpdates;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.Button cmdFRestart;
        private System.Windows.Forms.Button cmdRestart;
        private System.Windows.Forms.Button cmdWUGetList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker bgwWUQuery;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.SplitContainer Splitty;
        private System.Windows.Forms.TreeView TVPolicies;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ImageList TVImgList;
        private System.Windows.Forms.ToolStripMenuItem createpolicyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem policyEnabledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePolicyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowLevelEditPolicyToolStripMenuItem;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button cmdRemoteScreen;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtRecvText;
        private System.Windows.Forms.TextBox txtSendText;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.Timer timChat;
        private System.Windows.Forms.Button cmdBootNext;
    }
}