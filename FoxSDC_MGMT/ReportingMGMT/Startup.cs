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
    public partial class frmStartup : FForm
    {
        public ReportingPolicyElementStartup Element;

        public frmStartup(string Element)
        {
            if (Element == null)
                this.Element = null;
            else
                this.Element = JsonConvert.DeserializeObject<ReportingPolicyElementStartup>(Element);
            InitializeComponent();
        }

        private void frmStartup_Load(object sender, EventArgs e)
        {
            lstCheck.Items.Add("Exact Name");
            lstCheck.Items.Add("Name contains");
            lstCheck.Items.Add("Name starts with");
            lstCheck.SelectedIndex = 0;

            if (Element != null)
            {
                if (Element.Names == null)
                    Element.Names = new List<string>();
                txtPrograms.Text = "";
                foreach (string n in Element.Names)
                {
                    txtPrograms.Text += n + "\r\n";
                }

                txtLocations.Text = "";
                foreach (string n in Element.SearchLocations.Split(';'))
                {
                    if (string.IsNullOrWhiteSpace(n) == true)
                        continue;
                    txtLocations.Text += n + "\r\n";
                }
                lstCheck.SelectedIndex = Element.SearchNameIn;
                chkNotifyOnAdd.Checked = Element.NotifyOnAdd;
                chkNotifyOnRemove.Checked = Element.NotifyOnRemove;
                chkNotifyOnUpdate.Checked = Element.NotifyOnUpdate;
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Element = new ReportingPolicyElementStartup();
            Element.Names = new List<string>();
            foreach (string s in txtPrograms.Text.Split('\n', '\r'))
            {
                if (s.Trim() == "")
                    continue;
                Element.Names.Add(s.Trim());
            }

            Element.SearchLocations = "";
            foreach (string s in txtLocations.Text.Split('\n', '\r'))
            {
                if (s.Trim() == "")
                    continue;
                Element.SearchLocations += s.Trim() + ";";
            }
            if (Element.SearchLocations.EndsWith(";") == true)
                Element.SearchLocations = Element.SearchLocations.Substring(0, Element.SearchLocations.Length - 1);

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

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
