namespace FoxSDC_ManageScreen
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeClipboardAsTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendLayoutToRemoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableInputHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapshotToDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squeezePictureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cTRLALTDELETEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cTRLALTDELETEVKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cTRLSHIFTESCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtStatus = new System.Windows.Forms.ToolStripTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.timPing = new System.Windows.Forms.Timer(this.components);
            this.slowRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.txtStatus});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(820, 31);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetConnectionToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 27);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // resetConnectionToolStripMenuItem
            // 
            this.resetConnectionToolStripMenuItem.Name = "resetConnectionToolStripMenuItem";
            this.resetConnectionToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
            this.resetConnectionToolStripMenuItem.Text = "&Reset Connection";
            this.resetConnectionToolStripMenuItem.Click += new System.EventHandler(this.resetConnectionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(197, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.typeClipboardAsTextToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(48, 27);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // typeClipboardAsTextToolStripMenuItem
            // 
            this.typeClipboardAsTextToolStripMenuItem.Name = "typeClipboardAsTextToolStripMenuItem";
            this.typeClipboardAsTextToolStripMenuItem.Size = new System.Drawing.Size(233, 24);
            this.typeClipboardAsTextToolStripMenuItem.Text = "Type Clipboard as text";
            this.typeClipboardAsTextToolStripMenuItem.Click += new System.EventHandler(this.typeClipboardAsTextToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendLayoutToRemoteToolStripMenuItem,
            this.disableInputHereToolStripMenuItem,
            this.refreshScreenToolStripMenuItem,
            this.snapshotToDesktopToolStripMenuItem,
            this.squeezePictureToolStripMenuItem,
            this.slowRefreshToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(76, 27);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.optionsToolStripMenuItem_DropDownOpening);
            // 
            // sendLayoutToRemoteToolStripMenuItem
            // 
            this.sendLayoutToRemoteToolStripMenuItem.Name = "sendLayoutToRemoteToolStripMenuItem";
            this.sendLayoutToRemoteToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.sendLayoutToRemoteToolStripMenuItem.Text = "&Send layout to remote";
            this.sendLayoutToRemoteToolStripMenuItem.Click += new System.EventHandler(this.sendLayoutToRemoteToolStripMenuItem_Click);
            // 
            // disableInputHereToolStripMenuItem
            // 
            this.disableInputHereToolStripMenuItem.Name = "disableInputHereToolStripMenuItem";
            this.disableInputHereToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.disableInputHereToolStripMenuItem.Text = "&Disable input here";
            this.disableInputHereToolStripMenuItem.Click += new System.EventHandler(this.disableInputHereToolStripMenuItem_Click);
            // 
            // refreshScreenToolStripMenuItem
            // 
            this.refreshScreenToolStripMenuItem.Name = "refreshScreenToolStripMenuItem";
            this.refreshScreenToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.refreshScreenToolStripMenuItem.Text = "&Refresh screen";
            this.refreshScreenToolStripMenuItem.Click += new System.EventHandler(this.refreshScreenToolStripMenuItem_Click);
            // 
            // snapshotToDesktopToolStripMenuItem
            // 
            this.snapshotToDesktopToolStripMenuItem.Name = "snapshotToDesktopToolStripMenuItem";
            this.snapshotToDesktopToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.snapshotToDesktopToolStripMenuItem.Text = "S&napshot to desktop";
            this.snapshotToDesktopToolStripMenuItem.Click += new System.EventHandler(this.snapshotToDesktopToolStripMenuItem_Click);
            // 
            // squeezePictureToolStripMenuItem
            // 
            this.squeezePictureToolStripMenuItem.Name = "squeezePictureToolStripMenuItem";
            this.squeezePictureToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.squeezePictureToolStripMenuItem.Text = "S&queeze picture";
            this.squeezePictureToolStripMenuItem.Click += new System.EventHandler(this.squeezePictureToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cTRLALTDELETEToolStripMenuItem,
            this.cTRLALTDELETEVKToolStripMenuItem,
            this.cTRLSHIFTESCToolStripMenuItem,
            this.windowsKeyToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(72, 27);
            this.toolStripMenuItem1.Text = "&Macros";
            // 
            // cTRLALTDELETEToolStripMenuItem
            // 
            this.cTRLALTDELETEToolStripMenuItem.Name = "cTRLALTDELETEToolStripMenuItem";
            this.cTRLALTDELETEToolStripMenuItem.Size = new System.Drawing.Size(258, 24);
            this.cTRLALTDELETEToolStripMenuItem.Text = "&CTRL+ALT+DELETE (SAS)";
            this.cTRLALTDELETEToolStripMenuItem.Click += new System.EventHandler(this.CTRLALTDELETEToolStripMenuItem_Click);
            // 
            // cTRLALTDELETEVKToolStripMenuItem
            // 
            this.cTRLALTDELETEVKToolStripMenuItem.Name = "cTRLALTDELETEVKToolStripMenuItem";
            this.cTRLALTDELETEVKToolStripMenuItem.Size = new System.Drawing.Size(258, 24);
            this.cTRLALTDELETEVKToolStripMenuItem.Text = "CTRL+ALT+DELETE (&VK)";
            this.cTRLALTDELETEVKToolStripMenuItem.Click += new System.EventHandler(this.cTRLALTDELETEVKToolStripMenuItem_Click);
            // 
            // cTRLSHIFTESCToolStripMenuItem
            // 
            this.cTRLSHIFTESCToolStripMenuItem.Name = "cTRLSHIFTESCToolStripMenuItem";
            this.cTRLSHIFTESCToolStripMenuItem.Size = new System.Drawing.Size(258, 24);
            this.cTRLSHIFTESCToolStripMenuItem.Text = "CTRL+&SHIFT+ESC";
            this.cTRLSHIFTESCToolStripMenuItem.Click += new System.EventHandler(this.CTRLSHIFTESCToolStripMenuItem_Click);
            // 
            // windowsKeyToolStripMenuItem
            // 
            this.windowsKeyToolStripMenuItem.Name = "windowsKeyToolStripMenuItem";
            this.windowsKeyToolStripMenuItem.Size = new System.Drawing.Size(258, 24);
            this.windowsKeyToolStripMenuItem.Text = "&Windows key";
            this.windowsKeyToolStripMenuItem.Click += new System.EventHandler(this.windowsKeyToolStripMenuItem_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(200, 27);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.picDisplay);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 611);
            this.panel1.TabIndex = 1;
            // 
            // picDisplay
            // 
            this.picDisplay.Location = new System.Drawing.Point(0, 0);
            this.picDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(508, 135);
            this.picDisplay.TabIndex = 0;
            this.picDisplay.TabStop = false;
            this.picDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseDown);
            this.picDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseMove);
            this.picDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseUp);
            // 
            // timPing
            // 
            this.timPing.Enabled = true;
            this.timPing.Interval = 60000;
            this.timPing.Tick += new System.EventHandler(this.timPing_Tick);
            // 
            // slowRefreshToolStripMenuItem
            // 
            this.slowRefreshToolStripMenuItem.Name = "slowRefreshToolStripMenuItem";
            this.slowRefreshToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.slowRefreshToolStripMenuItem.Text = "&Slow refresh";
            this.slowRefreshToolStripMenuItem.Click += new System.EventHandler(this.slowRefreshToolStripMenuItem_Click);
            // 
            // MainDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 642);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainDLG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fox SDC Remote Screen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDLG_FormClosing);
            this.Load += new System.EventHandler(this.MainDLG_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainDLG_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainDLG_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cTRLALTDELETEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendLayoutToRemoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableInputHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snapshotToDesktopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squeezePictureToolStripMenuItem;
        private System.Windows.Forms.Timer timPing;
        private System.Windows.Forms.ToolStripMenuItem cTRLSHIFTESCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cTRLALTDELETEVKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem typeClipboardAsTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripTextBox txtStatus;
        private System.Windows.Forms.ToolStripMenuItem slowRefreshToolStripMenuItem;
    }
}

