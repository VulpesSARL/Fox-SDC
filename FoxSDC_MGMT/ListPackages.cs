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
    public partial class frmListPackages : FForm
    {
        bool MultiSelect;
        public List<Int64> IDs;
        public frmListPackages(bool MultiSelect)
        {
            InitializeComponent();
            this.MultiSelect = MultiSelect;
        }

        private void frmListPackages_Load(object sender, EventArgs e)
        {
            List<PackageData> pl = Program.net.GetPackages();
            if (pl == null)
                return;
            foreach (PackageData p in pl)
            {
                ListViewItem l = new ListViewItem(p.ID.ToString());
                l.Tag = p.ID;
                l.SubItems.Add(p.Title);
                l.SubItems.Add(p.Version.ToString());
                l.SubItems.Add(p.PackageID);
                lstPackages.Items.Add(l);
            }
            lstPackages.MultiSelect = MultiSelect;
            IDs = new List<long>();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (lstPackages.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem l in lstPackages.SelectedItems)
            {
                IDs.Add((Int64)l.Tag);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
