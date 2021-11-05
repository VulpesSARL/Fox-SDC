namespace FoxSDC_MGMT
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
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.uploadPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createCertificateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.createReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.showactiveUsersOnComputerInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Splitty = new System.Windows.Forms.SplitContainer();
            this.treeAction = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.createpolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.policyEnabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowLevelEditPolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer_ping = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.createsimpleTasksInThisGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Splitty)).BeginInit();
            this.Splitty.Panel1.SuspendLayout();
            this.Splitty.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serverToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(756, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(132, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.manageUsersToolStripMenuItem,
            this.toolStripMenuItem3,
            this.uploadPackageToolStripMenuItem,
            this.deletePackageToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.serverToolStripMenuItem.Text = "&Server";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(245, 24);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // manageUsersToolStripMenuItem
            // 
            this.manageUsersToolStripMenuItem.Name = "manageUsersToolStripMenuItem";
            this.manageUsersToolStripMenuItem.Size = new System.Drawing.Size(245, 24);
            this.manageUsersToolStripMenuItem.Text = "Manage u&sers";
            this.manageUsersToolStripMenuItem.Click += new System.EventHandler(this.manageUsersToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(242, 6);
            // 
            // uploadPackageToolStripMenuItem
            // 
            this.uploadPackageToolStripMenuItem.Name = "uploadPackageToolStripMenuItem";
            this.uploadPackageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.uploadPackageToolStripMenuItem.Size = new System.Drawing.Size(245, 24);
            this.uploadPackageToolStripMenuItem.Text = "&Upload Package";
            this.uploadPackageToolStripMenuItem.Click += new System.EventHandler(this.uploadPackageToolStripMenuItem_Click);
            // 
            // deletePackageToolStripMenuItem
            // 
            this.deletePackageToolStripMenuItem.Name = "deletePackageToolStripMenuItem";
            this.deletePackageToolStripMenuItem.Size = new System.Drawing.Size(245, 24);
            this.deletePackageToolStripMenuItem.Text = "&Delete Package";
            this.deletePackageToolStripMenuItem.Click += new System.EventHandler(this.deletePackageToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createCertificateToolStripMenuItem,
            this.toolStripMenuItem4,
            this.createReportToolStripMenuItem,
            this.runToolStripMenuItem,
            this.toolStripMenuItem5,
            this.showactiveUsersOnComputerInfoToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // createCertificateToolStripMenuItem
            // 
            this.createCertificateToolStripMenuItem.Name = "createCertificateToolStripMenuItem";
            this.createCertificateToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
            this.createCertificateToolStripMenuItem.Text = "&Create new certificate";
            this.createCertificateToolStripMenuItem.Click += new System.EventHandler(this.createCertificateToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(330, 6);
            // 
            // createReportToolStripMenuItem
            // 
            this.createReportToolStripMenuItem.Name = "createReportToolStripMenuItem";
            this.createReportToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
            this.createReportToolStripMenuItem.Text = "Create &report";
            this.createReportToolStripMenuItem.Click += new System.EventHandler(this.createReportToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
            this.runToolStripMenuItem.Text = "R&erun report";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(330, 6);
            // 
            // showactiveUsersOnComputerInfoToolStripMenuItem
            // 
            this.showactiveUsersOnComputerInfoToolStripMenuItem.Name = "showactiveUsersOnComputerInfoToolStripMenuItem";
            this.showactiveUsersOnComputerInfoToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
            this.showactiveUsersOnComputerInfoToolStripMenuItem.Text = "Show &active Users on Computer Info";
            this.showactiveUsersOnComputerInfoToolStripMenuItem.Click += new System.EventHandler(this.showactiveUsersOnComputerInfoToolStripMenuItem_Click);
            // 
            // Splitty
            // 
            this.Splitty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Splitty.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Splitty.Location = new System.Drawing.Point(0, 28);
            this.Splitty.Name = "Splitty";
            // 
            // Splitty.Panel1
            // 
            this.Splitty.Panel1.Controls.Add(this.treeAction);
            this.Splitty.Size = new System.Drawing.Size(756, 507);
            this.Splitty.SplitterDistance = 178;
            this.Splitty.TabIndex = 1;
            // 
            // treeAction
            // 
            this.treeAction.ContextMenuStrip = this.contextMenuStrip1;
            this.treeAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeAction.FullRowSelect = true;
            this.treeAction.HideSelection = false;
            this.treeAction.ImageIndex = 0;
            this.treeAction.ImageList = this.imageList1;
            this.treeAction.Location = new System.Drawing.Point(0, 0);
            this.treeAction.Name = "treeAction";
            this.treeAction.SelectedImageIndex = 0;
            this.treeAction.Size = new System.Drawing.Size(178, 507);
            this.treeAction.TabIndex = 0;
            this.treeAction.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeAction_BeforeExpand);
            this.treeAction.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeAction_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createGroupToolStripMenuItem,
            this.deleteGroupToolStripMenuItem,
            this.refreshGroupToolStripMenuItem,
            this.renameGroupToolStripMenuItem,
            this.toolStripMenuItem6,
            this.createsimpleTasksInThisGroupToolStripMenuItem,
            this.toolStripMenuItem2,
            this.createpolicyToolStripMenuItem,
            this.policyEnabledToolStripMenuItem,
            this.deletePolicyToolStripMenuItem,
            this.lowLevelEditPolicyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(307, 254);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // createGroupToolStripMenuItem
            // 
            this.createGroupToolStripMenuItem.Name = "createGroupToolStripMenuItem";
            this.createGroupToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.createGroupToolStripMenuItem.Text = "&Create group";
            this.createGroupToolStripMenuItem.Click += new System.EventHandler(this.createGroupToolStripMenuItem_Click);
            // 
            // deleteGroupToolStripMenuItem
            // 
            this.deleteGroupToolStripMenuItem.Name = "deleteGroupToolStripMenuItem";
            this.deleteGroupToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.deleteGroupToolStripMenuItem.Text = "&Delete group";
            this.deleteGroupToolStripMenuItem.Click += new System.EventHandler(this.deleteGroupToolStripMenuItem_Click);
            // 
            // refreshGroupToolStripMenuItem
            // 
            this.refreshGroupToolStripMenuItem.Name = "refreshGroupToolStripMenuItem";
            this.refreshGroupToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.refreshGroupToolStripMenuItem.Text = "R&efresh group";
            this.refreshGroupToolStripMenuItem.Click += new System.EventHandler(this.refreshGroupToolStripMenuItem_Click);
            // 
            // renameGroupToolStripMenuItem
            // 
            this.renameGroupToolStripMenuItem.Name = "renameGroupToolStripMenuItem";
            this.renameGroupToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.renameGroupToolStripMenuItem.Text = "&Rename group";
            this.renameGroupToolStripMenuItem.Click += new System.EventHandler(this.renameGroupToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(303, 6);
            // 
            // createpolicyToolStripMenuItem
            // 
            this.createpolicyToolStripMenuItem.Name = "createpolicyToolStripMenuItem";
            this.createpolicyToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.createpolicyToolStripMenuItem.Text = "Create &policy";
            this.createpolicyToolStripMenuItem.Click += new System.EventHandler(this.createpolicyToolStripMenuItem_Click);
            // 
            // policyEnabledToolStripMenuItem
            // 
            this.policyEnabledToolStripMenuItem.Name = "policyEnabledToolStripMenuItem";
            this.policyEnabledToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.policyEnabledToolStripMenuItem.Text = "Enable/Disable policy";
            this.policyEnabledToolStripMenuItem.Click += new System.EventHandler(this.policyEnabledToolStripMenuItem_Click);
            // 
            // deletePolicyToolStripMenuItem
            // 
            this.deletePolicyToolStripMenuItem.Name = "deletePolicyToolStripMenuItem";
            this.deletePolicyToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.deletePolicyToolStripMenuItem.Text = "Delete poli&cy";
            this.deletePolicyToolStripMenuItem.Click += new System.EventHandler(this.deletePolicyToolStripMenuItem_Click);
            // 
            // lowLevelEditPolicyToolStripMenuItem
            // 
            this.lowLevelEditPolicyToolStripMenuItem.Name = "lowLevelEditPolicyToolStripMenuItem";
            this.lowLevelEditPolicyToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.lowLevelEditPolicyToolStripMenuItem.Text = "Lo&w level edit policy";
            this.lowLevelEditPolicyToolStripMenuItem.Click += new System.EventHandler(this.lowLevelEditPolicyToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timer_ping
            // 
            this.timer_ping.Interval = 60000;
            this.timer_ping.Tick += new System.EventHandler(this.timer_ping_Tick);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(303, 6);
            // 
            // createsimpleTasksInThisGroupToolStripMenuItem
            // 
            this.createsimpleTasksInThisGroupToolStripMenuItem.Name = "createsimpleTasksInThisGroupToolStripMenuItem";
            this.createsimpleTasksInThisGroupToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.createsimpleTasksInThisGroupToolStripMenuItem.Text = "Create &simple tasks in this group";
            this.createsimpleTasksInThisGroupToolStripMenuItem.Click += new System.EventHandler(this.createsimpleTasksInThisGroupToolStripMenuItem_Click);
            // 
            // MainDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 535);
            this.Controls.Add(this.Splitty);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainDLG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fox SDC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDLG_FormClosing);
            this.Load += new System.EventHandler(this.MainDLG_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Splitty.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Splitty)).EndInit();
            this.Splitty.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer Splitty;
        private System.Windows.Forms.TreeView treeAction;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem createpolicyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem policyEnabledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePolicyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createCertificateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem uploadPackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePackageToolStripMenuItem;
        private System.Windows.Forms.Timer timer_ping;
        private System.Windows.Forms.ToolStripMenuItem lowLevelEditPolicyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem createReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageUsersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem showactiveUsersOnComputerInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem createsimpleTasksInThisGroupToolStripMenuItem;
    }
}

