using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    public class SyncPolicy
    {
        public static Int64 RequestCertPolicyID;
        public static Int64 RequestCertPolicyMessageID;
        public static byte[] RequestCertPolicyCERData;

        public static List<LoadedPolicyObject> ActivePolicy;

        public static bool DoSyncPolicy()
        {
            RequestCertPolicyID = 0;
            RequestCertPolicyMessageID = 0;
            RequestCertPolicyCERData = null;

            List<Int64> ProcessedPolicies = new List<long>();

            Network net;
            net = Utilities.ConnectNetwork(9);
            if (net == null)
                return (false);

            Status.UpdateMessage(9, "Downloading client settings");
            FoxEventLog.VerboseWriteEventLog("Downloading client settings", System.Diagnostics.EventLogEntryType.Information);
            ClientSettings settings = net.GetClientSettings();
            if (settings != null)
            {
                RegistryData.AdministratorName = settings.AdministratorName;
                RegistryData.MessageDisclaimer = settings.MessageDisclaimer;
            }

            Status.UpdateMessage(9, "Downloading policies");
            FoxEventLog.VerboseWriteEventLog("Downloading policies", System.Diagnostics.EventLogEntryType.Information);
            PolicyObjectListSigned policieslistsigned = net.GetPoliciesForComputer();
            List<PolicyObjectSigned> policies = policieslistsigned == null ? null : policieslistsigned.Items;
            if (policies == null)
            {
                FoxEventLog.VerboseWriteEventLog("Downloading policies - nix", System.Diagnostics.EventLogEntryType.Information);
                Status.UpdateMessage(9);
                net.CloseConnection();
                return (true);
            }

            if (FilesystemData.LoadedCertificates.Count > 0)
            {
                bool SignatureOK = false;
                foreach (FilesystemCertificateData cer in FilesystemData.LoadedCertificates)
                {
                    if (Certificates.Verify(policieslistsigned, cer.Certificate) == true)
                    {
                        SignatureOK = true;
                        break;
                    }
                }
                if (SignatureOK == false)
                {
                    FoxEventLog.WriteEventLog("Invalid signature for PolicyList - no policies will be processed.", System.Diagnostics.EventLogEntryType.Error);
                    net.CloseConnection();
                    return (true);
                }
            }
            if (RegistryData.Verbose == 1)
            {
                string data = "Got policy:\r\n";
                foreach (PolicyObjectSigned obj in policies)
                {
                    data += obj.Policy.Name + " [ID: " + obj.Policy.ID + " VER: " + obj.Policy.Version + "]\r\n";
                }
                FoxEventLog.VerboseWriteEventLog("Downloading policies " + data, System.Diagnostics.EventLogEntryType.Information);
            }

            if (FilesystemData.LoadedCertificates.Count > 0)
            {
                foreach (PolicyObjectSigned obj in policies)
                {
                    if (ApplicationCertificate.Verify(obj) == false)
                    {
                        FoxEventLog.WriteEventLog("One or more policies were tampered - no policies will be processed.", System.Diagnostics.EventLogEntryType.Error);
                        net.CloseConnection();
                        return (true);
                    }
                }
            }

            #region Certificate Checks

            foreach (PolicyObjectSigned obj in policies)
            {
                if (obj.Policy.Type == PolicyIDs.SignCertificate)
                {
                    if (FilesystemData.ContainsPolicy(obj.Policy, false, false) == true)
                        continue;
                    PolicyObjectSigned objj = net.GetPolicyObjectSigned(obj.Policy.ID);
                    //do not verify signing here - that won't work! - Fox
                    PolicySigningCertificates Cert = JsonConvert.DeserializeObject<PolicySigningCertificates>(objj.Policy.Data);
                    if (FilesystemData.ContainsLoadedCert(Convert.FromBase64String(Cert.UUCerFile)) == true)
                        continue;
                    bool sig = Certificates.Verify(Convert.FromBase64String(Cert.UUCerFile), Convert.FromBase64String(Cert.UUSignFile), InternalCertificate.Main);
                    if (sig == false)
                    {
                        RequestCertPolicyID = objj.Policy.ID;
                        RequestCertPolicyCERData = Convert.FromBase64String(Cert.UUCerFile);
                        string CN = Certificates.GetCN(Convert.FromBase64String(Cert.UUCerFile));
                        if (CN == null)
                        {
                            FoxEventLog.WriteEventLog("Invalid certificate from server (Policy ID=" + objj.Policy.ID.ToString() + " Name=" + objj.Policy.Name + ")", System.Diagnostics.EventLogEntryType.Error);
                            continue;
                        }
                        Status.RequestCertificateConfirm("The certificate with " + CN + " is not signed by Vulpes. This may that someone tampered the connection, or a false certificate is installed on the server.\nDo you want to continue, and trust this certificate?", RequestCertPolicyID);
                        RequestCertPolicyMessageID = Status.MessageID;
                        FoxEventLog.WriteEventLog("Got unsinged certificate (Policy ID=" + objj.Policy.ID.ToString() + " Name=" + objj.Policy.Name + " " + CN + ")", System.Diagnostics.EventLogEntryType.Warning);
                    }
                    else
                    {
                        string CN = Certificates.GetCN(Convert.FromBase64String(Cert.UUCerFile));
                        if (CN == null)
                        {
                            FoxEventLog.WriteEventLog("Invalid (Vulpes signed) certificate from server (Policy ID=" + objj.Policy.ID.ToString() + " Name=" + objj.Policy.Name + ")", System.Diagnostics.EventLogEntryType.Error);
                            continue;
                        }
                        FilesystemData.InstallCertificate(Convert.FromBase64String(Cert.UUCerFile));
                    }
                }
            }

            #endregion

            if (FilesystemData.LoadedCertificates.Count > 0)
            {
                foreach (PolicyObjectSigned obj in policies)
                {
                    if (FilesystemData.ContainsPolicy(obj.Policy, false, false) == true)
                    {
                        if (ProcessedPolicies.Contains(obj.Policy.ID) == false)
                            ProcessedPolicies.Add(obj.Policy.ID);
                        FilesystemData.UpdatePolicyOrder(obj.Policy, obj.Policy.Order);
                        continue;
                    }

                    PolicyObjectSigned objj = net.GetPolicyObjectSigned(obj.Policy.ID);
                    if (objj == null)
                    {
                        FoxEventLog.WriteEventLog("No data for policy - not applying (Policy ID=" + obj.Policy.ID.ToString() + " Name=" + obj.Policy.Name + ")", System.Diagnostics.EventLogEntryType.Error);
                        continue;
                    }
                    if (ApplicationCertificate.Verify(objj) == false)
                    {
                        FoxEventLog.WriteEventLog("Policy was tampered - not applying (Policy ID=" + objj.Policy.ID.ToString() + " Name=" + objj.Policy.Name + ")", System.Diagnostics.EventLogEntryType.Error);
                        continue;
                    }

                    if (FilesystemData.InstallPolicy(objj.Policy, obj.Policy.Order) == false)
                        continue;
                    if (ProcessedPolicies.Contains(obj.Policy.ID) == false)
                        ProcessedPolicies.Add(obj.Policy.ID);
                }

                List<LoadedPolicyObject> RemovePol = new List<LoadedPolicyObject>();

                foreach (LoadedPolicyObject lobj in FilesystemData.LoadedPolicyObjects)
                {
                    if (ProcessedPolicies.Contains(lobj.PolicyObject.ID) == false)
                        RemovePol.Add(lobj);
                }

                foreach (LoadedPolicyObject lobj in RemovePol)
                {
                    FilesystemData.DeletePolicy(lobj);
                }
            }

            net.CloseConnection();

            if (RequestCertPolicyID == 0)
                Status.UpdateMessage(9);

            FoxEventLog.VerboseWriteEventLog("Downloading policies - DONE", System.Diagnostics.EventLogEntryType.Information);
            return (true);
        }

        static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(PolicyObjectAttr), true).Length > 0)
                {
                    yield return (type);
                }
            }
        }

        public enum ApplyPolicyFunction
        {
            ApplySystem,
            ApplyUser,
            Uninstall
        }

        public static bool ApplyPolicy(ApplyPolicyFunction applymethod)
        {
            if (ActivePolicy == null)
                ActivePolicy = new List<LoadedPolicyObject>();

            FilesystemData.ReOrderPolicies();

            foreach (Type type in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
            {
                PolicyObjectAttr po = type.GetCustomAttribute<PolicyObjectAttr>();
                IPolicyClass instance = (IPolicyClass)Activator.CreateInstance(type);
                try
                {
                    instance.PreApplyPolicy();
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.WriteEventLog("SEH Error while finalising policy - Type 0x" + po.PolicyType.ToString("X"), System.Diagnostics.EventLogEntryType.Error);
                }
            }

            if (FilesystemData.UpdatePolicies != null)
            {
                foreach (KeyValuePair<LoadedPolicyObject, LoadedPolicyObject> kvp in FilesystemData.UpdatePolicies)
                {
                    foreach (Type type in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
                    {
                        PolicyObjectAttr po = type.GetCustomAttribute<PolicyObjectAttr>();
                        if (po.PolicyType == kvp.Key.PolicyObject.Type)
                        {
                            IPolicyClass instance = (IPolicyClass)Activator.CreateInstance(type);
                            try
                            {
                                instance.UpdatePolicy(kvp.Key, kvp.Value);
                            }
                            catch (Exception ee)
                            {
                                Debug.WriteLine(ee.ToString());
                                FoxEventLog.WriteEventLog("SEH Error while updating policy - (Policy ID=" + kvp.Value.PolicyObject.ID.ToString() + " Name=" + kvp.Value.PolicyObject.Name + ")", System.Diagnostics.EventLogEntryType.Error);
                            }
                            break;
                        }
                    }
                }
            }

            List<LoadedPolicyObject> RemovePol = new List<LoadedPolicyObject>();
            foreach (LoadedPolicyObject a in ActivePolicy)
            {
                bool PolicyFound = false;
                foreach (LoadedPolicyObject b in FilesystemData.LoadedPolicyObjects)
                {
                    if (a.PolicyObject.ID == b.PolicyObject.ID &&
                        a.PolicyObject.Type == b.PolicyObject.Type)
                    {
                        PolicyFound = true;
                        break;
                    }
                }
                if (PolicyFound == false)
                    RemovePol.Add(a);
            }

            foreach (LoadedPolicyObject a in RemovePol)
            {
                foreach (Type type in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
                {
                    PolicyObjectAttr po = type.GetCustomAttribute<PolicyObjectAttr>();
                    if (po.PolicyType == a.PolicyObject.Type)
                    {
                        IPolicyClass instance = (IPolicyClass)Activator.CreateInstance(type);
                        try
                        {
                            instance.RemovePolicy(a);
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());
                            FoxEventLog.WriteEventLog("SEH Error while removing policy - (Policy ID=" + a.PolicyObject.ID.ToString() + " Name=" + a.PolicyObject.Name + ")", System.Diagnostics.EventLogEntryType.Error);
                        }
                        ActivePolicy.Remove(a);
                        break;
                    }
                }
            }

            RemovePol = new List<LoadedPolicyObject>(); //empty out

            foreach (LoadedPolicyObject a in FilesystemData.LoadedPolicyObjects)
            {
                bool PolicyFound = false;
                foreach (LoadedPolicyObject b in ActivePolicy)
                {
                    if (a.PolicyObject.ID == b.PolicyObject.ID &&
                        a.PolicyObject.Version == b.PolicyObject.Version &&
                        a.PolicyObject.Type == b.PolicyObject.Type)
                    {
                        PolicyFound = true;
                        break;
                    }
                }
                if (PolicyFound == false)
                {
                    bool CanProcessPolicy = false;
                    bool FoundPolicyProcessor = false;

                    foreach (Type type in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
                    {
                        PolicyObjectAttr po = type.GetCustomAttribute<PolicyObjectAttr>();
                        if (po.PolicyType == a.PolicyObject.Type)
                        {
                            IPolicyClass instance = (IPolicyClass)Activator.CreateInstance(type);
                            try
                            {
                                instance.ApplyPolicy(a);
                            }
                            catch (Exception ee)
                            {
                                Debug.WriteLine(ee.ToString());
                                FoxEventLog.WriteEventLog("SEH Error while applying policy - (Policy ID=" + a.PolicyObject.ID.ToString() + " Name=" + a.PolicyObject.Name + ")", System.Diagnostics.EventLogEntryType.Error);
                            }
                            CanProcessPolicy = true;
                            FoundPolicyProcessor = true;
                            break;
                        }
                    }

                    if (CanProcessPolicy == true || FoundPolicyProcessor == true)
                        ActivePolicy.Add(a);

                    if (FoundPolicyProcessor == false)
                    {
                        FoxEventLog.WriteEventLog("Don't know how to process policy type 0x" + a.PolicyObject.Type.ToString("X") + " - (Policy ID=" + a.PolicyObject.ID.ToString() + " Name=" + a.PolicyObject.Name + ")", System.Diagnostics.EventLogEntryType.Warning);
                    }
                }
            }

            foreach (LoadedPolicyObject a in ActivePolicy)
            {
                foreach (Type type in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
                {
                    PolicyObjectAttr po = type.GetCustomAttribute<PolicyObjectAttr>();
                    if (po.PolicyType == a.PolicyObject.Type)
                    {
                        IPolicyClass instance = (IPolicyClass)Activator.CreateInstance(type);
                        try
                        {
                            instance.ApplyOrdering(a, a.PolicyObject.Order);
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());
                            FoxEventLog.WriteEventLog("SEH Error while reordering polcy - Type 0x" + po.PolicyType.ToString("X"), System.Diagnostics.EventLogEntryType.Error);
                        }
                    }
                }
            }

            foreach (Type type in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
            {
                PolicyObjectAttr po = type.GetCustomAttribute<PolicyObjectAttr>();
                IPolicyClass instance = (IPolicyClass)Activator.CreateInstance(type);
                try
                {
                    switch (applymethod)
                    {
                        case ApplyPolicyFunction.ApplySystem:
                            instance.FinaliseApplyPolicy();
                            break;
                        case ApplyPolicyFunction.ApplyUser:
                            instance.FinaliseApplyPolicyUserPart();
                            break;
                        case ApplyPolicyFunction.Uninstall:
                            instance.FinaliseUninstallProgramm();
                            break;
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.WriteEventLog("SEH Error while finalising policy (applymethod=" + applymethod.ToString() + ") - Type 0x" + po.PolicyType.ToString("X"), System.Diagnostics.EventLogEntryType.Error);
                }
            }

            FilesystemData.UpdatePolicies = null;

            if (applymethod == ApplyPolicyFunction.ApplySystem)
            {
                string ApplyUserSettingsApp = ProgramAgent.AppPath + "FoxSDC_ApplyUserSettings.exe";

                if (File.Exists(ApplyUserSettingsApp) == false)
                {
                    FoxEventLog.WriteEventLog("The file " + ApplyUserSettingsApp + " does not exist. User settings are not applied properly.", EventLogEntryType.Error);
                }
                else
                {
#if !DEBUG
                    if (ProgramAgent.CPP.VerifyEXESignature(ApplyUserSettingsApp) == false)
                    {
                        FoxEventLog.WriteEventLog("The file " + ApplyUserSettingsApp + " cannot be verified. User settings are not applied properly.", EventLogEntryType.Error);
                    }
                    else
#endif
                    {
                        try
                        {
                            Process.Start(ApplyUserSettingsApp, ""); //NT_AUTHORITY\SYSTEM
                        }
                        catch
                        {

                        }

                        try
                        {
                            ProgramAgent.CPP.StartAppAsUser(ApplyUserSettingsApp, ""); //all other logged in users
                        }
                        catch
                        {

                        }
                    }
                }
            }
            return (true);
        }
    }
}
