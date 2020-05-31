using FoxSDC_Common;
using Microsoft.Win32;
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

namespace FoxSDC_Agent.Redirs
{
    class PortMappings_Kernel
    {
        static List<PortMappings_ConnectionNode> Connections = new List<PortMappings_ConnectionNode>();

        static List<PortMappingPolicy> DeDup(List<PortMappingPolicy> lst)
        {
            List<PortMappingPolicy> newlst = new List<PortMappingPolicy>();

            foreach (PortMappingPolicy p in lst)
            {
                bool Found = false;
                foreach (PortMappingPolicy np in newlst)
                {
                    if (np.BindTo0000 == p.BindTo0000 && np.ClientPort == p.ClientPort &&
                        np.EditHOSTS == p.EditHOSTS && np.HOSTSEntry == p.HOSTSEntry &&
                        np.ID == p.ID && np.NoBindIfSDCServerIsDetected == p.NoBindIfSDCServerIsDetected)
                    {
                        Found = true;
                        break;
                    }
                }

                if (Found == false)
                {
                    newlst.Add(p);
                }
            }

            return (newlst);
        }

        public static void FinaliseApplyPolicy(List<PortMappingPolicy> Add, List<PortMappingPolicy> Remove)
        {
            Add = DeDup(Add);
            Remove = DeDup(Remove);

            foreach (PortMappingPolicy rm in Remove)
            {
                bool DontRemove = false;
                //check if this policy is in "add" (to prevent connection breaking)
                foreach (PortMappingPolicy ad in Add)
                {
                    if (rm.BindTo0000 == ad.BindTo0000 && rm.ClientPort == ad.ClientPort &&
                        rm.EditHOSTS == ad.EditHOSTS && rm.HOSTSEntry == ad.HOSTSEntry &&
                        rm.ID == ad.ID && rm.NoBindIfSDCServerIsDetected == ad.NoBindIfSDCServerIsDetected)
                    {
                        DontRemove = true;
                        break;
                    }
                }
                if (DontRemove == true)
                    continue;

                PortMappings_ConnectionNode RMC = null;

                foreach (PortMappings_ConnectionNode C in Connections)
                {
                    if (C.RunningPort == rm.ClientPort && C.PolicyID == rm.ID)
                    {
                        RMC = C;
                        break;
                    }
                }

                if (RMC != null)
                {
                    Connections.Remove(RMC);
                    RMC.StopConnection();
                }
            }

            for (int i = 0; i < Add.Count; i++)
            {
                bool BadPort = false;
                PortMappingPolicy ad = Add[i];
                for (int j = 0; j < Add.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (ad.ClientPort == Add[j].ClientPort)
                    {
                        FoxEventLog.WriteEventLog("Portmapping: Port " + ad.ClientPort + " conflicts with Policy ID " + ad.ID + " and " + Add[j].ID, EventLogEntryType.Error);
                        BadPort = true;
                        break;
                    }
                }
                if (BadPort == true)
                    continue;

                foreach (PortMappings_ConnectionNode cc in Connections)
                {
                    if (ad.ID == cc.PolicyID)
                    {
                        if (cc.PolicyData.BindTo0000 == ad.BindTo0000 && cc.PolicyData.ClientPort == ad.ClientPort &&
                            cc.PolicyData.EditHOSTS == ad.EditHOSTS && cc.PolicyData.HOSTSEntry == ad.HOSTSEntry &&
                            cc.PolicyData.NoBindIfSDCServerIsDetected == ad.NoBindIfSDCServerIsDetected)
                        {
                            //no changes - continue
                            BadPort = true;
                            break;
                        }
                        else
                        {
                            //remove connection
                            Connections.Remove(cc);
                            cc.StopConnection();
                            break;
                        }
                    }
                }
                if (BadPort == true)
                    continue;

                if (NetworkUtilities.PortAvailable(ad.ClientPort) == false)
                {
                    FoxEventLog.WriteEventLog("Portmapping: Port " + ad.ClientPort + " from Policy ID " + ad.ID + " is unavailable.", EventLogEntryType.Error);
                    continue;
                }

                if (ad.NoBindIfSDCServerIsDetected == true)
                {
                    bool FoundSDCS = false;
                    using (RegistryKey reg = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\FoxSDCS"))
                    {
                        if (reg != null)
                            FoundSDCS = true;
                    }
                    if (FoundSDCS == true)
                        continue;
                }

                if (ad.EditHOSTS == true)
                {
                    if (string.IsNullOrWhiteSpace(ad.HOSTSEntry) == false)
                    {
                        HostsEdit.AppendIntoHOSTSFile(ad.HOSTSEntry, "127.0.0.1");
                    }
                }

                PortMappings_ConnectionNode C = new PortMappings_ConnectionNode(ad);
                C.StartConnection();
                Connections.Add(C);
            }
        }

        public static void StopAllConnections()
        {
            foreach (PortMappings_ConnectionNode c in Connections)
            {
                c.StopConnection();
            }
        }
    }

    class PortMappings_ConnectionNode
    {
        Thread ListenerThread;
        Socket Sock;
        bool StopThread = false;
        Network net;
        List<NetworkConnectionThreadPasser> Connections = new List<NetworkConnectionThreadPasser>();
        PortMappingPolicy Mapping;
        ManualResetEvent connectedevent = new ManualResetEvent(false);
        Socket NextConnected;
        bool NewBeginAccept = true;

        public PortMappingPolicy PolicyData
        {
            get
            {
                return (Mapping);
            }
        }

        public int RunningPort
        {
            get
            {
                return (Mapping.ClientPort);
            }
        }

        public Int64 PolicyID
        {
            get
            {
                return (Mapping.ID);
            }
        }

        public PortMappings_ConnectionNode(PortMappingPolicy Mapping)
        {
            this.Mapping = Mapping;
        }

        class NetworkConnectionThreadPasser
        {
            public Network net;
            public Socket sock;
            public string GUID;
            public WebSocket ws;
            public string URL;
            public Thread RunningConnectionAccepterAndGetterThread;
        }

        void CloseAll(string GUID, NetworkConnectionThreadPasser p)
        {
            Debug.WriteLine("WS SOCKET: " + GUID + " Close connection");
            try
            {
                p.ws.Close();
            }
            catch
            {

            }

            try
            {
                p.net.CloseWSServerPortMappingConnection(GUID);
            }
            catch
            {

            }

            try
            {
                p.sock.Close();
            }
            catch
            {

            }
        }

        void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.RawData == null)
                return;
            WebSocket ws = sender as WebSocket;
            foreach (NetworkConnectionThreadPasser ntp in Connections)
            {
                if (ntp.URL == ws.Url.ToString())
                {
                    ntp.sock.Send(e.RawData);
                    break;
                }
            }
        }

        void RunningConnectionAccepterAndGetter(object s)
        {
            NetworkConnectionThreadPasser p = (NetworkConnectionThreadPasser)s;

            string WSURL = p.net.GetWebsocketURL();

            PushConnectNetworkResult res = p.net.CreateWSServerPortMappingConnection(Mapping.ID);
            if (res == null)
            {
                p.sock.Close();
                return;
            }

            if (res.Result != 0)
            {
                p.sock.Close();
                return;
            }

            Debug.WriteLine("WS SOCKET: " + res.ConnectedGUID + " Create connection");
            p.URL = WSURL + "websocket/agent-" + Uri.EscapeUriString(res.ConnectedGUID);
            Debug.WriteLine("WS URL: " + p.URL);

            Connections.Add(p);
            p.ws = new WebSocket(p.URL);
            p.ws.OnMessage += Ws_OnMessage;
            p.ws.SetCookie(new WebSocketSharp.Net.Cookie("Agent-SessionID", net.Session));
            p.ws.Connect();

            DateTime pingtestdt = DateTime.Now;

            while (p.sock.Connected == true && StopThread == false)
            {
                int av = p.sock.Available;
                if (av == 0)
                {
                    if (pingtestdt.AddMinutes(1) < DateTime.Now)
                    {
                        pingtestdt = DateTime.Now;
                        p.ws.Ping();
                        if (p.ws.IsAlive == false)
                            break;
                    }
                    Thread.Sleep(10);
                    continue;
                }
                byte[] data = new byte[av];
                int av2 = p.sock.Receive(data);
                if (av2 == 0)
                {
                    if (pingtestdt.AddMinutes(1) < DateTime.Now)
                    {
                        pingtestdt = DateTime.Now;
                        p.ws.Ping();
                        if (p.ws.IsAlive == false)
                            break;
                    }
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

                if (pingtestdt.AddMinutes(1) < DateTime.Now)
                {
                    pingtestdt = DateTime.Now;
                    p.ws.Ping();
                    if (p.ws.IsAlive == false)
                        break;
                }

                try
                {
                    if (p.ws.IsAlive == false)
                        break;
                    p.ws.Send(data);
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    CloseAll(res.ConnectedGUID, p);
                    return;
                }
            }

            CloseAll(res.ConnectedGUID, p);
        }

        void ServerConnectThread()
        {
            int Counter = 0;

            do
            {
                try
                {
                    if (net == null)
                    {
                        net = Utilities.ConnectNetwork(9);
                        if (net == null)
                        {
                            Counter = 0;
#if DEBUG
                            while (Counter < 30)
#else
                            while (Counter < 120)
#endif
                            {
                                Thread.Sleep(1000);
                                Counter++;
                                if (StopThread == true)
                                    return;
                            }
                        }
                    }
                    else
                    {
                        if (net.Ping() == false)
                        {
                            net = null;
                        }
                        else
                        {
                            Counter = 0;
#if DEBUG
                            while (Counter < 30)
#else
                            while (Counter < 120)
#endif
                            {
                                Thread.Sleep(1000);
                                Counter++;
                                if (StopThread == true)
                                    return;
                            }
                        }
                    }
                    if (StopThread == true)
                        return;
                }
                catch
                {
                    net = null;
                    Counter = 0;
#if DEBUG
                    while (Counter < 30)
#else
                    while (Counter < 120)
#endif
                    {
                        Thread.Sleep(1000);
                        Counter++;
                        if (StopThread == true)
                            return;
                    }
                }
            } while (StopThread == false);
        }

        void ListenerThreadFn()
        {
            Sock.Listen((int)SocketOptionName.MaxConnections);
            int Counter;

            Thread ct = new Thread(new ThreadStart(ServerConnectThread));
            ct.Start();

            try
            {
                do
                {
                    bool ConnectedRes = false;
                    if (NewBeginAccept == true)
                    {
                        connectedevent.Reset();
                        Sock.BeginAccept(new AsyncCallback(AcceptAsync), Sock);
                        NewBeginAccept = false;
                    }

                    Counter = 0;
#if DEBUG
                    while (Counter < 30)
#else
                    while (Counter < 60)
#endif
                    {
                        ConnectedRes = connectedevent.WaitOne(1000);
                        if (ConnectedRes == false)
                        {
                            Counter++;
                        }
                        else
                        {
                            break;
                        }
                        if (StopThread == true)
                            break;
                    }

                    if (StopThread == true)
                        break;

                    if (ConnectedRes == true)
                    {
                        if (net == null)
                        {
                            //reject connection
                            NextConnected.Close();
                            continue;
                        }
                        else
                        {
                            Thread t = new Thread(new ParameterizedThreadStart(RunningConnectionAccepterAndGetter));
                            NetworkConnectionThreadPasser p = new NetworkConnectionThreadPasser();
                            p.RunningConnectionAccepterAndGetterThread = t;
                            p.GUID = "";
                            p.net = net.CloneElement();
                            p.sock = NextConnected;
                            t.Start(p);
                        }
                    }
                } while (StopThread == false);
            }
            catch (Exception ee)
            {
                FoxEventLog.VerboseWriteEventLog("SEH in PortMappings_ConnectionNode::ListenerThreadFn() " + ee.ToString(), EventLogEntryType.Error);
            }
            try
            {
                Sock.Close();
            }
            catch
            {

            }
            StopThread = true;
        }

        void AcceptAsync(IAsyncResult res)
        {
            try
            {
                Socket sock = (Socket)res.AsyncState;
                NextConnected = sock.EndAccept(res);
                connectedevent.Set();
                NewBeginAccept = true;
            }
            catch
            {

            }
        }

        public bool StartConnection()
        {
            try
            {
                Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                EndPoint EP = new IPEndPoint(Mapping.BindTo0000 == false ? IPAddress.Loopback : IPAddress.Any, Mapping.ClientPort);
                Sock.Bind(EP);

                ListenerThread = new Thread(new ThreadStart(ListenerThreadFn));
                ListenerThread.Start();
            }
            catch
            {
                return (false);
            }

            return (true);
        }

        public void StopConnection()
        {
            StopThread = true;
            try
            {
                Sock.Close();
            }
            catch
            {

            }
        }
    }
}
