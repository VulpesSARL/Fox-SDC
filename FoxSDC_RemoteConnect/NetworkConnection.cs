using FoxSDC_Common;
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

namespace FoxSDC_RemoteConnect
{
    class NetworkConnection
    {
        public enum StatusID
        {
            Success = 0,
            ConnectionSendError = 1,
            RemoteConnectionError = 2,
            RemoteConnectionStatusError = 3
        }

        class NetworkConnectionThreadPasser
        {
            public Network net;
            public Socket sock;
            public string GUID;
            public WebSocket ws;
            public string URL;
        }

        Network net;
        string MachineID;
        int LocalPort;
        string RemoteAddress;
        int RemotePort;
        Thread ListenerThread;
        Socket Sock;
        bool StopThread = false;
        string WSURL;

        bool RX = false;
        bool TX = false;

        List<NetworkConnectionThreadPasser> Connections = new List<NetworkConnectionThreadPasser>();

        public delegate void RXTX(bool RX, bool TX, Int64 PlusRX, Int64 PlusTX);
        public event RXTX OnRXTX;

        public delegate void Status(StatusID Res);
        public event Status OnStatus;

        void OnStatusEvent(StatusID Res)
        {
            OnStatus?.Invoke(Res);
        }

        void OnRXTXEvent(bool? RX, bool? TX, Int64 PlusRX, Int64 PlusTX)
        {
            if (RX != null)
                this.RX = RX.Value;
            if (TX != null)
                this.TX = TX.Value;

            OnRXTX?.Invoke(this.RX, this.TX, PlusRX, PlusTX);
        }

        void RunningConnectionAccepterAndGetter(object s)
        {
            #region Websockets
            NetworkConnectionThreadPasser p = (NetworkConnectionThreadPasser)s;

            WSURL = p.net.GetWebsocketURL();

            PushConnectNetworkResult res = p.net.PushConnectToRemote2(MachineID, RemoteAddress, RemotePort);
            if (res == null)
            {
                OnStatusEvent(StatusID.RemoteConnectionError);
                p.sock.Close();
                return;
            }

            if (res.Result != 0)
            {
                OnStatusEvent(StatusID.RemoteConnectionStatusError);
                p.sock.Close();
                return;
            }

            Debug.WriteLine("WS SOCKET: " + res.ConnectedGUID + " Create connection");
            p.URL = WSURL + "websocket/mgmt-" + Uri.EscapeUriString(res.ConnectedGUID);
            Debug.WriteLine("WS URL: " + p.URL);

            Connections.Add(p);
            p.ws = new WebSocket(p.URL);
            p.ws.OnMessage += Ws_OnMessage;
            p.ws.SetCookie(new WebSocketSharp.Net.Cookie("MGMT-SessionID", net.Session));
            p.ws.Connect();

            OnStatusEvent(StatusID.Success);

            while (p.sock.Connected == true && StopThread == false)
            {
                int av = p.sock.Available;
                if (av == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                byte[] data = new byte[av];
                int av2 = p.sock.Receive(data);
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

                OnRXTXEvent(null, true, 0, data.LongLength);
                try
                {
                    p.ws.Send(data);
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    OnStatusEvent(StatusID.RemoteConnectionStatusError);
                    CloseAll(res.ConnectedGUID, p);
                    return;
                }
                OnRXTXEvent(null, false, 0, 0);
            }

            OnStatusEvent(StatusID.Success);
            CloseAll(res.ConnectedGUID, p);
            #endregion
        }

        private void CloseAll(string GUID, NetworkConnectionThreadPasser p)
        {
            Debug.WriteLine("WS SOCKET: " + GUID + " Close connection");
            try
            {
                p.ws.Close();
            }
            catch
            {

            }

            p.net.PushConnectionToRemoteClose2(MachineID, GUID);

            try
            {
                p.sock.Close();
            }
            catch
            {

            }
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.RawData == null)
                return;
            OnRXTXEvent(true, null, e.RawData.LongLength, 0);
            WebSocket ws = sender as WebSocket;
            foreach (NetworkConnectionThreadPasser ntp in Connections)
            {
                if (ntp.URL == ws.Url.ToString())
                {
                    ntp.sock.Send(e.RawData);
                    break;
                }
            }
            OnRXTXEvent(false, null, 0, 0);
        }

        void ListenerThreadFn()
        {
            Sock.Listen((int)SocketOptionName.MaxConnections);

            try
            {
                do
                {
                    Socket connected = Sock.Accept();
                    Thread t = new Thread(new ParameterizedThreadStart(RunningConnectionAccepterAndGetter));
                    NetworkConnectionThreadPasser p = new NetworkConnectionThreadPasser();
                    p.GUID = "";
                    p.net = net.CloneElement();
                    p.sock = connected;
                    t.Start(p);
                } while (StopThread == false);
            }
            catch
            {

            }
            try
            {
                Sock.Close();
            }
            catch
            {

            }
        }

        public bool StartConnection(Network net, string MachineID, int LocalPort, string RemoteAddress, int RemotePort)
        {
            this.net = net;
            this.MachineID = MachineID;
            this.LocalPort = LocalPort;
            this.RemoteAddress = RemoteAddress;
            this.RemotePort = RemotePort;
            try
            {
                Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                EndPoint EP = new IPEndPoint(IPAddress.Loopback, LocalPort);
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
