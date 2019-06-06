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
    public partial class ctlSimpleTasks : UserControl
    {
        string MID;
        public ctlSimpleTasks(string MachineID)
        {
            MID = MachineID;
            InitializeComponent();
        }

        void LoadData()
        {
            lstST.Items.Clear();
            List<SimpleTaskLite> data = Program.net.GetSimpleTasks(MID);
            if (data == null)
                return;
            foreach (SimpleTaskLite l in data)
            {
                ListViewItem lst = new ListViewItem(l.Name);
                lst.Tag = l;
                lst.ImageIndex = 23;
                lst.SubItems.Add(l.ComputerName);
                lst.SubItems.Add(l.ID.ToString());
                switch (l.Type)
                {
                    case 1:
                        lst.SubItems.Add("Execute program");
                        break;
                    case 2:
                        lst.SubItems.Add("Modify registry");
                        break;
                    default:
                        lst.SubItems.Add("??? 0x" + l.Type.ToString("X"));
                        break;
                }
                lstST.Items.Add(lst);
            }
        }

        private void ctlSimpleTasks_Load(object sender, EventArgs e)
        {
            Program.LoadImageList(imageList1);
            LoadData();
        }

        private void cmdNew_Click(object sender, EventArgs e)
        {
            frmSimpleTasks st = new frmSimpleTasks();
            if (st.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (lstST.SelectedItems.Count == 0)
                return;
            SimpleTaskLite ST = (SimpleTaskLite)lstST.SelectedItems[0].Tag;
            SimpleTask stb = Program.net.GetSimpleTaskDetails(ST.ID);
            if (stb == null)
            {
                MessageBox.Show(this, "Cannot get Simple Tasks details: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            frmSimpleTasks stfrm = new frmSimpleTasks(stb);
            if (stfrm.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (lstST.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show(this, "Do you really want to delete the " + lstST.SelectedItems.Count.ToString() +
                " Simple Task" + (lstST.SelectedItems.Count == 1 ? "" : "s") + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ListViewItem l in lstST.SelectedItems)
                {
                    SimpleTaskLite ST = (SimpleTaskLite)l.Tag;
                    Program.net.DeleteSimpleTask(ST.ID);
                }
                LoadData();
            }

        }
    }
}
