using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    static class SettingsManager
    {
        public static ServerSettings Settings = new ServerSettings();
        static bool GetBool(SQLLib sql, string SettingsName, bool DefaultValue = false)
        {
            string s = Convert.ToString(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]=@sn",
                new SQLParam("@sn", SettingsName)));
            if (s == null)
                return (DefaultValue);
            return (s.Trim() == "1" ? true : false);
        }

        static int GetInt(SQLLib sql, string SettingsName, int DefaultValue = 0)
        {
            string s = Convert.ToString(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]=@sn",
                new SQLParam("@sn", SettingsName)));
            if (s == null)
                return (DefaultValue);
            int o;
            if (int.TryParse(s, out o) == false)
                return (DefaultValue);
            return (o);
        }

        static Int64 GetInt64(SQLLib sql, string SettingsName, Int64 DefaultValue = 0)
        {
            string s = Convert.ToString(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]=@sn",
                new SQLParam("@sn", SettingsName)));
            if (s == null)
                return (DefaultValue);
            Int64 o;
            if (Int64.TryParse(s, out o) == false)
                return (DefaultValue);
            return (o);
        }

        static DateTime? GetDateTimex(SQLLib sql, string SettingsName)
        {
            string s = Convert.ToString(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]=@sn",
                new SQLParam("@sn", SettingsName)));
            if (string.IsNullOrWhiteSpace(s) == true)
                return (null);
            Int64 i;
            if (Int64.TryParse(s, out i) == false)
                return (null);
            return (DateTime.FromFileTimeUtc(i));
        }

        static string GetString(SQLLib sql, string SettingsName, String DefaultValue = "")
        {
            object oo = sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]=@sn",
                new SQLParam("@sn", SettingsName));
            if (oo == null)
                return (DefaultValue);
            return (Convert.ToString(oo));
        }

        static bool PutBool(SQLLib sql, string SettingsName, bool Value)
        {
            if (sql.ExecSQL("if exists(select * from Config where [key]=@key) update Config set [Value]=@value where [key]=@key else insert into Config values(@key,@value)",
                new SQLParam("@key", SettingsName),
                new SQLParam("@value", Value == true ? "1" : "0")) == false)
                return (false);
            return (true);
        }

        static bool PutInt(SQLLib sql, string SettingsName, int Value)
        {
            if (sql.ExecSQL("if exists(select * from Config where [key]=@key) update Config set [Value]=@value where [key]=@key else insert into Config values(@key,@value)",
                new SQLParam("@key", SettingsName),
                new SQLParam("@value", Value.ToString())) == false)
                return (false);
            return (true);
        }

        static bool PutString(SQLLib sql, string SettingsName, string Value)
        {
            if (sql.ExecSQL("if exists(select * from Config where [key]=@key) update Config set [Value]=@value where [key]=@key else insert into Config values(@key,@value)",
                new SQLParam("@key", SettingsName),
                new SQLParam("@value", Value)) == false)
                return (false);
            return (true);
        }

        static bool PutInt64(SQLLib sql, string SettingsName, Int64 Value)
        {
            if (sql.ExecSQL("if exists(select * from Config where [key]=@key) update Config set [Value]=@value where [key]=@key else insert into Config values(@key,@value)",
                new SQLParam("@key", SettingsName),
                new SQLParam("@value", Value.ToString())) == false)
                return (false);
            return (true);
        }

        static bool PutDateTimex(SQLLib sql, string SettingsName, DateTime? Value)
        {
            if (sql.ExecSQL("if exists(select * from Config where [key]=@key) update Config set [Value]=@value where [key]=@key else insert into Config values(@key,@value)",
                new SQLParam("@key", SettingsName),
                new SQLParam("@value", Value == null ? "" : Value.Value.ToFileTime().ToString())) == false)
                return (false);
            return (true);
        }

        public static void LoadSettings(SQLLib sql)
        {
            Settings.UseCertificate = GetString(sql, "UseCertificate");
            Settings.KeepEventLogDays = GetInt64(sql, "KeepEventLogDays");
            Settings.KeepBitlockerRK = GetInt64(sql, "KeepBitlockerRK");
            Settings.KeepNonPresentDisks = GetInt64(sql, "KeepNonPresentDisks");
            Settings.KeepReports = GetInt64(sql, "KeepReports");
            Settings.KeepChatLogs = GetInt64(sql, "KeepChatLogs");

            Settings.EMailAdminTo = GetString(sql, "EMailAdminTo");
            Settings.EMailFrom = GetString(sql, "EMailFrom");
            Settings.EMailFromFriendly = GetString(sql, "EMailFromFriendly");
            Settings.EMailPort = GetInt(sql, "EMailPort");
            Settings.EMailServer = GetString(sql, "EMailServer");
            Settings.EMailPassword = GetString(sql, "EMailPassword");
            Settings.EMailUsername = GetString(sql, "EMailUsername");
            Settings.EMailUseSSL = GetBool(sql, "EMailUseSSL");
            Settings.LastScheduleRanAdmin = GetDateTimex(sql, "LastScheduleRanAdmin");
            Settings.LastScheduleRanClient = GetDateTimex(sql, "LastScheduleRanClient");
            Settings.EMailAdminIsHTML = GetBool(sql, "EMailAdminIsHTML", false);
            Settings.EMailClientIsHTML = GetBool(sql, "EMailClientIsHTML", false);
            Settings.EMailAdminText = GetString(sql, "EMailAdminText", "{URGENT}Report - {NMACHINES} machine{NMACHINESS} affected");
            Settings.EMailClientText = GetString(sql, "EMailClientText", "{URGENT}Report for Client - {NMACHINES} machine{NMACHINESS} affected");
            Settings.EMailAdminSubject = GetString(sql, "EMailAdminSubject", "Fox SDC - {URGENT}Report");
            Settings.EMailClientSubject = GetString(sql, "EMailClientSubject", "Fox SDC - Client {URGENT}Report");
            Settings.AdminIPAddresses = GetString(sql, "AdminIPAddresses");
            Settings.AdministratorName = GetString(sql, "AdminstratorName", "Administrator");
            Settings.MessageDisclaimer = GetString(sql, "MessageDisclaimer");

            try
            {
                Settings.EMailAdminScheduling = JsonConvert.DeserializeObject<SchedulerPlanning>(GetString(sql, "EMailAdminSched"));
                if (Settings.EMailAdminScheduling == null)
                {
                    Settings.EMailAdminScheduling = Scheduler.Nix;
                    PutString(sql, "EMailAdminSched", JsonConvert.SerializeObject(Settings.EMailAdminScheduling));
                }
            }
            catch
            {
                Settings.EMailAdminScheduling = Scheduler.Nix;
                PutString(sql, "EMailAdminSched", JsonConvert.SerializeObject(Settings.EMailAdminScheduling));
            }

            try
            {
                Settings.EMailClientScheduling = JsonConvert.DeserializeObject<SchedulerPlanning>(GetString(sql, "EMailClientSched"));
                if (Settings.EMailClientScheduling == null)
                {
                    Settings.EMailClientScheduling = Scheduler.Nix;
                    PutString(sql, "EMailClientSched", JsonConvert.SerializeObject(Settings.EMailClientScheduling));
                }
            }
            catch
            {
                Settings.EMailClientScheduling = Scheduler.Nix;
                PutString(sql, "EMailClientSched", JsonConvert.SerializeObject(Settings.EMailClientScheduling));
            }
        }

        static public bool SaveApplySettings2(SQLLib sql, ServerSettings newsettings)
        {
            Settings.LastScheduleRanAdmin = newsettings.LastScheduleRanAdmin;
            Settings.LastScheduleRanClient = newsettings.LastScheduleRanClient;

            PutDateTimex(sql, "LastScheduleRanAdmin", Settings.LastScheduleRanAdmin);
            PutDateTimex(sql, "LastScheduleRanClient", Settings.LastScheduleRanClient);
            return (true);
        }

        static bool SaveApplySettings(SQLLib sql, ServerSettings newsettings)
        {
            if (Certificates.CertificateExists(newsettings.UseCertificate, System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine) == false)
                return (false);

            if (newsettings.EMailPort < 1 || newsettings.EMailPort > 65535)
                return (false);

            if (newsettings.KeepEventLogDays < 0)
                return (false);
            if (newsettings.KeepNonPresentDisks < 0)
                return (false);
            if (newsettings.KeepReports < 0)
                return (false);
            if (newsettings.KeepChatLogs < 0)
                return (false);
            if (newsettings.KeepBitlockerRK < 0)
                return (false);

            if (string.IsNullOrWhiteSpace(newsettings.AdministratorName) == true)
                newsettings.AdministratorName = "Administrator";

            if (string.IsNullOrWhiteSpace(newsettings.AdminIPAddresses) == true)
            {
                newsettings.AdminIPAddresses = "";
            }
            else
            {
                foreach (string s in newsettings.AdminIPAddresses.Split(','))
                {
                    if (s.Contains("/") == false)
                        return (false);
                    IPNetwork ip;
                    if (IPNetwork.TryParse(s, out ip) == false)
                        return (false);
                }
            }

            Settings.UseCertificate = newsettings.UseCertificate;
            Settings.KeepEventLogDays = newsettings.KeepEventLogDays;
            Settings.KeepBitlockerRK = newsettings.KeepBitlockerRK;
            Settings.KeepChatLogs = newsettings.KeepChatLogs;
            Settings.KeepNonPresentDisks = newsettings.KeepNonPresentDisks;
            Settings.EMailAdminTo = newsettings.EMailAdminTo;
            Settings.EMailFrom = newsettings.EMailFrom;
            Settings.EMailFromFriendly = newsettings.EMailFromFriendly;
            Settings.EMailPort = newsettings.EMailPort;
            Settings.EMailServer = newsettings.EMailServer;
            Settings.EMailUsername = newsettings.EMailUsername;
            Settings.EMailPassword = newsettings.EMailPassword;
            Settings.EMailUseSSL = newsettings.EMailUseSSL;
            Settings.EMailAdminScheduling = newsettings.EMailAdminScheduling;
            Settings.EMailClientScheduling = newsettings.EMailClientScheduling;
            Settings.EMailAdminIsHTML = newsettings.EMailAdminIsHTML;
            Settings.EMailClientIsHTML = newsettings.EMailClientIsHTML;
            Settings.EMailAdminText = newsettings.EMailAdminText;
            Settings.EMailClientText = newsettings.EMailClientText;
            Settings.EMailAdminSubject = newsettings.EMailAdminSubject;
            Settings.EMailClientSubject = newsettings.EMailClientSubject;
            Settings.AdminIPAddresses = newsettings.AdminIPAddresses;
            Settings.AdministratorName = newsettings.AdministratorName;
            Settings.KeepReports = newsettings.KeepReports;
            Settings.MessageDisclaimer = newsettings.MessageDisclaimer;

            PutString(sql, "UseCertificate", Settings.UseCertificate);
            PutInt64(sql, "KeepEventLogDays", Settings.KeepEventLogDays);
            PutInt64(sql, "KeepNonPresentDisks", Settings.KeepNonPresentDisks);
            PutInt64(sql, "KeepBitlockerRK", Settings.KeepBitlockerRK);
            PutInt64(sql, "KeepReports", Settings.KeepReports);
            PutInt64(sql, "KeepChatLogs", Settings.KeepChatLogs);
            PutString(sql, "EMailAdminTo", Settings.EMailAdminTo);
            PutString(sql, "EMailFrom", Settings.EMailFrom);
            PutString(sql, "EMailFromFriendly", Settings.EMailFromFriendly);
            PutInt(sql, "EMailPort", Settings.EMailPort);
            PutString(sql, "EMailServer", Settings.EMailServer);
            PutString(sql, "EMailUsername", Settings.EMailUsername);
            PutString(sql, "EMailPassword", Settings.EMailPassword);
            PutBool(sql, "EMailUseSSL", Settings.EMailUseSSL);
            PutBool(sql, "EMailAdminIsHTML", Settings.EMailAdminIsHTML);
            PutBool(sql, "EMailClientIsHTML", Settings.EMailClientIsHTML);
            PutString(sql, "EMailAdminText", Settings.EMailAdminText);
            PutString(sql, "EMailClientText", Settings.EMailClientText);
            PutString(sql, "EMailAdminSubject", Settings.EMailAdminSubject);
            PutString(sql, "EMailClientSubject", Settings.EMailClientSubject);
            PutString(sql, "AdminIPAddresses", Settings.AdminIPAddresses);
            PutString(sql, "AdminstratorName", Settings.AdministratorName);
            PutString(sql, "MessageDisclaimer", Settings.MessageDisclaimer);
            PutString(sql, "EMailAdminSched", JsonConvert.SerializeObject(Settings.EMailAdminScheduling));
            PutString(sql, "EMailClientSched", JsonConvert.SerializeObject(Settings.EMailClientScheduling));
            return (true);
        }

        public static bool SaveApplySettings(SQLLib sql, ServerSettings newsettings, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (false);
            }

            if (SaveApplySettings(sql, newsettings) == false)
            {
                ni.Error = "Invalid settings";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (false);
            }

            string ErrorReason;
            if (Utilities.TestSign(out ErrorReason) == false)
            {
                FoxEventLog.WriteEventLog("Cannot test-sign with the certificate " + SettingsManager.Settings.UseCertificate + ": " + ErrorReason, EventLogEntryType.Warning);
            }

            return (true);
        }
    }
}
