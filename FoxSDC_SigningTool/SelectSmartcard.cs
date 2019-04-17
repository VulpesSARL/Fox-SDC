using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_SigningTool
{
    public partial class frmSelectSmartcard : Form
    {
        public SecureString Pin;
        public string Service;

        public frmSelectSmartcard()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Pin = new SecureString();
            foreach (char c in txtPin.Text)
                Pin.AppendChar(c);
            Service = lstSC.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void frmSelectSmartcard_Load(object sender, EventArgs e)
        {
            List<string> srv = SmartCards.GetCSPProviders();
            foreach (string s in srv)
            {
                lstSC.Items.Add(s);
            }
            if (lstSC.Items.Count > 0)
                lstSC.SelectedIndex = 0;
        }
    }
}
