namespace FoxSDC_SigningTool
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.signToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.signPlainfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.signPlainFilewithcardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.certificateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPlainCERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.signToolStripMenuItem,
            this.certificateToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(639, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(45, 24);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(104, 24);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // signToolStripMenuItem
            // 
            this.signToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.signPlainfileToolStripMenuItem,
            this.signPlainFilewithcardToolStripMenuItem});
            this.signToolStripMenuItem.Name = "signToolStripMenuItem";
            this.signToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.signToolStripMenuItem.Text = "&Sign";
            // 
            // signPlainfileToolStripMenuItem
            // 
            this.signPlainfileToolStripMenuItem.Name = "signPlainfileToolStripMenuItem";
            this.signPlainfileToolStripMenuItem.Size = new System.Drawing.Size(253, 24);
            this.signPlainfileToolStripMenuItem.Text = "Sign plain &file";
            this.signPlainfileToolStripMenuItem.Click += new System.EventHandler(this.signPlainfileToolStripMenuItem_Click);
            // 
            // signPlainFilewithcardToolStripMenuItem
            // 
            this.signPlainFilewithcardToolStripMenuItem.Name = "signPlainFilewithcardToolStripMenuItem";
            this.signPlainFilewithcardToolStripMenuItem.Size = new System.Drawing.Size(253, 24);
            this.signPlainFilewithcardToolStripMenuItem.Text = "Sign plain file (with &card)";
            this.signPlainFilewithcardToolStripMenuItem.Click += new System.EventHandler(this.signPlainFilewithcardToolStripMenuItem_Click);
            // 
            // certificateToolStripMenuItem
            // 
            this.certificateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createPlainCERToolStripMenuItem});
            this.certificateToolStripMenuItem.Name = "certificateToolStripMenuItem";
            this.certificateToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.certificateToolStripMenuItem.Text = "&Certificate";
            // 
            // createPlainCERToolStripMenuItem
            // 
            this.createPlainCERToolStripMenuItem.Name = "createPlainCERToolStripMenuItem";
            this.createPlainCERToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.createPlainCERToolStripMenuItem.Text = "&Create certificate";
            this.createPlainCERToolStripMenuItem.Click += new System.EventHandler(this.createPlainCERToolStripMenuItem_Click);
            // 
            // MainDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 507);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainDLG";
            this.Text = "Fox Signing Tool [NDA]";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem signToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem certificateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createPlainCERToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem signPlainFilewithcardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem signPlainfileToolStripMenuItem;
    }
}

