using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Common;

namespace FoxSDC_MGMT.Policies
{
    public partial class ctlLinkPolicy : UserControl, PolicyElementInterface
    {
        Int64? SelectedPolicy = null;
        PolicyObject Pol;
        public ctlLinkPolicy()
        {
            InitializeComponent();
        }

        void UpdateStatus()
        {
            if (SelectedPolicy != null)
            {
                PolicyObject p = Program.net.GetPolicyObject(SelectedPolicy.Value);
                if (p == null)
                    lblLinkPolicy.Text = "Invalid policy";
                else
                    lblLinkPolicy.Text = p.Name;
            }
            else
            {
                lblLinkPolicy.Text = "No policy";
            }
        }

        private void LinkPolicy_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;
            UpdateStatus();
        }

        public bool SetData(FoxSDC_Common.PolicyObject obj)
        {
            Pol = obj;
            Int64 selpol;

            if (Int64.TryParse(Pol.Data, out selpol) == false)
            {
                SelectedPolicy = null;
            }
            else
            {
                SelectedPolicy = selpol;
            }
            UpdateStatus();

            return (true);
        }

        public string GetData()
        {
            return (SelectedPolicy == null ? "" : SelectedPolicy.Value.ToString());
        }

        private void cmdSelectPol_Click(object sender, EventArgs e)
        {
            frmSelectPolicy pol = new frmSelectPolicy();
            if (pol.ShowDialog(this) != DialogResult.OK)
                return;
            SelectedPolicy = pol.SelectedPolicyElement;
            UpdateStatus();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string data = GetData();
            Program.net.EditPolicy(Pol.ID, data);
        }
    }
}
