namespace FoxSDC_MGMT
{
    partial class frmSeverSettings
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
            this.lstCert = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEventLogFlush = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label22 = new System.Windows.Forms.Label();
            this.txtKeepBitlockerRK = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtKeepReports = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtKeepDisks = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkEMailUseSSL = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMailPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMailUsername = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMailAdminAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMailFromName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMailFrom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMailPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMailServer = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEMailClientSubject = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtEMailAdminSubject = new System.Windows.Forms.TextBox();
            this.chkClientEMailTextIsHTML = new System.Windows.Forms.CheckBox();
            this.chkAdminEMailTextIsHTML = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtClientEMailText = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAdminEMailText = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.cmdRunAdminReportNow = new System.Windows.Forms.Button();
            this.cmdSetScheduleClient = new System.Windows.Forms.Button();
            this.lblClientSched = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmdSetScheduleAdmin = new System.Windows.Forms.Button();
            this.lblAdminSched = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.txtMessageDisclaimer = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtAdminName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtAdminAccess = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lblAnchor = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtKeepChatLog = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(554, 385);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(635, 385);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server Certificate for singing policies:";
            // 
            // lstCert
            // 
            this.lstCert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstCert.FormattingEnabled = true;
            this.lstCert.Location = new System.Drawing.Point(221, 15);
            this.lstCert.Name = "lstCert";
            this.lstCert.Size = new System.Drawing.Size(359, 21);
            this.lstCert.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Flush Eventlog at days:";
            // 
            // txtEventLogFlush
            // 
            this.txtEventLogFlush.Location = new System.Drawing.Point(222, 15);
            this.txtEventLogFlush.Name = "txtEventLogFlush";
            this.txtEventLogFlush.Size = new System.Drawing.Size(100, 20);
            this.txtEventLogFlush.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 367);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Controls.Add(this.txtKeepChatLog);
            this.tabPage1.Controls.Add(this.label22);
            this.tabPage1.Controls.Add(this.txtKeepBitlockerRK);
            this.tabPage1.Controls.Add(this.label19);
            this.tabPage1.Controls.Add(this.txtKeepReports);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.txtKeepDisks);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtEventLogFlush);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(694, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Housekeeping";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 96);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(168, 13);
            this.label22.TabIndex = 10;
            this.label22.Text = "Keep Bitlocker Recovery for days:";
            // 
            // txtKeepBitlockerRK
            // 
            this.txtKeepBitlockerRK.Location = new System.Drawing.Point(222, 93);
            this.txtKeepBitlockerRK.Name = "txtKeepBitlockerRK";
            this.txtKeepBitlockerRK.Size = new System.Drawing.Size(100, 20);
            this.txtKeepBitlockerRK.TabIndex = 3;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 70);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(115, 13);
            this.label19.TabIndex = 8;
            this.label19.Text = "Keep Reports for days:";
            // 
            // txtKeepReports
            // 
            this.txtKeepReports.Location = new System.Drawing.Point(222, 67);
            this.txtKeepReports.Name = "txtKeepReports";
            this.txtKeepReports.Size = new System.Drawing.Size(100, 20);
            this.txtKeepReports.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(161, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Keep non-present disks for days:";
            // 
            // txtKeepDisks
            // 
            this.txtKeepDisks.Location = new System.Drawing.Point(222, 41);
            this.txtKeepDisks.Name = "txtKeepDisks";
            this.txtKeepDisks.Size = new System.Drawing.Size(100, 20);
            this.txtKeepDisks.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkEMailUseSSL);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txtMailPassword);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtMailUsername);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.txtMailAdminAddress);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtMailFromName);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtMailFrom);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtMailPort);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtMailServer);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(694, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "E-Mail Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkEMailUseSSL
            // 
            this.chkEMailUseSSL.AutoSize = true;
            this.chkEMailUseSSL.Location = new System.Drawing.Point(222, 119);
            this.chkEMailUseSSL.Name = "chkEMailUseSSL";
            this.chkEMailUseSSL.Size = new System.Drawing.Size(68, 17);
            this.chkEMailUseSSL.TabIndex = 4;
            this.chkEMailUseSSL.Text = "Use SSL";
            this.chkEMailUseSSL.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "E-Mail SSL:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "E-Mail Password:";
            // 
            // txtMailPassword
            // 
            this.txtMailPassword.Location = new System.Drawing.Point(222, 93);
            this.txtMailPassword.Name = "txtMailPassword";
            this.txtMailPassword.Size = new System.Drawing.Size(298, 20);
            this.txtMailPassword.TabIndex = 3;
            this.txtMailPassword.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "E-Mail Username:";
            // 
            // txtMailUsername
            // 
            this.txtMailUsername.Location = new System.Drawing.Point(222, 67);
            this.txtMailUsername.Name = "txtMailUsername";
            this.txtMailUsername.Size = new System.Drawing.Size(298, 20);
            this.txtMailUsername.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Administrative E-Mail Address:";
            // 
            // txtMailAdminAddress
            // 
            this.txtMailAdminAddress.Location = new System.Drawing.Point(222, 212);
            this.txtMailAdminAddress.Name = "txtMailAdminAddress";
            this.txtMailAdminAddress.Size = new System.Drawing.Size(298, 20);
            this.txtMailAdminAddress.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "E-Mail From (Name):";
            // 
            // txtMailFromName
            // 
            this.txtMailFromName.Location = new System.Drawing.Point(222, 169);
            this.txtMailFromName.Name = "txtMailFromName";
            this.txtMailFromName.Size = new System.Drawing.Size(298, 20);
            this.txtMailFromName.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "E-Mail From (Address):";
            // 
            // txtMailFrom
            // 
            this.txtMailFrom.Location = new System.Drawing.Point(222, 143);
            this.txtMailFrom.Name = "txtMailFrom";
            this.txtMailFrom.Size = new System.Drawing.Size(298, 20);
            this.txtMailFrom.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "E-Mail Port:";
            // 
            // txtMailPort
            // 
            this.txtMailPort.Location = new System.Drawing.Point(222, 41);
            this.txtMailPort.Name = "txtMailPort";
            this.txtMailPort.Size = new System.Drawing.Size(74, 20);
            this.txtMailPort.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "E-Mail Server:";
            // 
            // txtMailServer
            // 
            this.txtMailServer.Location = new System.Drawing.Point(222, 15);
            this.txtMailServer.Name = "txtMailServer";
            this.txtMailServer.Size = new System.Drawing.Size(298, 20);
            this.txtMailServer.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.label17);
            this.tabPage6.Controls.Add(this.txtEMailClientSubject);
            this.tabPage6.Controls.Add(this.label16);
            this.tabPage6.Controls.Add(this.txtEMailAdminSubject);
            this.tabPage6.Controls.Add(this.chkClientEMailTextIsHTML);
            this.tabPage6.Controls.Add(this.chkAdminEMailTextIsHTML);
            this.tabPage6.Controls.Add(this.label15);
            this.tabPage6.Controls.Add(this.txtClientEMailText);
            this.tabPage6.Controls.Add(this.label13);
            this.tabPage6.Controls.Add(this.txtAdminEMailText);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(694, 341);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "E-Mail Text";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 286);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(156, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "E-Mail subject for client E-Mails:";
            // 
            // txtEMailClientSubject
            // 
            this.txtEMailClientSubject.Location = new System.Drawing.Point(222, 283);
            this.txtEMailClientSubject.Name = "txtEMailClientSubject";
            this.txtEMailClientSubject.Size = new System.Drawing.Size(298, 20);
            this.txtEMailClientSubject.TabIndex = 5;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 128);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(196, 13);
            this.label16.TabIndex = 12;
            this.label16.Text = "E-Mail subject for Administrative E-Mails:";
            // 
            // txtEMailAdminSubject
            // 
            this.txtEMailAdminSubject.Location = new System.Drawing.Point(222, 125);
            this.txtEMailAdminSubject.Name = "txtEMailAdminSubject";
            this.txtEMailAdminSubject.Size = new System.Drawing.Size(298, 20);
            this.txtEMailAdminSubject.TabIndex = 2;
            // 
            // chkClientEMailTextIsHTML
            // 
            this.chkClientEMailTextIsHTML.AutoSize = true;
            this.chkClientEMailTextIsHTML.Location = new System.Drawing.Point(222, 260);
            this.chkClientEMailTextIsHTML.Name = "chkClientEMailTextIsHTML";
            this.chkClientEMailTextIsHTML.Size = new System.Drawing.Size(90, 17);
            this.chkClientEMailTextIsHTML.TabIndex = 4;
            this.chkClientEMailTextIsHTML.Text = "Text is HTML";
            this.chkClientEMailTextIsHTML.UseVisualStyleBackColor = true;
            // 
            // chkAdminEMailTextIsHTML
            // 
            this.chkAdminEMailTextIsHTML.AutoSize = true;
            this.chkAdminEMailTextIsHTML.Location = new System.Drawing.Point(222, 102);
            this.chkAdminEMailTextIsHTML.Name = "chkAdminEMailTextIsHTML";
            this.chkAdminEMailTextIsHTML.Size = new System.Drawing.Size(90, 17);
            this.chkAdminEMailTextIsHTML.TabIndex = 1;
            this.chkAdminEMailTextIsHTML.Text = "Text is HTML";
            this.chkAdminEMailTextIsHTML.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 176);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(139, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "E-Mail text for client E-Mails:";
            // 
            // txtClientEMailText
            // 
            this.txtClientEMailText.AcceptsReturn = true;
            this.txtClientEMailText.Location = new System.Drawing.Point(222, 173);
            this.txtClientEMailText.Multiline = true;
            this.txtClientEMailText.Name = "txtClientEMailText";
            this.txtClientEMailText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtClientEMailText.Size = new System.Drawing.Size(411, 81);
            this.txtClientEMailText.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(179, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "E-Mail text for Administrative E-Mails:";
            // 
            // txtAdminEMailText
            // 
            this.txtAdminEMailText.AcceptsReturn = true;
            this.txtAdminEMailText.Location = new System.Drawing.Point(222, 15);
            this.txtAdminEMailText.Multiline = true;
            this.txtAdminEMailText.Name = "txtAdminEMailText";
            this.txtAdminEMailText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAdminEMailText.Size = new System.Drawing.Size(411, 81);
            this.txtAdminEMailText.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.cmdRunAdminReportNow);
            this.tabPage4.Controls.Add(this.cmdSetScheduleClient);
            this.tabPage4.Controls.Add(this.lblClientSched);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.cmdSetScheduleAdmin);
            this.tabPage4.Controls.Add(this.lblAdminSched);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(694, 341);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Schedules";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // cmdRunAdminReportNow
            // 
            this.cmdRunAdminReportNow.Location = new System.Drawing.Point(601, 13);
            this.cmdRunAdminReportNow.Name = "cmdRunAdminReportNow";
            this.cmdRunAdminReportNow.Size = new System.Drawing.Size(75, 23);
            this.cmdRunAdminReportNow.TabIndex = 1;
            this.cmdRunAdminReportNow.Text = "Run now";
            this.cmdRunAdminReportNow.UseVisualStyleBackColor = true;
            this.cmdRunAdminReportNow.Click += new System.EventHandler(this.cmdRunAdminReportNow_Click);
            // 
            // cmdSetScheduleClient
            // 
            this.cmdSetScheduleClient.Location = new System.Drawing.Point(520, 69);
            this.cmdSetScheduleClient.Name = "cmdSetScheduleClient";
            this.cmdSetScheduleClient.Size = new System.Drawing.Size(75, 23);
            this.cmdSetScheduleClient.TabIndex = 2;
            this.cmdSetScheduleClient.Text = "Set";
            this.cmdSetScheduleClient.UseVisualStyleBackColor = true;
            this.cmdSetScheduleClient.Click += new System.EventHandler(this.cmdSetScheduleClient_Click);
            // 
            // lblClientSched
            // 
            this.lblClientSched.Location = new System.Drawing.Point(222, 74);
            this.lblClientSched.Name = "lblClientSched";
            this.lblClientSched.Size = new System.Drawing.Size(292, 51);
            this.lblClientSched.TabIndex = 10;
            this.lblClientSched.Text = "---";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(200, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Schedule for sending Client Notifications:";
            // 
            // cmdSetScheduleAdmin
            // 
            this.cmdSetScheduleAdmin.Location = new System.Drawing.Point(520, 13);
            this.cmdSetScheduleAdmin.Name = "cmdSetScheduleAdmin";
            this.cmdSetScheduleAdmin.Size = new System.Drawing.Size(75, 23);
            this.cmdSetScheduleAdmin.TabIndex = 0;
            this.cmdSetScheduleAdmin.Text = "Set";
            this.cmdSetScheduleAdmin.UseVisualStyleBackColor = true;
            this.cmdSetScheduleAdmin.Click += new System.EventHandler(this.cmdSetScheduleAdmin_Click);
            // 
            // lblAdminSched
            // 
            this.lblAdminSched.Location = new System.Drawing.Point(222, 18);
            this.lblAdminSched.Name = "lblAdminSched";
            this.lblAdminSched.Size = new System.Drawing.Size(292, 51);
            this.lblAdminSched.TabIndex = 7;
            this.lblAdminSched.Text = "---";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(203, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Schedule for sending Admin Notifications:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label21);
            this.tabPage3.Controls.Add(this.txtMessageDisclaimer);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.txtAdminName);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.txtAdminAccess);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.lstCert);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(694, 341);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Other";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 97);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(143, 13);
            this.label21.TabIndex = 9;
            this.label21.Text = "Disclaimer in Send Message:";
            // 
            // txtMessageDisclaimer
            // 
            this.txtMessageDisclaimer.AcceptsReturn = true;
            this.txtMessageDisclaimer.Location = new System.Drawing.Point(221, 94);
            this.txtMessageDisclaimer.Multiline = true;
            this.txtMessageDisclaimer.Name = "txtMessageDisclaimer";
            this.txtMessageDisclaimer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessageDisclaimer.Size = new System.Drawing.Size(359, 98);
            this.txtMessageDisclaimer.TabIndex = 8;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 71);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(104, 13);
            this.label20.TabIndex = 7;
            this.label20.Text = "Administrators name:";
            // 
            // txtAdminName
            // 
            this.txtAdminName.Location = new System.Drawing.Point(221, 68);
            this.txtAdminName.Name = "txtAdminName";
            this.txtAdminName.Size = new System.Drawing.Size(359, 20);
            this.txtAdminName.TabIndex = 6;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 45);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(206, 13);
            this.label18.TabIndex = 5;
            this.label18.Text = "Allow administrative access within this net:";
            // 
            // txtAdminAccess
            // 
            this.txtAdminAccess.Location = new System.Drawing.Point(221, 42);
            this.txtAdminAccess.Name = "txtAdminAccess";
            this.txtAdminAccess.Size = new System.Drawing.Size(359, 20);
            this.txtAdminAccess.TabIndex = 4;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lblAnchor);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(694, 341);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Report Papers";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lblAnchor
            // 
            this.lblAnchor.AutoSize = true;
            this.lblAnchor.Location = new System.Drawing.Point(6, 3);
            this.lblAnchor.Name = "lblAnchor";
            this.lblAnchor.Size = new System.Drawing.Size(71, 13);
            this.lblAnchor.TabIndex = 0;
            this.lblAnchor.Text = "---ANCHOR---";
            this.lblAnchor.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 122);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(122, 13);
            this.label23.TabIndex = 12;
            this.label23.Text = "Keep Chat logs for days:";
            // 
            // txtKeepChatLog
            // 
            this.txtKeepChatLog.Location = new System.Drawing.Point(222, 119);
            this.txtKeepChatLog.Name = "txtKeepChatLog";
            this.txtKeepChatLog.Size = new System.Drawing.Size(100, 20);
            this.txtKeepChatLog.TabIndex = 4;
            // 
            // frmSeverSettings
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(726, 420);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSeverSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sever Settings";
            this.Load += new System.EventHandler(this.SeverSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstCert;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEventLogFlush;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMailFromName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMailFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMailPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMailServer;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMailAdminAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMailPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMailUsername;
        private System.Windows.Forms.CheckBox chkEMailUseSSL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button cmdSetScheduleClient;
        private System.Windows.Forms.Label lblClientSched;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button cmdSetScheduleAdmin;
        private System.Windows.Forms.Label lblAdminSched;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtKeepDisks;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label lblAnchor;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.CheckBox chkClientEMailTextIsHTML;
        private System.Windows.Forms.CheckBox chkAdminEMailTextIsHTML;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtClientEMailText;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtAdminEMailText;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtEMailClientSubject;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtEMailAdminSubject;
        private System.Windows.Forms.Button cmdRunAdminReportNow;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtAdminAccess;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtKeepReports;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtAdminName;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtMessageDisclaimer;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtKeepBitlockerRK;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtKeepChatLog;
    }
}