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

namespace FoxSDC_Agent.Redirs
{
    class NetRedirWS
    {
        string ToServer;
        int Port;
        string SessionID;
        Socket socket = null;
        public bool InitSuccess = false;
        Thread RunningRXThread;
        public DateTime LastUpdated = DateTime.UtcNow;
        WebSocket ws = null;

        void CloseAll()
        {
            try
            {
                if (socket != null)
                    socket.Close();
            }
            catch
            {

            }

            try
            {
                if (ws != null)
                    ws.Close();
            }
            catch
            {

            }
        }


        ~NetRedirWS()
        {
            CloseAll();
        }

        void RXThread()
        {
            try
            {
                do
                {
                    int av = socket.Available;
                    if (av == 0)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    LastUpdated = DateTime.UtcNow;
                    byte[] d = new byte[av];
                    int r = socket.Receive(d);
                    if (r == 0)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    if (r != av)
                    {
                        byte[] rd = new byte[r];
                        for (int i = 0; i < r; i++)
                            rd[i] = d[i];
                        d = rd;
                    }

                    //Debug.WriteLine("RX: " + SessionID + " " + BitConverter.ToString(rd));
                    ws.Send(d);

                    LastUpdated = DateTime.UtcNow;
                } while (socket.Connected == true);
            }
            catch (Exception ee)
            {
                CloseAll();
                Debug.WriteLine("RX Error: " + ee.ToString());
            }
        }

        public NetRedirWS(Network net, string ToServer, int Port, string SessionID)
        {
            this.ToServer = ToServer;
            this.Port = Port;
            this.SessionID = SessionID;
            InitSuccess = false;

            try
            {
                IPAddress ip;
                IPHostEntry ipaddr;
                if (IPAddress.TryParse(ToServer, out ip) == false)
                {
                    ipaddr = Dns.GetHostEntry(ToServer);
                }
                else
                {
                    ipaddr = new IPHostEntry();
                    ipaddr.AddressList = new IPAddress[] { ip };
                }

                if (ipaddr == null)
                    return;
                if (ipaddr.AddressList.Length == 0)
                    return;

                string URL = ProgramAgent.GetWSUrl(net) + "websocket/agent-" + Uri.EscapeUriString(SessionID);
                ws = new WebSocket(URL);
                ws.OnMessage += Ws_OnMessage;
                ws.SetCookie(new WebSocketSharp.Net.Cookie("Agent-SessionID", net.Session));
                ws.Connect();                

                Debug.WriteLine("WSSOCKET: " + SessionID + " Create connection to " + ToServer + ":" + Port.ToString());
                socket = new Socket(ipaddr.AddressList[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipaddr.AddressList[0], Port);
                RunningRXThread = new Thread(new ThreadStart(RXThread));
                RunningRXThread.Start();
            }
            catch
            {
                CloseAll();
                return;
            }
            InitSuccess = true;
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                socket.Send(e.RawData);
            }
            catch (Exception ee)
            {
                Debug.WriteLine("TX Error: " + ee.ToString());
            }
        }

        public bool CloseConnection()
        {
            Debug.WriteLine("WSSOCKET: " + SessionID + " close");
            CloseAll();
            return (true);
        }
    }

    class MainNetRedirWS
    {
        static object Locker = new object();
        static Dictionary<string, NetRedirWS> Redirs = new Dictionary<string, NetRedirWS>();

        public static void TestTimeouts()
        {
            List<string> Kicks = new List<string>();
            lock (Locker)
            {
                foreach (KeyValuePair<string, NetRedirWS> pp in Redirs)
                {
                    if (pp.Value.LastUpdated.AddMinutes(30) < DateTime.UtcNow)
                        Kicks.Add(pp.Key);
                }
            }

            foreach (string K in Kicks)
            {
                lock (Locker)
                {
                    if (Redirs.ContainsKey(K) == false)
                        continue;
                    Redirs[K].CloseConnection();
                }
            }

            foreach (string K in Kicks)
            {
                lock (Locker)
                {
                    if (Redirs.ContainsKey(K) == false)
                        continue;
                    Redirs.Remove(K);
                    Debug.WriteLine("Kicked Socket " + K);
                }
            }
        }

        public static string StartNetRedir(Network net, string SessionID, string ToServer, int Port)
        {
            NetRedirWS R = new NetRedirWS(net, ToServer, Port, SessionID);
            if (R.InitSuccess == false)
                return ("");
            lock (Locker)
            {
                Redirs.Add(SessionID, R);
            }

            return (SessionID);
        }

        public static bool CloseConnection(string Data)
        {
            PushConnectNetworkData pdata = null;
            NetRedirWS redir = null;

            try
            {
                pdata = JsonConvert.DeserializeObject<PushConnectNetworkData>(Data);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }

            if (pdata == null)
                return (false);

            lock (Locker)
            {
                if (Redirs.ContainsKey(pdata.GUID) == false)
                    return (false);
                redir = Redirs[pdata.GUID];
            }

            return (redir.CloseConnection());
        }
    }
}
