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
    public partial class frmChangePaths : FForm
    {
        public enum BrowseWhere
        {
            Local,
            Remote,
            None
        }

        string Title;
        string From;
        string MID;
        BrowseWhere BW;
        public string ReturnedPath;

        public frmChangePaths(string Title, string From, string MID, BrowseWhere BW)
        {
            this.Title = Title;
            this.From = From;
            this.BW = BW;
            this.MID = MID;
            InitializeComponent();
        }

        private void frmChangePaths_Load(object sender, EventArgs e)
        {
            txtTo.Text = From;
            lblFrom.Text = From;
            this.Text = Title;
            if (BW == BrowseWhere.None)
                cmdBrowse.Enabled = false;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            switch (BW)
            {
                case BrowseWhere.Local:
                    FolderBrowserDialog fldr = new FolderBrowserDialog();
                    fldr.RootFolder = Environment.SpecialFolder.MyComputer;
                    fldr.ShowNewFolderButton = false;
                    fldr.Description = "Select folder";
                    if (fldr.ShowDialog(this) != DialogResult.OK)
                        return;
                    txtTo.Text = fldr.SelectedPath;
                    break;
                case BrowseWhere.Remote:
                    ComputerData cd = Program.net.GetComputerDetail(MID);
                    if (cd == null)
                    {
                        MessageBox.Show(this, "Cannot get computer informations", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    frmRemoteBrowseFolder frm = new frmRemoteBrowseFolder(MID, cd.SystemRoot, "Select folder", Program.net);
                    if (frm.ShowDialog(this) != DialogResult.OK)
                        return;
                    txtTo.Text = frm.SelectedFolder;
                    break;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtTo.Text.Trim() == "")
                return;
            ReturnedPath = txtTo.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
