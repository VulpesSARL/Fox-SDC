namespace FoxSDC_MGMT
{
    partial class frmRemoteBrowseFolder
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
            this.lstDrives = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDir = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstDirs = new FoxSDC_MGMT.NT3PathListBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lstDrives
            // 
            this.lstDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDrives.FormattingEnabled = true;
            this.lstDrives.Location = new System.Drawing.Point(12, 273);
            this.lstDrives.Name = "lstDrives";
            this.lstDrives.Size = new System.Drawing.Size(194, 21);
            this.lstDrives.TabIndex = 1;
            this.lstDrives.SelectedIndexChanged += new System.EventHandler(this.lstDrives_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 257);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Drive:";
            // 
            // lblDir
            // 
            this.lblDir.AutoEllipsis = true;
            this.lblDir.Location = new System.Drawing.Point(9, 26);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(197, 13);
            this.lblDir.TabIndex = 18;
            this.lblDir.Text = "C:\\Windows";
            this.lblDir.UseMnemonic = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Directories:";
            // 
            // lstDirs
            // 
            this.lstDirs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstDirs.FormattingEnabled = true;
            this.lstDirs.IntegralHeight = false;
            this.lstDirs.Location = new System.Drawing.Point(12, 49);
            this.lstDirs.Name = "lstDirs";
            this.lstDirs.ScrollAlwaysVisible = true;
            this.lstDirs.Size = new System.Drawing.Size(194, 195);
            this.lstDirs.TabIndex = 0;
            this.lstDirs.DoubleClick += new System.EventHandler(this.lstDirs_DoubleClick);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(131, 300);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(50, 300);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // frmRemoteBrowseFolder
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(221, 340);
            this.Controls.Add(this.lstDrives);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstDirs);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRemoteBrowseFolder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Folder";
            this.Load += new System.EventHandler(this.frmRemoteBrowseFolder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox lstDrives;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.Label label2;
        private NT3PathListBox lstDirs;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}