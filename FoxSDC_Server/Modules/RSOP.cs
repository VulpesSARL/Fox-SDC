using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class RSOP
    {

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/rsop", "", "")]
        public RESTStatus ReportRSOP(SQLLib sql, object RSOPData, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            //Keep older clients happy

            return (RESTStatus.Success);
        }
    }
}
