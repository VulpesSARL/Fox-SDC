using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_PackageCreator
{
    public partial class frmCompilePackage : FForm
    {
        PKGRootData Package;
        PackageCompiler PackageCompiler = null;
        public frmCompilePackage(PKGRootData package)
        {
            Package = package;
            InitializeComponent();
        }

        private void cmdSelectOutputFilename_Click(object sender, EventArgs e)
        {
            SaveFileDialog cmdlg = new SaveFileDialog();
            cmdlg.Title = "Save Package as";
            cmdlg.Filter = "Fox Package|*.foxpkg";
            cmdlg.DefaultExt = ".foxpkg";
            cmdlg.CheckPathExists = true;
            if (cmdlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            txtOutputFilename.Text = cmdlg.FileName;
        }

        private void frmCompilePackage_Load(object sender, EventArgs e)
        {
            lstCert.Items.Clear();
            lstExtSign.Items.Clear();
            string LastCert = "";

            foreach (string s in Certificates.GetCertificates(StoreLocation.CurrentUser))
            {
                if (Settings.Default.LastCertificate == s)
                    LastCert = s;

                lstCert.Items.Add(s);
            }
            if (LastCert != "")
                lstCert.Text = LastCert;

            string LastExtCert = "";
            foreach (string s in SmartCards.GetCSPProviders())
            {
                if (Settings.Default.LastExtCertificate == s)
                    LastExtCert = s;

                lstExtSign.Items.Add(s);
            }
            if (LastExtCert != "")
                lstExtSign.Text = LastExtCert;

            radCertSign.Checked = true;
            txtOutputFilename.Text = Package.Outputfile;

            lblStatus.Text = "";
            cmdStop.Enabled = false;

            if (lstCert.Text == "")
                if (lstCert.Items.Count > 0)
                    lstCert.SelectedIndex = 1;

            if (lstExtSign.Text == "")
                if (lstExtSign.Items.Count > 0)
                    lstExtSign.SelectedIndex = 1;

            switch (Settings.Default.LastSelectedCertificateType)
            {
                case "EXT":
                    radExtSign.Checked = true;
                    break;
                case "INT":
                    radCertSign.Checked = true;
                    break;
            }
        }

        private void radMULTI_CheckedChanged(object sender, EventArgs e)
        {
            lstCert.Enabled = radCertSign.Checked;
            lstExtSign.Enabled = !radCertSign.Checked;
        }

        private void cmdCompile_Click(object sender, EventArgs e)
        {
            cmdStop.Enabled = true;
            panelSettings.Enabled = false;
            Package.Outputfile = txtOutputFilename.Text.Trim();
            PKGCompilerArgs comp = new PKGCompilerArgs();
            comp.SignCert = lstCert.Text;
            comp.SignLocation = StoreLocation.CurrentUser;
            comp.SignExtCert = lstExtSign.Text;
            comp.UseExtSign = radExtSign.Checked;
            BG.RunWorkerAsync(comp);
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            if (PackageCompiler != null)
                PackageCompiler.AbortProcess = true;
        }

        private void BG_DoWork(object sender, DoWorkEventArgs e)
        {
            string ErrorText;
            string DLLFilename;

            PackageCompiler = new PackageCompiler();
            PackageCompiler.OnStatusUpdate += PackageCompiler_OnStatusUpdate;
            if (PackageCompiler.CheckScript(Package, out ErrorText) == false)
            {
                e.Result = ErrorText;
                e.Cancel = false;
                Debug.WriteLine("BG_DoWork() canceled");
                return;
            }

            if (PackageCompiler.CompileScript(Package.Script, out ErrorText, "", out DLLFilename) == null)
            {
                e.Result = ErrorText;
                e.Cancel = false;
                Debug.WriteLine("BG_DoWork() canceled (Compiler)");
                return;
            }

            if (PackageCompiler.CompilePackage(Package, (PKGCompilerArgs)e.Argument, out ErrorText) == false)
            {
                e.Result = ErrorText;
                e.Cancel = false;
                Debug.WriteLine("BG_DoWork() canceled (2)");
                return;
            }
            e.Cancel = false;
            e.Result = true;
            Debug.WriteLine("BG_DoWork() done");
        }

        delegate void UpdateSta(string Sta);
        void UpdateStatus(string Sta)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new UpdateSta(UpdateStatus), Sta);
                return;
            }
            lblStatus.Text = Sta;
        }

        void PackageCompiler_OnStatusUpdate(string Text)
        {
            UpdateStatus(Text);
        }

        private void BG_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdStop.Enabled = false;
            panelSettings.Enabled = true;
            if (e.Result is bool)
            {
                UpdateStatus("");
                MessageBox.Show(this, "Completed successfully.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                return;
            }
            else
            {
                UpdateStatus("");
                MessageBox.Show(this, "Process failed: " + e.Result.ToString(), Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmCompilePackage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (radCertSign.Checked == true)
            {
                Settings.Default.LastCertificate = lstCert.Text;
                Settings.Default.LastSelectedCertificateType = "INT";
            }
            else
            {
                Settings.Default.LastExtCertificate = lstExtSign.Text;
                Settings.Default.LastSelectedCertificateType = "EXT";
            }
        }
    }
}
