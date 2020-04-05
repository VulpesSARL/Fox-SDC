using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class RemoteNetworkConnectionLegacy
    {
        [VulpesRESTfulRet("NR1")]
        PushConnectNetworkResult res1;
        [VulpesRESTfulRet("NR2")]
        PushConnectNetworkResult res2;
        [VulpesRESTfulRet("NR3")]
        PushConnectNetworkResult res3;
        [VulpesRESTfulRet("NRD")]
        PushConnectNetworkData resd;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/connectremotegetdata", "NRD", "MachineID")]
        public RESTStatus PushNetworkDataPull(SQLLib sql, PushConnectNetworkData data, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string sess = Guid.NewGuid().ToString();
            data.data = null;

            PushData p = new PushData();
            p.Action = "netdatapull";
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
                resd = JsonConvert.DeserializeObject<PushConnectNetworkData>(resp.Data.ToString());
            }
            catch
            {
                ni.Error = "Faulty data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            return (RESTStatus.Success);
        }


        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/connectremotereq", "NR1", "MachineID")]
        public RESTStatus PushNetworkDataCreateConnection(SQLLib sql, PushConnectNetwork data, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string sess = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "netcreatedata";
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

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/connectremotedata", "NR2", "MachineID")]
        public RESTStatus PushNetworkDataPush(SQLLib sql, PushConnectNetworkData data, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string sess = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "netdata";
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
                res2 = JsonConvert.DeserializeObject<PushConnectNetworkResult>(resp.Data.ToString());
            }
            catch
            {
                ni.Error = "Faulty data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/connectremoteclosedata", "NR3", "MachineID")]
        public RESTStatus PushNetworkCloseData(SQLLib sql, PushConnectNetworkData data, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string sess = Guid.NewGuid().ToString();
            data.data = null;

            PushData p = new PushData();
            p.Action = "netclosedata";
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
