using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Common;

namespace FoxSDC_MGMT
{
    public partial class frmSeverSettings : FForm
    {
        public frmSeverSettings()
        {
            InitializeComponent();
        }

        private void SeverSettings_Load(object sender, EventArgs e)
        {
            List<string> Cert = Program.net.GetServerCertificates();
            if (Cert != null)
            {
                foreach (string C in Cert)
                {
                    lstCert.Items.Add(C);
                }
            }

            lblAnchor.Visible = false;
            ctlPaperManager paper;

            paper = new ctlPaperManager("COMPUTERREPORT", "Computer Report");
            paper.Location = lblAnchor.Location;
            tabPage5.Controls.Add(paper);

            ServerSettings ss = Program.net.GetServerSettings();
            if (ss != null)
            {
                lstCert.Text = ss.UseCertificate;
                txtEventLogFlush.Text = ss.KeepEventLogDays.ToString();
                txtKeepDisks.Text = ss.KeepNonPresentDisks.ToString();
                txtKeepReports.Text = ss.KeepReports.ToString();
                txtKeepBitlockerRK.Text = ss.KeepBitlockerRK.ToString();
                txtKeepChatLog.Text = ss.KeepChatLogs.ToString();

                txtMailFrom.Text = ss.EMailFrom;
                txtMailFromName.Text = ss.EMailFromFriendly;
                txtMailPort.Text = ss.EMailPort.ToString();
                txtMailServer.Text = ss.EMailServer;
                txtMailAdminAddress.Text = ss.EMailAdminTo;
                txtMailUsername.Text = ss.EMailUsername;
                txtMailPassword.Text = ss.EMailPassword;
                txtAdminEMailText.Text = ss.EMailAdminText;
                txtClientEMailText.Text = ss.EMailClientText;
                chkAdminEMailTextIsHTML.Checked = ss.EMailAdminIsHTML;
                chkClientEMailTextIsHTML.Checked = ss.EMailClientIsHTML;
                txtEMailClientSubject.Text = ss.EMailClientSubject;
                txtEMailAdminSubject.Text = ss.EMailAdminSubject;
                txtAdminAccess.Text = ss.AdminIPAddresses;
                txtAdminName.Text = ss.AdministratorName;
                txtMessageDisclaimer.Text = ss.MessageDisclaimer;

                chkEMailUseSSL.Checked = ss.EMailUseSSL;
                lblAdminSched.Text = Scheduler.Explain(ss.EMailAdminScheduling);
                lblAdminSched.Tag = ss.EMailAdminScheduling;
                lblClientSched.Text = Scheduler.Explain(ss.EMailClientScheduling);
                lblClientSched.Tag = ss.EMailClientScheduling;
            }
            else
            {
                tabControl1.Enabled = false;
                cmdOK.Enabled = false;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            ServerSettings ss = new ServerSettings();
            ss.UseCertificate = lstCert.Text;
            ss.EMailFrom = txtMailFrom.Text;
            ss.EMailFromFriendly = txtMailFromName.Text;
            ss.EMailServer = txtMailServer.Text;
            ss.EMailAdminTo = txtMailAdminAddress.Text;
            ss.EMailUsername = txtMailUsername.Text;
            ss.EMailPassword = txtMailPassword.Text;
            ss.EMailUseSSL = chkEMailUseSSL.Checked;
            ss.EMailAdminText = txtAdminEMailText.Text;
            ss.EMailClientText = txtClientEMailText.Text;
            ss.EMailAdminIsHTML = chkAdminEMailTextIsHTML.Checked;
            ss.EMailClientIsHTML = chkClientEMailTextIsHTML.Checked;
            ss.EMailClientSubject = txtEMailClientSubject.Text;
            ss.EMailAdminSubject = txtEMailAdminSubject.Text;
            ss.EMailAdminScheduling = (SchedulerPlanning)lblAdminSched.Tag;
            ss.EMailClientScheduling = (SchedulerPlanning)lblClientSched.Tag;
            ss.AdminIPAddresses = txtAdminAccess.Text;
            ss.AdministratorName= txtAdminName.Text;
            ss.MessageDisclaimer = txtMessageDisclaimer.Text;

            if (Int64.TryParse(txtEventLogFlush.Text, out ss.KeepEventLogDays) == false)
            {
                MessageBox.Show(this, "Invalid Eventlog delete entry.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Int64.TryParse(txtKeepDisks.Text, out ss.KeepNonPresentDisks) == false)
            {
                MessageBox.Show(this, "Invalid Non-Present Disks delete entry.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Int64.TryParse(txtKeepReports.Text, out ss.KeepReports) == false)
            {
                MessageBox.Show(this, "Invalid Reports delete entry.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Int64.TryParse(txtKeepBitlockerRK.Text, out ss.KeepBitlockerRK) == false)
            {
                MessageBox.Show(this, "Invalid Bitlocker RK delete entry.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Int64.TryParse(txtKeepChatLog.Text, out ss.KeepChatLogs) == false)
            {
                MessageBox.Show(this, "Invalid Chat Log delete entry.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (int.TryParse(txtMailPort.Text, out ss.EMailPort) == false)
            {
                MessageBox.Show(this, "Invalid Mail Port.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Program.net.SetServerSettings(ss) == false)
            {
                MessageBox.Show(this, "Cannot save settings: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdSetScheduleAdmin_Click(object sender, EventArgs e)
        {
            frmSetupScheduler frm = new frmSetupScheduler((SchedulerPlanning)lblAdminSched.Tag, "Change admin notification plan");
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            lblAdminSched.Tag = frm.Plan;
            lblAdminSched.Text = Scheduler.Explain(frm.Plan);
        }

        private void cmdSetScheduleClient_Click(object sender, EventArgs e)
        {
            frmSetupScheduler frm = new frmSetupScheduler((SchedulerPlanning)lblClientSched.Tag, "Change client notification plan");
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;
            lblClientSched.Tag = frm.Plan;
            lblClientSched.Text = Scheduler.Explain(frm.Plan);
        }

        private void cmdRunAdminReportNow_Click(object sender, EventArgs e)
        {
            Program.net.RunAdminReportNow();
        }
    }
}
