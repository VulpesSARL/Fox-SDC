using FoxSDC_Common;
using FoxSDC_MGMT.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmComputerInfo : FForm
    {
        string MID;
        delegate void DUpdatePing(string Text, bool EnableControls);
        delegate void DUpdateWUText(string Text, bool Reboot);
        delegate void DListTasks(List<PushTaskManagerListElement> Tasks);
        delegate void DUpdateWU(List<WUUpdateInfo> Lst);
        ComputerData data;
        Int64? SelectedPolicy = null;

        public frmComputerInfo(string MachineID)
        {
            MID = MachineID;
            InitializeComponent();
        }

        void AddTab(UserControl ctrl, string TabText)
        {
            TabPage tp = new TabPage(TabText);            
            ctrl.Dock = DockStyle.Fill;
            tp.Controls.Add(ctrl);
            tabControl1.TabPages.Add(tp);
        }

        private void frmComputerInfo_Load(object sender, EventArgs e)
        {
            data = Program.net.GetComputerDetail(MID);
            lblPing.Text = "Status pending ..";
            lblWUReboot.Text = lblWUStatus.Text = "";
            picStatus.Image = Resources.Help.ToBitmap();
            if (data == null)
            {
                this.Text = "Computer Information: ???";
                return;
            }

            Program.LoadImageList(TVImgList);

            PComputerData transformeddata = new PComputerData();
            ClassCopy.CopyClassData(data, transformeddata);
            ((PComputerData)transformeddata).IOSVerType = ((ComputerData)data).OSVerType;
            ((PComputerData)transformeddata).OSWin10Edition = Win10Version.GetWin10Version(transformeddata.OSVersion);
            ((PComputerData)transformeddata).BMeteredConnection = data.IsMeteredConnection;

            PropertiesG.SelectedObject = transformeddata;

            this.Text = "Computer information: " + data.Computername;

            AddTab(new ctlServices(MID), "Services");
            AddTab(new ctlEventLogs(MID), "Event Log");
            AddTab(new ctlAddRemovePrograms(MID), "Programs");
            AddTab(new ctlListDiskData(MID), "Disk");
            AddTab(new ctlListNetworkConfig(MID), "Network");
            AddTab(new ctlDevices(MID), "Devices");
            AddTab(new ctlDevicesFilter(MID), "Filter Drivers");
            AddTab(new ctlBitlockerRK(MID), "Bitlocker Recovery");
            AddTab(new ctlWindowsLicense(MID), "Windows License");
            AddTab(new ctlFileTransfer(MID), "File transfer");
            AddTab(new ctlStartupItems(MID), "Startup Items");
            AddTab(new ctlSMARTInfo(MID), "SMART Data");
            AddTab(new ctlSimpleTasks(MID), "Simple Tasks");

            timerPinger_Tick(null, null);

            lstRemotePort.Items.Add("HTTP");
            lstRemotePort.Items.Add("RDP");
            lstRemotePort.Items.Add("HTTPS");
            lstRemotePort.Items.Add("SSH");
            lstRemotePort.Items.Add("SMTP");
            lstRemotePort.Items.Add("POP3");
            lstRemotePort.Items.Add("IMAP");
            lstRemotePort.Items.Add("TELNET");

            LoadPolicies();
        }

        void LoadPolicies()
        {
            TVPolicies.Nodes.Clear();

            List<PolicyObject> pol = Program.net.ListPolicies(false, MID, null, false);
            if (pol != null)
            {
                foreach (PolicyObject ge in pol)
                {
                    int IconIndex = PolicyList.GetIconIndex(ge.Type);
                    TVPolicies.Nodes.Add(ge.ID.ToString(), ge.Name + (ge.Enabled == true ? "" : " [disabled]"), IconIndex, IconIndex);
                }
            }
        }

        private void frmComputerInfo_Shown(object sender, EventArgs e)
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count > 0)
                {
                    if (tab.Controls[0] is ctlEventLogs)
                    {
                        ctlEventLogs ev = (ctlEventLogs)tab.Controls[0];
                        ev.FixView();
                        break;
                    }
                }
            }
        }

        void UpdatePing(string Text, bool EnableControls)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new DUpdatePing(UpdatePing), Text, EnableControls);
                return;
            }
            lblPing.Text = Text;
            if (EnableControls == true)
                picStatus.Image = Resources.NetCon.ToBitmap();
            else
                picStatus.Image = Resources.NetNotCon.ToBitmap();
        }

        private void bgwPinger_DoWork(object sender, DoWorkEventArgs e)
        {
            Network net = Program.net.CloneElement();
            bool res = net.PushPing(MID);
            UpdatePing(res == true ? "Online" : "Not connected", res);
        }

        void UpdateTaskList(PushTaskManagerListElement element)
        {
            foreach (ListViewItem ll in lstTasks.Items)
            {
                if (((PushTaskManagerListElement)ll.Tag).ProcessID == element.ProcessID)
                {
                    ll.Tag = element;
                    ll.SubItems[3].Text = CommonUtilities.NiceSize(element.PrivateBytes);
                    ll.SubItems[4].Text = CommonUtilities.NiceSize(element.WorkingSet);
                    ll.SubItems[10].Text = element.TotalProcessorTime.ToString();
                    ll.SubItems[11].Text = element.UserProcessorTime.ToString();
                    return;
                }
            }

            ListViewItem l = new ListViewItem(element.ProcessName);
            l.Tag = element;
            l.SubItems.Add(data.Is64Bit == false ? "32 bit" : (element.IsWOWProcess == true ? "32 bit" : "64 bit"));
            l.SubItems.Add(element.ProcessID.ToString());
            l.SubItems.Add(CommonUtilities.NiceSize(element.PrivateBytes));
            l.SubItems.Add(CommonUtilities.NiceSize(element.WorkingSet));
            l.SubItems.Add(element.Description);
            l.SubItems.Add(element.CompanyName);
            l.SubItems.Add(element.Username);
            l.SubItems.Add(element.Filename);
            l.SubItems.Add(element.Arguments);
            l.SubItems.Add(element.TotalProcessorTime.ToString());
            l.SubItems.Add(element.UserProcessorTime.ToString());
            lstTasks.Items.Add(l);
        }

        void ListTasks(List<PushTaskManagerListElement> Tasks)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new DListTasks(ListTasks), Tasks);
                return;
            }

            List<int> PIDs = new List<int>();

            foreach (PushTaskManagerListElement element in Tasks)
            {
                UpdateTaskList(element);
                PIDs.Add(element.ProcessID);
            }

            List<ListViewItem> RM = new List<ListViewItem>();
            foreach (ListViewItem ll in lstTasks.Items)
            {
                if (PIDs.Contains(((PushTaskManagerListElement)ll.Tag).ProcessID) == false)
                {
                    RM.Add(ll);
                }
            }

            foreach (ListViewItem r in RM)
                lstTasks.Items.Remove(r);
        }

        private void timerPinger_Tick(object sender, EventArgs e)
        {
            if (bgwPinger.IsBusy == false)
                bgwPinger.RunWorkerAsync();
        }

        private void cmdRefreshTasks_Click(object sender, EventArgs e)
        {
            if (bgwListTasks.IsBusy == false)
                bgwListTasks.RunWorkerAsync();
        }

        private void bgwListTasks_DoWork(object sender, DoWorkEventArgs e)
        {
            Network net = Program.net.CloneElement();
            List<PushTaskManagerListElement> Tasks = net.PushGetTasks(MID);
            if (Tasks == null)
                return;
            ListTasks(Tasks);
        }

        private void chkAutoRefreshTasks_CheckedChanged(object sender, EventArgs e)
        {
            timerTaskList.Enabled = chkAutoRefreshTasks.Checked;
        }

        private void timerTaskList_Tick(object sender, EventArgs e)
        {
            if (bgwListTasks.IsBusy == false)
                bgwListTasks.RunWorkerAsync();
        }

        private void cmdKill_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedItems.Count == 0)
                return;
            PushTaskManagerListElement l = (PushTaskManagerListElement)lstTasks.SelectedItems[0].Tag;
            if (MessageBox.Show(this, "Do you want to kill the process " + l.ProcessName + " (PID: " + l.ProcessID.ToString() + ")?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            Program.net.PushKillTask(MID, l.ProcessID);
        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
            bool TaskManagerTimerRunning = timerTaskList.Enabled;
            timerTaskList.Enabled = false;
            frmNewTask frm = new frmNewTask(MID, data.SystemRoot);
            frm.ShowDialog(this);
            timerTaskList.Enabled = TaskManagerTimerRunning;
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            int LocalPort;
            int RemotePort;

            switch (lstRemotePort.Text)
            {
                case "HTTP": lstRemotePort.Text = "80"; break;
                case "HTTPS": lstRemotePort.Text = "443"; break;
                case "RDP": lstRemotePort.Text = "3389"; break;
                case "POP3": lstRemotePort.Text = "110"; break;
                case "SMTP": lstRemotePort.Text = "25"; break;
                case "IMAP": lstRemotePort.Text = "143"; break;
                case "SSH": lstRemotePort.Text = "22"; break;
                case "TELNET": lstRemotePort.Text = "23"; break;
            }

            if (int.TryParse(txtLocalPort.Text, out LocalPort) == false)
            {
                MessageBox.Show(this, "Invalid Local Port.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (LocalPort < 1 || LocalPort > 65535)
            {
                MessageBox.Show(this, "Invalid Local Port.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtRemoteServer.Text.Trim() == "")
            {
                MessageBox.Show(this, "Invalid Remote Server.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtRemoteServer.Text.Contains("\"") == true || txtRemoteServer.Text.Contains("^") == true ||
                txtRemoteServer.Text.Contains("\\") == true || txtRemoteServer.Text.Contains("%") == true)
            {
                MessageBox.Show(this, "Invalid Remote Server.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (int.TryParse(lstRemotePort.Text, out RemotePort) == false)
            {
                MessageBox.Show(this, "Invalid Remote Port.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (RemotePort < 1 || RemotePort > 65535)
            {
                MessageBox.Show(this, "Invalid Remote Port.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string Exec = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "FoxSDC_RemoteConnect.exe");
            string SessionID = Program.net.CloneSession();
            if (string.IsNullOrWhiteSpace(SessionID) == true)
            {
                MessageBox.Show(this, "Cannot get a new SessionID from the Server", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = Exec;
                p.StartInfo.Arguments = "-direct \"" + Program.net.ConnectedURL + "\" \"" + MID + "\" \"" + SessionID + "\" " + LocalPort.ToString() + " \"" + txtRemoteServer.Text + "\" " + RemotePort.ToString();
                p.StartInfo.UseShellExecute = false;
                p.Start();
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Cannot start the process " + Exec + " - " + ee.Message, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Debug.WriteLine(ee.ToString());
                return;
            }

        }

        private void chkWUAutoCheck_CheckedChanged(object sender, EventArgs e)
        {
            timerWU.Enabled = chkWUAutoCheck.Enabled;
        }

        void UpdateWUStatus(string WU, bool Reboot)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new DUpdateWUText(UpdateWUStatus), WU, Reboot);
                return;
            }
            lblWUStatus.Text = WU;
            lblWUReboot.Text = Reboot == true ? "Needed" : "Not needed";
        }

        private void cmdWUQuery_Click(object sender, EventArgs e)
        {
            if (bgwWUQuery.IsBusy == true)
                return;
            bgwWUQuery.RunWorkerAsync();
        }

        private void bgwWUQuery_DoWork(object sender, DoWorkEventArgs e)
        {
            Network net = Program.net.CloneElement();
            WUStatus sta = net.PushWUStatus(MID);
            bool Reboot = net.PushWUStatusRestart(MID);
            if (sta == null)
                UpdateWUStatus("N/A", Reboot);
            else
                UpdateWUStatus(sta.Text, Reboot);
        }

        private void timerWU_Tick(object sender, EventArgs e)
        {
            if (bgwWUQuery.IsBusy == true)
                return;
            bgwWUQuery.RunWorkerAsync();
        }

        private void cmdWUCheckUpdates_Click(object sender, EventArgs e)
        {
            Program.net.PushWUCheck(MID);
        }

        private void cmdWUInstallUpdates_Click(object sender, EventArgs e)
        {
            Program.net.PushWUInstall(MID);
        }

        void UpdateList(List<WUUpdateInfo> lst)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new DUpdateWU(UpdateList), lst);
                return;
            }
            lstWUUpdates.Items.Clear();
            if (lst == null)
                return;
            foreach (WUUpdateInfo l in lst)
            {
                ListViewItem i = new ListViewItem(l.ID);
                i.SubItems.Add(l.Name);
                i.SubItems.Add(l.Description);
                i.SubItems.Add(l.Link);
                lstWUUpdates.Items.Add(i);
            }
        }

        private void cmdRestart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to restart the computer " + data.Computername + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                Program.net.PushClientRestart(MID);
        }

        private void cmdFRestart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to FORCE restart the computer " + data.Computername + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                Program.net.PushClientRestartForced(MID);
        }

        private void cmdWUGetList_Click(object sender, EventArgs e)
        {
            List<WUUpdateInfo> lst = Program.net.PushWUGetList(MID);
            UpdateList(lst);
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createpolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreatePolicy frm = new frmCreatePolicy(MID, null);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                LoadPolicies();
        }

        private void policyEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TVPolicies.SelectedNode == null)
                return;
            Int64 SelPol = Convert.ToInt64(TVPolicies.SelectedNode.Name);

            PolicyObject obj = Program.net.GetPolicyObject(SelPol);
            if (obj == null)
                return;

            if (Program.net.EnableDisablePolicy(obj.ID, !obj.Enabled) == false)
            {
                MessageBox.Show(this, "Cannot enable/disable policy: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            TVPolicies.SelectedNode.Text = obj.Name + (obj.Enabled == false ? "" : " [disabled]");
        }

        private void deletePolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TVPolicies.SelectedNode == null)
                return;
            Int64 SelPol = Convert.ToInt64(TVPolicies.SelectedNode.Name);

            PolicyObject obj = Program.net.GetPolicyObject(SelPol);
            if (obj == null)
                return;

            if (MessageBox.Show(this, "Do you want to delete the policy " + obj.Name + "?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
                return;

            if (Program.net.DeletePolicy(obj.ID) == false)
            {
                MessageBox.Show(this, "Cannot delete policy: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TVPolicies.Nodes.Remove(TVPolicies.SelectedNode);
        }

        private void lowLevelEditPolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TVPolicies.SelectedNode == null)
                return;
            Int64 SelPol = Convert.ToInt64(TVPolicies.SelectedNode.Name);

            PolicyObject obj = Program.net.GetPolicyObject(SelPol);
            if (obj == null)
                return;
            frmLowLevelEdit frm = new frmLowLevelEdit(obj);
            frm.ShowDialog(this);
        }

        private void TVPolicies_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Splitty.Panel2.Controls.Clear();
            if (TVPolicies.SelectedNode == null)
            {
                SelectedPolicy = null;
                return;
            }

            SelectedPolicy = Convert.ToInt64(e.Node.Name);

            PolicyObject obj = Program.net.GetPolicyObject(SelectedPolicy.Value);
            if (obj == null)
            {
                MessageBox.Show(this, "Loading policy failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                PolicyElementInterface i = PolicyList.GetInstance(obj.Type);
                i.SetData(obj);
                UserControl ctl = (UserControl)i;
                ctl.Dock = DockStyle.Fill;
                Splitty.Panel2.Controls.Add(ctl);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            createpolicyToolStripMenuItem.Enabled = true;
            policyEnabledToolStripMenuItem.Enabled = false;
            lowLevelEditPolicyToolStripMenuItem.Enabled = false;
            deletePolicyToolStripMenuItem.Enabled = false;
            policyEnabledToolStripMenuItem.Text = "Enable/Disable polic&y";

            if (TVPolicies.SelectedNode == null)
                return;

            deletePolicyToolStripMenuItem.Enabled = true;
            policyEnabledToolStripMenuItem.Enabled = true;
            if (Settings.Default.EnableDebug == true)
            {
                lowLevelEditPolicyToolStripMenuItem.Visible = true;
                lowLevelEditPolicyToolStripMenuItem.Enabled = true;
            }
            else
            {
                lowLevelEditPolicyToolStripMenuItem.Visible = false;
                lowLevelEditPolicyToolStripMenuItem.Enabled = false;
            }

            Int64 SelPol = Convert.ToInt64(TVPolicies.SelectedNode.Name);
            PolicyObject obj = Program.net.GetPolicyObject(SelPol);
            if (obj == null)
            {
                policyEnabledToolStripMenuItem.Enabled = false;
            }
            else
            {
                policyEnabledToolStripMenuItem.Enabled = true;
                policyEnabledToolStripMenuItem.Text = obj.Enabled == true ? "Disable polic&y" : "Enable polic&y";
            }
        }

        private void cmdRemoteScreen_Click(object sender, EventArgs e)
        {
            Utilities.ConnectToScreen(this, MID);
        }

        void AppendText(PushChatMessage chat)
        {
            txtRecvText.Text += chat.DT.ToLocalTime().ToLongDateString() + " " + chat.DT.ToLocalTime().ToLongTimeString() +
                " - " + chat.Name + ":\r\n" + chat.Text + "\r\n";
            txtRecvText.SelectionStart = txtRecvText.Text.Length;
            txtRecvText.SelectionLength = 0;
            txtRecvText.ScrollToCaret();
        }

        private void timChat_Tick(object sender, EventArgs e)
        {
            List<PushChatMessage> chats = Program.net.GetChatMessages(MID);
            if (chats == null)
                return;
            if (chats.Count == 0)
                return;
            foreach (PushChatMessage chat in chats)
            {
                AppendText(chat);
            }
            WindowFlashing.FlashWindowEx(this);
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            if (txtSendText.Text.Trim() == "")
                return;
            Program.net.PushSendChat(MID, "", txtSendText.Text.Trim());
            PushChatMessage c = new PushChatMessage();
            c.DT = DateTime.UtcNow;
            c.Name = "You";
            c.Text = txtSendText.Text.Trim();
            c.ID = 0;
            AppendText(c);
            txtSendText.Text = "";
        }

        private void txtSendText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt == false && e.Control == false && e.Shift == false && (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {
                cmdSend_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
