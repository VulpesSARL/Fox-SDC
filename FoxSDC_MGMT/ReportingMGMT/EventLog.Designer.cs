namespace FoxSDC_MGMT.ReportingMGMT
{
    partial class frmEventLog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSources = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lstTypes = new System.Windows.Forms.CheckedListBox();
            this.txtCategories = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lstBook = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstInclExcl = new System.Windows.Forms.ComboBox();
            this.lstTexts = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmdInsert = new System.Windows.Forms.Button();
            this.cmdEdit = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Book:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Souces:";
            // 
            // txtSources
            // 
            this.txtSources.Location = new System.Drawing.Point(96, 38);
            this.txtSources.Name = "txtSources";
            this.txtSources.Size = new System.Drawing.Size(249, 20);
            this.txtSources.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Type:";
            // 
            // lstTypes
            // 
            this.lstTypes.CheckOnClick = true;
            this.lstTypes.FormattingEnabled = true;
            this.lstTypes.IntegralHeight = false;
            this.lstTypes.Location = new System.Drawing.Point(96, 64);
            this.lstTypes.Name = "lstTypes";
            this.lstTypes.ScrollAlwaysVisible = true;
            this.lstTypes.Size = new System.Drawing.Size(249, 67);
            this.lstTypes.TabIndex = 2;
            // 
            // txtCategories
            // 
            this.txtCategories.Location = new System.Drawing.Point(96, 137);
            this.txtCategories.Name = "txtCategories";
            this.txtCategories.Size = new System.Drawing.Size(249, 20);
            this.txtCategories.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Event Log IDs:";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(189, 330);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 10;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(270, 330);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 9;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lstBook
            // 
            this.lstBook.FormattingEnabled = true;
            this.lstBook.Location = new System.Drawing.Point(96, 12);
            this.lstBook.Name = "lstBook";
            this.lstBook.Size = new System.Drawing.Size(249, 21);
            this.lstBook.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Incl/Excl:";
            // 
            // lstInclExcl
            // 
            this.lstInclExcl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstInclExcl.FormattingEnabled = true;
            this.lstInclExcl.Location = new System.Drawing.Point(96, 163);
            this.lstInclExcl.Name = "lstInclExcl";
            this.lstInclExcl.Size = new System.Drawing.Size(249, 21);
            this.lstInclExcl.TabIndex = 4;
            // 
            // lstTexts
            // 
            this.lstTexts.FormattingEnabled = true;
            this.lstTexts.IntegralHeight = false;
            this.lstTexts.Location = new System.Drawing.Point(96, 190);
            this.lstTexts.Name = "lstTexts";
            this.lstTexts.ScrollAlwaysVisible = true;
            this.lstTexts.Size = new System.Drawing.Size(249, 96);
            this.lstTexts.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Texts:";
            // 
            // cmdInsert
            // 
            this.cmdInsert.Location = new System.Drawing.Point(96, 292);
            this.cmdInsert.Name = "cmdInsert";
            this.cmdInsert.Size = new System.Drawing.Size(75, 23);
            this.cmdInsert.TabIndex = 6;
            this.cmdInsert.Text = "Add";
            this.cmdInsert.UseVisualStyleBackColor = true;
            this.cmdInsert.Click += new System.EventHandler(this.cmdInsert_Click);
            // 
            // cmdEdit
            // 
            this.cmdEdit.Location = new System.Drawing.Point(177, 292);
            this.cmdEdit.Name = "cmdEdit";
            this.cmdEdit.Size = new System.Drawing.Size(75, 23);
            this.cmdEdit.TabIndex = 7;
            this.cmdEdit.Text = "Edit";
            this.cmdEdit.UseVisualStyleBackColor = true;
            this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Location = new System.Drawing.Point(258, 292);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(75, 23);
            this.cmdRemove.TabIndex = 8;
            this.cmdRemove.Text = "Remove";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // frmEventLog
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(357, 365);
            this.Controls.Add(this.cmdRemove);
            this.Controls.Add(this.cmdEdit);
            this.Controls.Add(this.cmdInsert);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lstTexts);
            this.Controls.Add(this.lstInclExcl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstBook);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.txtCategories);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lstTypes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSources);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEventLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Log check";
            this.Load += new System.EventHandler(this.EventLog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSources;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox lstTypes;
        private System.Windows.Forms.TextBox txtCategories;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ComboBox lstBook;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox lstInclExcl;
        private System.Windows.Forms.ListBox lstTexts;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cmdInsert;
        private System.Windows.Forms.Button cmdEdit;
        private System.Windows.Forms.Button cmdRemove;
    }
}