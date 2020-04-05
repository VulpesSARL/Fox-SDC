using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public interface CPPInterface
    {
        bool IsInHypervisor();
        bool StartAppAsUser(string Filename, string Args);
        bool StartAppInWinLogon(string Filename, string Args, out int ProcessID);
        bool SetToken();
        bool VerifyEXESignature(string Filename);
        bool SetDateTime(DateTime UTCTime);
        string FoxGetFirmwareType();
        PushRunningSessionList GetActiveTSSessions();
        bool StartAppAsUser(string Filename, string Args, int SessionID);
        int StartAppAsUserID(string Filename, string Args, int SessionID);
        Int64 WGetLastError();
        bool StartAppAsUserWait(string Filename, string Args, int SessionID);
        void RestartSystem();
        void RestartSystemForced();
        CPPFrameBufferData GetFrameBufferData();
        Int64 MoveMouse(int X, int Y, int delta, int flags);
        Int64 SetKeyboard(int VirtualKey, int ScanCode, int Flags);
        Int64 GetConsoleSessionID();
        void SendCTRLALTDELETE();
        int SetKeyboardLayout(Int64 ID);
        void TypeKeyboardChar(char ch);
        bool GetEFIBootDevices(out Dictionary<int, string> Dict);
        bool SetEFINextBootDevice(int ID);
        List<List<string>> DNSQueryTXT(string Name);
    }

    public class CPPFrameBufferData
    {
        public int X;
        public int Y;
        public Byte[] Data;
        public int CursorX;
        public int CursorY;
        public int CursorType;

        public int FailedAt;
        public bool Failed;
        public int Win32Error;
    }
}
