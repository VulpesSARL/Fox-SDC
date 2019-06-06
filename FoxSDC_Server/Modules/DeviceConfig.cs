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
    class DeviceConfig
    {
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/deviceslist", "", "")]
        public RESTStatus ReportDevicesConfig(SQLLib sql, PnPDeviceList devices, NetworkConnectionInfo ni)
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
                sql.ExecSQL("DELETE FROM DevicesConfig WHERE MachineID=@id", new SQLParam("@id", devices.MachineID));
            }

            if (devices.List == null)
                devices.List = new List<PnPDevice>();

            int Counter = 0;

            foreach (PnPDevice dev in devices.List)
            {
                if (dev.CompatibleID == null)
                    dev.CompatibleID = new List<string>();
                if (dev.HardwareID == null)
                    dev.HardwareID = new List<string>();

                lock (ni.sqllock)
                {
                    sql.InsertMultiData("DevicesConfig",
                        new SQLData("MachineID", devices.MachineID),
                        new SQLData("Index", Counter),
                        new SQLData("Availability", dev.Availability),
                        new SQLData("Caption", dev.Caption == null ? "" : dev.Caption),
                        new SQLData("ClassGuid", dev.ClassGuid == null ? "" : dev.ClassGuid),
                        new SQLData("CompatibleID", JsonConvert.SerializeObject(dev.CompatibleID)),
                        new SQLData("ConfigManagerErrorCode", dev.ConfigManagerErrorCode),
                        new SQLData("ConfigManagerUserConfig", dev.ConfigManagerUserConfig),
                        new SQLData("CreationClassName", dev.CreationClassName == null ? "" : dev.CreationClassName),
                        new SQLData("Description", dev.Description == null ? "" : dev.Description),
                        new SQLData("ErrorCleared", dev.ErrorCleared),
                        new SQLData("ErrorDescription", dev.ErrorDescription == null ? "" : dev.ErrorDescription),
                        new SQLData("HardwareID", JsonConvert.SerializeObject(dev.HardwareID)),
                        new SQLData("InstallDate", dev.InstallDate),
                        new SQLData("LastErrorCode", dev.LastErrorCode),
                        new SQLData("Manufacturer", dev.Manufacturer == null ? "" : dev.Manufacturer),
                        new SQLData("Name", dev.Name == null ? "" : dev.Name),
                        new SQLData("PNPClass", dev.PNPClass == null ? "" : dev.PNPClass),
                        new SQLData("PNPDeviceID", dev.PNPDeviceID == null ? "" : dev.PNPDeviceID),
                        new SQLData("Present", dev.Present),
                        new SQLData("Service", dev.Service == null ? "" : dev.Service),
                        new SQLData("Status", dev.Status == null ? "" : dev.Status),
                        new SQLData("StatusInfo", dev.StatusInfo));
                }
                Counter++;
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("LstDevData")]
        public PnPDeviceList LstDevData;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/devicesdata", "LstDevData", "id")]
        public RESTStatus ListDevicesData(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
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

            LstDevData = new PnPDeviceList();
            LstDevData.List = new List<PnPDevice>();
            LstDevData.MachineID = id;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM DevicesConfig WHERE MachineID=@mid", new SQLParam("@mid", id));
                while (dr.Read())
                {
                    PnPDevice n = new PnPDevice();
                    n.Availability = Convert.ToInt32(dr["Availability"]);
                    n.Caption = Convert.ToString(dr["Caption"]);
                    n.ClassGuid = Convert.ToString(dr["ClassGuid"]);
                    n.ConfigManagerErrorCode = Convert.ToInt32(dr["ConfigManagerErrorCode"]);
                    n.ConfigManagerUserConfig = Convert.ToBoolean(dr["ConfigManagerUserConfig"]);
                    n.CreationClassName = Convert.ToString(dr["CreationClassName"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.ErrorCleared = dr["ErrorCleared"] is DBNull ? (bool?)null : Convert.ToBoolean(dr["ErrorCleared"]);
                    n.ErrorDescription = Convert.ToString(dr["ErrorDescription"]);
                    n.Index = Convert.ToInt32(dr["Index"]);
                    n.InstallDate = dr["InstallDate"] is DBNull ? (DateTime?)null : SQLLib.GetDTUTC(dr["InstallDate"]);
                    n.LastErrorCode = dr["LastErrorCode"] is DBNull ? (int?)null : Convert.ToInt32(dr["LastErrorCode"]);
                    n.Manufacturer = Convert.ToString(dr["Manufacturer"]);
                    n.Name = Convert.ToString(dr["Name"]);
                    n.PNPClass = Convert.ToString(dr["PNPClass"]);
                    n.PNPDeviceID = Convert.ToString(dr["PNPDeviceID"]);
                    n.Present = Convert.ToBoolean(dr["Present"]);
                    n.Service = Convert.ToString(dr["Service"]);
                    n.Status = Convert.ToString(dr["Status"]);
                    n.StatusInfo = dr["StatusInfo"] is DBNull ? (int?)null : Convert.ToInt32(dr["StatusInfo"]);
                    n.HardwareID = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(dr["HardwareID"]));
                    n.CompatibleID = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(dr["CompatibleID"]));
                    LstDevData.List.Add(n);
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }
    }
}
