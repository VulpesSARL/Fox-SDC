using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class Services
    {
        [VulpesRESTfulRet("ListServices")]
        PushServicesInfoList ServicesList;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/listservices", "ListServices", "MachineID")]
        public RESTStatus ListServices(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "services";
            p.ReplyID = guid;

            PushServiceHelper.SendPushService(MachineID, p, 0);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 0, guid);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            try
            {
                ServicesList = JsonConvert.DeserializeObject<PushServicesInfoList>(resp.Data.ToString());
            }
            catch
            {
                ni.Error = "Faulty data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }
            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("ServiceState")]
        PushServiceControlState ServiceState;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/servicecontrol", "ServiceState", "MachineID")]
        public RESTStatus ServiceControl(SQLLib sql, PushServiceControlReq req, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "servicecontrol";
            p.ReplyID = guid;
            p.AdditionalData1 = JsonConvert.SerializeObject(req);

            PushServiceHelper.SendPushService(MachineID, p, 0);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 0, guid);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            try
            {
                ServiceState = JsonConvert.DeserializeObject<PushServiceControlState>(resp.Data.ToString());
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
