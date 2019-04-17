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
    public partial class frmAddRemovePrograms : FForm
    {
        public ReportingPolicyElementAddRemovePrograms Element;

        public frmAddRemovePrograms(string Element)
        {
            if (Element == null)
                this.Element = null;
            else
                this.Element = JsonConvert.DeserializeObject<ReportingPolicyElementAddRemovePrograms>(Element);
            InitializeComponent();
        }

        private void frmAddRemovePrograms_Load(object sender, EventArgs e)
        {
            lstCheck.Items.Add("Exact ID");
            lstCheck.Items.Add("Name contains");
            lstCheck.Items.Add("Name starts with");
            lstBits.Items.Add("(any)");
            lstBits.Items.Add("32 Bit");
            lstBits.Items.Add("64 Bit");
            lstBits.SelectedIndex = lstCheck.SelectedIndex = 0;

            if (Element != null)
            {
                if (Element.Names == null)
                    Element.Names = new List<string>();
                txtPrograms.Text = "";
                foreach (string n in Element.Names)
                {
                    txtPrograms.Text += n + "\r\n";
                }
                lstBits.SelectedIndex = Element.SearchBits;
                lstCheck.SelectedIndex = Element.SearchNameIn;
                chkNotifyOnAdd.Checked = Element.NotifyOnAdd;
                chkNotifyOnRemove.Checked = Element.NotifyOnRemove;
                chkNotifyOnUpdate.Checked = Element.NotifyOnUpdate;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Element = new ReportingPolicyElementAddRemovePrograms();
            Element.Names = new List<string>();
            foreach (string s in txtPrograms.Text.Split('\n', '\r'))
            {
                if (s.Trim() == "")
                    continue;
                Element.Names.Add(s.Trim());
            }
            Element.SearchBits = lstBits.SelectedIndex;
            Element.SearchNameIn = lstCheck.SelectedIndex;
            Element.NotifyOnAdd = chkNotifyOnAdd.Checked;
            Element.NotifyOnRemove = chkNotifyOnRemove.Checked;
            Element.NotifyOnUpdate = chkNotifyOnUpdate.Checked;
            if (Element.NotifyOnAdd == false && Element.NotifyOnRemove == false && Element.NotifyOnUpdate == false)
            {
                MessageBox.Show(this, "Please check at least one notification.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
