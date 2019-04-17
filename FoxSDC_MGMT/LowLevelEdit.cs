using FoxSDC_Common;
using Newtonsoft.Json.Linq;
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
    public partial class frmLowLevelEdit : FForm
    {
        PolicyObject Pol;

        public frmLowLevelEdit(PolicyObject Pol)
        {
            this.Pol = Pol;
            InitializeComponent();
        }

        private void frmLowLevelEdit_Load(object sender, EventArgs e)
        {
            this.Text = "Lowlevel edit Policy - " + Pol.Name;
            try
            {
                txtData.Text = JValue.Parse(Pol.Data).ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch
            {
                txtData.Text = "{}";
            }
            txtData.Font = new Font("Courier New", txtData.Font.Size, txtData.Font.Style, txtData.Font.Unit, txtData.Font.GdiCharSet, txtData.Font.GdiVerticalFont);
            txtData.SelectionStart = 0;
            txtData.SelectionLength = 0;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                string s = JValue.Parse(txtData.Text).ToString(Newtonsoft.Json.Formatting.None);
                Program.net.EditPolicy(Pol.ID, s);
            }
            catch
            {
                MessageBox.Show(this, "Invalid JSON data!", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
