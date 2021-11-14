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
    public partial class ctlPendingChats : UserControl
    {
        public ctlPendingChats()
        {
            InitializeComponent();
        }

        void UpdateList()
        {
#if ENABLECHAT
            lstComputers.Items.Clear();
            List<string> Machines = Program.net.GetPendingChats();
            if (Machines == null)
                return;
            foreach (string Machine in Machines)
            {
                ComputerData cd = Program.net.GetComputerDetail(Machine);
                ListViewItem l = new ListViewItem(cd.Computername);
                l.Tag = Machine;
                l.ImageIndex = 0;
                l.SubItems.Add(cd.GroupingPath);
                lstComputers.Items.Add(l);
            }
#endif
        }

        private void timUpdate_Tick(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void ctlPendingChats_Load(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void lstComputers_DoubleClick(object sender, EventArgs e)
        {
#if ENABLECHAT
            if (lstComputers.SelectedItems.Count == 0)
                return;
            frmComputerInfo pc = new frmComputerInfo((string)lstComputers.SelectedItems[0].Tag);
            pc.Show();
#endif
        }

        private void lstComputers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                lstComputers_DoubleClick(sender, e);
        }
    }
}
