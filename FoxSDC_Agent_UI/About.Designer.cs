namespace FoxSDC_Agent_UI
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTitle2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAgentVer = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblServerVer = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblUCID = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblComputername = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblContract = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.lblLicOwner = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLicID = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLicCustomID = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblMemoriam = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle1
            // 
            this.lblTitle1.AutoSize = true;
            this.lblTitle1.ContextMenuStrip = this.contextMenuStrip1;
            this.lblTitle1.Location = new System.Drawing.Point(12, 9);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(64, 13);
            this.lblTitle1.TabIndex = 0;
            this.lblTitle1.Text = "Vulpes SDC";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(114, 28);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // lblTitle2
            // 
            this.lblTitle2.AutoSize = true;
            this.lblTitle2.ContextMenuStrip = this.contextMenuStrip1;
            this.lblTitle2.Location = new System.Drawing.Point(12, 22);
            this.lblTitle2.Name = "lblTitle2";
            this.lblTitle2.Size = new System.Drawing.Size(165, 13);
            this.lblTitle2.TabIndex = 1;
            this.lblTitle2.Text = "Software Deployment and Control";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ContextMenuStrip = this.contextMenuStrip1;
            this.label3.Location = new System.Drawing.Point(226, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Agent Version:";
            // 
            // lblAgentVer
            // 
            this.lblAgentVer.AutoSize = true;
            this.lblAgentVer.ContextMenuStrip = this.contextMenuStrip1;
            this.lblAgentVer.Location = new System.Drawing.Point(335, 51);
            this.lblAgentVer.Name = "lblAgentVer";
            this.lblAgentVer.Size = new System.Drawing.Size(19, 13);
            this.lblAgentVer.TabIndex = 3;
            this.lblAgentVer.Text = "----";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.ContextMenuStrip = this.contextMenuStrip1;
            this.lblServer.Location = new System.Drawing.Point(335, 103);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(19, 13);
            this.lblServer.TabIndex = 5;
            this.lblServer.Text = "----";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ContextMenuStrip = this.contextMenuStrip1;
            this.label5.Location = new System.Drawing.Point(226, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Server:";
            // 
            // lblServerVer
            // 
            this.lblServerVer.AutoSize = true;
            this.lblServerVer.ContextMenuStrip = this.contextMenuStrip1;
            this.lblServerVer.Location = new System.Drawing.Point(335, 116);
            this.lblServerVer.Name = "lblServerVer";
            this.lblServerVer.Size = new System.Drawing.Size(19, 13);
            this.lblServerVer.TabIndex = 7;
            this.lblServerVer.Text = "----";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ContextMenuStrip = this.contextMenuStrip1;
            this.label6.Location = new System.Drawing.Point(226, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Server Version:";
            // 
            // lblUCID
            // 
            this.lblUCID.AutoSize = true;
            this.lblUCID.ContextMenuStrip = this.contextMenuStrip1;
            this.lblUCID.Location = new System.Drawing.Point(335, 64);
            this.lblUCID.Name = "lblUCID";
            this.lblUCID.Size = new System.Drawing.Size(19, 13);
            this.lblUCID.TabIndex = 9;
            this.lblUCID.Text = "----";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ContextMenuStrip = this.contextMenuStrip1;
            this.label7.Location = new System.Drawing.Point(226, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "UCID:";
            // 
            // lblComputername
            // 
            this.lblComputername.AutoSize = true;
            this.lblComputername.ContextMenuStrip = this.contextMenuStrip1;
            this.lblComputername.Location = new System.Drawing.Point(335, 77);
            this.lblComputername.Name = "lblComputername";
            this.lblComputername.Size = new System.Drawing.Size(19, 13);
            this.lblComputername.TabIndex = 13;
            this.lblComputername.Text = "----";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ContextMenuStrip = this.contextMenuStrip1;
            this.label8.Location = new System.Drawing.Point(226, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Name:";
            // 
            // lblContract
            // 
            this.lblContract.AutoSize = true;
            this.lblContract.ContextMenuStrip = this.contextMenuStrip1;
            this.lblContract.Location = new System.Drawing.Point(335, 90);
            this.lblContract.Name = "lblContract";
            this.lblContract.Size = new System.Drawing.Size(19, 13);
            this.lblContract.TabIndex = 15;
            this.lblContract.Text = "----";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ContextMenuStrip = this.contextMenuStrip1;
            this.label9.Location = new System.Drawing.Point(226, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Contract:";
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(481, 312);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 16;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // lblLicOwner
            // 
            this.lblLicOwner.AutoSize = true;
            this.lblLicOwner.ContextMenuStrip = this.contextMenuStrip1;
            this.lblLicOwner.Location = new System.Drawing.Point(335, 129);
            this.lblLicOwner.Name = "lblLicOwner";
            this.lblLicOwner.Size = new System.Drawing.Size(19, 13);
            this.lblLicOwner.TabIndex = 18;
            this.lblLicOwner.Text = "----";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ContextMenuStrip = this.contextMenuStrip1;
            this.label2.Location = new System.Drawing.Point(226, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "License owner:";
            // 
            // lblLicID
            // 
            this.lblLicID.AutoSize = true;
            this.lblLicID.ContextMenuStrip = this.contextMenuStrip1;
            this.lblLicID.Location = new System.Drawing.Point(335, 142);
            this.lblLicID.Name = "lblLicID";
            this.lblLicID.Size = new System.Drawing.Size(19, 13);
            this.lblLicID.TabIndex = 20;
            this.lblLicID.Text = "----";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ContextMenuStrip = this.contextMenuStrip1;
            this.label4.Location = new System.Drawing.Point(226, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "License ID:";
            // 
            // lblLicCustomID
            // 
            this.lblLicCustomID.AutoSize = true;
            this.lblLicCustomID.ContextMenuStrip = this.contextMenuStrip1;
            this.lblLicCustomID.Location = new System.Drawing.Point(335, 155);
            this.lblLicCustomID.Name = "lblLicCustomID";
            this.lblLicCustomID.Size = new System.Drawing.Size(19, 13);
            this.lblLicCustomID.TabIndex = 22;
            this.lblLicCustomID.Text = "----";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ContextMenuStrip = this.contextMenuStrip1;
            this.label10.Location = new System.Drawing.Point(226, 155);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "License Custom ID:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FoxSDC_Agent_UI.Properties.Resources.LogoAloneSquare256256;
            this.pictureBox1.Location = new System.Drawing.Point(-27, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(232, 282);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblMemoriam
            // 
            this.lblMemoriam.AutoSize = true;
            this.lblMemoriam.ContextMenuStrip = this.contextMenuStrip1;
            this.lblMemoriam.Location = new System.Drawing.Point(226, 317);
            this.lblMemoriam.Name = "lblMemoriam";
            this.lblMemoriam.Size = new System.Drawing.Size(16, 13);
            this.lblMemoriam.TabIndex = 23;
            this.lblMemoriam.Text = "---";
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(568, 347);
            this.Controls.Add(this.lblMemoriam);
            this.Controls.Add(this.lblLicCustomID);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblLicID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblLicOwner);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.lblContract);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblComputername);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblUCID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblServerVer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblAgentVer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTitle2);
            this.Controls.Add(this.lblTitle1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle1;
        private System.Windows.Forms.Label lblTitle2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAgentVer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblServerVer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblUCID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Label lblComputername;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblContract;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Label lblLicOwner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLicID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLicCustomID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblMemoriam;
    }
}