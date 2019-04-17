using FoxSDC_Common;
using FoxSDC_Server.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace FoxSDC_Server
{
    class EventLogReporting
    {
        [VulpesRESTfulRet("EventLogs")]
        public EventLogReportFullList EventLogs;

        [VulpesRESTfulRet("EventLogSources")]
        public NetStringList EventLogSources;


        static List<string> EventSourcesCache = new List<string>();
        static DateTime EventSourcesCacheDT = DateTime.Now.AddYears(-1);
        static object EventSourcesLock = new object();

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/eventlog", "", "")]
        public RESTStatus ReportEventLog(SQLLib sql, ListEventLogReport EventLogList, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            EventLogList.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", EventLogList.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Denied);
                }
            }

            if (EventLogList.Items == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (EventLogList.Items.Count == 0)
                return (RESTStatus.Created);

            DateTime DT = DateTime.Now;

            foreach (EventLogReport ar in EventLogList.Items)
            {
                if (NullTest.Test(ar) == false)
                {
                    ni.Error = "Invalid Items";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }
                CommonUtilities.CalcEventLogID(ar);
            }

            List<SQLParam> sqlparams = new List<SQLParam>();
            sqlparams.Add(new SQLParam("@id", EventLogList.MachineID));
            int count = 1;
            string vars = "";
            foreach (EventLogReport ar in EventLogList.Items)
            {
                sqlparams.Add(new SQLParam("@p" + count.ToString(), ar.LogID));
                vars += "@p" + count.ToString() + ",";
                count++;
            }
            if (vars.EndsWith(",") == true)
                vars = vars.Substring(0, vars.Length - 1);

            List<string> LogIDinDB = new List<string>();

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT LogID FROM EventLog WHERE MachineID=@id and LogID in (" + vars + ")", sqlparams.ToArray());
                while (dr.Read())
                {
                    LogIDinDB.Add(Convert.ToString(dr["LogID"]));
                }
                dr.Close();
            }

            List<EventLogReport> RemoveEVL = new List<EventLogReport>();
            foreach (EventLogReport ar in EventLogList.Items)
            {
                if (LogIDinDB.Contains(ar.LogID) == true)
                {
                    RemoveEVL.Add(ar);
                    continue;
                }
                if (SettingsManager.Settings.KeepEventLogDays > 0)
                {
                    if (ar.TimeGenerated < DateTime.UtcNow.AddDays(0 - SettingsManager.Settings.KeepEventLogDays))
                    {
                        RemoveEVL.Add(ar);
                        continue;
                    }
                }
            }

            foreach (EventLogReport ar in RemoveEVL)
            {
                EventLogList.Items.Remove(ar);
            }

            List<EventLogReportFull> car = new List<EventLogReportFull>();

            lock (ni.sqllock)
            {
                try
                {
                    sql.BeginTransaction();
                    sql.SEHError = true;

                    foreach (EventLogReport ar in EventLogList.Items)
                    {
                        EventLogReportFull arr = new EventLogReportFull();
                        ClassCopy.CopyClassData(ar, arr);
                        arr.Reported = DateTime.UtcNow;
                        arr.MachineID = EventLogList.MachineID;
                        List<SQLData> d = sql.InsertFromClassPrep(arr);
                        foreach (SQLData dd in d)
                        {
                            if (dd.Column == "ID")
                            {
                                dd.Data = DBNull.Value;
                                break;
                            }
                        }
                        car.Add(arr);
                        sql.InsertFromClass("EventLog", arr);
                    }
                    sql.CommitTransaction();
                }
                catch (Exception ee)
                {
                    sql.RollBackTransaction();
                    FoxEventLog.WriteEventLog("DB Error: Cannot insert data to EventLog: " + ee.ToString() + "\r\n\r\nJSON: " +
                        JsonConvert.SerializeObject(car, Formatting.Indented), System.Diagnostics.EventLogEntryType.Error);
                    return (RESTStatus.ServerError);
                }
                finally
                {
                    sql.SEHError = false;
                }
            }

            Thread t = new Thread(new ParameterizedThreadStart(new DReportingThread(ReportingThread)));
            t.Start(car);

            return (RESTStatus.Created);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/eventsources", "EventLogSources", "")]
        public RESTStatus GetEventLogSources(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            EventLogSources = new NetStringList();
            EventLogSources.Items = new List<string>();

            lock (EventSourcesLock)
            {
                if (EventSourcesCacheDT.AddHours(1) < DateTime.UtcNow)
                {
                    EventSourcesCache.Clear();
                    lock (ni.sqllock)
                    {
                        SqlDataReader dr = sql.ExecSQLReader("select distinct Source from EventLog order by Source");
                        while (dr.Read())
                        {
                            EventSourcesCache.Add(Convert.ToString(dr["Source"]));
                        }
                        dr.Close();
                    }
                    EventSourcesCacheDT = DateTime.UtcNow;
                }
            }

            lock (EventSourcesLock)
            {
                EventLogSources.Items = EventSourcesCache;
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/reports/eventlog", "EventLogs", "")]
        public RESTStatus GetEventLogs(SQLLib sql, EventLogSearch eventlogsearch, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            EventLogs = new EventLogReportFullList();
            EventLogs.Data = new List<EventLogReportFull>();

            if (eventlogsearch == null)
                return (RESTStatus.Success);

            if (eventlogsearch.QTY < 1)
                eventlogsearch.QTY = 500;

            string SQLQuery = "SELECT TOP " + eventlogsearch.QTY + " * FROM EventLog WHERE ";
            List<SQLParam> SQLQueryArgs = new List<SQLParam>();

            if (eventlogsearch.MachineID != null)
            {
                if (Computers.MachineExists(sql, eventlogsearch.MachineID) == false)
                {
                    ni.Error = "Invalid Data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }

                SQLQuery += "MachineID=@m AND ";
                SQLQueryArgs.Add(new SQLParam("@m", eventlogsearch.MachineID));
            }

            if (eventlogsearch.Source != null)
            {
                SQLQuery += "Source=@s AND ";
                SQLQueryArgs.Add(new SQLParam("@s", eventlogsearch.Source));
            }

            if (eventlogsearch.EventLogType != null)
            {
                SQLQuery += "EventLogType=@t AND ";
                SQLQueryArgs.Add(new SQLParam("@t", eventlogsearch.EventLogType));
            }

            if (eventlogsearch.FromDate != null)
            {
                SQLQuery += "TimeGenerated>=@tgf AND ";
                SQLQueryArgs.Add(new SQLParam("@tgf", eventlogsearch.FromDate));
            }

            if (eventlogsearch.ToDate != null)
            {
                SQLQuery += "TimeGenerated<=@tgt AND ";
                SQLQueryArgs.Add(new SQLParam("@tgt", eventlogsearch.ToDate));
            }

            if (eventlogsearch.EventLogBook != null)
            {
                SQLQuery += "EventLog=@evtb AND ";
                SQLQueryArgs.Add(new SQLParam("@evtb", eventlogsearch.EventLogBook));
            }

            if (eventlogsearch.CategoryNumber != null)
            {
                SQLQuery += "CategoryNumber=@catnum AND ";
                SQLQueryArgs.Add(new SQLParam("@catnum", eventlogsearch.CategoryNumber));
            }

            SQLQuery = SQLQuery.Trim();

            SQLQuery += "   1=1  ";

            SQLQuery += " ORDER BY TimeGenerated DESC";

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader(SQLQuery, SQLQueryArgs.ToArray());
                while (dr.Read())
                {
                    EventLogReportFull ev = new EventLogReportFull();
                    ev.Category = Convert.ToString(dr["Category"]);
                    ev.CategoryNumber = Convert.ToInt32(dr["CategoryNumber"]);
                    ev.Data = (byte[])dr["Data"];
                    ev.EventLog = Convert.ToString(dr["EventLog"]);
                    ev.EventLogType = Convert.ToInt32(dr["EventLogType"]);
                    ev.InstanceID = Convert.ToInt64(dr["InstanceID"]);
                    ev.JSONReplacementStrings = Convert.ToString(dr["JSONReplacementStrings"]);
                    ev.LogID = Convert.ToString(dr["LogID"]);
                    ev.MachineID = Convert.ToString(dr["MachineID"]);
                    ev.Message = Convert.ToString(dr["Message"]);
                    ev.Reported = SQLLib.GetDTUTC(dr["Reported"]);
                    ev.Source = Convert.ToString(dr["Source"]);
                    ev.TimeGenerated = SQLLib.GetDTUTC(dr["TimeGenerated"]);
                    ev.TimeWritten = SQLLib.GetDTUTC(dr["TimeWritten"]);
                    EventLogs.Data.Add(ev);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        delegate void DReportingThread(object EventDataListO);

        void ReportingThread(object EventDataListO)
        {
            SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for EventLog Data");
            if (sql == null)
            {
                FoxEventLog.WriteEventLog("Cannot connect to SQL Server for Event Log Data Reporting!", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            try
            {
                List<EventLogReportFull> EventDataList = (List<EventLogReportFull>)EventDataListO;
                if (EventDataList.Count == 0)
                    return;
                List<PolicyObject> Pol = Policies.GetPolicyForComputerInternal(sql, EventDataList[0].MachineID);
                Dictionary<string, Int64> AlreadyReported = new Dictionary<string, long>();
                foreach (PolicyObject PolO in Pol)
                {
                    if (PolO.Type != PolicyIDs.ReportingPolicy)
                        continue;
                    ReportingPolicyElement RepElementRoot = JsonConvert.DeserializeObject<ReportingPolicyElement>(Policies.GetPolicy(sql, PolO.ID).Data);
                    if (RepElementRoot.Type != ReportingPolicyType.EventLog)
                        continue;
                    if (RepElementRoot.ReportToAdmin == null)
                        RepElementRoot.ReportToAdmin = false;
                    if (RepElementRoot.ReportToClient == null)
                        RepElementRoot.ReportToClient = false;
                    if (RepElementRoot.UrgentForAdmin == null)
                        RepElementRoot.UrgentForAdmin = false;
                    if (RepElementRoot.UrgentForClient == null)
                        RepElementRoot.UrgentForClient = false;
                    if (RepElementRoot.ReportToAdmin == false && RepElementRoot.ReportToClient == false && RepElementRoot.UrgentForAdmin == false && RepElementRoot.UrgentForClient == false)
                        continue;

                    foreach (string Element in RepElementRoot.ReportingElements)
                    {
                        ReportingPolicyElementEventLog evrep = JsonConvert.DeserializeObject<ReportingPolicyElementEventLog>(Element);
                        if (evrep.Book == null)
                            evrep.Book = new List<string>();
                        if (evrep.CategoryNumbers == null)
                            evrep.CategoryNumbers = new List<int>();
                        if (evrep.EventLogTypes == null)
                            evrep.EventLogTypes = new List<int>();
                        if (evrep.Sources == null)
                            evrep.Sources = new List<string>();
                        if (evrep.Book.Count == 0 && evrep.CategoryNumbers.Count == 0 && evrep.EventLogTypes.Count == 0 && evrep.Sources.Count == 0)
                            continue;
                        foreach (EventLogReportFull EV in EventDataList)
                        {
                            if (evrep.Book.Count != 0)
                            {
                                bool Match = false;
                                foreach (string Book in evrep.Book)
                                {
                                    if (Book.ToLower() == EV.EventLog.ToLower())
                                    {
                                        Match = true;
                                        break;
                                    }
                                }
                                if (Match == false)
                                    continue;
                            }
                            if (evrep.Sources.Count != 0)
                            {
                                bool Match = false;
                                foreach (string Source in evrep.Sources)
                                {
                                    if (Source.ToLower() == EV.Source.ToLower())
                                    {
                                        Match = true;
                                        break;
                                    }
                                }
                                if (Match == false)
                                    continue;
                            }
                            if (evrep.EventLogTypes.Count != 0)
                            {
                                bool Match = false;
                                foreach (int EVLType in evrep.EventLogTypes)
                                {
                                    if (EVLType == EV.EventLogType)
                                    {
                                        Match = true;
                                        break;
                                    }
                                }
                                if (Match == false)
                                    continue;
                            }
                            if (evrep.CategoryNumbers.Count != 0)
                            {
                                bool Match = false;
                                foreach (int Cat in evrep.CategoryNumbers)
                                {
                                    if (Cat == (EV.InstanceID & 0x3FFFFFFF))
                                    {
                                        Match = true;
                                        break;
                                    }
                                }
                                if (Match == false)
                                    continue;
                            }

                            bool ReportToAdmin = RepElementRoot.ReportToAdmin.Value;
                            bool ReportToClient = RepElementRoot.ReportToClient.Value;
                            bool UrgentForAdmin = RepElementRoot.UrgentForAdmin.Value;
                            bool UrgentForClient = RepElementRoot.UrgentForClient.Value;

                            if (AlreadyReported.ContainsKey(EV.LogID) == true)
                            {
                                if ((AlreadyReported[EV.LogID] & (Int64)ReportingFlags.ReportToAdmin) != 0)
                                    ReportToAdmin = false;
                                if ((AlreadyReported[EV.LogID] & (Int64)ReportingFlags.ReportToClient) != 0)
                                    ReportToClient = false;
                                if ((AlreadyReported[EV.LogID] & (Int64)ReportingFlags.UrgentForAdmin) != 0)
                                    UrgentForAdmin = false;
                                if ((AlreadyReported[EV.LogID] & (Int64)ReportingFlags.UrgentForClient) != 0)
                                    UrgentForClient = false;
                            }

                            if (ReportToAdmin == false && ReportToClient == false && UrgentForAdmin == false && UrgentForClient == false)
                                continue;
                            ReportingFlags Flags = (ReportToAdmin == true ? ReportingFlags.ReportToAdmin : 0) |
                                (ReportToClient == true ? ReportingFlags.ReportToClient : 0) |
                                (UrgentForAdmin == true ? ReportingFlags.UrgentForAdmin : 0) |
                                (UrgentForClient == true ? ReportingFlags.UrgentForClient : 0);
                            switch ((EventLogEntryType)EV.EventLogType)
                            {
                                case 0:
                                case EventLogEntryType.Information:
                                    Flags = (ReportingFlags)((Int64)Flags | ((Int64)ReportingStatusPictureEnum.Info << (int)ReportingFlags.IconFlagsShift));
                                    break;
                                case EventLogEntryType.Warning:
                                    Flags = (ReportingFlags)((Int64)Flags | ((Int64)ReportingStatusPictureEnum.Exclamation << (int)ReportingFlags.IconFlagsShift));
                                    break;
                                case EventLogEntryType.Error:
                                    Flags = (ReportingFlags)((Int64)Flags | ((Int64)ReportingStatusPictureEnum.Stop << (int)ReportingFlags.IconFlagsShift));
                                    break;
                                case EventLogEntryType.SuccessAudit:
                                    Flags = (ReportingFlags)((Int64)Flags | ((Int64)ReportingStatusPictureEnum.Key << (int)ReportingFlags.IconFlagsShift));
                                    break;
                                case EventLogEntryType.FailureAudit:
                                    Flags = (ReportingFlags)((Int64)Flags | ((Int64)ReportingStatusPictureEnum.NoKey << (int)ReportingFlags.IconFlagsShift));
                                    break;
                            }
                            ReportingProcessor.ReportEventLog(sql, EV.MachineID, EV, Flags);
                            if (AlreadyReported.ContainsKey(EV.LogID) == true)
                            {
                                AlreadyReported[EV.LogID] |= (Int64)Flags;
                            }
                            else
                            {
                                AlreadyReported.Add(EV.LogID, (Int64)Flags);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("SEH in Event Data Reporting " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                try
                {
                    sql.CloseConnection();
                }
                catch
                {

                }
            }
        }


        [ReportingExplainAttr(ReportingPolicyType.EventLog)]
        public class EventLogDataReporting : IReportingExplain
        {
            public string Explain(string JSON)
            {
                if (JSON == null)
                    return ("Event Log Data: no data");

                try
                {
                    EventLogReportFull rd = JsonConvert.DeserializeObject<EventLogReportFull>(JSON);
                    string res = "S: " + rd.Source + " DT: " + rd.TimeGenerated.ToLongDateString() + " " + rd.TimeGenerated.ToLongTimeString() + "\r\n";
                    res += "E: " + rd.EventLog + " EV ID: " + (rd.InstanceID & 0x3FFFFFFF).ToString() + "\r\n";
                    res += rd.Message;
                    return (res);
                }
                catch
                {
                    return ("Event Log Data faulty: " + JSON);
                }
            }

            public Image GetIcon()
            {
                return (Resources.EventLog.ToBitmap());
            }
        }
    }
}
