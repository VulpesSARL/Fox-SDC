using FoxSDC_Agent;
using FoxSDC_Agent.PolicyObjects;
using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDCApplyUserSettings
{
    class Program
    {
        static bool NoPackages = false;
        static int Main(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.ToLower() == "-nopackages")
                    NoPackages = true;
            }

            FoxEventLog.Shutup = true;

            ProgramAgent.Init();

            if (ProgramAgent.LoadDLL() == false)
                return (1);

            FoxEventLog.Shutup = false;
#if !DEBUG
            List<string> Additionals = new List<string>();
            Additionals.Add(Assembly.GetExecutingAssembly().Location);
            if (ProgramAgent.TestIntegrity(Additionals) == false)
            {
                FoxEventLog.WriteEventLog("Apply User settings: Integrity failed!", EventLogEntryType.Error, true);
                return (1);
            }
#endif

            FoxEventLog.Shutup = true;

            if (SystemInfos.CollectSystemInfo() != 0)
                return (1);

            if (ApplicationCertificate.LoadCertificate() == false)
            {
                FoxEventLog.Shutup = false;
                FoxEventLog.WriteEventLog("Apply User settings: Cannot load certificate", System.Diagnostics.EventLogEntryType.Error);
                return (1);
            }

            FoxEventLog.Shutup = false;

            if (FilesystemData.LoadCertificates(true) == false)
                return (1);
            if (FilesystemData.LoadPolicies() == false)
                return (1);
            FilesystemData.LoadLocalPackageData();
            FilesystemData.LoadLocalPackages();
            FilesystemData.LoadUserPackageData();
            FilesystemData.LoadEventLogList();

            SyncPolicy.ApplyPolicy(SyncPolicy.ApplyPolicyFunction.ApplyUser);

            if (NoPackages == true)
                return (0);

            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                return (2);

            foreach (PackagesToInstall pkg in FilesystemData.LocalPackages)
            {
                LocalPackageData lpkg = FilesystemData.FindLocalPackageFromListLatest(pkg.PackageID);
                if (lpkg == null)
                    continue;
                if (pkg.Version != lpkg.Version)
                    continue;

                PackageInstaller inst = new PackageInstaller();
                string metafile = PackagesFolder + pkg.MetaFilename;
                if (File.Exists(metafile) == false)
                    continue;
                string Error;
                PKGRecieptData Reciept;
                PKGStatus res;
                if (inst.InstallPackage(metafile, PackageCertificate.ActivePackageCerts, PackageInstaller.InstallMode.ApplyUserSettingsTest, true, out Error, out res, out Reciept) == false)
                {
                    FoxEventLog.WriteEventLog("Apply User settings: The Metapackage " + pkg.MetaFilename + " cannot be tested: " + Error, System.Diagnostics.EventLogEntryType.Error);
                    continue;
                }
                FoxEventLog.VerboseWriteEventLog("Apply User settings: Applying user settings for " + pkg.MetaFilename, EventLogEntryType.Information);
                if (inst.ApplyUserSettings(metafile, PackageCertificate.ActivePackageCerts, out Error, out res) == false)
                {
                    FoxEventLog.WriteEventLog("Apply User settings: The Metapackage " + pkg.MetaFilename + " cannot be used to apply user settings: " + Error, System.Diagnostics.EventLogEntryType.Error);
                    continue;
                }
            }

            if (RegistryData.Verbose == 1)
            {
                FoxEventLog.VerboseWriteEventLog("Apply User settings: ApplyUserSettings success for " + Environment.UserDomainName + "\\" + Environment.UserName, EventLogEntryType.Information);
            }

            return (0);
        }
    }
}
