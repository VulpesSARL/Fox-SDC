using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class ScreendataWS
    {
        [VulpesRESTfulRet("NR1")]
        PushConnectNetworkResult res1;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/wscreatescreenconnection", "NR1", "MachineID")]
        public RESTStatus WSCreateScreenConnection(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string SessionConnectionGUID = RemoteNetworkConnectionWSCrosser.CreateSession(ni.Username, MachineID);
            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "startwsscreen";
            p.ReplyID = guid;
            p.AdditionalData1 = SessionConnectionGUID;

            PushServiceHelper.SendPushService(MachineID, p, 2);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 2, guid, Timeout: 240);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            try
            {
                res1 = JsonConvert.DeserializeObject<PushConnectNetworkResult>(resp.Data.ToString());
            }
            catch
            {
                ni.Error = "Faulty data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }
            return (RESTStatus.Success);
        }
    }
}
