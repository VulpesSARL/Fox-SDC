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

namespace FoxSDC_PackageCreator
{
    public partial class frmFileProperties : FForm
    {
        public string newpaath;
        public PKGFile file;

        public frmFileProperties(string path)
        {
            newpaath = path;
            file = null;
            InitializeComponent();
        }

        public frmFileProperties(PKGFile pkgfile)
        {
            file = pkgfile;
            newpaath = null;
            InitializeComponent();
        }

        private void frmFileProperties_Load(object sender, EventArgs e)
        {
            if (file == null)
            {
                txtDestPath.Text = newpaath;
                chkInstall.Checked = true;
            }
            else
            {
                txtDestPath.Text = file.FolderName;
                txtDestFilename.Text = file.FileName;
                txtSrcFilename.Text = file.SrcFile;
                txtID.Text = file.ID;
                chkInstall.Checked = file.InstallThisFile;
                chkKeepMeta.Checked = file.KeepInMeta;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (file == null)
            {
                file = new PKGFile();
                file.FolderName = txtDestPath.Text.Trim();
                file.FileName = txtDestFilename.Text.Trim();
                file.SrcFile = txtSrcFilename.Text.Trim();
                file.ID = txtID.Text.Trim();
                file.InstallThisFile = chkInstall.Checked;
                file.KeepInMeta = chkKeepMeta.Checked;
            }
            else
            {
                file.FolderName = txtDestPath.Text.Trim();
                file.FileName = txtDestFilename.Text.Trim();
                file.SrcFile = txtSrcFilename.Text.Trim();
                file.ID = txtID.Text.Trim();
                file.InstallThisFile = chkInstall.Checked;
                file.KeepInMeta = chkKeepMeta.Checked;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Filter = "All files|*.*";
            cmdlg.Title = "Browse for source file";
            cmdlg.Multiselect = false;
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            txtSrcFilename.Text = cmdlg.FileName;
        }
    }
}
