using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public partial class Network
    {
        public DateTime? PushGetClock(string MachineID)
        {
            PushClock clock;
            bool resb = SendReq<PushClock>("api/pushy/clock/" + MachineID, Verb.GET, out clock, out res, true);
            if (resb == true)
                return (clock.UTCDT);
            else
                return (null);
        }

        public bool PushPing(string MachineID)
        {
            NetBool pres;
            bool resb = SendReq<NetBool>("api/pushy/ping/" + MachineID, Verb.GET, out pres, out res, true);
            if (pres == null)
                return (false);
            else
                return (pres.Data);
        }

        public List<PushServicesInfo> PushGetServices(string MachineID)
        {
            PushServicesInfoList pres;
            bool resb = SendReq<PushServicesInfoList>("api/pushy/listservices/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres.Data);
        }

        public Dictionary<int,string> GetEFIBootDevices(string MachineID)
        {
            NetDictIntString pres;
            bool resb = SendReq<NetDictIntString>("api/pushy/listefibootdevices/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres.Dict);
        }

        public bool SetEFINextBootDevice(string MachineID, int ID)
        {
            NetInt32 p = new NetInt32();
            p.Data = ID;
            NetBool pres;
            bool resb = SendReq<NetInt32, NetBool>("api/pushy/listefibootdevices/" + MachineID, Verb.POST, p, out pres, out res, true);
            if (pres == null)
                return (false);
            else
                return (pres.Data);
        }

        public List<PushTaskManagerListElement> PushGetTasks(string MachineID)
        {
            PushTaskManagerList pres;
            bool resb = SendReq<PushTaskManagerList>("api/pushy/listtasks/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres.Tasks);
        }

        public bool PushKillTask(string MachineID, int PID)
        {
            NetInt32 p = new NetInt32();
            p.Data = PID;
            NetBool pres;
            bool resb = SendReq<NetInt32, NetBool>("api/pushy/killtask/" + MachineID, Verb.POST, p, out pres, out res, true);
            if (pres == null)
                return (false);
            else
                return (pres.Data);
        }

        public PushServiceControlState PushServiceControl(string MachineID, PushServiceControlReq ControlReq)
        {
            PushServiceControlState state;
            bool resb = SendReq<PushServiceControlReq, PushServiceControlState>("api/pushy/servicecontrol/" + MachineID, Verb.POST, ControlReq, out state, out res, true);
            if (state == null)
                return (null);
            else
                return (state);
        }

        public PushFileState PushCheckFileExistence(string MachineID, string File)
        {
            NetString p = new NetString();
            p.Data = File;
            NetInt32 pres;
            bool resb = SendReq<NetString, NetInt32>("api/pushy/checkfile/" + MachineID, Verb.POST, p, out pres, out res, true);
            if (pres == null)
                return (PushFileState.Error);
            else
                return ((PushFileState)pres.Data);
        }

        public List<string> PushGetFiles(string MachineID, bool ShowFiles, bool ShowFolders, string Folder, string Filter)
        {
            PushDirListReq r = new PushDirListReq();
            r.Filter = Filter;
            r.Folder = Folder;
            r.ShowFiles = ShowFiles;
            r.ShowFolders = ShowFolders;
            NetStringList pres;
            bool resb = SendReq<PushDirListReq, NetStringList>("api/pushy/listfiles/" + MachineID, Verb.POST, r, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres.Items);
        }

        public PushRunTaskResult PushRunFile(string MachineID, PushRunTask Task)
        {
            PushRunTaskResult pres;
            bool resb = SendReq<PushRunTask, PushRunTaskResult>("api/pushy/runtask/" + MachineID, Verb.POST, Task, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres);
        }

        public List<PushRunningSessionElement> PushGetSessions(string MachineID)
        {
            PushRunningSessionList pres;
            bool resb = SendReq<PushRunningSessionList>("api/pushy/getsessions/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres.Data);
        }

        public Push_Stdio_StdOut PushPopStdIO(string MachineID, string SessionID)
        {
            Push_Stdio_StdOut pres;
            NetString str = new NetString();
            str.Data = SessionID;
            bool resb = SendReq<NetString, Push_Stdio_StdOut>("api/pushy/popstdiodata/" + MachineID, Verb.POST, str, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres);
        }

        public bool PushPushStdIO(string MachineID, string SessionID, ConsoleUtilities.INPUT_RECORD_Flat[] Data)
        {
            if (Data.Length == 0)
                return (true);
            Push_Stdio_StdIn io = new Push_Stdio_StdIn();
            io.SessionID = SessionID;
            io.data = Data;
            io.State = PushStdInState.Normal;
            bool resb = SendReq<Push_Stdio_StdIn>("api/pushy/pushstdiodata/" + MachineID, Verb.POST, io, out res, true);
            return (resb);
        }

        public bool PushPushStdIO(string MachineID, Push_Stdio_StdIn io)
        {
            bool resb = SendReq<Push_Stdio_StdIn>("api/pushy/pushstdiodata/" + MachineID, Verb.POST, io, out res, true);
            return (resb);
        }

        public PushConnectNetworkResult PushConnectToRemote(string MachineID, string RemoteAddress, int RemotePort)
        {
            PushConnectNetworkResult pres;
            PushConnectNetwork str = new PushConnectNetwork();
            str.Address = RemoteAddress;
            str.Port = RemotePort;
            bool resb = SendReq<PushConnectNetwork, PushConnectNetworkResult>("api/pushy/connectremotereq/" + MachineID, Verb.POST, str, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres);
        }

        public PushConnectNetworkResult PushConnectToRemote2(string MachineID, string RemoteAddress, int RemotePort)
        {
            PushConnectNetworkResult pres;
            PushConnectNetwork str = new PushConnectNetwork();
            str.Address = RemoteAddress;
            str.Port = RemotePort;
            bool resb = SendReq<PushConnectNetwork, PushConnectNetworkResult>("api/pushy/wsconnectremotereq/" + MachineID, Verb.POST, str, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres);
        }

        public PushConnectNetworkResult PushConnectionToRemoteData(string MachineID, string ConnectionGUID, byte[] data, Int64 Sequence)
        {
            PushConnectNetworkData d = new PushConnectNetworkData();
            d.data = data;
            d.GUID = ConnectionGUID;
            d.Seq = Sequence;

            PushConnectNetworkResult b;
            bool resb = SendReq<PushConnectNetworkData, PushConnectNetworkResult>("api/pushy/connectremotedata/" + MachineID, Verb.POST, d, out b, out res, true);
            if (resb == false)
                return (null);
            if (b == null)
                return (null);
            return (b);
        }

        public PushConnectNetworkResult PushConnectionToRemoteClose(string MachineID, string ConnectionGUID)
        {
            PushConnectNetworkData d = new PushConnectNetworkData();
            d.data = null;
            d.GUID = ConnectionGUID;

            PushConnectNetworkResult b;
            bool resb = SendReq<PushConnectNetworkData, PushConnectNetworkResult>("api/pushy/connectremoteclosedata/" + MachineID, Verb.POST, d, out b, out res, true);
            if (resb == false)
                return (null);
            if (b == null)
                return (null);
            return (b);
        }

        public PushConnectNetworkResult PushConnectionToRemoteClose2(string MachineID, string ConnectionGUID)
        {
            PushConnectNetworkData d = new PushConnectNetworkData();
            d.data = null;
            d.GUID = ConnectionGUID;

            PushConnectNetworkResult b;
            bool resb = SendReq<PushConnectNetworkData, PushConnectNetworkResult>("api/pushy/wsconnectremoteclosedata/" + MachineID, Verb.POST, d, out b, out res, true);
            if (resb == false)
                return (null);
            if (b == null)
                return (null);
            return (b);
        }

        public PushConnectNetworkData PushConnectionFromRemoteData(string MachineID, string ConnectionGUID)
        {
            PushConnectNetworkData d = new PushConnectNetworkData();
            d.data = null;
            d.GUID = ConnectionGUID;

            PushConnectNetworkData b;
            bool resb = SendReq<PushConnectNetworkData, PushConnectNetworkData>("api/pushy/connectremotegetdata/" + MachineID, Verb.POST, d, out b, out res, true);
            if (resb == false)
                return (null);
            if (b == null)
                return (null);
            return (b);
        }

        public List<WUUpdateInfo> PushWUGetList(string MachineID)
        {
            WUUpdateInfoList pres;
            bool resb = SendReq<WUUpdateInfoList>("api/pushy/wugetlist/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres.List);
        }

        public WUStatus PushWUStatus(string MachineID)
        {
            WUStatus pres;
            bool resb = SendReq<WUStatus>("api/pushy/wustatus/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            if (pres == null)
                return (null);
            else
                return (pres);
        }

        public bool PushWUStatusRestart(string MachineID)
        {
            NetBool pres;
            bool resb = SendReq<NetBool>("api/pushy/wustatusrestart/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (false);
            if (pres == null)
                return (false);
            else
                return (pres.Data);
        }

        public bool PushWUCheck(string MachineID)
        {
            bool resb = SendReq("api/pushy/wucheck/" + MachineID, Verb.GET, out res, true);
            if (resb == false)
                return (false);
            return (true);
        }

        public bool PushWUInstall(string MachineID)
        {
            bool resb = SendReq("api/pushy/wuinstall/" + MachineID, Verb.GET, out res, true);
            if (resb == false)
                return (false);
            return (true);
        }

        public bool PushClientRestart(string MachineID)
        {
            bool resb = SendReq("api/pushy/restartclient/" + MachineID, Verb.GET, out res, true);
            if (resb == false)
                return (false);
            return (true);
        }

        public bool PushClientRestartForced(string MachineID)
        {
            bool resb = SendReq("api/pushy/restartforcedclient/" + MachineID, Verb.GET, out res, true);
            if (resb == false)
                return (false);
            return (true);
        }

        public PushScreenData PushGetScreenDataFull(string MachineID)
        {
            PushScreenData pres;
            bool resb = SendReq<PushScreenData>("api/pushy/getscreenbuffer/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            return (pres);
        }

        public PushConnectNetworkResult PushCreateWSScreenconnection(string MachineID)
        {
            PushConnectNetworkResult pres;
            bool resb = SendReq<PushConnectNetworkResult>("api/pushy/wscreatescreenconnection/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            return (pres);
        }

        public PushScreenData PushGetScreenDataDelta(string MachineID)
        {
            PushScreenData pres;
            bool resb = SendReq<PushScreenData>("api/pushy/getscreendelta/" + MachineID, Verb.GET, out pres, out res, true);
            if (resb == false)
                return (null);
            return (pres);
        }

        public bool PushSetMouse(string MachineID, int X, int Y, int Delta, int Flags)
        {
            PushMouseData m = new PushMouseData();
            m.Flags = Flags;
            m.X = X;
            m.Y = Y;
            bool resb = SendReq<PushMouseData>("api/pushy/setmousedata/" + MachineID, Verb.POST, m, out res, true);
            return (resb);
        }

        public bool PushSetKeyboard(string MachineID, int VirtualKey, int ScanCode, int Flags)
        {
            PushKeyboardData k = new PushKeyboardData();
            k.Flags = Flags;
            k.ScanCode = ScanCode;
            k.VirtualKey = VirtualKey;
            bool resb = SendReq<PushKeyboardData>("api/pushy/setkeyboarddata/" + MachineID, Verb.POST, k, out res, true);
            return (resb);
        }

        public bool PushSendChat(string MachineID, string Name, string Text)
        {
            PushChatMessage message = new PushChatMessage();
            message.Name = Name;
            message.Text = Text;
            bool resb = SendReq<PushChatMessage>("api/pushy/sendchatmessage/" + MachineID, Verb.POST, message, out res, true);
            return (resb);
        }
    }
}
