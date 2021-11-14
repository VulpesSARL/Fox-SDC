using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class WindowsLicClass
    {
        [VulpesRESTfulRet("WindowsLicData")]
        public WindowsLic WindowsLicData;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/windowslic", "", "")]
        public RESTStatus ReportWindowsLic(SQLLib sql, WindowsLic WinLic, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            WinLic.MachineID = ni.Username;
            WinLic.Reported = DateTime.Now;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
            new SQLParam("@m", WinLic.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("DELETE FROM WindowsLic WHERE MachineID=@m",
                new SQLParam("@m", WinLic.MachineID));
            }

            if (NullTest.Test(WinLic) == false)
            {
                ni.Error = "Invalid Data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                sql.InsertMultiData("WindowsLic",
                new SQLData("MachineID", WinLic.MachineID),
                new SQLData("Name", WinLic.Name),
                new SQLData("Description", WinLic.Description),
                new SQLData("GracePeriodRemaining", WinLic.GracePeriodRemaining),
                new SQLData("PartialProductKey", WinLic.PartialProductKey),
                new SQLData("ProductKeyID", WinLic.ProductKeyID),
                new SQLData("ProductKeyID2", WinLic.ProductKeyID2),
                new SQLData("LicenseFamily", WinLic.LicenseFamily),
                new SQLData("ProductKeyChannel", WinLic.ProductKeyChannel),
                new SQLData("LicenseStatus", WinLic.LicenseStatus),
                new SQLData("LicenseStatusText", WinLic.LicenseStatusText));
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/windowslic", "WindowsLicData", "id")]
        public RESTStatus GetWindowsLicData(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (string.IsNullOrWhiteSpace(id) == true)
            {
                ni.Error = "Missing Data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                if (Computers.MachineExists(sql, id) == false)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM WindowsLic WHERE MachineID=@m",
                    new SQLParam("@m", id));
                if (dr.HasRows == false)
                {
                    dr.Close();
                    ni.Error = "No Data";
                    ni.ErrorID = ErrorFlags.NoData;
                    return (RESTStatus.Fail);
                }

                WindowsLicData = new WindowsLic();
                dr.Read();

                WindowsLicData.Description = Convert.ToString(dr["Description"]);
                WindowsLicData.GracePeriodRemaining = Convert.ToInt64(dr["GracePeriodRemaining"]);
                WindowsLicData.LicenseFamily = Convert.ToString(dr["LicenseFamily"]);
                WindowsLicData.LicenseStatus = Convert.ToInt64(dr["LicenseStatus"]);
                WindowsLicData.LicenseStatusText = Convert.ToString(dr["LicenseStatusText"]);
                WindowsLicData.MachineID = Convert.ToString(dr["MachineID"]);
                WindowsLicData.Name = Convert.ToString(dr["Name"]);
                WindowsLicData.PartialProductKey = Convert.ToString(dr["PartialProductKey"]);
                WindowsLicData.ProductKeyChannel = Convert.ToString(dr["ProductKeyChannel"]);
                WindowsLicData.ProductKeyID = Convert.ToString(dr["ProductKeyID"]);
                WindowsLicData.ProductKeyID2 = Convert.ToString(dr["ProductKeyID2"]);
                WindowsLicData.Reported = SQLLib.GetDTUTC(dr["Reported"]);

                dr.Close();
            }

            return (RESTStatus.Success);
        }
    }
}
