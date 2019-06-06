using FoxSDC_Common;
using FoxSDC_MGMT.Properties;
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
    public partial class MainDLG : FForm
    {
        public MainDLG()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            this.Text = Program.Title + " - Connected to " + Program.net.serverinfo.Name;
            connectToolStripMenuItem.Enabled = false;
            timer_ping.Enabled = true;

            //Build tree
            treeAction.Nodes.Clear();
            treeAction.Nodes.Add("server", "This server", 14, 14);
            treeAction.Nodes.Add("unapprovcomputer", "Unapproved computers", 2, 2);
            treeAction.Nodes.Add("allcomputers", "All computers", 2, 2);
            treeAction.Nodes.Add("allprograms", "All programs", 3, 3);
            treeAction.Nodes.Add("alldiskdata", "All disk status", 7, 7);
            treeAction.Nodes.Add("alleventlogs", "All Event logs", 11, 11);
            treeAction.Nodes.Add("allstartup", "Startup Elements", 22, 22);
            treeAction.Nodes.Add("pendingchats", "Pending chats", 16, 16);
            treeAction.Nodes.Add("uploaddownload", "Uploads / Downloads", 21, 21);
            treeAction.Nodes.Add("simpletasks", "Simple Tasks", 23, 23);
            GroupFolders.CreateRootFolder(treeAction);
            treeAction.SelectedNode = treeAction.Nodes[0];

            settingsToolStripMenuItem.Enabled = true;
            uploadPackageToolStripMenuItem.Enabled = true;
            deletePackageToolStripMenuItem.Enabled = true;
            createReportToolStripMenuItem.Enabled = true;

            UploadDownloadDataThread.StartThread();
        }

        private void MainDLG_Load(object sender, EventArgs e)
        {
            this.Text = Program.Title + " - Not connected";
            this.Show();
            settingsToolStripMenuItem.Enabled = false;
            uploadPackageToolStripMenuItem.Enabled = false;
            deletePackageToolStripMenuItem.Enabled = false;
            createReportToolStripMenuItem.Enabled = false;
            Program.LoadImageList(imageList1);
            connectToolStripMenuItem_Click(sender, e);
        }

        private void treeAction_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Splitty.Panel2.Controls.Clear();
            switch (e.Node.Name)
            {
                case "server":
                    {
                        ctlFirstPage ctl = new ctlFirstPage();
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "unapprovcomputer":
                    {
                        ctlListPCs ctl = new ctlListPCs(false, null);
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "allcomputers":
                    {
                        ctlListPCs ctl = new ctlListPCs(null, null);
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "allprograms":
                    {
                        ctlAddRemovePrograms ctl = new ctlAddRemovePrograms("");
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "alldiskdata":
                    {
                        ctlListDiskData ctl = new ctlListDiskData("");
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "alleventlogs":
                    {
                        ctlEventLogs ctl = new ctlEventLogs();
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "allstartup":
                    {
                        ctlStartupItems ctl = new ctlStartupItems("");
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "pendingchats":
                    {
                        ctlPendingChats ctl = new ctlPendingChats();
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "uploaddownload":
                    {
                        ctlUploadDownloadStatus ctl = new ctlUploadDownloadStatus();
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
                case "simpletasks":
                    {
                        ctlSimpleTasks ctl = new ctlSimpleTasks("");
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                        break;
                    }
            }

            Int64? SelectedGroup;
            Int64? SelectedPolicy;
            if (GroupFolders.AfterSelect(treeAction, e, out SelectedGroup, out SelectedPolicy) == true)
            {
                if (SelectedGroup != null)
                {
                    ctlListPCs ctl = new ctlListPCs(null, SelectedGroup);
                    ctl.Dock = DockStyle.Fill;
                    Splitty.Panel2.Controls.Add(ctl);
                }
                if (SelectedPolicy != null)
                {
                    PolicyObject obj = Program.net.GetPolicyObject(SelectedPolicy.Value);
                    if (obj == null)
                    {
                        MessageBox.Show(this, "Loading policy failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        PolicyElementInterface i = PolicyList.GetInstance(obj.Type);
                        i.SetData(obj);
                        UserControl ctl = (UserControl)i;
                        ctl.Dock = DockStyle.Fill;
                        Splitty.Panel2.Controls.Add(ctl);
                    }
                }
            }
        }

        private void treeAction_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            GroupFolders.BeforeExpand(e, true);
        }

        private void createGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid;
            Int64? ID = GroupFolders.GetSelectedGroupID(treeAction.SelectedNode, out valid);
            if (valid == false)
                return;
            frmCreateGroup frm = new frmCreateGroup(ID, ID == null ? "(Root)" : treeAction.SelectedNode.Text, false);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                GroupFolders.UpdateTreeNode(treeAction.SelectedNode, true);
        }

        private void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid;
            Int64? ID = GroupFolders.GetSelectedGroupID(treeAction.SelectedNode, out valid);
            if (valid == false)
                return;
            if (ID == null)
            {
                MessageBox.Show(this, "Cannot delete the Root Group", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show(this, "Do you really want to delete the Group " + treeAction.SelectedNode.Text + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                return;
            if (Program.net.DeleteGroup(ID.Value) == false)
            {
                MessageBox.Show(this, "Delete group failed: " + Program.net.GetLastError() + "\nMake sure the group is empty.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            treeAction.SelectedNode.Parent.Nodes.Remove(treeAction.SelectedNode);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            createGroupToolStripMenuItem.Enabled = false;
            deleteGroupToolStripMenuItem.Enabled = false;
            refreshGroupToolStripMenuItem.Enabled = false;
            renameGroupToolStripMenuItem.Enabled = false;
            createpolicyToolStripMenuItem.Enabled = false;
            deletePolicyToolStripMenuItem.Enabled = false;
            policyEnabledToolStripMenuItem.Enabled = false;
            lowLevelEditPolicyToolStripMenuItem.Enabled = false;
            policyEnabledToolStripMenuItem.Text = "Enable/Disable polic&y";

            if (treeAction.SelectedNode == null)
                return;

            if (treeAction.SelectedNode.Name.StartsWith("grp:") == true)
            {
                createpolicyToolStripMenuItem.Enabled = true;
                createGroupToolStripMenuItem.Enabled = true;
                refreshGroupToolStripMenuItem.Enabled = true;
                if (treeAction.SelectedNode.Name.StartsWith("grp:root") == false)
                {
                    deleteGroupToolStripMenuItem.Enabled = true;
                    renameGroupToolStripMenuItem.Enabled = true;
                }
                else
                {
                    deleteGroupToolStripMenuItem.Enabled = false;
                    renameGroupToolStripMenuItem.Enabled = false;
                }
            }
            else
            {
                createGroupToolStripMenuItem.Enabled = false;
                deleteGroupToolStripMenuItem.Enabled = false;
                refreshGroupToolStripMenuItem.Enabled = false;
                renameGroupToolStripMenuItem.Enabled = false;
            }

            if (treeAction.SelectedNode.Name.StartsWith("pol:") == true)
            {
                deletePolicyToolStripMenuItem.Enabled = true;
                policyEnabledToolStripMenuItem.Enabled = true;
                if (Settings.Default.EnableDebug == true)
                {
                    lowLevelEditPolicyToolStripMenuItem.Visible = true;
                    lowLevelEditPolicyToolStripMenuItem.Enabled = true;
                }
                else
                {
                    lowLevelEditPolicyToolStripMenuItem.Visible = false;
                    lowLevelEditPolicyToolStripMenuItem.Enabled = false;
                }
                bool valid = false;
                Int64? PolID = GroupFolders.GetSelectedPolicyID(treeAction.SelectedNode, out valid);
                if (valid == false)
                {
                    policyEnabledToolStripMenuItem.Enabled = false;
                }
                else
                {
                    PolicyObject obj = Program.net.GetPolicyObject(PolID.Value);
                    if (obj == null)
                    {
                        policyEnabledToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        policyEnabledToolStripMenuItem.Enabled = true;
                        policyEnabledToolStripMenuItem.Text = obj.Enabled == true ? "Disable polic&y" : "Enable polic&y";
                    }
                }
            }
        }

        private void refreshGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid;
            Int64? ID = GroupFolders.GetSelectedGroupID(treeAction.SelectedNode, out valid);
            if (valid == false)
                return;
            GroupFolders.UpdateTreeNode(treeAction.SelectedNode, true);
        }

        private void renameGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid;
            Int64? ID = GroupFolders.GetSelectedGroupID(treeAction.SelectedNode, out valid);
            if (valid == false)
                return;
            if (ID == null)
            {
                MessageBox.Show(this, "Cannot rename the Root Group: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmCreateGroup frm = new frmCreateGroup(ID, ID == null ? "(Root)" : treeAction.SelectedNode.Text, true);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                treeAction.SelectedNode.Text = frm.NewName.Trim();

        }

        private void createpolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid;
            Int64? ID = GroupFolders.GetSelectedGroupID(treeAction.SelectedNode, out valid);
            if (valid == false)
                return;
            frmCreatePolicy frm = new frmCreatePolicy(null, ID);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                GroupFolders.UpdateTreeNode(treeAction.SelectedNode, true);
        }

        private void policyEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid = false;
            Int64? PolID = GroupFolders.GetSelectedPolicyID(treeAction.SelectedNode, out valid);
            if (valid == false)
                return;

            PolicyObject obj = Program.net.GetPolicyObject(PolID.Value);
            if (obj == null)
                return;

            if (Program.net.EnableDisablePolicy(obj.ID, !obj.Enabled) == false)
            {
                MessageBox.Show(this, "Cannot enable/disable policy: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            treeAction.SelectedNode.Text = obj.Name + (obj.Enabled == false ? "" : " [disabled]");
        }

        private void deletePolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid = false;
            Int64? PolID = GroupFolders.GetSelectedPolicyID(treeAction.SelectedNode, out valid);
            if (valid == false)
                return;

            PolicyObject obj = Program.net.GetPolicyObject(PolID.Value);
            if (obj == null)
                return;

            if (MessageBox.Show(this, "Do you want to delete the policy " + obj.Name + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
                return;

            if (Program.net.DeletePolicy(obj.ID) == false)
            {
                MessageBox.Show(this, "Cannot delete policy: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            treeAction.SelectedNode.Parent.Nodes.Remove(treeAction.SelectedNode);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSeverSettings frm = new frmSeverSettings();
            frm.ShowDialog(this);
        }

        private void createCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateCertificate frm = new frmCreateCertificate();
            frm.ShowDialog(this);
        }

        private void uploadPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Fox Packages|*.foxpkg";
            open.Title = "Upload Packages";
            open.Multiselect = true;
            if (open.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            foreach (string filename in open.FileNames)
            {
                frmUploadProgress frm = new frmUploadProgress(filename, 0x0);
                if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    break;
            }
        }

        private void deletePackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPackages frm = new frmListPackages(true);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            foreach (Int64 i in frm.IDs)
            {
                Program.net.DeletePackage(i);
            }
        }

        private void timer_ping_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Program.net != null)
                    Program.net.Ping();
            }
            catch
            {

            }
        }

        private void lowLevelEditPolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool valid = false;
            Int64? PolID = GroupFolders.GetSelectedPolicyID(treeAction.SelectedNode, out valid);
            if (valid == false)
            {
                policyEnabledToolStripMenuItem.Enabled = false;
            }
            else
            {
                PolicyObject obj = Program.net.GetPolicyObject(PolID.Value);
                if (obj == null)
                    return;
                frmLowLevelEdit frm = new frmLowLevelEdit(obj);
                frm.ShowDialog(this);
            }
        }

        private void createReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRequestReport frm = new frmRequestReport();
            frm.ShowDialog(this);
        }

        private void MainDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UploadDownloadDataThread.DataListRunning > 0)
            {
                if (MessageBox.Show(this, "Ther're some upload / downloads running. If you close the Management now, these will be canceled.\nDo you want to close the Management Window?",
                    Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            UploadDownloadDataThread.CancelThread = true;
        }
    }
}
