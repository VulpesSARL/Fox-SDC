using FoxSDC_Common;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    [PolicyObjectAttr(PolicyIDs.WSUS)]
    class WSUS : IPolicyClass
    {
        static List<WSUSPolicy> ToAdd;
        static List<WSUSPolicy> ToRemove;
        static List<WSUSPolicy> ActivePolicies = new List<WSUSPolicy>();
        static WSUSPolicy RunningPolicy;

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            WSUSPolicy t = JsonConvert.DeserializeObject<WSUSPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToAdd.Add(t);
            return (true);
        }

        void Merge()
        {
            List<WSUSPolicy> rm = new List<WSUSPolicy>();
            bool Removed = false;
            foreach (WSUSPolicy ap in ToRemove)
            {
                foreach (WSUSPolicy a in ActivePolicies)
                {
                    if (ap.ID == a.ID)
                    {
                        Removed = true;
                        rm.Add(a);
                    }
                }
            }

            foreach (WSUSPolicy r in rm)
            {
                ActivePolicies.Remove(r);
            }

            foreach (WSUSPolicy a in ToAdd)
            {
                ActivePolicies.Add(a);
            }

            rm.Clear();
            ToAdd.Clear();
            ToRemove.Clear();

            ActivePolicies.Sort((x, y) => x.Order.CompareTo(y.Order));

            RunningPolicy = new WSUSPolicy();
            foreach (WSUSPolicy p in ActivePolicies)
            {
                if (p.ConfigureWSUS == false)
                    RunningPolicy.ConfigureWSUS = false;
                if (p.ConfigureWSUS == true || p.ConfigureWSUS == null)
                {
                    if (p.SpecifyWUOptions == true)
                        RunningPolicy.WUOptions = p.WUOptions;
                    if (p.SpecifyWUOptions != null)
                        RunningPolicy.SpecifyWUOptions = p.SpecifyWUOptions;
                    if (p.InstallDuringMaintenance != null)
                        RunningPolicy.InstallDuringMaintenance = p.InstallDuringMaintenance;
                    if (p.SpecifyScheduleInstall == true)
                    {
                        RunningPolicy.ScheduleInstallDay = p.ScheduleInstallDay;
                        RunningPolicy.ScheduleInstallHour = p.ScheduleInstallHour;
                    }
                    if (p.SpecifyScheduleInstall != null)
                        RunningPolicy.SpecifyScheduleInstall = p.SpecifyScheduleInstall;
                    if (p.InstallMicrosoftUpdates != null)
                        RunningPolicy.InstallMicrosoftUpdates = p.InstallMicrosoftUpdates;
                    if (p.SpecifyWUServer == true)
                        RunningPolicy.WUServer = p.WUServer;
                    if (p.SpecifyWUServer != null)
                        RunningPolicy.SpecifyWUServer = p.SpecifyWUServer;
                    if (p.SpecifyStatusServer == true)
                        RunningPolicy.StatusServer = p.StatusServer;
                    if (p.SpecifyStatusServer != null)
                        RunningPolicy.SpecifyStatusServer = p.SpecifyStatusServer;
                    if (p.SpecifyClientSideTargeting == true)
                        RunningPolicy.Target = p.Target;
                    if (p.SpecifyClientSideTargeting != null)
                        RunningPolicy.SpecifyClientSideTargeting = p.SpecifyClientSideTargeting;
                    if (p.NoMSServer != null)
                        RunningPolicy.NoMSServer = p.NoMSServer;
                    if (p.SpecifyDetectionFreq == true)
                        RunningPolicy.DetectionFreq = p.DetectionFreq;
                    if (p.SpecifyDetectionFreq != null)
                        RunningPolicy.SpecifyDetectionFreq = p.SpecifyDetectionFreq;
                    if (p.DontAutoRestart != null)
                        RunningPolicy.DontAutoRestart = p.DontAutoRestart;
                    if (p.SpecifyAlwaysAutoRestart == true)
                        RunningPolicy.AlwaysAutoRestartDelay = p.AlwaysAutoRestartDelay;
                    if (p.SpecifyAlwaysAutoRestart != null)
                        RunningPolicy.SpecifyAlwaysAutoRestart = p.SpecifyAlwaysAutoRestart;
                    if (p.SpecifyDeadline == true)
                        RunningPolicy.DeadLine = p.DeadLine;
                    if (p.SpecifyDeadline != null)
                        RunningPolicy.SpecifyDeadline = p.SpecifyDeadline;
                    if (p.EnableDownloadMode == true)
                        RunningPolicy.DownloadMode = p.DownloadMode;
                    if (p.EnableDownloadMode != null)
                        RunningPolicy.EnableDownloadMode = p.EnableDownloadMode;
                    if (p.DisableDualScan != null)
                        RunningPolicy.DisableDualScan = p.DisableDualScan;
                    if (p.DontAutoRestartDuringActiveHours == true)
                    {
                        RunningPolicy.ActiveHoursFrom = p.ActiveHoursFrom;
                        RunningPolicy.ActiveHoursTo = p.ActiveHoursTo;
                    }
                    if (p.DontAutoRestartDuringActiveHours != null)
                        RunningPolicy.DontAutoRestartDuringActiveHours = p.DontAutoRestartDuringActiveHours;
                }
                if (p.ConfigureWSUS == true)
                {
                    RunningPolicy.ConfigureWSUS = true;
                    RunningPolicy.WUOptions = p.WUOptions;
                }
            }

            if (Removed == true && ActivePolicies.Count == 0)
            {
                RunningPolicy.ConfigureWSUS = false; //remove config
            }
        }

        void ApplyPolicy(WSUSPolicy p)
        {
            if (p.ConfigureWSUS == null)
                return;
            if (p.StatusServer == null)
                p.SpecifyStatusServer = null;
            if (p.WUServer == null)
                p.SpecifyWUServer = null;
            if (string.IsNullOrWhiteSpace(p.Target) == true)
                p.SpecifyClientSideTargeting = false;

            if (p.ConfigureWSUS == true)
            {
                using (RegistryKey regDOD = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\DeliveryOptimization"))
                {
                    if (regDOD == null || regDOD == null)
                    {
                        FoxEventLog.WriteEventLog("Cannot write Delivery Optimization Registry Settings", System.Diagnostics.EventLogEntryType.Error);
                        return;
                    }
                    if (p.EnableDownloadMode == true)
                        regDOD.SetValue("DODownloadMode", p.DownloadMode, RegistryValueKind.DWord);
                    else
                        regDOD.DeleteValue("DODownloadMode", false);
                }

                using (RegistryKey regWU = Registry.LocalMachine.CreateSubKey("Software\\Policies\\Microsoft\\Windows\\WindowsUpdate"))
                {
                    using (RegistryKey regAU = Registry.LocalMachine.CreateSubKey("Software\\Policies\\Microsoft\\Windows\\WindowsUpdate\\AU"))
                    {
                        if (regWU == null || regAU == null)
                        {
                            FoxEventLog.WriteEventLog("Cannot write Windows Update Registry Settings", System.Diagnostics.EventLogEntryType.Error);
                            return;
                        }
                        if (p.SpecifyWUOptions == true)
                            regAU.SetValue("AUOptions", p.WUOptions, RegistryValueKind.DWord);
                        else
                            regAU.DeleteValue("AUOptions", false);

                        if (p.InstallDuringMaintenance != null)
                            regAU.SetValue("AutomaticMaintenanceEnabled", p.InstallDuringMaintenance == true ? 1 : 0, RegistryValueKind.DWord);

                        if (p.SpecifyScheduleInstall == true)
                        {
                            regAU.SetValue("ScheduledInstallDay", p.ScheduleInstallDay, RegistryValueKind.DWord);
                            regAU.SetValue("ScheduledInstallTime", p.ScheduleInstallHour, RegistryValueKind.DWord);
                        }
                        if (p.SpecifyScheduleInstall == false)
                        {
                            regAU.DeleteValue("ScheduledInstallDay", false);
                            regAU.DeleteValue("ScheduledInstallTime", false);
                        }

                        if (p.InstallMicrosoftUpdates != null)
                            regAU.SetValue("AllowMUUpdateService", p.InstallMicrosoftUpdates == true ? 1 : 0, RegistryValueKind.DWord);
                        if (p.SpecifyWUServer != null)
                        {
                            regAU.SetValue("UseWUServer", p.SpecifyWUServer == true ? 1 : 0, RegistryValueKind.DWord);
                            if (p.SpecifyWUServer == true)
                            {
                                regWU.SetValue("WUServer", p.WUServer.Trim(), RegistryValueKind.String);
                                if (p.SpecifyStatusServer == true)
                                    regWU.SetValue("WUStatusServer", p.StatusServer.Trim(), RegistryValueKind.String);
                            }
                            else
                            {
                                regWU.DeleteValue("WUServer", false);
                                regWU.DeleteValue("WUStatusServer", false);
                            }
                        }
                        if (p.SpecifyClientSideTargeting != null)
                            regWU.SetValue("TargetGroupEnabled", p.SpecifyClientSideTargeting == true ? 1 : 0, RegistryValueKind.DWord);
                        if (p.SpecifyClientSideTargeting == true)
                            regWU.SetValue("TargetGroup", p.Target.Trim(), RegistryValueKind.String);
                        if (p.SpecifyClientSideTargeting == false)
                            regWU.DeleteValue("TargetGroup", false);
                        if (p.NoMSServer != null)
                            regWU.SetValue("DoNotConnectToWindowsUpdateInternetLocations", p.NoMSServer == true ? 1 : 0, RegistryValueKind.DWord);
                        if (p.SpecifyDetectionFreq != null)
                        {
                            regAU.SetValue("DetectionFrequencyEnabled", p.SpecifyDetectionFreq == true ? 1 : 0, RegistryValueKind.DWord);
                            if (p.SpecifyDetectionFreq == true)
                                regAU.SetValue("DetectionFrequency", p.DetectionFreq, RegistryValueKind.DWord);
                            if (p.SpecifyDetectionFreq == false)
                                regAU.DeleteValue("DetectionFrequency", false);
                        }
                        if (p.DontAutoRestart != null)
                            regAU.SetValue("NoAutoRebootWithLoggedOnUsers", p.DontAutoRestart == true ? 1 : 0, RegistryValueKind.DWord);
                        if (p.SpecifyAlwaysAutoRestart != null)
                        {
                            regAU.SetValue("AlwaysAutoRebootAtScheduledTime", p.SpecifyAlwaysAutoRestart == true ? 1 : 0, RegistryValueKind.DWord);
                            if (p.SpecifyAlwaysAutoRestart == true)
                                regAU.SetValue("AlwaysAutoRebootAtScheduledTime_Minutes", p.AlwaysAutoRestartDelay, RegistryValueKind.DWord);
                            else
                                regAU.DeleteValue("AlwaysAutoRebootAtScheduledTime_Minutes", false);
                        }
                        if (p.SpecifyDeadline != null)
                        {
                            regWU.SetValue("SetAutoRestartDeadline", p.SpecifyDeadline == true ? 1 : 0, RegistryValueKind.DWord);
                            if (p.SpecifyDeadline == true)
                                regWU.SetValue("AutoRestartDeadlinePeriodInDays", p.DeadLine, RegistryValueKind.DWord);
                            else
                                regWU.DeleteValue("AutoRestartDeadlinePeriodInDays", false);
                        }
                        if (p.DontAutoRestartDuringActiveHours != null)
                        {
                            regWU.SetValue("SetActiveHours", p.DontAutoRestartDuringActiveHours == true ? 1 : 0, RegistryValueKind.DWord);
                            if (p.DontAutoRestartDuringActiveHours == true)
                            {
                                regWU.SetValue("ActiveHoursStart", p.ActiveHoursFrom, RegistryValueKind.DWord);
                                regWU.SetValue("ActiveHoursEnd", p.ActiveHoursTo, RegistryValueKind.DWord);
                            }
                            else
                            {
                                regWU.DeleteValue("ActiveHoursStart", false);
                                regWU.DeleteValue("ActiveHoursEnd", false);
                            }
                        }
                        if (p.DisableDualScan != null)
                        {
                            regWU.SetValue("DisableDualScan", p.DisableDualScan == true ? 1 : 0, RegistryValueKind.DWord);
                        }
                        else
                        {
                            regWU.DeleteValue("DisableDualScan", false);
                        }
                    }
                }
            }
            if (p.ConfigureWSUS == false)
            {
                Registry.LocalMachine.DeleteSubKeyTree("Software\\Policies\\Microsoft\\Windows\\WindowsUpdate", false);
                Registry.LocalMachine.DeleteSubKeyTree("Software\\Policies\\Microsoft\\Windows\\DeliveryOptimization", false);
            }
        }

        public bool FinaliseApplyPolicy()
        {
            if (SystemInfos.SysInfo.RunningInWindowsPE == true)
                return (true);

            Merge();
            ApplyPolicy(RunningPolicy);

            return (true);
        }
        public bool FinaliseApplyPolicyUserPart()
        {
            return (true);
        }

        public bool FinaliseUninstallProgramm()
        {
            if (SystemInfos.SysInfo.RunningInWindowsPE == true)
                return (true);

            Merge();
            if (RunningPolicy.ConfigureWSUS != null)
            {
                WSUSPolicy p = new WSUSPolicy();
                p.ConfigureWSUS = false;
                ApplyPolicy(p);
            }

            return (true);
        }

        public bool PreApplyPolicy()
        {
            ToAdd = new List<WSUSPolicy>();
            ToRemove = new List<WSUSPolicy>();
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            WSUSPolicy t = JsonConvert.DeserializeObject<WSUSPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToRemove.Add(t);
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            WSUSPolicy t1 = JsonConvert.DeserializeObject<WSUSPolicy>(oldpolicy.PolicyObject.Data);
            WSUSPolicy t2 = JsonConvert.DeserializeObject<WSUSPolicy>(newpolicy.PolicyObject.Data);

            t1.ID = oldpolicy.PolicyObject.ID;
            t2.ID = newpolicy.PolicyObject.ID;
            ToAdd.Add(t2);
            ToRemove.Add(t1);
            return (true);
        }

        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            WSUSPolicy t = JsonConvert.DeserializeObject<WSUSPolicy>(policy.PolicyObject.Data);
            foreach (WSUSPolicy w in ToAdd)
            {
                if (policy.PolicyObject.ID == w.ID)
                {
                    w.Order = Ordering;
                }
            }
            foreach (WSUSPolicy w in ActivePolicies)
            {
                if (policy.PolicyObject.ID == w.ID)
                {
                    w.Order = Ordering;
                }
            }
            return (true);
        }
    }
}
