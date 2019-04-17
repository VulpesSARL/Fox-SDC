using FoxSDC_Common;
using FoxSDC_Server.Modules;
using FoxSDC_Server.Pushes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class NetworkConnectionProcessor
    {
        static public bool InitNi(NetworkConnectionInfo ni)
        {
            if (ni == null)
                return (false);
            if (ni.Inited == true)
                return (true);
            ni.ServerInfo = new ServerInfo();
            ni.ServerInfo.Name = Environment.MachineName;
            ni.ServerInfo.ProtocolVersion = 1;
            ni.ServerInfo.ServerVersion = FoxVersion.DTS.ToString();
            if (ni.FromClone == false)
            {
                ni.LoggedIn = false;
                ni.ComputerLoggedIn = false;
                ni.Username = "";
                ni.Name = "";
                ni.EMail = "";
            }

            ni.sql = SQLTest.ConnectSQL("Fox SDC Server for " + ni.ID);
            ni.sql.SEHError = true;
            ni.ServerInfo.InitSuccess = ni.sql == null ? false : true;

            ni.Inited = true;
            ni.ServerInfo.ServerGUID = Convert.ToString(ni.sql.ExecSQLScalar("SELECT [Value] from Config WHERE [Key]='GUID'"));

            ni.ServerInfo.SQLServer = "N/A";
            ni.ServerInfo.SQLService = "N/A";
            ni.ServerInfo.SQLCollation = "N/A";
            ni.ServerInfo.SQLEdition = "N/A";
            ni.ServerInfo.SQLProductVersion = "N/A";
            ni.ServerInfo.SQLProductLevel = "N/A";
            ni.ServerInfo.SQLProductName = "N/A";

            ni.ServerInfo.LicFeatures = "N/A";
            ni.ServerInfo.LicLicenseID = "N/A";
            ni.ServerInfo.LicLicenseType = "N/A";
            ni.ServerInfo.LicOwner = "N/A";
            ni.ServerInfo.LicOwnerCustomID = "N/A";
            ni.ServerInfo.LicSupportValidTo = null;
            ni.ServerInfo.LicUCID = "N/A";
            ni.ServerInfo.LicVacant1 = "N/A";
            ni.ServerInfo.LicVacant2 = "N/A";
            ni.ServerInfo.LicVacant3 = "N/A";
            ni.ServerInfo.LicVacant4 = "N/A";
            ni.ServerInfo.LicVacant5 = "N/A";
            ni.ServerInfo.LicValidFrom = new DateTime(2000, 1, 1, 0, 0, 0, 0);
            ni.ServerInfo.LicValidTo = null;

            if (Settings.Default.CensorSQLInformations == false)
            {
                ni.ServerInfo.SQLServer = Convert.ToString(ni.sql.ExecSQLScalar("select @@SERVERNAME"));
                ni.ServerInfo.SQLService = Convert.ToString(ni.sql.ExecSQLScalar("select @@SERVICENAME"));
                ni.ServerInfo.SQLCollation = Convert.ToString(ni.sql.ExecSQLScalar("select SERVERPROPERTY ('Collation')"));
                ni.ServerInfo.SQLEdition = Convert.ToString(ni.sql.ExecSQLScalar("select SERVERPROPERTY ('edition')"));
                ni.ServerInfo.SQLProductVersion = Convert.ToString(ni.sql.ExecSQLScalar("SELECT SERVERPROPERTY('productversion')"));
                ni.ServerInfo.SQLProductLevel = Convert.ToString(ni.sql.ExecSQLScalar("select SERVERPROPERTY ('productlevel')"));
                ni.ServerInfo.SQLProductName = Convert.ToString(ni.sql.ExecSQLScalar("SELECT LEFT(@@version, CHARINDEX(' - ', @@version))"));
            }

            if (Settings.Default.CensorLicInformations == false)
            {
                if (Fox_LicenseGenerator.SDCLicensing.Data != null)
                {
                    ni.ServerInfo.LicFeatures = Fox_LicenseGenerator.SDCLicensing.Data.Features;
                    ni.ServerInfo.LicLicenseID = Fox_LicenseGenerator.SDCLicensing.Data.LicenseID;
                    ni.ServerInfo.LicLicenseType = Fox_LicenseGenerator.SDCLicensing.Data.LicenseType;
                    ni.ServerInfo.LicOwner = Fox_LicenseGenerator.SDCLicensing.Data.Owner;
                    ni.ServerInfo.LicOwnerCustomID = Fox_LicenseGenerator.SDCLicensing.Data.OwnerCustomID;
                    ni.ServerInfo.LicSupportValidTo = Fox_LicenseGenerator.SDCLicensing.Data.SupportValidTo;
                    ni.ServerInfo.LicUCID = Fox_LicenseGenerator.SDCLicensing.Data.UCID;
                    ni.ServerInfo.LicVacant1 = Fox_LicenseGenerator.SDCLicensing.Data.Vacant1;
                    ni.ServerInfo.LicVacant2 = Fox_LicenseGenerator.SDCLicensing.Data.Vacant2;
                    ni.ServerInfo.LicVacant3 = Fox_LicenseGenerator.SDCLicensing.Data.Vacant3;
                    ni.ServerInfo.LicVacant4 = Fox_LicenseGenerator.SDCLicensing.Data.Vacant4;
                    ni.ServerInfo.LicVacant5 = Fox_LicenseGenerator.SDCLicensing.Data.Vacant5;
                    ni.ServerInfo.LicValidFrom = Fox_LicenseGenerator.SDCLicensing.Data.ValidFrom;
                    ni.ServerInfo.LicValidTo = Fox_LicenseGenerator.SDCLicensing.Data.ValidTo;
                }
                else
                {
                    ni.ServerInfo.LicOwner = "Error";
                }
            }

            return (true);
        }

        static public bool DeInitNi(NetworkConnectionInfo ni, bool KickPushService)
        {
            if (ni == null)
                return (true);
            if (ni.PushChannel != null)
            {
                if (KickPushService == true)
                    PushServiceHelper.DeletePushService(ni.Username, ni.PushChannel.Value);
            }

            Debug.WriteLine("DeInitNI " + ni.ID);
            try
            {
                if (ni.sql != null)
                    ni.sql.CloseConnection();
            }
            catch
            {

            }
            try
            {
                if (ni.Upload != null)
                    ni.Upload.Data.Close();
            }
            catch
            {

            }
            /*try
            {
                if (ni.Download != null)
                    ni.Download.Data.Close();
            }
            catch
            {

            }*/
            return (true);
        }
    }
}
