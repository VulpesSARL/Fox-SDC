namespace FoxSDC_Agent_UI
{
    partial class frmChat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChat));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtRecvText = new System.Windows.Forms.TextBox();
            this.txtSendText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdSend = new System.Windows.Forms.Button();
            this.timPull = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtRecvText);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtSendText);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(350, 156);
            this.splitContainer1.SplitterDistance = 79;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtRecvText
            // 
            this.txtRecvText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecvText.Location = new System.Drawing.Point(0, 0);
            this.txtRecvText.Multiline = true;
            this.txtRecvText.Name = "txtRecvText";
            this.txtRecvText.ReadOnly = true;
            this.txtRecvText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecvText.Size = new System.Drawing.Size(350, 79);
            this.txtRecvText.TabIndex = 0;
            // 
            // txtSendText
            // 
            this.txtSendText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendText.Location = new System.Drawing.Point(0, 0);
            this.txtSendText.Multiline = true;
            this.txtSendText.Name = "txtSendText";
            this.txtSendText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSendText.Size = new System.Drawing.Size(285, 73);
            this.txtSendText.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdSend);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(285, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(65, 73);
            this.panel1.TabIndex = 1;
            // 
            // cmdSend
            // 
            this.cmdSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdSend.Location = new System.Drawing.Point(10, 10);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(45, 53);
            this.cmdSend.TabIndex = 0;
            this.cmdSend.Text = "Send";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // timPull
            // 
            this.timPull.Interval = 1000;
            this.timPull.Tick += new System.EventHandler(this.timPull_Tick);
            // 
            // frmChat
            // 
            this.AcceptButton = this.cmdSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 156);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmChat_FormClosing);
            this.Load += new System.EventHandler(this.frmChat_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRecvText;
        private System.Windows.Forms.TextBox txtSendText;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.Timer timPull;
    }
}