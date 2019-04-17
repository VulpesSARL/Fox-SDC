using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmRemoteOpenSaveDlg : FForm
    {
        public enum OpenSaveMode
        {
            Open,
            Save
        }

        OpenSaveMode Mode;
        string Title;
        string[] Filter;
        Network net;
        bool MultiSelect;
        string MachineID;
        string SystemRoot;
        bool Init = true;
        public string SelectedFilename = "";
        public List<string> SelectedFilenames = new List<string>();

        public frmRemoteOpenSaveDlg(string MachineID, string SystemRoot, OpenSaveMode mode, string Title, string Filter, bool MultiSelect, Network net)
        {
            this.Mode = mode;
            this.Title = Title;
            this.Filter = Filter.Split('|');
            this.net = net;
            this.MultiSelect = MultiSelect;
            this.MachineID = MachineID;
            this.SystemRoot = SystemRoot;
            InitializeComponent();
        }

        void SelectDrive(string PPath, bool Update = true)
        {
            for (int i = 0; i < lstDrives.Items.Count; i++)
            {
                if (PPath.StartsWith(lstDrives.Items[i].ToString().Split(' ')[0]) == true)
                {
                    lstDrives.SelectedIndex = i;
                    return;
                }
            }
            lstDrives.SelectedIndex = -1;

            if (Update == true)
            {
                if (PPath.EndsWith("\\") == false)
                    PPath += "\\";
                SelectPath(PPath, true);
            }
        }

        void SelectPath(string PPath, bool Update = true)
        {
            lblDir.Text = PPath;
            this.toolTip1.SetToolTip(this.lblDir, PPath);

            if (Update == true)
            {
                List<string> folders = net.PushGetFiles(MachineID, false, true, PPath, "*.*");
                if (folders == null)
                    return;
                lstDirs.ListPath(PPath, folders);
            }
        }

        void ListFiles(string PPath)
        {
            lstFiles.Items.Clear();
            List<string> files = net.PushGetFiles(MachineID, true, false, PPath, Filter[(lstType.SelectedIndex * 2) + 1]);
            if (files == null)
                return;
            lstFiles.Items.AddRange(files.ToArray());
        }

        private void frmRemoteOpenSaveDlg_Load(object sender, EventArgs e)
        {
            this.Text = Title;
            lblDir.Text = "";
            this.toolTip1.SetToolTip(this.lblDir, "");

            List<string> drives = net.PushGetFiles(MachineID, false, false, "drives", "");
            if (drives == null)
            {
                cmdOK.Enabled = false;
                lstDirs.Enabled = lstDrives.Enabled = lstFiles.Enabled = lstType.Enabled = txtFilename.Enabled = false;
                MessageBox.Show(this, "Cannot list drives from the remote computer", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (string drive in drives)
                lstDrives.Items.Add(drive);
            for (int i = 0; i < Filter.Length; i += 2)
            {
                lstType.Items.Add(Filter[i]);
            }
            lstType.SelectedIndex = 0;
            lstFiles.SelectionMode = MultiSelect == true ? SelectionMode.MultiExtended : SelectionMode.One;
            Init = false;
            if (string.IsNullOrWhiteSpace(SystemRoot) == false)
            {
                SelectDrive(SystemRoot, false);
                SelectPath(SystemRoot);
                ListFiles(SystemRoot);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Init == true)
                return;
            ListFiles(lblDir.Text);
        }

        private void lstDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Init == true)
                return;
            string Drive = lstDrives.SelectedItem.ToString().Split(' ')[0];
            SelectPath(Drive, true);
            ListFiles(Drive);
        }

        private void lstDirs_DoubleClick(object sender, EventArgs e)
        {
            if (Init == true)
                return;
            if (lstDirs.SelectedItem == null)
                return;
            NT3PathListBox.NT3PathElement ele = (NT3PathListBox.NT3PathElement)lstDirs.SelectedItem;
            SelectPath(ele.FullFolderName, true);
            ListFiles(ele.FullFolderName);
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Init == true)
                return;
            if (lstFiles.SelectedIndex == -1)
                return;
            txtFilename.Text = lstFiles.SelectedItem.ToString();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            string Filename = "";

            if (txtFilename.Text.Contains("\\") == true)
            {
                Filename = txtFilename.Text;
            }
            else
            {
                Filename = lblDir.Text;
                if (Filename.EndsWith("\\") == false)
                    Filename += "\\";
                Filename += txtFilename.Text;

            }

            PushFileState res = net.PushCheckFileExistence(MachineID, Filename);
            if (res == PushFileState.File)
            {
                if (MultiSelect == true)
                {
                    if (lstFiles.SelectedItems.Count == 0)
                        return;
                    string dir = lblDir.Text;
                    if (dir.EndsWith("\\") == false)
                        dir += "\\";
                    foreach (string file in lstFiles.SelectedItems)
                    {
                        SelectedFilenames.Add(dir + file);
                    }
                    SelectedFilename = SelectedFilenames[0];
                }
                else
                {
                    SelectedFilename = Filename;
                    SelectedFilenames.Add(SelectedFilename);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            if (res == PushFileState.Folder)
            {
                SelectPath(Filename, true);
                ListFiles(Filename);
                return;
            }
            if (res == PushFileState.NotExistent)
            {
                MessageBox.Show(this, "The remote system cannot find the given file or directory.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (res == PushFileState.Error)
            {
                MessageBox.Show(this, "An error occoured with the connection.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (res == PushFileState.RemoteError)
            {
                MessageBox.Show(this, "The remote system reported an error (maybe invalid path/filename).", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void lstFiles_DoubleClick(object sender, EventArgs e)
        {
            cmdOK_Click(sender, e);
        }
    }
}
