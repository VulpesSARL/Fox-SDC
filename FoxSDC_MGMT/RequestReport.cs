using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmRequestReport : FForm
    {
        class KVP
        {
            public string K;
            public object V;

            public KVP(string K, object V)
            {
                this.K = K;
                this.V = V;
            }

            public override string ToString()
            {
                return (K);
            }
        }

        public frmRequestReport()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkFrom_CheckedChanged(object sender, EventArgs e)
        {
            DTFrom.Enabled = chkFrom.Checked;
        }

        private void chkTo_CheckedChanged(object sender, EventArgs e)
        {
            DTTo.Enabled = chkTo.Checked;
        }

        private void frmRequestReport_Load(object sender, EventArgs e)
        {
            DTFrom.Enabled = chkFrom.Checked;
            DTTo.Enabled = chkTo.Checked;

            DTTo.CustomFormat = DTFrom.CustomFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            DTTo.Format = DTFrom.Format = DateTimePickerFormat.Custom;

            lstMachines.Items.Add("(None)");
            lstContract.Items.Add("(None)");

            List<ComputerData> cdd = Program.net.GetComputerList(null, null);
            if (cdd == null)
            {
                MessageBox.Show(this, "Error requesting data: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (ComputerData cd in cdd)
            {
                lstMachines.Items.Add(new KVP(cd.Computername + " [" + cd.GroupingPath + "]", cd.MachineID));
            }

            List<ContractInfos> c = Program.net.GetContractInfos();
            if (c == null)
            {
                MessageBox.Show(this, "Error requesting data: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (ContractInfos cc in c)
            {
                lstContract.Items.Add(new KVP(cc.ContractID, cc));
            }

            lstMachines.SelectedIndex = 0;
            lstContract.SelectedIndex = 0;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (lstMachines.SelectedIndex == 0 && lstContract.SelectedIndex == 0)
            {
                MessageBox.Show(this, "Please select one option.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (lstMachines.SelectedIndex != 0 && lstContract.SelectedIndex != 0)
            {
                MessageBox.Show(this, "Please select only one option.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<string> Machines = new List<string>();

            if (lstMachines.SelectedIndex > 0)
            {
                Machines.Add((string)((KVP)lstMachines.SelectedItem).V);
            }
            if (lstContract.SelectedIndex > 0)
            {
                ContractInfos c = (ContractInfos)((KVP)lstContract.SelectedItem).V;
                if (c.IncludedComputers != null)
                {
                    foreach (ComputerData cd in c.IncludedComputers)
                    {
                        Machines.Add(cd.MachineID);
                    }
                }
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save PDF Report";
            save.CheckPathExists = true;
            save.OverwritePrompt = true;
            save.Filter = "PDF Files|*.pdf";
            if (save.ShowDialog(this) != DialogResult.OK)
                return;

            byte[] pdf = Program.net.PaperGetMachineReport(Machines, chkFrom.Checked == false ? (DateTime?)null : DTFrom.Value, chkTo.Checked == false ? (DateTime?)null : DTTo.Value);
            if (pdf == null)
            {
                MessageBox.Show(this, "Error requesting report: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                File.WriteAllBytes(save.FileName, pdf);
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Error saving report: " + ee.Message, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            this.Close();
        }

        private void cmdLM_Click(object sender, EventArgs e)
        {
            DateTime p = DateTime.Today.AddMonths(-1);
            DTFrom.Value = new DateTime(p.Year, p.Month, 1, 0, 0, 0, 0);
            DTTo.Value = new DateTime(p.Year, p.Month, DateTime.DaysInMonth(p.Year, p.Month), 23, 59, 59);
            chkFrom.Checked = chkTo.Checked = true;
        }

        private void cmdTM_Click(object sender, EventArgs e)
        {
            DateTime p = DateTime.Today;
            DTFrom.Value = new DateTime(p.Year, p.Month, 1, 0, 0, 0, 0);
            DTTo.Value = new DateTime(p.Year, p.Month, DateTime.DaysInMonth(p.Year, p.Month), 23, 59, 59);
            chkFrom.Checked = chkTo.Checked = true;
        }
    }
}
