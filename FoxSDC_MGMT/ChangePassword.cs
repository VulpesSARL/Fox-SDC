using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmChangePassword : FForm
    {
        string OldPW = "";
        public frmChangePassword()
        {
            InitializeComponent();
        }

        public frmChangePassword(string oldpw)
        {
            InitializeComponent();
            OldPW = oldpw;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text == "")
            {
                MessageBox.Show(this, "Enter your old password", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtNewPassword.Text == "" || txtReNewPassword.Text == "")
            {
                MessageBox.Show(this, "Enter a new password", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtNewPassword.Text != txtReNewPassword.Text)
            {
                MessageBox.Show(this, "The new passwords do not match.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Program.net.ChangeMyPassword(txtOldPassword.Text, txtNewPassword.Text) == false)
            {
                MessageBox.Show(this, "Changing password failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            txtOldPassword.Text = OldPW;
            if (txtOldPassword.Text != "")
                txtNewPassword.Focus();
        }
    }
}
