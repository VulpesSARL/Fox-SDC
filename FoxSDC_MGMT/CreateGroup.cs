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
    public partial class frmCreateGroup : FForm
    {
        Int64? GroupID;
        string GName;
        public Int64 NewID;
        public string NewName;
        bool RenameMode;

        public frmCreateGroup(Int64? GID, string name, bool Renamemode)
        {
            GroupID = GID;
            GName = name;
            RenameMode = Renamemode;
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtText.Text.Trim() == "")
            {
                MessageBox.Show(this, "Enter a name", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (RenameMode == false)
            {
                Int64? nid = Program.net.CreateGroup(txtText.Text.Trim(), GroupID);
                if (nid == null)
                {
                    MessageBox.Show(this, "Create group failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                NewID = nid.Value;
            }
            else
            {
                NewName = "";
                if (GroupID != null)
                {
                    if (Program.net.RenameGroup(GroupID.Value, txtText.Text.Trim()) == false)
                    {
                        MessageBox.Show(this, "Rename group failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    NewName = txtText.Text.Trim();
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void frmCreateGroup_Load(object sender, EventArgs e)
        {
            if (RenameMode == false)
            {
                lblCreateTo.Text = "Create new group into " + GName;
            }
            else
            {
                this.Text = "Rename group";
                txtText.Text = GName;
                lblCreateTo.Text = "";
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
