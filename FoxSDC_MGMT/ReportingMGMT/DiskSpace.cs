using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT.ReportingMGMT
{
    public partial class frmDiskSpace : FForm
    {
        public ReportingPolicyElementDisk Element;

        int LastSZType = -1;

        public frmDiskSpace(string Element)
        {
            if (Element == null)
                this.Element = null;
            else
                this.Element = JsonConvert.DeserializeObject<ReportingPolicyElementDisk>(Element);
            InitializeComponent();
        }

        private void frmDiskSpace_Load(object sender, EventArgs e)
        {
            lstDrive.Items.Add("(SYSTEM Drive)");
            for (char i = 'A'; i <= 'Z'; i++)
            {
                lstDrive.Items.Add(i.ToString());
            }

            lstDiv.Items.Add("B");
            lstDiv.Items.Add("KiB");
            lstDiv.Items.Add("MiB");
            lstDiv.Items.Add("GiB");
            lstDiv.Items.Add("TiB");

            lstMethod.Items.Add("Static size (in bytes)");
            lstMethod.Items.Add("Percentage (in %)");

            lstDrive.SelectedIndex = 0;
            lstMethod.SelectedIndex = 0;
            lstDiv.SelectedIndex = 0;

            if (Element != null)
            {
                lstMethod.SelectedIndex = Element.Method;
                lstDrive.SelectedIndex = Element.DriveLetter[0] == '$' ? 0 : (Convert.ToInt32(Element.DriveLetter.ToUpper()[0]) - Convert.ToInt32('A')) + 1;
                txtSize.Text = Element.MinimumSize.ToString();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Int64 SZ;
            if (Int64.TryParse(txtSize.Text, out SZ) == false)
            {
                MessageBox.Show(this, "Invalid entry", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SZ *= GetDiv();
            Element = new ReportingPolicyElementDisk();
            Element.MinimumSize = SZ;
            Element.DriveLetter = lstDrive.SelectedIndex == 0 ? "$" : Encoding.ASCII.GetString(new byte[] { (byte)('A' + (lstDrive.SelectedIndex - 1)) });
            Element.Method = lstMethod.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lstMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstDiv.Enabled = lstMethod.SelectedIndex == 0 ? true : false;
        }

        Int64 GetDiv(int? S = null)
        {
            switch (S == null ? lstDiv.SelectedIndex : S.Value)
            {
                case 0:
                    return (1);
                case 1:
                    return (1024);
                case 2:
                    return (1048576);
                case 3:
                    return (1073741824);
                case 4:
                    return (1099511627776);
            }
            return (1);
        }

        private void lstDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LastSZType == -1)
            {
                LastSZType = lstDiv.SelectedIndex;
                return;
            }
            Int64 t;
            if (Int64.TryParse(txtSize.Text, out t) == false)
                return;
            decimal ndiv = (decimal)GetDiv() / (decimal)GetDiv(LastSZType);
            t = (Int64)((decimal)t / ndiv);
            txtSize.Text = t.ToString();
            LastSZType = lstDiv.SelectedIndex;
        }
    }
}
