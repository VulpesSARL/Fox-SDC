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
using Newtonsoft.Json;

namespace FoxSDC_MGMT.Policies
{
    public partial class ctlInstallPackages : UserControl, PolicyElementInterface
    {
        PolicyObject Pol;
        PackagePolicy PP;
        public ctlInstallPackages()
        {
            InitializeComponent();
        }

        void UpdateStatus()
        {
            chkOptional.Checked = PP.OptionalInstallation;
            chkUpdate.Checked = PP.InstallUpdates;
            chkOptional_CheckedChanged(null, null);
            lstPackages.Items.Clear();
            foreach (Int64 id in PP.Packages)
            {
                PackageData p = Program.net.GetPackages(id);
                if (p == null)
                {
                    ListViewItem l = new ListViewItem(id.ToString());
                    l.Tag = id;
                    l.SubItems.Add("Invalid/non existent ID");
                    lstPackages.Items.Add(l);
                }
                else
                {
                    ListViewItem l = new ListViewItem(id.ToString());
                    l.Tag = id;
                    l.SubItems.Add(p.Title);
                    l.SubItems.Add(p.Version.ToString());
                    l.SubItems.Add(p.PackageID);
                    lstPackages.Items.Add(l);
                }
            }
        }

        private void ctlInstallPackages_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;
            UpdateStatus();
        }

        public bool SetData(FoxSDC_Common.PolicyObject obj)
        {
            Pol = obj;

            PP = JsonConvert.DeserializeObject<PackagePolicy>(obj.Data);
            if (PP == null)
                PP = new PackagePolicy();

            if (PP.Packages == null)
                PP.Packages = new List<long>();
            UpdateStatus();
            return (true);
        }

        public string GetData()
        {
            PP.InstallUpdates = chkUpdate.Checked;
            PP.OptionalInstallation = chkOptional.Checked;
            return (JsonConvert.SerializeObject(PP));
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string data = GetData();
            Program.net.EditPolicy(Pol.ID, data);
        }

        private void chkOptional_CheckedChanged(object sender, EventArgs e)
        {
            chkUpdate.Enabled = chkOptional.Checked;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmListPackages frm = new frmListPackages(true);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            foreach (Int64 i in frm.IDs)
            {
                if (PP.Packages.Contains(i) == false)
                    PP.Packages.Add(i);
            }
            UpdateStatus();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (lstPackages.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem l in lstPackages.SelectedItems)
            {
                Int64 id = (Int64)l.Tag;
                PP.Packages.Remove(id);
            }
            UpdateStatus();
        }
    }
}
