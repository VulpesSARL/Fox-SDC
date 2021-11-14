using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class BitlockerRKRep
    {
        [VulpesRESTfulRet("LstRKData")]
        public BitlockerRKList LstRKData;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/bitlockerrklist", "", "")]
        public RESTStatus ReportBitlockerRK(SQLLib sql, BitlockerRKList BitLockerRK, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            BitLockerRK.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", BitLockerRK.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Denied);
                }
            }

            if (BitLockerRK.List == null)
                BitLockerRK.List = new List<BitlockerRK>();

            if (BitLockerRK.List.Count == 0)
                return (RESTStatus.Success);

            List<string> DiskIDs = new List<string>();
            SqlDataReader dr = sql.ExecSQLReader("SELECT DeviceID FROM BitlockerRK WHERE MachineID=@m",
                new SQLParam("@m", BitLockerRK.MachineID));
            while (dr.Read())
            {
                DiskIDs.Add(Convert.ToString(dr["DeviceID"]).ToLower());
            }
            dr.Close();

            foreach (BitlockerRK disk in BitLockerRK.List)
            {
                if (disk.Keys == null)
                    continue;
                if (disk.Keys.Count == 0)
                    continue;
                disk.Reported = DateTime.UtcNow;
                if (disk.DeviceID == null)
                    continue;

                string RKs = JsonConvert.SerializeObject(disk.Keys);

                if (DiskIDs.Contains(disk.DeviceID.ToLower()) == true)
                {
                    lock (ni.sqllock)
                    {
                        sql.ExecSQL("DELETE FROM BitLockerRK WHERE MachineID=@m AND DeviceID=@d",
                        new SQLParam("@m", BitLockerRK.MachineID),
                        new SQLParam("@d", disk.DeviceID));
                    }
                }
                lock (ni.sqllock)
                {
                    sql.InsertMultiData("BitLockerRK",
                    new SQLData("MachineID", BitLockerRK.MachineID),
                    new SQLData("DeviceID", disk.DeviceID),
                    new SQLData("DriveLetter", disk.DriveLetter == null ? "" : disk.DriveLetter),
                    new SQLData("Keys", RKs),
                    new SQLData("Reported", DateTime.UtcNow));
                }
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/bitlockerrkdata", "LstRKData", "id")]
        public RESTStatus ListRKData(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
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

            LstRKData = new BitlockerRKList();
            LstRKData.List = new List<BitlockerRK>();
            LstRKData.MachineID = id;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM BitlockerRK WHERE MachineID=@m",
                new SQLParam("@m", id));
                while (dr.Read())
                {
                    BitlockerRK rk = new BitlockerRK();
                    rk.DeviceID = Convert.ToString(dr["DeviceID"]);
                    rk.DriveLetter = Convert.ToString(dr["DriveLetter"]);
                    rk.Reported = SQLLib.GetDTUTC(dr["Reported"]);
                    rk.Keys = JsonConvert.DeserializeObject<List<BitlockerRKKeyElement>>(Convert.ToString(dr["Keys"]));
                    LstRKData.List.Add(rk);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }
    }
}
