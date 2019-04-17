using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Push
{
    //used for general purpose tasks
    class PushMain0
    {
        class PushDataForThreadRunner
        {
            public PushData p;
            public Network net;
        }

        static bool StopThread = false;
        static Thread pm;
        const int WaitNoConnection = 120;
        const int WaitPDisNULL = 60;
        const int WaitPDisNULL2 = 10;
        const int WaitTamperIssue = 30;
        const int WaitCrash = 60;
        const int WaitQuit = 30;
        const int WaitNoClone = 30;
        static public void StartPushThread()
        {
            FoxEventLog.VerboseWriteEventLog("Push0: Starting Push Thread", System.Diagnostics.EventLogEntryType.Information);
            pm = new Thread(new ThreadStart(PushThread));
            pm.Start();
        }

        static public void StopPushThread()
        {
            FoxEventLog.VerboseWriteEventLog("Push0: Stopping Push Thread", System.Diagnostics.EventLogEntryType.Information);
            StopThread = true;
            if (pm != null)
                pm.Join();
        }

        static void PushThreadActionRunner(object o)
        {
            if (!(o is PushDataForThreadRunner))
                return;
            PushDataForThreadRunner t = (PushDataForThreadRunner)o;

            try

            {
                switch (t.p.Action)
                {
                    case "clock":
                        t.net.ResponsePushData0(DateTime.UtcNow, t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "ping":
                        t.net.ResponsePushData0("ok", t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "tasks":
                        t.net.ResponsePushData0(TaskManager.GetTasks(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "killtask":
                        t.net.ResponsePushData0(TaskManager.KillTask(t.p.AdditionalData1), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "listfiles":
                        t.net.ResponsePushData0(Filesystem.ListFiles(t.p.AdditionalData1), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "checkfile":
                        t.net.ResponsePushData0(Filesystem.CheckFile(t.p.AdditionalData1), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "getsessions":
                        t.net.ResponsePushData0(TaskManager.GetTSRunningSessions(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "runtask":
                        t.net.ResponsePushData0(TaskManager.RunTask(t.p.AdditionalData1, t.net), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "wugetlist":
                        t.net.ResponsePushData0(WindowsUpdateClient.GetUpdateList(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "wucheck":
                        t.net.ResponsePushData0(WindowsUpdateClient.CheckForUpdates(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "wustatus":
                        t.net.ResponsePushData0(WindowsUpdateClient.GetStatus(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "wuinstall":
                        t.net.ResponsePushData0(WindowsUpdateClient.InstallUpdates(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "wustatusrestart":
                        t.net.ResponsePushData0(WindowsUpdateClient.QueryRestartPending(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "restartsystem":
                        ProgramAgent.CPP.RestartSystem();
                        t.net.ResponsePushData0(new NetInt32() { Data = 0 }, t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "restartsystemforced":
                        ProgramAgent.CPP.RestartSystemForced();
                        t.net.ResponsePushData0(new NetInt32() { Data = 0 }, t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "services":
                        t.net.ResponsePushData0(Services.GetServices(), t.p.Action, 0, t.p.ReplyID);
                        break;
                    case "servicecontrol":
                        t.net.ResponsePushData0(Services.ServiceControl(t.p.AdditionalData1), t.p.Action, 0, t.p.ReplyID);
                        break;
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.VerboseWriteEventLog("Push0: PushThreadActionRunner thread crashed", System.Diagnostics.EventLogEntryType.Information);
            }
        }

        static void PushThread()
        {
            Network net = null;
            PushDataRoot pd;
            int Crashes = 0;
            do
            {
                try
                {
                    if (net == null)
                        net = Utilities.ConnectNetwork(-1);

                    if (net == null)
                    {
                        FoxEventLog.VerboseWriteEventLog("Push0: no connection", System.Diagnostics.EventLogEntryType.Information);
                        for (int i = 0; i < WaitNoConnection; i++)
                        {
                            Thread.Sleep(1000);
                            if (StopThread == true)
                                return;
                        }
                        continue;
                    }
                    pd = net.GetPushData0();
                    if (pd == null)
                    {
                        FoxEventLog.VerboseWriteEventLog("Push0: pd==null", System.Diagnostics.EventLogEntryType.Information);
                        for (int i = 0; i < WaitPDisNULL; i++)
                        {
                            Thread.Sleep(1000);
                            if (StopThread == true)
                                return;
                        }
                        pd = net.GetPushData0();
                        if (pd == null)
                        {
                            net = null;
                            FoxEventLog.VerboseWriteEventLog("Push0: pd==null - 2nd time - resetting connection", System.Diagnostics.EventLogEntryType.Information);
                            for (int i = 0; i < WaitPDisNULL2; i++)
                            {
                                Thread.Sleep(1000);
                                if (StopThread == true)
                                    return;
                            }
                            continue;
                        }
                    }
                    if (ApplicationCertificate.Verify(pd) == false)
                    {
                        FoxEventLog.WriteEventLog("Push0: One or more PushData were tampered - no PushData will be processed.", System.Diagnostics.EventLogEntryType.Error);
                        for (int i = 0; i < WaitTamperIssue; i++)
                        {
                            Thread.Sleep(1000);
                            if (StopThread == true)
                                return;
                        }
                        continue;
                    }
                    Crashes = 0;

                    if (pd.Data.Action == "repeat")
                    {
                        FoxEventLog.VerboseWriteEventLog("Push0: repeat", System.Diagnostics.EventLogEntryType.Information);
                        continue;
                    }
                    if (pd.Data.Action == "quit")
                    {
                        FoxEventLog.VerboseWriteEventLog("Push0: quit", System.Diagnostics.EventLogEntryType.Information);
                        net = null;
                        for (int i = 0; i < WaitQuit; i++)
                        {
                            Thread.Sleep(1000);
                            if (StopThread == true)
                                return;
                        }
                        continue;
                    }
                    if (pd.Data.Action == "stdin")
                    {
                        Redirs.MainSTDIORedir.ProcessStdInAgent(pd.Data.AdditionalData1);
                        continue;
                    }

                    Thread a = new Thread(new ParameterizedThreadStart(PushThreadActionRunner));
                    PushDataForThreadRunner t = new PushDataForThreadRunner();
                    t.net = net.CloneElement();
                    t.p = pd.Data;
                    a.Start(t);
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.VerboseWriteEventLog("Push0: SEH internally", System.Diagnostics.EventLogEntryType.Information);
                    Crashes++;
                    if (Crashes > 3)
                    {
                        FoxEventLog.VerboseWriteEventLog("Push0: Resetting connection due too many crashes", System.Diagnostics.EventLogEntryType.Information);
                        net = null;
                        Crashes = 0;
                    }
                    for (int i = 0; i < WaitCrash; i++)
                    {
                        Thread.Sleep(1000);
                        if (StopThread == true)
                            return;
                    }
                }
            } while (StopThread == false);
        }
    }
}
