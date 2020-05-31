using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace FoxSDC_Server
{
    class Program
    {
        public static bool ServiceRunning = false;
        public static int DBVersion = DBUpdate.DBVersion;
        public static string InstanceID = "";
#if !DEBUG
        static ServiceBase[] ServicesToRun;
#endif

        static int Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "-?")
                {
                    Console.WriteLine("Service only:");
                    Console.WriteLine("      -instance <instance id>");
                    Console.WriteLine("      -registereventlog");
                    Console.WriteLine("      -createlocaldb");
                    Console.WriteLine("      -noservice");
                    Console.WriteLine("      -install");
                    Console.WriteLine("      -httpcfg");
                    Console.WriteLine("      -httpcfgsystem");
                    return (0);
                }
                if (args[i].ToLower() == "-registereventlog")
                {
                    try
                    {
                        FoxEventLog.RegisterEventLog();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.ToString());
                        return (-1);
                    }
                    return (0);
                }
                if (args[i].ToLower() == "-noservice")
                {
                    Settings.Default.Load();
                    SMain();
                    return (0);
                }
                if (args[i].ToLower() == "-createlocaldb")
                {
                    Settings.Default.Load();
                    try
                    {
                        BlankLocalDB.CreateBlankLocalDB();
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

                    try
                    {
                        FoxEventLog.RegisterEventLog();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.ToString());
                        return (-1);
                    }
                    return (0);
                }
                if (args[i].ToLower() == "-instance")
                {
                    if (args.Length > i)
                    {
                        InstanceID = Utilities.FilterOutBadChars(args[i + 1].Trim());
                        i++;
                    }
                    else
                    {
                        Console.WriteLine("Missing supplement parameter");
                        return (-1);
                    }
                }
                if (args[i].ToLower() == "-httpcfg")
                {
                    Settings.Default.Load();
                    WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                    foreach (string lo in Settings.Default.ListenOn.Split('|'))
                    {
                        Console.WriteLine("Setting URLACL for " + lo + ", User: " + currentIdentity.Name);
                        HttpServerApi.ModifyNamespaceReservation(lo, currentIdentity.Name, HttpServerApiConfigurationAction.AddOrUpdate);
                    }
                    return (0);
                }
                if (args[i].ToLower() == "-httpcfgsystem")
                {
                    Settings.Default.Load();
                    foreach (string lo in Settings.Default.ListenOn.Split('|'))
                    {
                        Console.WriteLine("Setting URLACL for " + lo + ", User: " + "NT AUTHORITY\\SYSTEM");
                        HttpServerApi.ModifyNamespaceReservation(lo, "NT AUTHORITY\\SYSTEM", HttpServerApiConfigurationAction.AddOrUpdate);
                    }
                    return (0);
                }
            }

            Settings.Default.Load();
#if DEBUG
            SMain();
#else
            ServicesToRun = new ServiceBase[]
            {
                new FoxSDCASrvService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
            return (0);
        }

        public static void SMain()
        {
            string ErrorReason;
            Console.WriteLine("Server version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + (InstanceID == "" ? "" : " (" + InstanceID + ")"));
            FoxEventLog.WriteEventLog("Server version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " starting" + (InstanceID == "" ? "" : " (" + InstanceID + ")"), EventLogEntryType.Information);
            if (SQLTest.TestSettings(out ErrorReason) == false)
            {
                FoxEventLog.WriteEventLog("Settings are faulty: " + ErrorReason, EventLogEntryType.Error);
                Console.WriteLine("Settings are faulty: " + ErrorReason);
#if !DEBUG
                Process.GetCurrentProcess().Kill();
#endif
                return;
            }
            if (SQLTest.TestServer(out ErrorReason) == false)
            {
                FoxEventLog.WriteEventLog("Cannot connect to the server: " + ErrorReason, EventLogEntryType.Error);
                Console.WriteLine("Cannot connect to the server: " + ErrorReason);
#if !DEBUG
                Process.GetCurrentProcess().Kill();
#endif
                return;
            }
            RESTful.RegisterRESTfulClasses();
            FS_Watcher.InstallFSW();

            if (Utilities.TestSign(out ErrorReason) == false)
            {
                FoxEventLog.WriteEventLog("Cannot test-sign with the certificate " + SettingsManager.Settings.UseCertificate + ": " + ErrorReason, EventLogEntryType.Warning);
            }

#if DEBUG
            if (Fox_LicenseGenerator.SDCLicensing.ValidLicense == false)
            {
                FoxEventLog.WriteEventLog("Writing a crap-license into memory.", EventLogEntryType.Information);
                Fox_LicenseGenerator.SDCLicensing.NumComputers = 1000;
                Fox_LicenseGenerator.SDCLicensing.AllowContract = true;
                Fox_LicenseGenerator.SDCLicensing.Data = new Fox_LicenseGenerator.LicensingData();
                Fox_LicenseGenerator.SDCLicensing.Data.Features = "";
                Fox_LicenseGenerator.SDCLicensing.Data.LicenseID = Guid.NewGuid().ToString();
                Fox_LicenseGenerator.SDCLicensing.Data.LicenseType = "Memory";
                Fox_LicenseGenerator.SDCLicensing.Data.Owner = "Fox";
                Fox_LicenseGenerator.SDCLicensing.Data.OwnerCustomID = "";
                Fox_LicenseGenerator.SDCLicensing.Data.SupportValidTo = null;
                Fox_LicenseGenerator.SDCLicensing.Data.UCID = UCID.GetUCID();
                Fox_LicenseGenerator.SDCLicensing.Data.ValidTo = null;
                Fox_LicenseGenerator.SDCLicensing.Data.ValidFrom = DateTime.UtcNow.Date;
                Fox_LicenseGenerator.SDCLicensing.Data.Vacant1 = "1000";
                Fox_LicenseGenerator.SDCLicensing.ValidLicense = true;
            }
#endif

            try
            {
                Console.CancelKeyPress += Console_CancelKeyPress;
                WebServerHandler.RunWebServer();
                MaintenanceTasks.StartMaintenanceTreads();
                ReportingThread.StartReportingThreads();
                RemoteNetworkConnectionWSCrosser.InitialInitWS();
                Console.WriteLine("=============== Server started ===============");
                Debug.WriteLine("=============== Server started ===============");
                Console.WriteLine(Settings.Default.ListenOn);
                Debug.WriteLine(Settings.Default.ListenOn);
                Console.WriteLine(Settings.Default.WSListenOn);
                Debug.WriteLine(Settings.Default.WSListenOn);
                FoxEventLog.WriteEventLog("Server started", EventLogEntryType.Information);
                ServiceRunning = true;
                Thread tmm = new Thread(new ThreadStart(TimeoutManager));
                tmm.Start();
                do
                {
                    Thread.Sleep(1000);
                } while (ServiceRunning == true);
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("Cannot start server " + ee.Message, EventLogEntryType.Error);
                Console.WriteLine("Cannot start server " + ee.Message);
#if !DEBUG
                Process.GetCurrentProcess().Kill();
#endif
                return;
            }
        }

        public const int SessionTimeoutMin = 15;
        public const int WSSessionTimeoutMin = 15;

        static void TimeoutManager()
        {
            Debug.WriteLine(" === TMM started === ");
            int cnt = 0;
            do
            {
                Thread.Sleep(500);
                cnt++;
                if (cnt < 120)
                    continue;
                cnt = 0;

                try
                {
                    RemoteNetworkConnectionWSCrosser.TestTimeouts();
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                }

                try
                {
                    List<string> KillThem = new List<string>();
                    foreach (KeyValuePair<string, NetworkConnectionInfo> kvp in NetworkConnection.Connections)
                    {
                        Debug.WriteLine("Session " + kvp.Key + " -- " + kvp.Value.LastUsed.ToLongTimeString());
                        if (kvp.Value.LastUsed.AddMinutes(SessionTimeoutMin) < DateTime.Now)
                            KillThem.Add(kvp.Key);
                    }

                    foreach (string K in KillThem)
                    {
                        Debug.WriteLine("Killing Session " + K + " -- timeout");
                        NetworkConnection.DeleteSession(K);
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                }

            } while (Program.ServiceRunning == true);
        }

        static void KillConnections()
        {
            while (NetworkConnection.Connections.Count > 0)
            {
                NetworkConnection.DeleteSession(NetworkConnection.Connections.First().Key);
            }
        }

        static void Shutdown()
        {
            FoxEventLog.WriteEventLog("Server stopping", EventLogEntryType.Information);
            Debug.WriteLine("===== Cancelling =====");
            WebServerHandler.EndWebServer();
            RemoteNetworkConnectionWSCrosser.ShutdownWS();
            KillConnections();
            MaintenanceTasks.StopThreads();
            ReportingThread.StopThreads();
            ServiceRunning = false;
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("CTRL+C");
            Shutdown();
        }
    }
}
