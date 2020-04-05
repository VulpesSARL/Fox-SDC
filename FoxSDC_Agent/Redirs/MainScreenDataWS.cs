using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace FoxSDC_Agent.Redirs
{
    class ScreenDataWS
    {
        public string SessionID;
        public DateTime LastUpdated = DateTime.UtcNow;
        public WebSocket ws = null;
        ReaderWriterLockSlim RWLck = new ReaderWriterLockSlim();
        public byte[] UploadBytes = null;
        public List<byte> RecvBuffer = new List<byte>();
        const int UploadChunk = 100 * 1024;
        public bool InitSuccess = false;
        public bool Closed = false;
        public object RecvBufferLock = new object();
        public string URL;
        public string NetSessionID;

        void CloseAll()
        {
            Closed = true;
            try
            {
                if (ws != null)
                    ws.Close();
                ws = null;
            }
            catch
            {

            }
        }

        ~ScreenDataWS()
        {
            CloseAll();
        }

        bool ConnectWS()
        {
            try
            {
                ws = new WebSocket(URL);
                ws.OnMessage += Ws_OnMessage;
                ws.SetCookie(new WebSocketSharp.Net.Cookie("Agent-SessionID", NetSessionID));
                ws.Connect();

                Debug.WriteLine("Screen WSSOCKET: " + SessionID + " Create connection");
            }
            catch
            {
                CloseAll();
                return (false);
            }
            return (true);
        }

        public ScreenDataWS(Network net, string SessionID)
        {
            this.SessionID = SessionID;
            this.LastUpdated = DateTime.UtcNow;
            this.URL = ProgramAgent.GetWSUrl(net) + "websocket/agent-" + Uri.EscapeUriString(SessionID);
            this.NetSessionID = net.Session;

            if (ConnectWS() == false)
                return;

            InitSuccess = true;
        }

        void UploaderThread()
        {
            try
            {
                RWLck.EnterWriteLock();
                int l;
                l = UploadBytes.Length;
                for (int i = 0; i < l; i += UploadChunk)
                {
                    if (Closed == true)
                        break;
                    byte[] u = null;
                    if (UploadBytes == null)
                        return;
                    if (UploadBytes.Length != l)
                        return;
                    int ch = l < i + UploadChunk ? l - i : UploadChunk;
                    u = new byte[ch];
                    Array.Copy(UploadBytes, i, u, 0, ch);
                    try
                    {
                        ws.Send(u);
                    }
                    catch
                    {
                        if (Closed == false)
                        {
                            Debug.WriteLine("Screen RESETTING WS Connection");
                            if (ConnectWS() == true)
                            {
                                ws.Send(u);
                            }
                        }
                    }
                    LastUpdated = DateTime.UtcNow;
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
            finally
            {
                UploadBytes = null;
                RWLck.ExitWriteLock();
                Debug.WriteLine("Screen all buffer sent");
            }
        }

        public bool CloseConnection()
        {
            Debug.WriteLine("Screen WSSOCKET: " + SessionID + " close");
            CloseAll();
            return (true);
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                lock (RecvBufferLock)
                {
                    List<byte> Recv = new List<byte>(e.RawData);

                    while (Recv.Count > 0)
                    {
                        RecvBuffer.Add(Recv[0]);
                        Recv.RemoveAt(0);

                        #region Header Test

                        if (RecvBuffer.Count == 1)
                        {
                            if (RecvBuffer[0] != 0x46)
                            {
                                RecvBuffer.Clear();
                                continue;
                            }
                        }

                        if (RecvBuffer.Count == 2)
                        {
                            if (RecvBuffer[1] != 0x52)
                            {
                                RecvBuffer.Clear();
                                continue;
                            }
                        }

                        if (RecvBuffer.Count == 3)
                        {
                            if (RecvBuffer[2] != 0x53)
                            {
                                RecvBuffer.Clear();
                                continue;
                            }
                        }

                        if (RecvBuffer.Count == 4)
                        {
                            if (RecvBuffer[3] != 0x1)
                            {
                                RecvBuffer.Clear();
                                continue;
                            }
                        }

                        #endregion

                        if (RecvBuffer.Count == Marshal.SizeOf(typeof(SendDataData)))
                        {
                            LastUpdated = DateTime.UtcNow;
                            SendDataData ddd = CommonUtilities.Deserialize<SendDataData>(RecvBuffer.ToArray());
                            RecvBuffer.Clear();
                            switch (ddd.DataType)
                            {
                                case (int)SendDataType.Keyboard:
                                    Redirs.MainScreenSystem.SetKeyboard(ddd.Flag3, ddd.Flag2, ddd.Flag1);
                                    break;
                                case (int)SendDataType.Mouse:
                                    Redirs.MainScreenSystem.SetMousePosition(ddd.Flag1, ddd.Flag2, ddd.Flag3, ddd.Flag4);
                                    break;
                                case (int)SendDataType.DeltaScreen:
                                    {
                                        RWLck.EnterReadLock();
                                        if (UploadBytes != null)
                                        {
                                            RWLck.ExitReadLock();
                                            break;
                                        }
                                        RWLck.ExitReadLock();
                                        RWLck.EnterWriteLock();
                                        UploadBytes = Redirs.MainScreenSystem.GetDeltaScreen2();
                                        Thread t = new Thread(UploaderThread);
                                        t.Start();
                                        RWLck.ExitWriteLock();
                                        Debug.WriteLine("Screen D-Render Prep");
                                        break;
                                    }
                                case (int)SendDataType.RefreshScreen:
                                    {
                                        RWLck.EnterReadLock();
                                        if (UploadBytes != null)
                                        {
                                            RWLck.ExitReadLock();
                                            break;
                                        }
                                        RWLck.ExitReadLock();
                                        RWLck.EnterWriteLock();
                                        UploadBytes = Redirs.MainScreenSystem.GetFullscreen2();
                                        Thread t = new Thread(UploaderThread);
                                        t.Start();
                                        RWLck.ExitWriteLock();
                                        Debug.WriteLine("Screen F-Render Prep");
                                        break;
                                    }
                                case (int)SendDataType.ResetStream:
                                    Debug.WriteLine("Screen RESET Buffer");
                                    RecvBuffer.Clear();
                                    break;
                                case (int)SendDataType.Disconnect:
                                    CloseAll();
                                    return;
                                default:
                                    Debug.WriteLine("Screen ??? unknown code " + ddd.DataType);
                                    return;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine("Screen RX/TX Error: " + ee.ToString());
            }
        }
    }

    class MainScreenDataWS
    {
        public static Dictionary<string, ScreenDataWS> Sessions = new Dictionary<string, ScreenDataWS>();
        public static object SessionsLocker = new object();

        public static PushConnectNetworkResult StartRemoteScreen(Network net, string SessionID)
        {
            PushConnectNetworkResult Res = new PushConnectNetworkResult();
            Res.Result = 0;
            Res.ConnectedGUID = "";

            ScreenDataWS scr = new ScreenDataWS(net, SessionID);
            if (scr.InitSuccess == false)
            {
                Res.Result = 0x16;
                return (Res);
            }

            Res.ConnectedGUID = scr.SessionID;
            lock (SessionsLocker)
            {
                Sessions.Add(scr.SessionID, scr);
            }

            return (Res);
        }

        public static void TestTimeouts()
        {
            List<string> Kicks = new List<string>();
            lock (SessionsLocker)
            {
                foreach (KeyValuePair<string, ScreenDataWS> pp in Sessions)
                {
                    if (pp.Value.LastUpdated.AddMinutes(30) < DateTime.UtcNow || pp.Value.Closed == true)
                        Kicks.Add(pp.Key);
                }
            }

            foreach (string K in Kicks)
            {
                lock (SessionsLocker)
                {
                    if (Sessions.ContainsKey(K) == false)
                        continue;
                    Sessions[K].CloseConnection();
                }
            }

            foreach (string K in Kicks)
            {
                lock (SessionsLocker)
                {
                    if (Sessions.ContainsKey(K) == false)
                        continue;
                    Sessions.Remove(K);
                    Debug.WriteLine("Kicked Screen Session " + K);
                }
            }
        }

    }
}
