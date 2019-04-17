using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Agent;

namespace FoxSDC_Common
{
    static class Program
    {
        [STAThread]
        static int Main()
        {
            List<int> UIRunningInSession = new List<int>();
            const string PackageID = "Vulpes-SDCA1-Update";
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (dir.EndsWith("\\") == false)
                dir += "\\";

            string pkgfile = dir + "agentupdate.foxpkg";

            if (File.Exists(pkgfile) == false)
                return (1);

            PackageInstaller pkgi = new PackageInstaller();
            string Error;

            if (pkgi.PackageInfo(pkgfile, null, out Error) == false)
            {
                FoxEventLog.WriteEventLog("Self-Update: Error reading package file " + pkgfile + ": " + Error, EventLogEntryType.Error);
                return (2);
            }

            if (pkgi.PackageInfoData.PackageID != PackageID)
            {
                FoxEventLog.WriteEventLog("Self-Update: PackageID mismatch on file " + pkgfile, EventLogEntryType.Error);
                return (3);
            }

            FileVersionInfo fv = FileVersionInfo.GetVersionInfo(dir + "FoxSDC_Agent.exe");
            Int64 sdcversion = Convert.ToInt64(fv.FileBuildPart.ToString("0000") + fv.FilePrivatePart.ToString("0000"));

            if (pkgi.PackageInfoData.VersionID <= sdcversion)
            {
                FoxEventLog.WriteEventLog("Self-Update: Version on package is same or older than installed for file " + pkgfile + "\r\nPackage: " + pkgi.PackageInfoData.VersionID.ToString() + "\r\nProgram: " + fv.ToString(), EventLogEntryType.Error);
                return (4);
            }

            ServiceController svc = new ServiceController("FoxSDCA");

            try
            {
                svc.Stop();
            }
            catch
            {

            }

            int i = 0;

            do
            {
                i++;
                if (i > 120 * 4)
                    break;
                svc.Refresh();
                Thread.Sleep(1000);
            } while (svc.Status != ServiceControllerStatus.Stopped);

            #region Kill Processes

            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainModule.FileName.ToLower() == dir.ToLower() + "foxsdc_agent_ui.exe")
                    {
                        if (UIRunningInSession.Contains(proc.SessionId) == false)
                            UIRunningInSession.Add(proc.SessionId);
                        proc.Kill();
                    }
                }
                catch
                {

                }
            }

            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainModule.FileName.ToLower() == dir.ToLower() + "foxsdc_applyusersettings.exe")
                    {
                        proc.Kill();
                    }
                }
                catch
                {

                }
            }

            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainModule.FileName.ToLower() == dir.ToLower() + "foxsdc_agent.exe")
                    {
                        proc.Kill();
                    }
                }
                catch
                {

                }
            }

            #endregion

            FoxEventLog.WriteEventLog("Self-Update: Updating from " + pkgfile + "\r\nPackage: " + pkgi.PackageInfoData.VersionID.ToString() + "\r\nProgram: " + sdcversion.ToString(), EventLogEntryType.Information);

            PKGStatus status;
            PKGRecieptData reciept;

            if (pkgi.InstallPackage(pkgfile, null, PackageInstaller.InstallMode.Test, false, out Error, out status, out reciept, "FoxSDC_Selfupdate.exe") == false)
            {
                FoxEventLog.WriteEventLog("Self-Update: Error testing the package file " + pkgfile + ": " + Error, EventLogEntryType.Error);
                svc = new ServiceController("FoxSDCA");
                svc.Start();
                return (5);
            }

            if (pkgi.InstallPackage(pkgfile, null, PackageInstaller.InstallMode.Install, false, out Error, out status, out reciept, "FoxSDC_Selfupdate.exe") == false)
            {
                FoxEventLog.WriteEventLog("Self-Update: Error installing the package file " + pkgfile + ": " + Error, EventLogEntryType.Error);
                svc = new ServiceController("FoxSDCA");
                svc.Start();
                return (6);
            }

            FoxEventLog.WriteEventLog("Self-Update: Completed updating from " + pkgfile + "\r\nPackage: " + pkgi.PackageInfoData.VersionID.ToString() + "\r\nProgram: " + sdcversion.ToString(), EventLogEntryType.Information);

            svc = new ServiceController("FoxSDCA");
            svc.Start();

            try
            {
                File.Delete(pkgfile);
            }
            catch
            {
                CommonUtilities.PendingMove(pkgfile, null);
            }

            SessionStarter.StartProgramInSessions(dir.ToLower() + "foxsdc_agent_ui.exe", UIRunningInSession);

            return (0);
        }
    }
}
