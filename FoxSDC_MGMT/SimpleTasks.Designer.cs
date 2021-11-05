namespace FoxSDC_MGMT
{
    partial class frmSimpleTasks
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
            this.lstPCs = new FoxSDC_MGMT.ctlListPCs();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelReg = new System.Windows.Forms.Panel();
            this.txtRegValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lstRegValueType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRegValueName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lstRegRoot = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lstRegAction = new System.Windows.Forms.ComboBox();
            this.txtRegFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panelRunApp = new System.Windows.Forms.Panel();
            this.txtRunUser = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRunArgs = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRunExec = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTZ = new System.Windows.Forms.Label();
            this.chkDontExec = new System.Windows.Forms.CheckBox();
            this.DTExecBefore = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lstType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblIntoGroup = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelReg.SuspendLayout();
            this.panelRunApp.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstPCs
            // 
            this.lstPCs.AutoSize = true;
            this.lstPCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPCs.ListOnly = false;
            this.lstPCs.Location = new System.Drawing.Point(0, 0);
            this.lstPCs.Name = "lstPCs";
            this.lstPCs.ShowCheckBoxes = false;
            this.lstPCs.Size = new System.Drawing.Size(858, 209);
            this.lstPCs.TabIndex = 0;
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
            this.splitContainer1.Panel1.Controls.Add(this.lstPCs);
            this.splitContainer1.Panel1.Controls.Add(this.lblIntoGroup);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelReg);
            this.splitContainer1.Panel2.Controls.Add(this.panelRunApp);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(858, 565);
            this.splitContainer1.SplitterDistance = 209;
            this.splitContainer1.TabIndex = 1;
            // 
            // panelReg
            // 
            this.panelReg.Controls.Add(this.txtRegValue);
            this.panelReg.Controls.Add(this.label10);
            this.panelReg.Controls.Add(this.lstRegValueType);
            this.panelReg.Controls.Add(this.label9);
            this.panelReg.Controls.Add(this.txtRegValueName);
            this.panelReg.Controls.Add(this.label8);
            this.panelReg.Controls.Add(this.lstRegRoot);
            this.panelReg.Controls.Add(this.label7);
            this.panelReg.Controls.Add(this.lstRegAction);
            this.panelReg.Controls.Add(this.txtRegFolder);
            this.panelReg.Controls.Add(this.label5);
            this.panelReg.Controls.Add(this.label6);
            this.panelReg.Location = new System.Drawing.Point(269, 116);
            this.panelReg.Name = "panelReg";
            this.panelReg.Size = new System.Drawing.Size(387, 205);
            this.panelReg.TabIndex = 2;
            // 
            // txtRegValue
            // 
            this.txtRegValue.Location = new System.Drawing.Point(95, 136);
            this.txtRegValue.Multiline = true;
            this.txtRegValue.Name = "txtRegValue";
            this.txtRegValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRegValue.Size = new System.Drawing.Size(253, 86);
            this.txtRegValue.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 139);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Data:";
            // 
            // lstRegValueType
            // 
            this.lstRegValueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstRegValueType.FormattingEnabled = true;
            this.lstRegValueType.Location = new System.Drawing.Point(95, 109);
            this.lstRegValueType.Name = "lstRegValueType";
            this.lstRegValueType.Size = new System.Drawing.Size(253, 21);
            this.lstRegValueType.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Value type:";
            // 
            // txtRegValueName
            // 
            this.txtRegValueName.Location = new System.Drawing.Point(95, 83);
            this.txtRegValueName.Name = "txtRegValueName";
            this.txtRegValueName.Size = new System.Drawing.Size(253, 20);
            this.txtRegValueName.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Valuename:";
            // 
            // lstRegRoot
            // 
            this.lstRegRoot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstRegRoot.FormattingEnabled = true;
            this.lstRegRoot.Location = new System.Drawing.Point(95, 30);
            this.lstRegRoot.Name = "lstRegRoot";
            this.lstRegRoot.Size = new System.Drawing.Size(253, 21);
            this.lstRegRoot.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Root:";
            // 
            // lstRegAction
            // 
            this.lstRegAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstRegAction.FormattingEnabled = true;
            this.lstRegAction.Location = new System.Drawing.Point(95, 3);
            this.lstRegAction.Name = "lstRegAction";
            this.lstRegAction.Size = new System.Drawing.Size(253, 21);
            this.lstRegAction.TabIndex = 0;
            this.lstRegAction.SelectedIndexChanged += new System.EventHandler(this.lstRegAction_SelectedIndexChanged);
            // 
            // txtRegFolder
            // 
            this.txtRegFolder.Location = new System.Drawing.Point(95, 57);
            this.txtRegFolder.Name = "txtRegFolder";
            this.txtRegFolder.Size = new System.Drawing.Size(253, 20);
            this.txtRegFolder.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Folder:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Action:";
            // 
            // panelRunApp
            // 
            this.panelRunApp.Controls.Add(this.txtRunUser);
            this.panelRunApp.Controls.Add(this.label11);
            this.panelRunApp.Controls.Add(this.txtRunArgs);
            this.panelRunApp.Controls.Add(this.label3);
            this.panelRunApp.Controls.Add(this.txtRunExec);
            this.panelRunApp.Controls.Add(this.label2);
            this.panelRunApp.Location = new System.Drawing.Point(12, 110);
            this.panelRunApp.Name = "panelRunApp";
            this.panelRunApp.Size = new System.Drawing.Size(230, 82);
            this.panelRunApp.TabIndex = 1;
            // 
            // txtRunUser
            // 
            this.txtRunUser.Location = new System.Drawing.Point(95, 59);
            this.txtRunUser.Name = "txtRunUser";
            this.txtRunUser.Size = new System.Drawing.Size(253, 20);
            this.txtRunUser.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "User:";
            // 
            // txtRunArgs
            // 
            this.txtRunArgs.Location = new System.Drawing.Point(95, 29);
            this.txtRunArgs.Name = "txtRunArgs";
            this.txtRunArgs.Size = new System.Drawing.Size(253, 20);
            this.txtRunArgs.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Argurments:";
            // 
            // txtRunExec
            // 
            this.txtRunExec.Location = new System.Drawing.Point(95, 3);
            this.txtRunExec.Name = "txtRunExec";
            this.txtRunExec.Size = new System.Drawing.Size(253, 20);
            this.txtRunExec.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Executable:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblTZ);
            this.panel3.Controls.Add(this.chkDontExec);
            this.panel3.Controls.Add(this.DTExecBefore);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtName);
            this.panel3.Controls.Add(this.lstType);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(858, 104);
            this.panel3.TabIndex = 0;
            // 
            // lblTZ
            // 
            this.lblTZ.AutoSize = true;
            this.lblTZ.Location = new System.Drawing.Point(440, 85);
            this.lblTZ.Name = "lblTZ";
            this.lblTZ.Size = new System.Drawing.Size(16, 13);
            this.lblTZ.TabIndex = 4;
            this.lblTZ.Text = "---";
            // 
            // chkDontExec
            // 
            this.chkDontExec.AutoSize = true;
            this.chkDontExec.Location = new System.Drawing.Point(95, 56);
            this.chkDontExec.Name = "chkDontExec";
            this.chkDontExec.Size = new System.Drawing.Size(151, 17);
            this.chkDontExec.TabIndex = 2;
            this.chkDontExec.Text = "&Do not execute this before";
            this.chkDontExec.UseVisualStyleBackColor = true;
            this.chkDontExec.CheckedChanged += new System.EventHandler(this.chkDontExec_CheckedChanged);
            // 
            // DTExecBefore
            // 
            this.DTExecBefore.Location = new System.Drawing.Point(95, 79);
            this.DTExecBefore.Name = "DTExecBefore";
            this.DTExecBefore.Size = new System.Drawing.Size(339, 20);
            this.DTExecBefore.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(95, 30);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(339, 20);
            this.txtName.TabIndex = 1;
            // 
            // lstType
            // 
            this.lstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstType.FormattingEnabled = true;
            this.lstType.Location = new System.Drawing.Point(95, 3);
            this.lstType.Name = "lstType";
            this.lstType.Size = new System.Drawing.Size(339, 21);
            this.lstType.TabIndex = 0;
            this.lstType.SelectedIndexChanged += new System.EventHandler(this.lstType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 323);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(858, 29);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmdCancel);
            this.panel2.Controls.Add(this.cmdOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(694, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(164, 29);
            this.panel2.TabIndex = 0;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(5, 2);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(86, 2);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 6;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblIntoGroup
            // 
            this.lblIntoGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIntoGroup.Location = new System.Drawing.Point(0, 0);
            this.lblIntoGroup.Name = "lblIntoGroup";
            this.lblIntoGroup.Size = new System.Drawing.Size(858, 209);
            this.lblIntoGroup.TabIndex = 1;
            this.lblIntoGroup.Text = "---";
            // 
            // frmSimpleTasks
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(858, 565);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSimpleTasks";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SimpleTasks";
            this.Load += new System.EventHandler(this.frmSimpleTasks_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelReg.ResumeLayout(false);
            this.panelReg.PerformLayout();
            this.panelRunApp.ResumeLayout(false);
            this.panelRunApp.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ctlListPCs lstPCs;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox lstType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Panel panelRunApp;
        private System.Windows.Forms.TextBox txtRunArgs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRunExec;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Panel panelReg;
        private System.Windows.Forms.ComboBox lstRegRoot;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox lstRegAction;
        private System.Windows.Forms.TextBox txtRegFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRegValueName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRegValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox lstRegValueType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRunUser;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkDontExec;
        private System.Windows.Forms.DateTimePicker DTExecBefore;
        private System.Windows.Forms.Label lblTZ;
        private System.Windows.Forms.Label lblIntoGroup;
    }
}