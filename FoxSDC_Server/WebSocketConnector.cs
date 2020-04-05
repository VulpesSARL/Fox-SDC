using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace FoxSDC_Server
{
    class WS_AgentConnection : WebSocketBehavior
    {
        string SessionID;
        protected override void OnOpen()
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            RemoteNetworkConnectionWSCrosser.SendToMGMT(SessionID, e.RawData);
        }

        public void InitThis(string SessionID)
        {
            this.SessionID = SessionID;
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

            Debug.WriteLine("Cookie (AG) validated");
            return (true);
        }
    }

    class WS_MGMTConnection : WebSocketBehavior
    {
        string SessionID;
        protected override void OnOpen()
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            RemoteNetworkConnectionWSCrosser.SendToAgent(SessionID, e.RawData);
        }

        public void InitThis(string SessionID)
        {
            this.SessionID = SessionID;
            this.CookiesValidator = CookieValidator;
        }

        bool CookieValidator(CookieCollection req, CookieCollection res)
        {
            string InternalRunningSessionID = req["MGMT-SessionID"].Value;
            if (string.IsNullOrWhiteSpace(InternalRunningSessionID) == true)
                return (false);
            NetworkConnectionInfo ni = NetworkConnection.GetSession(InternalRunningSessionID);
            if (ni == null)
                return (false);
            if (ni.ComputerLoggedIn == true)
                return (false);
            lock (RemoteNetworkConnectionWSCrosser.DictLock)
            {
                if (RemoteNetworkConnectionWSCrosser.Sessions.ContainsKey(SessionID) == false)
                    return (false);
                if (RemoteNetworkConnectionWSCrosser.Sessions[SessionID].MGMTUser != ni.Username)
                    return (false);
            }

            Debug.WriteLine("Cookie (MG) validated");
            return (true);
        }
    }

    class WSSessionStorage
    {
        public string MachineID;
        public string SessionID;
        public string MGMTUser;
        public DateTime LastUpdated;

        public string AgentURL;
        public string MGMTURL;
    }

    static class RemoteNetworkConnectionWSCrosser
    {
        static WebSocketServer WSServer;
        static string AppendToURL;

        /// <summary>
        /// SessionID -> WSSessionStorage
        /// </summary>
        public static Dictionary<string, WSSessionStorage> Sessions = new Dictionary<string, WSSessionStorage>(StringComparer.InvariantCultureIgnoreCase);
        public static object DictLock = new object();

        static bool SendToXXX(string SessionID, bool MGMT, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(SessionID) == true)
                return (false);
            WebSocketServiceHost sh;
            lock (DictLock)
            {
                if (Sessions.ContainsKey(SessionID) == false)
                    return (false);

                if (WSServer.WebSocketServices.TryGetServiceHost(MGMT == true ? Sessions[SessionID].MGMTURL : Sessions[SessionID].AgentURL, out sh) == false)
                    return (false);
            }
            int counter = 0;
            while (sh.Sessions.Count == 0)
            {
                Thread.Sleep(100);
                counter++;
                if (counter > 10 * 60) //1 min
                {
                    Debug.WriteLine("Timeout: SendToXXX - " + SessionID + " " + MGMT.ToString());
                    return (false);
                }
            }
            lock (DictLock)
            {
                if (Sessions.ContainsKey(SessionID) == false)
                    return (false);
                Sessions[SessionID].LastUpdated = DateTime.UtcNow;
                sh.Sessions.Broadcast(data);
            }

            return (true);
        }

        public static bool SendToAgent(string SessionID, byte[] Data)
        {
            return (SendToXXX(SessionID, false, Data));
        }

        public static bool SendToMGMT(string SessionID, byte[] Data)
        {
            return (SendToXXX(SessionID, true, Data));
        }

        static string GetURL()
        {
            string U = Settings.Default.WSListenOn;
            if (U.EndsWith("/") == false)
                U += "/";
            U = U.Replace("://+", "://0.0.0.0");
            return (U);
        }

        public static void InitialInitWS()
        {
            Uri url = new Uri(GetURL(), UriKind.Absolute);
            AppendToURL = url.AbsolutePath;
            if (AppendToURL.EndsWith("/") == false)
                AppendToURL += "/";

            if (url.Scheme.ToLower() != "wss" && url.Scheme.ToLower() != "ws")
                throw new Exception("Scheme for WSListenOn (Registry) does not start with ws:// or wss://");

            WSServer = new WebSocketServer(url.Scheme + "://" + url.Authority);
            WSServer.KeepClean = true;
            WSServer.ReuseAddress = true;

            if (string.IsNullOrWhiteSpace(Settings.Default.WSSSLCert) == true && url.Scheme.ToLower() == "wss")
                throw new Exception("Missing WSSSLCert (Registry) Entry");

            if (string.IsNullOrWhiteSpace(Settings.Default.WSSSLCert) == false && url.Scheme.ToLower() == "wss")
            {
                X509Store my = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                my.Open(OpenFlags.ReadOnly);
                bool Found = false;
                foreach (X509Certificate2 cert in my.Certificates)
                {
                    if (cert.Subject == Settings.Default.WSSSLCert)
                    {
                        WSServer.SslConfiguration.ServerCertificate = cert;
                        Found = true;
                        break;
                    }
                }
                if (Found == false)
                    throw new Exception("Cannot find certificate " + Settings.Default.WSSSLCert);
            }

            WSServer.Start();
        }

        public static void ShutdownWS()
        {
            try
            {
                WSServer.Stop();
            }
            catch
            {

            }
        }

        public static void TestTimeouts()
        {
            List<string> RemoveSessionIDs = new List<string>();

            lock (DictLock)
            {
                foreach (KeyValuePair<string, WSSessionStorage> kvp in Sessions)
                {
                    if (kvp.Value.LastUpdated.AddMinutes(10) < DateTime.UtcNow)
                    {
                        WebSocketServiceHost sh1, sh2;
                        if (WSServer.WebSocketServices.TryGetServiceHost(kvp.Value.MGMTURL, out sh1) == false)
                        {
                            RemoveSessionIDs.Add(kvp.Key);
                            continue;
                        }
                        if (WSServer.WebSocketServices.TryGetServiceHost(kvp.Value.AgentURL, out sh2) == false)
                        {
                            RemoveSessionIDs.Add(kvp.Key);
                            continue;
                        }

                        bool PingSH1 = false, PingSH2 = false;

                        foreach (IWebSocketSession sess in sh1.Sessions.Sessions)
                        {
                            if (sh1.Sessions.PingTo(sess.ID) == true)
                            {
                                PingSH1 = true;
                                break;
                            }
                        }
                        foreach (IWebSocketSession sess in sh2.Sessions.Sessions)
                        {
                            if (sh2.Sessions.PingTo(sess.ID) == true)
                            {
                                PingSH2 = true;
                                break;
                            }
                        }

                        if (PingSH1 == false || PingSH2 == false)
                            RemoveSessionIDs.Add(kvp.Key);

                        kvp.Value.LastUpdated = DateTime.UtcNow;
                    }
                }
            }

            lock (DictLock)
            {
                foreach (string s in RemoveSessionIDs)
                {
                    CloseSession(s);
                }
            }
        }

        public static string CreateSession(string InvokerUsername, string MachineID)
        {
            string SessionID = Guid.NewGuid().ToString();
            Debug.WriteLine("Create new WS-Session " + SessionID);

            lock (DictLock)
            {
                Sessions.Add(SessionID, new WSSessionStorage());
                Sessions[SessionID].MachineID = MachineID;
                Sessions[SessionID].MGMTUser = InvokerUsername;
                Sessions[SessionID].SessionID = SessionID;
                Sessions[SessionID].LastUpdated = DateTime.UtcNow;
                Sessions[SessionID].AgentURL = AppendToURL + "websocket/agent-" + Uri.EscapeUriString(SessionID);
                Sessions[SessionID].MGMTURL = AppendToURL + "websocket/mgmt-" + Uri.EscapeUriString(SessionID);

                WSServer.AddWebSocketService<WS_AgentConnection>(Sessions[SessionID].AgentURL, ni => ni.InitThis(SessionID));
                WSServer.AddWebSocketService<WS_MGMTConnection>(Sessions[SessionID].MGMTURL, ni => ni.InitThis(SessionID));
            }
            return (SessionID);
        }

        public static void CloseSession(string SessionID)
        {
            lock (DictLock)
            {
                if (Sessions.ContainsKey(SessionID) == false)
                    return;
                Debug.WriteLine("Closed WS-Session " + SessionID);
                WSServer.RemoveWebSocketService(Sessions[SessionID].MGMTURL);
                WSServer.RemoveWebSocketService(Sessions[SessionID].AgentURL);
            }
        }
    }
}
