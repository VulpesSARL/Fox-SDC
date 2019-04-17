namespace FoxSDC_MGMT
{
    partial class ctlFileTransfer
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
            this.lstFiles = new System.Windows.Forms.ListView();
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel8 = new System.Windows.Forms.Panel();
            this.cmdTransferFromAgent = new System.Windows.Forms.Button();
            this.cmdCancelTrans = new System.Windows.Forms.Button();
            this.cmdToClient = new System.Windows.Forms.Button();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.cmdTransferToHere = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel8.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstFiles
            // 
            this.lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2});
            this.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.HideSelection = false;
            this.lstFiles.Location = new System.Drawing.Point(0, 0);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(754, 422);
            this.lstFiles.SmallImageList = this.imageList1;
            this.lstFiles.TabIndex = 0;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "File";
            this.columnHeader17.Width = 300;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Size";
            this.columnHeader18.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Progress";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Direction";
            this.columnHeader4.Width = 120;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Computer";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Group";
            this.columnHeader2.Width = 120;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel1);
            this.panel8.Controls.Add(this.cmdTransferToHere);
            this.panel8.Controls.Add(this.cmdTransferFromAgent);
            this.panel8.Controls.Add(this.cmdToClient);
            this.panel8.Controls.Add(this.cmdRefresh);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 422);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(754, 35);
            this.panel8.TabIndex = 1;
            // 
            // cmdTransferFromAgent
            // 
            this.cmdTransferFromAgent.Location = new System.Drawing.Point(233, 6);
            this.cmdTransferFromAgent.Name = "cmdTransferFromAgent";
            this.cmdTransferFromAgent.Size = new System.Drawing.Size(114, 23);
            this.cmdTransferFromAgent.TabIndex = 2;
            this.cmdTransferFromAgent.Text = "Transfer &from client";
            this.cmdTransferFromAgent.UseVisualStyleBackColor = true;
            this.cmdTransferFromAgent.Click += new System.EventHandler(this.cmdTransferFromAgent_Click);
            // 
            // cmdCancelTrans
            // 
            this.cmdCancelTrans.Location = new System.Drawing.Point(3, 6);
            this.cmdCancelTrans.Name = "cmdCancelTrans";
            this.cmdCancelTrans.Size = new System.Drawing.Size(114, 23);
            this.cmdCancelTrans.TabIndex = 0;
            this.cmdCancelTrans.Text = "Cancel &transaction";
            this.cmdCancelTrans.UseVisualStyleBackColor = true;
            this.cmdCancelTrans.Click += new System.EventHandler(this.cmdCancelTrans_Click);
            // 
            // cmdToClient
            // 
            this.cmdToClient.Location = new System.Drawing.Point(113, 6);
            this.cmdToClient.Name = "cmdToClient";
            this.cmdToClient.Size = new System.Drawing.Size(114, 23);
            this.cmdToClient.TabIndex = 1;
            this.cmdToClient.Text = "Transfer to &client";
            this.cmdToClient.UseVisualStyleBackColor = true;
            this.cmdToClient.Click += new System.EventHandler(this.cmdToClient_Click);
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Location = new System.Drawing.Point(3, 6);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(75, 23);
            this.cmdRefresh.TabIndex = 0;
            this.cmdRefresh.Text = "&Refresh";
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // cmdTransferToHere
            // 
            this.cmdTransferToHere.Location = new System.Drawing.Point(353, 6);
            this.cmdTransferToHere.Name = "cmdTransferToHere";
            this.cmdTransferToHere.Size = new System.Drawing.Size(114, 23);
            this.cmdTransferToHere.TabIndex = 3;
            this.cmdTransferToHere.Text = "Transfer to &here";
            this.cmdTransferToHere.UseVisualStyleBackColor = true;
            this.cmdTransferToHere.Click += new System.EventHandler(this.cmdTransferToHere_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdCancelTrans);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(632, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(122, 35);
            this.panel1.TabIndex = 4;
            // 
            // ctlFileTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.panel8);
            this.Name = "ctlFileTransfer";
            this.Size = new System.Drawing.Size(754, 457);
            this.Load += new System.EventHandler(this.ctlFileTransfer_Load);
            this.panel8.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button cmdRefresh;
        private System.Windows.Forms.Button cmdToClient;
        private System.Windows.Forms.Button cmdCancelTrans;
        private System.Windows.Forms.Button cmdTransferFromAgent;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdTransferToHere;
    }
}
