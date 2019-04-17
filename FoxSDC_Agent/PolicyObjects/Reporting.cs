using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    //not in use; a dummy to keep the Agent System shut!
    [PolicyObjectAttr(PolicyIDs.ReportingPolicy)]
    public class Reporting : IPolicyClass
    {
        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            return (true);
        }

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            return (true);
        }

        public bool FinaliseApplyPolicy()
        {
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
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            return (true);
        }
    }
}
