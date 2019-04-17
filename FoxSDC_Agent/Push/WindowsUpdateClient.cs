using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WUApiLib;

namespace FoxSDC_Agent.Push
{
    class WindowsUpdateClient
    {
        static ManualResetEvent EV = new ManualResetEvent(true);
        static WUStatus Status = new WUStatus() { Text = "Idle" };
        static WUUpdateInfoList lst;

        public static WUStatus GetStatus()
        {
            return (Status);
        }

        public static NetBool QueryRestartPending()
        {
            NetBool nb = new NetBool();
            nb.Data = false;
            using (RegistryKey r = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Component Based Servicing"))
            {
                if (r != null)
                {
                    foreach (string n in r.GetValueNames())
                    {
                        if (n.ToLower() == "rebootpending")
                        {
                            nb.Data = true;
                            return (nb);
                        }
                    }
                }
            }

            using (RegistryKey r = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\\Auto Update"))
            {
                if (r != null)
                {
                    foreach (string n in r.GetValueNames())
                    {
                        if (n.ToLower() == "rebootrequired")
                        {
                            nb.Data = true;
                            return (nb);
                        }
                    }
                }
            }

            return (nb);
        }

        public static int CheckForUpdates()
        {
            Thread t = new Thread(new ThreadStart(CheckForUpdatesT));
            t.Start();
            return (0);
        }

        public static WUUpdateInfoList GetUpdateList()
        {
            if (lst == null || lst.List == null)
            {
                WUUpdateInfoList l = new WUUpdateInfoList();
                l.List = new List<WUUpdateInfo>();
                return (l);
            }
            return (lst);
        }

        static void CheckForUpdatesT()
        {
            try
            {
                if (EV.WaitOne(1000) == false)
                    return;
                EV.Reset();

                lst = new WUUpdateInfoList();
                lst.List = new List<WUUpdateInfo>();

                Status.Text = "Checking for updates (list only)";

                UpdateSession UpdateSession = new UpdateSession();
                UpdateSession.ClientApplicationID = "Fox SDC Update Controller";

                IUpdateSearcher upd = UpdateSession.CreateUpdateSearcher();
                ISearchResult res = upd.Search("IsInstalled=0 and Type='Software' and IsHidden=0");

                if (res.Updates.Count == 0)
                    return;

                for (int i = 0; i < res.Updates.Count; i++)
                {
                    WUUpdateInfo wu = new WUUpdateInfo();
                    wu.Name = res.Updates[i].Title;
                    wu.Description = res.Updates[i].Description;
                    wu.Link = res.Updates[i].SupportUrl;
                    wu.ID = res.Updates[i].Identity.UpdateID;
                    lst.List.Add(wu);
                }
                return;
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("Failed to check for WU " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                lst = new WUUpdateInfoList();
                lst.List = new List<WUUpdateInfo>();
                return;
            }
            finally
            {
                Status.Text = "Idle";
                EV.Set();
            }
        }

        public static int InstallUpdates()
        {
            Thread t = new Thread(new ThreadStart(InstallUpdatesT));
            t.Start();
            return (0);
        }

        static void InstallUpdatesT()
        {
            try
            {
                if (EV.WaitOne(1000) == false)
                    return;
                EV.Reset();

                Status.Text = "Checking for updates";

                UpdateSession UpdateSession = new UpdateSession();
                UpdateSession.ClientApplicationID = "Fox SDC Update Controller";

                IUpdateSearcher upd = UpdateSession.CreateUpdateSearcher();
                ISearchResult res = upd.Search("IsInstalled=0 and Type='Software' and IsHidden=0");

                for (int i = 0; i < res.Updates.Count; i++)
                {
                    Status.Text = "Downloading " + res.Updates[i].Title + " (" + (i + 1).ToString() + " of " + res.Updates.Count.ToString() + ")";
                    UpdateDownloader downloader = UpdateSession.CreateUpdateDownloader();
                    downloader.Updates = new WUApiLib.UpdateCollection();
                    downloader.Updates.Add(res.Updates[i]);
                    downloader.Download();
                }

                for (int i = 0; i < res.Updates.Count; i++)
                {
                    Status.Text = "Installing " + res.Updates[i].Title + " (" + (i + 1).ToString() + " of " + res.Updates.Count.ToString() + ")";
                    IUpdateInstaller installer = UpdateSession.CreateUpdateInstaller();
                    if (installer.IsBusy == true)
                        return;
                    installer.Updates = new WUApiLib.UpdateCollection();
                    installer.Updates.Add(res.Updates[i]);
                    IInstallationResult ires = installer.Install();
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("Failed to install WU " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            finally
            {
                Status.Text = "Idle";
                EV.Set();
            }
            return;
        }
    }
}
