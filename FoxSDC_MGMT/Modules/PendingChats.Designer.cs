namespace FoxSDC_MGMT
{
    partial class ctlPendingChats
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlPendingChats));
            this.timUpdate = new System.Windows.Forms.Timer(this.components);
            this.lstComputers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // timUpdate
            // 
            this.timUpdate.Enabled = true;
            this.timUpdate.Interval = 30000;
            this.timUpdate.Tick += new System.EventHandler(this.timUpdate_Tick);
            // 
            // lstComputers
            // 
            this.lstComputers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstComputers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstComputers.FullRowSelect = true;
            this.lstComputers.HideSelection = false;
            this.lstComputers.Location = new System.Drawing.Point(0, 0);
            this.lstComputers.MultiSelect = false;
            this.lstComputers.Name = "lstComputers";
            this.lstComputers.Size = new System.Drawing.Size(390, 346);
            this.lstComputers.SmallImageList = this.imageList1;
            this.lstComputers.TabIndex = 0;
            this.lstComputers.UseCompatibleStateImageBehavior = false;
            this.lstComputers.View = System.Windows.Forms.View.Details;
            this.lstComputers.DoubleClick += new System.EventHandler(this.lstComputers_DoubleClick);
            this.lstComputers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstComputers_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Computer";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Group";
            this.columnHeader2.Width = 200;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Computer.ico");
            // 
            // ctlPendingChats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstComputers);
            this.Name = "ctlPendingChats";
            this.Size = new System.Drawing.Size(390, 346);
            this.Load += new System.EventHandler(this.ctlPendingChats_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timUpdate;
        private System.Windows.Forms.ListView lstComputers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
