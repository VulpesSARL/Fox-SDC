using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmLogin : FForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtServer.Text.Trim() == "")
            {
                MessageBox.Show(this, "Enter a server.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show(this, "Enter an username.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtPassword.Text == "")
            {
                MessageBox.Show(this, "Enter a password.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Program.net = new Network();
            if (Program.net.Connect(txtServer.Text) == false)
            {
                MessageBox.Show(this, "Connection to server " + txtServer.Text + " did not work.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Program.net.Login(txtUsername.Text, txtPassword.Text) == false)
            {
                if (Program.net.LoginError == null)
                    MessageBox.Show(this, "Login failed: " + "???", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(this, "Login failed: " + Program.net.LoginError.Error, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                Program.net.CloseConnection();
                return;
            }

            if (Program.net.NeedChangePassword() == true)
            {
                if (MessageBox.Show(this, "You need to change your password.", Program.Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.OK)
                {
                    Program.net.CloseConnection();
                    return;
                }
                else
                {
                    frmChangePassword frm = new frmChangePassword(txtPassword.Text);
                    if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    {
                        Program.net.CloseConnection();
                        return;
                    }
                }
            }

            if (Program.PreFillServer == "")
                Settings.Default.LastServer = txtServer.Text;

            Settings.Default.LastUsername = txtUsername.Text;
            Settings.Default.Save();

            Program.net.GetInfo(); //Get more data of the logged in user

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtServer.Text = Settings.Default.LastServer;
            if (Program.PreFillServer != "")
            {
                txtServer.Text = Program.PreFillServer;
                txtServer.Enabled = false;
            }
            txtUsername.Text = Settings.Default.LastUsername;
            if (txtServer.Text != "" && txtUsername.Text != "")
                this.ActiveControl = txtPassword;
        }
    }
}
