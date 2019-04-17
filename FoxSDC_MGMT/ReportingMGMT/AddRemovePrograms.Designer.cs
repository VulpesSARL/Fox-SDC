namespace FoxSDC_MGMT.ReportingMGMT
{
    partial class frmAddRemovePrograms
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
            this.txtPrograms = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstCheck = new System.Windows.Forms.ComboBox();
            this.lstBits = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkNotifyOnAdd = new System.Windows.Forms.CheckBox();
            this.chkNotifyOnRemove = new System.Windows.Forms.CheckBox();
            this.chkNotifyOnUpdate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(245, 248);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(326, 248);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 6;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Programs:";
            // 
            // txtPrograms
            // 
            this.txtPrograms.AcceptsReturn = true;
            this.txtPrograms.Location = new System.Drawing.Point(81, 12);
            this.txtPrograms.Multiline = true;
            this.txtPrograms.Name = "txtPrograms";
            this.txtPrograms.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrograms.Size = new System.Drawing.Size(320, 86);
            this.txtPrograms.TabIndex = 0;
            this.txtPrograms.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Check:";
            // 
            // lstCheck
            // 
            this.lstCheck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstCheck.FormattingEnabled = true;
            this.lstCheck.Location = new System.Drawing.Point(81, 104);
            this.lstCheck.Name = "lstCheck";
            this.lstCheck.Size = new System.Drawing.Size(320, 21);
            this.lstCheck.TabIndex = 1;
            // 
            // lstBits
            // 
            this.lstBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstBits.FormattingEnabled = true;
            this.lstBits.Location = new System.Drawing.Point(81, 131);
            this.lstBits.Name = "lstBits";
            this.lstBits.Size = new System.Drawing.Size(320, 21);
            this.lstBits.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Bits:";
            // 
            // chkNotifyOnAdd
            // 
            this.chkNotifyOnAdd.AutoSize = true;
            this.chkNotifyOnAdd.Location = new System.Drawing.Point(81, 175);
            this.chkNotifyOnAdd.Name = "chkNotifyOnAdd";
            this.chkNotifyOnAdd.Size = new System.Drawing.Size(90, 17);
            this.chkNotifyOnAdd.TabIndex = 3;
            this.chkNotifyOnAdd.Text = "Notify on &Add";
            this.chkNotifyOnAdd.UseVisualStyleBackColor = true;
            // 
            // chkNotifyOnRemove
            // 
            this.chkNotifyOnRemove.AutoSize = true;
            this.chkNotifyOnRemove.Location = new System.Drawing.Point(81, 198);
            this.chkNotifyOnRemove.Name = "chkNotifyOnRemove";
            this.chkNotifyOnRemove.Size = new System.Drawing.Size(111, 17);
            this.chkNotifyOnRemove.TabIndex = 4;
            this.chkNotifyOnRemove.Text = "Notify on &Remove";
            this.chkNotifyOnRemove.UseVisualStyleBackColor = true;
            // 
            // chkNotifyOnUpdate
            // 
            this.chkNotifyOnUpdate.AutoSize = true;
            this.chkNotifyOnUpdate.Location = new System.Drawing.Point(81, 221);
            this.chkNotifyOnUpdate.Name = "chkNotifyOnUpdate";
            this.chkNotifyOnUpdate.Size = new System.Drawing.Size(106, 17);
            this.chkNotifyOnUpdate.TabIndex = 5;
            this.chkNotifyOnUpdate.Text = "Notify on &Update";
            this.chkNotifyOnUpdate.UseVisualStyleBackColor = true;
            // 
            // frmAddRemovePrograms
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(417, 282);
            this.ControlBox = false;
            this.Controls.Add(this.chkNotifyOnUpdate);
            this.Controls.Add(this.chkNotifyOnRemove);
            this.Controls.Add(this.chkNotifyOnAdd);
            this.Controls.Add(this.lstBits);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPrograms);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddRemovePrograms";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Remove Programs Check";
            this.Load += new System.EventHandler(this.frmAddRemovePrograms_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrograms;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox lstCheck;
        private System.Windows.Forms.ComboBox lstBits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkNotifyOnAdd;
        private System.Windows.Forms.CheckBox chkNotifyOnRemove;
        private System.Windows.Forms.CheckBox chkNotifyOnUpdate;
    }
}