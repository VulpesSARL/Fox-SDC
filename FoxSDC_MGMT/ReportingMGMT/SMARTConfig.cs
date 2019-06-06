using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT.ReportingMGMT
{
    public partial class frmSMARTConfig : FForm
    {
        public ReportingPolicyElementSMART Element;

        public frmSMARTConfig(string Element)
        {
            if (Element == null)
                this.Element = null;
            else
                this.Element = JsonConvert.DeserializeObject<ReportingPolicyElementSMART>(Element);
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Element = new ReportingPolicyElementSMART();
            Element.SkipAttribUpdateReport = new List<int>();
            Element.NotifyOnAdd = chkNotifyOnAdd.Checked;
            Element.NotifyOnRemove = chkNotifyOnRemove.Checked;
            Element.NotifyOnUpdate = chkNotifyOnUpdate.Checked;
            Element.NotifyOnError = chkNotifyError.Checked;

            foreach (string s in txtSkipAttrib.Text.Split(','))
            {
                if (s.Trim() == "")
                    continue;
                int Cat;
                if (int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out Cat) == false)
                {
                    MessageBox.Show(this, "Invalid Attribute.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Element.SkipAttribUpdateReport.Add(Cat);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmSMARTConfig_Load(object sender, EventArgs e)
        {
            if (Element != null)
            {
                chkNotifyOnAdd.Checked = Element.NotifyOnAdd;
                chkNotifyOnRemove.Checked = Element.NotifyOnRemove;
                chkNotifyOnUpdate.Checked = Element.NotifyOnUpdate;
                chkNotifyError.Checked = Element.NotifyOnError;
            }
            txtSkipAttrib.Enabled = chkNotifyOnUpdate.Checked;

            if (Element.SkipAttribUpdateReport != null)
            {
                string s = "";
                foreach (int b in Element.SkipAttribUpdateReport)
                    s += b.ToString("X") + ", ";
                if (s.EndsWith(", ") == true)
                    s = s.Substring(0, s.Length - 2);
                txtSkipAttrib.Text = s;
            }
        }

        private void chkNotifyOnUpdate_CheckedChanged(object sender, EventArgs e)
        {
            txtSkipAttrib.Enabled = chkNotifyOnUpdate.Checked;
        }
    }
}
