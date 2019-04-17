using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class Chat
    {
        [VulpesRESTfulRet("ChatRes")]
        NetBool Res;

        [VulpesRESTfulRet("GetPendingChats")]
        NetStringList GetPendingChats;

        [VulpesRESTfulRet("ChatList")]
        PushChatMessageList ChatList;

        [VulpesRESTfulRet("ChatList2")]
        PushChatMessageList ChatList2;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/chatmessage", "", "")]
        public RESTStatus SendChatMessageToServer(SQLLib sql, PushChatMessage ChatMessage, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string MachineID = ni.Username;

            lock (ni.sqllock)
            {
                Int64? ID = sql.InsertMultiDataID("Chats",
                    new SQLData("MachineID", MachineID),
                    new SQLData("DT", DateTime.UtcNow),
                    new SQLData("Read", 0),
                    new SQLData("ToClient", 0),
                    new SQLData("Name", ni.Name),
                    new SQLData("Text", ChatMessage.Text));
            }
            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/reports/getchatmessages", "ChatList2", "")]
        public RESTStatus GetPendingChatMessages(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string MachineID = ni.Username;

            ChatList2 = new PushChatMessageList();
            ChatList2.List = new List<PushChatMessage>();

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("select * from Chats where [Read]=0 AND MachineID=@m AND ToClient=1 ORDER BY DT ASC",
                    new SQLParam("@m", MachineID));
                while (dr.Read())
                {
                    PushChatMessage pch = new PushChatMessage();
                    pch.ID = Convert.ToInt64(dr["ID"]);
                    pch.DT = SQLLib.GetDTUTC(dr["DT"]);
                    pch.Name = Convert.ToString(dr["Name"]);
                    pch.Text = Convert.ToString(dr["Text"]);
                    ChatList2.List.Add(pch);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/reports/confirmchitchat", "", "ID")]
        public RESTStatus ConfirmChat(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 ID)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }
            string MachineID = ni.Username;

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE Chats SET [Read]=1 WHERE MachineID=@m AND ID=@id",
                    new SQLParam("@m", MachineID),
                    new SQLParam("@id", ID));
            }
            return (RESTStatus.Success);
        }


        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/getpendingchats", "GetPendingChats", "")]
        public RESTStatus GetPendingMachines(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            GetPendingChats = new NetStringList();
            GetPendingChats.Items = new List<string>();

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("Select DISTINCT MachineID FROM Chats WHERE [Read]=0 AND ToClient=0");
                while (dr.Read())
                {
                    GetPendingChats.Items.Add(Convert.ToString(dr["MachineID"]));
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/getpendingchatdata", "ChatList", "MachineID")]
        public RESTStatus GetPendingChatData(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            ChatList = new PushChatMessageList();
            ChatList.List = new List<PushChatMessage>();
            List<Int64> IDs = new List<long>();

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("select * from Chats where [Read]=0 AND MachineID=@m AND ToClient=0 ORDER BY DT ASC",
                new SQLParam("@m", MachineID));
                while (dr.Read())
                {
                    IDs.Add(Convert.ToInt64(dr["ID"]));
                    PushChatMessage pch = new PushChatMessage();
                    pch.ID = Convert.ToInt64(dr["ID"]);
                    pch.DT = SQLLib.GetDTUTC(dr["DT"]);
                    pch.Name = Convert.ToString(dr["Name"]);
                    pch.Text = Convert.ToString(dr["Text"]);
                    ChatList.List.Add(pch);
                }
                dr.Close();
            }

            foreach(Int64 id in IDs)
            {
                lock (ni.sqllock)
                {
                    sql.ExecSQL("UPDATE Chats SET [Read]=1 WHERE ID=@id",
                        new SQLParam("@id", id));
                }
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/pushy/sendchatmessage", "ChatRes", "MachineID")]
        public RESTStatus SendChatmessageToClient(SQLLib sql, PushChatMessage ChatMessage, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            string guid = Guid.NewGuid().ToString();

            Int64? ID = null;
            lock (ni.sqllock)
            {
                ID = sql.InsertMultiDataID("Chats",
                    new SQLData("MachineID", MachineID),
                    new SQLData("DT", DateTime.UtcNow),
                    new SQLData("Read", 0),
                    new SQLData("ToClient", 1),
                    new SQLData("Name", ni.Name),
                    new SQLData("Text", ChatMessage.Text));
            }
            if (ID != null)
                ChatMessage.ID = ID.Value;
            else
                ChatMessage.ID = 0;

            ChatMessage.DT = DateTime.UtcNow;
            ChatMessage.Name = ni.Name;

            PushData p = new PushData();
            p.Action = "chatmessage";
            p.ReplyID = guid;
            p.AdditionalData1 = JsonConvert.SerializeObject(ChatMessage);

            PushServiceHelper.SendPushService(MachineID, p, 10);
            PushDataResponse resp = PushServiceHelper.PopResponse(MachineID, 10, guid);
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
    }
}
