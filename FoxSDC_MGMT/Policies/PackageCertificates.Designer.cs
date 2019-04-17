namespace FoxSDC_MGMT
{
    partial class ctlPackageCertificates
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
            this.lblName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblInstCert = new System.Windows.Forms.Label();
            this.cmdAddCer = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(46, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "-------------";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Installed Certificate:";
            // 
            // lblInstCert
            // 
            this.lblInstCert.AutoSize = true;
            this.lblInstCert.Location = new System.Drawing.Point(179, 34);
            this.lblInstCert.Name = "lblInstCert";
            this.lblInstCert.Size = new System.Drawing.Size(16, 13);
            this.lblInstCert.TabIndex = 2;
            this.lblInstCert.Text = "---";
            // 
            // cmdAddCer
            // 
            this.cmdAddCer.Location = new System.Drawing.Point(30, 102);
            this.cmdAddCer.Name = "cmdAddCer";
            this.cmdAddCer.Size = new System.Drawing.Size(151, 23);
            this.cmdAddCer.TabIndex = 0;
            this.cmdAddCer.Text = "Add CER Certificate";
            this.cmdAddCer.UseVisualStyleBackColor = true;
            this.cmdAddCer.Click += new System.EventHandler(this.cmdAddCer_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(30, 266);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 7;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // ctlAppCertificates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdAddCer);
            this.Controls.Add(this.lblInstCert);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblName);
            this.Name = "ctlAppCertificates";
            this.Size = new System.Drawing.Size(466, 402);
            this.Load += new System.EventHandler(this.ctlCertificates_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInstCert;
        private System.Windows.Forms.Button cmdAddCer;
        private System.Windows.Forms.Button cmdSave;
    }
}
