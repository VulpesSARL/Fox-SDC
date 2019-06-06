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
    public partial class ctlAddRemovePrograms : UserControl
    {
        string MID;
        public ctlAddRemovePrograms(string MachineID)
        {
            MID = MachineID;
            InitializeComponent();
        }

        private void ctlAddRemovePrograms_Load(object sender, EventArgs e)
        {
            Program.LoadImageList(imageList1);

            lstApps.Items.Clear();

            List<AddRemoveAppReport> lstApp = Program.net.GetAddRemovePrograms(MID);
            if (lstApp == null)
                return;

            foreach (AddRemoveAppReport l in lstApp)
            {
                ListViewItem lst = new ListViewItem(l.Name);
                lst.Tag = l;
                lst.ImageIndex = l.IsSystemComponent == true ? 4 : 3;
                lst.SubItems.Add(l.Computername);
                lst.SubItems.Add(string.IsNullOrWhiteSpace(l.Username) == true ? l.HKCUUser : l.Username);
                lst.SubItems.Add(l.DisplayVersion);
                lst.SubItems.Add(l.DisplayLanguage);
                lst.SubItems.Add(l.DT.ToLongDateString() + " " + l.DT.ToLongTimeString());
                lstApps.Items.Add(lst);
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstApps.SelectedItems.Count == 0)
                return;
            frmPropertiesWindow frm = new frmPropertiesWindow((AddRemoveAppReport)lstApps.SelectedItems[0].Tag);
            frm.ShowDialog(this);
        }

        private void lstApps_DoubleClick(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(sender, e);
        }

        private void lstApps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                propertiesToolStripMenuItem_Click(sender, e);
        }
    }
}
