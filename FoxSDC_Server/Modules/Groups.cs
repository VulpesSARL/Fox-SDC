using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class Groups
    {
        [VulpesRESTfulRet("GroupList")]
        public GroupElementList GroupList;
        [VulpesRESTfulRet("NewGroupID")]
        public NetInt64 NewGroupID;
        [VulpesRESTfulRet("GroupElement")]
        public GroupElement GroupElement;

        public static bool GroupExsits(SQLLib sql, Int64 GroupID)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Grouping WHERE ID=@id",
                    new SQLParam("@id", GroupID))) == 0)
                return (false);
            return (true);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/groups", "GroupList", "")]
        public RESTStatus GetGroups(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            GroupList = new GroupElementList();
            GroupList.List = new List<GroupElement>();
            lock (ni.sqllock)
            {
                SqlDataReader dr = dr = sql.ExecSQLReader("select * from Grouping WHERE ParentID is null order by Name");
                while (dr.Read())
                {
                    GroupElement ge = new GroupElement();
                    ge.ID = Convert.ToInt64(dr["ID"]);
                    ge.Name = Convert.ToString(dr["Name"]);
                    ge.ParentID = dr["ParentID"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["ParentID"]);
                    GroupList.List.Add(ge);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/groups", "GroupList", "id")]
        public RESTStatus GetGroups(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            GroupList = new GroupElementList();
            GroupList.List = new List<GroupElement>();

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("select * from Grouping WHERE ParentID=@p order by Name",
                      new SQLParam("@p", id));
                while (dr.Read())
                {
                    GroupElement ge = new GroupElement();
                    ge.ID = Convert.ToInt64(dr["ID"]);
                    ge.Name = Convert.ToString(dr["Name"]);
                    ge.ParentID = dr["ParentID"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["ParentID"]);
                    GroupList.List.Add(ge);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/groupdetail", "GroupElement", "id")]
        public RESTStatus GetGroupDetails(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (GroupExsits(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidID;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM Grouping WHERE ID=@id", new SQLParam("@id", id));
                while (dr.Read())
                {
                    GroupElement = new GroupElement();
                    GroupElement.ID = Convert.ToInt64(dr["ID"]);
                    GroupElement.Name = Convert.ToString(dr["Name"]);
                    GroupElement.ParentID = dr["ParentID"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["ParentID"]);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.PATCH, "api/mgmt/groups", "", "id")]
        public RESTStatus RenameGroup(SQLLib sql, NetString Rename, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Grouping WHERE ID=@id",
                new SQLParam("@id", id))) == 0)
                {
                    ni.Error = "Group does not exist";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }
            
            if (Rename.Data == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (Rename.Data.Trim() == "")
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            Int64? Parent = null;
            lock (ni.sqllock)
            {
                object P = sql.ExecSQLScalar("SELECT ParentID FROM Grouping WHERE ID=@id",
                new SQLParam("@id", id));
                Parent = P is DBNull ? (Int64?)null : Convert.ToInt64(P);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Grouping WHERE ParentID" + (Parent == null ? " is null " : "=@pid") + " AND Name=@n AND ID!=@id",
                    new SQLParam("@pid", Parent),
                    new SQLParam("@id", id),
                    new SQLParam("@n", Rename.Data.Trim()))) > 0)
                {
                    ni.Error = "Duplicate name";
                    ni.ErrorID = ErrorFlags.DuplicateElement;
                    return (RESTStatus.Fail);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE Grouping SET Name=@n WHERE ID=@id",
                new SQLParam("@n", Rename.Data.Trim()),
                new SQLParam("@id", id));
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/groups", "NewGroupID", "")]
        public RESTStatus CreateGroup(SQLLib sql, CreateGroup GroupName, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (GroupName.Name == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (GroupName.Name.Trim() == "")
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (GroupName.ToParent != null)
            {
                lock (ni.sqllock)
                {
                    if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Grouping WHERE ID=@id",
                    new SQLParam("@id", GroupName.ToParent))) == 0)
                    {
                        ni.Error = "Group does not exist";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.Fail);
                    }
                }
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Grouping WHERE ParentID=@pid AND Name=@n",
                new SQLParam("@pid", GroupName.ToParent),
                new SQLParam("@n", GroupName.Name.Trim()))) > 0)
                {
                    ni.Error = "Duplicate name";
                    ni.ErrorID = ErrorFlags.DuplicateElement;
                    return (RESTStatus.Fail);
                }
            }

            NewGroupID = new NetInt64();

            Int64? nid = null;
            lock (ni.sqllock)
            {
                nid = sql.InsertMultiDataID("Grouping",
                    new SQLData("Name", GroupName.Name.Trim()),
                    new SQLData("ParentID", GroupName.ToParent));
            }

            if (nid == null)
            {
                ni.Error = "SQL Error";
                ni.ErrorID = ErrorFlags.SQLError;
                return (RESTStatus.ServerError);
            }
            NewGroupID.Data = nid.Value;

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.DELETE, "api/mgmt/groups", "", "id")]
        public RESTStatus DeleteGroup(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Grouping WHERE ID=@id",
                  new SQLParam("@id", id))) == 0)
                {
                    ni.Error = "Group does not exist";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            try
            {
                lock (ni.sqllock)
                {
                    sql.ExecSQL("DELETE FROM Grouping WHERE ID=@id", new SQLParam("@id", id));
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
    }
}
