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
    public partial class frmEventLog : FForm
    {
        public ReportingPolicyElementEventLog Element;

        public frmEventLog(string Element)
        {
            if (Element == null)
                this.Element = null;
            else
                this.Element = JsonConvert.DeserializeObject<ReportingPolicyElementEventLog>(Element);
            InitializeComponent();
        }

        private void EventLog_Load(object sender, EventArgs e)
        {
            lstTypes.Items.Add("Information");
            lstTypes.Items.Add("Warning");
            lstTypes.Items.Add("Error");
            lstTypes.Items.Add("Success Audit");
            lstTypes.Items.Add("Failure Audit");

            lstBook.Items.Add("Application");
            lstBook.Items.Add("Security");
            lstBook.Items.Add("System");

            lstInclExcl.Items.Add("None");
            lstInclExcl.Items.Add("Only include with these texts");
            lstInclExcl.Items.Add("Exclude with these texts");
            lstInclExcl.SelectedIndex = 0;

            if (Element != null)
            {
                if (Element.Book != null)
                {
                    string s = "";
                    foreach (string b in Element.Book)
                        s += b.Trim() + ", ";
                    if (s.EndsWith(", ") == true)
                        s = s.Substring(0, s.Length - 2);
                    lstBook.Text = s;
                }
                if (Element.CategoryNumbers != null)
                {
                    string s = "";
                    foreach (int b in Element.CategoryNumbers)
                        s += b.ToString() + ", ";
                    if (s.EndsWith(", ") == true)
                        s = s.Substring(0, s.Length - 2);
                    txtCategories.Text = s;
                }
                if (Element.Sources != null)
                {
                    string s = "";
                    foreach (string b in Element.Sources)
                        s += b.Trim() + ", ";
                    if (s.EndsWith(", ") == true)
                        s = s.Substring(0, s.Length - 2);
                    txtSources.Text = s;
                }
                if (Element.EventLogTypes != null)
                {
                    foreach (int b in Element.EventLogTypes)
                    {
                        switch (b)
                        {
                            case 0:
                            case 4:
                                lstTypes.SetItemChecked(0, true);
                                break;
                            case 1:
                                lstTypes.SetItemChecked(2, true);
                                break;
                            case 2:
                                lstTypes.SetItemChecked(1, true);
                                break;
                            case 8:
                                lstTypes.SetItemChecked(3, true);
                                break;
                            case 16:
                                lstTypes.SetItemChecked(4, true);
                                break;
                        }
                    }
                }
                lstInclExcl.SelectedIndex = Element.IncludeExclude;
                if (Element.IncludeExcludeTexts != null)
                {
                    foreach (string l in Element.IncludeExcludeTexts)
                    {
                        lstTexts.Items.Add(l);
                    }
                }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Element = new ReportingPolicyElementEventLog();
            Element.Book = new List<string>();
            Element.CategoryNumbers = new List<int>();
            Element.EventLogTypes = new List<int>();
            Element.Sources = new List<string>();

            foreach (string s in lstBook.Text.Split(','))
            {
                if (s.Trim() == "")
                    continue;
                Element.Book.Add(s.Trim());
            }

            foreach (string s in txtSources.Text.Split(','))
            {
                if (s.Trim() == "")
                    continue;
                Element.Sources.Add(s.Trim());
            }

            foreach (string s in txtCategories.Text.Split(','))
            {
                if (s.Trim() == "")
                    continue;
                int Cat;
                if (int.TryParse(s, out Cat) == false)
                {
                    MessageBox.Show(this, "Invalid category number.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Element.CategoryNumbers.Add(Cat);
            }

            if (lstTypes.GetItemChecked(0) == true)
            {
                Element.EventLogTypes.Add(0);
                Element.EventLogTypes.Add(4);
            }
            if (lstTypes.GetItemChecked(1) == true)
                Element.EventLogTypes.Add(2);
            if (lstTypes.GetItemChecked(2) == true)
                Element.EventLogTypes.Add(1);
            if (lstTypes.GetItemChecked(3) == true)
                Element.EventLogTypes.Add(8);
            if (lstTypes.GetItemChecked(4) == true)
                Element.EventLogTypes.Add(16);

            Element.IncludeExclude = lstInclExcl.SelectedIndex;
            Element.IncludeExcludeTexts = new List<string>();
            foreach(string Item in lstTexts.Items)
            {
                Element.IncludeExcludeTexts.Add(Item);
            }

            if (Element.Book.Count == 0 && Element.CategoryNumbers.Count == 0 && Element.EventLogTypes.Count == 0 && Element.Sources.Count == 0)
            {
                MessageBox.Show(this, "Event Log Filter cannot be empty.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void cmdInsert_Click(object sender, EventArgs e)
        {
            frmAskText frm = new frmAskText("Add Event Log Text filter", "Add text to Event Log filter", "");
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            if (string.IsNullOrWhiteSpace(frm.RetText) == false)
                lstTexts.Items.Add(frm.RetText);
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (lstTexts.SelectedIndex == -1)
                return;

            frmAskText frm = new frmAskText("Add Event Log Text filter", "Add text to Event Log filter", lstTexts.Items[lstTexts.SelectedIndex].ToString());
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            if (string.IsNullOrWhiteSpace(frm.RetText) == false)
                lstTexts.Items[lstTexts.SelectedIndex] = frm.RetText;
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (lstTexts.SelectedIndex == -1)
                return;

            lstTexts.Items.RemoveAt(lstTexts.SelectedIndex);
        }
    }
}
