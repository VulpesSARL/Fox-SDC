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

namespace FoxSDC_Agent.Redirs
{
    class NetRedir
    {
        Network net;
        string ToServer;
        int Port;
        string SessionID;
        Socket socket;
        public bool InitSuccess = false;
        public List<byte[]> RXStack = new List<byte[]>();
        Thread RunningRXThread;
        object RXLocker = new object();
        ManualResetEvent RXEvent = new ManualResetEvent(false);
        public DateTime LastUpdated = DateTime.Now;
        Int64 Sequence = 0;
        Dictionary<Int64, PushConnectNetworkData> SequenceBuffer = new Dictionary<long, PushConnectNetworkData>();
        Int64 RunningSequence = 0;

        ~NetRedir()
        {
            try
            {
                socket.Close();
            }
            catch
            {

            }
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
                    LastUpdated = DateTime.Now;
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

                    lock (RXLocker)
                    {
                        //Debug.WriteLine("RX: " + SessionID + " " + BitConverter.ToString(rd));
                        RXStack.Add(d);
                        RXEvent.Set();
                    }
                    LastUpdated = DateTime.Now;
                } while (socket.Connected == true);
            }
            catch (Exception ee)
            {
                Debug.WriteLine("RX Error: " + ee.ToString());
            }
        }

        public NetRedir(string ToServer, int Port, string SessionID, Network net)
        {
            this.net = net;
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
                Debug.WriteLine("SOCKET: " + SessionID + " Create connection to " + ToServer + ":" + Port.ToString());
                socket = new Socket(ipaddr.AddressList[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipaddr.AddressList[0], Port);
                RunningRXThread = new Thread(new ThreadStart(RXThread));
                RunningRXThread.Start();
            }
            catch
            {
                return;
            }
            InitSuccess = true;
        }

        public bool SendDataToSocket(PushConnectNetworkData data)
        {
            try
            {
                if (SequenceBuffer.ContainsKey(RunningSequence) == true)
                {
                    do
                    {
                        RunningSequence++;
                        socket.Send(SequenceBuffer[RunningSequence - 1].data);
                        SequenceBuffer.Remove(RunningSequence - 1);
                    } while (SequenceBuffer.ContainsKey(RunningSequence) == true);
                }

                if (RunningSequence != data.Seq)
                {
                    RunningSequence++;
                    SequenceBuffer.Add(data.Seq, data);
                    return (true);
                }

                //Debug.WriteLine("TX: " + SessionID + " " + BitConverter.ToString(data));
                socket.Send(data.data);
                RunningSequence++;
                LastUpdated = DateTime.Now;
            }
            catch
            {
                return (false);
            }
            return (true);
        }

        public bool CloseConnection()
        {
            Debug.WriteLine("SOCKET: " + SessionID + " close");
            socket.Close();
            return (true);
        }

        public PushConnectNetworkData GetData()
        {
            PushConnectNetworkData res = new PushConnectNetworkData();
            res.GUID = SessionID;
            res.Result = 2;
            int Count = 0;
            LastUpdated = DateTime.Now;
            lock (RXLocker)
            {
                Count = RXStack.Count;
            }
            if (Count > 0)
            {
                lock (RXLocker)
                {
                    res.Seq = Sequence;
                    Sequence++;
                    res.data = RXStack[0];
                    RXStack.RemoveAt(0);
                    //Debug.WriteLine("RX: " + SessionID + " POP " + BitConverter.ToString(res.data));
                }
                res.Result = 0;
                return (res);
            }
            if (Count == 0)
            {
                lock (RXLocker)
                {
                    RXEvent.Reset();
                }
                if (RXEvent.WaitOne(30000) == false)
                {
                    res.Result = 1;
                    return (res);
                }
                lock (RXLocker)
                {
                    res.Seq = Sequence;
                    Sequence++;
                    res.Result = 0;
                    res.data = RXStack[0];
                    RXStack.RemoveAt(0);
                    //Debug.WriteLine("RX: " + SessionID + " POP " + BitConverter.ToString(res.data));
                }
                return (res);
            }

            return (res);
        }
    }

    class MainNetRedir
    {
        static object Locker = new object();
        static Dictionary<string, NetRedir> Redirs = new Dictionary<string, NetRedir>();

        public static void TestTimeouts()
        {
            List<string> Kicks = new List<string>();
            lock (Locker)
            {
                foreach (KeyValuePair<string, NetRedir> pp in Redirs)
                {
                    if (pp.Value.LastUpdated.AddMinutes(30) < DateTime.Now)
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

        public static PushConnectNetworkData ProcessDataRecv(string SessionID)
        {
            PushConnectNetworkData res = new PushConnectNetworkData();
            res.GUID = SessionID;
            res.Result = 0x16;
            NetRedir r = null;
            lock (Locker)
            {
                if (Redirs.ContainsKey(SessionID) == false)
                    return (res);
                r = Redirs[SessionID];
            }
            return (r.GetData());
        }

        public static string StartNetRedir(string ToServer, int Port, Network net)
        {
            string SessionID = Guid.NewGuid().ToString();
            Network cnet = net.CloneElement2();
            NetRedir R = new NetRedir(ToServer, Port, SessionID, cnet);
            if (R.InitSuccess == false)
                return ("");
            lock (Locker)
            {
                Redirs.Add(SessionID, R);
            }

            return (SessionID);
        }

        public static bool ProcessData(string Data)
        {
            PushConnectNetworkData pdata = null;
            NetRedir redir = null;

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

            return (redir.SendDataToSocket(pdata));
        }

        public static bool CloseConnection(string Data)
        {
            PushConnectNetworkData pdata = null;
            NetRedir redir = null;

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
