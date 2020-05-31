namespace FoxSDC_MGMT.Policies
{
    partial class ctlPortMapping
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtCliPort = new System.Windows.Forms.TextBox();
            this.chkMap0000 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkDontMapOnServer = new System.Windows.Forms.CheckBox();
            this.txtToPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtToServer = new System.Windows.Forms.TextBox();
            this.chkCreateHOSTS = new System.Windows.Forms.CheckBox();
            this.txtHostsEntry = new System.Windows.Forms.TextBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client Port:";
            // 
            // txtCliPort
            // 
            this.txtCliPort.Location = new System.Drawing.Point(96, 60);
            this.txtCliPort.Name = "txtCliPort";
            this.txtCliPort.Size = new System.Drawing.Size(100, 20);
            this.txtCliPort.TabIndex = 0;
            // 
            // chkMap0000
            // 
            this.chkMap0000.AutoSize = true;
            this.chkMap0000.Location = new System.Drawing.Point(96, 86);
            this.chkMap0000.Name = "chkMap0000";
            this.chkMap0000.Size = new System.Drawing.Size(189, 17);
            this.chkMap0000.TabIndex = 1;
            this.chkMap0000.Text = "Bind to 0.0.0.0 instead of localhost";
            this.chkMap0000.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(328, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server Side (seen from the server that has Fox SDC Server installed)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(220, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Client side (where Fox SDC Agent is installed)";
            // 
            // chkDontMapOnServer
            // 
            this.chkDontMapOnServer.AutoSize = true;
            this.chkDontMapOnServer.Location = new System.Drawing.Point(96, 109);
            this.chkDontMapOnServer.Name = "chkDontMapOnServer";
            this.chkDontMapOnServer.Size = new System.Drawing.Size(297, 17);
            this.chkDontMapOnServer.TabIndex = 2;
            this.chkDontMapOnServer.Text = "Do not map when Fox SDC Server is found on the system";
            this.chkDontMapOnServer.UseVisualStyleBackColor = true;
            // 
            // txtToPort
            // 
            this.txtToPort.Location = new System.Drawing.Point(96, 271);
            this.txtToPort.Name = "txtToPort";
            this.txtToPort.Size = new System.Drawing.Size(100, 20);
            this.txtToPort.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 274);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Port:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Server:";
            // 
            // txtToServer
            // 
            this.txtToServer.Location = new System.Drawing.Point(96, 245);
            this.txtToServer.Name = "txtToServer";
            this.txtToServer.Size = new System.Drawing.Size(225, 20);
            this.txtToServer.TabIndex = 5;
            // 
            // chkCreateHOSTS
            // 
            this.chkCreateHOSTS.AutoSize = true;
            this.chkCreateHOSTS.Location = new System.Drawing.Point(26, 141);
            this.chkCreateHOSTS.Name = "chkCreateHOSTS";
            this.chkCreateHOSTS.Size = new System.Drawing.Size(174, 17);
            this.chkCreateHOSTS.TabIndex = 3;
            this.chkCreateHOSTS.Text = "Create 127.0.0.1 HOSTS entry:";
            this.chkCreateHOSTS.UseVisualStyleBackColor = true;
            this.chkCreateHOSTS.CheckedChanged += new System.EventHandler(this.chkCreateHOSTS_CheckedChanged);
            // 
            // txtHostsEntry
            // 
            this.txtHostsEntry.Location = new System.Drawing.Point(96, 164);
            this.txtHostsEntry.Name = "txtHostsEntry";
            this.txtHostsEntry.Size = new System.Drawing.Size(225, 20);
            this.txtHostsEntry.TabIndex = 4;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(302, 307);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(91, 50);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(34, 13);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "---------";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(23, 317);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 40);
            this.label6.TabIndex = 11;
            this.label6.Text = "Note: these policies will NOT merge together, they will \"run\" on its own.";
            // 
            // ctlPortMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.txtHostsEntry);
            this.Controls.Add(this.chkCreateHOSTS);
            this.Controls.Add(this.txtToServer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtToPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkDontMapOnServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkMap0000);
            this.Controls.Add(this.txtCliPort);
            this.Controls.Add(this.label1);
            this.Name = "ctlPortMapping";
            this.Size = new System.Drawing.Size(441, 381);
            this.Load += new System.EventHandler(this.ctlPortMapping_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCliPort;
        private System.Windows.Forms.CheckBox chkMap0000;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDontMapOnServer;
        private System.Windows.Forms.TextBox txtToPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtToServer;
        private System.Windows.Forms.CheckBox chkCreateHOSTS;
        private System.Windows.Forms.TextBox txtHostsEntry;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label6;
    }
}
