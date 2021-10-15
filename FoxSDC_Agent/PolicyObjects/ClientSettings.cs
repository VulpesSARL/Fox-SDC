using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    [PolicyObjectAttr(PolicyIDs.ClientSettings)]
    public class ClientSettings : IPolicyClass
    {
        static List<ClientSettingsPolicy> ToAdd;
        static List<ClientSettingsPolicy> ToRemove;
        static List<ClientSettingsPolicy> ActivePolicies = new List<ClientSettingsPolicy>();
        static ClientSettingsPolicy RunningPolicy;

        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            ClientSettingsPolicy t = JsonConvert.DeserializeObject<ClientSettingsPolicy>(policy.PolicyObject.Data);
            foreach (ClientSettingsPolicy w in ToAdd)
            {
                if (policy.PolicyObject.ID == w.ID)
                {
                    w.Order = Ordering;
                }
            }
            foreach (ClientSettingsPolicy w in ActivePolicies)
            {
                if (policy.PolicyObject.ID == w.ID)
                {
                    w.Order = Ordering;
                }
            }
            return (true);
        }

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            ClientSettingsPolicy t = JsonConvert.DeserializeObject<ClientSettingsPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToAdd.Add(t);
            return (true);
        }

        void Merge()
        {
            List<ClientSettingsPolicy> rm = new List<ClientSettingsPolicy>();
            foreach (ClientSettingsPolicy ap in ToRemove)
            {
                foreach (ClientSettingsPolicy a in ActivePolicies)
                {
                    if (ap.ID == a.ID)
                        rm.Add(a);
                }
            }

            foreach (ClientSettingsPolicy r in rm)
            {
                ActivePolicies.Remove(r);
            }

            foreach (ClientSettingsPolicy a in ToAdd)
            {
                ActivePolicies.Add(a);
            }

            rm.Clear();
            ToAdd.Clear();
            ToRemove.Clear();

            ActivePolicies.Sort((x, y) => x.Order.CompareTo(y.Order));

            RunningPolicy = new ClientSettingsPolicy();
            foreach (ClientSettingsPolicy p in ActivePolicies)
            {
                if (p.DisableAddRemoveProgramsSync != null)
                    RunningPolicy.DisableAddRemoveProgramsSync = p.DisableAddRemoveProgramsSync;

                if (p.DisableDeviceManagerSync != null)
                    RunningPolicy.DisableDeviceManagerSync = p.DisableDeviceManagerSync;

                if (p.DisableDiskDataSync != null)
                    RunningPolicy.DisableDiskDataSync = p.DisableDiskDataSync;

                if (p.DisableEventLogSync != null)
                    RunningPolicy.DisableEventLogSync = p.DisableEventLogSync;

                if (p.DisableSimpleTasks != null)
                    RunningPolicy.DisableSimpleTasks = p.DisableSimpleTasks;

                if (p.DisableFilterDriverSync != null)
                    RunningPolicy.DisableFilterDriverSync = p.DisableFilterDriverSync;

                if (p.DisableNetadapterSync != null)
                    RunningPolicy.DisableNetadapterSync = p.DisableNetadapterSync;

                if (p.DisableWinLicenseSync != null)
                    RunningPolicy.DisableWinLicenseSync = p.DisableWinLicenseSync;

                if (p.EnableBitlockerRKSync != null)
                    RunningPolicy.EnableBitlockerRKSync = p.EnableBitlockerRKSync;

                if (p.DisableSMARTSync != null)
                    RunningPolicy.DisableSMARTSync = p.DisableSMARTSync;

                if (p.DisableUsersSync != null)
                    RunningPolicy.DisableUsersSync = p.DisableUsersSync;

                if (p.DisableStartupSync != null)
                    RunningPolicy.DisableStartupSync = p.DisableStartupSync;

                if (p.EnableAdditionalEventLogs != null)
                {
                    RunningPolicy.EnableAdditionalEventLogs = p.EnableAdditionalEventLogs;
                    if (RunningPolicy.EnableAdditionalEventLogs == true)
                        RunningPolicy.AdditionalEventLogs = p.AdditionalEventLogs;
                }
            }
        }

        void ApplyPolicy(ClientSettingsPolicy p)
        {
            if (p.DisableAddRemoveProgramsSync != null)
                RegistryData.DisableAddRemoveProgramsSync = p.DisableAddRemoveProgramsSync.Value;
            else
                RegistryData.DisableAddRemoveProgramsSync = false;

            if (p.DisableDeviceManagerSync != null)
                RegistryData.DisableDeviceManagerSync = p.DisableDeviceManagerSync.Value;
            else
                RegistryData.DisableDeviceManagerSync = false;

            if (p.DisableDiskDataSync != null)
                RegistryData.DisableDiskDataSync = p.DisableDiskDataSync.Value;
            else
                RegistryData.DisableDiskDataSync = false;

            if (p.DisableEventLogSync != null)
                RegistryData.DisableEventLogSync = p.DisableEventLogSync.Value;
            else
                RegistryData.DisableEventLogSync = false;

            if (p.DisableSimpleTasks != null)
                RegistryData.DisableSimpleTasks = p.DisableSimpleTasks.Value;
            else
                RegistryData.DisableSimpleTasks = false;

            if (p.DisableFilterDriverSync != null)
                RegistryData.DisableFilterDriverSync = p.DisableFilterDriverSync.Value;
            else
                RegistryData.DisableFilterDriverSync = false;

            if (p.DisableNetadapterSync != null)
                RegistryData.DisableNetadapterSync = p.DisableNetadapterSync.Value;
            else
                RegistryData.DisableNetadapterSync = false;

            if (p.DisableWinLicenseSync != null)
                RegistryData.DisableWinLicenseSync = p.DisableWinLicenseSync.Value;
            else
                RegistryData.DisableWinLicenseSync = false;

            if (p.EnableBitlockerRKSync != null)
                RegistryData.EnableBitlockerRKSync = p.EnableBitlockerRKSync.Value;
            else
                RegistryData.EnableBitlockerRKSync = false;

            if (p.DisableSMARTSync != null)
                RegistryData.DisableSMARTSync = p.DisableSMARTSync.Value;
            else
                RegistryData.DisableSMARTSync = false;

            if (p.DisableUsersSync != null)
                RegistryData.DisableUsersSync = p.DisableUsersSync.Value;
            else
                RegistryData.DisableUsersSync = false;

            if (p.DisableStartupSync != null)
                RegistryData.DisableStartupSync = p.DisableStartupSync.Value;
            else
                RegistryData.DisableStartupSync = false;

            if (p.EnableAdditionalEventLogs != null)
            {
                RegistryData.EnableAdditionalEventLogs = p.EnableAdditionalEventLogs.Value;
                if (p.EnableAdditionalEventLogs==true)
                {
                    RegistryData.AdditionalEventLogs = p.AdditionalEventLogs;
                }
            }
            else
                RegistryData.EnableAdditionalEventLogs = false;
        }

        public bool FinaliseApplyPolicy()
        {
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
            return (true);
        }

        public bool PreApplyPolicy()
        {
            ToAdd = new List<ClientSettingsPolicy>();
            ToRemove = new List<ClientSettingsPolicy>();
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            ClientSettingsPolicy t = JsonConvert.DeserializeObject<ClientSettingsPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToRemove.Add(t);
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            ClientSettingsPolicy t1 = JsonConvert.DeserializeObject<ClientSettingsPolicy>(oldpolicy.PolicyObject.Data);
            ClientSettingsPolicy t2 = JsonConvert.DeserializeObject<ClientSettingsPolicy>(newpolicy.PolicyObject.Data);

            t1.ID = oldpolicy.PolicyObject.ID;
            t2.ID = newpolicy.PolicyObject.ID;
            ToAdd.Add(t2);
            ToRemove.Add(t1);
            return (true);
        }
    }
}
