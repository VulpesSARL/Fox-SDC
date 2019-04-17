namespace FoxSDC_PackageCreator
{
    partial class frmCompilePackage
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
            this.label4 = new System.Windows.Forms.Label();
            this.cmdSelectOutputFilename = new System.Windows.Forms.Button();
            this.txtOutputFilename = new System.Windows.Forms.TextBox();
            this.radCertSign = new System.Windows.Forms.RadioButton();
            this.lstCert = new System.Windows.Forms.ComboBox();
            this.lstExtSign = new System.Windows.Forms.ComboBox();
            this.radExtSign = new System.Windows.Forms.RadioButton();
            this.cmdCompile = new System.Windows.Forms.Button();
            this.cmdStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.cmdClose = new System.Windows.Forms.Button();
            this.BG = new System.ComponentModel.BackgroundWorker();
            this.panelSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Output filename:";
            // 
            // cmdSelectOutputFilename
            // 
            this.cmdSelectOutputFilename.Location = new System.Drawing.Point(413, 4);
            this.cmdSelectOutputFilename.Name = "cmdSelectOutputFilename";
            this.cmdSelectOutputFilename.Size = new System.Drawing.Size(75, 23);
            this.cmdSelectOutputFilename.TabIndex = 1;
            this.cmdSelectOutputFilename.Text = "Browse";
            this.cmdSelectOutputFilename.UseVisualStyleBackColor = true;
            this.cmdSelectOutputFilename.Click += new System.EventHandler(this.cmdSelectOutputFilename_Click);
            // 
            // txtOutputFilename
            // 
            this.txtOutputFilename.Location = new System.Drawing.Point(106, 6);
            this.txtOutputFilename.Name = "txtOutputFilename";
            this.txtOutputFilename.Size = new System.Drawing.Size(301, 20);
            this.txtOutputFilename.TabIndex = 0;
            // 
            // radCertSign
            // 
            this.radCertSign.AutoSize = true;
            this.radCertSign.Location = new System.Drawing.Point(6, 44);
            this.radCertSign.Name = "radCertSign";
            this.radCertSign.Size = new System.Drawing.Size(158, 17);
            this.radCertSign.TabIndex = 2;
            this.radCertSign.TabStop = true;
            this.radCertSign.Text = "Sign with installed certificate";
            this.radCertSign.UseVisualStyleBackColor = true;
            this.radCertSign.CheckedChanged += new System.EventHandler(this.radMULTI_CheckedChanged);
            // 
            // lstCert
            // 
            this.lstCert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstCert.FormattingEnabled = true;
            this.lstCert.Location = new System.Drawing.Point(25, 67);
            this.lstCert.Name = "lstCert";
            this.lstCert.Size = new System.Drawing.Size(319, 21);
            this.lstCert.TabIndex = 3;
            // 
            // lstExtSign
            // 
            this.lstExtSign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstExtSign.FormattingEnabled = true;
            this.lstExtSign.Location = new System.Drawing.Point(25, 117);
            this.lstExtSign.Name = "lstExtSign";
            this.lstExtSign.Size = new System.Drawing.Size(319, 21);
            this.lstExtSign.TabIndex = 5;
            // 
            // radExtSign
            // 
            this.radExtSign.AutoSize = true;
            this.radExtSign.Location = new System.Drawing.Point(6, 94);
            this.radExtSign.Name = "radExtSign";
            this.radExtSign.Size = new System.Drawing.Size(157, 17);
            this.radExtSign.TabIndex = 4;
            this.radExtSign.TabStop = true;
            this.radExtSign.Text = "Sign with external certificate";
            this.radExtSign.UseVisualStyleBackColor = true;
            this.radExtSign.CheckedChanged += new System.EventHandler(this.radMULTI_CheckedChanged);
            // 
            // cmdCompile
            // 
            this.cmdCompile.Location = new System.Drawing.Point(413, 200);
            this.cmdCompile.Name = "cmdCompile";
            this.cmdCompile.Size = new System.Drawing.Size(75, 23);
            this.cmdCompile.TabIndex = 7;
            this.cmdCompile.Text = "Compile";
            this.cmdCompile.UseVisualStyleBackColor = true;
            this.cmdCompile.Click += new System.EventHandler(this.cmdCompile_Click);
            // 
            // cmdStop
            // 
            this.cmdStop.Location = new System.Drawing.Point(425, 261);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(75, 23);
            this.cmdStop.TabIndex = 18;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Location = new System.Drawing.Point(15, 266);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(404, 13);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "---------";
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.cmdClose);
            this.panelSettings.Controls.Add(this.label4);
            this.panelSettings.Controls.Add(this.txtOutputFilename);
            this.panelSettings.Controls.Add(this.cmdSelectOutputFilename);
            this.panelSettings.Controls.Add(this.cmdCompile);
            this.panelSettings.Controls.Add(this.radCertSign);
            this.panelSettings.Controls.Add(this.lstCert);
            this.panelSettings.Controls.Add(this.radExtSign);
            this.panelSettings.Controls.Add(this.lstExtSign);
            this.panelSettings.Location = new System.Drawing.Point(12, 12);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(490, 231);
            this.panelSettings.TabIndex = 20;
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(332, 200);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 8;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // BG
            // 
            this.BG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BG_DoWork);
            this.BG.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BG_RunWorkerCompleted);
            // 
            // frmCompilePackage
            // 
            this.AcceptButton = this.cmdCompile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(517, 295);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmdStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCompilePackage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compile Package";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCompilePackage_FormClosing);
            this.Load += new System.EventHandler(this.frmCompilePackage_Load);
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdSelectOutputFilename;
        private System.Windows.Forms.TextBox txtOutputFilename;
        private System.Windows.Forms.RadioButton radCertSign;
        private System.Windows.Forms.ComboBox lstCert;
        private System.Windows.Forms.ComboBox lstExtSign;
        private System.Windows.Forms.RadioButton radExtSign;
        private System.Windows.Forms.Button cmdCompile;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelSettings;
        private System.ComponentModel.BackgroundWorker BG;
        private System.Windows.Forms.Button cmdClose;
    }
}