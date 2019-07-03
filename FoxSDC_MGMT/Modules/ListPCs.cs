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
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Net.NetworkInformation;
using FoxSDC_MGMT.Properties;
using System.Net;

namespace FoxSDC_MGMT
{
    public partial class ctlListPCs : UserControl
    {
        bool? Approved;
        Int64? Group;
        bool LstOnly;

        public ctlListPCs(bool? approved, Int64? group)
        {
            Approved = approved;
            Group = group;
            InitializeComponent();
        }

        public void SelectMachineID(string MID)
        {
            foreach (ListViewItem lst in lstComputers.Items)
            {
                ComputerData cd = (ComputerData)lst.Tag;
                if (cd.MachineID.ToLower() == MID.ToLower())
                {
                    lst.Checked = true;
                    lst.Selected = true;
                    lstComputers.EnsureVisible(lst.Index);
                }
            }
        }

        public void UncheckItems()
        {
            foreach (ListViewItem lst in lstComputers.Items)
            {
                lst.Checked = false;
            }
        }

        public ctlListPCs()
        {
            Approved = true;
            Group = null;
            InitializeComponent();
        }
        public bool ShowCheckBoxes
        {
            get
            {
                return (lstComputers.CheckBoxes);
            }
            set
            {
                lstComputers.CheckBoxes = value;
            }
        }

        public bool ListOnly
        {
            get
            {
                return (LstOnly);
            }
            set
            {
                LstOnly = value;
            }
        }

        public ListView.CheckedListViewItemCollection CheckedItems
        {
            get
            {
                return (lstComputers.CheckedItems);
            }
        }

        void LoadList()
        {
            lstComputers.Items.Clear();
            List<ComputerData> cdlst = Program.net.GetComputerList(Approved, Group);
            if (cdlst == null)
                return;
            foreach (ComputerData cd in cdlst)
            {
                ListViewItem lst = new ListViewItem(cd.Computername);
                lst.Tag = cd;
                lst.ImageIndex = 2;
                lst.SubItems.Add(cd.GroupingPath);
                lst.SubItems.Add(cd.Comments);
                lst.SubItems.Add(cd.Approved == true ? "Approved" : "Not approved");
                lst.SubItems.Add(cd.OS);
                lst.SubItems.Add(cd.OSVersion);
                lst.SubItems.Add(Win10Version.GetWin10Version(cd.OSVersion));
                lst.SubItems.Add(cd.Is64Bit == true ? "64 bit" : "32 bit");
                lst.SubItems.Add(cd.OSSuite);
                lst.SubItems.Add(cd.Language);
                lst.SubItems.Add(cd.Make);
                lst.SubItems.Add(cd.LastUpdated.ToLongDateString() + " " + cd.LastUpdated.ToLongTimeString());
                lst.SubItems.Add(cd.ContractID);
                lst.SubItems.Add(cd.AgentVersion);
                lstComputers.Items.Add(lst);
            }
        }

        private void ctlUnapprovedPCs_Load(object sender, EventArgs e)
        {
            if (this.DesignMode == true)
                return;
            Program.LoadImageList(imageList1);
            LoadList();
            if (LstOnly == true)
                lstComputers.ContextMenu = null;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            if (lstComputers.SelectedItems.Count == 0)
                return;
            ComputerData cd = (ComputerData)lstComputers.SelectedItems[0].Tag;
            frmComputerInfo pci = new frmComputerInfo(cd.MachineID);
            pci.Show();
        }

        private void lstComputers_KeyDown(object sender, KeyEventArgs e)
        {
            if (LstOnly == true)
                return;
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                propertiesToolStripMenuItem_Click(sender, e);
        }

        private void approveRefuseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            if (lstComputers.SelectedItems.Count == 0)
                return;

            List<ApproveListElement> ll = new List<ApproveListElement>();

            foreach (ListViewItem l in lstComputers.SelectedItems)
            {
                ComputerData cd = (ComputerData)l.Tag;
                ApproveListElement a = new ApproveListElement();
                a.Computername = cd.Computername;
                a.MachineID = cd.MachineID;
                ll.Add(a);
            }
            frmApproveRefusePC frm = new frmApproveRefusePC(ll);
            if (frm.ShowDialog(this) == DialogResult.OK)
                LoadList();
        }

        private void lstComputers_DoubleClick(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            propertiesToolStripMenuItem_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            if (lstComputers.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem l in lstComputers.SelectedItems)
            {
                ComputerData cd = (ComputerData)l.Tag;
                DialogResult res = MessageBox.Show(this, "Do you really want to delete the computer " + cd.Computername + "?", Program.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (res == DialogResult.Cancel)
                    return;
                if (res == DialogResult.No)
                    continue;
                if (Program.net.DeleteComputer(cd.MachineID) == false)
                {
                    if (MessageBox.Show(this, "Delete of computer " + cd.Computername + " failed: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == DialogResult.Cancel)
                        return;
                }
            }
            LoadList();
        }

        private void setcommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            if (lstComputers.SelectedItems.Count != 1)
                return;
            foreach (ListViewItem l in lstComputers.SelectedItems)
            {
                ComputerData cd = (ComputerData)l.Tag;
                frmAskText txt = new frmAskText("Change comment", "Change comment on computer " + cd.Computername, cd.Comments);
                if (txt.ShowDialog(this) != DialogResult.OK)
                    return;
                Program.net.ChangeComputerComment(cd.MachineID, txt.RetText);
            }
            LoadList();
        }

        private void connectToScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            if (lstComputers.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem l in lstComputers.SelectedItems)
            {
                ComputerData cd = (ComputerData)l.Tag;
                if (cd.Approved == true)
                    Utilities.ConnectToScreen(this, cd.MachineID);
            }
        }

        private void openCommandPromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            if (lstComputers.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem l in lstComputers.SelectedItems)
            {
                ComputerData cd = (ComputerData)l.Tag;
                if (cd.Approved == true)
                {
                    PushRunTask nt = new PushRunTask();
                    nt.Executable = cd.SystemRoot;
                    if (nt.Executable.EndsWith("\\") == false)
                        nt.Executable += "\\";
                    nt.Executable += "System32\\cmd.exe";
                    nt.Args = "";
                    nt.SessionID = 0;
                    nt.Username = "";
                    nt.Password = "";
                    nt.Option = PushRunTaskOption.SystemUserConsoleRedir;

                    PushRunTaskResult Res = Program.net.PushRunFile(cd.MachineID, nt);
                    if (Res == null)
                    {
                        MessageBox.Show(this, "No response from Server / Agent.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (Res.Result == 0)
                    {
                        string Exec = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "FoxSDC_RedirConsole.exe");
                        string SessionID = Program.net.CloneSession();
                        if (string.IsNullOrWhiteSpace(SessionID) == true)
                        {
                            MessageBox.Show(this, "Program started successfully at the remote location, but didn't got a cloned Session ID.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        try
                        {
                            Process p = new Process();
                            p.StartInfo.FileName = Exec;
                            p.StartInfo.Arguments = "-direct \"" + Program.net.ConnectedURL + "\" \"" + cd.MachineID + "\" \"" + SessionID + "\" \"" + Res.SessionID + "\"";
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
                }
            }
        }

        bool PortAvailable(int Port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            bool isAvailable = true;

            foreach (IPEndPoint tcpi in tcpConnInfoArray)
            {
                if (tcpi.Port == Port)
                {
                    isAvailable = false;
                    break;
                }
            }
            return (isAvailable);
        }

        private void openRemoteDesktopConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LstOnly == true)
                return;
            if (lstComputers.SelectedItems.Count == 0)
                return;

            foreach (ListViewItem l in lstComputers.SelectedItems)
            {
                ComputerData cd = (ComputerData)l.Tag;
                if (cd.Approved == true)
                {
                    int Port = 9999;
                    while (PortAvailable(Port) == false)
                    {
                        Port++;
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
                        p.StartInfo.Arguments = "-direct \"" + Program.net.ConnectedURL + "\" \"" + cd.MachineID + "\" \"" + SessionID + "\" " + Port.ToString() + " \"" + "127.0.0.1" + "\" " + "3389";
                        p.StartInfo.UseShellExecute = false;
                        p.Start();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(this, "Cannot start the process " + Exec + " - " + ee.Message, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Debug.WriteLine(ee.ToString());
                        return;
                    }

                    try
                    {
                        string MSTSCSettings = Resources.MSTSC.Replace("{ADDR}", "127.0.0.1:" + Port.ToString());
                        string TempFile;
                        TempFile = Environment.ExpandEnvironmentVariables("%TEMP%");
                        if (TempFile.EndsWith("\\") == false)
                            TempFile += "\\";
                        TempFile += "FoxSDC-MSTSC-" + Guid.NewGuid().ToString() + ".rdp";

                        File.WriteAllText(TempFile, MSTSCSettings, Encoding.ASCII);

                        Process p = new Process();
                        p.StartInfo.FileName = Environment.ExpandEnvironmentVariables ("%systemroot%\\system32\\mstsc.exe");
                        p.StartInfo.Arguments = TempFile;
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
            }
        }
    }
}
