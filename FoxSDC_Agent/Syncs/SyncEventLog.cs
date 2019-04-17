using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class SyncEventLog
    {
        static List<EventLogReport> lst = new List<EventLogReport>();
        static HashSet<string> HasEVTLogs;
        static bool CollectEVT(string Book)
        {
            EventLog eventLog;
            eventLog = new EventLog(Book);
            try
            {
                foreach (EventLogEntry log in eventLog.Entries)
                {
                    EventLogReport ev = new EventLogReport();

                    ev.Category = log.Category;
                    ev.CategoryNumber = log.CategoryNumber;
                    ev.Data = log.Data;
                    ev.EventLog = Book;
                    ev.EventLogType = (int)log.EntryType;
                    ev.InstanceID = log.InstanceId;
                    ev.LogID = "";
                    ev.MachineID = SystemInfos.SysInfo.MachineID;
                    ev.Message = log.Message;
                    ev.Source = log.Source;
                    ev.TimeGenerated = log.TimeGenerated;
                    ev.TimeWritten = log.TimeWritten;
                    ev.JSONReplacementStrings = JsonConvert.SerializeObject(log.ReplacementStrings);
                    CommonUtilities.CalcEventLogID(ev);
                    HasEVTLogs.Add(ev.LogID);
                    lst.Add(ev);
                }
            }
            catch
            {
                FoxEventLog.WriteEventLog("Cannot collect EventLog " + Book, EventLogEntryType.Error);
            }
            return (true);
        }

        public static bool DoSyncEventLog()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);
                net.CloseConnection();

                Status.UpdateMessage(0, "Collecting EventLog data");
                lst = new List<EventLogReport>();
                HasEVTLogs = new HashSet<string>();

                Status.UpdateMessage(0, "Collecting EventLog data (Application)");

                if (CollectEVT("Application") == false)
                    return (false);

                Status.UpdateMessage(0, "Collecting EventLog data (Security)");

                if (CollectEVT("Security") == false)
                    return (false);

                Status.UpdateMessage(0, "Collecting EventLog data (System)");

                if (CollectEVT("System") == false)
                    return (false);

                Status.UpdateMessage(0, "Collecting EventLog data (Processing ...)");

                HashSet<string> RM = new HashSet<string>();

                foreach (string evt in FilesystemData.SyncedEventLog)
                {
                    if (HasEVTLogs.Contains(evt) == false)
                        RM.Add(evt);
                }

                foreach (string evt in RM)
                {
                    FilesystemData.SyncedEventLog.Remove(evt);
                }

                RM.Clear();

                List<EventLogReport> REP = new List<EventLogReport>();

                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);

                Status.UpdateMessage(0, "Collecting EventLog data (Sending data ...)");

                Int64 UploadCounter = 0;
                foreach (EventLogReport evt in lst)
                {
                    UploadCounter++;
                    if (FilesystemData.SyncedEventLog.Contains(evt.LogID) == true)
                        continue;
                    REP.Add(evt);
                    if (REP.Count > 99)
                    {
                        Status.UpdateMessage(0, "Collecting EventLog data (Sending data ... " + UploadCounter.ToString() + " of " + lst.Count.ToString() + ")");

                        if (net.ReportEventLogs(REP) == false)
                        {
                            net.CloseConnection();
                            Status.UpdateMessage(0);
                            return (false);
                        }
                        foreach (EventLogReport rep in REP)
                        {
                            FilesystemData.SyncedEventLog.Add(rep.LogID);
                        }
                        FilesystemData.WriteEventLogList();
                        REP.Clear();
                    }
                }

                if (REP.Count > 0)
                {
                    Status.UpdateMessage(0, "Collecting EventLog data (Sending data ...)");

                    if (net.ReportEventLogs(REP) == false)
                    {
                        net.CloseConnection();
                        Status.UpdateMessage(0);
                        return (false);
                    }
                    foreach (EventLogReport rep in REP)
                    {
                        FilesystemData.SyncedEventLog.Add(rep.LogID);
                    }
                    FilesystemData.WriteEventLogList();
                    REP.Clear();
                }
                lst.Clear();
                HasEVTLogs.Clear();
                net.CloseConnection();

            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing Event Log Data: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);
            return (true);
        }
    }
}
