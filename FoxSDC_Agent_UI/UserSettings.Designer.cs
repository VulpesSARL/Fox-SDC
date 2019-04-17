namespace FoxSDC_Agent_UI
{
    partial class frmUserSettings
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
            this.cmdCanccel = new System.Windows.Forms.Button();
            this.chkBlinkTitle = new System.Windows.Forms.CheckBox();
            this.chkAudibleAlert = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkChatAOT = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(205, 121);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCanccel
            // 
            this.cmdCanccel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCanccel.Location = new System.Drawing.Point(124, 121);
            this.cmdCanccel.Name = "cmdCanccel";
            this.cmdCanccel.Size = new System.Drawing.Size(75, 23);
            this.cmdCanccel.TabIndex = 4;
            this.cmdCanccel.Text = "Cancel";
            this.cmdCanccel.UseVisualStyleBackColor = true;
            this.cmdCanccel.Click += new System.EventHandler(this.cmdCanccel_Click);
            // 
            // chkBlinkTitle
            // 
            this.chkBlinkTitle.AutoSize = true;
            this.chkBlinkTitle.Location = new System.Drawing.Point(12, 12);
            this.chkBlinkTitle.Name = "chkBlinkTitle";
            this.chkBlinkTitle.Size = new System.Drawing.Size(179, 17);
            this.chkBlinkTitle.TabIndex = 0;
            this.chkBlinkTitle.Text = "&Blink titlebar when a chat arrives";
            this.chkBlinkTitle.UseVisualStyleBackColor = true;
            // 
            // chkAudibleAlert
            // 
            this.chkAudibleAlert.AutoSize = true;
            this.chkAudibleAlert.Location = new System.Drawing.Point(12, 35);
            this.chkAudibleAlert.Name = "chkAudibleAlert";
            this.chkAudibleAlert.Size = new System.Drawing.Size(180, 17);
            this.chkAudibleAlert.TabIndex = 1;
            this.chkAudibleAlert.Text = "&Audible alert when a chat arrives";
            this.chkAudibleAlert.UseVisualStyleBackColor = true;
            this.chkAudibleAlert.CheckedChanged += new System.EventHandler(this.chkAudibleAlert_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Hint: these settings are stored per User on this computer";
            // 
            // chkChatAOT
            // 
            this.chkChatAOT.AutoSize = true;
            this.chkChatAOT.Location = new System.Drawing.Point(12, 58);
            this.chkChatAOT.Name = "chkChatAOT";
            this.chkChatAOT.Size = new System.Drawing.Size(155, 17);
            this.chkChatAOT.TabIndex = 2;
            this.chkChatAOT.Text = "Chat &window always on top";
            this.chkChatAOT.UseVisualStyleBackColor = true;
            // 
            // frmUserSettings
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCanccel;
            this.ClientSize = new System.Drawing.Size(301, 157);
            this.Controls.Add(this.chkChatAOT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkAudibleAlert);
            this.Controls.Add(this.chkBlinkTitle);
            this.Controls.Add(this.cmdCanccel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmUserSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCanccel;
        private System.Windows.Forms.CheckBox chkBlinkTitle;
        private System.Windows.Forms.CheckBox chkAudibleAlert;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkChatAOT;
    }
}