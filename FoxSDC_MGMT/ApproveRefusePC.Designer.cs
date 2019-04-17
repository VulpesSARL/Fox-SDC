namespace FoxSDC_MGMT
{
    partial class frmApproveRefusePC
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
            this.lblPCs = new System.Windows.Forms.Label();
            this.lstPCs = new System.Windows.Forms.ListBox();
            this.lstState = new System.Windows.Forms.ComboBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdSelectGroup = new System.Windows.Forms.Button();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblPCs
            // 
            this.lblPCs.AutoSize = true;
            this.lblPCs.Location = new System.Drawing.Point(12, 9);
            this.lblPCs.Name = "lblPCs";
            this.lblPCs.Size = new System.Drawing.Size(46, 13);
            this.lblPCs.TabIndex = 0;
            this.lblPCs.Text = "-------------";
            // 
            // lstPCs
            // 
            this.lstPCs.FormattingEnabled = true;
            this.lstPCs.IntegralHeight = false;
            this.lstPCs.Location = new System.Drawing.Point(12, 25);
            this.lstPCs.Name = "lstPCs";
            this.lstPCs.ScrollAlwaysVisible = true;
            this.lstPCs.Size = new System.Drawing.Size(259, 264);
            this.lstPCs.Sorted = true;
            this.lstPCs.TabIndex = 0;
            // 
            // lstState
            // 
            this.lstState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstState.FormattingEnabled = true;
            this.lstState.Location = new System.Drawing.Point(12, 295);
            this.lstState.Name = "lstState";
            this.lstState.Size = new System.Drawing.Size(259, 21);
            this.lstState.TabIndex = 1;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(115, 351);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(196, 351);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdSelectGroup
            // 
            this.cmdSelectGroup.Location = new System.Drawing.Point(196, 322);
            this.cmdSelectGroup.Name = "cmdSelectGroup";
            this.cmdSelectGroup.Size = new System.Drawing.Size(75, 23);
            this.cmdSelectGroup.TabIndex = 3;
            this.cmdSelectGroup.Text = "&Group";
            this.cmdSelectGroup.UseVisualStyleBackColor = true;
            this.cmdSelectGroup.Click += new System.EventHandler(this.cmdSelectGroup_Click);
            // 
            // txtGroup
            // 
            this.txtGroup.Location = new System.Drawing.Point(12, 324);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.ReadOnly = true;
            this.txtGroup.Size = new System.Drawing.Size(178, 20);
            this.txtGroup.TabIndex = 2;
            // 
            // frmApproveRefusePC
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(283, 384);
            this.Controls.Add(this.txtGroup);
            this.Controls.Add(this.cmdSelectGroup);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.lstState);
            this.Controls.Add(this.lstPCs);
            this.Controls.Add(this.lblPCs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmApproveRefusePC";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Approve/Refuse PCs";
            this.Load += new System.EventHandler(this.frmApproveRefusePC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPCs;
        private System.Windows.Forms.ListBox lstPCs;
        private System.Windows.Forms.ComboBox lstState;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdSelectGroup;
        private System.Windows.Forms.TextBox txtGroup;
    }
}