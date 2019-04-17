namespace FoxSDC_MGMT
{
    partial class ctlUploadDownloadStatus
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
            this.lstEntries = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tim = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetFailedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelUploaddownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstEntries
            // 
            this.lstEntries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader7,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader8});
            this.lstEntries.ContextMenuStrip = this.contextMenuStrip1;
            this.lstEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstEntries.FullRowSelect = true;
            this.lstEntries.HideSelection = false;
            this.lstEntries.Location = new System.Drawing.Point(0, 0);
            this.lstEntries.Name = "lstEntries";
            this.lstEntries.Size = new System.Drawing.Size(632, 519);
            this.lstEntries.SmallImageList = this.imageList1;
            this.lstEntries.TabIndex = 0;
            this.lstEntries.UseCompatibleStateImageBehavior = false;
            this.lstEntries.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Direction";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Computer";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Group";
            this.columnHeader7.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Local filename";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Remote filename";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Size";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Progress Size";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Error";
            this.columnHeader8.Width = 150;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tim
            // 
            this.tim.Interval = 1000;
            this.tim.Tick += new System.EventHandler(this.tim_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetFailedToolStripMenuItem,
            this.cancelUploaddownloadToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(252, 52);
            // 
            // resetFailedToolStripMenuItem
            // 
            this.resetFailedToolStripMenuItem.Name = "resetFailedToolStripMenuItem";
            this.resetFailedToolStripMenuItem.Size = new System.Drawing.Size(251, 24);
            this.resetFailedToolStripMenuItem.Text = "&Reset failed";
            this.resetFailedToolStripMenuItem.Click += new System.EventHandler(this.resetFailedToolStripMenuItem_Click);
            // 
            // cancelUploaddownloadToolStripMenuItem
            // 
            this.cancelUploaddownloadToolStripMenuItem.Name = "cancelUploaddownloadToolStripMenuItem";
            this.cancelUploaddownloadToolStripMenuItem.Size = new System.Drawing.Size(251, 24);
            this.cancelUploaddownloadToolStripMenuItem.Text = "&Cancel upload/download";
            this.cancelUploaddownloadToolStripMenuItem.Click += new System.EventHandler(this.cancelUploaddownloadToolStripMenuItem_Click);
            // 
            // ctlUploadDownloadStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstEntries);
            this.Name = "ctlUploadDownloadStatus";
            this.Size = new System.Drawing.Size(632, 519);
            this.Load += new System.EventHandler(this.ctlUploadDownloadStatus_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstEntries;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Timer tim;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem resetFailedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelUploaddownloadToolStripMenuItem;
    }
}
