using FoxSDC_Agent.Push;
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
    partial class Threads
    {
        static Thread ReportingThreadHandle1 = null;
        static Thread ReportingThreadHandle2 = null;
        static Thread PolicyThreadHandle = null;
        static Thread LocalPackagesHandle = null;
        static Thread DownloadThreadHandle = null;

        static object LockLock1 = new object();
        static object LockLock2 = new object();
        static object LockLock3 = new object();

        static bool LockPolicyUpdate = false;

        static bool LockReportingUpdate1 = false;
        static bool LockReportingUpdate2 = false;

        static bool StopThreads = false;

        static bool InvokeReporting1 = false;
        static bool InvokeReporting2 = false;
        static bool InvokePolicy = false;

        static public void StartAllThreads()
        {
            FoxEventLog.VerboseWriteEventLog("StartAllThreads()", System.Diagnostics.EventLogEntryType.Information);
            ReportingThreadHandle1 = new Thread(new ThreadStart(ReportingThread1));
            ReportingThreadHandle2 = new Thread(new ThreadStart(ReportingThread2));
            PolicyThreadHandle = new Thread(new ThreadStart(PolicyThread));
            DownloadThreadHandle = new Thread(new ThreadStart(DownloadThread));
            ReportingThreadHandle1.Start();
            ReportingThreadHandle2.Start();
            PolicyThreadHandle.Start();
            DownloadThreadHandle.Start();
            PushMain0.StartPushThread();
            PushMain1.StartPushThread();
            PushMain2.StartPushThread();
            PushMain10.StartPushThread();
            UpdateCheck.RunUpdateCheckAndHouseKeepingThread();
            DownloadSystemFSData.StartThread();
            if (SystemInfos.SysInfo.RunningInWindowsPE == false || SystemInfos.SysInfo.RunningInWindowsPE == null)
            {
                LocalPackagesHandle = new Thread(new ThreadStart(LocalPackagesThread));
                LocalPackagesHandle.Start();
            }

            FoxEventLog.VerboseWriteEventLog("StartAllThreads() - DONE", System.Diagnostics.EventLogEntryType.Information);
        }

        static public void StopAllThreads()
        {
            FoxEventLog.VerboseWriteEventLog("StopAllThreads()", System.Diagnostics.EventLogEntryType.Information);
            StopThreads = true;
            Redirs.PortMappings_Kernel.StopAllConnections();
            if (PolicyThreadHandle != null)
                PolicyThreadHandle.Join();
            if (ReportingThreadHandle1 != null)
                ReportingThreadHandle1.Join();
            if (ReportingThreadHandle2 != null)
                ReportingThreadHandle2.Join();
            if (DownloadThreadHandle != null)
                DownloadThreadHandle.Join();
            if (LocalPackagesHandle != null)
                LocalPackagesHandle.Join();
            StopDownloads();
            PushMain0.StopPushThread();
            PushMain1.StopPushThread();
            PushMain2.StopPushThread();
            PushMain10.StopPushThread();
            UpdateCheck.StopUpdateThread();
            DownloadSystemFSData.StopThread();
            FoxEventLog.VerboseWriteEventLog("StopAllThreads() - Done", System.Diagnostics.EventLogEntryType.Information);
        }

        static public void InvokePolicySync()
        {
            InvokePolicy = true;
        }

        static public void InvokeReportingSync()
        {
            InvokeReporting1 = true;
            InvokeReporting2 = true;
        }

        static void PolicyThread()
        {
            Int64 NextRunTime = RegistryData.LastSyncPolicies;
            Int64 CurrentTime;
            while (StopThreads == false)
            {
                CurrentTime = CommonUtilities.DTtoINT(DateTime.Now);

                if (InvokePolicy == true)
                {
                    InvokePolicy = false;
                    NextRunTime = CommonUtilities.DTtoINT(DateTime.Now.AddMinutes(-1));
                }

                if (CurrentTime < NextRunTime)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                lock (LockLock3)
                {
                    if (LockPolicyUpdate == true)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    LockPolicyUpdate = true;
                }

                try
                {
                    SyncPolicy.DoSyncPolicy();

                    #region Prevent criss cross with reportings wait until done!
                    do
                    {
                        lock (LockLock1)
                        {
                            if (LockReportingUpdate1 == true)
                            {
                                if (StopThreads == true)
                                {
                                    LockReportingUpdate1 = false;
                                    break;
                                }
                                Thread.Sleep(1000);
                                continue;
                            }
                            LockReportingUpdate1 = true;
                            break;
                        }
                    } while (true);

                    #endregion

                    if (StopThreads == false)
                    {
                        SyncPolicy.ApplyPolicy(SyncPolicy.ApplyPolicyFunction.ApplySystem);
                    }

                    #region Prevent criss cross with reportings wait until done!
                    lock (LockLock1)
                    {
                        LockReportingUpdate1 = false;
                    }
                    #endregion
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.WriteEventLog("Servere error while syncing / applying policies: " + ee.ToString(), EventLogEntryType.Error);
                }
                Int64 Tim = RegistryData.LastSyncPoliciesWaitTime;
                if (Tim < 1)
                    Tim = 5;

                NextRunTime = CommonUtilities.DTtoINT(DateTime.Now.AddMinutes(Tim));
                RegistryData.LastSyncPolicies = NextRunTime;

                lock (LockLock3)
                {
                    LockPolicyUpdate = false;
                }
            }
        }

        static void ReportingThread2()
        {
            Int64 NextRunTime = RegistryData.LastSyncReporting2;
            Int64 CurrentTime;
            while (StopThreads == false)
            {
                CurrentTime = CommonUtilities.DTtoINT(DateTime.Now);

                if (InvokeReporting2 == true)
                {
                    InvokeReporting2 = false;
                    NextRunTime = CommonUtilities.DTtoINT(DateTime.Now.AddMinutes(-1));
                }

                if (CurrentTime < NextRunTime)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                lock (LockLock2)
                {
                    if (LockReportingUpdate2 == true)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    LockReportingUpdate2 = true;
                }

                if (SystemInfos.SysInfo.RunningInWindowsPE == null || SystemInfos.SysInfo.RunningInWindowsPE == false)
                {
                    if (StopThreads == false)
                        if (RegistryData.DisableSMARTSync == false)
                            SyncSMARTData.DoSyncSMART();
                    if (StopThreads == false)
                        if (RegistryData.EnableBitlockerRKSync == true)
                            SyncBitlockerRK.DoSyncBitlockerRK();
                    if (StopThreads == false)
                        if (RegistryData.DisableWinLicenseSync == false)
                            SyncWindowsLic.DoSyncWindowsLic();
                }

                Int64 Tim = RegistryData.LastSyncReportingWaitTime2;
                if (Tim < 1)
                    Tim = 60;

                NextRunTime = CommonUtilities.DTtoINT(DateTime.Now.AddMinutes(Tim));
                RegistryData.LastSyncReporting2 = NextRunTime;

                lock (LockLock2)
                {
                    LockReportingUpdate2 = false;
                }
            }
        }

        static void ReportingThread1()
        {
            Int64 NextRunTime = RegistryData.LastSyncReporting;
            Int64 CurrentTime;
            while (StopThreads == false)
            {
                CurrentTime = CommonUtilities.DTtoINT(DateTime.Now);

                if (InvokeReporting1 == true)
                {
                    InvokeReporting1 = false;
                    NextRunTime = CommonUtilities.DTtoINT(DateTime.Now.AddMinutes(-1));
                }

                if (CurrentTime < NextRunTime)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                lock (LockLock1)
                {
                    if (LockReportingUpdate1 == true)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    LockReportingUpdate1 = true;
                }

                if (SystemInfos.SysInfo.RunningInWindowsPE == null || SystemInfos.SysInfo.RunningInWindowsPE == false)
                {
                    if (StopThreads == false)
                        if (RegistryData.DisableAddRemoveProgramsSync == false)
                            SyncAddRemovePrograms.DoSyncAddRemovePrograms();

                    if (StopThreads == false)
                        if (RegistryData.DisableDiskDataSync == false)
                            SyncDiskData.DoSyncDiskData();

                    if (StopThreads == false)
                        if (RegistryData.DisableNetadapterSync == false)
                            SyncNetworkAdapterConfig.DoSyncNetAdapterConfig();

                    if (StopThreads == false)
                        if (RegistryData.DisableDeviceManagerSync == false)
                            SyncDeviceManager.DoSyncDeviceManager();

                    if (StopThreads == false)
                        if (RegistryData.DisableFilterDriverSync == false)
                            SyncFilterDrivers.DoSyncFilters();

                    if (StopThreads == false)
                        if (RegistryData.DisableUsersSync == false)
                            SyncUsers.DoSyncUsers();

                    if (StopThreads == false)
                        if (RegistryData.DisableStartupSync == false)
                            SyncStartups.DoSyncStartups();

                    if (StopThreads == false)
                        if (RegistryData.DisableSimpleTasks == false)
                            SyncSimpleTasks.DoSyncSimpleTasks();

                    if (StopThreads == false)
                        if (RegistryData.DisableEventLogSync == false)
                            SyncEventLog.DoSyncEventLog();
                }

                Int64 Tim = RegistryData.LastSyncReportingWaitTime;
                if (Tim < 1)
                    Tim = 10;

                NextRunTime = CommonUtilities.DTtoINT(DateTime.Now.AddMinutes(Tim));
                RegistryData.LastSyncReporting = NextRunTime;

                lock (LockLock1)
                {
                    LockReportingUpdate1 = false;
                }
            }
        }
    }
}
