using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class Status
    {
        [VulpesRESTfulRet("Err")]
        ErrorInfo Err;

        [VulpesRESTfulRet("Info")]
        ServerInfo Info;

        [VulpesRESTfulRet("Settingss")]
        ServerSettings Settingss;

        [VulpesRESTfulRet("CliSettings")]
        ClientSettings ClientSettings;

        [VulpesRESTfulRet("WSURL")]
        NetString WSURL;

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/status/error", "Err", "")]
        public RESTStatus GetLastError(SQLLib sql, object foo, NetworkConnectionInfo ni)
        {
            Err = new ErrorInfo();
            Err.Error = ni.Error;
            Err.ErrorID = (int)ni.ErrorID;
            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/status/ping", "", "")]
        public RESTStatus Ping(SQLLib sql, object foo, NetworkConnectionInfo ni)
        {
            return (RESTStatus.NoContent);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/status/info", "Info", "")]
        public RESTStatus GetInfo(SQLLib sql, object foo, NetworkConnectionInfo ni)
        {
            Info = ni.ServerInfo;
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/status/settings", "Settingss", "")]
        public RESTStatus GetSettings(SQLLib sql, object foo, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            Settingss = SettingsManager.Settings;
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/status/settings", "", "")]
        public RESTStatus GetSettings(SQLLib sql, ServerSettings settings, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (SettingsManager.SaveApplySettings(sql, settings, ni) == false)
                    return (RESTStatus.Fail);
            }
            return (RESTStatus.NoContent);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/client/settings", "CliSettings", "")]
        public RESTStatus GetClientSettings(SQLLib sql, object foo, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            ClientSettings = new ClientSettings();
            ClientSettings.AdministratorName = SettingsManager.Settings.AdministratorName;
            ClientSettings.MessageDisclaimer = SettingsManager.Settings.MessageDisclaimer;
            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/client/websocketurl", "WSURL", "")]
        public RESTStatus GetWebsocketURL(SQLLib sql, object foo, NetworkConnectionInfo ni)
        {
            WSURL = new NetString() { Data = Settings.Default.WSPublishURL };
            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/writemessage", "", "")]
        public RESTStatus SendMessage(SQLLib sql, WriteMessage message, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (message == null)
                return (RESTStatus.Fail);
            if (string.IsNullOrWhiteSpace(message.Name) == true)
                return (RESTStatus.Fail);
            if (string.IsNullOrWhiteSpace(message.Subject) == true)
                return (RESTStatus.Fail);
            if (string.IsNullOrWhiteSpace(message.Text) == true)
                return (RESTStatus.Fail);

            ComputerData pc;

            lock (ni.sqllock)
            {
                pc = Computers.GetComputerDetail(sql, ni.Username);
            }

            string ErrorMessage;
            string Message;
            string Subject;

            Subject = "Support Request: " + message.Subject + " (" + pc.Computername + ")";
            Message = "Support Request\n";
            Message += "Computer:    " + pc.Computername + " (" + pc.GroupingPath + ")\n";
            Message += "Computer ID: " + pc.MachineID + "\n";
            Message += "Date:        " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "\n";
            Message += "Name:        " + message.Name + "\n";
            Message += "Subject:     " + message.Subject + "\n";
            Message += "IP Address:  " + pc.IPAddress + "\n";
            Message += "Version:     " + pc.AgentVersion + " / " + pc.AgentVersionID.ToString() + "\n";
            if (Settings.Default.UseContract == true)
                Message += "Contract ID: " + pc.ContractID + "\n";
            Message += "\n";
            Message += message.Text;

            System.Net.Mail.MailPriority prio = System.Net.Mail.MailPriority.Normal;
            switch (message.Priority)
            {
                case 0: prio = System.Net.Mail.MailPriority.Low; break;
                case 2: prio = System.Net.Mail.MailPriority.High; break;
            }

            if (MailSender.SendEMailAdmin(Subject, Message, prio, out ErrorMessage) == false)
            {
                FoxEventLog.WriteEventLog("Cannot send Support Request E-Mail: " + ErrorMessage, System.Diagnostics.EventLogEntryType.Error);
                return (RESTStatus.Fail);
            }

            return (RESTStatus.NoContent);
        }
    }
}
