using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class MailSender
    {
        public static bool CheckConfig()
        {
            if (string.IsNullOrWhiteSpace(SettingsManager.Settings.EMailServer) == true)
                return (false);
            if (SettingsManager.Settings.EMailPort < 1 || SettingsManager.Settings.EMailPort > 65535)
                return (false);
            if (string.IsNullOrWhiteSpace(SettingsManager.Settings.EMailFrom) == true)
                return (false);
            return (true);
        }

        static SmtpClient GetSMTPCli()
        {
            SmtpClient cli = new SmtpClient(SettingsManager.Settings.EMailServer, SettingsManager.Settings.EMailPort);
            cli.EnableSsl = SettingsManager.Settings.EMailUseSSL;
            cli.UseDefaultCredentials = false;
            if (string.IsNullOrWhiteSpace(SettingsManager.Settings.EMailUsername) == false)
            {
                cli.Credentials = new NetworkCredential(SettingsManager.Settings.EMailUsername, SettingsManager.Settings.EMailPassword);
            }
            return (cli);
        }

        static MailMessage PrepMailMessage(string To)
        {
            MailAddress faddr;
            if (string.IsNullOrWhiteSpace(SettingsManager.Settings.EMailFromFriendly) == true)
                faddr = new MailAddress(SettingsManager.Settings.EMailFrom);
            else
                faddr = new MailAddress(SettingsManager.Settings.EMailFrom, SettingsManager.Settings.EMailFromFriendly);

            MailMessage mm = new MailMessage(faddr, new MailAddress(To));
            return (mm);
        }

        public static bool SendEMailAdmin(string Subject, string Body, out string ErrorMessage, bool BodyIsHTML = false)
        {
            return (SendEMailAdmin(Subject, Body, null, MailPriority.Normal, out ErrorMessage, BodyIsHTML));
        }

        public static bool SendEMailAdmin(string Subject, string Body, MailPriority Priority, out string ErrorMessage, bool BodyIsHTML = false)
        {
            return (SendEMailAdmin(Subject, Body, null, Priority, out ErrorMessage, BodyIsHTML));
        }

        public static bool SendEMailAdmin(string Subject, string Body, List<Attachment> Attachements, MailPriority Priority, out string ErrorMessage, bool BodyIsHTML)
        {
            ErrorMessage = "";

            if (CheckConfig() == false)
            {
                ErrorMessage = "Invalid Configuration";
                return (false);
            }

            if (string.IsNullOrWhiteSpace(SettingsManager.Settings.EMailAdminTo) == true)
            {
                ErrorMessage = "Invalid E-Mail Address";
                return (false);
            }

            SmtpClient cli = GetSMTPCli();
            MailMessage mm = PrepMailMessage(SettingsManager.Settings.EMailAdminTo);
            mm.Subject = Subject;
            mm.Body = Body;
            mm.BodyEncoding = Encoding.UTF8;
            mm.Priority = Priority;
            if (Attachements != null)
            {
                foreach (Attachment at in Attachements)
                {
                    mm.Attachments.Add(at);
                }
            }

            try
            {
                cli.Send(mm);
            }
            catch (Exception ee)
            {
                ErrorMessage = ee.Message;
            }
            return (true);
        }

        public static bool SendEMailClient(string To, string Subject, string Body, out string ErrorMessage, bool BodyIsHTML = false)
        {
            return (SendEMailClient(To, Subject, Body, null, MailPriority.Normal, out ErrorMessage, BodyIsHTML));
        }

        public static bool SendEMailClient(string To, string Subject, string Body, MailPriority Priority, out string ErrorMessage, bool BodyIsHTML = false)
        {
            return (SendEMailClient(To, Subject, Body, null, Priority, out ErrorMessage, BodyIsHTML));
        }

        public static bool SendEMailClient(string To, string Subject, string Body, List<Attachment> Attachements, MailPriority Priority, out string ErrorMessage, bool BodyIsHTML)
        {
            ErrorMessage = "";

            if (CheckConfig() == false)
            {
                ErrorMessage = "Invalid Configuration";
                return (false);
            }

            if (string.IsNullOrWhiteSpace(To) == true)
            {
                ErrorMessage = "Invalid E-Mail Address";
                return (false);
            }

            SmtpClient cli = GetSMTPCli();
            MailMessage mm = PrepMailMessage(To);
            mm.Subject = Subject;
            mm.Body = Body;
            mm.BodyEncoding = Encoding.UTF8;
            mm.Priority = Priority;
            if (Attachements != null)
            {
                foreach (Attachment at in Attachements)
                {
                    mm.Attachments.Add(at);
                }
            }

            try
            {
                cli.Send(mm);
            }
            catch (Exception ee)
            {
                ErrorMessage = ee.Message;
            }
            return (true);
        }
    }
}
