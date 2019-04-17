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
using System.Threading;

namespace FoxSDC_MGMT
{
    public partial class ctlEventLogs : UserControl
    {
        Dictionary<string, string> Computernames = new Dictionary<string, string>();
        string MachineID = null;
        bool SourcesLoaded = false;
        string GetComputerName(string MachineID)
        {
            if (Computernames.ContainsKey(MachineID.ToLower()) == true)
                return (Computernames[MachineID.ToLower()]);
            ComputerData d = Program.net.GetComputerDetail(MachineID);
            Computernames.Add(MachineID.ToLower(), d.Computername);
            return (d.Computername);
        }

        public ctlEventLogs()
        {
            InitializeComponent();
        }

        public ctlEventLogs(string machineid)
        {
            MachineID = machineid;
            InitializeComponent();
        }

        private void chkDTFrom_CheckedChanged(object sender, EventArgs e)
        {
            DTFrom.Enabled = chkDTFrom.Checked;
        }

        private void chkDTTo_CheckedChanged(object sender, EventArgs e)
        {
            DTTo.Enabled = chkDTTo.Checked;
        }

        private void ctlEventLogs_Load(object sender, EventArgs e)
        {
            if (MachineID == null)
            {
                lblComputer.Text = "(any)";
            }
            else
            {
                lblComputer.Text = GetComputerName(MachineID);
            }

            DTFrom.Enabled = chkDTFrom.Checked;
            DTTo.Enabled = chkDTTo.Checked;

            lstBook.Items.Add("(any)");
            lstBook.Items.Add("Application");
            lstBook.Items.Add("Security");
            lstBook.Items.Add("System");
            lstBook.SelectedIndex = 0;

            lstStatus.Items.Add("(any)");
            lstStatus.Items.Add("Error");
            lstStatus.Items.Add("Warning");
            lstStatus.Items.Add("Information");
            lstStatus.Items.Add("Success Audit");
            lstStatus.Items.Add("Failure Audit");
            lstStatus.SelectedIndex = 0;

            DTTo.CustomFormat = DTFrom.CustomFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            DTTo.Format = DTFrom.Format = DateTimePickerFormat.Custom;

            lblResults.Text = "Nothing";

            FixView();
        }

        public void FixView()
        {
            splitContainer1.SplitterDistance = lblResults.Top + lblResults.Height + 10;
        }

        private void cmdQuery_Click(object sender, EventArgs e)
        {
            EventLogSearch ls = new EventLogSearch();
            if (lstBook.Text.Trim() != "" && lstBook.Text != "(any)")
                ls.EventLogBook = lstBook.Text.Trim();
            if (lstSources.Text.Trim() != "")
                ls.Source = lstSources.Text.Trim();
            if (DTTo.Enabled == true)
                ls.ToDate = DTTo.Value;
            if (DTFrom.Enabled == true)
                ls.FromDate = DTFrom.Value;
            if (lstStatus.SelectedIndex > 0)
                ls.EventLogType = 1 << (lstStatus.SelectedIndex - 1);
            if (txtEventID.Text.Trim() != "")
            {
                int i = 0;
                if (int.TryParse(txtEventID.Text, out i) == true)
                    ls.CategoryNumber = i;
            }
            ls.MachineID = MachineID;
            int.TryParse(txtQTY.Text, out ls.QTY);
            if (ls.QTY < 1)
                ls.QTY = 1000;
            txtQTY.Text = ls.QTY.ToString();

            List<EventLogReportFull> evl = Program.net.GetEventLogs(ls);
            lstData.Items.Clear();

            if (evl != null)
            {
                foreach (EventLogReportFull ev in evl)
                {
                    int imgindex = 0;
                    string Text = "";
                    switch (ev.EventLogType)
                    {
                        case 1:
                            imgindex = 3;
                            Text = "Error";
                            break;
                        case 2:
                            imgindex = 2;
                            Text = "Warning";
                            break;
                        case 0:
                        case 4:
                            imgindex = 4;
                            Text = "Information";
                            break;
                        case 8:
                            imgindex = 0;
                            Text = "Success Audit";
                            break;
                        case 16:
                            imgindex = 1;
                            Text = "Failure Audit";
                            break;
                        default:
                            imgindex = 4;
                            Text = "?";
                            break;
                    }
                    ListViewItem i = new ListViewItem(Text, imgindex);
                    i.SubItems.Add(ev.TimeGenerated.ToLongDateString() + " " + ev.TimeGenerated.ToLongTimeString());
                    i.SubItems.Add(GetComputerName(ev.MachineID));
                    i.SubItems.Add(ev.EventLog);
                    i.SubItems.Add(ev.Source);
                    i.SubItems.Add(ev.CategoryNumber.ToString());
                    i.Tag = ev;
                    lstData.Items.Add(i);
                }
            }

            if (lstData.Items.Count == 0)
                if (evl == null)
                    lblResults.Text = "Nothing";
                else
                    lblResults.Text = lstData.Items.Count.ToString() + " item" + (lstData.Items.Count == 1 ? "" : "s");
            else
                lblResults.Text = lstData.Items.Count.ToString() + " item" + (lstData.Items.Count == 1 ? "" : "s");
        }

        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstData.SelectedItems.Count == 0)
            {
                txtEventLogText.Text = "";
                return;
            }

            ListViewItem i = lstData.SelectedItems[0];
            EventLogReportFull ev = (EventLogReportFull)i.Tag;
            string Text = "";
            switch (ev.EventLogType)
            {
                case 1:
                    Text = "Error";
                    break;
                case 2:
                    Text = "Warning";
                    break;
                case 4:
                    Text = "Information";
                    break;
                case 8:
                    Text = "Success Audit";
                    break;
                case 16:
                    Text = "Failure Audit";
                    break;
                default:
                    Text = ev.EventLogType.ToString();
                    break;
            }

            txtEventLogText.Text = ev.Message + "\r\n==============\r\n" +
                "Computer: " + GetComputerName(ev.MachineID) + "\r\n" +
                "Date: " + ev.TimeGenerated.ToLongDateString() + " " + ev.TimeGenerated.ToLongTimeString() + "\r\n" +
                "Book: " + ev.EventLog + "\r\n" +
                "Source: " + ev.Source + "\r\n" +
                "Event ID: " + ev.CategoryNumber + "\r\n" +
                "Type: " + Text + "\r\n" +
                "DBID: " + ev.LogID + "\r\n" +
                "Data: " + BitConverter.ToString(ev.Data).Replace("-", "") + "\r\n";
        }

        private void lstSources_DropDown(object sender, EventArgs e)
        {
            if (SourcesLoaded == false)
            {
                List<string> sources = Program.net.GetEventSources();
                if (sources != null)
                {
                    foreach (string src in sources)
                    {
                        lstSources.Items.Add(src);
                    }
                }
                SourcesLoaded = true;
            }
        }
    }
}
