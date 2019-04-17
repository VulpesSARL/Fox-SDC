using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    //The some of the routines are found in SyncPolicy.DoSyncPolicy(), due special cases

    [PolicyObjectAttr(PolicyIDs.SignCertificate)]
    class SignCertificate : IPolicyClass
    {
        static List<byte[]> ToRemove;

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            PolicySigningCertificates t = JsonConvert.DeserializeObject<PolicySigningCertificates>(policy.PolicyObject.Data);
            ToRemove.Add(Convert.FromBase64String(t.UUCerFile));
            return (true);
        }

        public bool FinaliseApplyPolicy()
        {
            List<FilesystemCertificateData> remove = new List<FilesystemCertificateData>();
            foreach (byte[] rm in ToRemove)
            {
                foreach (FilesystemCertificateData cer in FilesystemData.LoadedCertificates)
                {
                    if (cer.Certificate.SequenceEqual(rm) == true)
                    {
                        remove.Add(cer);
                        break;
                    }
                }
            }

            foreach (FilesystemCertificateData cer in remove)
            {
                string signfile = cer.FSFilename.Substring(0, cer.FSFilename.Length - 3) + "sign";
                try
                {
                    File.Delete(cer.FSFilename);
                }
                catch { }
                try
                {
                    File.Delete(signfile);
                }
                catch { }
                FilesystemData.LoadedCertificates.Remove(cer);
            }

            ToRemove = null;

            return (true);
        }

        public bool PreApplyPolicy()
        {
            ToRemove = new List<byte[]>();
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            PolicySigningCertificates t = JsonConvert.DeserializeObject<PolicySigningCertificates>(oldpolicy.PolicyObject.Data);
            PolicySigningCertificates t2 = JsonConvert.DeserializeObject<PolicySigningCertificates>(newpolicy.PolicyObject.Data);
            if (t.UUCerFile == t2.UUCerFile)
                return (true);
            ToRemove.Add(Convert.FromBase64String(t.UUCerFile));
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
