using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Configure
{
    public partial class MainDLG : FForm
    {
        public MainDLG()
        {
            InitializeComponent();
        }

        private void MainDLG_Load(object sender, EventArgs e)
        {
            chkUseOnPrem.Checked = RegistryData.UseOnPremServer;
            txtOnPrem.Text = RegistryData.ServerURL;
            txtContractID.Text = RegistryData.ContractID;
            txtPassword.Text = RegistryData.ContractPassword;
            lblStatus.Text = "";
            chkUseOnPrem_CheckedChanged(null, null);

            if (Tricks.IsAdmin() == false)
            {
                Tricks.UACToButton(cmdRestartAsAdmin, true);
                txtContractID.Enabled = txtOnPrem.Enabled = txtPassword.Enabled = false;
                chkUseOnPrem.Enabled = false;
                cmdOK.Enabled = false;
            }
            else
            {
                cmdRestartAsAdmin.Visible = cmdRestartAsAdmin.Enabled = false;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            RegistryData.UseOnPremServer = chkUseOnPrem.Checked;
            RegistryData.ServerURL = txtOnPrem.Text.Trim();
            RegistryData.ContractID = txtContractID.Text.Trim();
            RegistryData.ContractPassword = txtPassword.Text.Trim();

            txtContractID.Enabled = txtOnPrem.Enabled = txtPassword.Enabled = false;
            chkUseOnPrem.Enabled = false;
            cmdCancel.Enabled = cmdOK.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void cmdRestartAsAdmin_Click(object sender, EventArgs e)
        {
            Tricks.RestartAsAdmin();
        }

        delegate void UpdateTextDelegate(string Text);

        void UpdateText(string text)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new UpdateTextDelegate(UpdateText), text);
                return;
            }
            lblStatus.Text = text;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (dir.EndsWith("\\") == false)
                dir += "\\";

            UpdateText("Stopping services");

            ServiceController svc = new ServiceController("FoxSDCA");

            try
            {
                svc.Stop();
            }
            catch
            {

            }

            int i = 0;

            do
            {
                i++;
                if (i > 120 * 4)
                    break;
                svc.Refresh();
                Thread.Sleep(1000);
            } while (svc.Status != ServiceControllerStatus.Stopped);

            #region Kill Processes

            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainModule.FileName.ToLower() == dir.ToLower() + "foxsdc_agent.exe")
                    {
                        proc.Kill();
                    }
                }
                catch
                {

                }
            }

            #endregion

            UpdateText("Starting services");

            svc = new ServiceController("FoxSDCA");

            try
            {
                svc.Start();
            }
            catch
            {

            }
        }

        private void MainDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cmdCancel.Enabled == false)
                e.Cancel = true;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdCancel.Enabled = true;
            this.Close();
        }

        private void chkUseOnPrem_CheckedChanged(object sender, EventArgs e)
        {
            txtOnPrem.Enabled = chkUseOnPrem.Checked;
        }
    }
}
