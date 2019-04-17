using Fox_LicenseGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class FS_Watcher
    {
        static FileSystemWatcher updatepackagesfsw;
        static FileSystemWatcher licensefsw;

        public static void InstallFSW()
        {
            string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (AppPath.EndsWith("\\") == false)
                AppPath += "\\";
            AppPath += "Packages\\";

            if (Directory.Exists(AppPath) == false)
            {
                FoxEventLog.WriteEventLog("The directory " + AppPath + " does not exist. Note that clients will not be able to download their updates. Make sure the directory exists and the proper contents are available then restart this server application.", System.Diagnostics.EventLogEntryType.Warning);
            }
            else
            {
                updatepackagesfsw = new FileSystemWatcher(AppPath, "*.foxpkg");
                updatepackagesfsw.Changed += Fsw_Changed;
                updatepackagesfsw.Renamed += Fsw_Renamed;
                updatepackagesfsw.Created += Fsw_Created;
                updatepackagesfsw.Deleted += Fsw_Deleted;
                updatepackagesfsw.EnableRaisingEvents = true;
                ClientUpdate.ReadCheckVersions();
            }

            AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (AppPath.EndsWith("\\") == false)
                AppPath += "\\";

            licensefsw = new FileSystemWatcher(AppPath, "License.lic");
            licensefsw.Changed += Licensefsw_Changed;
            licensefsw.Renamed += Licensefsw_Renamed;
            licensefsw.Created += Licensefsw_Created;
            licensefsw.Deleted += Licensefsw_Deleted;
            licensefsw.EnableRaisingEvents = true;

            SDCLicensing.LoadLic();
        }

        private static void Licensefsw_Deleted(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            SDCLicensing.LoadLic();
        }

        private static void Licensefsw_Created(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            SDCLicensing.LoadLic();
        }

        private static void Licensefsw_Renamed(object sender, RenamedEventArgs e)
        {
            Thread.Sleep(1000);
            SDCLicensing.LoadLic();
        }

        private static void Licensefsw_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            SDCLicensing.LoadLic();
        }

        private static void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            ClientUpdate.ReadCheckVersions();
        }

        private static void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            ClientUpdate.ReadCheckVersions();
        }

        private static void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1000);
            ClientUpdate.ReadCheckVersions();
        }

        private static void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            Thread.Sleep(1000);
            ClientUpdate.ReadCheckVersions();
        }
    }
}
