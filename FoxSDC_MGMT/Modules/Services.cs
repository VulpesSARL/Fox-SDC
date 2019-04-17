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

namespace FoxSDC_MGMT
{
    public partial class ctlServices : UserControl
    {
        delegate void DListServices(List<PushServicesInfo> lst);

        string GetServiceError(Int64 ErrorCode)
        {
            string Err = "";
            switch (ErrorCode)
            {
                case 0: Err = "The request was accepted."; break;
                case 1: Err = "The request is not supported."; break;
                case 2: Err = "The user did not have the necessary access."; break;
                case 3: Err = "The service cannot be stopped because other services that are running are dependent on it."; break;
                case 4: Err = "The requested control code is not valid, or it is unacceptable to the service."; break;
                case 5: Err = "The requested control code cannot be sent to the service because the state of the service(Win32_BaseService: State) is equal to 0, 1, or 2."; break;
                case 6: Err = "The service has not been started."; break;
                case 7: Err = "The service did not respond to the start request in a timely fashion."; break;
                case 8: Err = "Unknown failure when starting the service."; break;
                case 9: Err = "The directory path to the service executable was not found."; break;
                case 10: Err = "The service is already running."; break;
                case 11: Err = "The database to add a new service is locked."; break;
                case 12: Err = "A dependency for which this service relies on has been removed from the system."; break;
                case 13: Err = "The service failed to find the service needed from a dependent service."; break;
                case 14: Err = "The service has been disabled from the system."; break;
                case 15: Err = "The service does not have the correct authentication to run on the system."; break;
                case 16: Err = "This service is being removed from the system."; break;
                case 17: Err = "There is no execution thread for the service."; break;
                case 18: Err = "There are circular dependencies when starting the service."; break;
                case 19: Err = "There is a service running under the same name."; break;
                case 20: Err = "There are invalid characters in the name of the service."; break;
                case 21: Err = "Invalid parameters have been passed to the service."; break;
                case 22: Err = "The account, which this service is to run under is either invalid or lacks the permissions to run the service."; break;
                case 23: Err = "The service exists in the database of services available from the system."; break;
                case 24: Err = "The service is currently paused in the system."; break;
                default: Err = new Win32Exception((int)ErrorCode).Message; break;
            }
            return (Err);
        }

        List<PushServicesInfo> Services = null;
        PushServicesInfo SelectedService = null;
        string MachineID;
        public ctlServices(string MachineID)
        {
            this.MachineID = MachineID;
            InitializeComponent();
        }

        private void ctlServices_Load(object sender, EventArgs e)
        {
            lstServiceType.Items.Add("(any)");
            lstServiceType.Items.Add("Kernel Driver");
            lstServiceType.Items.Add("File System Driver");
            lstServiceType.Items.Add("Adapter");
            lstServiceType.Items.Add("Recognizer Driver");
            lstServiceType.Items.Add("Own Process");
            lstServiceType.Items.Add("Share Process");
            lstServiceType.Items.Add("Interactive Process");
            lstServiceType.SelectedIndex = 0;
            lstServices.ListViewItemSorter = new ListViewItemComparer(1);
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (SelectedService == null)
                return;
            PushServiceControlReq c = new PushServiceControlReq();
            c.Service = SelectedService.Name;
            c.Control = 2;
            PushServiceControlState res = Program.net.PushServiceControl(MachineID, c);
            if (res.ResultCode != 0)
                MessageBox.Show(this, "Starting Service " + c.Service + " threw an error: 0x" + res.ResultCode.ToString("X8") + ", " + GetServiceError(res.ResultCode), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            UpdateStatus();
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            if (SelectedService == null)
                return;
            PushServiceControlReq c = new PushServiceControlReq();
            c.Service = SelectedService.Name;
            c.Control = 1;
            PushServiceControlState res = Program.net.PushServiceControl(MachineID, c);
            if (res.ResultCode != 0)
                MessageBox.Show(this, "Stopping Service " + c.Service + " threw an error: 0x" + res.ResultCode.ToString("X8") + ", " + GetServiceError(res.ResultCode), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            UpdateStatus();
        }

        private void cmdPause_Click(object sender, EventArgs e)
        {
            if (SelectedService == null)
                return;

            string PauseText = "";
            int Control = 0;

            if (SelectedService.State.ToLower() == "paused")
            {
                PauseText = "Unpausing";
                Control = 4;
            }
            else
            {
                PauseText = "Pausing";
                Control = 3;
            }

            PushServiceControlReq c = new PushServiceControlReq();
            c.Service = SelectedService.Name;
            c.Control = Control;
            PushServiceControlState res = Program.net.PushServiceControl(MachineID, c);
            if (res.ResultCode != 0)
                MessageBox.Show(this, PauseText + " Service " + c.Service + " threw an error: 0x" + res.ResultCode.ToString("X8") + ", " + GetServiceError(res.ResultCode), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            UpdateStatus();
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            timRefresh.Enabled = chkAutoRefresh.Checked;
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void bgwRefresh_DoWork(object sender, DoWorkEventArgs e)
        {
            Network net = Program.net.CloneElement();
            Services = net.PushGetServices(MachineID);
            if (Services == null)
                return;
            ListServices(Services);
        }

        void ListServices(List<PushServicesInfo> lst)
        {
            if (lst == null)
                return;

            if (this.InvokeRequired == true)
            {
                this.Invoke(new DListServices(ListServices), lst);
                return;
            }

            List<ListViewItem> ProcessedItem = new List<ListViewItem>();
            List<ListViewItem> DeleteItem = new List<ListViewItem>();

            string FilterType = "";
            if (lstServiceType.SelectedIndex > 0)
                FilterType = lstServiceType.Text;

            foreach (PushServicesInfo srv in lst)
            {
                if (FilterType != "")
                {
                    if (srv.ServiceType.ToLower() != FilterType.ToLower())
                        continue;
                }

                ListViewItem ii = null;
                foreach (ListViewItem l in lstServices.Items)
                {
                    if (((PushServicesInfo)l.Tag).Name == srv.Name)
                    {
                        ii = l;
                        break;
                    }
                }
                if (ii == null)
                {
                    ii = new ListViewItem(srv.Name);
                    ii.Tag = srv;
                    ii.SubItems.Add(srv.DisplayName);
                    ii.SubItems.Add(srv.State);
                    ii.SubItems.Add(srv.Description);
                    ii.SubItems.Add(srv.StartMode);
                    ii.SubItems.Add(srv.StartName);
                    ii.SubItems.Add(srv.ServiceType);
                    lstServices.Items.Add(ii);
                }
                else
                {
                    ii.Tag = srv;
                    ii.SubItems[1].Text = srv.DisplayName;
                    ii.SubItems[2].Text = srv.State;
                    ii.SubItems[3].Text = srv.Description;
                    ii.SubItems[4].Text = srv.StartMode;
                    ii.SubItems[5].Text = srv.StartName;
                    ii.SubItems[6].Text = srv.ServiceType;
                }

                ProcessedItem.Add(ii);
            }

            foreach (ListViewItem ii in lstServices.Items)
            {
                if (ProcessedItem.Contains(ii) == false)
                    DeleteItem.Add(ii);
            }

            foreach (ListViewItem ii in DeleteItem)
            {
                lstServices.Items.Remove(ii);
            }
            lstServices_SelectedIndexChanged(null, null);
        }

        private void timRefresh_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        void UpdateStatus()
        {
            if (bgwRefresh.IsBusy == true)
                return;
            bgwRefresh.RunWorkerAsync();
        }

        private void lstServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListServices(Services);
        }

        private void lstServices_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lstServices.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }

        private void lstServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdPause.Enabled = cmdStart.Enabled = cmdStop.Enabled = false;

            if (lstServices.SelectedItems.Count == 0)
            {
                cmdPause.Enabled = cmdStart.Enabled = cmdStop.Enabled = false;
                SelectedService = null;
            }
            else
            {
                PushServicesInfo srv = (PushServicesInfo)lstServices.SelectedItems[0].Tag;
                SelectedService = srv;
                switch (srv.State.ToLower())
                {
                    case "stopped":
                        cmdStart.Enabled = true;
                        break;
                    case "start pending":
                    case "stop pending":
                    case "continue pending":
                    case "pause pending":
                        break;
                    case "paused":
                        if (srv.AcceptStop == true)
                            cmdStop.Enabled = true;
                        if (srv.AcceptPause == true)
                            cmdPause.Enabled = true;
                        break;
                    case "running":
                        if (srv.AcceptStop == true)
                            cmdStop.Enabled = true;
                        if (srv.AcceptPause == true)
                            cmdPause.Enabled = true;
                        break;
                    case "unknown":
                        cmdPause.Enabled = cmdStart.Enabled = cmdStop.Enabled = true;
                        break;
                }


            }
        }
    }
}
