namespace FoxSDC_MGMT
{
    partial class frmAddFilesToRemoteClient
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
            this.cmdAddFile = new System.Windows.Forms.Button();
            this.cmdDir = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalSize = new System.Windows.Forms.Label();
            this.cmdChgRDir = new System.Windows.Forms.Button();
            this.cmdSelectAll = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.chkIgnoreMeteredConnection = new System.Windows.Forms.CheckBox();
            this.chkExecuteWhenDone = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdAddFile
            // 
            this.cmdAddFile.Location = new System.Drawing.Point(12, 12);
            this.cmdAddFile.Name = "cmdAddFile";
            this.cmdAddFile.Size = new System.Drawing.Size(75, 23);
            this.cmdAddFile.TabIndex = 0;
            this.cmdAddFile.Text = "Add &file";
            this.cmdAddFile.UseVisualStyleBackColor = true;
            this.cmdAddFile.Click += new System.EventHandler(this.cmdAddFile_Click);
            // 
            // cmdDir
            // 
            this.cmdDir.Location = new System.Drawing.Point(93, 12);
            this.cmdDir.Name = "cmdDir";
            this.cmdDir.Size = new System.Drawing.Size(75, 23);
            this.cmdDir.TabIndex = 1;
            this.cmdDir.Text = "Add &dir";
            this.cmdDir.UseVisualStyleBackColor = true;
            this.cmdDir.Click += new System.EventHandler(this.cmdDir_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.HideSelection = false;
            this.lstFiles.Location = new System.Drawing.Point(12, 41);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(463, 211);
            this.lstFiles.TabIndex = 5;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Source file (Local)";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Destination File (Remote)";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Size";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(319, 314);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 10;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(400, 314);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 9;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Total size:";
            // 
            // lblTotalSize
            // 
            this.lblTotalSize.AutoSize = true;
            this.lblTotalSize.Location = new System.Drawing.Point(70, 255);
            this.lblTotalSize.Name = "lblTotalSize";
            this.lblTotalSize.Size = new System.Drawing.Size(19, 13);
            this.lblTotalSize.TabIndex = 6;
            this.lblTotalSize.Text = "----";
            // 
            // cmdChgRDir
            // 
            this.cmdChgRDir.Location = new System.Drawing.Point(174, 12);
            this.cmdChgRDir.Name = "cmdChgRDir";
            this.cmdChgRDir.Size = new System.Drawing.Size(75, 23);
            this.cmdChgRDir.TabIndex = 2;
            this.cmdChgRDir.Text = "Chg &RDir";
            this.cmdChgRDir.UseVisualStyleBackColor = true;
            this.cmdChgRDir.Click += new System.EventHandler(this.cmdChgRDir_Click);
            // 
            // cmdSelectAll
            // 
            this.cmdSelectAll.Location = new System.Drawing.Point(255, 12);
            this.cmdSelectAll.Name = "cmdSelectAll";
            this.cmdSelectAll.Size = new System.Drawing.Size(75, 23);
            this.cmdSelectAll.TabIndex = 3;
            this.cmdSelectAll.Text = "Sel &All";
            this.cmdSelectAll.UseVisualStyleBackColor = true;
            this.cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Location = new System.Drawing.Point(336, 12);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(75, 23);
            this.cmdDelete.TabIndex = 4;
            this.cmdDelete.Text = "D&el";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // chkIgnoreMeteredConnection
            // 
            this.chkIgnoreMeteredConnection.AutoSize = true;
            this.chkIgnoreMeteredConnection.Location = new System.Drawing.Point(12, 271);
            this.chkIgnoreMeteredConnection.Name = "chkIgnoreMeteredConnection";
            this.chkIgnoreMeteredConnection.Size = new System.Drawing.Size(153, 17);
            this.chkIgnoreMeteredConnection.TabIndex = 7;
            this.chkIgnoreMeteredConnection.Text = "&Ignore metered connection";
            this.chkIgnoreMeteredConnection.UseVisualStyleBackColor = true;
            // 
            // chkExecuteWhenDone
            // 
            this.chkExecuteWhenDone.AutoSize = true;
            this.chkExecuteWhenDone.Location = new System.Drawing.Point(12, 294);
            this.chkExecuteWhenDone.Name = "chkExecuteWhenDone";
            this.chkExecuteWhenDone.Size = new System.Drawing.Size(363, 17);
            this.chkExecuteWhenDone.TabIndex = 8;
            this.chkExecuteWhenDone.Text = "&Execute file (after successfull download) (only EXE files; DANGEROUS)";
            this.chkExecuteWhenDone.UseVisualStyleBackColor = true;
            // 
            // frmAddFilesToRemoteClient
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(487, 349);
            this.Controls.Add(this.chkExecuteWhenDone);
            this.Controls.Add(this.chkIgnoreMeteredConnection);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdSelectAll);
            this.Controls.Add(this.cmdChgRDir);
            this.Controls.Add(this.lblTotalSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.cmdDir);
            this.Controls.Add(this.cmdAddFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddFilesToRemoteClient";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddFilesToRemoteClient";
            this.Load += new System.EventHandler(this.frmAddFilesToRemoteClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdAddFile;
        private System.Windows.Forms.Button cmdDir;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalSize;
        private System.Windows.Forms.Button cmdChgRDir;
        private System.Windows.Forms.Button cmdSelectAll;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.CheckBox chkIgnoreMeteredConnection;
        private System.Windows.Forms.CheckBox chkExecuteWhenDone;
    }
}