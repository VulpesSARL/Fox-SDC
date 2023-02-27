using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmAddFilesToRemoteClient : FForm
    {
        class FileEntry
        {
            public string LocalFile;
            public string RemoteFile;
            public Int64 Size;
        }

        List<FileEntry> Entries = new List<FileEntry>();

        bool UseMassUpload = false;
        Int64 GroupID;
        string GroupFullPath;

        string MID;
        string ComputerName;
        public frmAddFilesToRemoteClient(string MID)
        {
            this.MID = MID;
            Debug.Assert(string.IsNullOrWhiteSpace(this.MID) == false);

            ComputerData cd = Program.net.GetComputerDetail(MID);
            if (cd == null)
                ComputerName = "?";
            else
                ComputerName = cd.Computername;
            InitializeComponent();
        }

        public frmAddFilesToRemoteClient(Int64 GroupID, string GroupFullPath)
        {
            this.GroupFullPath = GroupFullPath;
            this.GroupID = GroupID;
            UseMassUpload = true;
            InitializeComponent();
        }

        private void frmAddFilesToRemoteClient_Load(object sender, EventArgs e)
        {
            if (UseMassUpload == false)
            {
            this.Text = "Copy files to " + ComputerName;
            }
            else
            {
                this.Text = "Copy files to group " + GroupFullPath;
            }
            LoadList();
        }

        void LoadList()
        {
            lstFiles.Items.Clear();
            Int64 TotalSize = 0;
            foreach (FileEntry fe in Entries)
            {
                ListViewItem l = new ListViewItem(fe.LocalFile);
                l.SubItems.Add(fe.RemoteFile);
                l.SubItems.Add(CommonUtilities.NiceSize(fe.Size));
                l.Tag = fe;
                lstFiles.Items.Add(l);
                TotalSize += fe.Size;
            }
            lblTotalSize.Text = CommonUtilities.NiceSize(TotalSize);
        }

        private void cmdAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Title = "Add files";
            cmdlg.Filter = "All files|*.*";
            cmdlg.Multiselect = true;
            cmdlg.SupportMultiDottedExtensions = true;
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != DialogResult.OK)
                return;
            foreach (string Filename in cmdlg.FileNames)
            {
                FileEntry fe = new FileEntry();
                fe.LocalFile = Filename;
                fe.RemoteFile = Filename;
                FileInfo fi = new FileInfo(Filename);
                fe.Size = fi.Length;
                Entries.Add(fe);
            }
            LoadList();
        }

        private void cmdDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fldr = new FolderBrowserDialog();
            fldr.RootFolder = Environment.SpecialFolder.MyComputer;
            fldr.ShowNewFolderButton = false;
            fldr.Description = "Select folder";
            if (fldr.ShowDialog(this) != DialogResult.OK)
                return;

            foreach (string Filename in Directory.EnumerateFiles(fldr.SelectedPath, "*.*", SearchOption.AllDirectories))
            {
                FileEntry fe = new FileEntry();
                fe.LocalFile = Filename;
                fe.RemoteFile = Filename;
                FileInfo fi = new FileInfo(Filename);
                fe.Size = fi.Length;
                Entries.Add(fe);
            }
            LoadList();
        }

        private void cmdChgRDir_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0)
                return;

            List<string> s = new List<string>();
            foreach (ListViewItem l in lstFiles.SelectedItems)
            {
                FileEntry fe = (FileEntry)l.Tag;
                s.Add(fe.RemoteFile);
            }

            string Same = CommonUtilities.GetCommonBeginning(s);
            string New = "";
            if (string.IsNullOrWhiteSpace(Same) == true)
            {
                MessageBox.Show(this, "Paths are different.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmChangePaths frm = new frmChangePaths("Change paths", Same, MID, UseMassUpload == false ? frmChangePaths.BrowseWhere.Remote : frmChangePaths.BrowseWhere.None);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;

            New = frm.ReturnedPath;
            if (New.Trim() == "")
                return;

            if (Same.EndsWith("\\") == true)
            {
                if (New.EndsWith("\\") == false)
                    New += "\\";
            }
            else
            {
                if (New.EndsWith("\\") == true)
                    New = New.Substring(0, New.Length - 1);
            }

            foreach (ListViewItem l in lstFiles.SelectedItems)
            {
                FileEntry fe = (FileEntry)l.Tag;
                if (fe.RemoteFile.ToLower().StartsWith(Same.ToLower()) == false)
                    continue;
                fe.RemoteFile = New + fe.RemoteFile.Substring(Same.Length, fe.RemoteFile.Length - Same.Length);
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

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem l in lstFiles.SelectedItems)
            {
                FileEntry fe = (FileEntry)l.Tag;
                Entries.Remove(fe);
            }
            LoadList();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (Entries.Count == 0)
            {
                MessageBox.Show(this, "There're no entries to upload.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (UseMassUpload == false)
            {
            List<FileEntry> pf = new List<FileEntry>();
            string Error = "";
            foreach (FileEntry file in Entries)
            {
                if (UploadDownloadDataThread.AddUploadToServer(MID, file.LocalFile, file.RemoteFile, chkIgnoreMeteredConnection.Checked, chkExecuteWhenDone.Checked, out Error) == false)
                {
                    MessageBox.Show(this, "There's an error processing the file\n" +
                        file.LocalFile + " -> " + file.RemoteFile + "\n" + Error, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (FileEntry file2 in pf)
                        Entries.Remove(file2);
                    LoadList();
                    return;
                }
            }
            }
            else
            {
                foreach(string CurrentMID in GetComputersMID(GroupID))
                {
                    List<FileEntry> pf = new List<FileEntry>();
                    string Error = "";
                    foreach (FileEntry file in Entries)
                    {
                        if (UploadDownloadDataThread.AddUploadToServer(CurrentMID, file.LocalFile, file.RemoteFile, chkIgnoreMeteredConnection.Checked, chkExecuteWhenDone.Checked, out Error) == false)
                        {
                            MessageBox.Show(this, "There's an error processing the file for MID " + CurrentMID + "\n" +
                                file.LocalFile + " -> " + file.RemoteFile + "\n" + Error, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            foreach (FileEntry file2 in pf)
                                Entries.Remove(file2);
                            LoadList();
                            return;
                        }
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private List<string> GetComputersMID(Int64 GroupID)
        {
            List<string> MIDs = new List<string>();

            List<GroupElement> gel = Program.net.GetGroups(GroupID);
            if (gel != null)
            {
                foreach (GroupElement g in gel)
                {
                    MIDs.AddRange(GetComputersMID(g.ID));
                }
            }

            List<ComputerData> cdlst = Program.net.GetComputerList(true, GroupID);
            if (cdlst != null)
            {
                foreach (ComputerData pc in cdlst)
                {
                    MIDs.Add(pc.MachineID);
                }
            }

            return (MIDs);
        }

    }
}
