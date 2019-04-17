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
    public partial class frmAddFilesToLocal : FForm
    {
        string MID;
        ComputerData cd;
        string ComputerName;

        class FileEntry
        {
            public string LocalFile;
            public string RemoteFile;
            public string MD5;
            public Int64 Size;
            public Int64 ID;
        }

        List<FileEntry> Entries = new List<FileEntry>();

        public frmAddFilesToLocal(string MID)
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

        private void frmAddFilesToLocal_Load(object sender, EventArgs e)
        {
            this.Text = "Copy files from " + ComputerName + " to here";
            List<FileUploadData> datas = Program.net.File_MGMT_GetFullFileList(MID);
            if (datas == null)
                return;
            foreach (FileUploadData data in datas)
            {
                if (data.Direction != 2)
                    continue;
                Entries.Add(new FileEntry()
                {
                    ID = data.ID,
                    LocalFile = data.RemoteFileLocation,
                    RemoteFile = data.RemoteFileLocation,
                    Size = data.Size,
                    MD5 = data.MD5CheckSum
                });
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

            frmChangePaths frm = new frmChangePaths("Change paths", Same, MID, frmChangePaths.BrowseWhere.Local);
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

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (Entries.Count == 0)
            {
                MessageBox.Show(this, "There're no entries to download.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<FileEntry> pf = new List<FileEntry>();

            string Error = "";
            foreach (FileEntry file in Entries)
            {
                if (UploadDownloadDataThread.AddDownloadFromServer(MID, file.ID, file.Size, file.RemoteFile, file.LocalFile, file.MD5, out Error) == false)
                {
                    MessageBox.Show(this, "There's an error processing the file\n" +
                        file.LocalFile + " -> " + file.RemoteFile + "\n" + Error, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (FileEntry file2 in pf)
                        Entries.Remove(file2);
                    LoadList();
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
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
    }
}
