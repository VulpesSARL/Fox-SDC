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
    public partial class frmAskText : FForm
    {
        public string RetText;

        string Title;
        string BodyText;
        string TextFill;
        public frmAskText(string Title, string BodyText, string TextFill)
        {
            this.Title = Title;
            this.BodyText = BodyText;
            this.TextFill = TextFill;
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            RetText = txtText.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmAskText_Load(object sender, EventArgs e)
        {
            this.Text = Title;
            lblText.Text = BodyText;
            txtText.Text = TextFill;
        }
    }
}
