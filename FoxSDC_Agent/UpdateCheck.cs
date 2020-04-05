using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class UpdateCheck
    {
        static bool StopThread = false;
        static Thread UpdateThreadHandle = null;
        static Network net = null;

        static bool RunUpdate()
        {
            string pkgfile = ProgramAgent.AppPath + "agentupdate.foxpkg";
            const string PackageID = "Vulpes-SDCA1-Update";

            net = Utilities.ConnectNetwork(-1);
            if (net == null)
                return (false);

            bool EarlyChannel = RegistryData.EarlyUpdates == 1 ? true : false;

            Int64? Version = net.GetAvailableAgentVersion(EarlyChannel);
            if (Version == null)
            {
                net.CloseConnection();
                return (false);
            }

            if (Version.Value <= FoxVersion.Version)
            {
                net.CloseConnection();
                return (false);
            }

            if (net.DownloadFile("api/" + (EarlyChannel == true ? "early" : "") + "update/package/1", pkgfile) == false)
            {
                net.CloseConnection();
                return (false);
            }

            net.CloseConnection();

            PackageInstaller pkgi = new PackageInstaller();
            string Error;

            if (pkgi.PackageInfo(pkgfile, null, out Error) == false)
            {
                CommonUtilities.SpecialDeleteFile(pkgfile);
                FoxEventLog.WriteEventLog("Self-Update: Error reading package file " + pkgfile + ": " + Error, System.Diagnostics.EventLogEntryType.Error);
                return (false);
            }

            if (pkgi.PackageInfoData.PackageID != PackageID)
            {
                CommonUtilities.SpecialDeleteFile(pkgfile);
                FoxEventLog.WriteEventLog("Self-Update: PackageID mismatch on file " + pkgfile, System.Diagnostics.EventLogEntryType.Error);
                return (false);
            }

            if (pkgi.PackageInfoData.VersionID <= FoxSDC_Agent.FoxVersion.Version)
            {
                CommonUtilities.SpecialDeleteFile(pkgfile);
                FoxEventLog.WriteEventLog("Self-Update: Version on package is same or older than installed for file " + pkgfile, System.Diagnostics.EventLogEntryType.Error);
                return (false);
            }

            string UpdateApp = ProgramAgent.AppPath + "FoxSDC_Selfupdate.exe";
            FoxEventLog.WriteEventLog("Self-Update: Starting " + UpdateApp + " to update from " + FoxSDC_Agent.FoxVersion.Version.ToString() + " to " + pkgi.PackageInfoData.VersionID.ToString(), System.Diagnostics.EventLogEntryType.Information);

#if !DEBUG
            if (ProgramAgent.CPP.VerifyEXESignature(UpdateApp) == false)
            {
                FoxEventLog.WriteEventLog("The file " + UpdateApp + " cannot be verified. Update will not work.", EventLogEntryType.Error);
            }
            else
#endif
            {
                try
                {
                    Process.Start(UpdateApp);
                }
                catch
                {

                }
            }

            return (true);
        }

        static void UpdateThreadRunner()
        {
            do
            {
                try
                {
                    if (RunUpdate() == true)
                        break;
                }
                catch(Exception ee)
                {
                    FoxEventLog.WriteEventLog("Something horribly got wrong while checking for updates: " + ee.ToString(), EventLogEntryType.Error);
                }

                try
                {
                    Redirs.MainNetRedirLegacy.TestTimeouts();
                }
                catch(Exception ee)
                {
                    FoxEventLog.WriteEventLog("Something horribly got wrong while performing housekeeping (1): " + ee.ToString(), EventLogEntryType.Error);
                }

                try
                {
                    Redirs.MainNetRedirWS.TestTimeouts();
                }
                catch (Exception ee)
                {
                    FoxEventLog.WriteEventLog("Something horribly got wrong while performing housekeeping (2): " + ee.ToString(), EventLogEntryType.Error);
                }

                try
                {
                    Redirs.MainScreenDataWS.TestTimeouts();
                }
                catch (Exception ee)
                {
                    FoxEventLog.WriteEventLog("Something horribly got wrong while performing housekeeping (3): " + ee.ToString(), EventLogEntryType.Error);
                }

                for (int i = 0; i < 120; i++)
                {
                    Thread.Sleep(1000);
                    if (StopThread == true)
                        break;
                }
            } while (StopThread == false);
        }

        public static void RunUpdateCheckAndHouseKeepingThread()
        {
            UpdateThreadHandle = new Thread(new ThreadStart(UpdateThreadRunner));
            UpdateThreadHandle.Start();
        }

        public static void StopUpdateThread()
        {
            StopThread = true;
            if (net != null)
                net.StopDownload = true;
            if (UpdateThreadHandle != null)
                UpdateThreadHandle.Join();

        }
    }
}
