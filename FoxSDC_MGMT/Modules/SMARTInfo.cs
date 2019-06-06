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

namespace FoxSDC_MGMT
{
    public partial class ctlSMARTInfo : UserControl
    {
        string MID;
        public ctlSMARTInfo(string MachineID)
        {
            MID = MachineID;
            InitializeComponent();
        }

        void LoadData()
        {
            lstSmartAttr.Items.Clear();
            lstDisks.Items.Clear();

            List<VulpesSMARTInfo> lst = Program.net.GetSMARTInfo(MID);
            if (lst == null)
                return;
            foreach (VulpesSMARTInfo ll in lst)
            {
                ListViewItem l = new ListViewItem(ll.Model);
                l.Tag = ll;
                l.SubItems.Add(ll.SerialNumber);
                l.SubItems.Add(ll.FirmwareRevision);
                l.SubItems.Add(ll.InterfaceType);
                l.SubItems.Add(CommonUtilities.NiceSize(ll.Size));
                l.SubItems.Add(ll.Status);
                l.SubItems.Add(ll.PredictFailure == true ? "FAILURE" : "");
                if (ll.Attributes == null)
                    l.SubItems.Add("no");
                else
                {
                    if (ll.Attributes.Count == 0)
                        l.SubItems.Add("no");
                    else
                        l.SubItems.Add("yes");
                }

                if (SMARTDescription.IsInError(ll) == true)
                {
                    l.ForeColor = Color.Red;
                    l.UseItemStyleForSubItems = true;
                }

                lstDisks.Items.Add(l);
            }
        }

        private void ctlSMARTInfo_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void lstDisks_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstSmartAttr.Items.Clear();
            if (lstDisks.SelectedItems.Count == 0)
                return;
            VulpesSMARTInfo i = (VulpesSMARTInfo)lstDisks.SelectedItems[0].Tag;
            if (i.Attributes == null)
                return;
            foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in i.Attributes)
            {
                ListViewItem l = new ListViewItem("0x" + kvp.Key.ToString("X"));
                l.Tag = kvp.Value;

                string Name = "???";
                string AttribIdeal = "";
                if (SMARTDescription.Descriptions.ContainsKey(kvp.Key) == true)
                {
                    Name = SMARTDescription.Descriptions[kvp.Key].Description;
                    switch (SMARTDescription.Descriptions[kvp.Key].Ideal)
                    {
                        case SMARTDescriptionEnum.Critical: AttribIdeal = "‼"; break;
                        case SMARTDescriptionEnum.LowIdeal: AttribIdeal = "↓"; break;
                        case SMARTDescriptionEnum.HighIdeal: AttribIdeal = "↑"; break;
                    }
                }

                l.SubItems.Add(Name);
                l.SubItems.Add(AttribIdeal);
                l.SubItems.Add(kvp.Value.FailureImminent == true ? "!true!" : "false");
                l.SubItems.Add(kvp.Value.Value.ToString());
                l.SubItems.Add(kvp.Value.Threshold.ToString());
                l.SubItems.Add(kvp.Value.Worst.ToString());
                l.SubItems.Add(kvp.Value.Vendordata.ToString());

                if (SMARTDescription.IsAttribInError(kvp.Value) == true)
                {
                    l.ForeColor = Color.Red;
                    l.UseItemStyleForSubItems = true;
                }

                lstSmartAttr.Items.Add(l);
            }
        }
    }
}
