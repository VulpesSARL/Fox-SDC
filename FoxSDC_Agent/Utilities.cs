using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PolicyObjectAttr : Attribute
    {
        public int PolicyType;
        public PolicyObjectAttr(int PolicyType)
        {
            this.PolicyType = PolicyType;
        }
    }

    interface IPolicyClass
    {
        bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy);
        bool ApplyPolicy(LoadedPolicyObject policy);
        bool RemovePolicy(LoadedPolicyObject policy);
        bool FinaliseApplyPolicy();
        bool PreApplyPolicy();
        bool FinaliseApplyPolicyUserPart();
        bool FinaliseUninstallProgramm();
        bool ApplyOrdering(LoadedPolicyObject policy, Int64 Ordering);
    }


    class Utilities
    {
        public static ServerInfo ServerInfo = null;
        public static string URL = null;

        public static SecureString MakeSecString(string s)
        {
            SecureString ss = new SecureString();
            foreach (char c in s)
                ss.AppendChar(c);
            return (ss);
        }

        public static Network ConnectNetwork(int MessageChannel)
        {
            if (SystemInfos.ServerURL == ProgramAgent.VulpesURL)
            {
                if (SystemInfos.ContractID == "" || SystemInfos.ContractPassword == "")
                {
                    FoxEventLog.VerboseWriteEventLog("Missing contract data for Vulpes Server (" + ProgramAgent.VulpesURL + ")", System.Diagnostics.EventLogEntryType.Error);
                    return (null);
                }
            }

            Network net = new Network();
            if (MessageChannel > -1)
                Status.UpdateMessage(MessageChannel, "Connecting to server " + SystemInfos.ServerURL);
            FoxEventLog.VerboseWriteEventLog("Connecting to server " + SystemInfos.ServerURL, System.Diagnostics.EventLogEntryType.Information);
            try
            {
                ServerInfo = null;
                URL = SystemInfos.ServerURL;
                if (net.Connect(SystemInfos.ServerURL) == false)
                {
                    if (MessageChannel > -1)
                        Status.UpdateMessage(MessageChannel, "Connecting to server " + SystemInfos.ServerURL + " failed");
                    FoxEventLog.VerboseWriteEventLog("Connecting to server " + SystemInfos.ServerURL + " failed", System.Diagnostics.EventLogEntryType.Information);
                    return (null);
                }

                try
                {
                    SystemInfos.SysInfo.IsMeteredConnection = MeteredConnection.IsMeteredConnection(); //always update this
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    SystemInfos.SysInfo.IsMeteredConnection = null;
                }

                if (net.ComputerLogin(SystemInfos.SysInfo.MachineID, SystemInfos.PasswordID, SystemInfos.ContractID, SystemInfos.ContractPassword, SystemInfos.SysInfo) == false)
                {
                    if (net.LoginError == null)
                    {
                        if (MessageChannel > -1)
                            Status.UpdateMessage(MessageChannel, "Login failed: <no data> ( on server " + SystemInfos.ServerURL + ")");
                        FoxEventLog.VerboseWriteEventLog("Login failed: <no data> ( on server " + SystemInfos.ServerURL + ")", System.Diagnostics.EventLogEntryType.Information);
                    }
                    else
                    {
                        if (MessageChannel > -1)
                            Status.UpdateMessage(MessageChannel, "Login failed: " + net.LoginError.Error + " - " + net.LoginError.ErrorID.ToString() + " ( on server " + SystemInfos.ServerURL + ")");
                        FoxEventLog.VerboseWriteEventLog("Login failed: " + net.LoginError.Error + " - " + net.LoginError.ErrorID.ToString() + " ( on server " + SystemInfos.ServerURL + ")", System.Diagnostics.EventLogEntryType.Information);
                    }
                    net.CloseConnection();
                    return (null);
                }
                if (net.GetInfo() == true)
                {
                    ServerInfo = net.serverinfo;
                    URL = SystemInfos.ServerURL;
                }
                if (MessageChannel > -1)
                    Status.UpdateMessage(MessageChannel);
            }
            catch
            {
                FoxEventLog.VerboseWriteEventLog("Login failed - something really has gone wrong: (" + SystemInfos.ServerURL + ")", System.Diagnostics.EventLogEntryType.Information);
                if (MessageChannel > -1)
                    Status.UpdateMessage(MessageChannel);
                return (null);
            }
            return (net);
        }
    }
}
