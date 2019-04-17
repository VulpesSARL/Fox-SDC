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
    class DiskData
    {
        [VulpesRESTfulRet("LstDiskData")]
        public ListDiskDataReport LstDiskData;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/diskdata", "", "")]
        public RESTStatus ReportDiskData(SQLLib sql, ListDiskDataReport DiskDataList, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            DiskDataList.MachineID = ni.Username;
            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", DiskDataList.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.NotFound);
                }
            }

            if (DiskDataList.Items == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE DiskData SET DevicePresent=0 WHERE MachineID=@m",
                    new SQLParam("@m", DiskDataList.MachineID));
            }

            foreach (DiskDataReport dr in DiskDataList.Items)
            {
                lock (ni.sqllock)
                {
                    sql.ExecSQL("DELETE FROM DiskData WHERE MachineID=@m AND DeviceID=@d",
                        new SQLParam("@d", dr.DeviceID),
                        new SQLParam("@m", DiskDataList.MachineID));
                }
            }

            List<List<SQLData>> bulkdata = new List<List<SQLData>>();
            DateTime DT = DateTime.Now;

            DiskDataReport cdr = null;

            lock (ni.sqllock)
            {
                try
                {
                    sql.BeginTransaction();
                    sql.SEHError = true;

                    foreach (DiskDataReport dr in DiskDataList.Items)
                    {
                        sql.InsertMultiData("DiskData",
                            new SQLData("MachineID", dr.MachineID),
                            new SQLData("DevicePresent", true),
                            new SQLData("LastUpdated", DT),
                            new SQLData("DeviceID", dr.DeviceID),
                            new SQLData("Access", dr.Access),
                            new SQLData("Automount", dr.Automount),
                            new SQLData("Availability", dr.Availability),
                            new SQLData("Capacity", dr.Capacity),
                            new SQLData("Caption", dr.Caption),
                            new SQLData("Compressed", dr.Compressed),
                            new SQLData("ConfigManagerErrorCode", dr.ConfigManagerErrorCode),
                            new SQLData("Description", dr.Description),
                            new SQLData("DirtyBitSet", dr.DirtyBitSet),
                            new SQLData("DriveLetter", dr.DriveLetter),
                            new SQLData("DriveType", dr.DriveType),
                            new SQLData("ErrorDescription", dr.ErrorDescription),
                            new SQLData("ErrorMethodology", dr.ErrorMethodology),
                            new SQLData("FileSystem", dr.FileSystem),
                            new SQLData("FreeSpace", dr.FreeSpace),
                            new SQLData("Label", dr.Label),
                            new SQLData("LastErrorCode", dr.LastErrorCode),
                            new SQLData("MaximumFileNameLength", dr.MaximumFileNameLength),
                            new SQLData("Name", dr.Name),
                            new SQLData("PNPDeviceID", dr.PNPDeviceID),
                            new SQLData("SerialNumber", dr.SerialNumber),
                            new SQLData("Status", dr.Status));
                    }
                    sql.CommitTransaction();
                }
                catch (Exception ee)
                {
                    sql.RollBackTransaction();
                    FoxEventLog.WriteEventLog("DB Error: Cannot insert data to DiskData: " + ee.ToString() + "\r\n\r\nJSON: " +
                        JsonConvert.SerializeObject(cdr, Formatting.Indented), System.Diagnostics.EventLogEntryType.Error);
                    return (RESTStatus.ServerError);
                }
                finally
                {
                    sql.SEHError = false;
                }
            }

            Thread t = new Thread(new ParameterizedThreadStart(new DReportingThread(ReportingThread)));
            t.Start(DiskDataList);

            return (RESTStatus.Success);
        }

        delegate void DReportingThread(object DiskDataListO);

        void ReportingThread(object DiskDataListO)
        {
            SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for DiskData");
            if (sql == null)
            {
                FoxEventLog.WriteEventLog("Cannot connect to SQL Server for Disk Data Reporting!", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            try
            {
                ListDiskDataReport DiskDataList = (ListDiskDataReport)DiskDataListO;
                List<PolicyObject> Pol = Policies.GetPolicyForComputerInternal(sql, DiskDataList.MachineID);

                Dictionary<string, Int64> AlreadyReported = new Dictionary<string, long>();
                foreach (PolicyObject PolO in Pol)
                {
                    if (PolO.Type != PolicyIDs.ReportingPolicy)
                        continue;
                    ReportingPolicyElement RepElementRoot = JsonConvert.DeserializeObject<ReportingPolicyElement>(Policies.GetPolicy(sql, PolO.ID).Data);
                    if (RepElementRoot.Type != ReportingPolicyType.Disk)
                        continue;

                    foreach (string Element in RepElementRoot.ReportingElements)
                    {
                        ReportingPolicyElementDisk diskrep = JsonConvert.DeserializeObject<ReportingPolicyElementDisk>(Element);
                        if (diskrep.DriveLetter == null)
                            continue;
                        if (diskrep.DriveLetter.Length != 1)
                            continue;

                        foreach (DiskDataReport DD in DiskDataList.Items)
                        {
                            string Drive = diskrep.DriveLetter;

                            if (diskrep.DriveLetter == "$")
                            {
                                ComputerData d = Computers.GetComputerDetail(sql, DiskDataList.MachineID);
                                if (d != null)
                                {
                                    if (string.IsNullOrWhiteSpace(d.SystemRoot) == false)
                                    {
                                        Drive = d.SystemRoot.Substring(0, 1);
                                    }
                                }
                            }

                            if (string.IsNullOrWhiteSpace(DD.DriveLetter) == true)
                                continue;
                            if (DD.DriveLetter.ToLower().Substring(0, 1) != Drive.ToLower())
                                continue;

                            Int64 SZLimit;

                            if (diskrep.Method == 1)
                            {
                                SZLimit = (Int64)((100m / (decimal)DD.Capacity) * (decimal)diskrep.MinimumSize);
                            }
                            else
                            {
                                SZLimit = diskrep.MinimumSize;
                            }

                            if (DD.FreeSpace < SZLimit)
                            {
                                bool ReportToAdmin = RepElementRoot.ReportToAdmin.Value;
                                bool ReportToClient = RepElementRoot.ReportToClient.Value;
                                bool UrgentForAdmin = RepElementRoot.UrgentForAdmin.Value;
                                bool UrgentForClient = RepElementRoot.UrgentForClient.Value;

                                if (AlreadyReported.ContainsKey(DD.DriveLetter) == true)
                                {
                                    if ((AlreadyReported[DD.DriveLetter] & (Int64)ReportingFlags.ReportToAdmin) != 0)
                                        ReportToAdmin = false;
                                    if ((AlreadyReported[DD.DriveLetter] & (Int64)ReportingFlags.ReportToClient) != 0)
                                        ReportToClient = false;
                                    if ((AlreadyReported[DD.DriveLetter] & (Int64)ReportingFlags.UrgentForAdmin) != 0)
                                        UrgentForAdmin = false;
                                    if ((AlreadyReported[DD.DriveLetter] & (Int64)ReportingFlags.UrgentForClient) != 0)
                                        UrgentForClient = false;
                                }

                                if (ReportToAdmin == false && ReportToClient == false && UrgentForAdmin == false && UrgentForClient == false)
                                    continue;
                                ReportingFlags Flags = (ReportToAdmin == true ? ReportingFlags.ReportToAdmin : 0) |
                                    (ReportToClient == true ? ReportingFlags.ReportToClient : 0) |
                                    (UrgentForAdmin == true ? ReportingFlags.UrgentForAdmin : 0) |
                                    (UrgentForClient == true ? ReportingFlags.UrgentForClient : 0);
                                ReportingProcessor.ReportDiskData(sql, DiskDataList.MachineID, DD.DriveLetter, SZLimit, DD.FreeSpace, DD.Capacity, Flags);
                                if (AlreadyReported.ContainsKey(DD.DriveLetter) == true)
                                {
                                    AlreadyReported[DD.DriveLetter] |= (Int64)Flags;
                                }
                                else
                                {
                                    AlreadyReported.Add(DD.DriveLetter, (Int64)Flags);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("SEH in Disk Data Reporting " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
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

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/diskdata", "LstDiskData", "id")]
        public RESTStatus ListDiskData(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
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

                    dr = sql.ExecSQLReader("select DiskData.*,ComputerName from DiskData inner join ComputerAccounts on ComputerAccounts.MachineID=DiskData.MachineID where diskData.MachineID=@m order by MachineID,Caption",
                            new SQLParam("@m", id));
                }
                else
                {
                    dr = sql.ExecSQLReader("select DiskData.*,ComputerName from DiskData inner join ComputerAccounts on ComputerAccounts.MachineID=DiskData.MachineID order by MachineID,Caption");
                }

                LstDiskData = new ListDiskDataReport();
                LstDiskData.Items = new List<DiskDataReport>();

                while (dr.Read())
                {
                    DiskDataReport d = new DiskDataReport();
                    sql.LoadIntoClass(dr, d);
                    LstDiskData.Items.Add(d);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [ReportingExplainAttr(ReportingPolicyType.Disk)]
        public class DiskDataReporting : IReportingExplain
        {
            public string Explain(string JSON)
            {
                if (JSON == null)
                    return ("Disk Data: no data");

                try
                {
                    ReportingDiskData rd = JsonConvert.DeserializeObject<ReportingDiskData>(JSON);
                    return (rd.Disk + ": Free space: " + CommonUtilities.NiceSize(rd.Is) + " of total: " + CommonUtilities.NiceSize(rd.TotalSZ) + " (Notify: " + CommonUtilities.NiceSize(rd.NotifyValue) + ")");
                }
                catch
                {
                    return ("Disk Data faulty: " + JSON);
                }
            }

            public Image GetIcon()
            {
                return (Resources.Disk.ToBitmap());
            }
        }
    }
}
