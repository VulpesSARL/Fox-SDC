using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class Computers
    {
        [VulpesRESTfulRet("UnapprovedList")]
        public ComputerDataList UnapprovedList;
        [VulpesRESTfulRet("ComputerDataInfo")]
        public ComputerData ComputerDataInfo;

        public static bool MachineExists(SQLLib sql, string MachineID)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", MachineID))) == 0)
                return (false);
            return (true);
        }

        static void PutComputerData(SqlDataReader dr, ref ComputerData cd)
        {
            cd.Comments = Convert.ToString(dr["Comments"]);
            cd.GroupingPath = Convert.ToString(dr["Path"]);
            cd.Computername = Convert.ToString(dr["ComputerName"]);
            cd.Approved = Convert.ToBoolean(dr["Accepted"]);
            cd.MachineID = Convert.ToString(dr["MachineID"]);
            cd.UCID = Convert.ToString(dr["UCID"]);
            cd.Make = Convert.ToString(dr["ComputerModel"]);
            cd.OS = Convert.ToString(dr["OSName"]);
            cd.OSVersion = Convert.ToString(dr["OSVerMaj"]) + "." + Convert.ToString(dr["OSVerMin"]) + "." + Convert.ToString(dr["OSVerBuild"]);
            cd.OSVerType = Convert.ToInt32(dr["OSVerType"]);
            cd.OSSuite = Convert.ToString(dr["OSSUite"]);
            cd.LastUpdated = SQLLib.GetDTUTC(dr["LastUpdated"]);
            cd.CPU = Convert.ToString(dr["CPU"]);
            cd.Language = Convert.ToString(dr["Language"]) + " (" + Convert.ToString(dr["DisplayLanguage"]) + ")";
            cd.Is64Bit = Convert.ToBoolean(dr["Is64Bit"]);
            cd.IsTSE = Convert.ToBoolean(dr["IsTSE"]);
            cd.AgentVersion = Convert.ToString(dr["AgentVersion"]);
            cd.AgentVersionID = Convert.ToInt64(dr["AgentVersionID"]);
            cd.RunningInHypervisor = Convert.ToBoolean(dr["RunningInHypervisor"]);
            cd.BIOS = Convert.ToString(dr["BIOS"]);
            cd.IPAddress = Convert.ToString(dr["IPAddress"]);
            cd.BIOSType = Convert.ToString(dr["BIOSType"]);
            cd.NumberOfLogicalProcessors = dr["NumberOfLogicalProcessors"] is DBNull ? 0 : Convert.ToInt32(dr["NumberOfLogicalProcessors"]);
            cd.NumberOfProcessors = dr["NumberOfProcessors"] is DBNull ? 0 : Convert.ToInt32(dr["NumberOfProcessors"]);
            cd.TotalPhysicalMemory = dr["TotalPhysicalMemory"] is DBNull ? 0L : Convert.ToInt64(dr["TotalPhysicalMemory"]);
            cd.CPUName = Convert.ToString(dr["CPUName"]);
            cd.SecureBootState = Convert.ToString(dr["SecureBootState"]);
            cd.SystemRoot = Convert.ToString(dr["SystemRoot"]);
            cd.SUSID = Convert.ToString(dr["SUSID"]);
            cd.RunningInWindowsPE = dr["RunningInWindowsPE"] is DBNull ? (bool?)null : Convert.ToBoolean(dr["RunningInWindowsPE"]);
            cd.IsMeteredConnection = dr["MeteredConnection"] is DBNull ? (bool?)null : Convert.ToBoolean(dr["MeteredConnection"]);

            if (Settings.Default.UseContract == true)
                cd.ContractID = Convert.ToString(dr["ContractID"]);
            else
                cd.ContractID = "";
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/computers", "UnapprovedList", "", WithQueryString: true)]
        public RESTStatus GetComputerList(SQLLib sql, object dummy, NetworkConnectionInfo ni, NameValueCollection QueryString)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            UnapprovedList = new ComputerDataList();
            UnapprovedList.List = new List<ComputerData>();
            string where = "";
            string group = "";

            bool? Approved = QueryString["Approved"] == null ? (bool?)null : (QueryString["Approved"] == "1" ? true : false);
            Int64? Group = null;
            if (QueryString["Group"] != null)
            {
                Int64 gi;
                if (Int64.TryParse(QueryString["Group"], out gi) == true)
                    Group = gi;
            }

            if (Approved == null)
                where = "where 1=1";
            if (Approved == true)
                where = "where Accepted=1";
            if (Approved == false)
                where = "where Accepted=0";

            if (Group == null)
            {
                group = " AND 1=1";
            }
            else
            {
                group = " AND Grouping=@group";
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader(@"
                WITH GroupingRecursive(ID, Name, ParentID, LEVEL, Path) AS
                (SELECT ID, name, ParentID, 0 AS LEVEL,
                CAST(name AS nvarchar(max)) AS treepath
                FROM Grouping
                WHERE ParentID IS NULL
                UNION ALL
                SELECT d.ID, d.Name, d.ParentID,
                GroupingRecursive.LEVEL + 1 AS LEVEL,
                CAST(GroupingRecursive.Path + '\' +
                CAST(d.Name AS nvarchar(max)) AS NVARCHAR(max)) AS treepath
                FROM Grouping as d
                INNER JOIN GroupingRecursive
                ON GroupingRecursive.ID = d.ParentID)
                select *,(SELECT Path FROM GroupingRecursive WHERE ID=ComputerAccounts.Grouping) as Path from ComputerAccounts " + where + group + " order by ComputerName",
                new SQLParam("@group", Group));
                while (dr.Read())
                {
                    ComputerData cd = new ComputerData();
                    PutComputerData(dr, ref cd);
                    UnapprovedList.List.Add(cd);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/computers", "ComputerDataInfo", "id")]
        public RESTStatus GetComputerDetail(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (MachineExists(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader(@"
                WITH GroupingRecursive(ID, Name, ParentID, LEVEL, Path) AS
                (SELECT ID, name, ParentID, 0 AS LEVEL,
                CAST(name AS nvarchar(max)) AS treepath
                FROM Grouping
                WHERE ParentID IS NULL
                UNION ALL
                SELECT d.ID, d.Name, d.ParentID,
                GroupingRecursive.LEVEL + 1 AS LEVEL,
                CAST(GroupingRecursive.Path + '\' +
                CAST(d.Name AS nvarchar(max)) AS NVARCHAR(max)) AS treepath
                FROM Grouping as d
                INNER JOIN GroupingRecursive
                ON GroupingRecursive.ID = d.ParentID)
                select *,(SELECT Path FROM GroupingRecursive WHERE ID=ComputerAccounts.Grouping) as Path from ComputerAccounts WHERE MachineID=@id order by ComputerName",
                new SQLParam("@id", id));
                ComputerData cd = null;
                while (dr.Read())
                {
                    cd = new ComputerData();
                    PutComputerData(dr, ref cd);
                }
                dr.Close();
                ComputerDataInfo = cd;
            }

            return (RESTStatus.Success);
        }

        public static ComputerData GetComputerDetail(SQLLib sql, string MachineID)
        {
            if (MachineExists(sql, MachineID) == false)
                return (null);

            SqlDataReader dr = sql.ExecSQLReader(@"
                WITH GroupingRecursive(ID, Name, ParentID, LEVEL, Path) AS
                (SELECT ID, name, ParentID, 0 AS LEVEL,
                CAST(name AS nvarchar(max)) AS treepath
                FROM Grouping
                WHERE ParentID IS NULL
                UNION ALL
                SELECT d.ID, d.Name, d.ParentID,
                GroupingRecursive.LEVEL + 1 AS LEVEL,
                CAST(GroupingRecursive.Path + '\' +
                CAST(d.Name AS nvarchar(max)) AS NVARCHAR(max)) AS treepath
                FROM Grouping as d
                INNER JOIN GroupingRecursive
                ON GroupingRecursive.ID = d.ParentID)
                select *,(SELECT Path FROM GroupingRecursive WHERE ID=ComputerAccounts.Grouping) as Path from ComputerAccounts WHERE MachineID=@id order by ComputerName",
                    new SQLParam("@id", MachineID));
            ComputerData cd = null;
            while (dr.Read())
            {
                cd = new ComputerData();
                PutComputerData(dr, ref cd);
            }
            dr.Close();

            return (cd);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.PATCH, "api/mgmt/computercomment", "", "id")]
        public RESTStatus ChangeCommentsComputer(SQLLib sql, NetString comments, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m", new SQLParam("@m", id))) == 0)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            if (comments.Data == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE ComputerAccounts SET Comments=@c WHERE MachineID=@m",
                    new SQLParam("@m", id),
                    new SQLParam("@c", comments.Data));
            }

            return (RESTStatus.NoContent);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.PATCH, "api/mgmt/computers", "", "id")]
        public RESTStatus ApproveComputer(SQLLib sql, ApproveComputer state, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m", new SQLParam("@m", id))) == 0)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Grouping WHERE ID=@id", new SQLParam("@id", state.Group))) == 0)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE ComputerAccounts SET Accepted=@a,Grouping=@g WHERE MachineID=@m",
                    new SQLParam("@m", id),
                    new SQLParam("@g", state.Group),
                    new SQLParam("@a", state.State == true ? 1 : 0));
            }
            return (RESTStatus.NoContent);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.DELETE, "api/mgmt/computers", "", "id")]
        public RESTStatus RemoveComputer(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m", new SQLParam("@m", id))) == 0)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQLScalar("UPDATE ComputerAccounts SET Accepted=0 WHERE MachineID=@m", new SQLParam("@m", id));
            }

            lock (ni.sqllock)
            {
                //delete other things
                sql.ExecSQLScalar(@"DECLARE @Deleted_Rows INT
                            DECLARE @Deleted_Rows_Total INT
                            SET @Deleted_Rows = 1
                            SET @Deleted_Rows_Total = 0

                            WHILE (@Deleted_Rows > 0)
                            BEGIN
                                delete top (10000) from EventLog where MachineID=@m
                                SET @Deleted_Rows = @@ROWCOUNT
                                SET @Deleted_Rows_Total = @Deleted_Rows_Total + @Deleted_Rows
                            END

                            Select @Deleted_Rows_Total",
                        new SQLParam("@m", id));
            }
            lock (ni.sqllock)
                sql.ExecSQL("UPDATE Policies SET MachineID=null, Enabled=0 WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM AddRemovePrograms WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM BitlockerRK WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM DevicesConfig WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM DevicesFilter WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM DiskData WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM NetworkConfigSuppl WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM NetworkConfig WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM Reporting WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM WindowsLic WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM Chats WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM UsersList WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM SMARTDataAttributes WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM SMARTData WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM SimpleTasks WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM Startups WHERE MachineID=@m", new SQLParam("@m", id));
            lock (ni.sqllock)
                Modules.FileTransfer.DeleteAllFiles(sql, id);
            //finally delete computer
            lock (ni.sqllock)
                sql.ExecSQL("DELETE FROM ComputerAccounts WHERE MachineID=@m", new SQLParam("@m", id));

            return (RESTStatus.NoContent);
        }
    }
}
