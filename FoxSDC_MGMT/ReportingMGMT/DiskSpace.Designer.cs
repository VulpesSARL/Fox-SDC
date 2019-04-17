namespace FoxSDC_MGMT.ReportingMGMT
{
    partial class frmDiskSpace
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
            this.label1 = new System.Windows.Forms.Label();
            this.lstDrive = new System.Windows.Forms.ComboBox();
            this.lstMethod = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstDiv = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(197, 113);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(116, 113);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Disk drive:";
            // 
            // lstDrive
            // 
            this.lstDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDrive.FormattingEnabled = true;
            this.lstDrive.Location = new System.Drawing.Point(81, 20);
            this.lstDrive.Name = "lstDrive";
            this.lstDrive.Size = new System.Drawing.Size(191, 21);
            this.lstDrive.TabIndex = 0;
            // 
            // lstMethod
            // 
            this.lstMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstMethod.FormattingEnabled = true;
            this.lstMethod.Location = new System.Drawing.Point(81, 47);
            this.lstMethod.Name = "lstMethod";
            this.lstMethod.Size = new System.Drawing.Size(191, 21);
            this.lstMethod.TabIndex = 1;
            this.lstMethod.SelectedIndexChanged += new System.EventHandler(this.lstMethod_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Method:";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(81, 74);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(125, 20);
            this.txtSize.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Size:";
            // 
            // lstDiv
            // 
            this.lstDiv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDiv.FormattingEnabled = true;
            this.lstDiv.Location = new System.Drawing.Point(212, 73);
            this.lstDiv.Name = "lstDiv";
            this.lstDiv.Size = new System.Drawing.Size(60, 21);
            this.lstDiv.TabIndex = 3;
            this.lstDiv.SelectedIndexChanged += new System.EventHandler(this.lstDiv_SelectedIndexChanged);
            // 
            // frmDiskSpace
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(291, 159);
            this.Controls.Add(this.lstDiv);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.lstMethod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstDrive);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDiskSpace";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Free diskspace check";
            this.Load += new System.EventHandler(this.frmDiskSpace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstDrive;
        private System.Windows.Forms.ComboBox lstMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox lstDiv;
    }
}