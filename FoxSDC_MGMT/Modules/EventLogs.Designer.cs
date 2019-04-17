namespace FoxSDC_MGMT
{
    partial class ctlEventLogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlEventLogs));
            this.label1 = new System.Windows.Forms.Label();
            this.lblComputer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstSources = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkDTFrom = new System.Windows.Forms.CheckBox();
            this.DTFrom = new System.Windows.Forms.DateTimePicker();
            this.DTTo = new System.Windows.Forms.DateTimePicker();
            this.chkDTTo = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblResults = new System.Windows.Forms.Label();
            this.txtEventID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmdQuery = new System.Windows.Forms.Button();
            this.txtQTY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lstStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lstBook = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lstData = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtEventLogText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Computer:";
            // 
            // lblComputer
            // 
            this.lblComputer.AutoSize = true;
            this.lblComputer.Location = new System.Drawing.Point(120, 9);
            this.lblComputer.Name = "lblComputer";
            this.lblComputer.Size = new System.Drawing.Size(19, 13);
            this.lblComputer.TabIndex = 0;
            this.lblComputer.Text = "----";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source:";
            // 
            // lstSources
            // 
            this.lstSources.FormattingEnabled = true;
            this.lstSources.Location = new System.Drawing.Point(123, 25);
            this.lstSources.Name = "lstSources";
            this.lstSources.Size = new System.Drawing.Size(381, 21);
            this.lstSources.TabIndex = 0;
            this.lstSources.DropDown += new System.EventHandler(this.lstSources_DropDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Date:";
            // 
            // chkDTFrom
            // 
            this.chkDTFrom.AutoSize = true;
            this.chkDTFrom.Location = new System.Drawing.Point(153, 58);
            this.chkDTFrom.Name = "chkDTFrom";
            this.chkDTFrom.Size = new System.Drawing.Size(15, 14);
            this.chkDTFrom.TabIndex = 1;
            this.chkDTFrom.UseVisualStyleBackColor = true;
            this.chkDTFrom.CheckedChanged += new System.EventHandler(this.chkDTFrom_CheckedChanged);
            // 
            // DTFrom
            // 
            this.DTFrom.Location = new System.Drawing.Point(174, 52);
            this.DTFrom.Name = "DTFrom";
            this.DTFrom.Size = new System.Drawing.Size(330, 20);
            this.DTFrom.TabIndex = 2;
            // 
            // DTTo
            // 
            this.DTTo.Location = new System.Drawing.Point(174, 78);
            this.DTTo.Name = "DTTo";
            this.DTTo.Size = new System.Drawing.Size(330, 20);
            this.DTTo.TabIndex = 4;
            // 
            // chkDTTo
            // 
            this.chkDTTo.AutoSize = true;
            this.chkDTTo.Location = new System.Drawing.Point(153, 84);
            this.chkDTTo.Name = "chkDTTo";
            this.chkDTTo.Size = new System.Drawing.Size(15, 14);
            this.chkDTTo.TabIndex = 3;
            this.chkDTTo.UseVisualStyleBackColor = true;
            this.chkDTTo.CheckedChanged += new System.EventHandler(this.chkDTTo_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "from";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(120, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "to";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblResults);
            this.splitContainer1.Panel1.Controls.Add(this.txtEventID);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.cmdQuery);
            this.splitContainer1.Panel1.Controls.Add(this.txtQTY);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.lstStatus);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.lstBook);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.chkDTFrom);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.DTTo);
            this.splitContainer1.Panel1.Controls.Add(this.lstSources);
            this.splitContainer1.Panel1.Controls.Add(this.chkDTTo);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.DTFrom);
            this.splitContainer1.Panel1.Controls.Add(this.lblComputer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(950, 668);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 11;
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(120, 207);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(19, 13);
            this.lblResults.TabIndex = 18;
            this.lblResults.Text = "----";
            // 
            // txtEventID
            // 
            this.txtEventID.Location = new System.Drawing.Point(123, 158);
            this.txtEventID.Name = "txtEventID";
            this.txtEventID.Size = new System.Drawing.Size(381, 20);
            this.txtEventID.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Event ID:";
            // 
            // cmdQuery
            // 
            this.cmdQuery.Location = new System.Drawing.Point(553, 182);
            this.cmdQuery.Name = "cmdQuery";
            this.cmdQuery.Size = new System.Drawing.Size(75, 23);
            this.cmdQuery.TabIndex = 9;
            this.cmdQuery.Text = "Query";
            this.cmdQuery.UseVisualStyleBackColor = true;
            this.cmdQuery.Click += new System.EventHandler(this.cmdQuery_Click);
            // 
            // txtQTY
            // 
            this.txtQTY.Location = new System.Drawing.Point(123, 184);
            this.txtQTY.Name = "txtQTY";
            this.txtQTY.Size = new System.Drawing.Size(381, 20);
            this.txtQTY.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 187);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "QTY:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status:";
            // 
            // lstStatus
            // 
            this.lstStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstStatus.FormattingEnabled = true;
            this.lstStatus.Location = new System.Drawing.Point(123, 131);
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.Size = new System.Drawing.Size(381, 21);
            this.lstStatus.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Book:";
            // 
            // lstBook
            // 
            this.lstBook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstBook.FormattingEnabled = true;
            this.lstBook.Location = new System.Drawing.Point(123, 104);
            this.lstBook.Name = "lstBook";
            this.lstBook.Size = new System.Drawing.Size(381, 21);
            this.lstBook.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lstData);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtEventLogText);
            this.splitContainer2.Size = new System.Drawing.Size(950, 428);
            this.splitContainer2.SplitterDistance = 209;
            this.splitContainer2.TabIndex = 0;
            // 
            // lstData
            // 
            this.lstData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lstData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstData.FullRowSelect = true;
            this.lstData.HideSelection = false;
            this.lstData.Location = new System.Drawing.Point(0, 0);
            this.lstData.MultiSelect = false;
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(950, 209);
            this.lstData.SmallImageList = this.imageList1;
            this.lstData.TabIndex = 2;
            this.lstData.UseCompatibleStateImageBehavior = false;
            this.lstData.View = System.Windows.Forms.View.Details;
            this.lstData.SelectedIndexChanged += new System.EventHandler(this.lstData_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Level";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Date & Time";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Computer";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Book";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Source";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Event ID";
            this.columnHeader6.Width = 100;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Lime;
            this.imageList1.Images.SetKeyName(0, "Eventvwr103.bmp");
            this.imageList1.Images.SetKeyName(1, "Eventvwr104.bmp");
            this.imageList1.Images.SetKeyName(2, "Eventvwr105.bmp");
            this.imageList1.Images.SetKeyName(3, "Eventvwr106.bmp");
            this.imageList1.Images.SetKeyName(4, "Eventvwr107.bmp");
            // 
            // txtEventLogText
            // 
            this.txtEventLogText.BackColor = System.Drawing.SystemColors.Window;
            this.txtEventLogText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEventLogText.Location = new System.Drawing.Point(0, 0);
            this.txtEventLogText.Multiline = true;
            this.txtEventLogText.Name = "txtEventLogText";
            this.txtEventLogText.ReadOnly = true;
            this.txtEventLogText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEventLogText.Size = new System.Drawing.Size(950, 215);
            this.txtEventLogText.TabIndex = 0;
            // 
            // ctlEventLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ctlEventLogs";
            this.Size = new System.Drawing.Size(950, 668);
            this.Load += new System.EventHandler(this.ctlEventLogs_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblComputer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox lstSources;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDTFrom;
        private System.Windows.Forms.DateTimePicker DTFrom;
        private System.Windows.Forms.DateTimePicker DTTo;
        private System.Windows.Forms.CheckBox chkDTTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox lstBook;
        private System.Windows.Forms.TextBox txtQTY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox lstStatus;
        private System.Windows.Forms.Button cmdQuery;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lstData;
        private System.Windows.Forms.TextBox txtEventLogText;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TextBox txtEventID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblResults;
    }
}
