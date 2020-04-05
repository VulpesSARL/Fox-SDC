using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FoxSDC_Common
{
    public partial class Network
    {
        enum Verb
        {
            POST,
            GET,
            PUT,
            PATCH,
            DELETE,
            HEAD
        }

        public ServerInfo serverinfo = null;
        string URL;
        string SessionID = "";
        int res;
        public ErrorInfo LoginError;
        public bool StopDownload = false;

        public string ConnectedURL
        {
            get
            {
                return (URL);
            }
        }

        public string ServerURL
        {
            get { return (URL); }
        }

        public bool SetSessionID(string URI, string session)
        {
            SessionID = session;
            URL = URI;
            return (GetInfo());
        }

        public bool SetSessionID(string URI, string session, ServerInfo si)
        {
            SessionID = session;
            URL = URI;
            serverinfo = si;
            return (true);
        }

        public Network CloneElement2()
        {
            Network n = new Network();
            n.SetSessionID(this.ServerURL, this.Session, this.serverinfo);
            return (n);
        }

        public Network CloneElement()
        {
            Network n = new Network();
            n.SetSessionID(this.ServerURL, this.Session);
            return (n);
        }

        public string Session
        {
            get { return (SessionID); }
        }

        public bool Connect(string Server)
        {
            URL = Server;
            if (URL.EndsWith("/") == false)
                URL += "/";
            return (true);
        }

        bool SendReq(string URLAppend, Verb verb, string sdata, out string rdata, out int HTTPResponseCode, bool LongTimeOut = false)
        {
            ServicePointManager.DefaultConnectionLimit = 1024;
            rdata = "";
            HTTPResponseCode = 200;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(sdata);
                HttpWebRequest client = (HttpWebRequest)WebRequest.Create(URL + URLAppend);
                client.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                client.Pipelined = false;
#if !COMPACT
                client.ServicePoint.Expect100Continue = false;
#endif
                client.AllowAutoRedirect = true;
                if (SessionID != "")
                    client.Headers.Add("Authorization", "Bearer " + SessionID);
#if DEBUG
                if (LongTimeOut == false)
                {
                    client.ReadWriteTimeout = 30000;
                    client.Timeout = 30000;
                }
                else
                {
                    client.ReadWriteTimeout = 1000 * 60 * 10; //10 Minutes
                    client.Timeout = 1000 * 60 * 10;
                }
#else
                if (LongTimeOut == false)
                {
                    client.ReadWriteTimeout = 60000;
                    client.Timeout = 60000;
                }
                else
                {
                    client.ReadWriteTimeout = 1000 * 60 * 10; //10 Minutes
                    client.Timeout = 1000 * 60 * 10;
                }
#endif
                client.UserAgent = "FoxSDC Client";
                switch (verb)
                {
                    case Verb.DELETE: client.Method = "DELETE"; break;
                    case Verb.GET: client.Method = "GET"; break;
                    case Verb.HEAD: client.Method = "HEAD"; break;
                    case Verb.PATCH: client.Method = "PATCH"; break;
                    case Verb.POST: client.Method = "POST"; break;
                    case Verb.PUT: client.Method = "PUT"; break;
                }

                if (verb != Verb.HEAD && verb != Verb.GET)
                {
                    client.ContentLength = data.Length;
                    client.ContentType = "application/json";
                    using (Stream send = client.GetRequestStream())
                    {
                        send.Write(data, 0, data.Length);
                        send.Close();
                    }
                }
                using (HttpWebResponse resp = (HttpWebResponse)client.GetResponse())
                {
                    HTTPResponseCode = (int)resp.StatusCode;
                    StreamReader recv = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                    rdata = recv.ReadToEnd();
                    recv.Close();
                }
                return (true);
            }
            catch (WebException ee)
            {
                HttpWebResponse resp = (HttpWebResponse)ee.Response;
                if (resp == null)
                {
                    HTTPResponseCode = 500;
                    Debug.WriteLine("HTTP TIMEOUT, or other issues " + verb.ToString() + " " + URL + URLAppend + " - " + ee.ToString());
                    rdata = "";
                }
                else
                {
                    HTTPResponseCode = (int)resp.StatusCode;
                    Debug.WriteLine("HTTP " + HTTPResponseCode.ToString() + " - " + URLAppend);
                    StreamReader recv = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                    rdata = recv.ReadToEnd();
                }
                return (false);
            }
            catch (Exception ee)
            {
                HTTPResponseCode = 500;
                Debug.WriteLine("HTTP other error");
                rdata = "";
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        bool SendReq<T, U>(string URLAppend, Verb verb, T sdata, out U rdata, out int HTTPResponseCode, bool LongTimeOut = false)
        {
            string sjson = JsonConvert.SerializeObject(sdata);
            string rjson = "";
            rdata = default(U);
            bool res = SendReq(URLAppend, verb, sjson, out rjson, out HTTPResponseCode, LongTimeOut);
            try
            {
                rdata = JsonConvert.DeserializeObject<U>(rjson);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
            return (res);
        }

        bool SendReq<U>(string URLAppend, Verb verb, out U rdata, out int HTTPResponseCode, bool LongTimeOut = false)
        {
            string sjson = "";
            string rjson = "";
            rdata = default(U);
            bool res = SendReq(URLAppend, verb, sjson, out rjson, out HTTPResponseCode, LongTimeOut);
            try
            {
                rdata = JsonConvert.DeserializeObject<U>(rjson);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
            return (res);
        }

        bool SendReq<T>(string URLAppend, Verb verb, T sdata, out int HTTPResponseCode, bool LongTimeOut = false)
        {
            string sjson = JsonConvert.SerializeObject(sdata);
            string rjson = "";
            bool res = SendReq(URLAppend, verb, sjson, out rjson, out HTTPResponseCode, LongTimeOut);
            return (res);
        }

        bool SendReq(string URLAppend, Verb verb, out int HTTPResponseCode, bool LongTimeOut = false)
        {
            string sjson = "";
            string rjson = "";
            bool res = SendReq(URLAppend, verb, sjson, out rjson, out HTTPResponseCode, LongTimeOut);
            return (res);
        }

        public bool HasACL(ACLFlags flag)
        {
            return (ACL.HasACL(serverinfo.Permissions, flag));
        }

        public bool GetInfo()
        {
            return (SendReq<ServerInfo>("api/status/info", Verb.GET, out serverinfo, out res));
        }

        public bool Login(string Username, string Password)
        {
            Logon logon = new Logon();
            logon.Username = Username;
            logon.Password = Password;

            bool resb = SendReq<Logon, ErrorInfo>("api/mgmt/login/user", Verb.POST, logon, out LoginError, out res);
            if (resb == true)
            {
                if (LoginError.Error.StartsWith("OK:") == true)
                    SessionID = LoginError.Error.Substring(3, LoginError.Error.Length - 3);
                else
                    return (false);
                return (GetInfo());
            }

            return (false);
        }

        public bool ComputerLogin(string Username, string Password, string ContractID, string ContractPassword, BaseSystemInfo sysinfo)
        {
            ComputerLogon cl = new ComputerLogon();
            cl.Username = Username;
            cl.Password = Password;
            cl.ContractID = ContractID;
            cl.ContractPassword = ContractPassword;
            cl.SysInfo = sysinfo;

            bool resb = SendReq<ComputerLogon, ErrorInfo>("api/login/computer", Verb.POST, cl, out LoginError, out res);
            if (resb == true)
            {
                if (LoginError.Error.StartsWith("OK:") == true)
                    SessionID = LoginError.Error.Substring(3, LoginError.Error.Length - 3);
                else
                    return (false);
                return (GetInfo());
            }
            return (false);
        }

        public void CloseConnection()
        {
            SendReq("api/login/logoff", Verb.POST, out res);
        }

        public bool Ping()
        {
            return (SendReq("api/status/ping", Verb.GET, out res));
        }

        public string CloneSession()
        {
            ErrorInfo erri;
            string tSessionID = "";

            bool resb = SendReq<ErrorInfo>("api/login/clone", Verb.POST, out erri, out res);
            if (resb == true)
            {
                if (erri.Error.StartsWith("OK:") == true)
                    tSessionID = erri.Error.Substring(3, erri.Error.Length - 3);
                else
                    return ("");
                return (tSessionID);
            }

            return ("");
        }

        public string GetLastError()
        {
            ErrorInfo erri;

            bool resb = SendReq<ErrorInfo>("api/status/error", Verb.GET, out erri, out res);
            if (resb == true)
                return (erri.Error);
            else
                return ("");
        }

        public ErrorFlags GetLastError2()
        {
            ErrorInfo erri;

            bool resb = SendReq<ErrorInfo>("api/status/error", Verb.GET, out erri, out res);
            if (resb == true)
                return ((ErrorFlags)erri.ErrorID);
            else
                return (ErrorFlags.FaultyData);
        }

        public bool NeedChangePassword()
        {
            NetBool pw;
            bool resb = SendReq<NetBool>("api/mgmt/login/user/password", Verb.GET, out pw, out res);
            if (resb == false)
                return (false);
            else
                return (pw.Data);
        }

        public bool ChangeMyPassword(string OldPassword, string NewPassword)
        {
            ChangePassword c = new ChangePassword();
            c.OldPassword = OldPassword;
            c.NewPassword = NewPassword;

            bool resb = SendReq<ChangePassword>("api/mgmt/login/user/password", Verb.POST, c, out res);
            return (resb);
        }

        public ClientSettings GetClientSettings()
        {
            ClientSettings set;
            bool resb = SendReq<ClientSettings>("api/client/settings", Verb.GET, out set, out res);
            if (resb == false)
                return (null);
            else
                return (set);
        }

        public string GetWebsocketURL()
        {
            NetString set;
            bool resb = SendReq<NetString>("api/client/websocketurl", Verb.GET, out set, out res);
            if (resb == false)
                return (null);
            if (set == null)
                return (null);
            if (set.Data.EndsWith("/") == false)
                set.Data += "/";
            return (set.Data);
        }

        public ServerSettings GetServerSettings()
        {
            ServerSettings set;
            bool resb = SendReq<ServerSettings>("api/mgmt/status/settings", Verb.GET, out set, out res);
            if (resb == false)
                return (null);
            else
                return (set);
        }

        public bool SetServerSettings(ServerSettings setting)
        {
            bool resb = SendReq<ServerSettings>("api/mgmt/status/settings", Verb.POST, setting, out res);
            return (resb);
        }

        public bool UploadRequest(int filetype, Int64 sz)
        {
            UploadRequest r = new UploadRequest();
            r.FileSize = sz;
            r.FileType = filetype;
            r.TempName = "";
            return (UploadRequest(r));
        }

        public bool UploadRequest(int filetype, Int64 sz, string PreTitle)
        {
            UploadRequest r = new UploadRequest();
            r.FileSize = sz;
            r.FileType = filetype;
            r.TempName = PreTitle;
            return (UploadRequest(r));
        }

        public bool UploadRequest(UploadRequest u)
        {
            bool resb = SendReq<UploadRequest>("api/mgmt/upload/request", Verb.POST, u, out res);
            return (resb);
        }

        public bool UploadData(UploadData u)
        {
            bool resb = SendReq<UploadData>("api/mgmt/upload/data", Verb.POST, u, out res);
            return (resb);
        }

        public NewUploadDataID UploadFinalise()
        {
            NewUploadDataID u;
            bool resb = SendReq<NewUploadDataID>("api/mgmt/upload/finalise", Verb.POST, out u, out res);
            if (resb == true)
                return (u);
            else
                return (null);
        }

        public bool UploadCancel()
        {
            bool resb = SendReq("api/mgmt/upload/cancel", Verb.POST, out res);
            return (resb);
        }

        public List<ComputerData> GetComputerList(bool? approved, Int64? Group)
        {
            string GetString = "";
            if (approved != null)
                GetString += (GetString == "" ? "" : "&") + "Approved=" + (approved == true ? "1" : "0");
            if (Group != null)
                GetString += (GetString == "" ? "" : "&") + "Group=" + Group.Value.ToString();

            if (GetString != "")
                GetString = "?" + GetString;

            ComputerDataList cdl;
            bool resb = SendReq<ComputerDataList>("api/mgmt/computers" + GetString, Verb.GET, out cdl, out res);
            if (resb == true)
                return (cdl.List);
            else
                return (null);
        }

        public bool ApproveComputers(string MachineID, bool state, Int64 Group)
        {
            ApproveComputer c = new ApproveComputer();
            c.State = state;
            c.Group = Group;

            bool resb = SendReq<ApproveComputer>("api/mgmt/computers/" + MachineID, Verb.PATCH, c, out res);
            return (resb);
        }

        public bool ChangeComputerComment(string MachineID, string Comment)
        {
            NetString str = new NetString();
            str.Data = Comment;

            bool resb = SendReq<NetString>("api/mgmt/computercomment/" + MachineID, Verb.PATCH, str, out res);
            return (resb);
        }

        public ComputerData GetComputerDetail(string MachineID)
        {
            ComputerData cd;
            bool resb = SendReq<ComputerData>("api/mgmt/computers/" + MachineID, Verb.GET, out cd, out res);
            if (resb == true)
                return (cd);
            else
                return (null);
        }

        public bool DeleteComputer(string MachineID)
        {
            bool resb = SendReq("api/mgmt/computers/" + MachineID, Verb.DELETE, out res);
            return (resb);
        }

        public List<GroupElement> GetGroups(Int64? Group)
        {
            string GetString = "";
            if (Group != null)
                GetString += (GetString == "" ? "" : "&") + Group.ToString();

            if (GetString != "")
                GetString = "/" + GetString;

            GroupElementList cd;
            bool resb = SendReq<GroupElementList>("api/mgmt/groups" + GetString, Verb.GET, out cd, out res);
            if (resb == true)
                return (cd.List);
            else
                return (null);
        }

        public Int64? CreateGroup(string Name, Int64? togroup)
        {
            CreateGroup req = new CreateGroup();
            req.Name = Name;
            req.ToParent = togroup;
            NetInt64 resi;

            bool resb = SendReq<CreateGroup, NetInt64>("api/mgmt/groups", Verb.POST, req, out resi, out res);
            if (resb == true)
                return (resi.Data);
            else
                return (null);
        }

        public GroupElement GetGroupDetails(Int64 GroupID)
        {
            GroupElement ge;
            bool resb = SendReq<GroupElement>("api/mgmt/groupdetail/" + GroupID.ToString(), Verb.GET, out ge, out res);
            if (resb == true)
                return (ge);
            else
                return (null);
        }

        public string GetGroupName(Int64 GroupID)
        {
            GroupElement ge = GetGroupDetails(GroupID);
            if (ge == null)
                return (null);
            return (ge.Name);
        }

        public bool DeleteGroup(Int64 ID)
        {
            bool resb = SendReq("api/mgmt/groups/" + ID.ToString(), Verb.DELETE, out res);
            return (resb);
        }

        public bool RenameGroup(Int64 ID, string Newname)
        {
            NetString g = new NetString();
            g.Data = Newname;

            bool resb = SendReq<NetString>("api/mgmt/groups/" + ID.ToString(), Verb.PATCH, g, out res);
            return (resb);
        }

        public bool ReportAddRemovePrograms(ListAddRemoveApps infos)
        {
            bool resb = SendReq<ListAddRemoveApps>("api/reports/addremove", Verb.POST, infos, out res);
            return (resb);
        }

        public bool ReportStartups(ListStartupItems infos)
        {
            bool resb = SendReq<ListStartupItems>("api/reports/startups", Verb.POST, infos, out res);
            return (resb);
        }

        public bool ReportDiskData(ListDiskDataReport infos)
        {
            bool resb = SendReq<ListDiskDataReport>("api/reports/diskdata", Verb.POST, infos, out res);
            return (resb);
        }

        public bool ReportNetworkAdapterConfiguration(ListNetworkAdapterConfiguration infos)
        {
            bool resb = SendReq<ListNetworkAdapterConfiguration>("api/reports/netadapterconfig", Verb.POST, infos, out res);
            return (resb);
        }

        public bool ReportDevicesList(PnPDeviceList infos)
        {
            bool resb = SendReq<PnPDeviceList>("api/reports/deviceslist", Verb.POST, infos, out res);
            return (resb);
        }

        public bool ReportUsers(UsersList users)
        {
            bool resb = SendReq<UsersList>("api/reports/repuserslist", Verb.POST, users, out res);
            return (resb);
        }

        public bool ReportBitlockerRKList(BitlockerRKList infos)
        {
            bool resb = SendReq<BitlockerRKList>("api/reports/bitlockerrklist", Verb.POST, infos, out res);
            return (resb);
        }

        public bool ReportFiltersList(FilterDriverList infos)
        {
            bool resb = SendReq<FilterDriverList>("api/reports/devicefilters", Verb.POST, infos, out res);
            return (resb);
        }

        public List<AddRemoveAppReport> GetAddRemovePrograms()
        {
            ListAddRemoveAppsReport ni;
            bool resb = SendReq<ListAddRemoveAppsReport>("api/mgmt/reports/addremove", Verb.GET, out ni, out res);
            if (resb == true)
                return (ni.Items);
            else
                return (null);
        }

        public List<VulpesSMARTInfo> GetSMARTInfo(string MachineID)
        {
            VulpesSMARTInfoList ni;
            bool resb = SendReq<VulpesSMARTInfoList>("api/mgmt/reports/smartinfo/" + MachineID, Verb.GET, out ni, out res);
            if (resb == true)
                return (ni.List);
            else
                return (null);
        }

        public List<AddRemoveAppReport> GetAddRemovePrograms(string MachineID)
        {
            ListAddRemoveAppsReport ni;
            bool resb = SendReq<ListAddRemoveAppsReport>("api/mgmt/reports/addremove/" + MachineID, Verb.GET, out ni, out res);
            if (resb == true)
                return (ni.Items);
            else
                return (null);
        }

        public List<StartupItemFull> GetStartupItems(string MachineID)
        {
            ListStartupItemReport ni;
            bool resb = SendReq<ListStartupItemReport>("api/mgmt/reports/startupitems/" + MachineID, Verb.GET, out ni, out res);
            if (resb == true)
                return (ni.Items);
            else
                return (null);
        }

        public Int64? CreatePolicy(NewPolicyReq policy)
        {
            NetInt64 ni;
            bool resb = SendReq<NewPolicyReq, NetInt64>("api/mgmt/policies", Verb.POST, policy, out ni, out res);
            if (resb == true)
                return (ni.Data);
            else
                return (null);
        }

        public bool DeletePolicy(Int64 ID)
        {
            bool resb = SendReq("api/mgmt/policies/" + ID.ToString(), Verb.DELETE, out res);
            return (resb);
        }

        public bool EditPolicy(EditPolicy policy)
        {
            bool resb = SendReq<EditPolicy>("api/mgmt/policies/" + policy.ID.ToString(), Verb.PUT, policy, out res);
            return (resb);
        }

        public bool EditPolicy(Int64 ID, string policy)
        {
            EditPolicy pol = new EditPolicy();
            pol.DataOnly = true;
            pol.Data = policy;
            pol.ID = ID;
            return (EditPolicy(pol));
        }

        public List<PolicyObject> ListPolicies(bool? AllPolicies, string MachineID, Int64? Grouping, bool? WithData)
        {
            string GetString = "";
            if (Grouping != null)
                GetString += (GetString == "" ? "" : "&") + "Grouping=" + Grouping.ToString();
            if (AllPolicies != null)
                GetString += (GetString == "" ? "" : "&") + "AllPolicies=" + (AllPolicies == true ? "1" : "0");
            if (MachineID != null && MachineID != "")
                GetString += (GetString == "" ? "" : "&") + "MachineID=" + MachineID;
            if (WithData != null)
                GetString += (GetString == "" ? "" : "&") + "WithData=" + (WithData == true ? "1" : "0");

            if (GetString != "")
                GetString = "?" + GetString;

            PolicyObjectList ni;
            bool resb = SendReq<PolicyObjectList>("api/mgmt/policies" + GetString, Verb.GET, out ni, out res);
            if (resb == true)
                return (ni.Items);
            else
                return (null);
        }

        public bool EnableDisablePolicy(Int64 ID, bool Enable)
        {
            PolicyEnableDisableRequest req = new PolicyEnableDisableRequest();
            req.Enable = Enable;
            bool resb = SendReq<PolicyEnableDisableRequest>("api/mgmt/policies/" + ID.ToString(), Verb.PATCH, req, out res);
            return (resb);
        }

        public PolicyObject GetPolicyObject(Int64 ID)
        {
            PolicyObject po;
            bool resb = SendReq<PolicyObject>("api/mgmt/policies/" + ID.ToString(), Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public PolicyObjectListSigned GetPoliciesForComputer()
        {
            PolicyObjectListSigned po;
            bool resb = SendReq<PolicyObjectListSigned>("api/mgmt/computerpolicies", Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public List<string> GetServerCertificates()
        {
            NetStringList po;
            bool resb = SendReq<NetStringList>("api/mgmt/certificates/installedcerts", Verb.GET, out po, out res);
            if (resb == true)
                return (po.Items);
            else
                return (null);
        }

        public PolicyObjectSigned GetPolicyObjectSigned(Int64 ID)
        {
            PolicyObjectSigned po;
            bool resb = SendReq<PolicyObjectSigned>("api/agent/signedpolicies/" + ID.ToString(), Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public List<DiskDataReport> GetDiskDataList()
        {
            ListDiskDataReport po;
            bool resb = SendReq<ListDiskDataReport>("api/mgmt/reports/diskdata", Verb.GET, out po, out res);
            if (resb == true)
                return (po.Items);
            else
                return (null);
        }

        public List<DiskDataReport> GetDiskDataList(string ID)
        {
            ListDiskDataReport po;
            bool resb = SendReq<ListDiskDataReport>("api/mgmt/reports/diskdata/" + ID, Verb.GET, out po, out res);
            if (resb == true)
                return (po.Items);
            else
                return (null);
        }

        public List<NetworkAdapterConfiguration> GetNetAdapterConfig(string ID)
        {
            ListNetworkAdapterConfiguration po;
            bool resb = SendReq<ListNetworkAdapterConfiguration>("api/mgmt/reports/netdata/" + ID, Verb.GET, out po, out res);
            if (resb == true)
                return (po.Items);
            else
                return (null);
        }

        public List<PnPDevice> GetDevicesConfig(string ID)
        {
            PnPDeviceList po;
            bool resb = SendReq<PnPDeviceList>("api/mgmt/reports/devicesdata/" + ID, Verb.GET, out po, out res);
            if (resb == true)
                return (po.List);
            else
                return (null);
        }

        public List<BitlockerRK> GetBitlockerRK(string ID)
        {
            BitlockerRKList po;
            bool resb = SendReq<BitlockerRKList>("api/mgmt/reports/bitlockerrkdata/" + ID, Verb.GET, out po, out res);
            if (resb == true)
                return (po.List);
            else
                return (null);
        }

        public List<FilterDriver> GetDevicesFilters(string ID)
        {
            FilterDriverList po;
            bool resb = SendReq<FilterDriverList>("api/mgmt/reports/devicesfilters/" + ID, Verb.GET, out po, out res);
            if (resb == true)
                return (po.List);
            else
                return (null);
        }

        public List<PackageData> GetPackages()
        {
            PackageDataList po;
            bool resb = SendReq<PackageDataList>("api/mgmt/packages", Verb.GET, out po, out res);
            if (resb == true)
                return (po.Items);
            else
                return (null);
        }

        public PackageData GetPackages(Int64 id)
        {
            PackageData po;
            bool resb = SendReq<PackageData>("api/mgmt/packages/" + id, Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public PackageDataSigned GetPackagesSigned(Int64 id)
        {
            PackageDataSigned po;
            bool resb = SendReq<PackageDataSigned>("api/agent/pkgsigned/" + id, Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public List<string> GetEventSources()
        {
            NetStringList lst;
            bool resb = SendReq<NetStringList>("api/mgmt/reports/eventsources", Verb.GET, out lst, out res);
            if (lst == null)
                return (null);
            if (resb == true)
                return (lst.Items);
            else
                return (null);
        }

        public List<EventLogReportFull> GetEventLogs(EventLogSearch search)
        {
            EventLogReportFullList lst;
            bool resb = SendReq<EventLogSearch, EventLogReportFullList>("api/mgmt/reports/eventlog", Verb.POST, search, out lst, out res);
            if (resb == true)
                return (lst.Data);
            else
                return (null);
        }

        static Dictionary<Int64, PackageData> PackageInfoCache = null;
        static object PackageInfoCacheLock = new object();

        public PackageData GetPackagesCached(Int64 id)
        {
            lock (PackageInfoCacheLock)
            {
                if (PackageInfoCache == null)
                    PackageInfoCache = new Dictionary<long, PackageData>();
            }

            lock (PackageInfoCacheLock)
            {
                if (PackageInfoCache.ContainsKey(id) == true)
                    return (PackageInfoCache[id]);
                PackageData pd = GetPackages(id);
                if (pd == null)
                    return (null);
                PackageInfoCache.Add(pd.ID, pd);
                return (pd);
            }
        }

        static Dictionary<Int64, PackageDataSigned> SignedPackageInfoCache = null;
        static object SignedPackageInfoCacheLock = new object();

        public PackageDataSigned SignedGetPackagesCached(Int64 id, List<byte[]> cer)
        {
            lock (SignedPackageInfoCacheLock)
            {
                if (SignedPackageInfoCache == null)
                    SignedPackageInfoCache = new Dictionary<long, PackageDataSigned>();
            }

            lock (SignedPackageInfoCacheLock)
            {
                if (SignedPackageInfoCache.ContainsKey(id) == true)
                    return (SignedPackageInfoCache[id]);
                PackageDataSigned pd = GetPackagesSigned(id);
                if (pd == null)
                    return (null);
                bool ValidCer = false;
                foreach (byte[] cerr in cer)
                {
                    if (Certificates.Verify(pd, cerr) == true)
                    {
                        ValidCer = true;
                        break;
                    }
                }
                if (ValidCer == false)
                    return (null);
                SignedPackageInfoCache.Add(pd.Package.ID, pd);
                return (pd);
            }
        }

        public void FlushPackageInfoCache()
        {
            lock (PackageInfoCacheLock)
            {
                PackageInfoCache = new Dictionary<long, PackageData>();
            }
        }

        public void FlushSignedPackageInfoCache()
        {
            lock (SignedPackageInfoCacheLock)
            {
                SignedPackageInfoCache = new Dictionary<long, PackageDataSigned>();
            }
        }

        public bool DeletePackage(Int64 id)
        {
            bool resb = SendReq("api/mgmt/packages/" + id, Verb.DELETE, out res);
            return (resb);
        }

        public bool ReportEventLogs(List<EventLogReport> lst)
        {
            ListEventLogReport req = new ListEventLogReport();
            req.Items = lst;
            bool resb = SendReq<ListEventLogReport>("api/reports/eventlog", Verb.POST, req, out res);
            return (resb);
        }

        public delegate void OnDownloadNotify(Int64 CurrentSZ, Int64 TotalSZ);
        public event OnDownloadNotify DownloadNotify;
        Int64 DownloadTotalSZ;

        public bool DownloadFile(string URLAppend, string ToFilename, Int64 TotalSZ = 0)
        {
            DownloadTotalSZ = TotalSZ;

            HttpWebRequest client = (HttpWebRequest)WebRequest.Create(URL + URLAppend);
            client.Pipelined = false;
#if !COMPACT
            client.ServicePoint.Expect100Continue = false;
#endif
            client.AllowAutoRedirect = true;
            if (SessionID != "")
                client.Headers.Add("Authorization", "Bearer " + SessionID);
#if DEBUG
            client.ReadWriteTimeout = 5000;
            client.Timeout = 5000;
#else
            client.ReadWriteTimeout = 60000;
            client.Timeout = 60000;
#endif
            client.UserAgent = "FoxSDC Client";
            client.Method = "GET";

            try
            {
                Int64 SeekTo = 0;
                FileMode fm = FileMode.Create;

                if (File.Exists(ToFilename) == true)
                {
                    FileInfo f = new FileInfo(ToFilename);
                    SeekTo = f.Length;
                    fm = FileMode.Open;
                }

                using (Stream FileStream = File.Open(ToFilename, fm, FileAccess.ReadWrite, FileShare.Read))
                {
                    FileStream.Seek(SeekTo, SeekOrigin.Begin);
                    HttpWebResponse resp = (HttpWebResponse)client.GetResponse();
                    using (Stream HTTPStream = resp.GetResponseStream())
                    {
                        const int ReadBufferSZ = 2048;

                        StopDownload = false;

                        byte[] data = new byte[ReadBufferSZ];
                        int ReadSZ = HTTPStream.Read(data, 0, ReadBufferSZ);
                        while (ReadSZ > 0)
                        {
                            FileStream.Write(data, 0, ReadSZ);

                            DownloadNotify?.Invoke(FileStream.Position, DownloadTotalSZ);

                            ReadSZ = HTTPStream.Read(data, 0, ReadBufferSZ);
                            if (StopDownload == true)
                                break;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
            return (true);
        }

        public Int64? GetAvailableAgentVersion(bool EarlyUpdate)
        {
            NetInt64 resi;

            bool resb = SendReq<NetInt64>("api/" + (EarlyUpdate == true ? "early" : "") + "update/version", Verb.GET, out resi, out res);
            if (resb == true)
                return (resi.Data);
            else
                return (null);
        }

        public WindowsLic GetWindowsLicData(string ID)
        {
            WindowsLic po;
            bool resb = SendReq<WindowsLic>("api/mgmt/reports/windowslic/" + ID, Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public bool RunAdminReportNow()
        {
            bool resb = SendReq("api/mgmt/reports/runadminnow", Verb.GET, out res);
            return (resb);
        }

        public bool ReportWindowsLicData(WindowsLic rs)
        {
            bool resb = SendReq<WindowsLic>("api/reports/windowslic", Verb.POST, rs, out res);
            return (resb);
        }

        public bool ReportSMARTInfos(string MachineID, List<VulpesSMARTInfo> smart)
        {
            VulpesSMARTInfoList rs = new VulpesSMARTInfoList();
            rs.List = smart;
            rs.MachineID = MachineID;
            bool resb = SendReq<VulpesSMARTInfoList>("api/reports/smartinfo", Verb.POST, rs, out res);
            return (resb);
        }

        public bool PaperSaveTemplate(string Paper, byte[] data)
        {
            ReportPaper p = new ReportPaper();
            p.data = data;
            p.Name = Paper;
            bool resb = SendReq<ReportPaper>("api/mgmt/rep/addpaper", Verb.POST, p, out res, true);
            if (resb == false)
                return (false);
            if (res != 200)
                return (false);
            return (true);
        }

        public List<string> PaperGetSupported()
        {
            NetStringList l;
            if (SendReq<NetStringList>("api/mgmt/rep/listpaper", Verb.GET, out l, out res, true) == false)
                return (null);
            return (l.Items);
        }

        public byte[] PaperGetTemplate(string Name)
        {
            NetByte l;
            if (SendReq<NetByte>("api/mgmt/rep/paper/" + Name, Verb.GET, out l, out res, true) == false)
                return (null);
            return (l.Data);
        }

        public byte[] PaperGetMachineReport(List<string> MachineIDs, DateTime? From, DateTime? To)
        {
            ReportPaperRequest req = new ReportPaperRequest();
            req.Name = "COMPUTERREPORT";
            req.From = From;
            req.To = To;
            req.MachineIDs = MachineIDs;
            NetByte l;
            if (SendReq<ReportPaperRequest, NetByte>("api/mgmt/rep/getpaper", Verb.POST, req, out l, out res, true) == false)
                return (null);
            return (l.Data);
        }

        public List<ContractInfos> GetContractInfos()
        {
            ContractInfosList cl;
            if (SendReq<ContractInfosList>("api/mgmt/getcontractinfos", Verb.GET, out cl, out res, true) == false)
                return (null);
            return (cl.Items);
        }

        public byte[] PaperTest(string Name)
        {
            NetByte l;
            if (SendReq<NetByte>("api/mgmt/rep/testpaper/" + Name, Verb.GET, out l, out res, true) == false)
                return (null);
            return (l.Data);
        }

        public PushDataRoot GetPushData0()
        {
            PushDataRoot pd;
            bool resb = SendReq<PushDataRoot>("api/httppush/service", Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd);
        }

        public PushDataRoot GetPushData1()
        {
            PushDataRoot pd;
            bool resb = SendReq<PushDataRoot>("api/httppush/1service", Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd);
        }

        public PushDataRoot GetPushData2()
        {
            PushDataRoot pd;
            bool resb = SendReq<PushDataRoot>("api/httppush/2service", Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd);
        }

        public PushDataRoot GetPushData3()
        {
            PushDataRoot pd;
            bool resb = SendReq<PushDataRoot>("api/httppush/3service", Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd);
        }

        public PushDataRoot GetPushData10()
        {
            PushDataRoot pd;
            bool resb = SendReq<PushDataRoot>("api/httppush/Aservice", Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd);
        }

        public bool ResponsePushData1(object data, string Action, Int64 Channel, string ReplyID)
        {
            PushDataResponse resp = new PushDataResponse();
            resp.TimeStampCheck = DateTime.UtcNow;
            resp.ReplyID = ReplyID;
            resp.Data = data;
            resp.Action = Action;
            resp.Channel = Channel;
            bool resb = SendReq<PushDataResponse>("api/httppush/r2sponse", Verb.POST, resp, out res);
            return (resb);
        }

        public bool ResponsePushData2(object data, string Action, Int64 Channel, string ReplyID)
        {
            PushDataResponse resp = new PushDataResponse();
            resp.TimeStampCheck = DateTime.UtcNow;
            resp.ReplyID = ReplyID;
            resp.Data = data;
            resp.Action = Action;
            resp.Channel = Channel;
            bool resb = SendReq<PushDataResponse>("api/httppush/r3sponse", Verb.POST, resp, out res);
            return (resb);
        }

        public bool ResponsePushData3(object data, string Action, Int64 Channel, string ReplyID)
        {
            PushDataResponse resp = new PushDataResponse();
            resp.TimeStampCheck = DateTime.UtcNow;
            resp.ReplyID = ReplyID;
            resp.Data = data;
            resp.Action = Action;
            resp.Channel = Channel;
            bool resb = SendReq<PushDataResponse>("api/httppush/r4sponse", Verb.POST, resp, out res);
            return (resb);
        }

        public bool ResponsePushData10(object data, string Action, Int64 Channel, string ReplyID)
        {
            PushDataResponse resp = new PushDataResponse();
            resp.TimeStampCheck = DateTime.UtcNow;
            resp.ReplyID = ReplyID;
            resp.Data = data;
            resp.Action = Action;
            resp.Channel = Channel;
            bool resb = SendReq<PushDataResponse>("api/httppush/rBsponse", Verb.POST, resp, out res);
            return (resb);
        }

        public bool ResponsePushData0(object data, string Action, Int64 Channel, string ReplyID)
        {
            PushDataResponse resp = new PushDataResponse();
            resp.TimeStampCheck = DateTime.UtcNow;
            resp.ReplyID = ReplyID;
            resp.Data = data;
            resp.Action = Action;
            resp.Channel = Channel;
            bool resb = SendReq<PushDataResponse>("api/httppush/response", Verb.POST, resp, out res);
            return (resb);
        }

        public bool WriteMessage(WriteMessage msg)
        {
            bool resb = SendReq<WriteMessage>("api/reports/writemessage", Verb.POST, msg, out res);
            if (res != 200 && res != 204)
                return (false);
            return (resb);
        }

        public bool ReportChatMessage(string Name, string Message)
        {
            PushChatMessage message = new PushChatMessage();
            message.Name = Name;
            message.Text = Message;
            bool resb = SendReq<PushChatMessage>("api/reports/chatmessage", Verb.POST, message, out res);
            if (res != 200 && res != 204)
                return (false);
            return (resb);
        }

        public List<string> GetPendingChats()
        {
            NetStringList pd;
            bool resb = SendReq<NetStringList>("api/mgmt/getpendingchats", Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd.Items);
        }

        public List<PushChatMessage> GetChatMessages(string MachineID)
        {
            PushChatMessageList pd;
            bool resb = SendReq<PushChatMessageList>("api/mgmt/getpendingchatdata/" + MachineID, Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd.List);
        }

        public List<PushChatMessage> GetChatMessagesForClient()
        {
            PushChatMessageList pd;
            bool resb = SendReq<PushChatMessageList>("api/reports/getchatmessages", Verb.GET, out pd, out res, true);
            if (pd == null)
                return (null);
            return (pd.List);
        }

        public bool ConfirmChat(Int64 ID)
        {
            bool resb = SendReq("api/reports/confirmchitchat/" + ID.ToString(), Verb.GET, out res, true);
            return (resb);
        }

        public FileUploadDataSigned File_Agent_GetFileData(Int64 ID)
        {
            FileUploadDataSigned fur;
            bool resb = SendReq<FileUploadDataSigned>("api/agent/filedata/" + ID.ToString(), Verb.GET, out fur, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (fur);
        }

        public FileUploadDataSigned File_Agent_GetFileAnyData(Int64 ID)
        {
            FileUploadDataSigned fur;
            bool resb = SendReq<FileUploadDataSigned>("api/agent/fileanydata/" + ID.ToString(), Verb.GET, out fur, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (fur);
        }

        public bool File_Agent_CancelUpload(Int64 ID)
        {
            bool resb = SendReq("api/agent/filecancelupload/" + ID.ToString(), Verb.GET, out res);
            if (resb == false)
                return (false);
            if (res != 200)
                return (false);
            return (true);
        }

        public bool File_Agent_AppendUpload(Int64 ID, byte[] data)
        {
            FileUploadAppendData ud = new FileUploadAppendData();
            ud.ID = ID;
            ud.Data = data;
            ud.Size = data.Length;
            ud.MD5 = MD5Utilities.CalcMD5(data);

            bool resb = SendReq<FileUploadAppendData>("api/agent/fileappendupload", Verb.POST, ud, out res, true);
            if (resb == false)
                return (false);
            if (res != 200)
                return (false);
            return (true);
        }

        public NetInt64ListSigned File_Agent_GetFileList()
        {
            NetInt64ListSigned fur;
            bool resb = SendReq<NetInt64ListSigned>("api/agent/fileidlist", Verb.GET, out fur, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (fur);
        }

        public Int64? File_Agent_NewUploadReq(string Filename, bool OverrideMeteredConnection, string MD5)
        {
            FileUploadData req = new FileUploadData();
            FileInfo fi = new FileInfo(Filename);
            req.RemoteFileLocation = Filename;
            req.FileLastModified = fi.LastWriteTimeUtc;
            req.MD5CheckSum = MD5;
            req.OverrideMeteredConnection = OverrideMeteredConnection;
            req.Size = fi.Length;

            NetInt64 nid;
            bool resb = SendReq<FileUploadData, NetInt64>("api/agent/filenewupload", Verb.POST, req, out nid, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (nid.Data);
        }

        public Int64? File_MGMT_NewUploadReq(string LocalFilename, string RemoteFilename, string MachineID, string MD5CheckSum, bool OverrideMeteredConnection)
        {
            FileUploadData req = new FileUploadData();
            FileInfo fi = new FileInfo(LocalFilename);
            req.RemoteFileLocation = RemoteFilename;
            req.MachineID = MachineID;
            req.FileLastModified = fi.LastWriteTimeUtc;
            req.MD5CheckSum = MD5CheckSum;
            req.OverrideMeteredConnection = OverrideMeteredConnection;
            req.Size = fi.Length;

            NetInt64 nid;
            bool resb = SendReq<FileUploadData, NetInt64>("api/mgmt/filenewupload", Verb.POST, req, out nid, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (nid.Data);
        }

        public Int64? File_MGMT_NewAgentUploadReq(string RemoteFilename, string MachineID, bool OverrideMeteredConnection)
        {
            FileUploadData req = new FileUploadData();
            req.MachineID = MachineID;
            req.RemoteFileLocation = RemoteFilename;
            req.OverrideMeteredConnection = OverrideMeteredConnection;

            NetInt64 nid;
            bool resb = SendReq<FileUploadData, NetInt64>("api/mgmt/filenewrequpload", Verb.POST, req, out nid, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (nid.Data);
        }

        public FileUploadDataSigned File_MGMT_GetFileData(Int64 ID)
        {
            FileUploadDataSigned fur;
            bool resb = SendReq<FileUploadDataSigned>("api/mgmt/filedata/" + ID.ToString(), Verb.GET, out fur, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (fur);
        }

        public bool File_MGMT_CancelUpload(Int64 ID)
        {
            bool resb = SendReq("api/mgmt/filecancelupload/" + ID.ToString(), Verb.GET, out res);
            if (resb == false)
                return (false);
            if (res != 200)
                return (false);
            return (true);
        }

        public bool File_MGMT_AppendUpload(string MachineID, Int64 ID, byte[] data)
        {
            FileUploadAppendData ud = new FileUploadAppendData();
            ud.ID = ID;
            ud.Data = data;
            ud.Size = data.Length;
            ud.MachineID = MachineID;
            ud.MD5 = MD5Utilities.CalcMD5(data);

            bool resb = SendReq<FileUploadAppendData>("api/mgmt/fileappendupload", Verb.POST, ud, out res, true);
            if (resb == false)
                return (false);
            if (res != 200)
                return (false);
            return (true);
        }

        public List<FileUploadData> File_MGMT_GetFullFileList(string MachineID)
        {
            FileUploadDataList fur;
            bool resb = SendReq<FileUploadDataList>("api/mgmt/filefulllist/" + MachineID, Verb.GET, out fur, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (fur.List);
        }

        public List<Int64> File_MGMT_GetFileList(string MachineID)
        {
            NetInt64List fur;
            bool resb = SendReq<NetInt64List>("api/mgmt/fileidlist/" + MachineID, Verb.GET, out fur, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (fur.data);
        }

        //Use DownloadFile()  api/agent/filefiledownload/<id>

        public List<SimpleTaskLite> GetSimpleTasks(string MachineID)
        {
            SimpleTaskLiteList po;
            bool resb = SendReq<SimpleTaskLiteList>("api/mgmt/liststasks/" + MachineID, Verb.GET, out po, out res);
            if (resb == true)
                return (po.List);
            else
                return (null);
        }

        public SimpleTask GetSimpleTaskDetails(Int64 ID)
        {
            SimpleTask po;
            bool resb = SendReq<SimpleTask>("api/mgmt/getstask/" + ID.ToString(), Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public Int64? SetSimpleTask(string Name, string MachineID, int Type, object data)
        {
            SimpleTask st = new SimpleTask();
            st.Data = JsonConvert.SerializeObject(data);
            st.MachineID = MachineID;
            st.Type = Type;
            st.Name = Name;
            return (SetSimpleTask(st));
        }

        public Int64? SetSimpleTask(SimpleTask st)
        {
            NetInt64 po;
            bool resb = SendReq<SimpleTask, NetInt64>("api/mgmt/setstask", Verb.POST, st, out po, out res);
            if (resb == false)
                return (null);
            else
                return (po.Data);
        }

        public Int64? PutSimpleTaskAside(Int64 st)
        {
            NetInt64 po;
            NetInt64 s = new NetInt64();
            s.Data = st;
            bool resb = SendReq<NetInt64, NetInt64>("api/agent/staskputaside", Verb.POST, s, out po, out res);
            if (resb == false)
                return (null);
            else
                return (po.Data);
        }

        public bool DeleteSimpleTask(Int64 ID)
        {
            bool resb = SendReq("api/mgmt/setstask/" + ID.ToString(), Verb.DELETE, out res);
            return (resb);
        }

        public SimpleTaskDataSigned GetSimpleTaskSigned()
        {
            SimpleTaskDataSigned po;
            bool resb = SendReq<SimpleTaskDataSigned>("api/agent/stasksigned", Verb.GET, out po, out res);
            if (resb == true)
                return (po);
            else
                return (null);
        }

        public bool CompleteSimpleTask(SimpleTaskResult st)
        {
            bool resb = SendReq<SimpleTaskResult>("api/agent/staskcompleted", Verb.POST, st, out res);
            if (resb == false)
                return (false);
            else
                return (true);
        }

        public UserInfo GetCurrentUserInfo()
        {
            UserInfo po;
            bool resb = SendReq<UserInfo>("api/mgmt/login/user/userinfo", Verb.GET, out po, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (po);
        }

        public List<UserDetails> GetAllUsers()
        {
            UserDetailsList po;
            bool resb = SendReq<UserDetailsList>("api/mgmt/login/user/allusers", Verb.GET, out po, out res);
            if (resb == false)
                return (null);
            if (res != 200)
                return (null);
            return (po.List);
        }

        public bool ChangeUser(UserDetailsPassword User)
        {
            bool resb = SendReq<UserDetailsPassword>("api/mgmt/login/user/changeuser", Verb.GET, User, out res);
            if (res != 200)
                return (false);
            return (resb);
        }

        public bool AddUser(string User)
        {
            bool resb = SendReq<NetString>("api/mgmt/login/user/adduser", Verb.GET, new NetString() { Data = User }, out res);
            if (res != 200)
                return (false);
            return (resb);
        }

        public bool DeleteUser(string User)
        {
            bool resb = SendReq<NetString>("api/mgmt/login/user/deleteuser", Verb.GET, new NetString() { Data = User }, out res);
            if (res != 200)
                return (false);
            return (resb);
        }

        public RecoveryData GetRecoveryLogon(RecoveryLogon Logon)
        {
            RecoveryData rd;
            bool resb = SendReq<RecoveryLogon, RecoveryData>("api/login/recovery", Verb.POST, Logon, out rd, out res);
            if (resb == true)
                return (rd);
            else
                return (null);
        }
    }
}
