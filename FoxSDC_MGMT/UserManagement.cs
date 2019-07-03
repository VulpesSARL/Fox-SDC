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
    public partial class frmUserManagement : FForm
    {
        List<UserDetails> Users;

        class PermKVP
        {
            public Int64 Permission;
            public string Name;
            public PermKVP(string Name, Int64 Permission)
            {
                this.Permission = Permission;
                this.Name = Name;

            }
            public override string ToString()
            {
                return (Name + " (0x" + Permission.ToString("X") + ")");
            }
        }

        void LoadList()
        {
            Users = Program.net.GetAllUsers();
            if (Users == null)
            {
                panel1.Enabled = false;
                lstUsers.Enabled = false;
                MessageBox.Show(this, "Loading users failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            lstUsers.Items.Clear();
            foreach (UserDetails u in Users)
            {
                lstUsers.Items.Add(u.Username);
            }

            if (lstUsers.Items.Count > 0)
                lstUsers.SelectedIndex = 0;
        }
        public frmUserManagement()
        {
            InitializeComponent();
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            lstPermissions.Items.Clear();
            foreach (ACLFlags Flag in (ACLFlags[])Enum.GetValues(typeof(ACLFlags)))
            {
                lstPermissions.Items.Add(new PermKVP(Enum.GetName(typeof(ACLFlags), Flag), (Int64)Flag));
            }

            chkLDAP_CheckedChanged(null, null);

            LoadList();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdChangeUser_Click(object sender, EventArgs e)
        {
            UserDetailsPassword ud = new UserDetailsPassword();
            ud.EMail = txtEMail.Text.Trim();
            ud.LDAPUsername = txtLDAP.Text.Trim();
            ud.MustChangePassword = chkMustChangePassword.Checked;
            ud.Name = txtName.Text.Trim();
            ud.NewPassword = txtPassword.Text;
            ud.Permissions = GetPermission();
            ud.UseLDAP = chkLDAP.Checked;
            ud.Username = lstUsers.Text;
            if (Program.net.ChangeUser(ud) == false)
            {
                MessageBox.Show(this, "Saving user failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void cmdNew_Click(object sender, EventArgs e)
        {
            frmAskText frm = new frmAskText("New User", "Specify an username for the new user", "");
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            if (Program.net.AddUser(frm.RetText) == false)
            {
                MessageBox.Show(this, "Create new user failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            LoadList();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to delete the user " + lstUsers.Text + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            if (Program.net.DeleteUser(lstUsers.Text) == false)
            {
                MessageBox.Show(this, "Deleting user failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            LoadList();
        }

        Int64 GetPermission()
        {
            Int64 Perm = 0;
            for (int i = 0; i < lstPermissions.Items.Count; i++)
            {
                PermKVP p = (PermKVP)lstPermissions.Items[i];
                if (lstPermissions.GetItemChecked(i) == true)
                    Perm |= p.Permission;
            }
            return (Perm);
        }

        void LoadPermission(Int64 Permission)
        {
            for (int i = 0; i < lstPermissions.Items.Count; i++)
            {
                PermKVP p = (PermKVP)lstPermissions.Items[i];
                if ((Permission & p.Permission) == 0)
                    lstPermissions.SetItemChecked(i, false);
                else
                    lstPermissions.SetItemChecked(i, true);
            }
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            if (lstUsers.SelectedIndex == -1)
                panel1.Enabled = false;
            else
            {
                Users = Program.net.GetAllUsers();
                if (Users == null)
                    return;
                foreach (UserDetails u in Users)
                {
                    if (u.Username == lstUsers.Text)
                    {
                        txtEMail.Text = u.EMail;
                        txtLDAP.Text = u.LDAPUsername;
                        txtName.Text = u.Name;
                        txtPassword.Text = "";
                        LoadPermission(u.Permissions);
                        chkLDAP.Checked = u.UseLDAP;
                        chkMustChangePassword.Checked = u.MustChangePassword;
                        panel1.Enabled = true;
                    }
                }
            }
        }

        private void chkLDAP_CheckedChanged(object sender, EventArgs e)
        {
            txtLDAP.Enabled = chkLDAP.Checked;
            txtPassword.Enabled = !chkLDAP.Checked;
            chkMustChangePassword.Enabled = !chkLDAP.Checked;
        }
    }
}
