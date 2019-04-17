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
    public partial class ctlListDiskData : UserControl
    {
        string MachineID;
        public ctlListDiskData(string machineid)
        {
            MachineID = machineid;
            InitializeComponent();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstDiskData.SelectedItems.Count == 0)
                return;
            frmPropertiesWindow frm = new frmPropertiesWindow((DiskDataReport)lstDiskData.SelectedItems[0].Tag);
            frm.ShowDialog(this);
        }

        private void ListDiskData_Load(object sender, EventArgs e)
        {
            lstDiskData.Items.Clear();
            List<DiskDataReport> lstdiskData = Program.net.GetDiskDataList(MachineID);
            
            if (lstdiskData != null)
            {
                foreach (DiskDataReport l in lstdiskData)
                {
                    ListViewItem lst = new ListViewItem(l.Computername);
                    lst.Tag = l;
                    lst.SubItems.Add(l.DevicePresent == true ? "yes" : "no");
                    lst.SubItems.Add(l.DriveLetter);
                    lst.SubItems.Add(l.DriveType.ToString());
                    lst.SubItems.Add(l.FileSystem);
                    lst.SubItems.Add(l.Label);
                    lst.SubItems.Add(CommonUtilities.NiceSize(l.Capacity));
                    lst.SubItems.Add(CommonUtilities.NiceSize(l.FreeSpace));
                    lst.SubItems.Add(l.Status);
                    lst.SubItems.Add(l.LastUpdated.ToLongDateString() + " " + l.LastUpdated.ToLongTimeString());
                    lstDiskData.Items.Add(lst);
                }
            }
        }
    }
}
