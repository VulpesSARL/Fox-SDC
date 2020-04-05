using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class CerdentialRecovery
    {
        [VulpesRESTfulRet("RD")]
        RecoveryData RD;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/login/recovery", "RD", "", false, PutIPAddress = true)]
        public RESTStatus ComputerLogin(SQLLib sql, RecoveryLogon logon, NetworkConnectionInfo ni, string IPAddress)
        {
            RD = new RecoveryData();

            if (Fox_LicenseGenerator.SDCLicensing.ValidLicense == false)
            {
                RD.Worked = false;
                return (RESTStatus.Fail);
            }

            if (Fox_LicenseGenerator.SDCLicensing.TestExpiry() == false)
            {
                RD.Worked = false;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(logon.UCID) == true)
            {
                RD.Worked = false;
                return (RESTStatus.Fail);
            }

            if (Settings.Default.UseContract == true)
            {
                if (string.IsNullOrWhiteSpace(logon.ContractID) == true || string.IsNullOrWhiteSpace(logon.ContractPassword) == true)
                {
                    RD.Worked = false;
                    return (RESTStatus.Fail);
                }
            }

            string newID = NetworkConnection.NewSession();
            ni = NetworkConnection.GetSession(newID);
            if (NetworkConnectionProcessor.InitNi(ni) == false)
            {
                NetworkConnection.DeleteSession(newID);
                RD.Worked = false;
                return (RESTStatus.ServerError);
            }

            sql = ni.sql;

            if (Settings.Default.UseContract == true)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT Count(*) FROM Contracts WHERE ContractID=@id AND ContractPassword=@pw AND Disabled=0",
                    new SQLParam("@id", logon.ContractID),
                    new SQLParam("@pw", logon.ContractPassword))) == 0)
                {
                    NetworkConnection.DeleteSession(newID);
                    RD.Worked = false;
                    return (RESTStatus.Fail);
                }

                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE ContractID=@id AND UCID=@u",
                    new SQLParam("@id", logon.ContractID),
                    new SQLParam("@u", logon.UCID))) == 0)
                {
                    NetworkConnection.DeleteSession(newID);
                    RD.Worked = false;
                    return (RESTStatus.Fail);
                }
            }

            if(Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE UCID=@u",
                new SQLParam ("@u", logon.UCID))) == 0)
            {
                NetworkConnection.DeleteSession(newID);
                RD.Worked = false;
                return (RESTStatus.Fail);
            }

            SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM ComputerAccounts WHERE UCID=@u",
                new SQLParam("@u", logon.UCID));

            dr.Read();

            string Check = Convert.ToString(dr["CPUName"]).Trim();
            Check += Convert.ToString(dr["ComputerModel"]).Trim();
            Check += Convert.ToString(dr["BIOS"]).Trim();

            string MD5 = MD5Utilities.CalcMD5(Check);
            if (MD5.ToLower() != logon.MoreMachineHash.ToLower())
            {
                dr.Close();
                NetworkConnection.DeleteSession(newID);
                RD.Worked = false;
                return (RESTStatus.Fail);
            }

            RD.MachineID = Convert.ToString(dr["MachineID"]);
            RD.MachinePassword = Convert.ToString(dr["Password"]);
            RD.Worked = true;

            dr.Close();

            NetworkConnection.DeleteSession(newID);
            return (RESTStatus.Success);
        }
    }
}
