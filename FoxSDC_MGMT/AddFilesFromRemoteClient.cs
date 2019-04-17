using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmAddFilesFromRemoteClient : FForm
    {
        string MID;
        string ComputerName;
        ComputerData cd;
        List<string> RemoteFiles = new List<string>();
        public frmAddFilesFromRemoteClient(string MID)
        {
            this.MID = MID;
            Debug.Assert(string.IsNullOrWhiteSpace(this.MID) == false);

            cd = Program.net.GetComputerDetail(MID);
            if (cd == null)
                ComputerName = "?";
            else
                ComputerName = cd.Computername;
            InitializeComponent();
        }

        void LoadList()
        {
            lstFiles.Items.Clear();
            foreach (string file in RemoteFiles)
            {
                ListViewItem l = new ListViewItem(file);
                lstFiles.Items.Add(l);
            }
        }

        private void frmAddFilesFromRemoteClient_Load(object sender, EventArgs e)
        {
            this.Text = "Copy files from " + ComputerName;
            LoadList();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem l in lstFiles.SelectedItems)
            {
                string fe = (string)l.Tag;
                RemoteFiles.Remove(fe);
            }
            LoadList();
        }

        private void cmdSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem l in lstFiles.Items)
            {
                l.Selected = true;
            }
        }

        private void cmdAddFile_Click(object sender, EventArgs e)
        {
            frmRemoteOpenSaveDlg frm = new frmRemoteOpenSaveDlg(MID, cd.SystemRoot, frmRemoteOpenSaveDlg.OpenSaveMode.Open, "Copy files from " + ComputerName,
                "All files|*.*", true, Program.net);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            foreach (string filename in frm.SelectedFilenames)
            {
                if (RemoteFiles.Contains(filename) == false)
                    RemoteFiles.Add(filename);
            }
            LoadList();
        }

        void RecurseFolders(string Folder)
        {
            List<string> files = Program.net.PushGetFiles(MID, true, false, Folder, "*.*");
            if (files == null)
                files = new List<string>();
            foreach (string filename in files)
            {
                if (RemoteFiles.Contains(filename) == false)
                    RemoteFiles.Add(filename);
            }
            List<string> dirs = Program.net.PushGetFiles(MID, false, true, Folder, "*.*");
            if (dirs == null)
                dirs = new List<string>();
            foreach (string dir in dirs)
            {
                RecurseFolders(dir);
            }
        }

        private void cmdDir_Click(object sender, EventArgs e)
        {
            frmRemoteBrowseFolder fldr = new frmRemoteBrowseFolder(MID, cd.SystemRoot, "Copy files from " + ComputerName, Program.net);
            if (fldr.ShowDialog(this) != DialogResult.OK)
                return;
            RecurseFolders(fldr.SelectedFolder);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (RemoteFiles.Count == 0)
            {
                MessageBox.Show(this, "There're no entries to request.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (string file in RemoteFiles)
            {
                retr:
                if (Program.net.File_MGMT_NewAgentUploadReq(file, MID, chkIgnoreMeteredConnection.Checked) == null)
                {
                    DialogResult res = MessageBox.Show(this, "Cannot request file\n" + file + "\n" + Program.net.GetLastError(), Program.Title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    if (res == DialogResult.Abort)
                        return;
                    if (res == DialogResult.Retry)
                        goto retr;
                    if (res == DialogResult.Ignore)
                        continue;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
