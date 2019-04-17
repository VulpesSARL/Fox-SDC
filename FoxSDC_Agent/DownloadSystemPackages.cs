using FoxSDC_Agent.PolicyObjects;
using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    partial class Threads
    {
        static Thread MetaDownloaderThreadHandle = null;
        static Thread RunningFullPackageDownloadThreadHandle = null;
        static Dictionary<Int64, PackagesToInstall> PackagesToInst;
        static List<DownloadQueueElement> DownloadQueueMeta;
        static ManualResetEvent FullDownloadRst = new ManualResetEvent(true);

        static bool DeleteMetaDownload = false;
        static DownloadQueueElement RunningMetaDownload = null;
        static Network RunningMetaDownloadNet = null;

        static PackagesToInstall RunningFullPackageDownload = null;
        static Network RunningFullDownloadNet = null;
        static bool FullPackageDownloadFailed = false;

        static void AddLocalPackagesToInstallCheck(ref List<PackagesToInstall> pkgtoinstall, PackagesToInstall install)
        {
            for (int i = 0; i < pkgtoinstall.Count; i++)
            {
                if (pkgtoinstall[i].PackageID.ToLower() == install.PackageID.ToLower())
                {
                    if (pkgtoinstall[i].Version < install.Version)
                    {
                        pkgtoinstall.RemoveAt(i);
                        i = 0;
                        if (pkgtoinstall.Contains(install) == false) //dup?
                            pkgtoinstall.Add(install);
                    }
                }
            }
        }

        static void RunningFullPackageDownloadThread()
        {
            try
            {
                FoxEventLog.VerboseWriteEventLog("RunningFullPackageDownloadThread()", System.Diagnostics.EventLogEntryType.Information);

                FullPackageDownloadFailed = false;

                if (RunningFullPackageDownload == null)
                    return;

                RunningFullPackageDownload.Filename = "Package " + RunningFullPackageDownload.ID.ToString("X16") + ".foxpkg";

                string LocalFile = SystemInfos.ProgramData + "Packages\\" + RunningFullPackageDownload.Filename;
                string Downloading = LocalFile + ".downloading";

                RunningFullDownloadNet = Utilities.ConnectNetwork(-1);
                if (RunningFullDownloadNet == null)
                    return;
                string URL = "api/agent/packagedata/" + RunningFullPackageDownload.ID.ToString();

                FoxEventLog.VerboseWriteEventLog("RunningFullPackageDownloadThread() Starting Download from " + URL + " to " + Downloading, System.Diagnostics.EventLogEntryType.Information);

                bool res = RunningFullDownloadNet.DownloadFile(URL, Downloading);
                string Error = "";
                if (res == false)
                    Error = RunningFullDownloadNet.GetLastError();

                RunningFullDownloadNet.CloseConnection();
                RunningFullDownloadNet = null;
                FoxEventLog.VerboseWriteEventLog("RunningFullPackageDownloadThread() res=" + res.ToString() + " Error=" + Error, System.Diagnostics.EventLogEntryType.Information);

                if (StopThreads == true)
                {
                    FoxEventLog.VerboseWriteEventLog("RunningFullPackageDownloadThread() StopThreads=true", System.Diagnostics.EventLogEntryType.Information);
                    FullPackageDownloadFailed = false;
                    return;
                }
                if (res == false)
                {
                    FullPackageDownloadFailed = true;
                    FoxEventLog.WriteEventLog("The Package cannot be downloaded from " + URL + ": " + Error, System.Diagnostics.EventLogEntryType.Error);
                    RunningFullPackageDownload.Filename = null;
                    RunningFullPackageDownload = null;
                    return;
                }
                else
                {
                    PackageInstaller inst = new PackageInstaller();
                    PKGStatus pkgres;
                    PKGRecieptData reciept;
                    res = Process2ProcessComm.InvokeInstallPackage(Downloading, PackageCertificate.ActivePackageCerts, PackageInstaller.InstallMode.TestPackageOnly, true, out Error, out pkgres, out reciept);
                    if (res == false)
                    {
                        FoxEventLog.WriteEventLog("The Package downloaded from " + URL + " cannot be verifed: " + Error, System.Diagnostics.EventLogEntryType.Error);
                        CommonUtilities.SpecialDeleteFile(Downloading);
                        RunningFullPackageDownload.Filename = null;
                        RunningFullPackageDownload = null;
                        FullPackageDownloadFailed = true;
                        return;
                    }

                    if (inst.PackageInfo(Downloading, PackageCertificate.ActivePackageCerts, out Error) == false)
                    {
                        FoxEventLog.WriteEventLog("The Package downloaded from " + URL + " cannot be read: " + Error, System.Diagnostics.EventLogEntryType.Error);
                        CommonUtilities.SpecialDeleteFile(Downloading);
                        RunningFullPackageDownload.Filename = null;
                        RunningFullPackageDownload = null;
                        FullPackageDownloadFailed = true;
                        return;
                    }

                    if (inst.PackageInfoData.PackageID != RunningFullPackageDownload.PackageID || inst.PackageInfoData.VersionID != RunningFullPackageDownload.Version)
                    {
                        FoxEventLog.WriteEventLog("The Package downloaded from " + URL + " does not match the informations from server\n" +
                        "Server: " + RunningFullPackageDownload.PackageID + " V" + RunningFullPackageDownload.Version.ToString() + "\n" +
                        "Package: " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                        CommonUtilities.SpecialDeleteFile(Downloading);
                        RunningFullPackageDownload.Filename = null;
                        RunningFullPackageDownload = null;
                        FullPackageDownloadFailed = true;
                        return;
                    }

                    #region Lock Packages ID
                    bool ReservedPackage = false;
                    switch (inst.PackageInfoData.PackageID)
                    {
                        case "Vulpes - SDCA1 - Update":
                            FoxEventLog.WriteEventLog("The Package " + LocalFile + " has a reserved Package ID and cannot be installed using policies.", EventLogEntryType.Warning);
                            ReservedPackage = true;
                            break;
                    }
                    if (ReservedPackage == true)
                    {
                        RunningFullPackageDownload.Filename = null;
                        RunningFullPackageDownload = null;
                        FullPackageDownloadFailed = true;
                        return;
                    }
                    #endregion

                    try
                    {
                        if (File.Exists(LocalFile) == true)
                            File.Delete(LocalFile);
                        File.Move(Downloading, LocalFile);
                        FullPackageDownloadFailed = false;
                        FoxEventLog.VerboseWriteEventLog("RunningFullPackageDownloadThread() success " + Downloading + "->" + LocalFile, System.Diagnostics.EventLogEntryType.Information);
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        FoxEventLog.WriteEventLog("Cannot process the file " + Downloading + " due to an error: " + ee.Message, System.Diagnostics.EventLogEntryType.Error);
                        RunningFullPackageDownload.Filename = null;
                        RunningFullPackageDownload = null;
                        FullPackageDownloadFailed = true;
                        return;
                    }
                }
            }
            finally
            {
                FullDownloadRst.Set();
            }
        }

        static void LocalPackagesThread()
        {
            FoxEventLog.VerboseWriteEventLog("LocalPackagesThread()", System.Diagnostics.EventLogEntryType.Information);
            while (StopThreads == false)
            {
                List<PackagesToInstall> pkgtoinstall = new List<PackagesToInstall>();
                string Error;
                int Tim = 0;

                if (PackagesToInst == null)
                {
                    FoxEventLog.VerboseWriteEventLog("LocalPackagesThread() PackagesToInst == null", System.Diagnostics.EventLogEntryType.Information);
                    Tim = 0;
                    while (Tim < 60)
                    {
                        Thread.Sleep(1000);
                        Tim++;
                        if (StopThreads == true)
                            break;
                    }
                }

                lock (PackagesToInst)
                {
                    foreach (PackagesToInstall pkg in FilesystemData.LocalPackages)
                    {
                        LocalPackageData pkgl = FilesystemData.FindLocalPackageFromListLatest(pkg.PackageID);
                        if (pkgl == null)
                        {
                            AddLocalPackagesToInstallCheck(ref pkgtoinstall, pkg);
                            continue;
                        }
                        if (pkgl.Version < pkg.Version)
                        {
                            AddLocalPackagesToInstallCheck(ref pkgtoinstall, pkg);
                        }
                    }
                }

                Int64 LastChecked = RegistryData.PackagesCheckWait;
                Int64 LastCheckedSuccess = RegistryData.PackagesCheckSuccessWait;

                lock (PackagesToInst)
                {
                    foreach (PackagesToInstall pkg in FilesystemData.LocalPackages)
                    {
                        LocalPackageData lpkg = FilesystemData.FindLocalPackageFromList(pkg.PackageID, pkg.Version);
                        if (lpkg == null)
                        {
                            lpkg = new LocalPackageData();
                            lpkg.InstallStatus = PKGStatus.Unknown;
                            lpkg.DownloadFailed = false;
                            lpkg.Downloaded = false;
                            lpkg.LastChecked = DateTime.UtcNow.AddMinutes(0L - LastChecked - 1);
                            lpkg.PKGRecieptFilename = null;
                            lpkg.PackageID = pkg.PackageID;
                            lpkg.Version = pkg.Version;
                            lpkg.ServerHasPackage = true;
                        }

                        FilesystemData.AddUpdateLocalPackage(lpkg);
                    }
                }

                FilesystemData.WritePackageDataList();

                List<PackagesToInstall> pkgtoinstcheck = new List<PackagesToInstall>();

                lock (PackagesToInst)
                {
                    foreach (PackagesToInstall pkg in FilesystemData.LocalPackages)
                    {
                        LocalPackageData lpkg = FilesystemData.FindLocalPackageFromList(pkg.PackageID, pkg.Version);

                        if (lpkg.InstallStatus == PKGStatus.NotNeeded || lpkg.InstallStatus == PKGStatus.Success)
                        {
                            if (lpkg.LastChecked.AddMinutes(LastCheckedSuccess) < DateTime.UtcNow)
                            {
                                pkgtoinstcheck.Add(pkg);
                            }
                        }
                        else
                        {
                            if (lpkg.LastChecked.AddMinutes(LastChecked) < DateTime.UtcNow)
                            {
                                pkgtoinstcheck.Add(pkg);
                            }
                        }
                    }
                }

                if (RegistryData.Verbose == 1)
                {
                    string data = "LocalPackagesThread() - DATA\r\npkgtoinstall.Count=" + pkgtoinstall.Count.ToString() + "\r\n";
                    foreach (PackagesToInstall pkgl in pkgtoinstall)
                    {
                        data += pkgl.PackageID + " V=" + pkgl.Version + "ID=" + pkgl.ID + " -- " + (pkgl.MetaFilename == null ? "null" : pkgl.MetaFilename) + " " + (pkgl.Filename == null ? "null" : pkgl.Filename) + "\r\n";
                    }
                    data += "\r\n\r\n";

                    lock (PackagesToInst)
                    {
                        data += "FilesystemData.LocalPackages.Count = " + FilesystemData.LocalPackages.Count.ToString() + "\r\n";
                        foreach (PackagesToInstall pkgl in FilesystemData.LocalPackages)
                        {
                            data += pkgl.PackageID + " V=" + pkgl.Version + "ID=" + pkgl.ID + " -- " + (pkgl.MetaFilename == null ? "null" : pkgl.MetaFilename) + " " + (pkgl.Filename == null ? "null" : pkgl.Filename) + "\r\n";
                        }
                        data += "\r\n\r\n";
                    }

                    data += "pkgtoinstcheck.Count = " + pkgtoinstcheck.Count.ToString() + "\r\n";
                    foreach (PackagesToInstall pkgl in pkgtoinstcheck)
                    {
                        data += pkgl.PackageID + " V=" + pkgl.Version + "ID=" + pkgl.ID + " -- " + (pkgl.MetaFilename == null ? "null" : pkgl.MetaFilename) + " " + (pkgl.Filename == null ? "null" : pkgl.Filename) + " Update=" + pkgl.InstallUpdates.ToString() + "\r\n";
                    }
                    data += "\r\n\r\n";

                    data += "FilesystemData.LocalPackageDataList.Count = " + FilesystemData.LocalPackageDataList.Count.ToString() + "\r\n";
                    foreach (LocalPackageData pkgl in FilesystemData.LocalPackageDataList)
                    {
                        data += pkgl.PackageID + " V=" + pkgl.Version + pkgl.LastChecked.ToLongDateString() + " " + pkgl.LastChecked.ToLongTimeString() + " " + (pkgl.PKGRecieptFilename == null ? "null" : pkgl.PKGRecieptFilename) + " " + pkgl.InstallStatus.ToString() + "\r\n";
                    }
                    data += "\r\n\r\n";

                    FoxEventLog.VerboseWriteEventLog(data, EventLogEntryType.Information);
                }

                foreach (PackagesToInstall pkg in pkgtoinstcheck)
                {
                    LocalPackageData lpkg = FilesystemData.FindLocalPackageFromList(pkg.PackageID, pkg.Version);
                    if (lpkg == null)
                        continue;

                    if (pkg.InstallUpdates == false && pkg.Optional == true && FilesystemData.CanInstallOptionalPackage(pkg))
                    {
                        FoxEventLog.WriteEventLog("The Package " + pkg.PackageID + " is optional, not to be installed and not to be updated.\n", System.Diagnostics.EventLogEntryType.Information);
                        continue;
                    }

                    string LocalMetaFile = SystemInfos.ProgramData + "Packages\\" + pkg.MetaFilename;
                    PackageInstaller inst = new PackageInstaller();
                    PKGRecieptData Reciept = null;
                    try
                    {
                        if (inst.PackageInfo(LocalMetaFile, PackageCertificate.ActivePackageCerts, out Error) == false)
                        {
                            FoxEventLog.WriteEventLog("The Metapackage " + pkg.MetaFilename + " cannot be read: " + Error, System.Diagnostics.EventLogEntryType.Error);
                            FilesystemData.RemovePackage(pkg);
                            continue;
                        }
                        if (inst.PackageInfoData.PackageID != pkg.PackageID || inst.PackageInfoData.VersionID != pkg.Version)
                        {
                            FoxEventLog.WriteEventLog("The Metapackage " + pkg.MetaFilename + " does not match the informations from server\n" +
                            "Server: " + pkg.PackageID + " V" + pkg.Version.ToString() + "\n" +
                            "Package: " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                            FilesystemData.RemovePackage(pkg);
                            continue;
                        }
                    }
                    catch(Exception ee)
                    {
                        FoxEventLog.WriteEventLog("The Metapackage " + pkg.MetaFilename + " cannot be read: " + ee.Message, EventLogEntryType.Error);
                        FilesystemData.RemovePackage(pkg);
                        continue;
                    }

                    lpkg.LastChecked = DateTime.UtcNow;

                    #region Lock Packages ID
                    bool ReservedPackage = false;
                    switch (inst.PackageInfoData.PackageID)
                    {
                        case "Vulpes - SDCA1 - Update":
                            FoxEventLog.WriteEventLog("The Metapackage " + pkg.MetaFilename + " has a reserved Package ID and cannot be installed using policies.", EventLogEntryType.Warning);
                            ReservedPackage = true;
                            break;
                    }
                    if (ReservedPackage == true)
                        continue;
                    #endregion

                    PackageInstaller.InstallMode Test = PackageInstaller.InstallMode.Test;
                    PackageInstaller.InstallMode Install = PackageInstaller.InstallMode.Install;
                    if (pkg.InstallUpdates == true)
                    {
                        Test = PackageInstaller.InstallMode.UpdateTest;
                        Install = PackageInstaller.InstallMode.Update;
                    }

                    if (Process2ProcessComm.InvokeInstallPackage(LocalMetaFile, PackageCertificate.ActivePackageCerts, Test, true, out Error, out lpkg.InstallStatus, out Reciept) == false)
                    {
                        FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " failed internally: " + Error + "\n" +
                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                        lpkg.InstallStatus = PKGStatus.Failed;
                        FilesystemData.WritePackageDataList();
                        continue;
                    }
                    switch (lpkg.InstallStatus)
                    {
                        case PKGStatus.DependencyFailed:
                            FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " has a dependency failure.\n" +
                            "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                            FilesystemData.WritePackageDataList();
                            break;
                        case PKGStatus.Failed:
                            FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " returned failed status.\n" +
                            "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                            FilesystemData.WritePackageDataList();
                            break;
                        case PKGStatus.NotNeeded:
                            FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " is not needed.\n" +
                            "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Information);
                            FilesystemData.WritePackageDataList();

                            if (FilesystemData.CanInstallOptionalPackage(pkg) == true)
                            {
                                Test = PackageInstaller.InstallMode.Test;
                                Install = PackageInstaller.InstallMode.Install;

                                if (Process2ProcessComm.InvokeInstallPackage(LocalMetaFile, PackageCertificate.ActivePackageCerts, Test, true, out Error, out lpkg.InstallStatus, out Reciept) == false)
                                {
                                    FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " failed internally: " + Error + "\n" +
                                    "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                    lpkg.InstallStatus = PKGStatus.Failed;
                                    FilesystemData.WritePackageDataList();
                                    continue;
                                }
                                switch (lpkg.InstallStatus)
                                {
                                    case PKGStatus.DependencyFailed:
                                        FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " has a dependency failure.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                    case PKGStatus.Failed:
                                        FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " returned failed status.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                    case PKGStatus.NotNeeded:
                                        FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " is not needed.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Information);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                    case PKGStatus.Unknown:
                                        FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " returned unknown status.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                }
                            }
                            break;
                        case PKGStatus.Unknown:
                            FoxEventLog.WriteEventLog("The Metapackage (" + Install.ToString() + ") " + pkg.MetaFilename + " returned unknown status.\n" +
                            "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                            FilesystemData.WritePackageDataList();
                            break;
                    }

                    if (lpkg.InstallStatus != PKGStatus.Success)
                    {
                        FilesystemData.WritePackageDataList();
                        continue;
                    }

                    if (pkg.Filename == null)
                    {
                        if (lpkg.ServerHasPackage == false)
                        {
                            FoxEventLog.VerboseWriteEventLog("The Package " + pkg.PackageID + " V" + pkg.Version + " will not be downloaded: the server won't offer it anymore.\n", System.Diagnostics.EventLogEntryType.Information);
                            continue;
                        }

                        if (RunDownload(pkg) == false)
                        {
                            if (StopThreads == true)
                                return;
                            lpkg.DownloadFailed = true;
                            lpkg.Downloaded = false;
                            pkg.Filename = null;
                            FilesystemData.WritePackageDataList();
                            FilesystemData.WritePackageList();
                            continue;
                        }
                        else
                        {
                            lpkg.DownloadFailed = false;
                            lpkg.Downloaded = true;
                            FilesystemData.WritePackageDataList();
                            FilesystemData.WritePackageList();
                        }
                        if (StopThreads == true)
                            return;
                    }
                    string LocalFile = "";

                    if (pkg.Filename != null)
                    {
                        inst = new PackageInstaller();

                        LocalFile = SystemInfos.ProgramData + "Packages\\" + pkg.Filename;
                        if (File.Exists(LocalFile) == false)
                        {
                            pkg.Filename = null;
                            continue;
                        }

                        if (inst.PackageInfo(LocalFile, PackageCertificate.ActivePackageCerts, out Error) == false)
                        {
                            FoxEventLog.WriteEventLog("The Package " + pkg.Filename + " cannot be read: " + Error, System.Diagnostics.EventLogEntryType.Error);
                            FilesystemData.RemovePackage(pkg);
                            continue;
                        }
                        if (inst.PackageInfoData.PackageID != pkg.PackageID || inst.PackageInfoData.VersionID != pkg.Version)
                        {
                            FoxEventLog.WriteEventLog("The Package " + pkg.Filename + " does not match the informations from server\n" +
                            "Server: " + pkg.PackageID + " V" + pkg.Version.ToString() + "\n" +
                            "Package: " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                            FilesystemData.RemovePackage(pkg);
                            continue;
                        }

                        if (Process2ProcessComm.InvokeInstallPackage(LocalFile, PackageCertificate.ActivePackageCerts, Test, false, out Error, out lpkg.InstallStatus, out Reciept) == false)
                        {
                            FoxEventLog.WriteEventLog("The Package (" + Install.ToString() + ") " + pkg.Filename + " failed internally: " + Error + "\n" +
                            "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                            lpkg.InstallStatus = PKGStatus.Failed;
                            FilesystemData.WritePackageDataList();
                            continue;
                        }

                        if (lpkg.InstallStatus != PKGStatus.Success)
                        {
                            if (RegistryData.Verbose == 1)
                            {
                                switch (lpkg.InstallStatus)
                                {
                                    case PKGStatus.DependencyFailed:
                                        FoxEventLog.VerboseWriteEventLog("The Package (Test) (" + Install.ToString() + ") " + pkg.MetaFilename + " has a dependency failure.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                    case PKGStatus.Failed:
                                        FoxEventLog.VerboseWriteEventLog("The Package (Test) (" + Install.ToString() + ") " + pkg.MetaFilename + " returned failed status.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                    case PKGStatus.NotNeeded:
                                        FoxEventLog.VerboseWriteEventLog("The Package (Test) (" + Install.ToString() + ") " + pkg.MetaFilename + " is not needed.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Information);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                    case PKGStatus.Unknown:
                                        FoxEventLog.VerboseWriteEventLog("The Package (Test) (" + Install.ToString() + ") " + pkg.MetaFilename + " returned unknown status.\n" +
                                        "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                        FilesystemData.WritePackageDataList();
                                        break;
                                }
                            }
                            FilesystemData.WritePackageList();
                            FilesystemData.WritePackageDataList();
                            continue;
                        }

                        if (Process2ProcessComm.InvokeInstallPackage(LocalFile, PackageCertificate.ActivePackageCerts, Install, false, out Error, out lpkg.InstallStatus, out Reciept) == false)
                        {
                            FoxEventLog.WriteEventLog("The Package (" + Install.ToString() + ") " + pkg.Filename + " failed internally: " + Error + "\n" +
                            "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                            lpkg.InstallStatus = PKGStatus.Failed;
                            FilesystemData.WritePackageDataList();
                            continue;
                        }

                        switch (lpkg.InstallStatus)
                        {
                            case PKGStatus.DependencyFailed:
                                FoxEventLog.WriteEventLog("The Package (" + Install.ToString() + ") " + pkg.Filename + " has a dependency failure.\n" +
                                "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                FilesystemData.WritePackageDataList();
                                break;
                            case PKGStatus.Failed:
                                FoxEventLog.WriteEventLog("The Package (" + Install.ToString() + ") " + pkg.Filename + " returned failed status.\n" +
                                "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                FilesystemData.WritePackageDataList();
                                break;
                            case PKGStatus.NotNeeded:
                                FoxEventLog.WriteEventLog("The Package (" + Install.ToString() + ") " + pkg.Filename + " is not needed.\n" +
                                "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Information);
                                FilesystemData.WritePackageDataList();
                                break;
                            case PKGStatus.Unknown:
                                FoxEventLog.WriteEventLog("The Package (" + Install.ToString() + ") " + pkg.Filename + " returned unknown status.\n" +
                                "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                                FilesystemData.WritePackageDataList();
                                break;
                            case PKGStatus.Success:
                                FoxEventLog.WriteEventLog("The Package (" + Install.ToString() + ") " + pkg.Filename + " was installed successfull.\n" +
                                "Package: " + inst.PackageInfoData.Title + " " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Information);
                                FilesystemData.WritePackageDataList();
                                break;
                        }

                        if (lpkg.InstallStatus == PKGStatus.Success && Reciept != null)
                        {
                            lpkg.PKGRecieptFilename = "Reciept " + pkg.ID.ToString("X16") + ".foxrecp";

                            string LocalRFile = SystemInfos.ProgramData + "Packages\\" + lpkg.PKGRecieptFilename;
                            string LocalRFileSign = LocalRFile + ".sign";
#if DEBUG
                            string recps = JsonConvert.SerializeObject(Reciept, Formatting.Indented);
#else
                            string recps = JsonConvert.SerializeObject(Reciept);
#endif
                            File.WriteAllText(LocalRFile, recps, Encoding.UTF8);
                            byte[] sign = ApplicationCertificate.Sign(Encoding.UTF8.GetBytes(recps));
                            File.WriteAllBytes(LocalRFileSign, sign);
                        }

                        FilesystemData.WritePackageDataList();
                        FilesystemData.WritePackageList();
                    }
                }

                Tim = 0;
                while (Tim < 60)
                {
                    Thread.Sleep(1000);
                    Tim++;
                    if (StopThreads == true)
                        break;
                }
            }
        }

        static bool RunDownload(PackagesToInstall pkg)
        {
            FullDownloadRst.Reset();
            RunningFullPackageDownload = pkg;
            RunningFullPackageDownloadThreadHandle = new Thread(new ThreadStart(RunningFullPackageDownloadThread));
            RunningFullPackageDownloadThreadHandle.Start();
            FullDownloadRst.WaitOne();
            return (!FullPackageDownloadFailed);
        }

        static void MetaDownloaderThread()
        {
            FoxEventLog.VerboseWriteEventLog("MetaDownloaderThread()", System.Diagnostics.EventLogEntryType.Information);

            RunningMetaDownloadNet = Utilities.ConnectNetwork(-1);
            string LocalFile = SystemInfos.ProgramData + "Packages\\" + RunningMetaDownload.LocalFile;

            if (RunningMetaDownloadNet != null)
            {
                FoxEventLog.VerboseWriteEventLog("MetaDownloaderThread() - Downloading " + RunningMetaDownload.URL + " to " + LocalFile + ".downloading", System.Diagnostics.EventLogEntryType.Information);
                bool res = RunningMetaDownloadNet.DownloadFile(RunningMetaDownload.URL, LocalFile + ".downloading");
                RunningMetaDownloadNet.CloseConnection();
                RunningMetaDownloadNet = null;
                if (StopThreads == true)
                {
                    FoxEventLog.VerboseWriteEventLog("MetaDownloaderThread() - StopThreads=true", System.Diagnostics.EventLogEntryType.Information);
                    return;
                }
                FoxEventLog.VerboseWriteEventLog("MetaDownloaderThread() - res=" + res.ToString() + " DeleteMetaDownload=" + DeleteMetaDownload.ToString(), System.Diagnostics.EventLogEntryType.Information);
                if (res == true && DeleteMetaDownload == true)
                {
                    CommonUtilities.SpecialDeleteFile(LocalFile + ".downloading");
                    RunningMetaDownload = null;
                    return;
                }
                if (res == true && DeleteMetaDownload == false)
                {
                    PackageInstaller inst = new PackageInstaller();
                    string Error;
                    PKGStatus pkgres;
                    PKGRecieptData reciept;
                    res = Process2ProcessComm.InvokeInstallPackage(LocalFile + ".downloading", PackageCertificate.ActivePackageCerts, PackageInstaller.InstallMode.TestPackageOnly, true, out Error, out pkgres, out reciept);
                    if (res == false)
                    {
                        lock (DownloadQueueMeta)
                        {
                            DownloadQueueMeta.RemoveAt(0);
                        }
                        FoxEventLog.WriteEventLog("The Metapackage downloaded from " + RunningMetaDownload.URL + " cannot be verifed: " + Error, System.Diagnostics.EventLogEntryType.Error);
                        CommonUtilities.SpecialDeleteFile(LocalFile + ".downloading");
                        RunningMetaDownload = null;
                        return;
                    }

                    if (inst.PackageInfo(LocalFile + ".downloading", PackageCertificate.ActivePackageCerts, out Error) == false)
                    {
                        lock (DownloadQueueMeta)
                        {
                            DownloadQueueMeta.RemoveAt(0);
                        }
                        FoxEventLog.WriteEventLog("The Metapackage downloaded from " + RunningMetaDownload.URL + " cannot be read: " + Error, System.Diagnostics.EventLogEntryType.Error);
                        CommonUtilities.SpecialDeleteFile(LocalFile + ".downloading");
                        RunningMetaDownload = null;
                        return;
                    }

                    PackagesToInstall pp = (PackagesToInstall)RunningMetaDownload.SupplementalData;
                    if (inst.PackageInfoData.PackageID != pp.PackageID || inst.PackageInfoData.VersionID != pp.Version)
                    {
                        lock (DownloadQueueMeta)
                        {
                            DownloadQueueMeta.RemoveAt(0);
                        }
                        FoxEventLog.WriteEventLog("The Metapackage downloaded from " + RunningMetaDownload.URL + " does not match the informations from server\n" +
                        "Server: " + pp.PackageID + " V" + pp.Version.ToString() + "\n" +
                        "Package: " + inst.PackageInfoData.PackageID + " V" + inst.PackageInfoData.PackageID, System.Diagnostics.EventLogEntryType.Error);
                        CommonUtilities.SpecialDeleteFile(LocalFile + ".downloading");
                        RunningMetaDownload = null;
                        return;
                    }

                    try
                    {
                        if (File.Exists(LocalFile) == true)
                            File.Delete(LocalFile);
                        File.Move(LocalFile + ".downloading", LocalFile);
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        lock (DownloadQueueMeta)
                        {
                            DownloadQueueMeta.RemoveAt(0);
                        }
                        FoxEventLog.WriteEventLog("Cannot process the file " + LocalFile + ".downloading due to an error: " + ee.Message, System.Diagnostics.EventLogEntryType.Error);
                        RunningMetaDownload = null;
                        return;
                    }

                    PackagesToInstall pkg = new PackagesToInstall();
                    pkg.MetaFilename = RunningMetaDownload.LocalFile;
                    pkg.Filename = null;
                    pkg.ID = pp.ID;
                    pkg.InstallUpdates = pp.InstallUpdates;
                    pkg.Optional = pp.Optional;
                    pkg.Version = pp.Version;
                    pkg.PackageID = pp.PackageID;

                    FoxEventLog.WriteEventLog("New Metapackage registred: \n" + pp.PackageID + " V" + pp.Version.ToString(), System.Diagnostics.EventLogEntryType.Information);
                    FilesystemData.RegisterPackage(pkg);
                    lock (DownloadQueueMeta)
                    {
                        DownloadQueueMeta.RemoveAt(0);
                    }
                    RunningMetaDownload = null;
                }
                else
                {
                    RunningMetaDownload = null;
                }
            }
            else
            {
                RunningMetaDownload = null;
            }
        }

        static void StopDownloads()
        {
            FoxEventLog.VerboseWriteEventLog("StopDownloads()", System.Diagnostics.EventLogEntryType.Information);

            if (RunningMetaDownloadNet != null)
                RunningMetaDownloadNet.StopDownload = true;
            if (RunningFullDownloadNet != null)
                RunningFullDownloadNet.StopDownload = true;

            if (MetaDownloaderThreadHandle != null)
                MetaDownloaderThreadHandle.Join();
            if (RunningFullPackageDownloadThreadHandle != null)
                RunningFullPackageDownloadThreadHandle.Join();

            FoxEventLog.VerboseWriteEventLog("StopDownloads() - DONE", System.Diagnostics.EventLogEntryType.Information);
        }

        static void DownloadThread()
        {
            FoxEventLog.VerboseWriteEventLog("DownloadThread()", System.Diagnostics.EventLogEntryType.Information);

            bool PackagesToInstCreated = false;
            PackagesToInst = new Dictionary<long, PackagesToInstall>();
            DownloadQueueMeta = new List<DownloadQueueElement>();

            while (StopThreads == false)
            {
                if (PackagesPolicy.UpdatePackages == true)
                {
                    Network net = Utilities.ConnectNetwork(-1);
                    if (net != null)
                    {
                        DownloadQueueMeta.Clear();

                        lock (PackagesPolicy.ActivePackagesLock)
                        {
                            lock (PackagesToInst)
                            {
                                List<PackagesToInstall> PackagesToRemove = new List<PackagesToInstall>();
                                if (PackagesToInstCreated == true)
                                {
                                    foreach (PackagesToInstall pkg in FilesystemData.LocalPackages)
                                    {
                                        bool PackageFound = false;
                                        foreach (KeyValuePair<Int64, PackagesToInstall> ppkg in PackagesToInst)
                                        {
                                            if (ppkg.Value.PackageID == pkg.PackageID && ppkg.Value.Version == pkg.Version)
                                            {
                                                PackageFound = true;
                                                break;
                                            }
                                        }
                                        if (PackageFound == false)
                                        {
                                            PackagesToRemove.Add(pkg);
                                        }
                                    }
                                    foreach (PackagesToInstall pkg in PackagesToRemove)
                                    {
                                        FoxEventLog.VerboseWriteEventLog("Package Removed: \n" + pkg.PackageID + " V" + pkg.Version.ToString(), System.Diagnostics.EventLogEntryType.Information);
                                        FilesystemData.RemovePackage(pkg);
                                    }
                                }

                                foreach (PackagePolicy pp in PackagesPolicy.ActivePackages)
                                {
                                    foreach (Int64 ppid in pp.Packages)
                                    {
                                        if (PackagesToInst.ContainsKey(ppid) == true)
                                        {
                                            if (PackagesToInst[ppid].Optional == true && pp.OptionalInstallation == false)
                                                PackagesToInst[ppid].Optional = false;
                                            if (PackagesToInst[ppid].Optional == true)
                                            {
                                                if (PackagesToInst[ppid].InstallUpdates == false && pp.InstallUpdates == true)
                                                    PackagesToInst[ppid].InstallUpdates = true;
                                            }
                                        }

                                        PackagesToInstall pi = new PackagesToInstall();
                                        PackageDataSigned pd = net.SignedGetPackagesCached(ppid, FilesystemData.LoadedCertificatesArray);
                                        if (pd == null)
                                            continue;
                                        pi.ID = pd.Package.ID;
                                        pi.PackageID = pd.Package.PackageID;
                                        pi.Version = pd.Package.Version;
                                        pi.Optional = pp.OptionalInstallation;
                                        pi.InstallUpdates = pp.InstallUpdates;
                                        pi.Title = pd.Package.Title;
                                        if (PackagesToInst.ContainsKey(ppid) == true)
                                            PackagesToInst.Remove(ppid);
                                        PackagesToInst.Add(ppid, pi);

                                        if (StopThreads == true)
                                            break;
                                    }
                                    if (StopThreads == true)
                                        break;
                                }

                                PackagesToInstCreated = true;

                                FilesystemData.AvailableUserPackages = new List<PackageIDData>();

                                foreach (KeyValuePair<Int64, PackagesToInstall> kvp in PackagesToInst)
                                {
                                    if (StopThreads == true)
                                        break;

                                    if (FilesystemData.HasLocalPackage(kvp.Value) == false)
                                    {
                                        string DownloadURL = "api/agent/packagedata/" + kvp.Key.ToString();
                                        string LocalFile = "MPackage " + kvp.Key.ToString("X16") + ".foxpkg";
                                        string URL = "api/agent/packagemetadata/" + kvp.Value.ID.ToString();
                                        bool FoundInQueue = false;
                                        foreach (DownloadQueueElement q in DownloadQueueMeta)
                                        {
                                            if (q.URL.ToLower() == URL.ToLower())
                                            {
                                                FoundInQueue = true;
                                                break;
                                            }
                                        }
                                        if (FoundInQueue == false)
                                        {
                                            DownloadQueueElement q = new DownloadQueueElement();
                                            q.LocalFile = LocalFile;
                                            q.URL = URL;
                                            q.SupplementalData = kvp.Value;
                                            DownloadQueueMeta.Add(q);
                                        }
                                    }

                                    if (kvp.Value.Optional == true)
                                    {
                                        PackageIDData pkgid = new PackageIDData();
                                        pkgid.PackageID = kvp.Value.PackageID;
                                        pkgid.Title = kvp.Value.Title;

                                        bool Found = false;
                                        foreach (PackageIDData ppp in FilesystemData.AvailableUserPackages)
                                        {
                                            if (ppp.PackageID.ToLower() == pkgid.PackageID.ToLower())
                                            {
                                                Found = true;
                                                break;
                                            }
                                        }

                                        if (Found == false)
                                        {
                                            FilesystemData.AvailableUserPackages.Add(pkgid);
                                        }
                                    }
                                }

                                if (RegistryData.Verbose == 1)
                                {
                                    string data = "FilesystemData.AvailableUserPackages.Count=" + FilesystemData.AvailableUserPackages.Count.ToString() + "\r\n";
                                    foreach (PackageIDData pkg in FilesystemData.AvailableUserPackages)
                                    {
                                        data += pkg.PackageID + " -- " + pkg.Title + "\r\n";
                                    }
                                    FoxEventLog.VerboseWriteEventLog(data, EventLogEntryType.Information);
                                }

                                PackagesPolicy.UpdatePackages = false;
                            }
                        }
                        net.CloseConnection();

                        if (RunningMetaDownload != null)
                        {
                            lock (RunningMetaDownload)
                            {
                                bool FoundInRunningQueue = false;
                                foreach (DownloadQueueElement q in DownloadQueueMeta)
                                {
                                    if (q.URL.ToLower() == RunningMetaDownload.URL.ToLower())
                                    {
                                        FoundInRunningQueue = true;
                                        break;
                                    }
                                }
                                if (FoundInRunningQueue == false)
                                {
                                    DeleteMetaDownload = true;
                                    if (RunningMetaDownloadNet != null)
                                        RunningMetaDownloadNet.StopDownload = true;
                                }
                            }
                        }
                    }
                    FilesystemData.WritePackageList();
                    FilesystemData.WriteUserPackageList();
                }

                if (RunningMetaDownload == null)
                {
                    if (DownloadQueueMeta.Count > 0)
                    {
                        FoxEventLog.VerboseWriteEventLog("DownloadThread() - Grabbing DL queue", System.Diagnostics.EventLogEntryType.Information);
                        RunningMetaDownload = DownloadQueueMeta[0];
                        MetaDownloaderThreadHandle = new Thread(new ThreadStart(MetaDownloaderThread));
                        MetaDownloaderThreadHandle.Start();
                    }
                }

                int Tim = 0;
                while (Tim < 60)
                {
                    Thread.Sleep(1000);
                    Tim++;
                    if (StopThreads == true)
                        break;
                }
            }
        }
    }
}
