namespace FoxSDC_MGMT
{
    partial class frmUserManagement
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
            this.label1 = new System.Windows.Forms.Label();
            this.lstUsers = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdChangeUser = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLDAP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEMail = new System.Windows.Forms.TextBox();
            this.chkLDAP = new System.Windows.Forms.CheckBox();
            this.chkMustChangePassword = new System.Windows.Forms.CheckBox();
            this.lstPermissions = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdNew = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User:";
            // 
            // lstUsers
            // 
            this.lstUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(112, 12);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(347, 21);
            this.lstUsers.TabIndex = 0;
            this.lstUsers.SelectedIndexChanged += new System.EventHandler(this.lstUsers_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdChangeUser);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtLDAP);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtEMail);
            this.panel1.Controls.Add(this.chkLDAP);
            this.panel1.Controls.Add(this.chkMustChangePassword);
            this.panel1.Controls.Add(this.lstPermissions);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Location = new System.Drawing.Point(0, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 344);
            this.panel1.TabIndex = 1;
            // 
            // cmdChangeUser
            // 
            this.cmdChangeUser.Location = new System.Drawing.Point(363, 308);
            this.cmdChangeUser.Name = "cmdChangeUser";
            this.cmdChangeUser.Size = new System.Drawing.Size(96, 23);
            this.cmdChangeUser.TabIndex = 7;
            this.cmdChangeUser.Text = "Save";
            this.cmdChangeUser.UseVisualStyleBackColor = true;
            this.cmdChangeUser.Click += new System.EventHandler(this.cmdChangeUser_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 285);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "LDAP Username:";
            // 
            // txtLDAP
            // 
            this.txtLDAP.Location = new System.Drawing.Point(112, 282);
            this.txtLDAP.Name = "txtLDAP";
            this.txtLDAP.Size = new System.Drawing.Size(347, 20);
            this.txtLDAP.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 236);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "E-Mail:";
            // 
            // txtEMail
            // 
            this.txtEMail.Location = new System.Drawing.Point(112, 233);
            this.txtEMail.Name = "txtEMail";
            this.txtEMail.Size = new System.Drawing.Size(347, 20);
            this.txtEMail.TabIndex = 4;
            // 
            // chkLDAP
            // 
            this.chkLDAP.AutoSize = true;
            this.chkLDAP.Location = new System.Drawing.Point(112, 259);
            this.chkLDAP.Name = "chkLDAP";
            this.chkLDAP.Size = new System.Drawing.Size(178, 17);
            this.chkLDAP.TabIndex = 5;
            this.chkLDAP.Text = "Use LDAP account for this user:";
            this.chkLDAP.UseVisualStyleBackColor = true;
            this.chkLDAP.CheckedChanged += new System.EventHandler(this.chkLDAP_CheckedChanged);
            // 
            // chkMustChangePassword
            // 
            this.chkMustChangePassword.AutoSize = true;
            this.chkMustChangePassword.Location = new System.Drawing.Point(112, 55);
            this.chkMustChangePassword.Name = "chkMustChangePassword";
            this.chkMustChangePassword.Size = new System.Drawing.Size(221, 17);
            this.chkMustChangePassword.TabIndex = 2;
            this.chkMustChangePassword.Text = "Password must be changed at next logon";
            this.chkMustChangePassword.UseVisualStyleBackColor = true;
            // 
            // lstPermissions
            // 
            this.lstPermissions.FormattingEnabled = true;
            this.lstPermissions.IntegralHeight = false;
            this.lstPermissions.Location = new System.Drawing.Point(112, 78);
            this.lstPermissions.Name = "lstPermissions";
            this.lstPermissions.ScrollAlwaysVisible = true;
            this.lstPermissions.Size = new System.Drawing.Size(347, 149);
            this.lstPermissions.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Permissions:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(112, 29);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(347, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(112, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(347, 20);
            this.txtName.TabIndex = 0;
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(363, 389);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(96, 23);
            this.cmdClose.TabIndex = 4;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdNew
            // 
            this.cmdNew.Location = new System.Drawing.Point(112, 389);
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(96, 23);
            this.cmdNew.TabIndex = 2;
            this.cmdNew.Text = "New";
            this.cmdNew.UseVisualStyleBackColor = true;
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Location = new System.Drawing.Point(214, 389);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(96, 23);
            this.cmdDelete.TabIndex = 3;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // frmUserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(477, 425);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdNew);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserManagement";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "User management";
            this.Load += new System.EventHandler(this.UserManagement_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstUsers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdChangeUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLDAP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEMail;
        private System.Windows.Forms.CheckBox chkLDAP;
        private System.Windows.Forms.CheckBox chkMustChangePassword;
        private System.Windows.Forms.CheckedListBox lstPermissions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdNew;
        private System.Windows.Forms.Button cmdDelete;
    }
}