using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class Policies
    {
        [VulpesRESTfulRet("NewPolicyID")]
        public NetInt64 NewPolicyID;
        [VulpesRESTfulRet("PolicyList")]
        public PolicyObjectList PolicyList;
        [VulpesRESTfulRet("PolicyListSigned")]
        public PolicyObjectListSigned PolicyListSigned;
        [VulpesRESTfulRet("PolicyObject")]
        public PolicyObject PolicyObj;
        [VulpesRESTfulRet("PolicyObjSigned")]
        public PolicyObjectSigned PolicyObjSigned;

        public static bool PolicyExsits(SQLLib sql, Int64 ID)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Policies WHERE ID=@id",
                    new SQLParam("@id", ID))) == 0)
                return (false);
            return (true);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/policies", "NewPolicyID", "")]
        public RESTStatus CreatePolicy(SQLLib sql, NewPolicyReq request, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (request.Name == null || request.Name.Trim() == "")
            {
                ni.Error = "Invalid name";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            request.Name = request.Name.Trim();

            if (request.Grouping != null && request.MachineID != null)
            {
                ni.Error = "Either Grouping OR MachineID should be set";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (request.Grouping != null)
            {
                lock (ni.sqllock)
                {
                    if (Groups.GroupExsits(sql, request.Grouping.Value) == false)
                    {
                        ni.Error = "Group does not exists";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.Fail);
                    }
                }
            }

            if (request.MachineID != null)
            {
                lock (ni.sqllock)
                {
                    if (Computers.MachineExists(sql, request.MachineID) == false)
                    {
                        ni.Error = "MachineID does not exists";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.Fail);
                    }
                }
            }

            try
            {
                JsonConvert.DeserializeObject(request.Data);
            }
            catch
            {
                ni.Error = "JSON error";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            NewPolicyID = new NetInt64();
            Int64? res;
            lock (ni.sqllock)
            {
                res = sql.InsertMultiDataID("Policies",
                new SQLData("Type", request.Type),
                new SQLData("Name", request.Name),
                new SQLData("Grouping", request.Grouping),
                new SQLData("MachineID", request.MachineID),
                new SQLData("DataBlob", request.Data),
                new SQLData("DT", DateTime.Now),
                new SQLData("Version", 1),
                new SQLData("Enabled", 0));
            }
            if (res == null)
            {
                ni.Error = "SQL Error";
                ni.ErrorID = ErrorFlags.SQLError;
                return (RESTStatus.ServerError);
            }
            NewPolicyID.Data = res.Value;

            return (RESTStatus.Created);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.DELETE, "api/mgmt/policies", "", "id")]
        public RESTStatus DeletePolicy(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Policies.PolicyExsits(sql, id) == false)
                {
                    ni.Error = "Invalid ID";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            try
            {
                lock (ni.sqllock)
                {
                    sql.ExecSQL("DELETE FROM Policies WHERE ID=@id", new SQLParam("@id", id));
                }
            }
            catch
            {
                ni.Error = "SQL Error";
                ni.ErrorID = ErrorFlags.SQLError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.PUT, "api/mgmt/policies", "", "id")]
        public RESTStatus EditPolicy(SQLLib sql, EditPolicy request, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Policies.PolicyExsits(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }
            }

            if (id != request.ID)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (request.DataOnly == false)
            {
                if (request.Name == null || request.Name.Trim() == "")
                {
                    ni.Error = "Invalid name";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }

                request.Name = request.Name.Trim();

                if (request.Grouping != null && request.MachineID != null)
                {
                    ni.Error = "Either Grouping OR MachineID should be set";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }

                if (request.Grouping != null)
                {
                    lock (ni.sqllock)
                    {
                        if (Groups.GroupExsits(sql, request.Grouping.Value) == false)
                        {
                            ni.Error = "Group does not exists";
                            ni.ErrorID = ErrorFlags.InvalidData;
                            return (RESTStatus.Fail);
                        }
                    }
                }

                if (request.MachineID != null)
                {
                    lock (ni.sqllock)
                    {
                        if (Computers.MachineExists(sql, request.MachineID) == false)
                        {
                            ni.Error = "MachineID does not exists";
                            ni.ErrorID = ErrorFlags.InvalidData;
                            return (RESTStatus.Fail);
                        }
                    }
                }
            }

            try
            {
                JsonConvert.DeserializeObject(request.Data);
            }
            catch
            {
                ni.Error = "JSON error";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            try
            {
                if (request.DataOnly == false)
                {
                    lock (ni.sqllock)
                    {
                        sql.ExecSQL("Update Policies SET Name=@n, Grouping=@g, MachineID=@m, DataBlob=@blob, DT=Getutcdate(), Version=Version+1 WHERE ID=@id",
                        new SQLParam("@id", request.ID),
                        new SQLParam("@n", request.Name),
                        new SQLParam("@g", request.Grouping),
                        new SQLParam("@m", request.MachineID),
                        new SQLParam("@blob", request.Data));
                    }
                }
                else
                {
                    lock (ni.sqllock)
                    {
                        sql.ExecSQL("Update Policies SET DataBlob=@blob, DT=Getutcdate(), Version=Version+1 WHERE ID=@id",
                        new SQLParam("@id", request.ID),
                        new SQLParam("@blob", request.Data));
                    }
                }
            }
            catch
            {
                ni.Error = "SQL Error";
                ni.ErrorID = ErrorFlags.SQLError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/policies", "PolicyObject", "id")]
        public RESTStatus GetPolicyObject(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Policies.PolicyExsits(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                PolicyObj = GetPolicy(sql, id);
            }

            return (RESTStatus.Success);
        }

        public static PolicyObject GetPolicy(SQLLib sql, Int64 ID)
        {
            PolicyObject PolicyObj = null;
            SqlDataReader dr = sql.ExecSQLReader("select * from Policies where ID=@id", new SQLParam("@id", ID));
            while (dr.Read())
            {
                PolicyObj = LoadPolicyDB(dr, true, false);
            }
            dr.Close();
            return (PolicyObj);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/signedpolicies", "PolicyObjSigned", "id")]
        public RESTStatus GetPolicyObjectSigned(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Policies.PolicyExsits(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("select * from Policies where ID=@id", new SQLParam("@id", id));
                while (dr.Read())
                {
                    PolicyObj = LoadPolicyDB(dr, true, true);
                }
                dr.Close();
            }

            PolicyObjectSigned objs = new PolicyObjectSigned();
            objs.Policy = PolicyObj;
            if (Certificates.Sign(objs, SettingsManager.Settings.UseCertificate) == false)
            {
                FoxEventLog.WriteEventLog("Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                ni.Error = "Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate;
                ni.ErrorID = ErrorFlags.CannotSign;
                return (RESTStatus.ServerError);
            }

            PolicyObjSigned = objs;

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.PATCH, "api/mgmt/policies", "PolicyObject", "id")]
        public RESTStatus EnableDisablePolicy(SQLLib sql, PolicyEnableDisableRequest request, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Policies.PolicyExsits(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE Policies SET Enabled=@en WHERE ID=@id",
                    new SQLParam("@id", id),
                    new SQLParam("@en", request.Enable));
            }

            return (RESTStatus.Success);
        }

        static PolicyObject LoadPolicyDB(SqlDataReader dr, bool withdata, bool CensorData)
        {
            PolicyObject po = new PolicyObject();
            if (withdata == false)
                po.Data = null;
            else
                po.Data = Convert.ToString(dr["DataBlob"]);
            po.DT = SQLLib.GetDTUTC(dr["DT"]);
            po.Grouping = dr["Grouping"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["Grouping"]);
            po.ID = Convert.ToInt64(dr["ID"]);
            po.MachineID = dr["MachineID"] is DBNull ? null : Convert.ToString(dr["MachineID"]);
            po.Name = Convert.ToString(dr["Name"]);
            po.Version = Convert.ToInt64(dr["Version"]);
            po.Enabled = Convert.ToBoolean(dr["Enabled"]);
            po.Type = Convert.ToInt32(dr["Type"]);

            if (CensorData == true && withdata == true)
            {
                if (po.Type == PolicyIDs.PortMapping)
                {
                    try
                    {
                        PortMappingPolicy p = JsonConvert.DeserializeObject<PortMappingPolicy>(po.Data);
                        p.ServerPort = 0;
                        p.ServerServer = "";
                        po.Data = JsonConvert.SerializeObject(p);
                    }
                    catch
                    {
                        po.Data = null;
                    }
                }
            }

            return (po);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/policies", "PolicyList", "", WithQueryString: true)]
        public RESTStatus ListPolicies(SQLLib sql, object dummy, NetworkConnectionInfo ni, NameValueCollection QueryString)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (QueryString["MachineID"] != null && QueryString["Grouping"] != null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            PolicyList = new PolicyObjectList();
            PolicyList.Items = new List<PolicyObject>();

            bool AllPolicies = true;
            bool WithData = false;
            int tmp;
            int.TryParse(QueryString["AllPolicies"] == null ? "1" : QueryString["AllPolicies"], out tmp);
            AllPolicies = tmp == 1 ? true : false;
            int.TryParse(QueryString["WithData"] == null ? "0" : QueryString["WithData"], out tmp);
            WithData = tmp == 1 ? true : false;

            lock (ni.sqllock)
            {
                SqlDataReader dr = null;
                if (AllPolicies == true)
                {
                    dr = sql.ExecSQLReader("select * from Policies order by Name");
                }
                else
                {
                    dr = sql.ExecSQLReader("select * from Policies where " + (QueryString["Grouping"] == null ? " Grouping is null AND " : " Grouping=@grouping AND ") +
                        (QueryString["MachineID"] == null ? " MachineID is null " : " MachineID=@machineid ") + " order by Name",
                        new SQLParam("@machineid", QueryString["MachineID"]),
                        new SQLParam("@grouping", QueryString["Grouping"]));
                }

                while (dr.Read())
                {
                    PolicyList.Items.Add(LoadPolicyDB(dr, WithData, false));
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }


        //Don't add [VulpesRESTProtected] here!!!!!
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/computerpolicies", "PolicyListSigned", "")]
        public RESTStatus GetPolicyForComputer(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            PolicyListSigned = new PolicyObjectListSigned();
            PolicyListSigned.Items = new List<PolicyObjectSigned>();

            string MachineID = ni.Username;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM Policies WHERE MachineID=@m AND Enabled=1 AND Type not in (" + PolicyIDs.HiddenPoliciesSQLINClause + ")",
                    new SQLParam("@m", MachineID));
                while (dr.Read())
                {
                    PolicyObject obj = LoadPolicyDB(dr, false, true);
                    PolicyObjectSigned objs = new PolicyObjectSigned();
                    objs.Policy = obj;
                    if (Certificates.Sign(objs, SettingsManager.Settings.UseCertificate) == false)
                    {
                        FoxEventLog.WriteEventLog("Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                        ni.Error = "Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate;
                        ni.ErrorID = ErrorFlags.CannotSign;
                        dr.Close();
                        return (RESTStatus.ServerError);
                    }
                    PolicyListSigned.Items.Add(objs);
                }
                dr.Close();
            }

            Int64? GroupID = null;
            object sqlo = sql.ExecSQLScalar("select Grouping from ComputerAccounts where MachineID=@m", new SQLParam("@m", MachineID));
            if (sqlo is DBNull || sqlo == null)
                GroupID = null;
            else
                GroupID = Convert.ToInt64(sqlo);

            do
            {
                lock (ni.sqllock)
                {
                    SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM Policies WHERE " + (GroupID == null ? "Grouping is NULL" : "Grouping=@g") + " AND Enabled=1 AND Type NOT IN (" + PolicyIDs.HiddenPoliciesSQLINClause + ") AND MachineID is NULL",
                        new SQLParam("@g", GroupID));
                    while (dr.Read())
                    {
                        PolicyObject obj = LoadPolicyDB(dr, false, true);
                        PolicyObjectSigned objs = new PolicyObjectSigned();
                        objs.Policy = obj;
                        PolicyListSigned.Items.Add(objs);
                    }
                    dr.Close();
                }

                if (GroupID != null)
                {
                    lock (ni.sqllock)
                    {
                        sqlo = sql.ExecSQLScalar("select ParentID FROM Grouping WHERE ID=@g", new SQLParam("@g", GroupID));
                    }

                    if (sqlo is DBNull || sqlo == null)
                        GroupID = null;
                    else
                        GroupID = Convert.ToInt64(sqlo);
                }
                else
                {
                    break;
                }

            } while (true);

            PolicyListSigned.Items.Reverse();
            Int64 Count = 1;
            foreach (PolicyObjectSigned p in PolicyListSigned.Items)
            {
                p.Policy.Order = Count;
                Count++;
                if (Certificates.Sign(p, SettingsManager.Settings.UseCertificate) == false)
                {
                    FoxEventLog.WriteEventLog("Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                    ni.Error = "Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate;
                    ni.ErrorID = ErrorFlags.CannotSign;
                    return (RESTStatus.ServerError);
                }
            }

            List<PolicyObjectSigned> RemoveFromList = new List<PolicyObjectSigned>();

            #region Resolve Linked Policies for client

            for (int i = 0; i < PolicyListSigned.Items.Count; i++)
            {
                PolicyObjectSigned pol = PolicyListSigned.Items[i];
                if (pol.Policy.Type == PolicyIDs.LinkedPolicy)
                {
                    List<Int64> RunningIDs = new List<Int64>();

                    PolicyObject po = pol.Policy;

                    while (true)
                    {
                        if (RunningIDs.Contains(po.ID) == true)
                        {
                            FoxEventLog.WriteEventLog("Policy ID " + pol.Policy.ID.ToString() + " (" + pol.Policy.Name + ") creates a loop!", System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }

                        RunningIDs.Add(po.ID);

                        lock (ni.sqllock)
                        {
                            po.Data = Convert.ToString(sql.ExecSQLScalar("SELECT DataBlob FROM Policies WHERE ID=@id",
                            new SQLParam("@id", po.ID)));
                        }

                        Int64 PolID;
                        if (Int64.TryParse(po.Data, out PolID) == false)
                        {
                            FoxEventLog.WriteEventLog("Cannot read data of policy ID " + po.ID.ToString() + " (" + po.Name + ") for linking.", System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }

                        lock (ni.sqllock)
                        {
                            if (Policies.PolicyExsits(sql, PolID) == false)
                            {
                                FoxEventLog.WriteEventLog("Policy ID " + PolID.ToString() + " referencing from " + po.ID.ToString() + " (" + po.Name + ") does not exist.", System.Diagnostics.EventLogEntryType.Warning);
                                break;
                            }
                        }

                        lock (ni.sqllock)
                        {
                            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT Enabled FROM Policies WHERE ID=@id", new SQLParam("@id", PolID))) != 1)
                            {
                                break;
                            }
                        }

                        lock (ni.sqllock)
                        {
                            SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM Policies WHERE ID=@id",
                            new SQLParam("@id", PolID));
                            dr.Read();
                            po = LoadPolicyDB(dr, false, true);
                            dr.Close();
                        }

                        if (po == null)
                        {
                            FoxEventLog.WriteEventLog("Cannot read policy ID for linking " + PolID.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }

                        if (po.Type != PolicyIDs.LinkedPolicy)
                        {
                            if (PolicyIDs.HiddenPolicies.Contains(po.Type) == true)
                            {
                                pol.Policy = null;
                                pol.Signature = null;
                                break;
                            }
                            else
                            {
                                pol.Policy = po;
                                pol.Signature = null;
                                break;
                            }
                        }
                    }

                    if (pol.Policy != null)
                    {
                        if (Certificates.Sign(pol, SettingsManager.Settings.UseCertificate) == false)
                        {
                            FoxEventLog.WriteEventLog("Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                            ni.Error = "Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate;
                            ni.ErrorID = ErrorFlags.CannotSign;
                            return (RESTStatus.ServerError);
                        }
                    }
                    else
                    {
                        RemoveFromList.Add(pol);
                    }
                }
            }

            foreach (PolicyObjectSigned pos in RemoveFromList)
            {
                PolicyListSigned.Items.Remove(pos);
            }

            #endregion

            if (Certificates.Sign(PolicyListSigned, SettingsManager.Settings.UseCertificate) == false)
            {
                FoxEventLog.WriteEventLog("Cannot sign policy list with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                ni.Error = "Cannot sign policy list with Certificate " + SettingsManager.Settings.UseCertificate;
                ni.ErrorID = ErrorFlags.CannotSign;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        public static List<PolicyObject> GetPolicyForComputerInternal(SQLLib sql, string MachineID)
        {
            PolicyObjectList PolicyListSigned = new PolicyObjectList();
            PolicyListSigned.Items = new List<PolicyObject>();

            SqlDataReader dr;
            dr = sql.ExecSQLReader("SELECT * FROM Policies WHERE MachineID=@m AND Enabled=1",
                new SQLParam("@m", MachineID));
            while (dr.Read())
            {
                PolicyObject obj = LoadPolicyDB(dr, false, false);
                PolicyListSigned.Items.Add(obj);
            }
            dr.Close();

            Int64? GroupID = null;
            object sqlo = sql.ExecSQLScalar("select Grouping from ComputerAccounts where MachineID=@m", new SQLParam("@m", MachineID));
            if (sqlo is DBNull || sqlo == null)
                GroupID = null;
            else
                GroupID = Convert.ToInt64(sqlo);

            do
            {
                dr = sql.ExecSQLReader("SELECT * FROM Policies WHERE " + (GroupID == null ? "Grouping is NULL" : "Grouping=@g") + " AND Enabled=1 AND MachineID is NULL",
                    new SQLParam("@g", GroupID));
                while (dr.Read())
                {
                    PolicyObject obj = LoadPolicyDB(dr, false, false);
                    PolicyListSigned.Items.Add(obj);
                }
                dr.Close();

                if (GroupID != null)
                {
                    sqlo = sql.ExecSQLScalar("select ParentID FROM Grouping WHERE ID=@g", new SQLParam("@g", GroupID));
                    if (sqlo is DBNull || sqlo == null)
                        GroupID = null;
                    else
                        GroupID = Convert.ToInt64(sqlo);
                }
                else
                {
                    break;
                }

            } while (true);

            PolicyListSigned.Items.Reverse();
            Int64 Count = 1;
            foreach (PolicyObject p in PolicyListSigned.Items)
            {
                p.Order = Count;
                Count++;
            }

            #region Resolve Linked Policies for client

            for (int i = 0; i < PolicyListSigned.Items.Count; i++)
            {
                PolicyObject pol = PolicyListSigned.Items[i];
                if (pol.Type == PolicyIDs.LinkedPolicy)
                {
                    List<Int64> RunningIDs = new List<Int64>();

                    PolicyObject po = pol;

                    while (true)
                    {
                        if (RunningIDs.Contains(po.ID) == true)
                        {
                            FoxEventLog.WriteEventLog("Policy ID " + pol.ID.ToString() + " (" + pol.Name + ") creates a loop!", System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }

                        RunningIDs.Add(po.ID);

                        Int64 PolID;
                        po.Data = Convert.ToString(sql.ExecSQLScalar("SELECT DataBlob FROM Policies WHERE ID=@id",
                            new SQLParam("@id", po.ID)));
                        if (Int64.TryParse(po.Data, out PolID) == false)
                        {
                            FoxEventLog.WriteEventLog("Cannot read data of policy ID " + po.ID.ToString() + " (" + po.Name + ") for linking.", System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }

                        if (Policies.PolicyExsits(sql, PolID) == false)
                        {
                            FoxEventLog.WriteEventLog("Policy ID " + PolID.ToString() + " referencing from " + po.ID.ToString() + " (" + po.Name + ") does not exist.", System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }

                        if (Convert.ToInt32(sql.ExecSQLScalar("SELECT Enabled FROM Policies WHERE ID=@id", new SQLParam("@id", PolID))) != 1)
                        {
                            break;
                        }

                        dr = sql.ExecSQLReader("SELECT * FROM Policies WHERE ID=@id",
                            new SQLParam("@id", PolID));
                        dr.Read();
                        po = LoadPolicyDB(dr, false, false);
                        dr.Close();
                        if (po == null)
                        {
                            FoxEventLog.WriteEventLog("Cannot read policy ID for linking " + PolID.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }
                        if (po.Type != PolicyIDs.LinkedPolicy)
                        {
                            pol = po;
                            PolicyListSigned.Items[i] = po;
                            break;
                        }
                    }
                }
            }

            #endregion

            return (PolicyListSigned.Items);
        }
    }
}
