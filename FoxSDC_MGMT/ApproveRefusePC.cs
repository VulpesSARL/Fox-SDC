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
    public partial class frmApproveRefusePC : FForm
    {
        List<ApproveListElement> ListPCs;
        Int64? SelectedGroup;
        public frmApproveRefusePC(List<ApproveListElement> lst)
        {
            ListPCs = lst;
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (SelectedGroup == null)
                return;

            foreach (ApproveListElement l in ListPCs)
            {
                if (Program.net.ApproveComputers(l.MachineID, lstState.SelectedIndex == 0 ? true : false, SelectedGroup.Value) == false)
                {
                    MessageBox.Show(this, "Approving computer " + l.Computername + " failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void frmApproveRefusePC_Load(object sender, EventArgs e)
        {
            lblPCs.Text = ListPCs.Count.ToString() + " computer" + (ListPCs.Count == 1 ? "" : "s") + " to change";
            foreach (ApproveListElement pc in ListPCs)
            {
                lstPCs.Items.Add(pc.Computername);
            }
            lstState.Items.Add("Approve");
            lstState.Items.Add("Not approved");
            lstState.SelectedIndex = 0;
            SelectedGroup = null;
        }

        private void cmdSelectGroup_Click(object sender, EventArgs e)
        {
            frmSelectGroup grp = new frmSelectGroup();
            if (grp.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            SelectedGroup = grp.SelectedElement;
            txtGroup.Text = grp.SelectedGroupName;
        }
    }
}
