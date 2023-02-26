using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class NetworkConnectionInfo
    {
        public readonly string ID = Guid.NewGuid().ToString();
        public SQLLib sql;
        public object sqllock = new object();
        public ServerInfo ServerInfo;
        public string Username;
        public string EMail;
        public string Name;
        public bool LoggedIn;
        public bool ComputerLoggedIn;
        public bool MustChangePassword;
        public string Error;
        public ErrorFlags ErrorID;
        public Int64 Permissions = 0;
        public bool Inited = false;
        public DateTime LastUsed = DateTime.Now;
        public bool FromClone = false;
        public string IPAddress = "";
        public Int64? PushChannel = null;
        public UploadRunner Upload = null;
        public ReaderWriterLockSlim RWLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        public bool IsLDAP = false;
        public Int64 AgentVersionID;

        public bool HasAcl(ACLFlags acl)
        {
            return (ACL.HasACL(Permissions, acl));
        }
    }

    class UploadRunner
    {
        public Int64 Size;
        public Int64 CurrentPosition;
        public Int64 Counter;
        /// <summary>
        /// See UploadRequest.FileType
        /// </summary>
        public int FileType;
        public Stream Data;
        public string TempName;
        public string TempFilename;
    }

    static class NetworkConnection
    {
        public static Dictionary<string, NetworkConnectionInfo> Connections = new Dictionary<string, NetworkConnectionInfo>();

        public static string NewSession()
        {
            NetworkConnectionInfo nc = new NetworkConnectionInfo();
            lock (Connections)
                Connections.Add(nc.ID, nc);
            return (nc.ID);
        }

        static bool AnotherNiPushRunningSameMachine(string MySession, string MachineID, Int64? Channel)
        {
            if (Channel == null)
                return (false);
            lock (Connections)
            {
                foreach (KeyValuePair<string, NetworkConnectionInfo> kvp in Connections)
                {
                    if (kvp.Value.ComputerLoggedIn == false)
                        continue;
                    if (kvp.Key == MySession)
                        continue;
                    if (MachineID.ToLower() == kvp.Value.Username.ToLower())
                        return (true);
                }
            }

            return (false);
        }

        public static void DeleteSession(string Session)
        {
            if (Connections.ContainsKey(Session) == true)
            {
                NetworkConnectionProcessor.DeInitNi(Connections[Session],
                    !AnotherNiPushRunningSameMachine(Session, Connections[Session].Username, Connections[Session].PushChannel));
                lock (Connections)
                    Connections.Remove(Session);
            }
        }

        public static NetworkConnectionInfo GetSession(string Session)
        {
            if (Connections.ContainsKey(Session) == true)
            {
                Connections[Session].LastUsed = DateTime.Now;
                return (Connections[Session]);
            }
            return (null);
        }
    }
}

