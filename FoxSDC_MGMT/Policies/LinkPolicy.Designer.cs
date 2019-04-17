namespace FoxSDC_MGMT.Policies
{
    partial class ctlLinkPolicy
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblLinkPolicy = new System.Windows.Forms.Label();
            this.cmdSelectPol = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.cmdSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Linked policy";
            // 
            // lblLinkPolicy
            // 
            this.lblLinkPolicy.AutoSize = true;
            this.lblLinkPolicy.Location = new System.Drawing.Point(49, 71);
            this.lblLinkPolicy.Name = "lblLinkPolicy";
            this.lblLinkPolicy.Size = new System.Drawing.Size(43, 13);
            this.lblLinkPolicy.TabIndex = 1;
            this.lblLinkPolicy.Text = "------------";
            // 
            // cmdSelectPol
            // 
            this.cmdSelectPol.Location = new System.Drawing.Point(52, 105);
            this.cmdSelectPol.Name = "cmdSelectPol";
            this.cmdSelectPol.Size = new System.Drawing.Size(75, 23);
            this.cmdSelectPol.TabIndex = 2;
            this.cmdSelectPol.Text = "&Select policy";
            this.cmdSelectPol.UseVisualStyleBackColor = true;
            this.cmdSelectPol.Click += new System.EventHandler(this.cmdSelectPol_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(46, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "-------------";
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(52, 147);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 4;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // ctlLinkPolicy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cmdSelectPol);
            this.Controls.Add(this.lblLinkPolicy);
            this.Controls.Add(this.label1);
            this.Name = "ctlLinkPolicy";
            this.Size = new System.Drawing.Size(562, 530);
            this.Load += new System.EventHandler(this.LinkPolicy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLinkPolicy;
        private System.Windows.Forms.Button cmdSelectPol;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button cmdSave;
    }
}
