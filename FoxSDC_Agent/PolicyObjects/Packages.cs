using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    [PolicyObjectAttr(PolicyIDs.PackagePolicy)]
    class PackagesPolicy : IPolicyClass
    {
        static List<PackagePolicy> ToAdd;
        static List<PackagePolicy> ToRemove;
        public static List<PackagePolicy> ActivePackages;
        public static bool UpdatePackages = false;
        public static object ActivePackagesLock = new object();

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            PackagePolicy t = JsonConvert.DeserializeObject<PackagePolicy>(policy.PolicyObject.Data);
            ToAdd.Add(t);
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            PackagePolicy t = JsonConvert.DeserializeObject<PackagePolicy>(policy.PolicyObject.Data);
            ToRemove.Add(t);
            return (true);
        }

        public bool FinaliseApplyPolicy()
        {
            bool NeedUpdates = false;

            foreach (PackagePolicy ap in ToRemove)
            {
                lock (ActivePackagesLock)
                {
                    foreach (PackagePolicy r in ActivePackages)
                    {
                        if (ap.InstallUpdates == r.InstallUpdates && ap.OptionalInstallation == r.OptionalInstallation &&
                            ap.Packages.SequenceEqual(r.Packages) == true)
                        {
                            ActivePackages.Remove(r);

                            NeedUpdates = true;
                            break;
                        }
                    }
                }
            }

            foreach (PackagePolicy ap in ToAdd)
            {
                lock (ActivePackagesLock)
                {
                    ActivePackages.Add(ap);
                }
                NeedUpdates = true;
            }

            if (ActivePackages.Count == 0 && FilesystemData.LocalPackages.Count > 0)
                NeedUpdates = true;

            if (UpdatePackages == false && NeedUpdates == true)
                UpdatePackages = true;

            ToAdd = null;
            ToRemove = null;
            return (true);
        }

        public bool PreApplyPolicy()
        {
            ToAdd = new List<PackagePolicy>();
            ToRemove = new List<PackagePolicy>();
            lock (ActivePackagesLock)
            {
                if (ActivePackages == null)
                    ActivePackages = new List<PackagePolicy>();
            }
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            PackagePolicy t1 = JsonConvert.DeserializeObject<PackagePolicy>(oldpolicy.PolicyObject.Data);
            PackagePolicy t2 = JsonConvert.DeserializeObject<PackagePolicy>(newpolicy.PolicyObject.Data);

            ToAdd.Add(t2);
            ToRemove.Add(t1);

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

        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            return (true);
        }
    }
}
