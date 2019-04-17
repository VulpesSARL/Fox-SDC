namespace FoxSDC_MGMT
{
    partial class frmAddFilesFromRemoteClient
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
            this.chkIgnoreMeteredConnection = new System.Windows.Forms.CheckBox();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.cmdSelectAll = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdDir = new System.Windows.Forms.Button();
            this.cmdAddFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkIgnoreMeteredConnection
            // 
            this.chkIgnoreMeteredConnection.AutoSize = true;
            this.chkIgnoreMeteredConnection.Location = new System.Drawing.Point(12, 271);
            this.chkIgnoreMeteredConnection.Name = "chkIgnoreMeteredConnection";
            this.chkIgnoreMeteredConnection.Size = new System.Drawing.Size(153, 17);
            this.chkIgnoreMeteredConnection.TabIndex = 16;
            this.chkIgnoreMeteredConnection.Text = "&Ignore metered connection";
            this.chkIgnoreMeteredConnection.UseVisualStyleBackColor = true;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Location = new System.Drawing.Point(255, 12);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(75, 23);
            this.cmdDelete.TabIndex = 13;
            this.cmdDelete.Text = "D&el";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // cmdSelectAll
            // 
            this.cmdSelectAll.Location = new System.Drawing.Point(174, 12);
            this.cmdSelectAll.Name = "cmdSelectAll";
            this.cmdSelectAll.Size = new System.Drawing.Size(75, 23);
            this.cmdSelectAll.TabIndex = 12;
            this.cmdSelectAll.Text = "Sel &All";
            this.cmdSelectAll.UseVisualStyleBackColor = true;
            this.cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(400, 314);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 18;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(319, 314);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 19;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.HideSelection = false;
            this.lstFiles.Location = new System.Drawing.Point(12, 41);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(463, 211);
            this.lstFiles.TabIndex = 15;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Source file (Remote)";
            this.columnHeader1.Width = 400;
            // 
            // cmdDir
            // 
            this.cmdDir.Location = new System.Drawing.Point(93, 12);
            this.cmdDir.Name = "cmdDir";
            this.cmdDir.Size = new System.Drawing.Size(75, 23);
            this.cmdDir.TabIndex = 10;
            this.cmdDir.Text = "Add &dir";
            this.cmdDir.UseVisualStyleBackColor = true;
            this.cmdDir.Click += new System.EventHandler(this.cmdDir_Click);
            // 
            // cmdAddFile
            // 
            this.cmdAddFile.Location = new System.Drawing.Point(12, 12);
            this.cmdAddFile.Name = "cmdAddFile";
            this.cmdAddFile.Size = new System.Drawing.Size(75, 23);
            this.cmdAddFile.TabIndex = 9;
            this.cmdAddFile.Text = "Add &file";
            this.cmdAddFile.UseVisualStyleBackColor = true;
            this.cmdAddFile.Click += new System.EventHandler(this.cmdAddFile_Click);
            // 
            // frmAddFilesFromRemoteClient
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(487, 349);
            this.Controls.Add(this.chkIgnoreMeteredConnection);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdSelectAll);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.cmdDir);
            this.Controls.Add(this.cmdAddFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddFilesFromRemoteClient";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddFilesFromRemoteClient";
            this.Load += new System.EventHandler(this.frmAddFilesFromRemoteClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIgnoreMeteredConnection;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Button cmdSelectAll;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button cmdDir;
        private System.Windows.Forms.Button cmdAddFile;
    }
}