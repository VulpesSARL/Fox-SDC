namespace FoxSDC_RemoteConnect
{
    partial class MainDLG
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDLG));
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lstComputer = new System.Windows.Forms.ComboBox();
            this.txtListenOn = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConnectTo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtConnectPort = new System.Windows.Forms.TextBox();
            this.cmdStart = new System.Windows.Forms.Button();
            this.panelLogin = new System.Windows.Forms.Panel();
            this.panelConnectData = new System.Windows.Forms.Panel();
            this.lblPing = new System.Windows.Forms.Label();
            this.picStatus = new System.Windows.Forms.PictureBox();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblRXTXStat = new System.Windows.Forms.Label();
            this.lblRXTXErr = new System.Windows.Forms.Label();
            this.lblTX = new System.Windows.Forms.Label();
            this.lblRX = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.bgwPinger = new System.ComponentModel.BackgroundWorker();
            this.timerPinger = new System.Windows.Forms.Timer(this.components);
            this.panelLogin.SuspendLayout();
            this.panelConnectData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
            this.panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(82, 67);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(296, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Password:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(82, 41);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(296, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Username:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(82, 6);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(296, 20);
            this.txtServer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Server:";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(303, 93);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(75, 23);
            this.cmdConnect.TabIndex = 3;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Computer:";
            // 
            // lstComputer
            // 
            this.lstComputer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstComputer.FormattingEnabled = true;
            this.lstComputer.Location = new System.Drawing.Point(82, 3);
            this.lstComputer.Name = "lstComputer";
            this.lstComputer.Size = new System.Drawing.Size(296, 21);
            this.lstComputer.Sorted = true;
            this.lstComputer.TabIndex = 0;
            this.lstComputer.SelectedIndexChanged += new System.EventHandler(this.lstComputer_SelectedIndexChanged);
            // 
            // txtListenOn
            // 
            this.txtListenOn.Location = new System.Drawing.Point(82, 62);
            this.txtListenOn.Name = "txtListenOn";
            this.txtListenOn.Size = new System.Drawing.Size(100, 20);
            this.txtListenOn.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Listen on:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Connect to:";
            // 
            // txtConnectTo
            // 
            this.txtConnectTo.Location = new System.Drawing.Point(82, 106);
            this.txtConnectTo.Name = "txtConnectTo";
            this.txtConnectTo.Size = new System.Drawing.Size(296, 20);
            this.txtConnectTo.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Port:";
            // 
            // txtConnectPort
            // 
            this.txtConnectPort.Location = new System.Drawing.Point(82, 132);
            this.txtConnectPort.Name = "txtConnectPort";
            this.txtConnectPort.Size = new System.Drawing.Size(100, 20);
            this.txtConnectPort.TabIndex = 3;
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(308, 158);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(75, 23);
            this.cmdStart.TabIndex = 4;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // panelLogin
            // 
            this.panelLogin.Controls.Add(this.label1);
            this.panelLogin.Controls.Add(this.txtServer);
            this.panelLogin.Controls.Add(this.label2);
            this.panelLogin.Controls.Add(this.txtUsername);
            this.panelLogin.Controls.Add(this.label3);
            this.panelLogin.Controls.Add(this.txtPassword);
            this.panelLogin.Controls.Add(this.cmdConnect);
            this.panelLogin.Location = new System.Drawing.Point(0, 0);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(398, 130);
            this.panelLogin.TabIndex = 0;
            // 
            // panelConnectData
            // 
            this.panelConnectData.Controls.Add(this.lblPing);
            this.panelConnectData.Controls.Add(this.picStatus);
            this.panelConnectData.Controls.Add(this.label4);
            this.panelConnectData.Controls.Add(this.lstComputer);
            this.panelConnectData.Controls.Add(this.cmdStart);
            this.panelConnectData.Controls.Add(this.txtListenOn);
            this.panelConnectData.Controls.Add(this.label7);
            this.panelConnectData.Controls.Add(this.label5);
            this.panelConnectData.Controls.Add(this.txtConnectPort);
            this.panelConnectData.Controls.Add(this.label6);
            this.panelConnectData.Controls.Add(this.txtConnectTo);
            this.panelConnectData.Location = new System.Drawing.Point(0, 136);
            this.panelConnectData.Name = "panelConnectData";
            this.panelConnectData.Size = new System.Drawing.Size(398, 192);
            this.panelConnectData.TabIndex = 1;
            // 
            // lblPing
            // 
            this.lblPing.AutoSize = true;
            this.lblPing.Location = new System.Drawing.Point(117, 36);
            this.lblPing.Name = "lblPing";
            this.lblPing.Size = new System.Drawing.Size(19, 13);
            this.lblPing.TabIndex = 23;
            this.lblPing.Text = "----";
            // 
            // picStatus
            // 
            this.picStatus.Location = new System.Drawing.Point(82, 27);
            this.picStatus.Margin = new System.Windows.Forms.Padding(0);
            this.picStatus.Name = "picStatus";
            this.picStatus.Size = new System.Drawing.Size(32, 32);
            this.picStatus.TabIndex = 22;
            this.picStatus.TabStop = false;
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.lblRXTXStat);
            this.panelStatus.Controls.Add(this.lblRXTXErr);
            this.panelStatus.Controls.Add(this.lblTX);
            this.panelStatus.Controls.Add(this.lblRX);
            this.panelStatus.Location = new System.Drawing.Point(0, 334);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(398, 50);
            this.panelStatus.TabIndex = 2;
            // 
            // lblRXTXStat
            // 
            this.lblRXTXStat.Location = new System.Drawing.Point(140, 6);
            this.lblRXTXStat.Name = "lblRXTXStat";
            this.lblRXTXStat.Size = new System.Drawing.Size(243, 31);
            this.lblRXTXStat.TabIndex = 26;
            this.lblRXTXStat.Text = "--------";
            // 
            // lblRXTXErr
            // 
            this.lblRXTXErr.ForeColor = System.Drawing.Color.Red;
            this.lblRXTXErr.Location = new System.Drawing.Point(79, 0);
            this.lblRXTXErr.Name = "lblRXTXErr";
            this.lblRXTXErr.Size = new System.Drawing.Size(25, 25);
            this.lblRXTXErr.TabIndex = 25;
            this.lblRXTXErr.Text = "??";
            this.lblRXTXErr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTX
            // 
            this.lblTX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblTX.Location = new System.Drawing.Point(43, 0);
            this.lblTX.Name = "lblTX";
            this.lblTX.Size = new System.Drawing.Size(25, 25);
            this.lblTX.TabIndex = 24;
            this.lblTX.Text = "TX";
            this.lblTX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRX
            // 
            this.lblRX.ForeColor = System.Drawing.Color.Blue;
            this.lblRX.Location = new System.Drawing.Point(13, 0);
            this.lblRX.Name = "lblRX";
            this.lblRX.Size = new System.Drawing.Size(25, 25);
            this.lblRX.TabIndex = 23;
            this.lblRX.Text = "RX";
            this.lblRX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(308, 390);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 22;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // bgwPinger
            // 
            this.bgwPinger.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwPinger_DoWork);
            // 
            // timerPinger
            // 
            this.timerPinger.Enabled = true;
            this.timerPinger.Interval = 5000;
            this.timerPinger.Tick += new System.EventHandler(this.timerPinger_Tick);
            // 
            // MainDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(400, 424);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelConnectData);
            this.Controls.Add(this.panelLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainDLG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fox SDC RC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDLG_FormClosing);
            this.Load += new System.EventHandler(this.MainDLG_Load);
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.panelConnectData.ResumeLayout(false);
            this.panelConnectData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
            this.panelStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox lstComputer;
        private System.Windows.Forms.TextBox txtListenOn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConnectTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtConnectPort;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Panel panelConnectData;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblTX;
        private System.Windows.Forms.Label lblRX;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Label lblRXTXErr;
        private System.Windows.Forms.Label lblPing;
        private System.Windows.Forms.PictureBox picStatus;
        private System.ComponentModel.BackgroundWorker bgwPinger;
        private System.Windows.Forms.Timer timerPinger;
        private System.Windows.Forms.Label lblRXTXStat;
    }
}

