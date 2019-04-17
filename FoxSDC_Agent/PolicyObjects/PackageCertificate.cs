using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    [PolicyObjectAttr(PolicyIDs.PackageCertificate)]
    public class PackageCertificate : IPolicyClass
    {
        static List<PolicyPackageCertificates> ToAdd;
        static List<PolicyPackageCertificates> ToRemove;
        public static List<byte[]> ActivePackageCerts;
        public static object ActivePackageCertsLock = new object();

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            PolicyPackageCertificates t = JsonConvert.DeserializeObject<PolicyPackageCertificates>(policy.PolicyObject.Data);
            ToAdd.Add(t);
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            PolicyPackageCertificates t = JsonConvert.DeserializeObject<PolicyPackageCertificates>(policy.PolicyObject.Data);
            ToRemove.Add(t);
            return (true);
        }

        public bool FinaliseApplyPolicy()
        {
            foreach (PolicyPackageCertificates ap in ToRemove)
            {
                lock (ActivePackageCertsLock)
                {
                    byte[] d = Convert.FromBase64String(ap.UUCerFile);
                    for (int i = 0; i < ActivePackageCerts.Count; i++)
                    {
                        byte[] b = ActivePackageCerts[i];
                        if (b.SequenceEqual(d) == true)
                        {
                            ActivePackageCerts.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            foreach (PolicyPackageCertificates ap in ToAdd)
            {
                lock (ActivePackageCertsLock)
                {
                    byte[] d = Convert.FromBase64String(ap.UUCerFile);
                    bool HasData = false;
                    for (int i = 0; i < ActivePackageCerts.Count; i++)
                    {
                        byte[] b = ActivePackageCerts[i];
                        if (b.SequenceEqual(d) == true)
                        {
                            HasData = true;
                            break;
                        }
                    }

                    if (HasData == false)
                        ActivePackageCerts.Add(d);
                }
            }

            ToAdd = null;
            ToRemove = null;

            if (RegistryData.Verbose == 1)
            {
                string blahblah = "ActivePackageCerts:\n\n";
                lock (ActivePackageCertsLock)
                {
                    foreach(byte[] data in ActivePackageCerts)
                    {
                        blahblah += "0x";
                        foreach(byte b in data)
                        {
                            blahblah += b.ToString("X2");
                        }
                        blahblah += "\n";
                    }
                }
                FoxEventLog.VerboseWriteEventLog(blahblah, System.Diagnostics.EventLogEntryType.Information);
            }
            return (true);
        }

        public bool PreApplyPolicy()
        {
            ToAdd = new List<PolicyPackageCertificates>();
            ToRemove = new List<PolicyPackageCertificates>();
            lock (ActivePackageCertsLock)
            {
                if (ActivePackageCerts == null)
                    ActivePackageCerts = new List<byte[]>();
            }
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            PolicyPackageCertificates t1 = JsonConvert.DeserializeObject<PolicyPackageCertificates>(oldpolicy.PolicyObject.Data);
            PolicyPackageCertificates t2 = JsonConvert.DeserializeObject<PolicyPackageCertificates>(newpolicy.PolicyObject.Data);

            if (t1.UUCerFile == t2.UUCerFile)
                return (true);

            ToAdd.Add(t2);
            ToRemove.Add(t1);

            return (true);
        }

        public bool FinaliseApplyPolicyUserPart()
        {
            return (FinaliseApplyPolicy());
        }

        public bool FinaliseUninstallProgramm()
        {
            return (FinaliseApplyPolicy());
        }

        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            return (true);
        }
    }
}
