using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class PushClock
    {
        public DateTime UTCDT;
    }

    public class PushTaskManagerList
    {
        public List<PushTaskManagerListElement> Tasks;
    }

    public class PushTaskManagerListElement
    {
        public int ProcessID;
        public string Filename;
        public string CompanyName;
        public string Description;
        public string ProcessName;
        public int SessionID;
        public string Arguments;
        public DateTime StartTime;
        public string Username;
        public int ParentProcessID;
        public bool IsWOWProcess;
        public TimeSpan TotalProcessorTime;
        public TimeSpan UserProcessorTime;
        public Int64 PrivateBytes;
        public Int64 WorkingSet;
    }

    public class PushServicesInfo
    {
        public string Name;
        public string DisplayName;
        public string DisplayRegistryName;
        public string Description;
        public string RegistryDescription;
        public string ServiceType;
        public string PathName;
        public string State;
        public string StartMode;
        public string StartName;
        public bool DelayedAutoStart;
        public Int64 ProcessId;
        public bool Started;
        public bool DesktopInteract;
        public bool AcceptPause;
        public bool AcceptStop;
        public Int64 ServiceSpecificExitCode;
        public Int64 ExitCode;
        public string ErrorControl;
    }

    public class PushServicesInfoList
    {
        public List<PushServicesInfo> Data;
    }

    public class PushDirListReq
    {
        public bool ShowFolders;
        public bool ShowFiles;
        public string Folder;
        public string Filter;
    }

    public class PushRunningSessionElement
    {
        public int SessionID;
        public string User;
        public string Domain;
    }

    public enum PushRunTaskOption : int
    {
        ActualUser = 0,
        OtherUser = 1,
        SystemUserConsoleRedir = 2
    }

    public class PushRunTask
    {
        public string Executable;
        public string Args;
        public PushRunTaskOption Option;
        public int SessionID;
        public string Username;
        public string Password;
    }

    public class PushRunTaskResult
    {
        public Int64 Result;
        public string SessionID;
    }

    public class PushRunningSessionList
    {
        public List<PushRunningSessionElement> Data;
    }

    public enum PushFileState : int
    {
        NotExistent = 0,
        File = 1,
        Folder = 2,
        Error = 99,
        RemoteError = 98
    }

    public enum PushStdoutState : int
    {
        OK = 0,
        End = 1,
        Timeout = 2,
        InternalError = 3
    }

    public enum PushStdInState : int
    {
        Normal = 0,
        CTRL_C = 1,
        CTRL_BREAK = 2
    }

    public class Push_Stdio_StdIn
    {
        public string SessionID;
        public ConsoleUtilities.INPUT_RECORD_Flat[] data;
        public PushStdInState State;
    }

    public class Push_Stdio_StdOut
    {
        public string SessionID;
        public int WindowsX;
        public int WindowsY;
        public int CursorX;
        public int CursorY;
        public ConsoleUtilities.CHAR_INFO2[] Data;
        public PushStdoutState State;
    }

    public class PushConnectNetwork
    {
        public string Address;
        public int Port;
    }

    public class PushConnectNetworkResult
    {
        public int Result;
        public string ConnectedGUID;
    }

    public class PushConnectNetworkData
    {
        public string GUID;
        public byte[] data;
        public int Result;
        public Int64 Seq;
    }

    public class PushServiceControlState
    {
        public Int64 ResultCode;
    }

    public class PushServiceControlReq
    {
        public string Service;
        public int Control;
    }

    public class PushScreenData
    {
        public int X;
        public int Y;
        public int DataType;
        public Int64 FailedCode;
        public byte[] Data;
        public int CursorX;
        public int CursorY;
        public List<Int64> ChangedBlocks;
        public int BlockX;
        public int BlockY;
    }

    public class PushMouseData
    {
        public int X;
        public int Y;
        public int Delta;
        public int Flags;
    }
       
    [Flags]
    public enum MouseDataFlags : int
    {
        LeftButton = 0x1,
        MiddleButton = 0x2,
        RightButton = 0x4,
        XButton1 = 0x8,
        XButton2 = 0x10
    }

    public class PushKeyboardData
    {
        public int VirtualKey;
        public int ScanCode;
        public int Flags;
    }

    public class PushChatMessage
    {
        public DateTime DT;
        public string Name;
        public string Text;
        public Int64 ID;
    }

    public class PushChatMessageList
    {
        public List<PushChatMessage> List;
    }
}
