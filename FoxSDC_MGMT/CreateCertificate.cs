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
    public partial class frmCreateCertificate : FForm
    {
        public frmCreateCertificate()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtCN.Text.Trim() == "")
            {
                MessageBox.Show(this, "Enter a CN (Common Name) for the new certificate.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtPW.Text == "")
            {
                if (MessageBox.Show(this, "Do you want to create a certificate without a password. Note that the private key can be read, and false or malicious data can be signed.", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Information) != System.Windows.Forms.DialogResult.Yes)
                    return;
            }

            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Certificate files|*.pfx";
            file.DefaultExt = ".pfx";
            file.Title = "Create certificate with private key";
            file.CheckPathExists = true;
            if (file.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
            {
                MessageBox.Show(this, "Operation canceled.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string FilenamePFX = file.FileName;

            file = new SaveFileDialog();
            file.Filter = "Certificate files|*.cer";
            file.DefaultExt = ".cer";
            file.Title = "Create public certificate";
            file.CheckPathExists = true;
            if (file.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
            {
                MessageBox.Show(this, "Operation canceled.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string FilenameCER = file.FileName;

            byte[] CERData = null;
            byte[] PFXData = CertificateCreation.GenerateRootCertificate(txtCN.Text.Trim(), txtPW.Text, out CERData);

            try
            {
                File.WriteAllBytes(FilenamePFX, PFXData);
                File.WriteAllBytes(FilenameCER, CERData);
            }
            catch
            {
                MessageBox.Show(this, "Operation failed.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            MessageBox.Show(this, "Operation completed successfully.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            return;
        }
    }
}
