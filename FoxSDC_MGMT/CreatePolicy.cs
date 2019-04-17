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
    public partial class frmCreatePolicy : FForm
    {
        string MachineID;
        Int64? GroupID;

        public Int64 NEWID;

        public frmCreatePolicy(string machineid, Int64? groupid)
        {
            MachineID = machineid;
            GroupID = groupid;
            InitializeComponent();
        }

        private void frmCreatePolicy_Load(object sender, EventArgs e)
        {
            if (MachineID != null && GroupID != null)
            {
                lblCreatePolicy.Text = "Create policy in <THIS SHOULDN'T HAPPEN>";
                cmdOK.Enabled = false;
            }
            if (MachineID != null && GroupID == null)
            {
                ComputerData G = Program.net.GetComputerDetail(MachineID);
                lblCreatePolicy.Text = "Create policy for computer " + (G == null ? "<ID ERROR>" : G.Computername);
                if (G == null)
                    cmdOK.Enabled = false;
            }
            if (GroupID != null && MachineID == null)
            {
                string G = Program.net.GetGroupName(GroupID.Value);
                lblCreatePolicy.Text = "Create policy in group " + (G == null ? "<ID ERROR>" : G);
                if (G == null)
                    cmdOK.Enabled = false;
            }
            if (MachineID == null && GroupID == null)
            {
                lblCreatePolicy.Text = "Create policy in (Root Group)";
            }
            foreach (PolicyListElement ele in PolicyList.Elements)
            {
                lstPolicies.Items.Add(ele);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show(this, "Enter a name.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstPolicies.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Select the policy item you want to create.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            PolicyListElement ele = (PolicyListElement)lstPolicies.SelectedItem;
            NewPolicyReq req = new NewPolicyReq();
            req.Data = "";
            req.Grouping = GroupID;
            req.MachineID = MachineID;
            req.Name = txtName.Text.Trim();
            req.Type = ele.TypeID;
            Int64? newID = Program.net.CreatePolicy(req);
            if (newID == null)
            {
                MessageBox.Show(this, "Creating new policy failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            NEWID = newID.Value;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void lstPolicies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPolicies.SelectedIndex == -1)
                return;
            PolicyListElement ele = (PolicyListElement)lstPolicies.SelectedItem;
            txtDesc.Text = ele.Description;
        }
    }
}
