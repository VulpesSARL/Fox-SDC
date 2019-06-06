using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class SyncAddRemovePrograms
    {
        static ListAddRemoveApps CollectInfos(RegistryKey RegPart, string RegBranch, string RegBranchWOW32, string HKCUUser)
        {
            ListAddRemoveApps data = new ListAddRemoveApps();
            data.MachineID = SystemInfos.SysInfo.MachineID;
            data.Items = new List<AddRemoveApp>();

            RegistryKey reg = RegPart.OpenSubKey(RegBranch, false);
            if (reg != null)
            {
                foreach (string u in reg.GetSubKeyNames())
                {
                    RegistryKey ureg = reg.OpenSubKey(u, false);
                    if (ureg == null)
                        continue;

                    AddRemoveApp ara = new AddRemoveApp();
                    ara.ProductID = u;
                    ara.HKCUUser = HKCUUser;
                    ara.DisplayVersion = ureg.GetValue("DisplayVersion", "").ToString();
                    ara.Name = ureg.GetValue("DisplayName", "").ToString();
                    ara.UninstallString = ureg.GetValue("UninstallString", "").ToString();
                    if (ara.Name == "")
                        continue;
                    int msi = 0;
                    int.TryParse(ureg.GetValue("WindowsInstaller", "0").ToString(), out msi);
                    ara.IsMSI = msi == 1 ? true : false;
                    int.TryParse(ureg.GetValue("VersionMajor", "0").ToString(), out ara.VersionMajor);
                    int.TryParse(ureg.GetValue("VersionMinor", "0").ToString(), out ara.VersionMinor);
                    ara.IsWOWBranch = false;
                    int SystemComponent;
                    int.TryParse(ureg.GetValue("SystemComponent", "0").ToString(), out SystemComponent);
                    ara.IsSystemComponent = SystemComponent == 1 ? true : false;

                    int Language;
                    int.TryParse(ureg.GetValue("Language", 0).ToString(), out Language);
                    if (Language != 0)
                    {
                        try
                        {
                            CultureInfo ci = new CultureInfo(Language);
                            if (ci != null)
                            {
                                ara.DisplayLanguage = ci.EnglishName;
                                ara.Language = ci.Name;
                            }
                        }
                        catch
                        {

                        }
                    }

                    data.Items.Add(ara);
                    ureg.Close();
                }
                reg.Close();
            }

            if (SystemInfos.SysInfo.CPU == "EM64T")
            {
                reg = RegPart.OpenSubKey(RegBranchWOW32, false);
                if (reg != null)
                {
                    foreach (string u in reg.GetSubKeyNames())
                    {
                        RegistryKey ureg = reg.OpenSubKey(u, false);
                        if (ureg == null)
                            continue;

                        AddRemoveApp ara = new AddRemoveApp();
                        ara.ProductID = u;
                        ara.HKCUUser = HKCUUser;
                        ara.DisplayVersion = ureg.GetValue("DisplayVersion", "").ToString();
                        ara.Name = ureg.GetValue("DisplayName", "").ToString();
                        ara.UninstallString = ureg.GetValue("UninstallString", "").ToString();
                        if (ara.Name == "")
                            continue;
                        int msi = 0;
                        int.TryParse(ureg.GetValue("WindowsInstaller", "0").ToString(), out msi);
                        ara.IsMSI = msi == 1 ? true : false;
                        int.TryParse(ureg.GetValue("VersionMajor", "0").ToString(), out ara.VersionMajor);
                        int.TryParse(ureg.GetValue("VersionMinor", "0").ToString(), out ara.VersionMinor);
                        ara.IsWOWBranch = true;

                        int Language;
                        int.TryParse(ureg.GetValue("Language", 0).ToString(), out Language);
                        if (Language != 0)
                        {
                            try
                            {
                                CultureInfo ci = new CultureInfo(Language);
                                if (ci != null)
                                {
                                    ara.DisplayLanguage = ci.EnglishName;
                                    ara.Language = ci.Name;
                                }
                            }
                            catch
                            {

                            }
                        }

                        data.Items.Add(ara);
                        ureg.Close();
                    }
                    reg.Close();
                }
            }
            return (data);
        }

        public static bool DoSyncAddRemovePrograms()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);

                Status.UpdateMessage(0, "Collecting installed programs (HKLM)");

                ListAddRemoveApps Finaldata = new ListAddRemoveApps();
                Finaldata.Items = new List<AddRemoveApp>();
                Finaldata.MachineID = SystemInfos.SysInfo.MachineID;
                Finaldata.SIDUsers = new List<string>();

                ListAddRemoveApps data = CollectInfos(Registry.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", "");
                if (data == null)
                    return (false);
                if (data.Items != null)
                    Finaldata.Items.AddRange(data.Items);

                foreach (string kvp in Userregistries.GetLoadedUserRegistries())
                {
                    Status.UpdateMessage(0, "Collecting installed programs (" + kvp + ")");

                    Finaldata.SIDUsers.Add(kvp);

                    data = CollectInfos(Registry.Users, kvp + "\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", kvp + "\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", kvp);
                    if (data == null)
                        continue;

                    if (data.Items == null)
                        continue;

                    Finaldata.Items.AddRange(data.Items);
                }

                Status.UpdateMessage(0, "Reporting installed programs");
                net.ReportAddRemovePrograms(Finaldata);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing AddRemovePrograms: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }
    }
}
