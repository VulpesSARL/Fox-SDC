using FoxSDC_Agent.Redirs;
using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    [PolicyObjectAttr(PolicyIDs.PortMapping)]
    public class PortMapper : IPolicyClass
    {
        static List<PortMappingPolicy> ToAdd;
        static List<PortMappingPolicy> ToRemove;
        List<int> PortUsages;

        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            //no ordering
            return (true);
        }

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            PortMappingPolicy t = JsonConvert.DeserializeObject<PortMappingPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToAdd.Add(t);
            return (true);
        }

        public bool FinaliseApplyPolicy()
        {
            PortMappings_Kernel.FinaliseApplyPolicy(ToAdd, ToRemove);
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
            ToAdd = new List<PortMappingPolicy>();
            ToRemove = new List<PortMappingPolicy>();
            PortUsages = new List<int>();
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            PortMappingPolicy t = JsonConvert.DeserializeObject<PortMappingPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToRemove.Add(t);
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            PortMappingPolicy t1 = JsonConvert.DeserializeObject<PortMappingPolicy>(oldpolicy.PolicyObject.Data);
            PortMappingPolicy t2 = JsonConvert.DeserializeObject<PortMappingPolicy>(newpolicy.PolicyObject.Data);

            t1.ID = oldpolicy.PolicyObject.ID;
            t2.ID = newpolicy.PolicyObject.ID;
            ToAdd.Add(t2);
            ToRemove.Add(t1);
            return (true);
        }
    }
}
