namespace FoxSDC_MGMT
{
    partial class frmRequestReport
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.lstContract = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstMachines = new System.Windows.Forms.ComboBox();
            this.chkFrom = new System.Windows.Forms.CheckBox();
            this.DTFrom = new System.Windows.Forms.DateTimePicker();
            this.DTTo = new System.Windows.Forms.DateTimePicker();
            this.chkTo = new System.Windows.Forms.CheckBox();
            this.cmdLM = new System.Windows.Forms.Button();
            this.cmdTM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(321, 213);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(240, 213);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lstContract
            // 
            this.lstContract.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstContract.FormattingEnabled = true;
            this.lstContract.Location = new System.Drawing.Point(96, 17);
            this.lstContract.Name = "lstContract";
            this.lstContract.Size = new System.Drawing.Size(300, 21);
            this.lstContract.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Contract:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Machine:";
            // 
            // lstMachines
            // 
            this.lstMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstMachines.FormattingEnabled = true;
            this.lstMachines.Location = new System.Drawing.Point(96, 44);
            this.lstMachines.Name = "lstMachines";
            this.lstMachines.Size = new System.Drawing.Size(300, 21);
            this.lstMachines.TabIndex = 1;
            // 
            // chkFrom
            // 
            this.chkFrom.AutoSize = true;
            this.chkFrom.Location = new System.Drawing.Point(30, 105);
            this.chkFrom.Name = "chkFrom";
            this.chkFrom.Size = new System.Drawing.Size(52, 17);
            this.chkFrom.TabIndex = 2;
            this.chkFrom.Text = "From:";
            this.chkFrom.UseVisualStyleBackColor = true;
            this.chkFrom.CheckedChanged += new System.EventHandler(this.chkFrom_CheckedChanged);
            // 
            // DTFrom
            // 
            this.DTFrom.Location = new System.Drawing.Point(96, 102);
            this.DTFrom.Name = "DTFrom";
            this.DTFrom.Size = new System.Drawing.Size(300, 20);
            this.DTFrom.TabIndex = 3;
            // 
            // DTTo
            // 
            this.DTTo.Location = new System.Drawing.Point(96, 128);
            this.DTTo.Name = "DTTo";
            this.DTTo.Size = new System.Drawing.Size(300, 20);
            this.DTTo.TabIndex = 5;
            // 
            // chkTo
            // 
            this.chkTo.AutoSize = true;
            this.chkTo.Location = new System.Drawing.Point(30, 131);
            this.chkTo.Name = "chkTo";
            this.chkTo.Size = new System.Drawing.Size(42, 17);
            this.chkTo.TabIndex = 4;
            this.chkTo.Text = "To:";
            this.chkTo.UseVisualStyleBackColor = true;
            this.chkTo.CheckedChanged += new System.EventHandler(this.chkTo_CheckedChanged);
            // 
            // cmdLM
            // 
            this.cmdLM.Location = new System.Drawing.Point(96, 154);
            this.cmdLM.Name = "cmdLM";
            this.cmdLM.Size = new System.Drawing.Size(75, 23);
            this.cmdLM.TabIndex = 6;
            this.cmdLM.Text = "Last month";
            this.cmdLM.UseVisualStyleBackColor = true;
            this.cmdLM.Click += new System.EventHandler(this.cmdLM_Click);
            // 
            // cmdTM
            // 
            this.cmdTM.Location = new System.Drawing.Point(177, 154);
            this.cmdTM.Name = "cmdTM";
            this.cmdTM.Size = new System.Drawing.Size(75, 23);
            this.cmdTM.TabIndex = 7;
            this.cmdTM.Text = "This month";
            this.cmdTM.UseVisualStyleBackColor = true;
            this.cmdTM.Click += new System.EventHandler(this.cmdTM_Click);
            // 
            // frmRequestReport
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(419, 248);
            this.Controls.Add(this.cmdTM);
            this.Controls.Add(this.cmdLM);
            this.Controls.Add(this.DTTo);
            this.Controls.Add(this.chkTo);
            this.Controls.Add(this.DTFrom);
            this.Controls.Add(this.chkFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstMachines);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstContract);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRequestReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Request Report";
            this.Load += new System.EventHandler(this.frmRequestReport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ComboBox lstContract;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox lstMachines;
        private System.Windows.Forms.CheckBox chkFrom;
        private System.Windows.Forms.DateTimePicker DTFrom;
        private System.Windows.Forms.DateTimePicker DTTo;
        private System.Windows.Forms.CheckBox chkTo;
        private System.Windows.Forms.Button cmdLM;
        private System.Windows.Forms.Button cmdTM;
    }
}