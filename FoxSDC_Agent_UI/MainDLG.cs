using FoxSDC_Agent_UI.Properties;
using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    public partial class MainDLG : FForm
    {
        private bool allowshowdisplay = false;
        public bool allowclose = false;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        int TimerCounter = 0;

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }

        public MainDLG()
        {
            InitializeComponent();

            IntPtr ptr = FindWindow("Shell_TrayWnd", null);
            if (ptr.ToInt32() == 0)
            {
                Program.NT3 = new frmNT3Icon();
                Program.NT3.Show();
            }

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += Timer_Tick;

            if (Environment.OSVersion.Version.Major == 6)
            {
                if (Environment.OSVersion.Version.Minor <= 2)
                {
                    //older version of Windows
                    frmNotes frm = new frmNotes(
                        @"Astellung vum Support vu Windows 7 & Windows 8.0

Vulpes stellt deemnächst de Support vu Windows 7 a Windows 8.0 iwwert den Software Deployment and Control Programm an. Kontaktéier Vulpes, ob en Update op Windows 10 gemeet ka ginn. Wann der do näischt maacht, wäert de Programm einfach säint Déngscht astellen oder desinstalléiert sech automatesch.

Discontinuation supporting Windows 7 & Windows 8.0

Vulpes will discontinue to support Windows 7 & Windows 8.0 trough Software Deployment and Control Application. Please contact Vulpes if there’s a possibility to update to Windows 10. If you do nothing, the Program will stop running or uninstalls itself automatically.

Einstellung des Supports für Windows 7 & Windows 8.0

Vulpes stellt demnächst den Support für Windows 7 & Windows 8.0 über das Software Deployment and Control Programm ein. Bitte kontaktieren Sie Vulpes ob es eine Möglichkeit besteht ein Update auf Windows 10 durchzuführen. Falls Sie nichts unternehmen, wird das Programm den Dienst einstellen oder deinstalliert sich selbst.",
                        "OldOSVersion");
                    if (frm.ShouldShowThis() == true)
                        frm.Show();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerCounter++;
#if !DEBUG
            if (TimerCounter > 60)
#else
            if (TimerCounter > 5)
#endif
            {
                TimerCounter = 0;
                if (Program.Chat == null)
                {
                    PushChatMessage chat = Status.PopChatMessage();
                    if (chat == null)
                        return;
                    Program.Chat = new frmChat(chat);
                    Program.Chat.Show();
                }
            }
        }

        private void MainDLG_Load(object sender, EventArgs e)
        {
            timUpdate.Enabled = true;
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void MainDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowclose == true)
                return;
            if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.TaskManagerClosing)
                return;
            e.Cancel = true;
            this.Visible = false;
        }

        private void niIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showStatusToolStripMenuItem_Click(sender, e);
        }

        public void showStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status.UpdateStatus(this);
            allowshowdisplay = true;
            this.Visible = true;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
        }

        private void timUpdate_Tick(object sender, EventArgs e)
        {
            Status.UpdateStatus(this);
            if (Status.HasIssues == true)
                timUpdate.Interval = 30000;
            else
                timUpdate.Interval = 1000;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            string Admin = RegistryData.AdministratorName.Trim();
            if (Admin == "")
                Admin = "Administrator";
            mnwriteAmessageToVulpesToolStripMenuItem.Text = "Write a &message to " + Admin;
            startChatWithVulpesToolStripMenuItem1.Text = "&Start chat with " + Admin;
#if !DEBUG
            if (Control.ModifierKeys == Keys.Control || RegistryData.ShowClientEnhancedMenu == 1)
            {
                exitToolStripMenuItem.Visible = true;
                invokeToolStripMenuItem.Visible = true;
            }
            else
            {
                exitToolStripMenuItem.Visible = false;
                invokeToolStripMenuItem.Visible = false;
            }
#else
            exitToolStripMenuItem.Visible = true;
            invokeToolStripMenuItem.Visible = true;
#endif
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allowclose = true;
            this.Close();
            if (Program.NT3 != null)
                Program.NT3.Close();
            Application.Exit();
        }

        private void policiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status.InvokeUpdate(MessageInvoke.ReloadPolicies);
        }

        private void reportingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status.InvokeUpdate(MessageInvoke.ReloadReports);
        }

        private void cmdButton1_Click(object sender, EventArgs e)
        {
            Status.ResponseMessage(this, MessageResponse.Button1);
        }

        private void cmdButton2_Click(object sender, EventArgs e)
        {
            Status.ResponseMessage(this, MessageResponse.Button2);
        }

        private void cmdButton3_Click(object sender, EventArgs e)
        {
            Status.ResponseMessage(this, MessageResponse.Button3);
        }

        private void installOptionalPackagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCustomPackages frm = new frmCustomPackages();
            frm.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.Show();
        }

        private void writeAmessageToVulpesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWriteMessage frm = new frmWriteMessage();
            frm.Show();
        }

        private void writeMessageToVulpesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWriteMessage frm = new frmWriteMessage();
            frm.Show();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.Show();
        }

        private void mnSDC_DropDownOpening(object sender, EventArgs e)
        {
            string Admin = RegistryData.AdministratorName.Trim();
            if (Admin == "")
                Admin = "Administrator";
            writeMessageToVulpesToolStripMenuItem.Text = "Write a &message to " + Admin;
            startChatWithVulpesToolStripMenuItem.Text = "&Start chat with " + Admin;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void startChatWithVulpesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Chat == null)
            {
                Program.Chat = new frmChat(null);
                Program.Chat.Show();
            }
            else
            {
                Program.Chat.BringToFront();
                Program.Chat.Activate();
            }
        }

        private void startChatWithVulpesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            startChatWithVulpesToolStripMenuItem_Click(sender, e);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserSettings frm = new frmUserSettings();
            frm.ShowDialog(this);
        }
    }
}
