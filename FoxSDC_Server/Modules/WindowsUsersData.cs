using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class UsersData
    {
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/repuserslist", "", "")]
        public RESTStatus ReportUsersList(SQLLib sql, UsersList users, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (users == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            users.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                new SQLParam("@m", users.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("DELETE FROM UsersList WHERE MachineID=@id", new SQLParam("@id", users.MachineID));
            }

            if (users.Users == null)
                users.Users = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> kvp in users.Users)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key) == true || string.IsNullOrWhiteSpace(kvp.Value) == true)
                    continue;

                lock (ni.sqllock)
                {
                    sql.InsertMultiData("UsersList",
                            new SQLData("MachineID", users.MachineID),
                            new SQLData("SID", kvp.Key),
                            new SQLData("Username", kvp.Value));
                }
            }

            return (RESTStatus.Success);
        }
    }
}
