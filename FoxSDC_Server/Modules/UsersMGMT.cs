using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class UsersMGMT
    {
        private bool MeetPasswordPolicy(string PW)
        {
            if (PW.Length < 8)
                return (false);
            bool UpperChar = false;
            bool LowerChar = false;
            bool Number = false;

            foreach (char S in PW)
            {
                if (S >= 48 && S <= 57)
                    Number = true;
                if (S >= 65 && S <= 90)
                    UpperChar = true;
                if (S >= 97 && S <= 122)
                    LowerChar = true;
            }

            if (Number == false || UpperChar == false || LowerChar == false)
                return (false);

            return (true); //OK 
        }

        [VulpesRESTfulRet("PwStatus")]
        NetBool PwStatus;

        [VulpesRESTfulRet("UserLists")]
        UserDetailsList UserLists;

        [VulpesRESTfulRet("CurrentUser")]
        UserInfo CurrentUser;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/login/user/password", "PwStatus", "")]
        public RESTStatus Password(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            PwStatus = new NetBool();
            PwStatus.Data = ni.MustChangePassword;
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/login/user/password", "", "")]
        public RESTStatus ChangeMyPassword(SQLLib sql, ChangePassword chgpw, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == true)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            ni.Error = "";

            if (ni.IsLDAP == true)
            {
                ni.Error = "Password is LDAP";
                ni.ErrorID = ErrorFlags.IsLDAP;
                return (RESTStatus.Fail);
            }

            string PWMD5REQ = Convert.ToBase64String(Encoding.Unicode.GetBytes(chgpw.OldPassword));

            if (MeetPasswordPolicy(chgpw.NewPassword) == false)
            {
                ni.Error = "Password policy not met";
                ni.ErrorID = ErrorFlags.PWPolicyNotMet;
                return (RESTStatus.Fail);
            }
            int Count = Convert.ToInt32(sql.ExecSQLScalar("SELECT Count(*) FROM Users WHERE Username=@u AND Password=@p",
                new SQLParam("@u", ni.Username),
                new SQLParam("@p", PWMD5REQ)));
            if (Count < 1)
            {
                ni.Error = "Invalid old password";
                ni.ErrorID = ErrorFlags.InvalidPassword;
                return (RESTStatus.Fail);
            }
            string PWMD5New = Convert.ToBase64String(Encoding.Unicode.GetBytes(chgpw.NewPassword));
            sql.ExecSQLNQ("UPDATE Users SET Password=@pw, MustChangePassword=0 WHERE Username=@u",
                new SQLParam("@u", ni.Username),
                new SQLParam("@pw", PWMD5New));
            ni.MustChangePassword = false;

            return (RESTStatus.NoContent);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/login/user/userinfo", "CurrentUser", "")]
        public RESTStatus GetUser(SQLLib sql, object Dummy, NetworkConnectionInfo ni)
        {
            CurrentUser = new UserInfo();
            CurrentUser.Username = ni.Username;
            CurrentUser.Name = ni.Name;
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/login/user/allusers", "UserLists", "")]
        public RESTStatus ListUsers(SQLLib sql, object Dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Fail);
            }

            UserLists = new UserDetailsList();
            UserLists.List = new List<UserDetails>();

            SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM Users ORDER BY Username");
            while (dr.Read())
            {
                UserDetails d = new UserDetails();
                d.EMail = Convert.ToString(dr["EMail"]);
                d.LDAPUsername = Convert.ToString(dr["LDAPUsername"]);
                d.MustChangePassword = Convert.ToBoolean(dr["MustChangePassword"]);
                d.Name = Convert.ToString(dr["Name"]);
                d.Permissions = Convert.ToInt64(dr["Permissions"]);
                d.UseLDAP = Convert.ToBoolean(dr["UseLDAP"]);
                d.Username = Convert.ToString(dr["Username"]);
                UserLists.List.Add(d);
            }
            dr.Close();
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/login/user/changeuser", "", "")]
        public RESTStatus ChangeUser(SQLLib sql, UserDetailsPassword User, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Fail);
            }

            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Users WHERE Username=@u",
                new SQLParam("@u", User.Username))) == 0)
            {
                ni.Error = "Invalid User";
                ni.ErrorID = ErrorFlags.InvalidID;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(User.NewPassword) == false)
            {
                if (MeetPasswordPolicy(User.NewPassword) == false)
                {
                    ni.Error = "Password policy not met";
                    ni.ErrorID = ErrorFlags.PWPolicyNotMet;
                    return (RESTStatus.Fail);
                }
                string PWMD5REQ = Convert.ToBase64String(Encoding.Unicode.GetBytes(User.NewPassword));
                sql.ExecSQL("UPDATE Users SET Password=@p WHERE Username=@u",
                    new SQLParam("@u", User.Username),
                    new SQLParam("@p", PWMD5REQ));
            }

            if (User.Username.ToLower().Trim() == "root")
                User.Permissions = SQLTest.AllPermissions;

            sql.ExecSQL("UPDATE Users SET Name=@n, Permissions=@p, MustChangePassword=@mchg,EMail=@email,UseLDAP=@UseLDAP,LDAPUsername=@LDAPUsername WHERE Username=@u",
                new SQLParam("@u", User.Username),
                new SQLParam("@n", User.Name),
                new SQLParam("@p", User.Permissions),
                new SQLParam("@mchg", User.MustChangePassword),
                new SQLParam("@email", User.EMail),
                new SQLParam("@useldap", User.UseLDAP),
                new SQLParam("@LDAPUsername", User.LDAPUsername));

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/login/user/adduser", "", "")]
        public RESTStatus AddUser(SQLLib sql, NetString User, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(User.Data) == true)
            {
                ni.Error = "Missing username";
                ni.ErrorID = ErrorFlags.InvalidID;
                return (RESTStatus.Fail);
            }

            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Users WHERE Username=@u",
                new SQLParam("@u", User.Data.Trim()))) != 0)
            {
                ni.Error = "User already exists";
                ni.ErrorID = ErrorFlags.InvalidID;
                return (RESTStatus.Fail);
            }

            sql.InsertMultiData("Users",
                new SQLData("Username", User.Data.Trim()),
                new SQLData("Name", "new user"),
                new SQLData("Permissions", 0),
                new SQLData("Password", ""));

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/login/user/deleteuser", "", "")]
        public RESTStatus DeleteUser(SQLLib sql, NetString User, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Fail);
            }

            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Users WHERE Username=@u",
                new SQLParam("@u", User.Data))) == 0)
            {
                ni.Error = "Invalid User";
                ni.ErrorID = ErrorFlags.InvalidID;
                return (RESTStatus.Fail);
            }

            if (User.Data.ToLower().Trim() == "root")
            {
                ni.Error = "Invalid User";
                ni.ErrorID = ErrorFlags.InvalidID;
                return (RESTStatus.Fail);
            }

            sql.ExecSQL("DELETE FROM Users WHERE Username=@u",
                new SQLParam("@u", User.Data));

            return (RESTStatus.Success);
        }
    }
}
