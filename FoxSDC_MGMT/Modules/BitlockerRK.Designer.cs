namespace FoxSDC_MGMT
{
    partial class ctlBitlockerRK
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdGetKeys = new System.Windows.Forms.Button();
            this.tvRK = new System.Windows.Forms.TreeView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdGetKeys);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(702, 32);
            this.panel1.TabIndex = 0;
            // 
            // cmdGetKeys
            // 
            this.cmdGetKeys.Location = new System.Drawing.Point(3, 3);
            this.cmdGetKeys.Name = "cmdGetKeys";
            this.cmdGetKeys.Size = new System.Drawing.Size(126, 23);
            this.cmdGetKeys.TabIndex = 0;
            this.cmdGetKeys.Text = "Get keys";
            this.cmdGetKeys.UseVisualStyleBackColor = true;
            this.cmdGetKeys.Click += new System.EventHandler(this.cmdGetKeys_Click);
            // 
            // tvRK
            // 
            this.tvRK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRK.FullRowSelect = true;
            this.tvRK.Location = new System.Drawing.Point(0, 32);
            this.tvRK.Name = "tvRK";
            this.tvRK.Size = new System.Drawing.Size(702, 522);
            this.tvRK.TabIndex = 1;
            // 
            // ctlBitlockerRK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvRK);
            this.Controls.Add(this.panel1);
            this.Name = "ctlBitlockerRK";
            this.Size = new System.Drawing.Size(702, 554);
            this.Load += new System.EventHandler(this.ctlBitlockerRK_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdGetKeys;
        private System.Windows.Forms.TreeView tvRK;
    }
}
