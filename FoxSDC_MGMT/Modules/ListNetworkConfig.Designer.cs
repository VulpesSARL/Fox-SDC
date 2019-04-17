namespace FoxSDC_MGMT
{
    partial class ctlListNetworkConfig
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
            this.TVConfigs = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // TVConfigs
            // 
            this.TVConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TVConfigs.FullRowSelect = true;
            this.TVConfigs.Location = new System.Drawing.Point(0, 0);
            this.TVConfigs.Name = "TVConfigs";
            this.TVConfigs.Size = new System.Drawing.Size(584, 325);
            this.TVConfigs.TabIndex = 0;
            // 
            // ctlListNetworkConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TVConfigs);
            this.Name = "ctlListNetworkConfig";
            this.Size = new System.Drawing.Size(584, 325);
            this.Load += new System.EventHandler(this.ctlListNetworkConfig_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView TVConfigs;
    }
}
