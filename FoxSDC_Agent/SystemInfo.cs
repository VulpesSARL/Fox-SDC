using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Agent
{
    public static class SystemInfos
    {
        public static BaseSystemInfo SysInfo = null;
        public static string ServerURL = "";
        public static string PasswordID = "";
        public static string ProgramData = "";

        public static string ContractID;
        public static string ContractPassword;

        #region WIN32 HACKS

        [DllImport("kernel32.dll")]
        static extern void GetNativeSystemInfo(ref SystemInfo lpSystemInfo);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(ref SystemInfo lpSystemInfo);

        [StructLayout(LayoutKind.Sequential)]
        struct SystemInfo
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }

        const ushort ProcessorArchitectureIntel = 0;
        const ushort ProcessorArchitectureIa64 = 6;
        const ushort ProcessorArchitectureAmd64 = 9;
        const ushort ProcessorArchitectureARM = 5;
        const ushort ProcessorArchitectureARM64 = 12;
        const ushort ProcessorArchitectureUnknown = 0xFFFF;

        public enum CPUType
        {
            Intel32,
            EM64T,
            IA64,
            Unknown,
            ARM64,
            ARM
        }

        public static CPUType GetCPU()
        {
            SystemInfo SysInfoNative = new SystemInfo();
            GetNativeSystemInfo(ref SysInfoNative);

            if (SysInfoNative.wProcessorArchitecture == ProcessorArchitectureUnknown)
                return (CPUType.Unknown);
            if (SysInfoNative.wProcessorArchitecture == ProcessorArchitectureIntel)
                return (CPUType.Intel32);
            if (SysInfoNative.wProcessorArchitecture == ProcessorArchitectureAmd64)
                return (CPUType.EM64T);
            if (SysInfoNative.wProcessorArchitecture == ProcessorArchitectureIa64)
                return (CPUType.IA64);
            if (SysInfoNative.wProcessorArchitecture == ProcessorArchitectureARM)
                return (CPUType.ARM);
            if (SysInfoNative.wProcessorArchitecture == ProcessorArchitectureARM64)
                return (CPUType.ARM64);
            return (CPUType.Unknown);
        }

        #endregion

        #region TSE Hack

        [StructLayout(LayoutKind.Sequential)]
        struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] szCSDVersion;
            public Int16 wServicePackMajor;
            public Int16 wServicePackMinor;
            public Int16 wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }
        [DllImport("kernel32.dll")]
        static extern void GetVersionEx(ref OSVERSIONINFOEX lpVersionInfo);

        const int VER_SUITE_TERMINAL = 0x10;
        const int VER_SUITE_SINGLEUSERTS = 0x100;

        static bool IsSystemTSE()
        {
            OSVERSIONINFOEX os = new OSVERSIONINFOEX();
            os.dwOSVersionInfoSize = Marshal.SizeOf(os);
            GetVersionEx(ref os);
            if ((os.wSuiteMask & VER_SUITE_SINGLEUSERTS) == 0 && (os.wSuiteMask & VER_SUITE_TERMINAL) == VER_SUITE_TERMINAL)
                return (true);
            return (false);
        }

        enum OSSuite
        {
            Workstation,
            Server,
            DomainController,
            Unknown
        }

        static OSSuite GetOSSuite()
        {
            OSVERSIONINFOEX os = new OSVERSIONINFOEX();
            os.dwOSVersionInfoSize = Marshal.SizeOf(os);
            GetVersionEx(ref os);
            switch (os.wProductType)
            {
                case 0x1:
                    return (OSSuite.Workstation);
                case 0x2:
                    return (OSSuite.DomainController);
                case 0x3:
                    return (OSSuite.Server);
                default:
                    return (OSSuite.Unknown);
            }
        }


        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        static bool? Is64Bit()
        {
            bool res;
            if (IsWow64Process(Process.GetCurrentProcess().Handle, out res) == false)
                return (null);

            return (!res);
        }

        #endregion

        #region WMI Tricks

        static string GetWindowsProduct()
        {
            try
            {
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM  Win32_OperatingSystem");
                foreach (ManagementObject objManagement in objMOS.Get())
                {
                    object OS = objManagement.GetPropertyValue("Caption");
                    if (OS != null)
                        return (OS.ToString());
                    else
                        return ("?");
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return ("?");
            }
            return ("?");
        }

        static void GetProcessorInfos(out int NumProc, out int NumLogProc, out Int64 TotalMemory)
        {
            NumProc = NumLogProc = 0;
            TotalMemory = 0;
            try
            {
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM  Win32_ComputerSystem");
                foreach (ManagementObject objManagement in objMOS.Get())
                {
                    NumLogProc = Convert.ToInt32(objManagement.GetPropertyValue("NumberOfLogicalProcessors"));
                    NumProc = Convert.ToInt32(objManagement.GetPropertyValue("NumberOfProcessors"));
                    TotalMemory = Convert.ToInt64(objManagement.GetPropertyValue("TotalPhysicalMemory"));
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return;
            }
            return;
        }

        static string GetBIOSData()
        {
            try
            {
                string B = "";
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM  Win32_BIOS");
                foreach (ManagementObject objManagement in objMOS.Get())
                {
                    object Data;
                    Data = objManagement.GetPropertyValue("Manufacturer");
                    if (Data != null)
                        B += Data.ToString() + " ";
                    else
                        B += "? ";

                    Data = objManagement.GetPropertyValue("SerialNumber");
                    if (Data != null)
                        B += Data.ToString() + " ";
                    else
                        B += "? ";

                    Data = objManagement.GetPropertyValue("ReleaseDate");
                    if (Data != null)
                        B += Data.ToString() + " ";
                    else
                        B += "? ";

                    Data = objManagement.GetPropertyValue("SMBIOSBIOSVersion");
                    if (Data != null)
                        B += Data.ToString() + " ";
                    else
                        B += "? ";

                    break;
                }
                return (B.Trim());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return ("?");
            }
        }

        static string GetComputerModel()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                if (moc.Count > 0)
                {
                    foreach (ManagementObject mo in mc.GetInstances())
                    {
                        return (mo["Manufacturer"].ToString().Trim() + " " + mo["Model"].ToString().Trim()).Trim();
                    }
                }
                return ("");
            }
            catch
            {
                return (null);
            }
        }

        static string GetProcessorName()
        {
            try
            {
                string B = "";
                List<string> CPUs = new List<string>();
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject objManagement in objMOS.Get())
                {
                    string c = Convert.ToString(objManagement.GetPropertyValue("Name"));
                    if (CPUs.Contains(c) == false)
                        CPUs.Add(c);
                }
                foreach (string c in CPUs)
                {
                    B += c + ", ";
                }
                if (B.EndsWith(", ") == true)
                    B = B.Substring(0, B.Length - 2).Trim();
                return (B);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return ("");
            }
        }

        #endregion

        #region Wine Tricks

        [DllImport("ntdll.dll", CharSet = CharSet.Ansi, EntryPoint = "wine_get_version")]
        static extern string wine_get_version();

        static string GetWineVersion()
        {
            string ver = "";
            try
            {
                ver = wine_get_version();
            }
            catch
            {
                ver = "";
            }
            return (ver);
        }

        #endregion

        #region Mono Tricks
        static bool IsRunningOnMono()
        {
            return (Type.GetType("Mono.Runtime") != null);
        }

        #endregion

        #region Registry

        static public string GetSecureBootState()
        {
            RegistryKey reg = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\SecureBoot\\State");
            if (reg == null)
                return ("Unknown");
            object o = reg.GetValue("UEFISecureBootEnabled");
            if (o == null)
            {
                reg.Close();
                return ("Unknown");
            }
            reg.Close();
            int state = 0;
            if (int.TryParse(o.ToString(), out state) == false)
                return ("Unknown");
            if (state == 0)
                return ("Disabled");
            if (state == 1)
                return ("Enabled");
            return ("Unknown");
        }

        static string GetSUSID()
        {
            RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate");
            if (reg == null)
                return ("");
            object o = reg.GetValue("SusClientId");
            if (o == null)
            {
                reg.Close();
                return ("");
            }
            reg.Close();

            return (Convert.ToString(o));
        }


        #endregion

        public static int CollectSystemInfo()
        {
            if (IsRunningOnMono() == true)
            {
                FoxEventLog.WriteEventLog("Running on Mono is not supported!", EventLogEntryType.Error);
                Console.WriteLine("Running on Mono is not supported!");
                return (1);
            }

            if (GetWineVersion() != "")
            {
                FoxEventLog.WriteEventLog("Running on Wine/CrossOver is not supported!", EventLogEntryType.Error);
                Console.WriteLine("Running on Wine/CrossOver is not supported!");
                return (1);
            }

            SysInfo = new BaseSystemInfo();
            SysInfo.RunningInWindowsPE = WindowsPE.IsRunningInWindowsPE;
            SysInfo.ComputerModel = GetComputerModel();
            SysInfo.ComputerName = SystemInformation.ComputerName;
            SysInfo.CPU = GetCPU().ToString();
            SysInfo.MachineID = RegistryData.MachineID;
            SysInfo.IsTSE = IsSystemTSE();
            SysInfo.OSName = SysInfo.RunningInWindowsPE == true ? "Windows PE / MiniNT" : GetWindowsProduct();
            SysInfo.OSSuite = GetOSSuite().ToString();
            SysInfo.OSVerBuild = Environment.OSVersion.Version.Build;
            SysInfo.OSVerMaj = Environment.OSVersion.Version.Major;
            SysInfo.OSVerMin = Environment.OSVersion.Version.Minor;
            SysInfo.Language = CultureInfo.InstalledUICulture.Name;
            SysInfo.DisplayLanguage = CultureInfo.InstalledUICulture.EnglishName;
            SysInfo.RunningInHypervisor = ProgramAgent.CPP.IsInHypervisor();
            SysInfo.BIOS = GetBIOSData();
            SysInfo.BIOSType = ProgramAgent.CPP.FoxGetFirmwareType();
            GetProcessorInfos(out SysInfo.NumberOfProcessors, out SysInfo.NumberOfLogicalProcessors, out SysInfo.TotalPhysicalMemory);
            SysInfo.CPUName = GetProcessorName();
            SysInfo.SecureBootState = GetSecureBootState();
            SysInfo.SystemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            SysInfo.SUSID = GetSUSID();

            try
            {
                SysInfo.IsMeteredConnection = MeteredConnection.IsMeteredConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                SysInfo.IsMeteredConnection = null;
            }

            OSVERSIONINFOEX os = new OSVERSIONINFOEX();
            os.dwOSVersionInfoSize = Marshal.SizeOf(os);
            GetVersionEx(ref os);
            SysInfo.OSVerType = os.wSuiteMask;

#if DEBUG
            SysInfo.AgentVersion = VulpesBranding.AgentIdentifier + " [DEBUG] " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#else
            SysInfo.AgentVersion = VulpesBranding.AgentIdentifier + " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#endif
            SysInfo.AgentVersionID = FoxVersion.Version;

            CPUType CPU = GetCPU();

            if (CPU != CPUType.Intel32 && CPU != CPUType.EM64T && CPU != CPUType.ARM64)
            {
                FoxEventLog.WriteEventLog("Only i386, AMD64 (EM64T) or ARM64 CPU are supported!", EventLogEntryType.Error);
                return (1);
            }

            if (CPU == CPUType.EM64T)
            {
                if (Is64Bit() == null)
                {
                    FoxEventLog.WriteEventLog("Cannot determine WOW state.", EventLogEntryType.Error);
                    return (1);
                }

                if (Is64Bit() == false && CPU == CPUType.EM64T)
                {
                    FoxEventLog.WriteEventLog("If you've a 64 bit CPU, then run this process as 64 bit!!!!", EventLogEntryType.Error);
                    return (1);
                }

                SysInfo.Is64Bit = Is64Bit().Value;
            }
            else
            {
                SysInfo.Is64Bit = false;
            }

            if (SysInfo.MachineID == "")
            {
                FoxEventLog.WriteEventLog("Cannot get machine ID.", EventLogEntryType.Error);
                return (1);
            }

            if (SysInfo.ComputerModel == null)
            {
                FoxEventLog.WriteEventLog("Cannot get Computer Model.", EventLogEntryType.Error);
                return (1);
            }

            PasswordID = RegistryData.MachinePassword;
            if (PasswordID == "")
            {
                FoxEventLog.WriteEventLog("Cannot get PassID.", EventLogEntryType.Error);
                return (1);
            }

            if (SysInfo.OSSuite == "Unknown")
            {
                FoxEventLog.WriteEventLog("Unknown OS Suite.", EventLogEntryType.Error);
                return (1);
            }

            if (RegistryData.UseOnPrem == true)
                ServerURL = RegistryData.ServerURL;
            else
                ServerURL = ProgramAgent.VulpesURL;

            if (ServerURL == "")
            {
                FoxEventLog.WriteEventLog("Cannot get Server URL.", EventLogEntryType.Error);
                return (1);
            }

            ProgramData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (ProgramData.EndsWith("\\") == false)
                ProgramData += "\\";
            ProgramData += "Fox\\SDC Agent\\";

            try
            {
                if (Directory.Exists(ProgramData) == false)
                {
                    Directory.CreateDirectory(ProgramData);
                }
            }
            catch
            {
                FoxEventLog.WriteEventLog("Error accessing " + ProgramData, EventLogEntryType.Error);
                return (1);
            }

            SysInfo.UCID = UCID.GetUCID();
            SysInfo.LegacyUCID = UCID.GetUCIDLegacy();

            if (RegistryData.UCIDOverride != "")
            {
                string UUU = RegistryData.UCIDOverride.Trim().ToUpper();
                if (UUU.Length == 32)
                {
                    FoxEventLog.WriteEventLog("UCID Overriden from " + SysInfo.UCID + " to " + UUU, EventLogEntryType.Warning);
                    SysInfo.UCID = UUU;
                }
            }

            string SysInfoResumee = "";

            ContractID = RegistryData.ContractID.Trim();
            ContractPassword = RegistryData.ContractPassword;

            SysInfoResumee += "System Info:\r\n";
            SysInfoResumee += "Agent:                 " + SysInfo.AgentVersion + "\r\n";
            SysInfoResumee += SysInfo.OSName + "\r\n";
            SysInfoResumee += "aka Windows NT         " + SysInfo.OSVerMaj.ToString() + "." + SysInfo.OSVerMin.ToString() + "." + SysInfo.OSVerBuild.ToString() + "\r\n";
            SysInfoResumee += "Suite:                 " + SysInfo.OSSuite + "\r\n";
            SysInfoResumee += "Language:              " + SysInfo.Language + " (" + SysInfo.DisplayLanguage + ")\r\n";
            SysInfoResumee += "Bits:                  " + (SysInfo.Is64Bit == true ? "64 bit" : "32 bit") + "\r\n";
            SysInfoResumee += "TSE:                   " + (SysInfo.IsTSE == false ? "no" : "yes") + "\r\n";
            SysInfoResumee += "OS Suite:              " + SysInfo.OSVerType + "\r\n";
            SysInfoResumee += "CPU:                   " + CPU.ToString() + "\r\n";
            SysInfoResumee += "Model:                 " + SysInfo.ComputerModel + "\r\n";
            SysInfoResumee += "BIOS:                  " + SysInfo.BIOS + "\r\n";
            SysInfoResumee += "Computername:          " + SysInfo.ComputerName + "\r\n";
            SysInfoResumee += "Machine ID:            " + SysInfo.MachineID + "\r\n";
            SysInfoResumee += "Server URL:            " + ServerURL + "\r\n";
            SysInfoResumee += "UCID:                  " + SysInfo.UCID + "\r\n";
            SysInfoResumee += "Program Data:          " + ProgramData + "\r\n";
            SysInfoResumee += "Running in Hypervisor: " + (SysInfo.RunningInHypervisor == false ? "no" : "yes") + "\r\n";

            FoxEventLog.WriteEventLog("Fox SDC Agent starting: " + SysInfoResumee, EventLogEntryType.Information);

            return (0);
        }
    }
}
