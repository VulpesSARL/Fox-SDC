using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class StdIORedir
    {
        [VulpesRESTfulRet("Res")]
        Push_Stdio_StdOut res;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/pushstdiodata", "", "MachineID")]
        public RESTStatus PushSTDIOData(SQLLib sql, Push_Stdio_StdIn stdin, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (stdin == null)
            {
                ni.Error = "Missing data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            string sess = "stdio-" + stdin.SessionID;

            PushData p = new PushData();
            p.Action = "stdin";
            p.ReplyID = sess;
            p.AdditionalData1 = JsonConvert.SerializeObject(stdin);

            PushServiceHelper.SendPushService(MachineID, p, 0);

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/popstdiodata", "Res", "MachineID")]
        public RESTStatus PopSTDIOData(SQLLib sql, NetString StdIOSession, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (StdIOSession == null)
            {
                ni.Error = "Missing Session Data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(StdIOSession.Data) == true)
            {
                ni.Error = "Missing Session Data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            string sess = "stdio-" + StdIOSession.Data;
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 0, sess, true);
            if (resp == null)
            {
                res = new Push_Stdio_StdOut();
                res.SessionID = StdIOSession.Data;
                res.State = PushStdoutState.Timeout;
                return (RESTStatus.NoContent);
            }

            try
            {
                res = JsonConvert.DeserializeObject<Push_Stdio_StdOut>(resp.Data.ToString());
            }
            catch
            {
                res = new Push_Stdio_StdOut();
                res.SessionID = StdIOSession.Data;
                res.State = PushStdoutState.InternalError;
                return (RESTStatus.NoContent);
            }

            return (RESTStatus.Success);
        }
    }
}
