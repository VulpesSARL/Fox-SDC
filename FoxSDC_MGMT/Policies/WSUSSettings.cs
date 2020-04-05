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

namespace FoxSDC_MGMT.Policies
{
    /*
    Dual Scan Documentation: https://blogs.technet.microsoft.com/swisspfe/2018/04/13/win10-updates-store-gpos-dualscandisabled-sup-wsus/
    */
    public partial class ctlWSUSSettings : UserControl, PolicyElementInterface
    {
        PolicyObject Pol;
        WSUSPolicy WSUS;

        CheckState C(bool? b)
        {
            switch (b)
            {
                case true:
                    return (CheckState.Checked);
                case false:
                    return (CheckState.Unchecked);
                default:
                    return (CheckState.Indeterminate);
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

        public ctlWSUSSettings()
        {
            InitializeComponent();
        }

        void UpdateStatus()
        {
            lstActiveHours1.Items.Clear();
            lstActiveHours2.Items.Clear();
            lstSchedInstHour.Items.Clear();
            lstWUOptions.Items.Clear();
            lstSchedInstDay.Items.Clear();
            lstDownloadMode.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                lstActiveHours1.Items.Add(i.ToString() + ":00");
                lstActiveHours2.Items.Add(i.ToString() + ":00");
                lstSchedInstHour.Items.Add(i.ToString() + ":00");
            }
            lstWUOptions.Items.Add("Notify for download and auto install");//2
            lstWUOptions.Items.Add("Auto download and notify for install");//3
            lstWUOptions.Items.Add("Auto download and schedule the install");//4
            lstWUOptions.Items.Add("Allow local admin to choose setting");//5
            lstSchedInstDay.Items.Add("Everyday");
            lstSchedInstDay.Items.Add("Sunday");
            lstSchedInstDay.Items.Add("Monday");
            lstSchedInstDay.Items.Add("Tuesday");
            lstSchedInstDay.Items.Add("Wednesday");
            lstSchedInstDay.Items.Add("Thursday");
            lstSchedInstDay.Items.Add("Friday");
            lstSchedInstDay.Items.Add("Saturday");
            lstDownloadMode.Items.Add("HTTP only, no peering");
            lstDownloadMode.Items.Add("HTTP blended with peering behind the same NAT");
            lstDownloadMode.Items.Add("HTTP blended with peering across a private group");
            lstDownloadMode.Items.Add("HTTP blended with Internet Peering");
            lstDownloadMode.Items.Add("Simple download mode with no peering");
            lstDownloadMode.Items.Add("Bypass");
            chkAlwaysAutoRestart.CheckState = C(WSUS.SpecifyAlwaysAutoRestart);
            chkClientSideTargeting.CheckState = C(WSUS.SpecifyClientSideTargeting);
            chkConfWU.CheckState = C(WSUS.ConfigureWSUS);
            chkDeadline.CheckState = C(WSUS.SpecifyDeadline);
            chkDetectionFreq.CheckState = C(WSUS.SpecifyDetectionFreq);
            chkDoNotAutoRestartDuringWorkHours.CheckState = C(WSUS.DontAutoRestartDuringActiveHours);
            chkDontRestart.CheckState = C(WSUS.DontAutoRestart);
            chkNoMSServer.CheckState = C(WSUS.NoMSServer);
            chkSpecWUServer.CheckState = C(WSUS.SpecifyWUServer);
            chkWUStatServer.CheckState = C(WSUS.SpecifyStatusServer);
            chkInstallDuringMaintenance.CheckState = C(WSUS.InstallDuringMaintenance);
            chkScheduleInstall.CheckState = C(WSUS.SpecifyScheduleInstall);
            chkMicrosoftUpdate.CheckState = C(WSUS.InstallMicrosoftUpdates);
            chkSpecUpdateMethod.CheckState = C(WSUS.SpecifyWUOptions);
            chkDisableDualScan.CheckState = C(WSUS.DisableDualScan);
            chkDownloadMode.CheckState = C(WSUS.EnableDownloadMode);
            lstSchedInstDay.SelectedIndex = WSUS.ScheduleInstallDay;
            lstSchedInstHour.SelectedIndex = WSUS.ScheduleInstallHour;
            txtClientTarget.Text = WSUS.Target;
            txtDeadline.Text = WSUS.DeadLine.ToString();
            txtDetectionFreq.Text = WSUS.DetectionFreq.ToString();
            txtAutoRestartMin.Text = WSUS.AlwaysAutoRestartDelay.ToString();
            txtStatusServer.Text = WSUS.StatusServer;
            txtWUServer.Text = WSUS.WUServer;
            lstActiveHours1.SelectedIndex = WSUS.ActiveHoursFrom;
            lstActiveHours2.SelectedIndex = WSUS.ActiveHoursTo;
            if (WSUS.WUOptions - 2 >= 0)
                lstWUOptions.SelectedIndex = WSUS.WUOptions - 2;
            else
                lstWUOptions.SelectedIndex = 0;

            switch (WSUS.DownloadMode)
            {
                case 0: lstDownloadMode.SelectedIndex = 0; break;
                case 1: lstDownloadMode.SelectedIndex = 1; break;
                case 2: lstDownloadMode.SelectedIndex = 2; break;
                case 3: lstDownloadMode.SelectedIndex = 3; break;
                case 99: lstDownloadMode.SelectedIndex = 4; break;
                case 100: lstDownloadMode.SelectedIndex = 5; break;
                default: lstDownloadMode.SelectedIndex = 0; break;
            }

            chkSpecWUServer_CheckedChanged(null, null);
            chkWUStatServer_CheckedChanged(null, null);
            chkClientSideTargeting_CheckedChanged(null, null);
            chkDetectionFreq_CheckedChanged(null, null);
            chkDeadline_CheckedChanged(null, null);
            chkActiveHours_CheckedChanged(null, null);
            chkConfWU_CheckedChanged(null, null);
            chkScheduleInstall_CheckStateChanged(null, null);
            chkSpecUpdateMethod_CheckStateChanged(null, null);
            chkAlwaysAutoRestart_CheckStateChanged(null, null);
            chkDownloadMode_CheckedChanged(null, null);
        }

        public string GetData()
        {
            WSUS.SpecifyAlwaysAutoRestart = C(chkAlwaysAutoRestart.CheckState);
            WSUS.SpecifyClientSideTargeting = C(chkClientSideTargeting.CheckState);
            WSUS.ConfigureWSUS = C(chkConfWU.CheckState);
            WSUS.SpecifyDeadline = C(chkDeadline.CheckState);
            WSUS.SpecifyDetectionFreq = C(chkDetectionFreq.CheckState);
            WSUS.DontAutoRestartDuringActiveHours = C(chkDoNotAutoRestartDuringWorkHours.CheckState);
            WSUS.DontAutoRestart = C(chkDontRestart.CheckState);
            WSUS.NoMSServer = C(chkNoMSServer.CheckState);
            WSUS.SpecifyWUServer = C(chkSpecWUServer.CheckState);
            WSUS.SpecifyStatusServer = C(chkWUStatServer.CheckState);
            WSUS.InstallDuringMaintenance = C(chkInstallDuringMaintenance.CheckState);
            WSUS.SpecifyScheduleInstall = C(chkScheduleInstall.CheckState);
            WSUS.InstallMicrosoftUpdates = C(chkMicrosoftUpdate.CheckState);
            WSUS.SpecifyWUOptions = C(chkSpecUpdateMethod.CheckState);
            WSUS.EnableDownloadMode = C(chkDownloadMode.CheckState);
            WSUS.DisableDualScan = C(chkDisableDualScan.CheckState);
            WSUS.ScheduleInstallDay = lstSchedInstDay.SelectedIndex;
            WSUS.ScheduleInstallHour = lstSchedInstHour.SelectedIndex;
            WSUS.Target = txtClientTarget.Text;
            WSUS.StatusServer = txtStatusServer.Text;
            WSUS.WUServer = txtWUServer.Text;
            WSUS.ActiveHoursFrom = lstActiveHours1.SelectedIndex;
            WSUS.ActiveHoursTo = lstActiveHours2.SelectedIndex;
            WSUS.WUOptions = lstWUOptions.SelectedIndex + 2;
            chkDownloadMode.CheckState = C(WSUS.EnableDownloadMode);
            switch (lstDownloadMode.SelectedIndex)
            {
                case 0: WSUS.DownloadMode = 0; break;
                case 1: WSUS.DownloadMode = 1; break;
                case 2: WSUS.DownloadMode = 2; break;
                case 3: WSUS.DownloadMode = 3; break;
                case 4: WSUS.DownloadMode = 99; break;
                case 5: WSUS.DownloadMode = 100; break;
                default: WSUS.DownloadMode = 0; break;
            }
            int.TryParse(txtDeadline.Text, out WSUS.DeadLine);
            int.TryParse(txtDetectionFreq.Text, out WSUS.DetectionFreq);
            int.TryParse(txtAutoRestartMin.Text, out WSUS.AlwaysAutoRestartDelay);
            return (JsonConvert.SerializeObject(WSUS));
        }

        public bool SetData(PolicyObject obj)
        {
            Pol = obj;

            WSUS = JsonConvert.DeserializeObject<WSUSPolicy>(obj.Data);
            if (WSUS == null)
                WSUS = new WSUSPolicy();

            UpdateStatus();
            return (true);
        }

        private void WSUSSettings_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;
            UpdateStatus();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string d = GetData();
            Program.net.EditPolicy(Pol.ID, d);
        }

        private void chkSpecWUServer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSpecWUServer.CheckState != CheckState.Checked)
            {
                chkWUStatServer.Enabled = txtWUServer.Enabled = false;
                chkWUStatServer.CheckState = CheckState.Unchecked;
            }
            else
            {
                chkWUStatServer.Enabled = txtWUServer.Enabled = true;
            }
        }

        private void chkWUStatServer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWUStatServer.CheckState != CheckState.Checked)
                txtStatusServer.Enabled = false;
            else
                txtStatusServer.Enabled = true;
        }

        private void chkClientSideTargeting_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClientSideTargeting.CheckState != CheckState.Checked)
                txtClientTarget.Enabled = false;
            else
                txtClientTarget.Enabled = true;
        }

        private void chkDetectionFreq_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDetectionFreq.CheckState != CheckState.Checked)
                txtDetectionFreq.Enabled = false;
            else
                txtDetectionFreq.Enabled = true;
        }

        private void chkDeadline_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeadline.CheckState != CheckState.Checked)
                txtDeadline.Enabled = false;
            else
                txtDeadline.Enabled = true;
        }

        private void chkActiveHours_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDoNotAutoRestartDuringWorkHours.CheckState != CheckState.Checked)
                lstActiveHours1.Enabled = lstActiveHours2.Enabled = false;
            else
                lstActiveHours1.Enabled = lstActiveHours2.Enabled = true;
        }

        private void chkConfWU_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConfWU.CheckState != CheckState.Unchecked)
                panel1.Enabled = true;
            else
                panel1.Enabled = false;
        }

        private void chkScheduleInstall_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkScheduleInstall.CheckState != CheckState.Checked)
                lstSchedInstDay.Enabled = lstSchedInstHour.Enabled = false;
            else
                lstSchedInstDay.Enabled = lstSchedInstHour.Enabled = true;
        }

        private void chkSpecUpdateMethod_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkSpecUpdateMethod.CheckState != CheckState.Checked)
                lstWUOptions.Enabled = false;
            else
                lstWUOptions.Enabled = true;
        }

        private void chkAlwaysAutoRestart_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkAlwaysAutoRestart.CheckState != CheckState.Checked)
                txtAutoRestartMin.Enabled = false;
            else
                txtAutoRestartMin.Enabled = true;
        }

        private void chkDownloadMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDownloadMode.CheckState != CheckState.Checked)
                lstDownloadMode.Enabled = false;
            else
                lstDownloadMode.Enabled = true;
        }
    }
}
