using FoxSDC_Common;
using FoxSDC_Server.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class SimpleTasks
    {
        [VulpesRESTfulRet("SimpleList")]
        SimpleTaskLiteList SimpleList;

        [VulpesRESTfulRet("SimpleTask")]
        SimpleTask SimpleTask;

        [VulpesRESTfulRet("NewTaskID")]
        public NetInt64 NewTaskID;

        [VulpesRESTfulRet("STaskDataSigned")]
        SimpleTaskDataSigned STaskDataSigned;

        delegate void DReportingThread(object SimpleTaskResult);

        public static bool STaskExsits(SQLLib sql, Int64 ID)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM SimpleTasks WHERE ID=@id",
                    new SQLParam("@id", ID))) == 0)
                return (false);
            return (true);
        }

        public static bool STaskExsits(SQLLib sql, Int64 ID, string MID)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM SimpleTasks WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@id", ID),
                    new SQLParam("@mid", MID))) == 0)
                return (false);
            return (true);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/liststasks", "SimpleList", "id")]
        public RESTStatus ListSTasks(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr;
                if (string.IsNullOrWhiteSpace(id) == false)
                {
                    if (Computers.MachineExists(sql, id) == false)
                    {
                        ni.Error = "Access denied";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.NotFound);
                    }

                    dr = sql.ExecSQLReader("select SimpleTasks.*,ComputerName from SimpleTasks inner join ComputerAccounts on ComputerAccounts.MachineID=SimpleTasks.MachineID where SimpleTasks.MachineID=@m order by MachineID,Name",
                            new SQLParam("@m", id));
                }
                else
                {
                    dr = sql.ExecSQLReader("select SimpleTasks.*,ComputerName from SimpleTasks inner join ComputerAccounts on ComputerAccounts.MachineID=SimpleTasks.MachineID order by MachineID,Name");
                }

                SimpleList = new SimpleTaskLiteList();
                SimpleList.List = new List<SimpleTaskLite>();

                while (dr.Read())
                {
                    SimpleTaskLite d = new SimpleTaskLite();
                    sql.LoadIntoClass(dr, d);
                    SimpleList.List.Add(d);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/getstask", "SimpleTask", "id")]
        public RESTStatus GetSTask(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("select * from SimpleTasks WHERE ID=@id", new SQLParam("@id", id));
                if (dr.HasRows == false)
                {
                    dr.Close();
                    return (RESTStatus.NotFound);
                }

                SimpleTask = new SimpleTask();

                while (dr.Read())
                {
                    sql.LoadIntoClass(dr, SimpleTask);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/setstask", "NewTaskID", "")]
        public RESTStatus SetSTasks(SQLLib sql, SimpleTask NewTask, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (NewTask == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.NotFound);
            }

            if (string.IsNullOrWhiteSpace(NewTask.Name) == true || string.IsNullOrWhiteSpace(NewTask.Data) == true || NewTask.Type < 1)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.NotFound);
            }

            lock (ni.sqllock)
            {
                if (Computers.MachineExists(sql, NewTask.MachineID) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }

                Int64? ID = sql.InsertMultiDataID("SimpleTasks",
                   new SQLData("MachineID", NewTask.MachineID),
                   new SQLData("Type", NewTask.Type),
                   new SQLData("Name", NewTask.Name),
                   new SQLData("Data", NewTask.Data));

                if (ID == null)
                {
                    ni.Error = "Server error";
                    ni.ErrorID = ErrorFlags.SQLError;
                    return (RESTStatus.ServerError);
                }

                NewTaskID = new NetInt64();
                NewTaskID.Data = ID.Value;
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.DELETE, "api/mgmt/setstask", "", "id")]
        public RESTStatus DeleteSTask(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (STaskExsits(sql, id) == false)
                {
                    ni.Error = "Invalid ID";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }

                sql.ExecSQL("DELETE FROM SimpleTasks WHERE ID=@id", new SQLParam("@id", id));
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/stasksigned", "STaskDataSigned", "")]
        public RESTStatus GetSTaskSigned(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            SimpleTask SimpleTask;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("select top 1 * from SimpleTasks WHERE MachineID=@id ORDER BY ID asc", new SQLParam("@id", ni.Username));
                if (dr.HasRows == false)
                {
                    dr.Close();
                    return (RESTStatus.NoContent);
                }

                SimpleTask = new SimpleTask();

                while (dr.Read())
                {
                    sql.LoadIntoClass(dr, SimpleTask);
                }
                dr.Close();
            }

            STaskDataSigned = new SimpleTaskDataSigned();
            STaskDataSigned.STask = SimpleTask;
            if (Certificates.Sign(STaskDataSigned, SettingsManager.Settings.UseCertificate) == false)
            {
                FoxEventLog.WriteEventLog("Cannot sign STask Data with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                ni.Error = "Cannot sign STask Data with Certificate " + SettingsManager.Settings.UseCertificate;
                ni.ErrorID = ErrorFlags.CannotSign;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/agent/staskputaside", "NewTaskID", "")]
        public RESTStatus PutTaskAside(SQLLib sql, NetInt64 id, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (STaskExsits(sql, id.Data, ni.Username) == false)
            {
                ni.Error = "Invalid ID";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.NotFound);
            }

            object res = sql.ExecSQLScalar(@"DECLARE @tabl table(ID bigint); 
                insert into SimpleTasks (MachineID,Type,Name,Data) OUTPUT Inserted.ID INTO @tabl 
                Select MachineID,Type,Name,Data from SimpleTasks WHERE ID=@id
                DELETE FROM SimpleTasks where ID=@id
                SELECT * FROM @tabl",
                new SQLParam("@id", id.Data));

            if (res == null || res is DBNull)
            {
                ni.Error = "SQL error";
                ni.ErrorID = ErrorFlags.SQLError;
                return (RESTStatus.NotFound);
            }

            NewTaskID = new NetInt64();
            try
            {
                NewTaskID.Data = Convert.ToInt64(res);
            }
            catch
            {
                ni.Error = "other SQL error";
                ni.ErrorID = ErrorFlags.SQLError;
                return (RESTStatus.NotFound);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/agent/staskcompleted", "", "")]
        public RESTStatus CompleteSTask(SQLLib sql, SimpleTaskResult STaskResult, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (STaskResult == null)
            {
                ni.Error = "Invalid ID";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.NotFound);
            }

            lock (ni.sqllock)
            {
                STaskResult.MachineID = ni.Username;
                if (STaskExsits(sql, STaskResult.ID, STaskResult.MachineID) == false)
                {
                    ni.Error = "Invalid ID";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                STaskResult.Name = Convert.ToString(sql.ExecSQLScalar("SELECT Name FROM SimpleTasks WHERE ID=@id",
                    new SQLParam("@id", STaskResult.ID)));
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("DELETE FROM SimpleTasks WHERE ID=@id",
                    new SQLParam("@id", STaskResult.ID));
            }

            Thread t = new Thread(new ParameterizedThreadStart(new DReportingThread(ReportingThread)));
            t.Start(STaskResult);

            return (RESTStatus.Success);
        }

        void ReportingThread(object SimpleTaskO)
        {
            SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for SimpleTask Result Data");
            if (sql == null)
            {
                FoxEventLog.WriteEventLog("Cannot connect to SQL Server for SimpleTask Result Reporting!", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            try
            {
                SimpleTaskResult SimpleTask = (SimpleTaskResult)SimpleTaskO;

                List<PolicyObject> Pol = Policies.GetPolicyForComputerInternal(sql, SimpleTask.MachineID);
                Dictionary<string, Int64> AlreadyReported = new Dictionary<string, long>();
                foreach (PolicyObject PolO in Pol)
                {
                    if (PolO.Type != PolicyIDs.ReportingPolicy)
                        continue;
                    ReportingPolicyElement RepElementRoot = JsonConvert.DeserializeObject<ReportingPolicyElement>(Policies.GetPolicy(sql, PolO.ID).Data);
                    if (RepElementRoot.Type != ReportingPolicyType.SimpleTaskCompleted)
                        continue;
                    if (RepElementRoot.ReportToAdmin == null)
                        RepElementRoot.ReportToAdmin = false;
                    if (RepElementRoot.ReportToClient == null)
                        RepElementRoot.ReportToClient = false;
                    if (RepElementRoot.UrgentForAdmin == null)
                        RepElementRoot.UrgentForAdmin = false;
                    if (RepElementRoot.UrgentForClient == null)
                        RepElementRoot.UrgentForClient = false;
                    if (RepElementRoot.ReportToAdmin == false && RepElementRoot.ReportToClient == false && RepElementRoot.UrgentForAdmin == false &&
                        RepElementRoot.UrgentForClient == false)
                        continue;

                    foreach (string Element in RepElementRoot.ReportingElements)
                    {
                        ReportingPolicyElementSimpleTaskCompleted arprep = JsonConvert.DeserializeObject<ReportingPolicyElementSimpleTaskCompleted>(Element);

                        ReportThings(sql, SimpleTask.MachineID, "Completed", SimpleTask, ref AlreadyReported, RepElementRoot);
                    }
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("SEH in SimpleTask Result Reporting " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
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

        void ReportThings(SQLLib sql, string MachineID, string Method, SimpleTaskResult ar, ref Dictionary<string, Int64> AlreadyReported, ReportingPolicyElement RepElementRoot)
        {
            string ID = MachineID + "\\" + Method;

            bool ReportToAdmin = RepElementRoot.ReportToAdmin.Value;
            bool ReportToClient = RepElementRoot.ReportToClient.Value;
            bool UrgentForAdmin = RepElementRoot.UrgentForAdmin.Value;
            bool UrgentForClient = RepElementRoot.UrgentForClient.Value;

            if (AlreadyReported.ContainsKey(ID) == true)
            {
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.ReportToAdmin) != 0)
                    ReportToAdmin = false;
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.ReportToClient) != 0)
                    ReportToClient = false;
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.UrgentForAdmin) != 0)
                    UrgentForAdmin = false;
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.UrgentForClient) != 0)
                    UrgentForClient = false;
            }

            if (ReportToAdmin == false && ReportToClient == false && UrgentForAdmin == false && UrgentForClient == false)
                return;
            ReportingFlags Flags = (ReportToAdmin == true ? ReportingFlags.ReportToAdmin : 0) |
                (ReportToClient == true ? ReportingFlags.ReportToClient : 0) |
                (UrgentForAdmin == true ? ReportingFlags.UrgentForAdmin : 0) |
                (UrgentForClient == true ? ReportingFlags.UrgentForClient : 0);

            ReportingProcessor.ReportSimpleTaskCompletion(sql, MachineID, Method, ar, Flags);

            if (AlreadyReported.ContainsKey(ID) == true)
            {
                AlreadyReported[ID] |= (Int64)Flags;
            }
            else
            {
                AlreadyReported.Add(ID, (Int64)Flags);
            }
        }

        [ReportingExplainAttr(ReportingPolicyType.SimpleTaskCompleted)]
        public class SimpleTaskReporting : IReportingExplain
        {
            public string Explain(string JSON)
            {
                if (JSON == null)
                    return ("SimpleTask: no data");

                try
                {
                    ReportingSimpleTaskCompletion rd = JsonConvert.DeserializeObject<ReportingSimpleTaskCompletion>(JSON);
                    if (rd.App == null)
                        return ("SimpleTask: no data");

                    string res = "Action: " + rd.Action + "\r\n";
                    res += "Name: " + rd.App.Name + "\r\n";
                    res += "Result: 0x" + rd.App.Result.ToString("X") + "\r\n";
                    if (string.IsNullOrWhiteSpace(rd.App.Text) == false)
                        res += "Text: " + rd.App.Text + "\r\n";

                    return (res);
                }
                catch
                {
                    return ("SimpleTask Data faulty: " + JSON);
                }
            }

            public Image GetIcon()
            {
                return (Resources.ST.ToBitmap());
            }
        }
    }
}
