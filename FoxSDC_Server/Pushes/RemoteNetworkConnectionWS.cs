using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace FoxSDC_Server.Pushes
{
    class RemoteNetworkConnectionWS
    {
        [VulpesRESTfulRet("NR1")]
        PushConnectNetworkResult res1;
        [VulpesRESTfulRet("NR3")]
        PushConnectNetworkResult res3;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/wsconnectremotereq", "NR1", "MachineID")]
        public RESTStatus PushNetworkDataCreateConnection(SQLLib sql, PushConnectNetwork data, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string SessionConnectionGUID = RemoteNetworkConnectionWSCrosser.CreateSession(ni.Username, MachineID);
            data.SessionID = SessionConnectionGUID;

            string sess = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "netcreatedata2";
            p.ReplyID = sess;
            p.AdditionalData1 = JsonConvert.SerializeObject(data);

            PushServiceHelper.SendPushService(MachineID, p, 1);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 1, sess);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                RemoteNetworkConnectionWSCrosser.CloseSession(SessionConnectionGUID);
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
                RemoteNetworkConnectionWSCrosser.CloseSession(SessionConnectionGUID);
                return (RESTStatus.NoContent);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/wsconnectremoteclosedata", "NR3", "MachineID")]
        public RESTStatus PushNetworkCloseData(SQLLib sql, PushConnectNetworkData data, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            RemoteNetworkConnectionWSCrosser.CloseSession(data.GUID);

            string sess = Guid.NewGuid().ToString();
            data.data = null;

            PushData p = new PushData();
            p.Action = "netclosedata2";
            p.ReplyID = sess;
            p.AdditionalData1 = JsonConvert.SerializeObject(data);

            PushServiceHelper.SendPushService(MachineID, p, 1);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 1, sess);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            try
            {
                res3 = JsonConvert.DeserializeObject<PushConnectNetworkResult>(resp.Data.ToString());
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
