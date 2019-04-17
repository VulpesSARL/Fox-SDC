namespace FoxSDC_MGMT
{
    partial class ctlReporting
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
            this.lstType = new System.Windows.Forms.ComboBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.lstItems = new System.Windows.Forms.ListBox();
            this.cmdAddItem = new System.Windows.Forms.Button();
            this.cmdDeleteItem = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.cmdEdit = new System.Windows.Forms.Button();
            this.chkReportToAdmin = new System.Windows.Forms.CheckBox();
            this.chkUrgentReportAdmin = new System.Windows.Forms.CheckBox();
            this.chkUrgentReportToClient = new System.Windows.Forms.CheckBox();
            this.chkReportToClient = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type:";
            // 
            // lstType
            // 
            this.lstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstType.FormattingEnabled = true;
            this.lstType.Location = new System.Drawing.Point(59, 17);
            this.lstType.Name = "lstType";
            this.lstType.Size = new System.Drawing.Size(251, 21);
            this.lstType.TabIndex = 0;
            this.lstType.SelectedIndexChanged += new System.EventHandler(this.lstType_SelectedIndexChanged);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(503, 359);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(100, 37);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.Text = "&Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // lstItems
            // 
            this.lstItems.FormattingEnabled = true;
            this.lstItems.IntegralHeight = false;
            this.lstItems.Location = new System.Drawing.Point(59, 44);
            this.lstItems.Name = "lstItems";
            this.lstItems.ScrollAlwaysVisible = true;
            this.lstItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstItems.Size = new System.Drawing.Size(544, 249);
            this.lstItems.TabIndex = 1;
            this.lstItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstItems_KeyDown);
            this.lstItems.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstItems_MouseDoubleClick);
            // 
            // cmdAddItem
            // 
            this.cmdAddItem.Location = new System.Drawing.Point(447, 299);
            this.cmdAddItem.Name = "cmdAddItem";
            this.cmdAddItem.Size = new System.Drawing.Size(75, 23);
            this.cmdAddItem.TabIndex = 3;
            this.cmdAddItem.Text = "&Add";
            this.cmdAddItem.UseVisualStyleBackColor = true;
            this.cmdAddItem.Click += new System.EventHandler(this.cmdAddItem_Click);
            // 
            // cmdDeleteItem
            // 
            this.cmdDeleteItem.Location = new System.Drawing.Point(528, 299);
            this.cmdDeleteItem.Name = "cmdDeleteItem";
            this.cmdDeleteItem.Size = new System.Drawing.Size(75, 23);
            this.cmdDeleteItem.TabIndex = 4;
            this.cmdDeleteItem.Text = "&Delete";
            this.cmdDeleteItem.UseVisualStyleBackColor = true;
            this.cmdDeleteItem.Click += new System.EventHandler(this.cmdDeleteItem_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(19, 13);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "----";
            // 
            // cmdEdit
            // 
            this.cmdEdit.Location = new System.Drawing.Point(366, 299);
            this.cmdEdit.Name = "cmdEdit";
            this.cmdEdit.Size = new System.Drawing.Size(75, 23);
            this.cmdEdit.TabIndex = 2;
            this.cmdEdit.Text = "&Edit";
            this.cmdEdit.UseVisualStyleBackColor = true;
            this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
            // 
            // chkReportToAdmin
            // 
            this.chkReportToAdmin.AutoSize = true;
            this.chkReportToAdmin.Location = new System.Drawing.Point(59, 305);
            this.chkReportToAdmin.Name = "chkReportToAdmin";
            this.chkReportToAdmin.Size = new System.Drawing.Size(102, 17);
            this.chkReportToAdmin.TabIndex = 5;
            this.chkReportToAdmin.Text = "Report to Admin";
            this.chkReportToAdmin.UseVisualStyleBackColor = true;
            // 
            // chkUrgentReportAdmin
            // 
            this.chkUrgentReportAdmin.AutoSize = true;
            this.chkUrgentReportAdmin.Location = new System.Drawing.Point(59, 328);
            this.chkUrgentReportAdmin.Name = "chkUrgentReportAdmin";
            this.chkUrgentReportAdmin.Size = new System.Drawing.Size(137, 17);
            this.chkUrgentReportAdmin.TabIndex = 6;
            this.chkUrgentReportAdmin.Text = "Urgent Report to Admin";
            this.chkUrgentReportAdmin.UseVisualStyleBackColor = true;
            // 
            // chkUrgentReportToClient
            // 
            this.chkUrgentReportToClient.AutoSize = true;
            this.chkUrgentReportToClient.Location = new System.Drawing.Point(59, 374);
            this.chkUrgentReportToClient.Name = "chkUrgentReportToClient";
            this.chkUrgentReportToClient.Size = new System.Drawing.Size(134, 17);
            this.chkUrgentReportToClient.TabIndex = 8;
            this.chkUrgentReportToClient.Text = "Urgent Report to Client";
            this.chkUrgentReportToClient.UseVisualStyleBackColor = true;
            // 
            // chkReportToClient
            // 
            this.chkReportToClient.AutoSize = true;
            this.chkReportToClient.Location = new System.Drawing.Point(59, 351);
            this.chkReportToClient.Name = "chkReportToClient";
            this.chkReportToClient.Size = new System.Drawing.Size(99, 17);
            this.chkReportToClient.TabIndex = 7;
            this.chkReportToClient.Text = "Report to Client";
            this.chkReportToClient.UseVisualStyleBackColor = true;
            // 
            // ctlReporting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkUrgentReportToClient);
            this.Controls.Add(this.chkReportToClient);
            this.Controls.Add(this.chkUrgentReportAdmin);
            this.Controls.Add(this.chkReportToAdmin);
            this.Controls.Add(this.cmdEdit);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cmdDeleteItem);
            this.Controls.Add(this.cmdAddItem);
            this.Controls.Add(this.lstItems);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.lstType);
            this.Controls.Add(this.label1);
            this.Name = "ctlReporting";
            this.Size = new System.Drawing.Size(638, 418);
            this.Load += new System.EventHandler(this.ctlReporting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstType;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.ListBox lstItems;
        private System.Windows.Forms.Button cmdAddItem;
        private System.Windows.Forms.Button cmdDeleteItem;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button cmdEdit;
        private System.Windows.Forms.CheckBox chkReportToAdmin;
        private System.Windows.Forms.CheckBox chkUrgentReportAdmin;
        private System.Windows.Forms.CheckBox chkUrgentReportToClient;
        private System.Windows.Forms.CheckBox chkReportToClient;
    }
}
