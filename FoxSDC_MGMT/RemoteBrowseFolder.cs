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
    public partial class frmRemoteBrowseFolder : FForm
    {
        string Title;
        Network net;
        string MachineID;
        string SystemRoot;
        bool Init = true;
        public string SelectedFolder = "";

        public frmRemoteBrowseFolder(string MachineID, string SystemRoot, string Title, Network net)
        {
            this.Title = Title;
            this.net = net;
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

        private void frmRemoteBrowseFolder_Load(object sender, EventArgs e)
        {
            this.Text = Title;
            lblDir.Text = "";
            this.toolTip1.SetToolTip(this.lblDir, "");

            List<string> drives = net.PushGetFiles(MachineID, false, false, "drives", "");
            if (drives == null)
            {
                cmdOK.Enabled = false;
                lstDirs.Enabled = lstDrives.Enabled = false;
                MessageBox.Show(this, "Cannot list drives from the remote computer", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (string drive in drives)
                lstDrives.Items.Add(drive);           
            Init = false;
            if (string.IsNullOrWhiteSpace(SystemRoot) == false)
            {
                SelectDrive(SystemRoot, false);
                SelectPath(SystemRoot);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            SelectedFolder = lblDir.Text;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lstDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Init == true)
                return;
            string Drive = lstDrives.SelectedItem.ToString().Split(' ')[0];
            SelectPath(Drive, true);
        }

        private void lstDirs_DoubleClick(object sender, EventArgs e)
        {
            if (Init == true)
                return;
            if (lstDirs.SelectedItem == null)
                return;
            NT3PathListBox.NT3PathElement ele = (NT3PathListBox.NT3PathElement)lstDirs.SelectedItem;
            SelectPath(ele.FullFolderName, true);
        }
    }
}
