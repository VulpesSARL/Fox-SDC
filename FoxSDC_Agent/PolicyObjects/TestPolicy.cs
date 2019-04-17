using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    [PolicyObjectAttr(PolicyIDs.Test)]
    class TestPolicy : IPolicyClass
    {
        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
#if DEBUG
            PolicyTesting t = JsonConvert.DeserializeObject<PolicyTesting>(policy.PolicyObject.Data);
            FoxEventLog.VerboseWriteEventLog("Testing: " + t.CheckBox1.ToString() + " " + t.CheckBox2.ToString() + " " + t.CheckBox3.ToString() + " " + t.Text, System.Diagnostics.EventLogEntryType.Information);
#endif
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
#if DEBUG
            PolicyTesting t = JsonConvert.DeserializeObject<PolicyTesting>(policy.PolicyObject.Data);
            FoxEventLog.VerboseWriteEventLog("REMOVEAL Testing: " + t.CheckBox1.ToString() + " " + t.CheckBox2.ToString() + " " + t.CheckBox3.ToString() + " " + t.Text, System.Diagnostics.EventLogEntryType.Information);
#endif
            return (true);
        }

        public bool FinaliseApplyPolicy()
        {
#if DEBUG
            FoxEventLog.VerboseWriteEventLog("TestPolicy.FinaliseApplyPolicy() called", System.Diagnostics.EventLogEntryType.Information);
#endif
            return (true);
        }

        public bool PreApplyPolicy()
        {
#if DEBUG
            FoxEventLog.VerboseWriteEventLog("TestPolicy.PreApplyPolicy() called", System.Diagnostics.EventLogEntryType.Information);
#endif
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
#if DEBUG
            PolicyTesting t = JsonConvert.DeserializeObject<PolicyTesting>(oldpolicy.PolicyObject.Data);
            FoxEventLog.VerboseWriteEventLog("[OLD] Testing: " + t.CheckBox1.ToString() + " " + t.CheckBox2.ToString() + " " + t.CheckBox3.ToString() + " " + t.Text, System.Diagnostics.EventLogEntryType.Information);
            t = JsonConvert.DeserializeObject<PolicyTesting>(newpolicy.PolicyObject.Data);
            FoxEventLog.VerboseWriteEventLog("[NEW] Testing: " + t.CheckBox1.ToString() + " " + t.CheckBox2.ToString() + " " + t.CheckBox3.ToString() + " " + t.Text, System.Diagnostics.EventLogEntryType.Information);
#endif
            return (true);
        }

        public bool FinaliseApplyPolicyUserPart()
        {
#if DEBUG
            FoxEventLog.VerboseWriteEventLog("TestPolicy.FinaliseApplyPolicyUserPart() called", System.Diagnostics.EventLogEntryType.Information);
#endif
            return (true);
        }

        public bool FinaliseUninstallProgramm()
        {
#if DEBUG
            FoxEventLog.VerboseWriteEventLog("TestPolicy.FinaliseApplyPolicyUserPart() called", System.Diagnostics.EventLogEntryType.Information);
#endif
            return (true);
        }

        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            return (true);
        }
    }
}
