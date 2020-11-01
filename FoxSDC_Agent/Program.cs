using FoxSDC_Agent.Redirs;
using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static FoxSDC_Common.PackageInstaller;

namespace FoxSDC_Agent
{
    public class ProgramAgent
    {
        public static CPPInterface CPP;
        public const string VulpesURL = VulpesBranding.BuiltInOnPremURL;
        public static string AppPath;
        public static string DLLFile;

#if !DEBUG || DEBUGSERVICE
        static ServiceBase[] ServicesToRun;
        static FoxSDCAService service = new FoxSDCAService();
#endif

#if DEBUG
        public static bool LoadDLLForced(SystemInfos.CPUType arch)
        {
            try
            {
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filename = "";
                switch (arch)
                {
                    case SystemInfos.CPUType.Intel32:
                        filename = Path.Combine(dir, "FoxSDC_AgentDLL32.dll");
                        break;
                    case SystemInfos.CPUType.EM64T:
                        filename = Path.Combine(dir, "FoxSDC_AgentDLL64.dll");
                        break;
                    case SystemInfos.CPUType.ARM64:
                        filename = Path.Combine(dir, "FoxSDC_AgentDLLARM64.dll");
                        break;
                }
                FoxEventLog.VerboseWriteEventLog("DLL: " + filename, System.Diagnostics.EventLogEntryType.Information);
                Debug.WriteLine(">>> LOADING: " + filename);
                if (filename == "")
                {
                    FoxEventLog.WriteEventLog("Cannot load FoxSDC_AgentDLL<xx>.DLL - Unknown architecture", System.Diagnostics.EventLogEntryType.Error);
                    return (false);
                }
                Assembly asm = Assembly.LoadFile(filename);
                Type t = asm.GetType("Fox.FoxCWrapper");
                CPP = (CPPInterface)Activator.CreateInstance(t);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Cannot load FoxSDC_AgentDLL<xx>.DLL", System.Diagnostics.EventLogEntryType.Error);
                FoxEventLog.VerboseWriteEventLog("Cannot load FoxSDC_AgentDLL<xx>.DLL - SEH: " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                return (false);
            }
            return (true);
        }
#endif

        public static bool LoadDLL()
        {
            try
            {
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filename = "";
                switch (SystemInfos.GetCPU())
                {
                    case SystemInfos.CPUType.Intel32:
                        filename = Path.Combine(dir, "FoxSDC_AgentDLL32.dll");
                        break;
                    case SystemInfos.CPUType.EM64T:
                        filename = Path.Combine(dir, "FoxSDC_AgentDLL64.dll");
                        break;
                    case SystemInfos.CPUType.ARM64:
                        filename = Path.Combine(dir, "FoxSDC_AgentDLLARM64.dll");
                        break;
                }
                FoxEventLog.VerboseWriteEventLog("DLL: " + filename, System.Diagnostics.EventLogEntryType.Information);
                Debug.WriteLine(">>> LOADING: " + filename);
                if (filename == "")
                {
                    FoxEventLog.WriteEventLog("Cannot load FoxSDC_AgentDLL<xx>.DLL - Unknown architecture", System.Diagnostics.EventLogEntryType.Error);
#if !DEBUG || DEBUGSERVICE
                    service.Stop();
#endif
                    return (false);
                }
                Assembly asm = Assembly.LoadFile(filename);
                Type t = asm.GetType("Fox.FoxCWrapper");
                CPP = (CPPInterface)Activator.CreateInstance(t);
                DLLFile = filename;
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
#if DEBUG
                FoxEventLog.WriteEventLog("Cannot load FoxSDC_AgentDLL<xx>.DLL\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
#else
                FoxEventLog.WriteEventLog("Cannot load FoxSDC_AgentDLL<xx>.DLL", System.Diagnostics.EventLogEntryType.Error);
#endif
#if !DEBUG || DEBUGSERVICE
                service.Stop();
#endif
                return (false);
            }
            return (true);
        }

        public static bool TestIntegrity(List<string> CustomFiles)
        {
            if (CPP.VerifyEXESignature(Assembly.GetExecutingAssembly().Location) == false)
                return (false);
            if (CPP.VerifyEXESignature(DLLFile) == false)
                return (false);
            if (CustomFiles != null)
                foreach (string file in CustomFiles)
                    if (CPP.VerifyEXESignature(file) == false)
                        return (false);
            return (true);
        }

        public static bool? IsSystemUser()
        {
#if !DEBUG
            try
            {
                WindowsIdentity wi = WindowsIdentity.GetCurrent();
                if (wi == null)
                {
                    FoxEventLog.WriteEventLog("Cannot get current user.", EventLogEntryType.Error);
                    return (null);
                }
                if (wi.User != new SecurityIdentifier("S-1-5-18"))
                {
                    FoxEventLog.WriteEventLog("User is not NT_AUTHORITY\\SYSTEM (it is " + WindowsIdentity.GetCurrent().Name + " - " + WindowsIdentity.GetCurrent().User.AccountDomainSid.ToString() + ") - change the service user and try again.", EventLogEntryType.Error);
                    return (false);
                }
                WindowsPrincipal principal = new WindowsPrincipal(wi);
                if (principal.IsInRole(WindowsBuiltInRole.Administrator) == false)
                {
                    FoxEventLog.WriteEventLog("SYSTEM user does not have admin rights.", EventLogEntryType.Error);
                    return (null);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("SID ERROR", EventLogEntryType.Error);
                return (null);
            }
#endif
            return (true);
        }

        public static void SMain()
        {
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
            FoxEventLog.RegisterEventLog();

            if (UsePipeAction == false && UseScreenAction == false && UseDNSAutoConfig == false && UseLoginRecovery == false) //Pipe Actions can also be run in user-space ...
            {
                if (IsSystemUser() != true)
                {
#if !DEBUG || DEBUGSERVICE
                    service.Stop();
#endif
                    return;
                }
            }

            if (LoadDLL() == false)
            {
#if !DEBUG || DEBUGSERVICE
                service.Stop();
#endif
                return;
            }

#if !DEBUG
            if (TestIntegrity(null) == false)
            {
                FoxEventLog.WriteEventLog("Integrity check failed!", EventLogEntryType.Error);
                service.Stop();
                return;
            }
#endif
            if (UseDNSAutoConfig == true)
            {
                try
                {
                    List<List<string>> Query = CPP.DNSQueryTXT("sdc-contract.my-vulpes-config.lu");

                    if (Query == null)
                        return;

                    string ContractID = null;
                    string ContractPassword = null;
                    string UseOnPrem = null;
                    string OnPremURL = null;

                    foreach (List<string> Q in Query)
                    {
                        if (Q == null)
                            continue;

                        foreach (string QR in Q)
                        {
                            if (string.IsNullOrWhiteSpace(QR) == true)
                                continue;
                            if (QR.ToLower().StartsWith("contractid=") == true)
                                ContractID = QR.Substring(11).Trim();
                            if (QR.ToLower().StartsWith("contractpassword=") == true)
                                ContractPassword = QR.Substring(17).Trim();
                            if (QR.ToLower().StartsWith("useonprem=") == true)
                                UseOnPrem = QR.Substring(10).Trim();
                            if (QR.ToLower().StartsWith("onpremurl=") == true)
                                OnPremURL = QR.Substring(10).Trim();
                        }
                    }

                    using (RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC"))
                    {
                        if (string.IsNullOrWhiteSpace(ContractID) == false && string.IsNullOrWhiteSpace(ContractPassword) == false)
                        {
                            k.SetValue("ContractID", ContractID, RegistryValueKind.String);
                            k.SetValue("ContractPassword", ContractPassword, RegistryValueKind.String);
                        }

                        int UseOnPremInt;
                        if (int.TryParse(UseOnPrem, out UseOnPremInt) == true)
                        {
                            if (UseOnPremInt == 1 && string.IsNullOrWhiteSpace(OnPremURL) == false)
                            {
                                k.SetValue("UseOnPremServer", 1, RegistryValueKind.DWord);
                                k.SetValue("Server", OnPremURL, RegistryValueKind.String);
                            }
                            else
                            {
                                k.SetValue("UseOnPremServer", 0, RegistryValueKind.DWord);
                            }
                        }
                        else
                        {
                            k.SetValue("UseOnPremServer", 0, RegistryValueKind.DWord);
                        }
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                }
                return;
            }

            if (UseLoginRecovery == true)
            {
                if (SystemInfos.CollectSystemInfo() != 0)
                    return;

#if !DEBUG
                if (SystemInfos.SysInfo.RunningInWindowsPE == false || SystemInfos.SysInfo.RunningInWindowsPE == null)
                    return;
#endif

                RecoveryLogon reclogon = new RecoveryLogon();
                reclogon.UCID = SystemInfos.SysInfo.UCID;
                reclogon.ContractID = SystemInfos.ContractID;
                reclogon.ContractPassword = SystemInfos.ContractPassword;

                string Check = SystemInfos.SysInfo.CPUName.Trim();
                Check += SystemInfos.SysInfo.ComputerModel == "" ? "N/A" : SystemInfos.SysInfo.ComputerModel.Trim();
                Check += SystemInfos.SysInfo.BIOS == "" ? "N/A" : SystemInfos.SysInfo.BIOS.Trim();

                reclogon.MoreMachineHash = MD5Utilities.CalcMD5(Check);
                Network net = Utilities.NoConnectNetwork();

                RecoveryData rd = net.GetRecoveryLogon(reclogon);
                if (rd == null)
                    return;
                if (rd.Worked == false)
                    return;

                string Registry = "Windows Registry Editor Version 5.00\r\n\r\n[HKEY_LOCAL_MACHINE\\SOFTWARE\\Fox\\SDC]\r\n\"ID\"=\"" + rd.MachineID + "\"\r\n\"PassID\"=\"" + rd.MachinePassword + "\"";
                try
                {
                    File.WriteAllText(Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\\Fox SDC MachinePW.reg"), Registry, Encoding.Unicode);
                }
                catch
                {

                }
                return;
            }

            if (UsePipeAction == false && UseScreenAction == false)
            {
                using (RegistryKey installer = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + VulpesBranding.MSIGUID, false))
                {
                    if (installer == null)
                    {
                        FoxEventLog.WriteEventLog("Missing installer key in HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + VulpesBranding.MSIGUID + ". This can lead to erratic behavoir of the program!", EventLogEntryType.Warning);
                    }
                }

                if (CPP.SetToken() == false)
                {
                    FoxEventLog.WriteEventLog("Cannot setup token - 0x" + Marshal.GetLastWin32Error().ToString("X") + " - " + new Win32Exception(Marshal.GetLastWin32Error()).Message, System.Diagnostics.EventLogEntryType.Error);
                    return;
                }
            }

            if (UsePipeAction == true)
            {
                try
                {
                    Process2ProcessCommClient.RunPipeClient();
                }
                catch (Exception ee)
                {
                    FoxEventLog.WriteEventLog("Pipe SEH " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                }
                return;
            }

            if (UseScreenAction == true)
            {
                try
                {
                    if (SystemInfos.CollectSystemInfo() != 0)
                        return;
                    MainScreenSystemClient.RunPipeClient();
                }
                catch (Exception ee)
                {
                    FoxEventLog.WriteEventLog("Screen Pipe SEH " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                }
                return;
            }

            if (SystemInfos.CollectSystemInfo() != 0)
            {
#if !DEBUG || DEBUGSERVICE
                service.Stop();
#endif
                return;
            }

            RegistryData.InstallPath = AppPath;

            if (ApplicationCertificate.LoadCertificate() == false)
            {
                FoxEventLog.WriteEventLog("Cannot load certificate", System.Diagnostics.EventLogEntryType.Error);
#if !DEBUG || DEBUGSERVICE
                service.Stop();
#endif
                return;
            }

            if (FilesystemData.LoadCertificates() == false)
            {
#if !DEBUG || DEBUGSERVICE
                service.Stop();
#endif
                return;
            }
            if (FilesystemData.LoadPolicies() == false)
            {
#if !DEBUG || DEBUGSERVICE
                service.Stop();
#endif
                return;
            }
            FilesystemData.LoadLocalPackageData();
            FilesystemData.LoadLocalPackages();
            FilesystemData.LoadUserPackageData();
            FilesystemData.LoadEventLogList();

            SyncPolicy.ApplyPolicy(SyncPolicy.ApplyPolicyFunction.ApplySystem);

            PipeCommunicationSRV.StartPipeSrv();

            try
            {
                string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (p.EndsWith("\\") == false)
                    p += "\\";

                using (RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    reg.SetValue("FoxSDCAgent", p + "FoxSDC_Agent_UI.exe", RegistryValueKind.String);
                    reg.SetValue("FoxSDCAgentApply", p + "FoxSDC_ApplyUserSettings.exe", RegistryValueKind.String);
                    reg.Close();
                }
            }
            catch
            {

            }

            try
            {
                using (RegistryKey reg = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true))
                {
                    object o = reg.GetValue("SoftwareSASGeneration", null);
                    int rvalue = 0;
                    if (o != null)
                        rvalue = Convert.ToInt32(o);

                    if (rvalue != 1 && rvalue != 3)
                        reg.SetValue("SoftwareSASGeneration", 1);
                    reg.Close();
                }
            }
            catch
            {

            }

            Threads.StartAllThreads();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (UsePipeAction == false && UseScreenAction == false)
                {
                    FoxEventLog.WriteEventLog("General SEH!: " + e.ExceptionObject.ToString(), EventLogEntryType.Error);
                    Process proc = new Process();
                    proc.StartInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().Location, "-reset");
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start();
                }
                else
                {
                    FoxEventLog.WriteEventLog("General SEH! (not resetting this process!): " + e.ExceptionObject.ToString(), EventLogEntryType.Error);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
                Console.WriteLine("with:");
                Console.WriteLine(e.ExceptionObject.ToString());
            }
            finally
            {
                Thread.Sleep(1000);
                try
                {
                    Process.GetCurrentProcess().Kill();
                }
                catch
                {

                }
            }

        }

        static public void Init()
        {
            AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (AppPath.EndsWith("\\") == false)
                AppPath += "\\";
        }

        public static bool UsePipeAction = false;
        public static bool UseScreenAction = false;
        public static bool UseDNSAutoConfig = false;
        public static bool UseLoginRecovery = false;
        public static string PipeGUID = "";

        public static string GetWSUrl(Network net)
        {
            string WS = net.GetWebsocketURL();
            return (WS);
        }

        static int Main(string[] args)
        {
            Init();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "-reset")
                {
                    Console.WriteLine("Resetting");
                    FoxEventLog.WriteEventLog("RESET: Recovering from a crash", EventLogEntryType.Warning);
                    try
                    {
                        ServiceController ctrl = new ServiceController("FoxSDCA");
                        ctrl.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 5, 0));
                        ctrl.Start();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.ToString());
                        return (-1);
                    }
                    return (0);
                }
                if (args[i].ToLower() == "-install")
                {
                    try
                    {
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.ToString());
                        return (-1);
                    }
                    return (0);
                }
                if (args[i].ToLower() == "-pipeaction")
                {
                    if (args.Length <= i)
                        return (-1);
                    PipeGUID = args[i + 1];
                    UsePipeAction = true;
                    i++;
                    SMain();
                    return (0);
                }
                if (args[i].ToLower() == "-screen")
                {
                    if (args.Length <= i)
                        return (-1);
                    PipeGUID = args[i + 1];
                    UseScreenAction = true;
                    i++;
                    SMain();
                    return (0);
                }
                if (args[i].ToLower() == "-autodnsconfig")
                {
                    UseDNSAutoConfig = true;
                    i++;
                    SMain();
                    return (0);
                }
                if (args[i].ToLower() == "-recovercreds")
                {
                    UseLoginRecovery = true;
                    i++;
                    SMain();
                    return (0);
                }
            }

#if DEBUG && !DEBUGSERVICE
            SMain();
            Console.WriteLine("DEBUG: Running - Press any key . . . ");
            Console.ReadKey(true);
            Console.WriteLine("DEBUG: Stopping . . . ");
            Threads.StopAllThreads();
#else
            ServicesToRun = new ServiceBase[]
            {
                service
            };
            ServiceBase.Run(ServicesToRun);
#endif
            return (0);
        }
    }
}
