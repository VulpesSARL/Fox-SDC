namespace FoxSDC_PackageCreator
{
    partial class frmFileProperties
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDestPath = new System.Windows.Forms.TextBox();
            this.txtDestFilename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSrcFilename = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lstAction = new System.Windows.Forms.ComboBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.chkInstall = new System.Windows.Forms.CheckBox();
            this.chkKeepMeta = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(340, 205);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(421, 205);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Destination path:";
            // 
            // txtDestPath
            // 
            this.txtDestPath.Location = new System.Drawing.Point(123, 38);
            this.txtDestPath.Name = "txtDestPath";
            this.txtDestPath.Size = new System.Drawing.Size(373, 20);
            this.txtDestPath.TabIndex = 1;
            // 
            // txtDestFilename
            // 
            this.txtDestFilename.Location = new System.Drawing.Point(123, 64);
            this.txtDestFilename.Name = "txtDestFilename";
            this.txtDestFilename.Size = new System.Drawing.Size(373, 20);
            this.txtDestFilename.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Destination filename:";
            // 
            // txtSrcFilename
            // 
            this.txtSrcFilename.Location = new System.Drawing.Point(123, 90);
            this.txtSrcFilename.Name = "txtSrcFilename";
            this.txtSrcFilename.Size = new System.Drawing.Size(292, 20);
            this.txtSrcFilename.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Source filename:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Action:";
            // 
            // lstAction
            // 
            this.lstAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstAction.FormattingEnabled = true;
            this.lstAction.Location = new System.Drawing.Point(123, 116);
            this.lstAction.Name = "lstAction";
            this.lstAction.Size = new System.Drawing.Size(373, 21);
            this.lstAction.TabIndex = 5;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(123, 12);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(373, 20);
            this.txtID.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "ID:";
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(421, 88);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowse.TabIndex = 4;
            this.cmdBrowse.Text = "Browse";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // chkInstall
            // 
            this.chkInstall.AutoSize = true;
            this.chkInstall.Location = new System.Drawing.Point(123, 156);
            this.chkInstall.Name = "chkInstall";
            this.chkInstall.Size = new System.Drawing.Size(88, 17);
            this.chkInstall.TabIndex = 6;
            this.chkInstall.Text = "&Install this file";
            this.chkInstall.UseVisualStyleBackColor = true;
            // 
            // chkKeepMeta
            // 
            this.chkKeepMeta.AutoSize = true;
            this.chkKeepMeta.Location = new System.Drawing.Point(123, 179);
            this.chkKeepMeta.Name = "chkKeepMeta";
            this.chkKeepMeta.Size = new System.Drawing.Size(119, 17);
            this.chkKeepMeta.TabIndex = 11;
            this.chkKeepMeta.Text = "Include in Meta-&ZIP";
            this.chkKeepMeta.UseVisualStyleBackColor = true;
            // 
            // frmFileProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(508, 240);
            this.Controls.Add(this.chkKeepMeta);
            this.Controls.Add(this.chkInstall);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lstAction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSrcFilename);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDestFilename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDestPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFileProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Properties";
            this.Load += new System.EventHandler(this.frmFileProperties_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDestPath;
        private System.Windows.Forms.TextBox txtDestFilename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSrcFilename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox lstAction;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.CheckBox chkInstall;
        private System.Windows.Forms.CheckBox chkKeepMeta;
    }
}