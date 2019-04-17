using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class Clocky
    {
        [VulpesRESTfulRet("ClockData")]
        PushClock ClockData;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/clock", "ClockData", "MachineID")]
        public RESTStatus Clock(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "clock";
            p.ReplyID = guid;

            PushServiceHelper.SendPushService(MachineID, p, 0);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 0, guid);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            if (!(resp.Data is DateTime))
            {
                ni.Error = "Faulty data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            ClockData = new PushClock();
            ClockData.UTCDT = (DateTime)resp.Data;

            return (RESTStatus.Success);
        }
    }
}

