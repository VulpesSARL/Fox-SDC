namespace FoxSDC_MGMT
{
    partial class frmSetupScheduler
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
            this.lstMethod = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DTStart = new System.Windows.Forms.DateTimePicker();
            this.panelMonthly = new System.Windows.Forms.Panel();
            this.lstDays = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lstMonths = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelWeekly = new System.Windows.Forms.Panel();
            this.lstDayOfWeek = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblWeeks = new System.Windows.Forms.Label();
            this.txtRecurWeeks = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panelDaily = new System.Windows.Forms.Panel();
            this.lblDays = new System.Windows.Forms.Label();
            this.txtRecurDays = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.panelMonthly.SuspendLayout();
            this.panelWeekly.SuspendLayout();
            this.panelDaily.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Method:";
            // 
            // lstMethod
            // 
            this.lstMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstMethod.FormattingEnabled = true;
            this.lstMethod.Location = new System.Drawing.Point(141, 12);
            this.lstMethod.Name = "lstMethod";
            this.lstMethod.Size = new System.Drawing.Size(121, 21);
            this.lstMethod.TabIndex = 1;
            this.lstMethod.SelectedIndexChanged += new System.EventHandler(this.lstMethod_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start Date:";
            // 
            // DTStart
            // 
            this.DTStart.Location = new System.Drawing.Point(141, 48);
            this.DTStart.Name = "DTStart";
            this.DTStart.Size = new System.Drawing.Size(382, 20);
            this.DTStart.TabIndex = 3;
            // 
            // panelMonthly
            // 
            this.panelMonthly.Controls.Add(this.lstDays);
            this.panelMonthly.Controls.Add(this.label4);
            this.panelMonthly.Controls.Add(this.lstMonths);
            this.panelMonthly.Controls.Add(this.label3);
            this.panelMonthly.Location = new System.Drawing.Point(15, 74);
            this.panelMonthly.Name = "panelMonthly";
            this.panelMonthly.Size = new System.Drawing.Size(511, 216);
            this.panelMonthly.TabIndex = 4;
            // 
            // lstDays
            // 
            this.lstDays.CheckOnClick = true;
            this.lstDays.FormattingEnabled = true;
            this.lstDays.IntegralHeight = false;
            this.lstDays.Location = new System.Drawing.Point(126, 106);
            this.lstDays.Name = "lstDays";
            this.lstDays.ScrollAlwaysVisible = true;
            this.lstDays.Size = new System.Drawing.Size(282, 88);
            this.lstDays.TabIndex = 3;
            this.lstDays.ThreeDCheckBoxes = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Days:";
            // 
            // lstMonths
            // 
            this.lstMonths.CheckOnClick = true;
            this.lstMonths.FormattingEnabled = true;
            this.lstMonths.IntegralHeight = false;
            this.lstMonths.Location = new System.Drawing.Point(126, 12);
            this.lstMonths.Name = "lstMonths";
            this.lstMonths.ScrollAlwaysVisible = true;
            this.lstMonths.Size = new System.Drawing.Size(282, 88);
            this.lstMonths.TabIndex = 1;
            this.lstMonths.ThreeDCheckBoxes = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Months:";
            // 
            // panelWeekly
            // 
            this.panelWeekly.Controls.Add(this.lstDayOfWeek);
            this.panelWeekly.Controls.Add(this.label7);
            this.panelWeekly.Controls.Add(this.lblWeeks);
            this.panelWeekly.Controls.Add(this.txtRecurWeeks);
            this.panelWeekly.Controls.Add(this.label5);
            this.panelWeekly.Location = new System.Drawing.Point(15, 378);
            this.panelWeekly.Name = "panelWeekly";
            this.panelWeekly.Size = new System.Drawing.Size(511, 216);
            this.panelWeekly.TabIndex = 5;
            // 
            // lstDayOfWeek
            // 
            this.lstDayOfWeek.CheckOnClick = true;
            this.lstDayOfWeek.FormattingEnabled = true;
            this.lstDayOfWeek.IntegralHeight = false;
            this.lstDayOfWeek.Location = new System.Drawing.Point(126, 33);
            this.lstDayOfWeek.Name = "lstDayOfWeek";
            this.lstDayOfWeek.ScrollAlwaysVisible = true;
            this.lstDayOfWeek.Size = new System.Drawing.Size(282, 88);
            this.lstDayOfWeek.TabIndex = 5;
            this.lstDayOfWeek.ThreeDCheckBoxes = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Days:";
            // 
            // lblWeeks
            // 
            this.lblWeeks.AutoSize = true;
            this.lblWeeks.Location = new System.Drawing.Point(232, 10);
            this.lblWeeks.Name = "lblWeeks";
            this.lblWeeks.Size = new System.Drawing.Size(38, 13);
            this.lblWeeks.TabIndex = 2;
            this.lblWeeks.Text = "weeks";
            // 
            // txtRecurWeeks
            // 
            this.txtRecurWeeks.Location = new System.Drawing.Point(126, 7);
            this.txtRecurWeeks.Name = "txtRecurWeeks";
            this.txtRecurWeeks.Size = new System.Drawing.Size(100, 20);
            this.txtRecurWeeks.TabIndex = 1;
            this.txtRecurWeeks.TextChanged += new System.EventHandler(this.txtRecurWeeks_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Recur every:";
            // 
            // panelDaily
            // 
            this.panelDaily.Controls.Add(this.lblDays);
            this.panelDaily.Controls.Add(this.txtRecurDays);
            this.panelDaily.Controls.Add(this.label10);
            this.panelDaily.Location = new System.Drawing.Point(15, 539);
            this.panelDaily.Name = "panelDaily";
            this.panelDaily.Size = new System.Drawing.Size(511, 216);
            this.panelDaily.TabIndex = 6;
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(232, 10);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(29, 13);
            this.lblDays.TabIndex = 2;
            this.lblDays.Text = "days";
            // 
            // txtRecurDays
            // 
            this.txtRecurDays.Location = new System.Drawing.Point(126, 7);
            this.txtRecurDays.Name = "txtRecurDays";
            this.txtRecurDays.Size = new System.Drawing.Size(100, 20);
            this.txtRecurDays.TabIndex = 1;
            this.txtRecurDays.TextChanged += new System.EventHandler(this.txtRecurDays_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Recur every:";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(370, 296);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(451, 296);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // frmSetupScheduler
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(540, 328);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.panelDaily);
            this.Controls.Add(this.panelWeekly);
            this.Controls.Add(this.panelMonthly);
            this.Controls.Add(this.DTStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstMethod);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetupScheduler";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetupScheduler";
            this.Load += new System.EventHandler(this.frmSetupScheduler_Load);
            this.panelMonthly.ResumeLayout(false);
            this.panelMonthly.PerformLayout();
            this.panelWeekly.ResumeLayout(false);
            this.panelWeekly.PerformLayout();
            this.panelDaily.ResumeLayout(false);
            this.panelDaily.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DTStart;
        private System.Windows.Forms.Panel panelMonthly;
        private System.Windows.Forms.CheckedListBox lstDays;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox lstMonths;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelWeekly;
        private System.Windows.Forms.Label lblWeeks;
        private System.Windows.Forms.TextBox txtRecurWeeks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox lstDayOfWeek;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelDaily;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.TextBox txtRecurDays;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
    }
}