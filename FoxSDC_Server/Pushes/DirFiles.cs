using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class DirFiles
    {
        [VulpesRESTfulRet("Res")]
        NetStringList Lists;

        [VulpesRESTfulRet("FileRes")]
        NetInt32 FileRes;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/listfiles", "Res", "MachineID")]
        public RESTStatus ListFiles(SQLLib sql, PushDirListReq Req, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "listfiles";
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
                Lists = JsonConvert.DeserializeObject<NetStringList>(resp.Data.ToString());
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
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/checkfile", "FileRes", "MachineID")]
        public RESTStatus CheckFile(SQLLib sql, NetString Req, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            PushData p = new PushData();
            p.Action = "checkfile";
            p.ReplyID = guid;
            p.AdditionalData1 = Req.Data;

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
                FileRes = new NetInt32();
                if (Int32.TryParse(resp.Data.ToString(), out FileRes.Data) == false)
                {
                    ni.Error = "Faulty data";
                    ni.ErrorID = ErrorFlags.NoData;
                    return (RESTStatus.NoContent);
                }
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
