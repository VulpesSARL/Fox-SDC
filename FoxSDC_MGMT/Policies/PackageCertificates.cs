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
using System.IO;

namespace FoxSDC_MGMT
{
    public partial class ctlPackageCertificates : UserControl, PolicyElementInterface
    {
        PolicyObject Pol;
        PolicyPackageCertificates Cert;
        public ctlPackageCertificates()
        {
            InitializeComponent();
        }

        void UpdateStatus()
        {
            if (Cert.UUCerFile == null)
                Cert.UUCerFile = "";

            if (Cert.UUCerFile.Trim() == "")
            {
                lblInstCert.Text = "not set";
                cmdSave.Enabled = false;
            }
            else
            {
                try
                {
                    cmdSave.Enabled = true;
                    string S = Certificates.GetCN(Convert.FromBase64String(Cert.UUCerFile));
                    if (S == null)
                    {
                        lblInstCert.Text = "<ERROR>";
                        cmdSave.Enabled = false;
                    }
                    else
                    {
                        lblInstCert.Text = S;
                    }
                }
                catch
                {
                    lblInstCert.Text = "<ERROR>";
                    cmdSave.Enabled = false;
                }               
            }
        }

        private void ctlCertificates_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;
            UpdateStatus();
        }

        public bool SetData(FoxSDC_Common.PolicyObject obj)
        {
            Pol = obj;

            Cert = JsonConvert.DeserializeObject<PolicyPackageCertificates>(obj.Data);
            if (Cert == null)
                Cert = new PolicyPackageCertificates();
            UpdateStatus();
            return (true);
        }

        public string GetData()
        {
            return (JsonConvert.SerializeObject(Cert));
        }

        private void cmdAddCer_Click(object sender, EventArgs e)
        {
            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Filter = "Certificate files|*.cer";
            cmdlg.Title = "Select Certificate file";
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != DialogResult.OK)
                return;
            FileInfo file = new FileInfo(cmdlg.FileName);
            if (file.Length > 1048576)
            {
                MessageBox.Show(this, "File too large (>1 MB).", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                Cert.UUCerFile = Convert.ToBase64String(File.ReadAllBytes(cmdlg.FileName));
                UpdateStatus();
            }
            catch
            {
                MessageBox.Show(this, "Cannot read the file.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cert.UUCerFile = "";
                UpdateStatus();
            }
        }  

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string data = GetData();
            Program.net.EditPolicy(Pol.ID, data);
        }
    }
}
