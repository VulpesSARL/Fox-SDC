namespace FoxSDC_MGMT
{
    partial class ctlListPCs
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstComputers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.approveRefuseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setcommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.connectToScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCommandPromptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openRemoteDesktopConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstComputers
            // 
            this.lstComputers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader10,
            this.columnHeader13,
            this.columnHeader9,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader14,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader3,
            this.columnHeader11,
            this.columnHeader12});
            this.lstComputers.ContextMenuStrip = this.contextMenuStrip1;
            this.lstComputers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstComputers.FullRowSelect = true;
            this.lstComputers.HideSelection = false;
            this.lstComputers.Location = new System.Drawing.Point(0, 0);
            this.lstComputers.Name = "lstComputers";
            this.lstComputers.Size = new System.Drawing.Size(527, 377);
            this.lstComputers.SmallImageList = this.imageList1;
            this.lstComputers.TabIndex = 0;
            this.lstComputers.UseCompatibleStateImageBehavior = false;
            this.lstComputers.View = System.Windows.Forms.View.Details;
            this.lstComputers.DoubleClick += new System.EventHandler(this.lstComputers_DoubleClick);
            this.lstComputers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstComputers_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Computername";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Location";
            this.columnHeader10.Width = 150;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Comments";
            this.columnHeader13.Width = 150;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "State";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Operating System";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "OS Version";
            this.columnHeader4.Width = 70;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Win 10";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Platform";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Suite";
            this.columnHeader6.Width = 70;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Language";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Make";
            this.columnHeader8.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Last Updated";
            this.columnHeader3.Width = 200;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Contract ID";
            this.columnHeader11.Width = 200;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Agent Version";
            this.columnHeader12.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.approveRefuseToolStripMenuItem,
            this.setcommentToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.connectToScreenToolStripMenuItem,
            this.openCommandPromptToolStripMenuItem,
            this.openRemoteDesktopConnectionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(320, 206);
            // 
            // approveRefuseToolStripMenuItem
            // 
            this.approveRefuseToolStripMenuItem.Name = "approveRefuseToolStripMenuItem";
            this.approveRefuseToolStripMenuItem.Size = new System.Drawing.Size(319, 24);
            this.approveRefuseToolStripMenuItem.Text = "&Approve/Refuse";
            this.approveRefuseToolStripMenuItem.Click += new System.EventHandler(this.approveRefuseToolStripMenuItem_Click);
            // 
            // setcommentToolStripMenuItem
            // 
            this.setcommentToolStripMenuItem.Name = "setcommentToolStripMenuItem";
            this.setcommentToolStripMenuItem.Size = new System.Drawing.Size(319, 24);
            this.setcommentToolStripMenuItem.Text = "Set &comment";
            this.setcommentToolStripMenuItem.Click += new System.EventHandler(this.setcommentToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(319, 24);
            this.propertiesToolStripMenuItem.Text = "&Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(316, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(319, 24);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(316, 6);
            // 
            // connectToScreenToolStripMenuItem
            // 
            this.connectToScreenToolStripMenuItem.Name = "connectToScreenToolStripMenuItem";
            this.connectToScreenToolStripMenuItem.Size = new System.Drawing.Size(319, 24);
            this.connectToScreenToolStripMenuItem.Text = "&Connect to screen";
            this.connectToScreenToolStripMenuItem.Click += new System.EventHandler(this.connectToScreenToolStripMenuItem_Click);
            // 
            // openCommandPromptToolStripMenuItem
            // 
            this.openCommandPromptToolStripMenuItem.Name = "openCommandPromptToolStripMenuItem";
            this.openCommandPromptToolStripMenuItem.Size = new System.Drawing.Size(319, 24);
            this.openCommandPromptToolStripMenuItem.Text = "Open Command P&rompt";
            this.openCommandPromptToolStripMenuItem.Click += new System.EventHandler(this.openCommandPromptToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // openRemoteDesktopConnectionToolStripMenuItem
            // 
            this.openRemoteDesktopConnectionToolStripMenuItem.Name = "openRemoteDesktopConnectionToolStripMenuItem";
            this.openRemoteDesktopConnectionToolStripMenuItem.Size = new System.Drawing.Size(319, 24);
            this.openRemoteDesktopConnectionToolStripMenuItem.Text = "Open Remote &Desktop Connection";
            this.openRemoteDesktopConnectionToolStripMenuItem.Click += new System.EventHandler(this.openRemoteDesktopConnectionToolStripMenuItem_Click);
            // 
            // ctlListPCs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstComputers);
            this.Name = "ctlListPCs";
            this.Size = new System.Drawing.Size(527, 377);
            this.Load += new System.EventHandler(this.ctlUnapprovedPCs_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstComputers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem approveRefuseToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setcommentToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem connectToScreenToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ToolStripMenuItem openCommandPromptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRemoteDesktopConnectionToolStripMenuItem;
    }
}
