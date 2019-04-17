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
        }

        Network net;
        string MachineID;
        int LocalPort;
        string RemoteAddress;
        int RemotePort;
        Thread ListenerThread;
        Socket Sock;
        bool StopThread = false;

        bool RX = false;
        bool TX = false;

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
            NetworkConnectionThreadPasser p = (NetworkConnectionThreadPasser)s;

            PushConnectNetworkResult res = p.net.PushConnectToRemote(MachineID, RemoteAddress, RemotePort);
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

            Debug.WriteLine("SOCKET: " + res.ConnectedGUID + " Create connection");

            NetworkConnectionThreadPasser p2 = new NetworkConnectionThreadPasser();
            p2.GUID = res.ConnectedGUID;
            p2.net = p.net.CloneElement();
            p2.sock = p.sock;
            Thread t = new Thread(new ParameterizedThreadStart(RunningConnectionAccepterSender));
            t.Start(p2);
            bool Errror = false;

            Int64 Sequence = 0;

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
                PushConnectNetworkResult resi = p.net.PushConnectionToRemoteData(MachineID, res.ConnectedGUID, data, Sequence);
                Sequence++;
                if (resi == null || resi.Result != 0)
                {
                    p.sock.Close();
                    p.net.PushConnectionToRemoteClose(MachineID, res.ConnectedGUID);
                    OnStatusEvent(StatusID.ConnectionSendError);
                    Errror = true;
                    break;
                }
                OnRXTXEvent(null, false, 0, 0);
            }
            OnRXTXEvent(null, false, 0, 0);
            p.net.PushConnectionToRemoteClose(MachineID, res.ConnectedGUID);
            try
            {
                p.sock.Close();
            }
            catch
            {

            }
            Debug.WriteLine("SOCKET: " + res.ConnectedGUID + " Close connection");
            if (Errror == false)
                OnStatusEvent(StatusID.Success);
        }

        void RunningConnectionAccepterSender(object s)
        {
            NetworkConnectionThreadPasser p = (NetworkConnectionThreadPasser)s;
            Dictionary<Int64, PushConnectNetworkData> SequenceBuffer = new Dictionary<long, PushConnectNetworkData>();
            Int64 RunningSequence = 0;

            while (p.sock.Connected == true && StopThread == false)
            {
                OnRXTXEvent(true, null, 0, 0);
                if (SequenceBuffer.ContainsKey(RunningSequence) == true)
                {
                    try
                    {
                        RunningSequence++;
                        Debug.WriteLine("Agent->RC Seq# IB! " + SequenceBuffer[RunningSequence - 1].Seq);
                        p.sock.Send(SequenceBuffer[RunningSequence - 1].data);
                        SequenceBuffer.Remove(RunningSequence - 1);
                        continue;
                    }
                    catch
                    {

                    }
                }

                PushConnectNetworkData data = p.net.PushConnectionFromRemoteData(MachineID, p.GUID);
                if (data == null)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (data.Result != 0)
                {
                    if (data.Result == 0x16)
                    {
                        p.sock.Close();
                        OnRXTXEvent(false, null, 0, 0);
                        break;
                    }
                    Thread.Sleep(10);
                    continue;
                }
                if (data.data == null)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (data.data.Length == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                if (RunningSequence != data.Seq)
                {
                    SequenceBuffer.Add(data.Seq, data);
                    Debug.WriteLine("Agent->RC Seq# OOB! " + data.Seq);
                    continue;
                }

                try
                {
                    Debug.WriteLine("Agent->RC Seq# " + data.Seq);
                    RunningSequence++;
                    p.sock.Send(data.data);
                    OnRXTXEvent(true, null, data.data.LongLength, 0);
                }
                catch
                {

                }
                OnRXTXEvent(false, null, 0, 0);
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
