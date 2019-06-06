using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class SyncStartups
    {
        static ListStartupItems CollectInfosReg(RegistryKey RegPart, string RegBranch, string RegBranchWOW32, string HKCUUser)
        {
            ListStartupItems data = new ListStartupItems();
            data.MachineID = SystemInfos.SysInfo.MachineID;
            data.Items = new List<StartupItem>();

            RegistryKey reg = RegPart.OpenSubKey(RegBranch, false);
            if (reg != null)
            {
                foreach (string key in reg.GetValueNames())
                {
                    StartupItem s = new StartupItem();
                    s.Location = "REG";
                    s.Key = key;
                    s.Item = Convert.ToString(reg.GetValue(key));
                    s.HKCUUser = HKCUUser;
                    if (string.IsNullOrWhiteSpace(s.Key) == true)
                        s.Key = "(Default)";
                    data.Items.Add(s);
                }

                reg.Close();
            }

            if (SystemInfos.SysInfo.CPU == "EM64T")
            {
                reg = RegPart.OpenSubKey(RegBranchWOW32, false);
                if (reg != null)
                {
                    foreach (string key in reg.GetValueNames())
                    {
                        StartupItem s = new StartupItem();
                        s.Location = "REG_WOW";
                        s.Key = key;
                        s.Item = Convert.ToString(reg.GetValue(key));
                        s.HKCUUser = HKCUUser;
                        if (string.IsNullOrWhiteSpace(s.Key) == true)
                            s.Key = "(Default)";
                        data.Items.Add(s);
                    }

                    reg.Close();
                }
            }

            return (data);
        }

        public static bool DoSyncStartups()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);

                Status.UpdateMessage(0, "Collecting startups (HKLM)");

                ListStartupItems Finaldata = new ListStartupItems();
                Finaldata.Items = new List<StartupItem>();
                Finaldata.MachineID = SystemInfos.SysInfo.MachineID;
                Finaldata.SIDUsers = new List<string>();

                ListStartupItems data = CollectInfosReg(Registry.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", "");
                if (data == null)
                    return (false);

                Finaldata.Items.AddRange(data.Items);

                foreach (string kvp in Userregistries.GetLoadedUserRegistries())
                {
                    Status.UpdateMessage(0, "Collecting startups (" + kvp + ")");

                    Finaldata.SIDUsers.Add(kvp);

                    data = CollectInfosReg(Registry.Users, kvp + "\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", kvp + "\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", kvp);
                    if (data == null)
                        continue;

                    Finaldata.Items.AddRange(data.Items);
                }

                Status.UpdateMessage(0, "Reporting startups");
                net.ReportStartups(Finaldata);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing Startups: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }
    }
}
