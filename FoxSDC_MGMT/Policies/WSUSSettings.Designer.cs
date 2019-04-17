namespace FoxSDC_MGMT.Policies
{
    partial class ctlWSUSSettings
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
            this.chkConfWU = new System.Windows.Forms.CheckBox();
            this.chkSpecWUServer = new System.Windows.Forms.CheckBox();
            this.txtWUServer = new System.Windows.Forms.TextBox();
            this.txtStatusServer = new System.Windows.Forms.TextBox();
            this.chkWUStatServer = new System.Windows.Forms.CheckBox();
            this.txtDetectionFreq = new System.Windows.Forms.TextBox();
            this.chkDetectionFreq = new System.Windows.Forms.CheckBox();
            this.chkDontRestart = new System.Windows.Forms.CheckBox();
            this.chkAlwaysAutoRestart = new System.Windows.Forms.CheckBox();
            this.txtClientTarget = new System.Windows.Forms.TextBox();
            this.chkClientSideTargeting = new System.Windows.Forms.CheckBox();
            this.chkNoMSServer = new System.Windows.Forms.CheckBox();
            this.chkDoNotAutoRestartDuringWorkHours = new System.Windows.Forms.CheckBox();
            this.txtDeadline = new System.Windows.Forms.TextBox();
            this.chkDeadline = new System.Windows.Forms.CheckBox();
            this.lstActiveHours1 = new System.Windows.Forms.ComboBox();
            this.lstActiveHours2 = new System.Windows.Forms.ComboBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstDownloadMode = new System.Windows.Forms.ComboBox();
            this.chkDownloadMode = new System.Windows.Forms.CheckBox();
            this.txtAutoRestartMin = new System.Windows.Forms.TextBox();
            this.chkSpecUpdateMethod = new System.Windows.Forms.CheckBox();
            this.chkMicrosoftUpdate = new System.Windows.Forms.CheckBox();
            this.lstSchedInstHour = new System.Windows.Forms.ComboBox();
            this.lstSchedInstDay = new System.Windows.Forms.ComboBox();
            this.chkScheduleInstall = new System.Windows.Forms.CheckBox();
            this.chkInstallDuringMaintenance = new System.Windows.Forms.CheckBox();
            this.lstWUOptions = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(34, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "---------";
            // 
            // chkConfWU
            // 
            this.chkConfWU.AutoSize = true;
            this.chkConfWU.Location = new System.Drawing.Point(3, 33);
            this.chkConfWU.Name = "chkConfWU";
            this.chkConfWU.Size = new System.Drawing.Size(156, 17);
            this.chkConfWU.TabIndex = 0;
            this.chkConfWU.Text = "Configure Windows Update";
            this.chkConfWU.ThreeState = true;
            this.chkConfWU.UseVisualStyleBackColor = true;
            this.chkConfWU.CheckStateChanged += new System.EventHandler(this.chkConfWU_CheckedChanged);
            // 
            // chkSpecWUServer
            // 
            this.chkSpecWUServer.AutoSize = true;
            this.chkSpecWUServer.Location = new System.Drawing.Point(14, 111);
            this.chkSpecWUServer.Name = "chkSpecWUServer";
            this.chkSpecWUServer.Size = new System.Drawing.Size(120, 17);
            this.chkSpecWUServer.TabIndex = 7;
            this.chkSpecWUServer.Text = "Specify WU Server:";
            this.chkSpecWUServer.ThreeState = true;
            this.chkSpecWUServer.UseVisualStyleBackColor = true;
            this.chkSpecWUServer.CheckStateChanged += new System.EventHandler(this.chkSpecWUServer_CheckedChanged);
            // 
            // txtWUServer
            // 
            this.txtWUServer.Location = new System.Drawing.Point(274, 109);
            this.txtWUServer.Name = "txtWUServer";
            this.txtWUServer.Size = new System.Drawing.Size(220, 20);
            this.txtWUServer.TabIndex = 8;
            // 
            // txtStatusServer
            // 
            this.txtStatusServer.Location = new System.Drawing.Point(274, 132);
            this.txtStatusServer.Name = "txtStatusServer";
            this.txtStatusServer.Size = new System.Drawing.Size(220, 20);
            this.txtStatusServer.TabIndex = 10;
            // 
            // chkWUStatServer
            // 
            this.chkWUStatServer.AutoSize = true;
            this.chkWUStatServer.Location = new System.Drawing.Point(14, 134);
            this.chkWUStatServer.Name = "chkWUStatServer";
            this.chkWUStatServer.Size = new System.Drawing.Size(131, 17);
            this.chkWUStatServer.TabIndex = 9;
            this.chkWUStatServer.Text = "Specify Status Server:";
            this.chkWUStatServer.ThreeState = true;
            this.chkWUStatServer.UseVisualStyleBackColor = true;
            this.chkWUStatServer.CheckStateChanged += new System.EventHandler(this.chkWUStatServer_CheckedChanged);
            // 
            // txtDetectionFreq
            // 
            this.txtDetectionFreq.Location = new System.Drawing.Point(274, 201);
            this.txtDetectionFreq.Name = "txtDetectionFreq";
            this.txtDetectionFreq.Size = new System.Drawing.Size(220, 20);
            this.txtDetectionFreq.TabIndex = 15;
            // 
            // chkDetectionFreq
            // 
            this.chkDetectionFreq.AutoSize = true;
            this.chkDetectionFreq.Location = new System.Drawing.Point(14, 203);
            this.chkDetectionFreq.Name = "chkDetectionFreq";
            this.chkDetectionFreq.Size = new System.Drawing.Size(163, 17);
            this.chkDetectionFreq.TabIndex = 14;
            this.chkDetectionFreq.Text = "Detection Frequency (hours):";
            this.chkDetectionFreq.ThreeState = true;
            this.chkDetectionFreq.UseVisualStyleBackColor = true;
            this.chkDetectionFreq.CheckStateChanged += new System.EventHandler(this.chkDetectionFreq_CheckedChanged);
            // 
            // chkDontRestart
            // 
            this.chkDontRestart.AutoSize = true;
            this.chkDontRestart.Location = new System.Drawing.Point(14, 227);
            this.chkDontRestart.Name = "chkDontRestart";
            this.chkDontRestart.Size = new System.Drawing.Size(274, 17);
            this.chkDontRestart.TabIndex = 16;
            this.chkDontRestart.Text = "Don\'t automatically restart when an user is logged on";
            this.chkDontRestart.ThreeState = true;
            this.chkDontRestart.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysAutoRestart
            // 
            this.chkAlwaysAutoRestart.AutoSize = true;
            this.chkAlwaysAutoRestart.Location = new System.Drawing.Point(14, 250);
            this.chkAlwaysAutoRestart.Name = "chkAlwaysAutoRestart";
            this.chkAlwaysAutoRestart.Size = new System.Drawing.Size(221, 17);
            this.chkAlwaysAutoRestart.TabIndex = 17;
            this.chkAlwaysAutoRestart.Text = "Always automatically restart after minutes:";
            this.chkAlwaysAutoRestart.ThreeState = true;
            this.chkAlwaysAutoRestart.UseVisualStyleBackColor = true;
            this.chkAlwaysAutoRestart.CheckStateChanged += new System.EventHandler(this.chkAlwaysAutoRestart_CheckStateChanged);
            // 
            // txtClientTarget
            // 
            this.txtClientTarget.Location = new System.Drawing.Point(275, 155);
            this.txtClientTarget.Name = "txtClientTarget";
            this.txtClientTarget.Size = new System.Drawing.Size(220, 20);
            this.txtClientTarget.TabIndex = 12;
            // 
            // chkClientSideTargeting
            // 
            this.chkClientSideTargeting.AutoSize = true;
            this.chkClientSideTargeting.Location = new System.Drawing.Point(14, 157);
            this.chkClientSideTargeting.Name = "chkClientSideTargeting";
            this.chkClientSideTargeting.Size = new System.Drawing.Size(121, 17);
            this.chkClientSideTargeting.TabIndex = 11;
            this.chkClientSideTargeting.Text = "Client side targeting:";
            this.chkClientSideTargeting.ThreeState = true;
            this.chkClientSideTargeting.UseVisualStyleBackColor = true;
            this.chkClientSideTargeting.CheckStateChanged += new System.EventHandler(this.chkClientSideTargeting_CheckedChanged);
            // 
            // chkNoMSServer
            // 
            this.chkNoMSServer.AutoSize = true;
            this.chkNoMSServer.Location = new System.Drawing.Point(14, 180);
            this.chkNoMSServer.Name = "chkNoMSServer";
            this.chkNoMSServer.Size = new System.Drawing.Size(192, 17);
            this.chkNoMSServer.TabIndex = 13;
            this.chkNoMSServer.Text = "Do not connect to Microsoft Server";
            this.chkNoMSServer.ThreeState = true;
            this.chkNoMSServer.UseVisualStyleBackColor = true;
            // 
            // chkDoNotAutoRestartDuringWorkHours
            // 
            this.chkDoNotAutoRestartDuringWorkHours.AutoSize = true;
            this.chkDoNotAutoRestartDuringWorkHours.Location = new System.Drawing.Point(14, 296);
            this.chkDoNotAutoRestartDuringWorkHours.Name = "chkDoNotAutoRestartDuringWorkHours";
            this.chkDoNotAutoRestartDuringWorkHours.Size = new System.Drawing.Size(250, 17);
            this.chkDoNotAutoRestartDuringWorkHours.TabIndex = 21;
            this.chkDoNotAutoRestartDuringWorkHours.Text = "Do not automatically restart during active hours:";
            this.chkDoNotAutoRestartDuringWorkHours.ThreeState = true;
            this.chkDoNotAutoRestartDuringWorkHours.UseVisualStyleBackColor = true;
            this.chkDoNotAutoRestartDuringWorkHours.CheckStateChanged += new System.EventHandler(this.chkActiveHours_CheckedChanged);
            // 
            // txtDeadline
            // 
            this.txtDeadline.Location = new System.Drawing.Point(274, 271);
            this.txtDeadline.Name = "txtDeadline";
            this.txtDeadline.Size = new System.Drawing.Size(220, 20);
            this.txtDeadline.TabIndex = 20;
            // 
            // chkDeadline
            // 
            this.chkDeadline.AutoSize = true;
            this.chkDeadline.Location = new System.Drawing.Point(14, 273);
            this.chkDeadline.Name = "chkDeadline";
            this.chkDeadline.Size = new System.Drawing.Size(113, 17);
            this.chkDeadline.TabIndex = 19;
            this.chkDeadline.Text = "Deadline (in days):";
            this.chkDeadline.ThreeState = true;
            this.chkDeadline.UseVisualStyleBackColor = true;
            this.chkDeadline.CheckStateChanged += new System.EventHandler(this.chkDeadline_CheckedChanged);
            // 
            // lstActiveHours1
            // 
            this.lstActiveHours1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstActiveHours1.FormattingEnabled = true;
            this.lstActiveHours1.Location = new System.Drawing.Point(275, 294);
            this.lstActiveHours1.Name = "lstActiveHours1";
            this.lstActiveHours1.Size = new System.Drawing.Size(64, 21);
            this.lstActiveHours1.TabIndex = 22;
            // 
            // lstActiveHours2
            // 
            this.lstActiveHours2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstActiveHours2.FormattingEnabled = true;
            this.lstActiveHours2.Location = new System.Drawing.Point(345, 294);
            this.lstActiveHours2.Name = "lstActiveHours2";
            this.lstActiveHours2.Size = new System.Drawing.Size(64, 21);
            this.lstActiveHours2.TabIndex = 23;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(29, 469);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(91, 50);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstDownloadMode);
            this.panel1.Controls.Add(this.chkDownloadMode);
            this.panel1.Controls.Add(this.txtAutoRestartMin);
            this.panel1.Controls.Add(this.chkSpecUpdateMethod);
            this.panel1.Controls.Add(this.chkMicrosoftUpdate);
            this.panel1.Controls.Add(this.lstSchedInstHour);
            this.panel1.Controls.Add(this.lstSchedInstDay);
            this.panel1.Controls.Add(this.chkScheduleInstall);
            this.panel1.Controls.Add(this.chkInstallDuringMaintenance);
            this.panel1.Controls.Add(this.lstWUOptions);
            this.panel1.Controls.Add(this.chkSpecWUServer);
            this.panel1.Controls.Add(this.txtWUServer);
            this.panel1.Controls.Add(this.lstActiveHours2);
            this.panel1.Controls.Add(this.chkWUStatServer);
            this.panel1.Controls.Add(this.lstActiveHours1);
            this.panel1.Controls.Add(this.txtStatusServer);
            this.panel1.Controls.Add(this.chkDetectionFreq);
            this.panel1.Controls.Add(this.txtDeadline);
            this.panel1.Controls.Add(this.txtDetectionFreq);
            this.panel1.Controls.Add(this.chkDeadline);
            this.panel1.Controls.Add(this.chkDontRestart);
            this.panel1.Controls.Add(this.chkDoNotAutoRestartDuringWorkHours);
            this.panel1.Controls.Add(this.chkAlwaysAutoRestart);
            this.panel1.Controls.Add(this.chkNoMSServer);
            this.panel1.Controls.Add(this.chkClientSideTargeting);
            this.panel1.Controls.Add(this.txtClientTarget);
            this.panel1.Location = new System.Drawing.Point(15, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 376);
            this.panel1.TabIndex = 1;
            // 
            // lstDownloadMode
            // 
            this.lstDownloadMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDownloadMode.FormattingEnabled = true;
            this.lstDownloadMode.Location = new System.Drawing.Point(274, 321);
            this.lstDownloadMode.Name = "lstDownloadMode";
            this.lstDownloadMode.Size = new System.Drawing.Size(221, 21);
            this.lstDownloadMode.TabIndex = 25;
            // 
            // chkDownloadMode
            // 
            this.chkDownloadMode.AutoSize = true;
            this.chkDownloadMode.Location = new System.Drawing.Point(14, 323);
            this.chkDownloadMode.Name = "chkDownloadMode";
            this.chkDownloadMode.Size = new System.Drawing.Size(127, 17);
            this.chkDownloadMode.TabIndex = 24;
            this.chkDownloadMode.Text = "Delivery Optimisation:";
            this.chkDownloadMode.ThreeState = true;
            this.chkDownloadMode.UseVisualStyleBackColor = true;
            this.chkDownloadMode.CheckedChanged += new System.EventHandler(this.chkDownloadMode_CheckedChanged);
            // 
            // txtAutoRestartMin
            // 
            this.txtAutoRestartMin.Location = new System.Drawing.Point(274, 248);
            this.txtAutoRestartMin.Name = "txtAutoRestartMin";
            this.txtAutoRestartMin.Size = new System.Drawing.Size(220, 20);
            this.txtAutoRestartMin.TabIndex = 18;
            // 
            // chkSpecUpdateMethod
            // 
            this.chkSpecUpdateMethod.AutoSize = true;
            this.chkSpecUpdateMethod.Location = new System.Drawing.Point(14, 15);
            this.chkSpecUpdateMethod.Name = "chkSpecUpdateMethod";
            this.chkSpecUpdateMethod.Size = new System.Drawing.Size(138, 17);
            this.chkSpecUpdateMethod.TabIndex = 0;
            this.chkSpecUpdateMethod.Text = "Specify update method:";
            this.chkSpecUpdateMethod.ThreeState = true;
            this.chkSpecUpdateMethod.UseVisualStyleBackColor = true;
            this.chkSpecUpdateMethod.CheckStateChanged += new System.EventHandler(this.chkSpecUpdateMethod_CheckStateChanged);
            // 
            // chkMicrosoftUpdate
            // 
            this.chkMicrosoftUpdate.AutoSize = true;
            this.chkMicrosoftUpdate.Location = new System.Drawing.Point(14, 86);
            this.chkMicrosoftUpdate.Name = "chkMicrosoftUpdate";
            this.chkMicrosoftUpdate.Size = new System.Drawing.Size(227, 17);
            this.chkMicrosoftUpdate.TabIndex = 6;
            this.chkMicrosoftUpdate.Text = "Install updates for other Microsoft Products";
            this.chkMicrosoftUpdate.ThreeState = true;
            this.chkMicrosoftUpdate.UseVisualStyleBackColor = true;
            // 
            // lstSchedInstHour
            // 
            this.lstSchedInstHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSchedInstHour.FormattingEnabled = true;
            this.lstSchedInstHour.Location = new System.Drawing.Point(344, 61);
            this.lstSchedInstHour.Name = "lstSchedInstHour";
            this.lstSchedInstHour.Size = new System.Drawing.Size(64, 21);
            this.lstSchedInstHour.TabIndex = 5;
            // 
            // lstSchedInstDay
            // 
            this.lstSchedInstDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSchedInstDay.FormattingEnabled = true;
            this.lstSchedInstDay.Location = new System.Drawing.Point(274, 61);
            this.lstSchedInstDay.Name = "lstSchedInstDay";
            this.lstSchedInstDay.Size = new System.Drawing.Size(64, 21);
            this.lstSchedInstDay.TabIndex = 4;
            // 
            // chkScheduleInstall
            // 
            this.chkScheduleInstall.AutoSize = true;
            this.chkScheduleInstall.Location = new System.Drawing.Point(14, 63);
            this.chkScheduleInstall.Name = "chkScheduleInstall";
            this.chkScheduleInstall.Size = new System.Drawing.Size(103, 17);
            this.chkScheduleInstall.TabIndex = 3;
            this.chkScheduleInstall.Text = "Schedule install:";
            this.chkScheduleInstall.ThreeState = true;
            this.chkScheduleInstall.UseVisualStyleBackColor = true;
            this.chkScheduleInstall.CheckStateChanged += new System.EventHandler(this.chkScheduleInstall_CheckStateChanged);
            // 
            // chkInstallDuringMaintenance
            // 
            this.chkInstallDuringMaintenance.AutoSize = true;
            this.chkInstallDuringMaintenance.Location = new System.Drawing.Point(14, 40);
            this.chkInstallDuringMaintenance.Name = "chkInstallDuringMaintenance";
            this.chkInstallDuringMaintenance.Size = new System.Drawing.Size(198, 17);
            this.chkInstallDuringMaintenance.TabIndex = 2;
            this.chkInstallDuringMaintenance.Text = "Install during automatic maintenance";
            this.chkInstallDuringMaintenance.ThreeState = true;
            this.chkInstallDuringMaintenance.UseVisualStyleBackColor = true;
            // 
            // lstWUOptions
            // 
            this.lstWUOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstWUOptions.FormattingEnabled = true;
            this.lstWUOptions.Location = new System.Drawing.Point(274, 13);
            this.lstWUOptions.Name = "lstWUOptions";
            this.lstWUOptions.Size = new System.Drawing.Size(221, 21);
            this.lstWUOptions.TabIndex = 1;
            // 
            // ctlWSUSSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.chkConfWU);
            this.Controls.Add(this.lblName);
            this.Name = "ctlWSUSSettings";
            this.Size = new System.Drawing.Size(554, 565);
            this.Load += new System.EventHandler(this.WSUSSettings_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkConfWU;
        private System.Windows.Forms.CheckBox chkSpecWUServer;
        private System.Windows.Forms.TextBox txtWUServer;
        private System.Windows.Forms.TextBox txtStatusServer;
        private System.Windows.Forms.CheckBox chkWUStatServer;
        private System.Windows.Forms.TextBox txtDetectionFreq;
        private System.Windows.Forms.CheckBox chkDetectionFreq;
        private System.Windows.Forms.CheckBox chkDontRestart;
        private System.Windows.Forms.CheckBox chkAlwaysAutoRestart;
        private System.Windows.Forms.TextBox txtClientTarget;
        private System.Windows.Forms.CheckBox chkClientSideTargeting;
        private System.Windows.Forms.CheckBox chkNoMSServer;
        private System.Windows.Forms.CheckBox chkDoNotAutoRestartDuringWorkHours;
        private System.Windows.Forms.TextBox txtDeadline;
        private System.Windows.Forms.CheckBox chkDeadline;
        private System.Windows.Forms.ComboBox lstActiveHours1;
        private System.Windows.Forms.ComboBox lstActiveHours2;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox lstWUOptions;
        private System.Windows.Forms.CheckBox chkInstallDuringMaintenance;
        private System.Windows.Forms.ComboBox lstSchedInstHour;
        private System.Windows.Forms.ComboBox lstSchedInstDay;
        private System.Windows.Forms.CheckBox chkScheduleInstall;
        private System.Windows.Forms.CheckBox chkMicrosoftUpdate;
        private System.Windows.Forms.CheckBox chkSpecUpdateMethod;
        private System.Windows.Forms.TextBox txtAutoRestartMin;
        private System.Windows.Forms.ComboBox lstDownloadMode;
        private System.Windows.Forms.CheckBox chkDownloadMode;
    }
}
