using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Push
{
    class TaskManager
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
        [DllImport("ntdll.dll")]
        static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWow64Process([In] IntPtr processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);


        [StructLayout(LayoutKind.Sequential)]
        public struct ParentProcessUtilities
        {
            internal IntPtr Reserved1;
            internal IntPtr PebBaseAddress;
            internal IntPtr Reserved2_0;
            internal IntPtr Reserved2_1;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;
        }

        public static string KillTask(string PIDstring)
        {
            int PID;
            if (int.TryParse(PIDstring, out PID) == false)
                return ("failed");
            try
            {
                Process proc = Process.GetProcessById(PID);
                proc.Kill();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return ("failed");
            }
            return ("ok");
        }

        public static PushRunningSessionList GetTSRunningSessions()
        {
            return (ProgramAgent.CPP.GetActiveTSSessions());
        }

        public static PushTaskManagerList GetTasks()
        {
            PushTaskManagerList tl = new PushTaskManagerList();
            tl.Tasks = new List<PushTaskManagerListElement>();

            ManagementClass mc = new ManagementClass("Win32_Process");
            foreach (ManagementBaseObject mo in mc.GetInstances())
            {
                PushTaskManagerListElement element = new PushTaskManagerListElement();
                element.ProcessID = Convert.ToInt32(mo["ProcessId"]);
                element.Filename = Convert.ToString(mo["ExecutablePath"]);
                element.Arguments = Convert.ToString(mo["CommandLine"]);
                element.ProcessName = Convert.ToString(mo["Caption"]);
                element.SessionID = Convert.ToInt32(mo["SessionId"]);
                element.StartTime = ManagementDateTimeConverter.ToDateTime(Convert.ToString(mo["CreationDate"])).ToUniversalTime();
                element.UserProcessorTime = TimeSpan.FromTicks(Convert.ToInt64(mo["UserModeTime"]));
                element.TotalProcessorTime = TimeSpan.FromTicks(Convert.ToInt64(mo["UserModeTime"]) + Convert.ToInt64(mo["KernelModeTime"]));
                element.WorkingSet = Convert.ToInt64(mo["WorkingSetSize"]);
                element.PrivateBytes = Convert.ToInt64(mo["VirtualSize"]);

                if (string.IsNullOrWhiteSpace(element.Filename) == false)
                {
                    FileVersionInfo ver = FileVersionInfo.GetVersionInfo(element.Filename);
                    element.CompanyName = ver.CompanyName;
                    element.Description = ver.FileDescription;
                }

                element.Username = "N/A";
                element.ParentProcessID = 0;
                element.IsWOWProcess = false;

                Process proc = Process.GetProcessById(element.ProcessID);
                IntPtr processHandle = IntPtr.Zero;
                try
                {
                    if (OpenProcessToken(proc.Handle, 8, out processHandle) == true)
                    {
                        WindowsIdentity wi = new WindowsIdentity(processHandle);
                        element.Username = wi.Name;

                        ParentProcessUtilities pbi = new ParentProcessUtilities();
                        int returnLength;
                        int status = NtQueryInformationProcess(proc.Handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
                        if (status == 0)
                        {
                            element.ParentProcessID = pbi.InheritedFromUniqueProcessId.ToInt32();
                        }

                        IsWow64Process(proc.Handle, out element.IsWOWProcess);
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                }
                finally
                {
                    if (processHandle != IntPtr.Zero)
                        CloseHandle(processHandle);
                }

                tl.Tasks.Add(element);
            }

            return (tl);
        }

        public static PushRunTaskResult RunTask(string ReqString, Network net)
        {
            PushRunTaskResult Res = new PushRunTaskResult();
            PushRunTask req;
            try
            {
                req = JsonConvert.DeserializeObject<PushRunTask>(ReqString);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                Res.Result = 0x16;
                return (Res);
            }

            Res.Result = 0;
#if !DEBUG || DEBUGSERVICE
            int ProcessID;
#endif

            switch (req.Option)
            {
                case PushRunTaskOption.ActualUser:
#if !DEBUG || DEBUGSERVICE
                    if (SystemInfos.SysInfo.RunningInWindowsPE == true)
                    {
                        if (ProgramAgent.CPP.StartAppInWinLogon(req.Executable, req.Args, out ProcessID) == false)
                            Res.Result = ProgramAgent.CPP.WGetLastError();
                    }
                    else
                    {
                        if (ProgramAgent.CPP.StartAppAsUser(req.Executable, req.Args, req.SessionID) == false)
                            Res.Result = ProgramAgent.CPP.WGetLastError();
                    }
#else
                    //Crude: since we cannot use WTSQueryUserToken function as normal user / nor admin user
                    try
                    {
                        Process.Start(req.Executable, req.Args);
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                    }
#endif
                    break;
                case PushRunTaskOption.OtherUser:
                    DataHRunasUserResult ores;
                    if (Process2ProcessComm.InvokeRunAsUser(req.Executable, req.Args, req.Username, req.Password, req.SessionID, out ores, out Res.Result) == true)
                    {
                        Res.Result = ores.Result;
                    }
                    break;
                case PushRunTaskOption.SystemUserConsoleRedir:
                    Res.SessionID = Redirs.MainSTDIORedir.StartRedir(req.Executable, req.Args, net);
                    if (string.IsNullOrWhiteSpace(Res.SessionID) == true)
                        Res.Result = 0x8000FFFF;
                    else
                        Res.Result = 0;
                    break;
            }


            return (Res);
        }

    }
}
