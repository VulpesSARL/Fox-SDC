using FoxSDC_Common;
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
    public partial class frmNewTask : FForm
    {
        string MachineID;
        string SystemRoot;
        public frmNewTask(string MachineID, string SystemRoot)
        {
            this.MachineID = MachineID;
            this.SystemRoot = SystemRoot;
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtFilename.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please specifiy a filename to execute on the remote computer.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (txtFilename.Text.ToLower().Trim())
            {
                case "cmd": txtFilename.Text = SystemRoot + "system32\\cmd.exe"; break;
                case "powershell": txtFilename.Text = SystemRoot + "System32\\WindowsPowerShell\\v1.0\\Powershell.exe"; break;
            }

            if (lstSessions.SelectedItem == null && lstRunAs.SelectedIndex != 2)
            {
                MessageBox.Show(this, "Please specifiy the session where to run the program.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SessionElement sess = (SessionElement)lstSessions.SelectedItem;

            PushRunTask nt = new PushRunTask();
            nt.Executable = txtFilename.Text;
            nt.Args = txtArgs.Text;
            nt.SessionID = sess == null ? 0 : sess.SessionID;
            nt.Username = txtUsername.Text;
            nt.Password = txtPassword.Text;
            nt.Option = (PushRunTaskOption)lstRunAs.SelectedIndex;

            PushRunTaskResult Res = Program.net.PushRunFile(MachineID, nt);
            if (Res == null)
            {
                MessageBox.Show(this, "No response from Server / Agent.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Res.Result == 0)
            {
                if (lstRunAs.SelectedIndex == 2)
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
                        p.StartInfo.Arguments = "-direct \"" + Program.net.ConnectedURL + "\" \"" + MachineID + "\" \"" + SessionID + "\" \"" + Res.SessionID + "\"";
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
                this.Close();
                return;
            }
            else
            {
                string errorMessage = new Win32Exception((int)(Res.Result)).Message;
                MessageBox.Show(this, "Failed to run task: 0x" + Res.Result.ToString("X") + " - " + errorMessage, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            frmRemoteOpenSaveDlg frm = new frmRemoteOpenSaveDlg(MachineID, SystemRoot, frmRemoteOpenSaveDlg.OpenSaveMode.Open, "Run", "Executables|*.exe|All files|*.*", false, Program.net.CloneElement());
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            txtFilename.Text = frm.SelectedFilename;
        }

        private void lstSessions_DropDown(object sender, EventArgs e)
        {
            if (lstSessions.Items.Count == 0)
                cmdRefreshSessions_Click(sender, e);
        }

        private void cmdRefreshSessions_Click(object sender, EventArgs e)
        {
            lstSessions.Items.Clear();
            List<PushRunningSessionElement> sessions = Program.net.PushGetSessions(MachineID);
            if (sessions == null)
                return;
            foreach (PushRunningSessionElement s in sessions)
                lstSessions.Items.Add(new SessionElement(s));
            if (lstSessions.Items.Count > 0)
                lstSessions.SelectedIndex = 0;
        }

        private void frmNewTask_Load(object sender, EventArgs e)
        {
            lstRunAs.Items.Add("Run as actual user in session");
            lstRunAs.Items.Add("Run as different user in session");
            lstRunAs.Items.Add("Redirect console to here");
            lstRunAs.Items.Add("Steal Winlogon Token");
            lstRunAs.SelectedIndex = 0;
            if (SystemRoot.EndsWith("\\") == false)
                SystemRoot += "\\";
        }

        private void lstRunAs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRunAs.SelectedIndex == 1)
                txtPassword.Enabled = txtUsername.Enabled = true;
            else
                txtPassword.Enabled = txtUsername.Enabled = false;
        }

        private void cmdBrowseArgs_Click(object sender, EventArgs e)
        {
            frmRemoteOpenSaveDlg frm = new frmRemoteOpenSaveDlg(MachineID, SystemRoot, frmRemoteOpenSaveDlg.OpenSaveMode.Open, "Browse for argurments", "All files|*.*", false, Program.net.CloneElement());
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            txtArgs.Text = frm.SelectedFilename;
        }
    }
}
