using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class DeviceFilters
    {
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/devicefilters", "", "")]
        public RESTStatus ReportDevicesFilter(SQLLib sql, FilterDriverList devices, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (devices == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            devices.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
              new SQLParam("@m", devices.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("DELETE FROM DevicesFilter WHERE MachineID=@id", new SQLParam("@id", devices.MachineID));
            }

            if (devices.List == null)
                devices.List = new List<FilterDriver>();

            int Counter = 0;

            foreach (FilterDriver flt in devices.List)
            {
                lock (ni.sqllock)
                {
                    sql.InsertMultiData("DevicesFilter",
                        new SQLData("MachineID", devices.MachineID),
                        new SQLData("Index", Counter),
                        new SQLData("ClassGUID", flt.ClassGUID),
                        new SQLData("ServiceName", flt.ServiceName),
                        new SQLData("Type", flt.Type));
                }
                Counter++;
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("LstFltData")]
        public FilterDriverList LstFltData;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/devicesfilters", "LstFltData", "id")]
        public RESTStatus ListDevicesFilters(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (string.IsNullOrWhiteSpace(id) == true)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.NotFound);
            }

            lock (ni.sqllock)
            {
                if (Computers.MachineExists(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            LstFltData = new FilterDriverList();
            LstFltData.List = new List<FilterDriver>();
            LstFltData.MachineID = id;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM DevicesFilter WHERE MachineID=@mid", new SQLParam("@mid", id));
                while (dr.Read())
                {
                    FilterDriver n = new FilterDriver();
                    n.ClassGUID = Convert.ToString(dr["ClassGUID"]);
                    n.ServiceName = Convert.ToString(dr["ServiceName"]);
                    n.Type = Convert.ToInt32(dr["Type"]);
                    LstFltData.List.Add(n);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }
    }
}
