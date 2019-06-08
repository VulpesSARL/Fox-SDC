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
                lst.SubItems.Add(l.IsMSI == true ? "yes" : "no");
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

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstApps.SelectedItems.Count == 0)
                return;
            List<string> MIDs = new List<string>();

            if (((AddRemoveAppReport)lstApps.SelectedItems[0].Tag).IsMSI == true)
            {
                foreach (ListViewItem lst in lstApps.SelectedItems)
                {
                    AddRemoveAppReport lt = (AddRemoveAppReport)lst.Tag;
                    if (MIDs.Contains(lt.MachineID.ToLower()) == false)
                        MIDs.Add(lt.MachineID.ToLower());
                    if (lt.IsMSI == false)
                    {
                        MessageBox.Show(this, "You cannot mix MSI installations with non-MSI installations to uninstall in the selection.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                if (MessageBox.Show(this, "Do you really want to uninstall the selected " + lstApps.SelectedItems.Count.ToString() + " application" + (lstApps.SelectedItems.Count == 1 ? "" : "s") + " from " + MIDs.Count.ToString() + " computer" + (MIDs.Count == 1 ? "" : "s") + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                foreach (ListViewItem lst in lstApps.SelectedItems)
                {
                    AddRemoveAppReport lt = (AddRemoveAppReport)lst.Tag;

                    if (string.IsNullOrWhiteSpace(lt.ProductID) == true)
                        continue;

                    SimpleTaskRunProgramm run = new SimpleTaskRunProgramm();
                    if (lt.IsWOWBranch == false)
                        run.Executable = "%SYSTEMROOT%\\System32\\MSIExec.exe";
                    else
                        run.Executable = "%SYSTEMROOT%\\SysWOW64\\MSIExec.exe";

                    run.Parameters = "/x " + lt.ProductID + " /passive /quiet /norestart";
                    run.User = lt.Username;

                    Program.net.SetSimpleTask("Uninstall: " + lt.Name + (string.IsNullOrWhiteSpace(lt.Username) == true ? "" : " (as " + lt.Username + ")"), lt.MachineID, 1, run);
                }
            }
            else
            {
                if (lstApps.SelectedItems.Count > 1)
                {
                    MessageBox.Show(this, "Only one non MSI can be selected to uninstall.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                AddRemoveAppReport lt = (AddRemoveAppReport)lstApps.SelectedItems[0].Tag;
                SimpleTaskRunProgramm run = new SimpleTaskRunProgramm();
                if (lt.UninstallString.StartsWith("\"") == true)
                {
                    if (lt.UninstallString.IndexOf('"', 1) == -1)
                    {
                        run.Executable = lt.UninstallString.Substring(1);
                    }
                    else
                    {
                        run.Executable = lt.UninstallString.Substring(1, lt.UninstallString.IndexOf('"', 1) - 1);
                        run.Parameters = lt.UninstallString.Substring(lt.UninstallString.IndexOf('"', 1) + 1);
                    }
                    run.User = lt.Username;
                }
                else
                {
                    if (lt.UninstallString.IndexOf(' ', 0) == -1)
                    {
                        run.Executable = lt.UninstallString;
                    }
                    else
                    {
                        run.Executable = lt.UninstallString.Substring(0, lt.UninstallString.IndexOf(' '));
                        run.Parameters = lt.UninstallString.Substring(lt.UninstallString.IndexOf(' ') + 1);
                    }

                    run.User = lt.Username;
                }

                SimpleTask st = new SimpleTask();
                st.Data = JsonConvert.SerializeObject(run);
                st.MachineID = lt.MachineID;
                st.Type = 1;
                st.ID = -1;
                st.Name = "Uninstall: " + lt.Name + (string.IsNullOrWhiteSpace(lt.Username) == true ? "" : " (as " + lt.Username + ")");
                frmSimpleTasks frm = new frmSimpleTasks(st);
                frm.ShowDialog(this);
            }
        }
    }
}
