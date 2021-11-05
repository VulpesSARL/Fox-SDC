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

        private void removeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstStart.SelectedItems.Count == 0)
                return;

            if (MessageBox.Show(this, "Do you want to remove the selected " + lstStart.SelectedItems.Count.ToString() + " entr" + (lstStart.SelectedItems.Count == 1 ? "y" : "ies") + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;

            foreach (ListViewItem lst in lstStart.SelectedItems)
            {
                StartupItemFull start = (StartupItemFull)lst.Tag;
                SimpleTaskRegistry reg = new SimpleTaskRegistry();

                if (string.IsNullOrWhiteSpace(start.HKCUUser) == false)
                {
                    reg.Root = 1; //HKCU
                    reg.Folder = start.HKCUUser + "\\";
                }
                else
                {
                    reg.Root = 0; //HKLM                   
                    reg.Folder = "";
                }

                switch (start.Location.ToLower())
                {
                    case "reg":
                        reg.Folder += "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
                        break;
                    case "reg_wow":
                        reg.Folder += "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";
                        break;
                    default:
                        continue;
                }

                reg.Action = 1;
                reg.Valuename = start.Key == "(Default)" ? "" : start.Key;

                Program.net.SetSimpleTask("Removing Startup Item: " + start.Key + (string.IsNullOrWhiteSpace(start.Username) == true ? "" : " (as " + start.Username + ")"), start.MachineID, null, 2, reg);
            }
        }
    }
}
