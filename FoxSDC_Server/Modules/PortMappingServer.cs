using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using CookieCollection = WebSocketSharp.Net.CookieCollection;

namespace FoxSDC_Server.Modules
{
    class WS_ServerPortMappingConnection : WebSocketBehavior
    {
        string SessionID;
        Socket ConnectedSock;
        bool StopThread = false;
        Thread GetterThreadHandle = null;

        void GetterThread()
        {
            while (ConnectedSock.Connected == true && StopThread == false)
            {
                int av = ConnectedSock.Available;
                if (av == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                byte[] data = new byte[av];
                int av2 = ConnectedSock.Receive(data);
                if (av2 == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (av != av2)
                {
                    byte[] data2 = new byte[av2];
                    for (int i = 0; i < av2; i++)
                        data2[i] = data[i];
                    data = data2;
                }

                try
                {
                    this.Send(data);
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    try
                    {
                        this.Close();
                    }
                    catch
                    {

                    }
                    return;
                }
            }
        }

        protected override void OnOpen()
        {
            GetterThreadHandle = new Thread(new ThreadStart(GetterThread));
            GetterThreadHandle.Start();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            try
            {
                ConnectedSock.Close();
            }
            catch
            {

            }
            base.OnClose(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.RawData == null)
                return;
            ConnectedSock.Send(e.RawData);
        }

        public void InitThis(ref string SessionID, Socket ConnectedSock)
        {
            this.SessionID = SessionID;
            this.ConnectedSock = ConnectedSock;
            this.CookiesValidator = CookieValidator;
        }

        bool CookieValidator(CookieCollection req, CookieCollection res)
        {
            string InternalRunningSessionID = req["Agent-SessionID"].Value;
            if (string.IsNullOrWhiteSpace(InternalRunningSessionID) == true)
                return (false);
            NetworkConnectionInfo ni = NetworkConnection.GetSession(InternalRunningSessionID);
            if (ni == null)
                return (false);
            if (ni.ComputerLoggedIn == false)
                return (false);
            lock (RemoteNetworkConnectionWSCrosser.DictLock)
            {
                if (RemoteNetworkConnectionWSCrosser.Sessions.ContainsKey(SessionID) == false)
                    return (false);
                if (RemoteNetworkConnectionWSCrosser.Sessions[SessionID].MachineID != ni.Username)
                    return (false);
            }

            Debug.WriteLine("Cookie (AG-SPM) validated");
            return (true);
        }
    }

    class PortMappingServer
    {
        [VulpesRESTfulRet("Res")]
        public PushConnectNetworkResult Res;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/agent/wsserverportmappingconnect", "Res", "")]
        public RESTStatus ConnectWSServerMappingPort(SQLLib sql, NetInt64 ID, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            List<PolicyObject> pols = Policies.GetPolicyForComputerInternal(sql, ni.Username);
            PolicyObject FoundPolicy = null;
            foreach (PolicyObject p in pols)
            {
                if (p.ID == ID.Data && p.Type == PolicyIDs.PortMapping)
                {
                    FoundPolicy = p;
                    break;
                }
            }

            if (FoundPolicy == null)
            {
                ni.Error = "Not found";
                ni.ErrorID = ErrorFlags.NotAccepted;
                return (RESTStatus.Denied);
            }

            PortMappingPolicy pmp = JsonConvert.DeserializeObject<PortMappingPolicy>(Policies.GetPolicy(sql, FoundPolicy.ID).Data);

            IPAddress ip;
            IPHostEntry ipaddr;
            if (IPAddress.TryParse(pmp.ServerServer, out ip) == false)
            {
                ipaddr = Dns.GetHostEntry(pmp.ServerServer);
            }
            else
            {
                ipaddr = new IPHostEntry();
                ipaddr.AddressList = new IPAddress[] { ip };
            }

            if (ipaddr == null)
            {
                ni.Error = "Cannot resolve";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.Fail);
            }
            if (ipaddr.AddressList.Length == 0)
            {
                ni.Error = "Resolve - no data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.Fail);
            }

            Socket socket;
            try
            {
                socket = new Socket(ipaddr.AddressList[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipaddr.AddressList[0], pmp.ServerPort);
            }
            catch (Exception ee)
            {
                Debug.WriteLine("Cannot connect " + ee.ToString());
                ni.Error = "Resolve - no data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.Fail);
            }

            string SessionID = "";
            RemoteNetworkConnectionWSCrosser.CreateCustomAgentConnection<WS_ServerPortMappingConnection>(ni.Username, ref SessionID, i => i.InitThis(ref SessionID, socket));

            Res = new PushConnectNetworkResult();
            Res.ConnectedGUID = SessionID;
            Res.Result = 0;
            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/agent/wsserverportmappingclose", "Res", "")]
        public RESTStatus CloseWSServerMappingPort(SQLLib sql, NetString GUID, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            RemoteNetworkConnectionWSCrosser.CloseSession(ni.Username, GUID.Data);

            Res = new PushConnectNetworkResult();
            Res.Result = 0;
            return (RESTStatus.Success);
        }
    }
}
