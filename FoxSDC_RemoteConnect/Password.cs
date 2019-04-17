using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_RemoteConnect
{
    public partial class frmPassword : FForm
    {
        string Server;
        string Username;
        public string Password = "";
        public frmPassword(string Server, string Username)
        {
            this.Server = Server;
            this.Username = Username;
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Password = txtPassword.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmPassword_Load(object sender, EventArgs e)
        {
            lblPassword.Text = "Enter the password for " + Username + " to connect to the server " + Server;
        }
    }
}
