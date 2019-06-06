using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Common;
using Newtonsoft.Json;
using FoxSDC_MGMT.ReportingMGMT;

namespace FoxSDC_MGMT
{
    public partial class ctlReporting : UserControl, PolicyElementInterface
    {
        PolicyObject Pol;
        ReportingPolicyElement Cert;
        int LastType = -1;
        class ReportingPolElement
        {
            public string Element;
            public string Explain;
            public override string ToString()
            {
                return (Explain);
            }
        }

        CheckState C(bool? b)
        {
            switch (b)
            {
                case true:
                    return (CheckState.Checked);
                case false:
                    return (CheckState.Unchecked);
                default:
                    return (CheckState.Unchecked);
            }
        }

        bool? C(CheckState b)
        {
            switch (b)
            {
                case CheckState.Checked:
                    return (true);
                case CheckState.Unchecked:
                    return (false);
                default:
                    return (null);
            }
        }

        string Explain(string e)
        {
            string res = "Missing description: " + e;
            switch (lstType.SelectedIndex)
            {
                case 1:
                    {
                        ReportingPolicyElementDisk d = JsonConvert.DeserializeObject<ReportingPolicyElementDisk>(e);
                        res = (d.DriveLetter == "$" ? "(SYSTEM DRIVE)" : d.DriveLetter + ":\\") + " - Size < " + d.MinimumSize.ToString() + " " + (d.Method == 1 ? "%" : "bytes");
                        break;
                    }
                case 2:
                    {
                        ReportingPolicyElementEventLog d = JsonConvert.DeserializeObject<ReportingPolicyElementEventLog>(e);
                        res = "";
                        if (d.Book == null)
                            d.Book = new List<string>();
                        if (d.Sources == null)
                            d.Sources = new List<string>();
                        if (d.CategoryNumbers == null)
                            d.CategoryNumbers = new List<int>();
                        if (d.EventLogTypes == null)
                            d.EventLogTypes = new List<int>();

                        if (d.Book.Count == 0)
                        {
                            res += "Book: (all)";
                        }
                        else
                        {
                            res += "Book: ";
                            foreach (string s in d.Book)
                            {
                                res += s + ", ";
                            }
                            if (res.EndsWith(", ") == true)
                                res = res.Substring(0, res.Length - 2);
                        }

                        if (d.Sources.Count == 0)
                        {
                            res += " Sources: (any)";
                        }
                        else
                        {
                            res += " Sources: ";
                            foreach (string s in d.Sources)
                            {
                                res += s + ", ";
                            }
                            if (res.EndsWith(", ") == true)
                                res = res.Substring(0, res.Length - 2);
                        }

                        if (d.EventLogTypes.Count == 0)
                        {
                            res += " Types: (any)";
                        }
                        else
                        {
                            bool GotInfo = false;
                            res += " Types: ";
                            foreach (int s in d.EventLogTypes)
                            {
                                switch (s)
                                {
                                    case 0:
                                    case 4:
                                        res += GotInfo == false ? " Information, " : ""; GotInfo = true; break;
                                    case 1:
                                        res += " Error, "; break;
                                    case 2:
                                        res += " Warning, "; break;
                                    case 8:
                                        res += " Success Audit, "; break;
                                    case 16:
                                        res += " Failure Audit, "; break;
                                    default:
                                        res += " ??? " + d.ToString() + ", "; break;
                                }
                            }
                            if (res.EndsWith(", ") == true)
                                res = res.Substring(0, res.Length - 2);
                        }

                        if (d.CategoryNumbers.Count == 0)
                        {
                            res += " Event IDs: (any)";
                        }
                        else
                        {
                            res += " Event IDs: ";
                            foreach (int s in d.CategoryNumbers)
                            {
                                res += s.ToString() + ", ";
                            }
                            if (res.EndsWith(", ") == true)
                                res = res.Substring(0, res.Length - 2);
                        }
                        break;
                    }
                case 3:
                    {
                        ReportingPolicyElementAddRemovePrograms d = JsonConvert.DeserializeObject<ReportingPolicyElementAddRemovePrograms>(e);
                        if (d.Names == null)
                            d.Names = new List<string>();
                        res = "";
                        switch (d.SearchNameIn)
                        {
                            case 0: res += "Programs with their exact IDs: "; break;
                            case 1: res += "Programs containing: "; break;
                            case 2: res += "Programs starting with: "; break;
                            default: res += "????? " + d.SearchNameIn.ToString() + ": "; break;
                        }
                        if (d.Names.Count == 0)
                        {
                            res += "(any)";
                        }
                        else
                        {
                            foreach (string s in d.Names)
                            {
                                res += "\"" + s + "\", ";
                            }
                            if (res.EndsWith(", ") == true)
                                res = res.Substring(0, res.Length - 2);
                        }
                        res += " ";
                        switch (d.SearchBits)
                        {
                            case 0: break;
                            case 1: res += "(32 bit only)"; break;
                            case 2: res += "(64 bit only)"; break;
                        }

                        res += " Notify on: ";
                        if (d.NotifyOnAdd == true)
                            res += "Add, ";
                        if (d.NotifyOnRemove == true)
                            res += "Removal, ";
                        if (d.NotifyOnUpdate == true)
                            res += "Update, ";
                        if (res.EndsWith(", ") == true)
                            res = res.Substring(0, res.Length - 2);
                        break;
                    }
                case 4:
                    {
                        ReportingPolicyElementStartup d = JsonConvert.DeserializeObject<ReportingPolicyElementStartup>(e);
                        if (d.Names == null)
                            d.Names = new List<string>();
                        res = "";
                        switch (d.SearchNameIn)
                        {
                            case 0: res += "Startup Item with their exact text: "; break;
                            case 1: res += "Startup Item containing: "; break;
                            case 2: res += "Startup Item starting with: "; break;
                            default: res += "????? " + d.SearchNameIn.ToString() + ": "; break;
                        }
                        if (d.Names.Count == 0)
                        {
                            res += "(any)";
                        }
                        else
                        {
                            foreach (string s in d.Names)
                            {
                                res += "\"" + s + "\", ";
                            }
                            if (res.EndsWith(", ") == true)
                                res = res.Substring(0, res.Length - 2);
                        }
                        res += " ";

                        if (string.IsNullOrWhiteSpace(d.SearchLocations) == false)
                        {
                            res += " Locations: " + d.SearchLocations;
                        }

                        res += " Notify on: ";
                        if (d.NotifyOnAdd == true)
                            res += "Add, ";
                        if (d.NotifyOnRemove == true)
                            res += "Removal, ";
                        if (d.NotifyOnUpdate == true)
                            res += "Update, ";
                        if (res.EndsWith(", ") == true)
                            res = res.Substring(0, res.Length - 2);
                        break;
                    }
                case 5:
                    {
                        ReportingPolicyElementSMART d = JsonConvert.DeserializeObject<ReportingPolicyElementSMART>(e);
                        res = "";

                        res += " Notify on: ";
                        if (d.NotifyOnAdd == true)
                            res += "Add, ";
                        if (d.NotifyOnRemove == true)
                            res += "Removal, ";
                        if (d.NotifyOnUpdate == true)
                        {
                            res += "Update";
                            if (d.SkipAttribUpdateReport != null)
                            {
                                if (d.SkipAttribUpdateReport.Count > 0)
                                {
                                    res += " (Skip Attributes: ";
                                    foreach (int s in d.SkipAttribUpdateReport)
                                    {
                                        res += "0x" + s.ToString("X") + ", ";
                                    }
                                    if (res.EndsWith(", ") == true)
                                        res = res.Substring(0, res.Length - 2);
                                    res += "), ";
                                }
                                else
                                {
                                    res += ", ";
                                }
                            }
                            else
                            {
                                res += ", ";
                            }
                        }
                        if (d.NotifyOnError == true)
                            res += "Error, ";
                        if (res.EndsWith(", ") == true)
                            res = res.Substring(0, res.Length - 2);
                        break;
                    }
                case 6:
                    {
                        res = "Simple Task is completed";
                        break;
                    }
            }
            return (res);
        }

        void UpdateStatus()
        {
            if (lstType.Items.Count == 0)
            {
                lstType.Items.Add("None");
                lstType.Items.Add("Disk Space");
                lstType.Items.Add("Event Log");
                lstType.Items.Add("Add/Remove Programs");
                lstType.Items.Add("Startup Elements");
                lstType.Items.Add("SMART Reporting");
                lstType.Items.Add("Simple Tasks completed");
            }

            lstType.SelectedIndex = (int)Cert.Type;
            chkReportToAdmin.CheckState = C(Cert.ReportToAdmin);
            chkReportToClient.CheckState = C(Cert.ReportToClient);
            chkUrgentReportAdmin.CheckState = C(Cert.UrgentForAdmin);
            chkUrgentReportToClient.CheckState = C(Cert.UrgentForClient);

            lstItems.Items.Clear();
            foreach (string e in Cert.ReportingElements)
            {
                ReportingPolElement r = new ReportingPolElement();
                r.Element = e;
                r.Explain = Explain(e);
                lstItems.Items.Add(r);
            }
        }

        public ctlReporting()
        {
            InitializeComponent();
        }

        public string GetData()
        {
            Cert.Type = (ReportingPolicyType)lstType.SelectedIndex;
            Cert.ReportToAdmin = C(chkReportToAdmin.CheckState);
            Cert.ReportToClient = C(chkReportToClient.CheckState);
            Cert.UrgentForAdmin = C(chkUrgentReportAdmin.CheckState);
            Cert.UrgentForClient = C(chkUrgentReportToClient.CheckState);
            return (JsonConvert.SerializeObject(Cert));
        }

        public bool SetData(PolicyObject obj)
        {
            Pol = obj;

            Cert = JsonConvert.DeserializeObject<ReportingPolicyElement>(obj.Data);
            if (Cert == null)
                Cert = new ReportingPolicyElement();

            if (Cert.ReportingElements == null)
                Cert.ReportingElements = new List<string>();
            UpdateStatus();
            return (true);
        }

        private void ctlReporting_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;
            UpdateStatus();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string data = GetData();
            Program.net.EditPolicy(Pol.ID, data);
        }

        private void cmdAddItem_Click(object sender, EventArgs e)
        {
            if (LastType == -1)
                return;
            switch (LastType)
            {
                case 1:
                    {
                        frmDiskSpace d = new frmDiskSpace(null);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements.Add(JsonConvert.SerializeObject(d.Element));
                        UpdateStatus();
                    }
                    break;
                case 2:
                    {
                        frmEventLog d = new frmEventLog(null);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements.Add(JsonConvert.SerializeObject(d.Element));
                        UpdateStatus();
                    }
                    break;
                case 3:
                    {
                        frmAddRemovePrograms d = new frmAddRemovePrograms(null);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements.Add(JsonConvert.SerializeObject(d.Element));
                        UpdateStatus();
                    }
                    break;
                case 4:
                    {
                        frmStartup d = new frmStartup(null);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements.Add(JsonConvert.SerializeObject(d.Element));
                        UpdateStatus();
                    }
                    break;
                case 5:
                    {
                        frmSMARTConfig d = new frmSMARTConfig(null);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements.Add(JsonConvert.SerializeObject(d.Element));
                        UpdateStatus();
                    }
                    break;
                case 6:
                    {
                        Cert.ReportingElements.Add(JsonConvert.SerializeObject(new ReportingPolicyElementSimpleTaskCompleted()));
                        UpdateStatus();
                    }
                    break;
            }
        }

        private void cmdDeleteItem_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show(this, "Do you want to delete the " + lstItems.SelectedItems.Count.ToString() + " item" + (lstItems.SelectedItems.Count == 1 ? "" : "s") + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
            foreach (ReportingPolElement r in lstItems.SelectedItems)
            {
                Cert.ReportingElements.Remove(r.Element);
            }
            UpdateStatus();
        }

        private void lstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LastType == -1)
            {
                LastType = lstType.SelectedIndex;
                return;
            }
            if (lstType.SelectedIndex != LastType)
            {
                if (lstItems.Items.Count > 0)
                {
                    if (MessageBox.Show(this, "Warning: the list will be deleted when you change the type.\nContinue?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                    {
                        lstType.SelectedIndex = LastType;
                        return;
                    }
                    lstItems.Items.Clear();
                    Cert.ReportingElements.Clear();
                }
                LastType = lstType.SelectedIndex;
                Cert.Type = (ReportingPolicyType)LastType;
            }
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItems.Count == 0)
                return;
            if (lstItems.SelectedItems.Count > 1)
            {
                MessageBox.Show(this, "You can only edit one item at a time.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ReportingPolElement r = (ReportingPolElement)lstItems.SelectedItem;
            int index = Cert.ReportingElements.IndexOf(r.Element);
            if (index == -1)
            {
                MessageBox.Show(this, "Cannot find the element internally!", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            switch (LastType)
            {
                case 1:
                    {
                        frmDiskSpace d = new frmDiskSpace(r.Element);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements[index] = JsonConvert.SerializeObject(d.Element);
                        UpdateStatus();
                    }
                    break;
                case 2:
                    {
                        frmEventLog d = new frmEventLog(r.Element);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements[index] = JsonConvert.SerializeObject(d.Element);
                        UpdateStatus();
                    }
                    break;
                case 3:
                    {
                        frmAddRemovePrograms d = new frmAddRemovePrograms(r.Element);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements[index] = JsonConvert.SerializeObject(d.Element);
                        UpdateStatus();
                    }
                    break;
                case 4:
                    {
                        frmStartup d = new frmStartup(r.Element);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements[index] = JsonConvert.SerializeObject(d.Element);
                        UpdateStatus();
                    }
                    break;
                case 5:
                    {
                        frmSMARTConfig d = new frmSMARTConfig(r.Element);
                        if (d.ShowDialog(this) != DialogResult.OK)
                            return;
                        Cert.ReportingElements[index] = JsonConvert.SerializeObject(d.Element);
                        UpdateStatus();
                    }
                    break;
                case 6:
                    {
                        Cert.ReportingElements[index] = JsonConvert.SerializeObject(new ReportingPolicyElementSimpleTaskCompleted());
                        UpdateStatus();
                    }
                    break;
            }
        }

        private void lstItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cmdEdit_Click(sender, e);
        }

        private void lstItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                cmdEdit_Click(sender, e);
        }
    }
}
