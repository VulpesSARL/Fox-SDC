using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Push
{
    class PushMain10
    {
        class PushDataForThreadRunner
        {
            public PushData p;
            public Network net;
        }

        static bool StopThread = false;
        static Thread pm;

        static object ChatQueueLock = new object();
        static List<PushChatMessage> ChatQueue = new List<PushChatMessage>();
        static List<Int64> ChatPickedUp = new List<long>();

        static void PushMessage(PushChatMessage msg)
        {
            lock (ChatQueueLock)
            {
                foreach (PushChatMessage q in ChatQueue)
                {
                    if (q.ID == msg.ID)
                        return;
                }
                ChatQueue.Add(msg);
                ChatQueue = ChatQueue.OrderBy(c => c.DT).ToList();
            }
        }

        public static PushChatMessage PopMessage()
        {
            lock (ChatQueueLock)
            {
                if (ChatQueue.Count == 0)
                    return (null);
                PushChatMessage q = ChatQueue[0];
                ChatQueue.RemoveAt(0);
                if (q.ID != 0)
                    ChatPickedUp.Add(q.ID);
                return (q);
            }
        }

        static void ConfirmPopMessage(Network net)
        {
            lock (ChatQueueLock)
            {
                if (ChatPickedUp.Count == 0)
                    return;
                do
                {
                    if (net.ConfirmChat(ChatPickedUp[0]) == false)
                        return;
                    ChatPickedUp.RemoveAt(0);
                } while (ChatPickedUp.Count > 0);
            }
        }

        const int WaitNoConnection = 120;
        const int WaitPDisNULL = 60;
        const int WaitPDisNULL2 = 10;
        const int WaitTamperIssue = 30;
        const int WaitCrash = 60;
        const int WaitQuit = 30;
        const int WaitNoClone = 30;
        const int ChatPickupPeriodMin = 5;

        static void PickupMessages(Network net)
        {
            List<PushChatMessage> chats = net.GetChatMessagesForClient();
            if (chats == null)
                return;
            foreach (PushChatMessage c in chats)
                PushMessage(c);
        }

        static public void StartPushThread()
        {
            FoxEventLog.VerboseWriteEventLog("Push10: Starting Push Thread", System.Diagnostics.EventLogEntryType.Information);
            pm = new Thread(new ThreadStart(PushThread));
            pm.Start();
        }

        static public void StopPushThread()
        {
            FoxEventLog.VerboseWriteEventLog("Push10: Stopping Push Thread", System.Diagnostics.EventLogEntryType.Information);
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
                    case "ping":
                        t.net.ResponsePushData10("ok", t.p.Action, 10, t.p.ReplyID);
                        break;
                    case "chatmessage":
                        try
                        {
                            PushChatMessage m = JsonConvert.DeserializeObject<PushChatMessage>(t.p.AdditionalData1);
                            PushMessage(m);
                        }
                        catch
                        {

                        }
                        t.net.ResponsePushData10(new NetBool() { Data = true }, t.p.Action, 10, t.p.ReplyID);
                        break;
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.VerboseWriteEventLog("Push10: PushThreadActionRunner thread crashed", System.Diagnostics.EventLogEntryType.Information);
            }
        }

        static void PushThread()
        {
            Network net = null;
            PushDataRoot pd;
            int Crashes = 0;
            DateTime? ChatPickup = null;
            do
            {
                try
                {
                    if (net == null)
                        net = Utilities.ConnectNetwork(-1);

                    if (net == null)
                    {
                        FoxEventLog.VerboseWriteEventLog("Push10: no connection", System.Diagnostics.EventLogEntryType.Information);
                        for (int i = 0; i < WaitNoConnection; i++)
                        {
                            Thread.Sleep(1000);
                            if (StopThread == true)
                                return;
                        }
                        continue;
                    }

                    ConfirmPopMessage(net);
                    if (ChatPickup == null)
                        ChatPickup = DateTime.UtcNow.AddDays(-1);
                    if ((DateTime.UtcNow - ChatPickup.Value).TotalMinutes > ChatPickupPeriodMin)
                    {
                        PickupMessages(net);
                        ChatPickup = DateTime.UtcNow;
                    }

                    pd = net.GetPushData10();
                    if (pd == null)
                    {
                        FoxEventLog.VerboseWriteEventLog("Push10: pd==null", System.Diagnostics.EventLogEntryType.Information);
                        for (int i = 0; i < WaitPDisNULL; i++)
                        {
                            Thread.Sleep(1000);
                            if (StopThread == true)
                                return;
                        }
                        pd = net.GetPushData10();
                        if (pd == null)
                        {
                            net = null;
                            FoxEventLog.VerboseWriteEventLog("Push10: pd==null - 2nd time - resetting connection", System.Diagnostics.EventLogEntryType.Information);
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
                        FoxEventLog.WriteEventLog("Push10: One or more PushData were tampered - no PushData will be processed.", System.Diagnostics.EventLogEntryType.Error);
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
                        FoxEventLog.VerboseWriteEventLog("Push10: repeat", System.Diagnostics.EventLogEntryType.Information);
                        continue;
                    }
                    if (pd.Data.Action == "quit")
                    {
                        FoxEventLog.VerboseWriteEventLog("Push10: quit", System.Diagnostics.EventLogEntryType.Information);
                        net = null;
                        for (int i = 0; i < WaitQuit; i++)
                        {
                            Thread.Sleep(1000);
                            if (StopThread == true)
                                return;
                        }
                        continue;
                    }

                    Thread a = new Thread(new ParameterizedThreadStart(PushThreadActionRunner));
                    PushDataForThreadRunner t = new PushDataForThreadRunner();
                    t.net = net.CloneElement2();
                    t.p = pd.Data;
                    a.Start(t);
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.VerboseWriteEventLog("Push10: SEH internally", System.Diagnostics.EventLogEntryType.Information);
                    Crashes++;
                    if (Crashes > 3)
                    {
                        FoxEventLog.VerboseWriteEventLog("Push10: Resetting connection due too many crashes", System.Diagnostics.EventLogEntryType.Information);
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
