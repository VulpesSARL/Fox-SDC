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
    class Startups
    {
        [VulpesRESTfulRet("StartupsRep")]
        public ListStartupItemReport StartupsRep;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/startups", "", "")]
        public RESTStatus ReportStatups(SQLLib sql, ListStartupItems StartupList, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            StartupList.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                new SQLParam("@m", StartupList.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Denied);
                }
            }

            if (StartupList.Items == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }
            if (StartupList.SIDUsers == null)
                StartupList.SIDUsers = new List<string>();

            foreach (StartupItem rep in StartupList.Items)
            {
                if (string.IsNullOrWhiteSpace(rep.Location) == true)
                {
                    ni.Error = "Invalid Items";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }
                if (string.IsNullOrWhiteSpace(rep.Key) == true)
                {
                    ni.Error = "Invalid Items";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }

                if (string.IsNullOrWhiteSpace(rep.HKCUUser) == true)
                    rep.HKCUUser = "";
                if (string.IsNullOrWhiteSpace(rep.Item) == true)
                    rep.Item = "";
            }

            List<StartupItem> Installed = new List<StartupItem>();
            List<StartupItem> Reported = StartupList.Items;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM Startups WHERE MachineID=@id", new SQLParam("@id", StartupList.MachineID));
                while (dr.Read())
                {
                    StartupItem ar = new StartupItem();
                    ar.Location = Convert.ToString(dr["Location"]);
                    ar.Key = Convert.ToString(dr["Key"]);
                    ar.Item = Convert.ToString(dr["Item"]);
                    ar.HKCUUser = Convert.ToString(dr["HKCUUser"]);
                    if (string.IsNullOrWhiteSpace(ar.HKCUUser) == true)
                        ar.HKCUUser = "";

                    Installed.Add(ar);
                }
                dr.Close();
            }

            List<StartupItem> Updated = new List<StartupItem>();
            List<StartupItem> Unchanged = new List<StartupItem>();
            List<StartupItem> Removed = new List<StartupItem>();
            List<StartupItem> Added = new List<StartupItem>();

            foreach (StartupItem inst in Installed)
            {
                inst.Location = inst.Location.Trim();
                inst.Key = inst.Key.Trim();
                inst.Item = inst.Item.Trim();
                inst.HKCUUser = inst.HKCUUser.Trim();

                bool Found = false;
                foreach (StartupItem rep in Reported)
                {
                    rep.Location = rep.Location.Trim();
                    rep.Key = rep.Key.Trim();
                    rep.Item = rep.Item.Trim();
                    rep.HKCUUser = rep.HKCUUser.Trim();

                    if (rep.Location.ToLower() == inst.Location.ToLower() && rep.Key == inst.Key && rep.HKCUUser == inst.HKCUUser)
                    {
                        if (rep.Item.ToLower() == inst.Item.ToLower())
                        {
                            Unchanged.Add(rep);
                            Found = true;
                            break;
                        }
                        else
                        {
                            Updated.Add(rep);
                            Found = true;
                            break;
                        }
                    }
                }
                if (Found == false)
                {
                    if (inst.HKCUUser != "" && StartupList.SIDUsers.Contains(inst.HKCUUser, StringComparer.InvariantCultureIgnoreCase) == false)
                    {
                        //likely that this user is not logged on or such
                        Unchanged.Add(inst);
                    }
                    else
                    {
                        Removed.Add(inst);
                    }
                }
            }

            foreach (StartupItem inst in Reported)
            {
                bool Found = false;
                foreach (StartupItem rep in Installed)
                {
                    if (rep.Key.ToLower() == inst.Key.ToLower() && rep.Location == inst.Location && rep.HKCUUser == inst.HKCUUser)
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found == false)
                    Added.Add(inst);
            }

            lock (ni.sqllock)
            {
                try
                {
                    sql.BeginTransaction();
                    sql.SEHError = true;
                    foreach (StartupItem ar in Removed)
                    {
                        sql.ExecSQL("DELETE FROM Startups WHERE MachineID=@id AND [Key]=@key AND Location=@location AND HKCUUser=@user",
                            new SQLParam("@id", StartupList.MachineID),
                            new SQLParam("@key", ar.Key),
                            new SQLParam("@user", ar.HKCUUser),
                            new SQLParam("@location", ar.Location));
                    }
                    foreach (StartupItem ar in Updated)
                    {
                        sql.ExecSQL(@"UPDATE Startups SET
                            Item=@item,
                            DT=@DT
                            WHERE MachineID=@id AND [Key]=@key AND Location=@location AND HKCUUser=@user",
                            new SQLParam("@id", StartupList.MachineID),
                            new SQLParam("@key", ar.Key),
                            new SQLParam("@user", ar.HKCUUser),
                            new SQLParam("@location", ar.Location),
                            new SQLParam("@item", ar.Item),
                            new SQLParam("@DT", DateTime.UtcNow));
                    }
                    foreach (StartupItem ar in Added)
                    {
                        sql.InsertMultiData("Startups",
                            new SQLData("MachineID", StartupList.MachineID),
                            new SQLData("Item", ar.Item),
                            new SQLData("Location", ar.Location),
                            new SQLData("Key", ar.Key),
                            new SQLData("HKCUUser", ar.HKCUUser),
                            new SQLData("DT", DateTime.UtcNow));
                    }
                    sql.CommitTransaction();
                }
                catch (Exception ee)
                {
                    sql.RollBackTransaction();
                    FoxEventLog.WriteEventLog("DB Error: Cannot update Startups: " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    return (RESTStatus.ServerError);
                }
                finally
                {
                    sql.SEHError = false;
                }
            }

            StartupLst l = new StartupLst();
            l.Added = Added;
            l.Removed = Removed;
            l.Unchanged = Unchanged;
            l.Updated = Updated;
            l.MachineID = StartupList.MachineID;
            Thread t = new Thread(new ParameterizedThreadStart(new DReportingThread(ReportingThread)));
            t.Start(l);

            return (RESTStatus.Created);
        }

        class StartupLst
        {
            public List<StartupItem> Updated;
            public List<StartupItem> Unchanged;
            public List<StartupItem> Removed;
            public List<StartupItem> Added;
            public string MachineID;
        }

        delegate void DReportingThread(object AddRemoveProgramsListO);

        List<StartupItem> GetFilteredData(List<StartupItem> FullList, ComputerData computerdata, ReportingPolicyElementStartup arprep)
        {
            List<StartupItem> Lst = new List<StartupItem>();
            if (arprep.Names == null)
                arprep.Names = new List<string>();
            foreach (StartupItem ar in FullList)
            {
                if (arprep.Names.Count != 0)
                {
                    bool Found = false;
                    foreach (string n in arprep.Names)
                    {
                        switch (arprep.SearchNameIn)
                        {
                            case 0:
                                if (n.ToLower() == ar.Key.ToLower() ||
                                    n.ToLower() == ar.Item.ToLower())
                                    Found = true;
                                break;
                            case 1:
                                if (ar.Key.ToLower().Contains(n.ToLower()) == true ||
                                    ar.Item.ToLower().Contains(n.ToLower()) == true)
                                    Found = true;
                                break;
                            case 2:
                                if (ar.Key.ToLower().StartsWith(n.ToLower()) == true ||
                                    ar.Item.ToLower().StartsWith(n.ToLower()) == true)
                                    Found = true;
                                break;
                        }
                        if (Found == true)
                            break;
                    }
                    if (Found == false)
                        continue;
                }
                if (string.IsNullOrWhiteSpace(arprep.SearchLocations) == false)
                {
                    bool Found = false;
                    foreach (string loc in arprep.SearchLocations.Split(';'))
                    {
                        if (loc.ToLower().Trim() == ar.Location.ToLower())
                        {
                            Found = true;
                            break;
                        }
                    }
                    if (Found == false)
                        continue;
                }
                Lst.Add(ar);
            }
            return (Lst);
        }

        void ReportThings(SQLLib sql, string MachineID, string Method, StartupItem ar, ref Dictionary<string, Int64> AlreadyReported, ReportingPolicyElement RepElementRoot)
        {
            string ID = MachineID + "\\\\" + ar.Key + "\\\\" + ar.Location;
            if (string.IsNullOrWhiteSpace(ar.HKCUUser) == false)
                ID += "\\\\" + ar.HKCUUser;
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

            ReportingProcessor.ReportStartup(sql, MachineID, Method, ar, Flags);

            if (AlreadyReported.ContainsKey(ID) == true)
            {
                AlreadyReported[ID] |= (Int64)Flags;
            }
            else
            {
                AlreadyReported.Add(ID, (Int64)Flags);
            }
        }

        void ReportingThread(object StartupListO)
        {
            try
            {
                using (SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for Startup Data"))
                {
                    if (sql == null)
                    {
                        FoxEventLog.WriteEventLog("Cannot connect to SQL Server for Startup Reporting!", System.Diagnostics.EventLogEntryType.Error);
                        return;
                    }
                    StartupLst StartupList = (StartupLst)StartupListO;
                    ComputerData computerdata = Computers.GetComputerDetail(sql, StartupList.MachineID);
                    if (computerdata == null)
                    {
                        FoxEventLog.WriteEventLog("Cannot get any computer data for Startup Reporting!", System.Diagnostics.EventLogEntryType.Error);
                        return;
                    }

                    List<PolicyObject> Pol = Policies.GetPolicyForComputerInternal(sql, StartupList.MachineID);
                    Dictionary<string, Int64> AlreadyReported = new Dictionary<string, long>();
                    foreach (PolicyObject PolO in Pol)
                    {
                        if (PolO.Type != PolicyIDs.ReportingPolicy)
                            continue;
                        ReportingPolicyElement RepElementRoot = JsonConvert.DeserializeObject<ReportingPolicyElement>(Policies.GetPolicy(sql, PolO.ID).Data);
                        if (RepElementRoot.Type != ReportingPolicyType.Startup)
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
                            ReportingPolicyElementStartup arprep = JsonConvert.DeserializeObject<ReportingPolicyElementStartup>(Element);
                            if (arprep.NotifyOnAdd == false && arprep.NotifyOnRemove == false && arprep.NotifyOnUpdate == false)
                                continue;

                            if (arprep.NotifyOnAdd == true)
                            {
                                foreach (StartupItem ar in GetFilteredData(StartupList.Added, computerdata, arprep))
                                {
                                    ReportThings(sql, StartupList.MachineID, "Add", ar, ref AlreadyReported, RepElementRoot);
                                }
                            }

                            if (arprep.NotifyOnUpdate == true)
                            {
                                foreach (StartupItem ar in GetFilteredData(StartupList.Updated, computerdata, arprep))
                                {
                                    ReportThings(sql, StartupList.MachineID, "Update", ar, ref AlreadyReported, RepElementRoot);
                                }
                            }

                            if (arprep.NotifyOnRemove == true)
                            {
                                foreach (StartupItem ar in GetFilteredData(StartupList.Removed, computerdata, arprep))
                                {
                                    ReportThings(sql, StartupList.MachineID, "Remove", ar, ref AlreadyReported, RepElementRoot);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("SEH in Startup Reporting " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
        }

        [ReportingExplainAttr(ReportingPolicyType.Startup)]
        public class StartupReporting : IReportingExplain
        {
            public string Explain(string JSON)
            {
                if (JSON == null)
                    return ("Startup: no data");

                try
                {
                    ReportingStartup rd = JsonConvert.DeserializeObject<ReportingStartup>(JSON);
                    string res = "Action: " + rd.Action + "\r\n";
                    res += rd.App.Key + "\r\n";
                    res += "Location: " + rd.App.Location + "\r\n";
                    res += "Item: " + rd.App.Item + "\r\n";
                    res += "HKCUUser: " + (string.IsNullOrWhiteSpace(rd.App.HKCUUser) == true ? "HKLM SYSTEM" : rd.App.HKCUUser);
                    return (res);
                }
                catch
                {
                    return ("Startup Data faulty: " + JSON);
                }
            }

            public Image GetIcon()
            {
                return (Resources.Run.ToBitmap());
            }
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/startupitems", "StartupsRep", "id")]
        public RESTStatus GetStartupItems(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            StartupsRep = new ListStartupItemReport();
            StartupsRep.Items = new List<StartupItemFull>();

            lock (ni.sqllock)
            {
                SqlDataReader dr;
                if (string.IsNullOrWhiteSpace(id) == true)
                {
                    dr = sql.ExecSQLReader("select * from Startups inner join ComputerAccounts on ComputerAccounts.MachineID=Startups.MachineID left outer join UsersList on UsersList.SID=HKCUUser AND UsersList.MachineID=Startups.MachineID order by Location,[Key]");
                }
                else
                {
                    if (Computers.MachineExists(sql, id) == false)
                    {
                        ni.Error = "Invalid data";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.NotFound);
                    }

                    dr = sql.ExecSQLReader("select * from Startups inner join ComputerAccounts on ComputerAccounts.MachineID=Startups.MachineID left outer join UsersList on UsersList.SID=HKCUUser AND UsersList.MachineID=Startups.MachineID WHERE ComputerAccounts.MachineID=@m order by Location,[Key]",
                        new SQLParam("@m", id));
                }

                while (dr.Read())
                {
                    StartupItemFull ar = new StartupItemFull();
                    ar.Computername = Convert.ToString(dr["Computername"]);
                    ar.DT = SQLLib.GetDTUTC(dr["DT"]);
                    ar.HKCUUser = Convert.ToString(dr["HKCUUser"]);
                    ar.MachineID = Convert.ToString(dr["MachineID"]);
                    ar.Username = Convert.ToString(dr["Username"]);
                    ar.Item = Convert.ToString(dr["Item"]);
                    ar.Key = Convert.ToString(dr["Key"]);
                    ar.Location = Convert.ToString(dr["Location"]);

                    StartupsRep.Items.Add(ar);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }
    }
}
