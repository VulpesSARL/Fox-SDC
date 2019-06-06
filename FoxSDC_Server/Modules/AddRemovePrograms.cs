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
    class AddRemovePrograms
    {
        [VulpesRESTfulRet("AddRemoveRep")]
        public ListAddRemoveAppsReport AddRemoveRep;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/addremove", "", "")]
        public RESTStatus ReportAddRemovePrograms(SQLLib sql, ListAddRemoveApps AddRemoveList, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            AddRemoveList.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", AddRemoveList.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Denied);
                }
            }

            if (AddRemoveList.Items == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }
            if (AddRemoveList.SIDUsers == null)
                AddRemoveList.SIDUsers = new List<string>();

            foreach (AddRemoveApp rep in AddRemoveList.Items)
            {
                if (rep.ProductID == null)
                    rep.ProductID = "";
                if (rep.Name == null)
                    rep.Name = "";
                if (rep.DisplayVersion == null)
                    rep.DisplayVersion = "";
                if (rep.UninstallString == null)
                    rep.UninstallString = "";
                if (rep.Language == null)
                    rep.Language = "";
                if (rep.DisplayLanguage == null)
                    rep.DisplayLanguage = "";
                if (rep.HKCUUser == null)
                    rep.HKCUUser = "";

                rep.ProductID = rep.ProductID.Trim();
                rep.Name = rep.Name.Trim();
                rep.DisplayVersion = rep.DisplayVersion.Trim();
                rep.UninstallString = rep.UninstallString.Trim();
                rep.Language = rep.Language.Trim();
                rep.DisplayLanguage = rep.DisplayLanguage.Trim();
                rep.HKCUUser = rep.HKCUUser.Trim();
            }

            List<AddRemoveApp> Installed = new List<AddRemoveApp>();
            List<AddRemoveApp> Reported = AddRemoveList.Items;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM AddRemovePrograms WHERE MachineID=@id", new SQLParam("@id", AddRemoveList.MachineID));
                while (dr.Read())
                {
                    AddRemoveApp ar = new AddRemoveApp();
                    ar.DisplayLanguage = Convert.ToString(dr["DisplayLanguage"]);
                    ar.DisplayVersion = Convert.ToString(dr["DisplayVersion"]);
                    ar.IsMSI = Convert.ToBoolean(dr["IsMSI"]);
                    ar.IsWOWBranch = Convert.ToBoolean(dr["IsWOWBranch"]);
                    ar.IsSystemComponent = Convert.ToBoolean(dr["IsSystemComponent"]);
                    ar.Language = Convert.ToString(dr["Language"]);
                    ar.MachineID = Convert.ToString(dr["MachineID"]);
                    ar.Name = Convert.ToString(dr["Name"]);
                    ar.ProductID = Convert.ToString(dr["ProductID"]);
                    ar.UninstallString = Convert.ToString(dr["UninstallString"]);
                    ar.VersionMajor = Convert.ToInt32(dr["VersionMajor"]);
                    ar.VersionMinor = Convert.ToInt32(dr["VersionMinor"]);
                    ar.HKCUUser = Convert.ToString(dr["HKCUUser"]);
                    if (string.IsNullOrWhiteSpace(ar.HKCUUser) == true)
                        ar.HKCUUser = "";

                    Installed.Add(ar);
                }
                dr.Close();
            }

            List<AddRemoveApp> Updated = new List<AddRemoveApp>();
            List<AddRemoveApp> Unchanged = new List<AddRemoveApp>();
            List<AddRemoveApp> Removed = new List<AddRemoveApp>();
            List<AddRemoveApp> Added = new List<AddRemoveApp>();

            foreach (AddRemoveApp inst in Installed)
            {
                if (inst.ProductID == null)
                    inst.ProductID = "";
                if (inst.Name == null)
                    inst.Name = "";
                if (inst.DisplayVersion == null)
                    inst.DisplayVersion = "";
                if (inst.UninstallString == null)
                    inst.UninstallString = "";
                if (inst.Language == null)
                    inst.Language = "";
                if (inst.DisplayLanguage == null)
                    inst.DisplayLanguage = "";

                inst.ProductID = inst.ProductID.Trim();
                inst.Name = inst.Name.Trim();
                inst.DisplayVersion = inst.DisplayVersion.Trim();
                inst.UninstallString = inst.UninstallString.Trim();
                inst.Language = inst.Language.Trim();
                inst.DisplayLanguage = inst.DisplayLanguage.Trim();
                inst.HKCUUser = inst.HKCUUser.Trim();

                bool Found = false;
                foreach (AddRemoveApp rep in Reported)
                {
                    if (rep.ProductID.ToLower() == inst.ProductID.ToLower() && rep.IsWOWBranch == inst.IsWOWBranch && rep.HKCUUser == inst.HKCUUser)
                    {
                        if (rep.IsMSI == inst.IsMSI && rep.IsSystemComponent == inst.IsSystemComponent && rep.Name.ToLower() == inst.Name.ToLower() &&
                            rep.DisplayVersion.ToLower() == inst.DisplayVersion.ToLower() && inst.UninstallString.ToLower() == rep.UninstallString.ToLower() &&
                            rep.VersionMajor == inst.VersionMajor && rep.VersionMinor == inst.VersionMinor && rep.Language.ToLower() == inst.Language.ToLower() &&
                            rep.DisplayLanguage.ToLower() == inst.DisplayLanguage.ToLower())
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
                    if (inst.HKCUUser != "" && AddRemoveList.SIDUsers.Contains(inst.HKCUUser, StringComparer.InvariantCultureIgnoreCase) == false)
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

            foreach (AddRemoveApp inst in Reported)
            {
                bool Found = false;
                foreach (AddRemoveApp rep in Installed)
                {
                    if (rep.ProductID.ToLower() == inst.ProductID.ToLower() && rep.IsWOWBranch == inst.IsWOWBranch && rep.HKCUUser == inst.HKCUUser)
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
                    foreach (AddRemoveApp ar in Removed)
                    {
                        sql.ExecSQL("DELETE FROM AddRemovePrograms WHERE MachineID=@id AND IsWOWBranch=@wow AND ProductID=@prod AND HKCUUser=@user",
                            new SQLParam("@id", AddRemoveList.MachineID),
                            new SQLParam("@prod", ar.ProductID),
                            new SQLParam("@user", ar.HKCUUser),
                            new SQLParam("@wow", ar.IsWOWBranch));
                    }
                    foreach (AddRemoveApp ar in Updated)
                    {
                        sql.ExecSQL(@"UPDATE AddRemovePrograms SET
                            IsMSI=@IsMSI,
                            IsSystemComponent=@IsSystemComponent,
                            Name=@Name,
                            DisplayVersion=@DisplayVersion,
                            UninstallString=@UninstallString,
                            VersionMajor=@VersionMajor,
                            VersionMinor=@VersionMinor,
                            Language=@Language,
                            DisplayLanguage=@DisplayLanguage,
                            DT=@DT
                            WHERE MachineID=@id AND IsWOWBranch=@wow AND ProductID=@prod AND HKCUUser=@user",
                            new SQLParam("@id", AddRemoveList.MachineID),
                            new SQLParam("@prod", ar.ProductID),
                            new SQLParam("@wow", ar.IsWOWBranch),
                            new SQLParam("@IsMSI", ar.IsMSI),
                            new SQLParam("@IsSystemComponent", ar.IsSystemComponent),
                            new SQLParam("@Name", ar.Name.Trim()),
                            new SQLParam("@DisplayVersion", ar.DisplayVersion),
                            new SQLParam("@UninstallString", ar.UninstallString),
                            new SQLParam("@VersionMajor", ar.VersionMajor),
                            new SQLParam("@VersionMinor", ar.VersionMinor),
                            new SQLParam("@Language", ar.Language),
                            new SQLParam("@DisplayLanguage", ar.DisplayLanguage),
                            new SQLParam("@user", ar.HKCUUser),
                            new SQLParam("@DT", DateTime.UtcNow));
                    }
                    foreach (AddRemoveApp ar in Added)
                    {
                        sql.InsertMultiData("AddRemovePrograms",
                            new SQLData("MachineID", AddRemoveList.MachineID),
                            new SQLData("ProductID", ar.ProductID),
                            new SQLData("IsWOWBranch", ar.IsWOWBranch),
                            new SQLData("IsMSI", ar.IsMSI),
                            new SQLData("IsSystemComponent", ar.IsSystemComponent),
                            new SQLData("Name", ar.Name.Trim()),
                            new SQLData("DisplayVersion", ar.DisplayVersion),
                            new SQLData("UninstallString", ar.UninstallString),
                            new SQLData("VersionMajor", ar.VersionMajor),
                            new SQLData("VersionMinor", ar.VersionMinor),
                            new SQLData("Language", ar.Language),
                            new SQLData("DisplayLanguage", ar.DisplayLanguage),
                            new SQLData("HKCUUser", ar.HKCUUser),
                            new SQLData("DT", DateTime.UtcNow));
                    }
                    sql.CommitTransaction();
                }
                catch (Exception ee)
                {
                    sql.RollBackTransaction();
                    FoxEventLog.WriteEventLog("DB Error: Cannot update AddRemovePrograms: " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    return (RESTStatus.ServerError);
                }
                finally
                {
                    sql.SEHError = false;
                }
            }

            AddRemoveProgramsLst l = new AddRemoveProgramsLst();
            l.Added = Added;
            l.Removed = Removed;
            l.Unchanged = Unchanged;
            l.Updated = Updated;
            l.MachineID = AddRemoveList.MachineID;
            Thread t = new Thread(new ParameterizedThreadStart(new DReportingThread(ReportingThread)));
            t.Start(l);

            return (RESTStatus.Created);
        }

        class AddRemoveProgramsLst
        {
            public List<AddRemoveApp> Updated;
            public List<AddRemoveApp> Unchanged;
            public List<AddRemoveApp> Removed;
            public List<AddRemoveApp> Added;
            public string MachineID;
        }

        delegate void DReportingThread(object AddRemoveProgramsListO);

        List<AddRemoveApp> GetFilteredData(List<AddRemoveApp> FullList, ComputerData computerdata, ReportingPolicyElementAddRemovePrograms arprep)
        {
            List<AddRemoveApp> Lst = new List<AddRemoveApp>();
            if (arprep.Names == null)
                arprep.Names = new List<string>();
            foreach (AddRemoveApp ar in FullList)
            {
                if (arprep.Names.Count != 0)
                {
                    bool Found = false;
                    foreach (string n in arprep.Names)
                    {
                        switch (arprep.SearchNameIn)
                        {
                            case 0:
                                if (n.ToLower() == ar.ProductID.ToLower())
                                    Found = true;
                                break;
                            case 1:
                                if (ar.Name.ToLower().Contains(n.ToLower()) == true)
                                    Found = true;
                                break;
                            case 2:
                                if (ar.Name.ToLower().StartsWith(n.ToLower()) == true)
                                    Found = true;
                                break;
                        }
                        if (Found == true)
                            break;
                    }
                    if (Found == false)
                        continue;
                }
                if (arprep.SearchBits != 0)
                {
                    if (computerdata.Is64Bit == false)
                    {
                        if (arprep.SearchBits != 1)
                            continue;
                    }
                    else
                    {
                        if (arprep.SearchBits == 1 && ar.IsWOWBranch == false)
                            continue;
                        if (arprep.SearchBits == 2 && ar.IsWOWBranch == true)
                            continue;
                    }
                }
                Lst.Add(ar);
            }
            return (Lst);
        }

        void ReportThings(SQLLib sql, string MachineID, string Method, AddRemoveApp ar, ref Dictionary<string, Int64> AlreadyReported, ReportingPolicyElement RepElementRoot)
        {
            string ID = MachineID + "\\\\" + ar.ProductID.ToLower() + "\\\\" + (ar.IsWOWBranch == true ? "1" : "0");
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

            ReportingProcessor.ReportAddRemoveApps(sql, MachineID, Method, ar, Flags);

            if (AlreadyReported.ContainsKey(ID) == true)
            {
                AlreadyReported[ID] |= (Int64)Flags;
            }
            else
            {
                AlreadyReported.Add(ID, (Int64)Flags);
            }
        }

        void ReportingThread(object AddRemoveProgramsListO)
        {
            SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for AddRemovePrograms Data");
            if (sql == null)
            {
                FoxEventLog.WriteEventLog("Cannot connect to SQL Server for AddRemovePrograms Reporting!", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            try
            {
                AddRemoveProgramsLst AddRemoveProgramsList = (AddRemoveProgramsLst)AddRemoveProgramsListO;
                ComputerData computerdata = Computers.GetComputerDetail(sql, AddRemoveProgramsList.MachineID);
                if (computerdata == null)
                {
                    FoxEventLog.WriteEventLog("Cannot get any computer data for AddRemovePrograms Reporting!", System.Diagnostics.EventLogEntryType.Error);
                    return;
                }

                List<PolicyObject> Pol = Policies.GetPolicyForComputerInternal(sql, AddRemoveProgramsList.MachineID);
                Dictionary<string, Int64> AlreadyReported = new Dictionary<string, long>();
                foreach (PolicyObject PolO in Pol)
                {
                    if (PolO.Type != PolicyIDs.ReportingPolicy)
                        continue;
                    ReportingPolicyElement RepElementRoot = JsonConvert.DeserializeObject<ReportingPolicyElement>(Policies.GetPolicy(sql, PolO.ID).Data);
                    if (RepElementRoot.Type != ReportingPolicyType.AddRemovePrograms)
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
                        ReportingPolicyElementAddRemovePrograms arprep = JsonConvert.DeserializeObject<ReportingPolicyElementAddRemovePrograms>(Element);
                        if (arprep.NotifyOnAdd == false && arprep.NotifyOnRemove == false && arprep.NotifyOnUpdate == false)
                            continue;

                        if (arprep.NotifyOnAdd == true)
                        {
                            foreach (AddRemoveApp ar in GetFilteredData(AddRemoveProgramsList.Added, computerdata, arprep))
                            {
                                ReportThings(sql, AddRemoveProgramsList.MachineID, "Add", ar, ref AlreadyReported, RepElementRoot);
                            }
                        }

                        if (arprep.NotifyOnUpdate == true)
                        {
                            foreach (AddRemoveApp ar in GetFilteredData(AddRemoveProgramsList.Updated, computerdata, arprep))
                            {
                                ReportThings(sql, AddRemoveProgramsList.MachineID, "Update", ar, ref AlreadyReported, RepElementRoot);
                            }
                        }

                        if (arprep.NotifyOnRemove == true)
                        {
                            foreach (AddRemoveApp ar in GetFilteredData(AddRemoveProgramsList.Removed, computerdata, arprep))
                            {
                                ReportThings(sql, AddRemoveProgramsList.MachineID, "Remove", ar, ref AlreadyReported, RepElementRoot);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("SEH in AddRemovePrograms Reporting " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
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

        [ReportingExplainAttr(ReportingPolicyType.AddRemovePrograms)]
        public class AddRemoveProgramsReporting : IReportingExplain
        {
            public string Explain(string JSON)
            {
                if (JSON == null)
                    return ("Add Remove Programs: no data");

                try
                {
                    ReportingAddRemovePrograms rd = JsonConvert.DeserializeObject<ReportingAddRemovePrograms>(JSON);
                    string res = "Action: " + rd.Action + "\r\n";
                    res += rd.App.Name + "\r\n";
                    res += "Version: " + rd.App.DisplayVersion + "\r\n";
                    res += "ID: " + rd.App.ProductID + "\r\n";
                    res += "HKCUUser: " + (string.IsNullOrWhiteSpace(rd.App.HKCUUser) == true ? "HKLM SYSTEM" : rd.App.HKCUUser) + "\r\n";
                    res += "IsWOW: " + (rd.App.IsWOWBranch == false ? "no" : "yes") +
                        " IsMSI: " + (rd.App.IsMSI == false ? "no" : "yes") +
                        " IsSystem: " + (rd.App.IsSystemComponent == false ? "no" : "yes");
                    return (res);
                }
                catch
                {
                    return ("Add Remove Programs Data faulty: " + JSON);
                }
            }

            public Image GetIcon()
            {
                return (Resources.setup.ToBitmap());
            }
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/addremove", "AddRemoveRep", "id")]
        public RESTStatus GetAddRemovePrograms(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            AddRemoveRep = new ListAddRemoveAppsReport();
            AddRemoveRep.Items = new List<AddRemoveAppReport>();

            lock (ni.sqllock)
            {
                SqlDataReader dr;
                if (string.IsNullOrWhiteSpace(id) == true)
                {
                    dr = sql.ExecSQLReader("select * from AddRemovePrograms inner join ComputerAccounts on ComputerAccounts.MachineID=AddRemovePrograms.MachineID left outer join UsersList on UsersList.SID=HKCUUser AND UsersList.MachineID=AddRemovePrograms.MachineID order by Name,VersionMajor,VersionMinor");
                }
                else
                {
                    if (Computers.MachineExists(sql, id) == false)
                    {
                        ni.Error = "Invalid data";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.NotFound);
                    }

                    dr = sql.ExecSQLReader("select * from AddRemovePrograms inner join ComputerAccounts on ComputerAccounts.MachineID=AddRemovePrograms.MachineID left outer join UsersList on UsersList.SID=HKCUUser AND UsersList.MachineID=AddRemovePrograms.MachineID WHERE ComputerAccounts.MachineID=@m order by Name,VersionMajor,VersionMinor",
                        new SQLParam("@m", id));
                }

                while (dr.Read())
                {
                    AddRemoveAppReport ar = new AddRemoveAppReport();
                    ar.Computername = Convert.ToString(dr["Computername"]);
                    ar.DisplayLanguage = Convert.ToString(dr["DisplayLanguage"]);
                    ar.DisplayVersion = Convert.ToString(dr["DisplayVersion"]);
                    ar.IsMSI = Convert.ToBoolean(dr["IsMSI"]);
                    ar.IsWOWBranch = Convert.ToBoolean(dr["IsWOWBranch"]);
                    ar.IsSystemComponent = Convert.ToBoolean(dr["IsSystemComponent"]);
                    ar.Language = Convert.ToString(dr["Language"]);
                    ar.MachineID = Convert.ToString(dr["MachineID"]);
                    ar.Name = Convert.ToString(dr["Name"]);
                    ar.ProductID = Convert.ToString(dr["ProductID"]);
                    ar.UninstallString = Convert.ToString(dr["UninstallString"]);
                    ar.VersionMajor = Convert.ToInt32(dr["VersionMajor"]);
                    ar.VersionMinor = Convert.ToInt32(dr["VersionMinor"]);
                    ar.DT = SQLLib.GetDTUTC(dr["DT"]);
                    ar.HKCUUser = Convert.ToString(dr["HKCUUser"]);
                    ar.Username = Convert.ToString(dr["Username"]);
                    if (string.IsNullOrWhiteSpace(ar.HKCUUser) == true)
                        ar.HKCUUser = "";

                    AddRemoveRep.Items.Add(ar);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }
    }
}
