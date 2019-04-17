using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class TaskManager
    {
        [VulpesRESTfulRet("TaskData")]
        PushTaskManagerList TaskDataList;

        [VulpesRESTfulRet("OK")]
        NetBool OK;

        [VulpesRESTfulRet("SessionList")]
        PushRunningSessionList SessionList;

        [VulpesRESTfulRet("Res")]
        PushRunTaskResult Res;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/killtask", "OK", "MachineID")]
        public RESTStatus KillTask(SQLLib sql, NetInt32 PID, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "killtask";
            p.ReplyID = guid;
            p.AdditionalData1 = PID.Data.ToString();

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

            if (resp.Data.ToString() != "ok")
            {
                OK.Data = false;
                return (RESTStatus.Success);
            }

            OK.Data = true;
            return (RESTStatus.Success);
        }


        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/listtasks", "TaskData", "MachineID")]
        public RESTStatus TaskData(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "tasks";
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
                TaskDataList = JsonConvert.DeserializeObject<PushTaskManagerList>(resp.Data.ToString());
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
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/pushy/getsessions", "SessionList", "MachineID")]
        public RESTStatus GetSessions(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "getsessions";
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
                SessionList = JsonConvert.DeserializeObject<PushRunningSessionList>(resp.Data.ToString());
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
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/runtask", "Res", "MachineID")]
        public RESTStatus RunTask(SQLLib sql, PushRunTask Req, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "runtask";
            p.ReplyID = guid;
            p.AdditionalData1 = JsonConvert.SerializeObject(Req);

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
                Res = JsonConvert.DeserializeObject<PushRunTaskResult>(resp.Data.ToString());
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
