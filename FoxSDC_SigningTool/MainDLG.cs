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

namespace FoxSDC_SigningTool
{
    public partial class MainDLG : FForm
    {
        public MainDLG()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void signPlainfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string CN = "";
            frmAskCert acert = new frmAskCert();
            if (acert.ShowDialog(this) != DialogResult.OK)
                return;
            CN = acert.SelectedCert;

            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Filter = "All files files|*.*";
            cmdlg.Title = "Select any file to sign";
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != DialogResult.OK)
                return;
            FileStream file = new FileStream(cmdlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] sign = Certificates.Sign(file, CN, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);
            file.Close();

            if (sign == null)
                return;

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Signature|*.sig";
            save.DefaultExt = ".sig";
            save.Title = "Save Signature file";
            if (save.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            File.WriteAllBytes(save.FileName, sign);
        }

        private void createPlainCERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateCER frm = new frmCreateCER();
            frm.ShowDialog(this);
        }

        private void signPlainFilewithcardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelectSmartcard frm = new frmSelectSmartcard();
            if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Filter = "All files files|*.*";
            cmdlg.Title = "Select any file to sign";
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != DialogResult.OK)
                return;
            byte[] data = File.ReadAllBytes(cmdlg.FileName);
            SmartCards sm = new SmartCards();
            sm.ParentWindowHandle = this.Handle;
            byte[] sign = sm.SignData(frm.Service, data, frm.Pin);
            if (sign == null)
            {
                MessageBox.Show(this, "Cannot sign: " + sm.LastError, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (sign == null)
                return;

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Signature|*.sig";
            save.DefaultExt = ".sig";
            save.Title = "Save Signature file";
            if (save.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            File.WriteAllBytes(save.FileName, sign);
        }
    }
}
