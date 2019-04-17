using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class Screendata
    {
        [VulpesRESTfulRet("ScreenBuffer")]
        PushScreenData ScreenData;

        [VulpesRESTfulRet("PushRemoteRes")]
        NetBool Res;

        [VulpesRESTfulRet("PushRemoteRes2")]
        NetBool Res2;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/getscreenbuffer", "ScreenBuffer", "MachineID")]
        public RESTStatus GetScreenBuffer(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "getfullscreen";
            p.ReplyID = guid;

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
                ScreenData = JsonConvert.DeserializeObject<PushScreenData>(resp.Data.ToString());
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
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/getscreendelta", "ScreenBuffer", "MachineID")]
        public RESTStatus GetScreenDelta(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "getdeltascreen";
            p.ReplyID = guid;

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
                ScreenData = JsonConvert.DeserializeObject<PushScreenData>(resp.Data.ToString());
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
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/setmousedata", "PushRemoteRes", "MachineID")]
        public RESTStatus SetMouseData(SQLLib sql, PushMouseData md, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "setmousedata";
            p.ReplyID = guid;
            p.AdditionalData1 = JsonConvert.SerializeObject(md);

            PushServiceHelper.SendPushService(MachineID, p, 3);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 3, guid, Timeout: 240);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            try
            {
                Res = JsonConvert.DeserializeObject<NetBool>(resp.Data.ToString());
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
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/setkeyboarddata", "PushRemoteRes2", "MachineID")]
        public RESTStatus SetKeyboardData(SQLLib sql, PushKeyboardData kb, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "setkeyboarddata";
            p.ReplyID = guid;
            p.AdditionalData1 = JsonConvert.SerializeObject(kb);

            PushServiceHelper.SendPushService(MachineID, p, 3);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 3, guid, Timeout: 240);
            if (resp == null)
            {
                ni.Error = "No response";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.NoContent);
            }

            try
            {
                Res2 = JsonConvert.DeserializeObject<NetBool>(resp.Data.ToString());
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
