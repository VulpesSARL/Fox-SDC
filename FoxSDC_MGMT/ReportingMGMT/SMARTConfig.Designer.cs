namespace FoxSDC_MGMT.ReportingMGMT
{
    partial class frmSMARTConfig
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
            this.chkNotifyOnUpdate = new System.Windows.Forms.CheckBox();
            this.chkNotifyOnRemove = new System.Windows.Forms.CheckBox();
            this.chkNotifyOnAdd = new System.Windows.Forms.CheckBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.chkNotifyError = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSkipAttrib = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkNotifyOnUpdate
            // 
            this.chkNotifyOnUpdate.AutoSize = true;
            this.chkNotifyOnUpdate.Location = new System.Drawing.Point(12, 58);
            this.chkNotifyOnUpdate.Name = "chkNotifyOnUpdate";
            this.chkNotifyOnUpdate.Size = new System.Drawing.Size(106, 17);
            this.chkNotifyOnUpdate.TabIndex = 2;
            this.chkNotifyOnUpdate.Text = "Notify on &Update";
            this.chkNotifyOnUpdate.UseVisualStyleBackColor = true;
            this.chkNotifyOnUpdate.CheckedChanged += new System.EventHandler(this.chkNotifyOnUpdate_CheckedChanged);
            // 
            // chkNotifyOnRemove
            // 
            this.chkNotifyOnRemove.AutoSize = true;
            this.chkNotifyOnRemove.Location = new System.Drawing.Point(12, 35);
            this.chkNotifyOnRemove.Name = "chkNotifyOnRemove";
            this.chkNotifyOnRemove.Size = new System.Drawing.Size(111, 17);
            this.chkNotifyOnRemove.TabIndex = 1;
            this.chkNotifyOnRemove.Text = "Notify on &Remove";
            this.chkNotifyOnRemove.UseVisualStyleBackColor = true;
            // 
            // chkNotifyOnAdd
            // 
            this.chkNotifyOnAdd.AutoSize = true;
            this.chkNotifyOnAdd.Location = new System.Drawing.Point(12, 12);
            this.chkNotifyOnAdd.Name = "chkNotifyOnAdd";
            this.chkNotifyOnAdd.Size = new System.Drawing.Size(90, 17);
            this.chkNotifyOnAdd.TabIndex = 0;
            this.chkNotifyOnAdd.Text = "Notify on &Add";
            this.chkNotifyOnAdd.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(306, 152);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(387, 152);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // chkNotifyError
            // 
            this.chkNotifyError.AutoSize = true;
            this.chkNotifyError.Location = new System.Drawing.Point(12, 120);
            this.chkNotifyError.Name = "chkNotifyError";
            this.chkNotifyError.Size = new System.Drawing.Size(93, 17);
            this.chkNotifyError.TabIndex = 4;
            this.chkNotifyError.Text = "Notify on &Error";
            this.chkNotifyError.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Skip these Attributes (Hex):";
            // 
            // txtSkipAttrib
            // 
            this.txtSkipAttrib.Location = new System.Drawing.Point(34, 94);
            this.txtSkipAttrib.Name = "txtSkipAttrib";
            this.txtSkipAttrib.Size = new System.Drawing.Size(428, 20);
            this.txtSkipAttrib.TabIndex = 3;
            // 
            // frmSMARTConfig
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(474, 183);
            this.Controls.Add(this.txtSkipAttrib);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkNotifyError);
            this.Controls.Add(this.chkNotifyOnUpdate);
            this.Controls.Add(this.chkNotifyOnRemove);
            this.Controls.Add(this.chkNotifyOnAdd);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSMARTConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SMART Reporting";
            this.Load += new System.EventHandler(this.frmSMARTConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkNotifyOnUpdate;
        private System.Windows.Forms.CheckBox chkNotifyOnRemove;
        private System.Windows.Forms.CheckBox chkNotifyOnAdd;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.CheckBox chkNotifyError;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSkipAttrib;
    }
}