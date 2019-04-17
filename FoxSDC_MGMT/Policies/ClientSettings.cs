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
    public partial class ctlClientSettings : UserControl, PolicyElementInterface
    {
        ClientSettingsPolicy CliSettings;
        PolicyObject Pol;

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

        public ctlClientSettings()
        {
            InitializeComponent();
        }

        void UpdateStatus()
        {
            chkDisableAddRemoveProgramsSync.CheckState = C(CliSettings.DisableAddRemoveProgramsSync);
            chkDisableEventLogSync.CheckState = C(CliSettings.DisableEventLogSync);
            chkDisableDiskDataSync.CheckState = C(CliSettings.DisableDiskDataSync);
            chkDisableNetadapterSync.CheckState = C(CliSettings.DisableNetadapterSync);
            chkDisableDeviceManagerSync.CheckState = C(CliSettings.DisableDeviceManagerSync);
            chkDisableFilterDriverSync.CheckState = C(CliSettings.DisableFilterDriverSync);
            chkDisableWinLicenseSync.CheckState = C(CliSettings.DisableWinLicenseSync);
            chkEnableBitlockerRKSync.CheckState = C(CliSettings.EnableBitlockerRKSync);
        }

        public string GetData()
        {
            CliSettings.DisableAddRemoveProgramsSync = C(chkDisableAddRemoveProgramsSync.CheckState);
            CliSettings.DisableEventLogSync = C(chkDisableEventLogSync.CheckState);
            CliSettings.DisableDiskDataSync = C(chkDisableDiskDataSync.CheckState);
            CliSettings.DisableNetadapterSync = C(chkDisableNetadapterSync.CheckState);
            CliSettings.DisableDeviceManagerSync = C(chkDisableDeviceManagerSync.CheckState);
            CliSettings.DisableFilterDriverSync = C(chkDisableFilterDriverSync.CheckState);
            CliSettings.DisableWinLicenseSync = C(chkDisableWinLicenseSync.CheckState);
            CliSettings.EnableBitlockerRKSync = C(chkEnableBitlockerRKSync.CheckState);
            return (JsonConvert.SerializeObject(CliSettings));
        }

        public bool SetData(PolicyObject obj)
        {
            Pol = obj;

            CliSettings = JsonConvert.DeserializeObject<ClientSettingsPolicy>(obj.Data);
            if (CliSettings == null)
                CliSettings = new ClientSettingsPolicy();

            UpdateStatus();
            return (true);
        }

        private void ctlClientSettings_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;
            UpdateStatus();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string d = GetData();
            Program.net.EditPolicy(Pol.ID, d);
        }
    }
}
