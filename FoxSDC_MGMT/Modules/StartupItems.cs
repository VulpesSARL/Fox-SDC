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
    public partial class ctlStartupItems : UserControl
    {
        string MID;
        public ctlStartupItems(string MachineID)
        {
            MID = MachineID;
            InitializeComponent();
        }

        private void ctlStartupItems_Load(object sender, EventArgs e)
        {
            Program.LoadImageList(imageList1);

            lstStart.Items.Clear();

            List<StartupItemFull> lstApp = Program.net.GetStartupItems(MID);
            if (lstApp == null)
                return;

            foreach (StartupItemFull l in lstApp)
            {
                ListViewItem lst = new ListViewItem(l.Key);
                lst.Tag = l;
                lst.ImageIndex = 22;
                lst.SubItems.Add(l.Computername);
                lst.SubItems.Add(string.IsNullOrWhiteSpace(l.Username) == true ? l.HKCUUser : l.Username);
                lst.SubItems.Add(l.Location);
                lst.SubItems.Add(l.Item);
                lst.SubItems.Add(l.DT.ToLongDateString() + " " + l.DT.ToLongTimeString());
                lstStart.Items.Add(lst);
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstStart.SelectedItems.Count == 0)
                return;
            frmPropertiesWindow frm = new frmPropertiesWindow((StartupItemFull)lstStart.SelectedItems[0].Tag);
            frm.ShowDialog(this);
        }

        private void lstStart_DoubleClick(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(sender, e);
        }

        private void lstStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                propertiesToolStripMenuItem_Click(sender, e);
        }
    }
}
