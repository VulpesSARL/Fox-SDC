using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    class SessionStarter
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSQueryUserToken(int sessionId, out IntPtr Token);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("userenv.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyEnvironmentBlock(IntPtr lpEnvironment);

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        
        private const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;

        static void StartApplication(string Filename, int toSessionID)
        {
            try
            {
                IntPtr SessionToken;
                if (WTSQueryUserToken(toSessionID, out SessionToken) == false)
                    return;

                STARTUPINFO start = new STARTUPINFO();
                start.cb = Marshal.SizeOf(typeof(STARTUPINFO));
                start.lpDesktop = "winsta0\\default";
                PROCESS_INFORMATION proc;

                IntPtr Environment = IntPtr.Zero;
                if (CreateEnvironmentBlock(out Environment, SessionToken, false) == false)
                    Environment = IntPtr.Zero;

                if (CreateProcessAsUser(SessionToken, Filename, null, IntPtr.Zero, IntPtr.Zero, false, CREATE_UNICODE_ENVIRONMENT, Environment, null, ref start, out proc) == false)
                {
                    Debug.WriteLine("CreateProcess FAILED " + Marshal.GetLastWin32Error().ToString());
                }
                else
                {
                    CloseHandle(proc.hProcess);
                    CloseHandle(proc.hThread);
                }

                if (Environment != IntPtr.Zero)
                    DestroyEnvironmentBlock(Environment);

                CloseHandle(SessionToken);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        public static void StartProgramInSessions(string Filename, List<int> Sessions)
        {
            if (Sessions == null)
                return;
            if (Sessions.Count == 0)
                return;

            foreach (int sess in Sessions)
                StartApplication(Filename, sess);
        }
    }
}
