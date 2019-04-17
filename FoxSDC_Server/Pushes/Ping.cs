using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class Pingy
    {
        [VulpesRESTfulRet("OK")]
        public NetBool OK;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/ping", "OK", "MachineID")]
        public RESTStatus Ping(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "ping";
            p.ReplyID = guid;

            PushServiceHelper.SendPushService(MachineID, p, 0);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 0, guid);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            OK = new NetBool();
            OK.Data = false;

            if (resp.Data.ToString()!="ok")
            {
                OK.Data = false;
                return (RESTStatus.Success);
            }

            OK.Data = true;

            return (RESTStatus.Success);
        }
    }
}

