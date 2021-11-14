using FoxSDC_Common;
using FoxSDC_Server.ReportingSystem;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class ReportingThread
    {
        [VulpesRESTfulRet("Dummy")]
        public NetString Dummy;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/runadminnow", "Dummy", "")]
        public RESTStatus RunAdminNowREST(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            RunAdminNow = true;
            Dummy = new NetString();
            Dummy.Data = "OK";

            return (RESTStatus.Success);
        }

        static bool StopThread = false;
        static Thread RTT = null;
        static bool RunAdminNow = false;

#if !TXTREPORT
        const string ReportAttachementFilename = "Report.pdf";
#else
        const string ReportAttachementFilename = "Report.txt";
#endif

        class ConcernedMachineIDsClient
        {
            public string MachineID;
            public string EMail;
            public string ContractID;
        }

        public static void StartReportingThreads()
        {
            RTT = new Thread(new ThreadStart(RThread));
            RTT.Start();
        }

        static void Pause()
        {
            for (int i = 0; i < 60; i++)
            {
                Thread.Sleep(1000);
                if (StopThread == true)
                    break;
                if (RunAdminNow == true)
                    break;
            }
        }

        public static void StopThreads()
        {
            StopThread = true;
            if (RTT != null)
                RTT.Join();
        }

        static void RThread()
        {
            do
            {
                try
                {

                    using (SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for Reporting", 0))
                    {
                        if (sql == null)
                        {
                            Pause();
                            if (StopThread == true)
                                break;
                        }
                        string ErrorMessage;
                        if (MailSender.CheckConfig() == false)
                        {
                            Pause();
                            if (StopThread == true)
                                break;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(SettingsManager.Settings.EMailAdminTo) == true)
                        {
                            Pause();
                            if (StopThread == true)
                                break;
                            continue;
                        }

                        SqlDataReader dr;
                        List<string> ConcernedMachineIDs;

                        #region Normal Admin Report

                        if (SettingsManager.Settings.LastScheduleRanAdmin == null)
                        {
                            SettingsManager.Settings.LastScheduleRanAdmin = DateTime.UtcNow;
                            SettingsManager.SaveApplySettings2(sql, SettingsManager.Settings);
                        }
                        else
                        {
                            DateTime? Planned = Scheduler.GetNextRunDate(SettingsManager.Settings.LastScheduleRanAdmin.Value, SettingsManager.Settings.EMailAdminScheduling);
                            if (RunAdminNow == true)
                            {
                                Planned = DateTime.UtcNow.AddMinutes(-1);
                                RunAdminNow = false;
                            }

                            if (Planned != null)
                            {
                                if (Planned.Value < DateTime.UtcNow) //is in the past - run now (may also be a "miss")
                                {
                                    dr = sql.ExecSQLReader("Select distinct machineid from Reporting where (Flags & @f1)!=0 AND (Flags & @f2)=0",
                                        new SQLParam("@f1", ReportingFlags.ReportToAdmin),
                                        new SQLParam("@f2", ReportingFlags.AdminReported));

                                    ConcernedMachineIDs = new List<string>();

                                    while (dr.Read())
                                    {
                                        ConcernedMachineIDs.Add(Convert.ToString(dr["MachineID"]));
                                    }
                                    dr.Close();

                                    if (ConcernedMachineIDs.Count > 0)
                                    {
                                        byte[] PDFFile = RenderReport.RenderMachineReport(sql, ConcernedMachineIDs, null, null, ReportingFlagsPaper.ReportAdmin, "PDF");
                                        if (PDFFile == null)
                                        {
                                            FoxEventLog.WriteEventLog("Admin Report has no data.", System.Diagnostics.EventLogEntryType.Error);
                                        }
                                        else
                                        {
                                            string Text = SettingsManager.Settings.EMailAdminText;
                                            string Subject = SettingsManager.Settings.EMailAdminSubject;
                                            Text = Text.Replace("{URGENT}", "").
                                                Replace("{NMACHINESS}", ConcernedMachineIDs.Count == 1 ? "" : "s").
                                                Replace("{NMACHINES}", ConcernedMachineIDs.Count.ToString());
                                            Subject = Subject.Replace("{URGENT}", "").
                                                Replace("{NMACHINESS}", ConcernedMachineIDs.Count == 1 ? "" : "s").
                                                Replace("{NMACHINES}", ConcernedMachineIDs.Count.ToString());
                                            if (MailSender.SendEMailAdmin(Subject, Text, new List<System.Net.Mail.Attachment> { new System.Net.Mail.Attachment(new MemoryStream(PDFFile), ReportAttachementFilename) }, System.Net.Mail.MailPriority.Normal, out ErrorMessage, SettingsManager.Settings.EMailAdminIsHTML) == false)
                                            {
                                                FoxEventLog.WriteEventLog("Cannot send Admin E-Mail: " + ErrorMessage, System.Diagnostics.EventLogEntryType.Error);
                                            }
                                        }
                                    }

                                    SettingsManager.Settings.LastScheduleRanAdmin = DateTime.UtcNow;
                                    SettingsManager.SaveApplySettings2(sql, SettingsManager.Settings);
                                }
                            }
                            else
                            {
                                //update anyways
                                SettingsManager.Settings.LastScheduleRanAdmin = DateTime.UtcNow;
                                SettingsManager.SaveApplySettings2(sql, SettingsManager.Settings);
                            }
                        }

                        #endregion

                        #region Urgent Admin

                        dr = sql.ExecSQLReader("Select distinct machineid from Reporting where (Flags & @f1)!=0 AND (Flags & @f2)=0",
                            new SQLParam("@f1", ReportingFlags.UrgentForAdmin),
                            new SQLParam("@f2", ReportingFlags.UrgentAdminReported));

                        ConcernedMachineIDs = new List<string>();

                        while (dr.Read())
                        {
                            ConcernedMachineIDs.Add(Convert.ToString(dr["MachineID"]));
                        }
                        dr.Close();

                        if (ConcernedMachineIDs.Count > 0)
                        {
                            byte[] PDFFile = RenderReport.RenderMachineReport(sql, ConcernedMachineIDs, null, null, ReportingFlagsPaper.UrgentAdmin, "PDF");
                            if (PDFFile == null)
                            {
                                FoxEventLog.WriteEventLog("Urgent Admin Report has no data.", System.Diagnostics.EventLogEntryType.Error);
                            }
                            else
                            {
                                string Text = SettingsManager.Settings.EMailAdminText;
                                string Subject = SettingsManager.Settings.EMailAdminSubject;
                                Text = Text.Replace("{URGENT}", "Urgent ").
                                    Replace("{NMACHINESS}", ConcernedMachineIDs.Count == 1 ? "" : "s").
                                    Replace("{NMACHINES}", ConcernedMachineIDs.Count.ToString());
                                Subject = Subject.Replace("{URGENT}", "Urgent ").
                                    Replace("{NMACHINESS}", ConcernedMachineIDs.Count == 1 ? "" : "s").
                                    Replace("{NMACHINES}", ConcernedMachineIDs.Count.ToString());
                                if (MailSender.SendEMailAdmin(Subject, Text, new List<System.Net.Mail.Attachment> { new System.Net.Mail.Attachment(new MemoryStream(PDFFile), ReportAttachementFilename) }, System.Net.Mail.MailPriority.High, out ErrorMessage, SettingsManager.Settings.EMailAdminIsHTML) == false)
                                {
                                    FoxEventLog.WriteEventLog("Cannot send Urgent Admin E-Mail: " + ErrorMessage, System.Diagnostics.EventLogEntryType.Error);
                                }
                            }
                        }

                        #endregion

                        #region Normal Client Report

                        if (Settings.Default.UseContract == true)
                        {
                            if (SettingsManager.Settings.LastScheduleRanClient == null)
                            {
                                SettingsManager.Settings.LastScheduleRanClient = DateTime.UtcNow;
                                SettingsManager.SaveApplySettings2(sql, SettingsManager.Settings);
                            }
                            else
                            {
                                DateTime? Planned = Scheduler.GetNextRunDate(SettingsManager.Settings.LastScheduleRanClient.Value, SettingsManager.Settings.EMailClientScheduling);
                                if (Planned != null)
                                {
                                    if (Planned.Value < DateTime.UtcNow) //is in the past - run now (may also be a "miss")
                                    {
                                        dr = sql.ExecSQLReader(@"select ComputerAccounts.MachineID,ComputerAccounts.ContractID,EMail from ComputerAccounts
                                        inner join Contracts on Contracts.ContractID = ComputerAccounts.ContractID
                                        where Disabled = 0 and MachineID in (Select distinct machineid from Reporting where (Flags & @f1) != 0 AND(Flags & @f2) = 0) and EMail is not null and EMail !=''",
                                            new SQLParam("@f1", ReportingFlags.ReportToClient),
                                            new SQLParam("@f2", ReportingFlags.ClientReported));

                                        List<ConcernedMachineIDsClient> ConcernedMachineIDC = new List<ConcernedMachineIDsClient>();
                                        List<string> ContractIDs = new List<string>();

                                        while (dr.Read())
                                        {
                                            ConcernedMachineIDsClient m = new ConcernedMachineIDsClient();
                                            m.ContractID = Convert.ToString(dr["ContractID"]);
                                            m.MachineID = Convert.ToString(dr["MachineID"]);
                                            m.EMail = Convert.ToString(dr["EMail"]);
                                            ConcernedMachineIDC.Add(m);
                                            if (ContractIDs.Contains(m.ContractID) == false)
                                                ContractIDs.Add(m.ContractID);
                                        }
                                        dr.Close();

                                        foreach (string ContractID in ContractIDs)
                                        {
                                            List<string> ConcernedMachineIDsForClient = new List<string>();
                                            string EMail = "";
                                            foreach (ConcernedMachineIDsClient m in ConcernedMachineIDC)
                                            {
                                                if (m.ContractID == ContractID)
                                                    ConcernedMachineIDsForClient.Add(m.MachineID);
                                                if (string.IsNullOrWhiteSpace(EMail) == true)
                                                    EMail = m.EMail;
                                            }

                                            if (string.IsNullOrWhiteSpace(EMail) == true)
                                                continue;

                                            if (ConcernedMachineIDsForClient.Count > 0)
                                            {
                                                byte[] PDFFile = RenderReport.RenderMachineReport(sql, ConcernedMachineIDsForClient, null, null, ReportingFlagsPaper.ReportClient, "PDF");
                                                if (PDFFile == null)
                                                {
                                                    FoxEventLog.WriteEventLog("Client Report has no data.", System.Diagnostics.EventLogEntryType.Error);
                                                }
                                                else
                                                {
                                                    string Text = SettingsManager.Settings.EMailClientText;
                                                    string Subject = SettingsManager.Settings.EMailClientSubject;
                                                    Text = Text.Replace("{URGENT}", "").
                                                        Replace("{NMACHINESS}", ConcernedMachineIDsForClient.Count == 1 ? "" : "s").
                                                        Replace("{NMACHINES}", ConcernedMachineIDsForClient.Count.ToString());
                                                    Subject = Subject.Replace("{URGENT}", "").
                                                        Replace("{NMACHINESS}", ConcernedMachineIDsForClient.Count == 1 ? "" : "s").
                                                        Replace("{NMACHINES}", ConcernedMachineIDsForClient.Count.ToString());
                                                    if (MailSender.SendEMailClient(EMail, Subject, Text, new List<System.Net.Mail.Attachment> { new System.Net.Mail.Attachment(new MemoryStream(PDFFile), ReportAttachementFilename) }, System.Net.Mail.MailPriority.High, out ErrorMessage, SettingsManager.Settings.EMailClientIsHTML) == false)
                                                    {
                                                        FoxEventLog.WriteEventLog("Cannot send Client E-Mail: " + ErrorMessage, System.Diagnostics.EventLogEntryType.Error);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //update anyways
                                    SettingsManager.Settings.LastScheduleRanClient = DateTime.UtcNow;
                                    SettingsManager.SaveApplySettings2(sql, SettingsManager.Settings);
                                }
                            }
                        }

                        #endregion

                        #region Urgent Client

                        if (Settings.Default.UseContract == true)
                        {
                            dr = sql.ExecSQLReader(@"select ComputerAccounts.MachineID,ComputerAccounts.ContractID,EMail from ComputerAccounts
                                inner join Contracts on Contracts.ContractID = ComputerAccounts.ContractID
                                where Disabled = 0 and MachineID in (Select distinct machineid from Reporting where (Flags & @f1) != 0 AND(Flags & @f2) = 0) and EMail is not null and EMail !=''",
                                new SQLParam("@f1", ReportingFlags.UrgentForClient),
                                new SQLParam("@f2", ReportingFlags.UrgentClientReported));

                            List<ConcernedMachineIDsClient> ConcernedMachineIDC = new List<ConcernedMachineIDsClient>();
                            List<string> ContractIDs = new List<string>();

                            while (dr.Read())
                            {
                                ConcernedMachineIDsClient m = new ConcernedMachineIDsClient();
                                m.ContractID = Convert.ToString(dr["ContractID"]);
                                m.MachineID = Convert.ToString(dr["MachineID"]);
                                m.EMail = Convert.ToString(dr["EMail"]);
                                ConcernedMachineIDC.Add(m);
                                if (ContractIDs.Contains(m.ContractID) == false)
                                    ContractIDs.Add(m.ContractID);
                            }
                            dr.Close();

                            foreach (string ContractID in ContractIDs)
                            {
                                List<string> ConcernedMachineIDsForClient = new List<string>();
                                string EMail = "";
                                foreach (ConcernedMachineIDsClient m in ConcernedMachineIDC)
                                {
                                    if (m.ContractID == ContractID)
                                        ConcernedMachineIDsForClient.Add(m.MachineID);
                                    if (string.IsNullOrWhiteSpace(EMail) == true)
                                        EMail = m.EMail;
                                }

                                if (string.IsNullOrWhiteSpace(EMail) == true)
                                    continue;

                                if (ConcernedMachineIDsForClient.Count > 0)
                                {
                                    byte[] PDFFile = RenderReport.RenderMachineReport(sql, ConcernedMachineIDsForClient, null, null, ReportingFlagsPaper.UrgentClient, "PDF");
                                    if (PDFFile == null)
                                    {
                                        FoxEventLog.WriteEventLog("Urgent Client Report has no data.", System.Diagnostics.EventLogEntryType.Error);
                                    }
                                    else
                                    {
                                        string Text = SettingsManager.Settings.EMailClientText;
                                        string Subject = SettingsManager.Settings.EMailClientSubject;
                                        Text = Text.Replace("{URGENT}", "Urgent ").
                                            Replace("{NMACHINESS}", ConcernedMachineIDsForClient.Count == 1 ? "" : "s").
                                            Replace("{NMACHINES}", ConcernedMachineIDsForClient.Count.ToString());
                                        Subject = Subject.Replace("{URGENT}", "Urgent ").
                                            Replace("{NMACHINESS}", ConcernedMachineIDsForClient.Count == 1 ? "" : "s").
                                            Replace("{NMACHINES}", ConcernedMachineIDsForClient.Count.ToString());
                                        if (MailSender.SendEMailClient(EMail, Subject, Text, new List<System.Net.Mail.Attachment> { new System.Net.Mail.Attachment(new MemoryStream(PDFFile), ReportAttachementFilename) }, System.Net.Mail.MailPriority.High, out ErrorMessage, SettingsManager.Settings.EMailClientIsHTML) == false)
                                        {
                                            FoxEventLog.WriteEventLog("Cannot send Urgent Client E-Mail: " + ErrorMessage, System.Diagnostics.EventLogEntryType.Error);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                }
                catch (Exception ee)
                {
                    FoxEventLog.WriteEventLog("Cannot process reporting data\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                }

                Pause();
                if (StopThread == true)
                    break;
            } while (StopThread == false);
        }
    }
}
