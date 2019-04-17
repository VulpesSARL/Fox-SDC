namespace FoxSDC_Agent_UI
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
            this.niIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cntxMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnwriteAmessageToVulpesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startChatWithVulpesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.showStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installOptionalPackagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invokeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.policiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdClose = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.cmdButton1 = new System.Windows.Forms.Button();
            this.cmdButton2 = new System.Windows.Forms.Button();
            this.cmdButton3 = new System.Windows.Forms.Button();
            this.timUpdate = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnSDC = new System.Windows.Forms.ToolStripMenuItem();
            this.writeMessageToVulpesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startChatWithVulpesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cntxMain.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // niIcon
            // 
            this.niIcon.ContextMenuStrip = this.cntxMain;
            this.niIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("niIcon.Icon")));
            this.niIcon.Visible = true;
            this.niIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niIcon_MouseDoubleClick);
            // 
            // cntxMain
            // 
            this.cntxMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnwriteAmessageToVulpesToolStripMenuItem,
            this.startChatWithVulpesToolStripMenuItem1,
            this.toolStripMenuItem1,
            this.showStatusToolStripMenuItem,
            this.installOptionalPackagesToolStripMenuItem,
            this.invokeToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.cntxMain.Name = "contextMenuStrip1";
            this.cntxMain.Size = new System.Drawing.Size(265, 178);
            this.cntxMain.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnwriteAmessageToVulpesToolStripMenuItem
            // 
            this.mnwriteAmessageToVulpesToolStripMenuItem.Name = "mnwriteAmessageToVulpesToolStripMenuItem";
            this.mnwriteAmessageToVulpesToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.mnwriteAmessageToVulpesToolStripMenuItem.Text = "Write a &message to Vulpes";
            this.mnwriteAmessageToVulpesToolStripMenuItem.Click += new System.EventHandler(this.writeAmessageToVulpesToolStripMenuItem_Click);
            // 
            // startChatWithVulpesToolStripMenuItem1
            // 
            this.startChatWithVulpesToolStripMenuItem1.Name = "startChatWithVulpesToolStripMenuItem1";
            this.startChatWithVulpesToolStripMenuItem1.Size = new System.Drawing.Size(264, 24);
            this.startChatWithVulpesToolStripMenuItem1.Text = "Start chat with Vulpes";
            this.startChatWithVulpesToolStripMenuItem1.Click += new System.EventHandler(this.startChatWithVulpesToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(261, 6);
            // 
            // showStatusToolStripMenuItem
            // 
            this.showStatusToolStripMenuItem.Name = "showStatusToolStripMenuItem";
            this.showStatusToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.showStatusToolStripMenuItem.Text = "&Show status";
            this.showStatusToolStripMenuItem.Click += new System.EventHandler(this.showStatusToolStripMenuItem_Click);
            // 
            // installOptionalPackagesToolStripMenuItem
            // 
            this.installOptionalPackagesToolStripMenuItem.Name = "installOptionalPackagesToolStripMenuItem";
            this.installOptionalPackagesToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.installOptionalPackagesToolStripMenuItem.Text = "&Install optional packages";
            this.installOptionalPackagesToolStripMenuItem.Click += new System.EventHandler(this.installOptionalPackagesToolStripMenuItem_Click);
            // 
            // invokeToolStripMenuItem
            // 
            this.invokeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.policiesToolStripMenuItem,
            this.reportingToolStripMenuItem});
            this.invokeToolStripMenuItem.Name = "invokeToolStripMenuItem";
            this.invokeToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.invokeToolStripMenuItem.Text = "&Resync ...";
            // 
            // policiesToolStripMenuItem
            // 
            this.policiesToolStripMenuItem.Name = "policiesToolStripMenuItem";
            this.policiesToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.policiesToolStripMenuItem.Text = "&Policies";
            this.policiesToolStripMenuItem.Click += new System.EventHandler(this.policiesToolStripMenuItem_Click);
            // 
            // reportingToolStripMenuItem
            // 
            this.reportingToolStripMenuItem.Name = "reportingToolStripMenuItem";
            this.reportingToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.reportingToolStripMenuItem.Text = "&Reporting";
            this.reportingToolStripMenuItem.Click += new System.EventHandler(this.reportingToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(376, 161);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 5;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 31);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(439, 95);
            this.txtMessage.TabIndex = 0;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 132);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(439, 23);
            this.progress.TabIndex = 1;
            // 
            // cmdButton1
            // 
            this.cmdButton1.Location = new System.Drawing.Point(12, 161);
            this.cmdButton1.Name = "cmdButton1";
            this.cmdButton1.Size = new System.Drawing.Size(75, 23);
            this.cmdButton1.TabIndex = 2;
            this.cmdButton1.Text = "button1";
            this.cmdButton1.UseVisualStyleBackColor = true;
            this.cmdButton1.Click += new System.EventHandler(this.cmdButton1_Click);
            // 
            // cmdButton2
            // 
            this.cmdButton2.Location = new System.Drawing.Point(93, 161);
            this.cmdButton2.Name = "cmdButton2";
            this.cmdButton2.Size = new System.Drawing.Size(75, 23);
            this.cmdButton2.TabIndex = 3;
            this.cmdButton2.Text = "button2";
            this.cmdButton2.UseVisualStyleBackColor = true;
            this.cmdButton2.Click += new System.EventHandler(this.cmdButton2_Click);
            // 
            // cmdButton3
            // 
            this.cmdButton3.Location = new System.Drawing.Point(174, 161);
            this.cmdButton3.Name = "cmdButton3";
            this.cmdButton3.Size = new System.Drawing.Size(75, 23);
            this.cmdButton3.TabIndex = 4;
            this.cmdButton3.Text = "button3";
            this.cmdButton3.UseVisualStyleBackColor = true;
            this.cmdButton3.Click += new System.EventHandler(this.cmdButton3_Click);
            // 
            // timUpdate
            // 
            this.timUpdate.Interval = 1000;
            this.timUpdate.Tick += new System.EventHandler(this.timUpdate_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnSDC});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(463, 28);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnSDC
            // 
            this.mnSDC.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.writeMessageToVulpesToolStripMenuItem,
            this.startChatWithVulpesToolStripMenuItem,
            this.toolStripMenuItem3,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem1,
            this.toolStripMenuItem2,
            this.closeToolStripMenuItem});
            this.mnSDC.Name = "mnSDC";
            this.mnSDC.Size = new System.Drawing.Size(49, 24);
            this.mnSDC.Text = "&SDC";
            this.mnSDC.DropDownOpening += new System.EventHandler(this.mnSDC_DropDownOpening);
            // 
            // writeMessageToVulpesToolStripMenuItem
            // 
            this.writeMessageToVulpesToolStripMenuItem.Name = "writeMessageToVulpesToolStripMenuItem";
            this.writeMessageToVulpesToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.writeMessageToVulpesToolStripMenuItem.Text = "Write &message to Vulpes";
            this.writeMessageToVulpesToolStripMenuItem.Click += new System.EventHandler(this.writeMessageToVulpesToolStripMenuItem_Click);
            // 
            // startChatWithVulpesToolStripMenuItem
            // 
            this.startChatWithVulpesToolStripMenuItem.Name = "startChatWithVulpesToolStripMenuItem";
            this.startChatWithVulpesToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.startChatWithVulpesToolStripMenuItem.Text = "&Start chat with Vulpes";
            this.startChatWithVulpesToolStripMenuItem.Click += new System.EventHandler(this.startChatWithVulpesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(249, 6);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(252, 24);
            this.aboutToolStripMenuItem1.Text = "&About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(249, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(252, 24);
            this.settingsToolStripMenuItem.Text = "S&ettings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // MainDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(463, 190);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.cmdButton3);
            this.Controls.Add(this.cmdButton2);
            this.Controls.Add(this.cmdButton1);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.cmdClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainDLG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Fox Software Deployment and Control Status";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDLG_FormClosing);
            this.Load += new System.EventHandler(this.MainDLG_Load);
            this.cntxMain.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon niIcon;
        private System.Windows.Forms.ToolStripMenuItem showStatusToolStripMenuItem;
        private System.Windows.Forms.Button cmdClose;
        internal System.Windows.Forms.TextBox txtMessage;
        internal System.Windows.Forms.ProgressBar progress;
        internal System.Windows.Forms.Button cmdButton1;
        internal System.Windows.Forms.Button cmdButton2;
        internal System.Windows.Forms.Button cmdButton3;
        private System.Windows.Forms.Timer timUpdate;
        private System.Windows.Forms.ToolStripMenuItem invokeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem policiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installOptionalPackagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnwriteAmessageToVulpesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnSDC;
        private System.Windows.Forms.ToolStripMenuItem writeMessageToVulpesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip cntxMain;
        private System.Windows.Forms.ToolStripMenuItem startChatWithVulpesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem startChatWithVulpesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}

