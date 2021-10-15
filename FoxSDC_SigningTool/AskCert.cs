using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_SigningTool
{
    public partial class frmAskCert : FForm
    {
        public string SelectedCert = "";

        public frmAskCert()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (lstCert.SelectedIndex == -1)
                return;
            SelectedCert = lstCert.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmAskCert_Load(object sender, EventArgs e)
        {
            foreach (string s in Certificates.GetCertificates(StoreLocation.CurrentUser))
            {
                lstCert.Items.Add(s);
            }
            if (lstCert.Items.Count > 0)
                lstCert.SelectedIndex = 0;
        }
    }
}
