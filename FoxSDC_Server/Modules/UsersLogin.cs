using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class Users
    {
        static bool LDAPLogin(string LDAPUsername, string Password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                bool isValid = pc.ValidateCredentials(LDAPUsername, Password);
                if (isValid == true)
                    return (true);
            }

            return (false);
        }

        [VulpesRESTfulRet("Err")]
        ErrorInfo Err;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/login/computer", "Err", "", false, PutIPAddress = true)]
        public RESTStatus ComputerLogin(SQLLib sql, ComputerLogon logon, NetworkConnectionInfo ni, string IPAddress)
        {
            Err = new ErrorInfo();

            if (logon == null)
            {
                Err.Error = "Faulty data";
                Err.ErrorID = (int)ErrorFlags.FaultyData;
                return (RESTStatus.Fail);
            }

            if (NullTest.Test(logon) == false)
            {
                Err.Error = "Invalid data";
                Err.ErrorID = (int)ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (logon.SysInfo != null)
            {
                if (logon.SysInfo.BIOSType == null)
                    logon.SysInfo.BIOSType = "N/A";
                if (logon.SysInfo.CPUName == null)
                    logon.SysInfo.CPUName = "N/A";
                if (logon.SysInfo.SecureBootState == null)
                    logon.SysInfo.SecureBootState = "N/A";
                if (logon.SysInfo.ComputerModel == null)
                    logon.SysInfo.ComputerModel = "N/A";
                if (logon.SysInfo.ComputerModel.Trim() == "")
                    logon.SysInfo.ComputerModel = "N/A";
                if (logon.SysInfo.SystemRoot == null)
                    logon.SysInfo.SystemRoot = "C:\\Windows";
                if (logon.SysInfo.SUSID == null)
                    logon.SysInfo.SUSID = "";
                if (logon.SysInfo.SUSID.Trim() == "")
                    logon.SysInfo.SUSID = Consts.NullGUID;
                if (logon.SysInfo.LegacyUCID == null)
                    logon.SysInfo.LegacyUCID = Consts.NullUCID;
            }

            if (NullTest.Test(logon.SysInfo, "IsMeteredConnection") == false)
            {
                Err.Error = "Invalid data";
                Err.ErrorID = (int)ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (logon.Password == "" && logon.Username == "")
            {
                Err.Error = "Invalid data";
                Err.ErrorID = (int)ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (NullTest.TestBlankString(logon.SysInfo, "IsMeteredConnection") == false)
            {
                Err.Error = "Invalid data";
                Err.ErrorID = (int)ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            Guid testguid;

            if (Guid.TryParse(logon.Username, out testguid) == false)
            {
                Err.Error = "Invalid username";
                Err.ErrorID = (int)ErrorFlags.InvalidUsername;
                return (RESTStatus.Fail);
            }

            if (Guid.TryParse(logon.Password, out testguid) == false)
            {
                Err.Error = "Invalid password";
                Err.ErrorID = (int)ErrorFlags.InvalidPassword;
                return (RESTStatus.Fail);
            }

            if (logon.SysInfo.UCID.Trim().Length != 32)
            {
                Err.Error = "Invalid UCID";
                Err.ErrorID = (int)ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (logon.ContractID == null)
                logon.ContractID = "";
            if (logon.ContractPassword == null)
                logon.ContractPassword = "";

            if (Fox_LicenseGenerator.SDCLicensing.ValidLicense == false)
            {
                FoxEventLog.WriteEventLog("Licensing error: no valid license found. Client login are rejected.", EventLogEntryType.Warning);
                Err.Error = "Licensing error";
                Err.ErrorID = (int)ErrorFlags.LicensingError;
                return (RESTStatus.Fail);
            }

            if (Fox_LicenseGenerator.SDCLicensing.TestExpiry() == false)
            {
                FoxEventLog.WriteEventLog("Licensing error: license expired. Client login are rejected.", EventLogEntryType.Warning);
                Err.Error = "Licensing error";
                Err.ErrorID = (int)ErrorFlags.LicensingError;
                return (RESTStatus.Fail);
            }

            if (Settings.Default.UseContract == true)
            {
                if (logon.ContractID == "")
                {
                    Err.Error = "Invalid Contract Data";
                    Err.ErrorID = (int)ErrorFlags.FaultyContractData;
                    return (RESTStatus.Fail);
                }
                if (logon.ContractPassword == "")
                {
                    Err.Error = "Invalid Contract Data";
                    Err.ErrorID = (int)ErrorFlags.FaultyContractData;
                    return (RESTStatus.Fail);
                }
            }

            logon.Username = logon.Username.Trim().ToUpper();
            logon.SysInfo.MachineID = logon.SysInfo.MachineID.Trim().ToUpper();

            if (logon.Username != logon.SysInfo.MachineID)
            {
                Err.Error = "Invalid Username/MachineID";
                Err.ErrorID = (int)ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            string newID = NetworkConnection.NewSession();
            ni = NetworkConnection.GetSession(newID);
            if (NetworkConnectionProcessor.InitNi(ni) == false)
            {
                NetworkConnection.DeleteSession(newID);
                Err.Error = "System Error";
                Err.ErrorID = (int)ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }
            sql = ni.sql;

            ni.Permissions = 0;
            ni.Error = "";
            ni.ErrorID = ErrorFlags.NoError;

            if (Fox_LicenseGenerator.SDCLicensing.NumComputers != null)
            {
                Int64 Computers = Convert.ToInt64(sql.ExecSQLScalar("select count(*) from ComputerAccounts where Accepted=1"));
                if (Computers > Fox_LicenseGenerator.SDCLicensing.NumComputers.Value)
                {
                    NetworkConnection.DeleteSession(newID);
                    FoxEventLog.WriteEventLog("Licensing error: too many computers are accepted. Client login are rejected.\n" +
                        "Lic=" + Fox_LicenseGenerator.SDCLicensing.NumComputers.Value.ToString() + " Listed=" + Computers.ToString(),
                        EventLogEntryType.Warning);
                    Err.Error = "Licensing error";
                    Err.ErrorID = (int)ErrorFlags.LicensingError;
                    return (RESTStatus.Fail);
                }
            }

            if (Settings.Default.UseContract == true)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT Count(*) FROM Contracts WHERE ContractID=@id AND ContractPassword=@pw AND Disabled=0",
                new SQLParam("@id", logon.ContractID),
                new SQLParam("@pw", logon.ContractPassword))) == 0)
                {
                    NetworkConnection.DeleteSession(newID);
                    Err.Error = "Invalid/Disabled Contract Data";
                    Err.ErrorID = (int)ErrorFlags.FaultyContractData;
                    return (RESTStatus.Fail);
                }
            }

            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                new SQLParam("@m", logon.Username))) == 0)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE UCID=@u",
                    new SQLParam("@u", logon.SysInfo.UCID.Trim()))) > 0)
                {
                    NetworkConnection.DeleteSession(newID);
                    Err.Error = "UCID used for a different machine";
                    Err.ErrorID = (int)ErrorFlags.UCIDissues;
                    return (RESTStatus.Fail);
                }

                if (Settings.Default.UseContract == true)
                {
                    object o = sql.ExecSQLScalar("Select MaxComputers FROM Contracts WHERE ContractID=@cid", new SQLParam("@cid", logon.ContractID));
                    if (!(o is DBNull))
                    {
                        int i;
                        if (int.TryParse(o.ToString(), out i) == true)
                        {
                            int cnt = Convert.ToInt32(sql.ExecSQL("Select count(*) FROM ComputerAccounts WHERE ContractID=@cid", new SQLParam("@cid", logon.ContractID)));
                            if (cnt + 1 > i)
                            {
                                NetworkConnection.DeleteSession(newID);
                                Err.Error = "Number of computers exhausted";
                                Err.ErrorID = (int)ErrorFlags.ContractNumComputersExhausted;
                                return (RESTStatus.Fail);
                            }
                        }
                    }
                }

                sql.InsertMultiData("ComputerAccounts",
                    new SQLData("MachineID", logon.SysInfo.MachineID),
                    new SQLData("UCID", logon.SysInfo.UCID),
                    new SQLData("Password", logon.Password),
                    new SQLData("Is64Bit", logon.SysInfo.Is64Bit),
                    new SQLData("OSName", logon.SysInfo.OSName),
                    new SQLData("OSVerMaj", logon.SysInfo.OSVerMaj),
                    new SQLData("OSVerMin", logon.SysInfo.OSVerMin),
                    new SQLData("OSVerBuild", logon.SysInfo.OSVerBuild),
                    new SQLData("OSSuite", logon.SysInfo.OSSuite),
                    new SQLData("IsTSE", logon.SysInfo.IsTSE),
                    new SQLData("CPU", logon.SysInfo.CPU),
                    new SQLData("ComputerModel", logon.SysInfo.ComputerModel),
                    new SQLData("ComputerName", logon.SysInfo.ComputerName),
                    new SQLData("Language", logon.SysInfo.Language),
                    new SQLData("DisplayLanguage", logon.SysInfo.DisplayLanguage),
                    new SQLData("AgentVersion", logon.SysInfo.AgentVersion),
                    new SQLData("AgentVersionID", logon.SysInfo.AgentVersionID),
                    new SQLData("OSVerType", logon.SysInfo.OSVerType),
                    new SQLData("RunningInHypervisor", logon.SysInfo.RunningInHypervisor),
                    new SQLData("BIOS", logon.SysInfo.BIOS),
                    new SQLData("BIOSType", logon.SysInfo.BIOSType),
                    new SQLData("NumberOfLogicalProcessors", logon.SysInfo.NumberOfLogicalProcessors),
                    new SQLData("NumberOfProcessors", logon.SysInfo.NumberOfProcessors),
                    new SQLData("TotalPhysicalMemory", logon.SysInfo.TotalPhysicalMemory),
                    new SQLData("CPUName", logon.SysInfo.CPUName),
                    new SQLData("SecureBootState", logon.SysInfo.SecureBootState),
                    new SQLData("IPAddress", IPAddress),
                    new SQLData("SystemRoot", logon.SysInfo.SystemRoot),
                    new SQLData("SUSID", logon.SysInfo.SUSID),
                    new SQLData("MeteredConnection", logon.SysInfo.IsMeteredConnection),
                    new SQLData("ContractID", Settings.Default.UseContract == true ? (object)logon.ContractID : DBNull.Value));

                Err.Error = "Not accepted (computer registered)";
                Err.ErrorID = (int)ErrorFlags.NotAccepted;
                NetworkConnection.DeleteSession(newID);
                return (RESTStatus.Fail);
            }

            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m AND UCID=@ucid",
                new SQLParam("@m", logon.Username),
                new SQLParam("@ucid", logon.SysInfo.UCID.Trim()))) == 0)
            {
                #region Remove in future
                if (logon.SysInfo.LegacyUCID != Consts.NullUCID && logon.SysInfo.LegacyUCID.Trim().Length == 32)
                {
                    if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE UCID=@u AND MachineID=@m AND Password=@p",
                        new SQLParam("@u", logon.SysInfo.LegacyUCID.Trim()),
                        new SQLParam("@p", logon.Password),
                        new SQLParam("@m", logon.Username))) > 0)
                    {
                        sql.ExecSQL("UPDATE ComputerAccounts SET UCID=@nu WHERE UCID=@u AND MachineID=@m",
                            new SQLParam("@nu", logon.SysInfo.UCID.Trim()),
                            new SQLParam("@u", logon.SysInfo.LegacyUCID.Trim()),
                            new SQLParam("@m", logon.Username));

                        FoxEventLog.WriteEventLog("UCID changed:\nMachineID: " + logon.Username + "\nName: " + logon.SysInfo.ComputerName + "\nLUCID: " + logon.SysInfo.LegacyUCID + "\nUCID: " + logon.SysInfo.UCID, EventLogEntryType.Warning);

                        Err.Error = "UCID changed!";
                        Err.ErrorID = (int)ErrorFlags.NotAccepted;
                        NetworkConnection.DeleteSession(newID);
                        return (RESTStatus.Fail);
                    }
                }
                #endregion

                Err.Error = "MachineID & UCID do not match";
                Err.ErrorID = (int)ErrorFlags.MachineUCIDmissmatch;
                NetworkConnection.DeleteSession(newID);
                return (RESTStatus.Fail);
            }

            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m AND Password=@p",
                new SQLParam("@m", logon.Username),
                new SQLParam("@p", logon.Password))) == 0)
            {
                Err.Error = "Invalid password";
                Err.ErrorID = (int)ErrorFlags.InvalidPassword;
                NetworkConnection.DeleteSession(newID);
                return (RESTStatus.Fail);
            }

            if (Settings.Default.UseContract == true)
            {
                string CurrentContractID = Convert.ToString(sql.ExecSQLScalar("SELECT ContractID FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", logon.Username)));
                if (CurrentContractID == null)
                    CurrentContractID = "";
                if (CurrentContractID != "")
                {
                    if (CurrentContractID.Trim().ToLower() != logon.ContractID.Trim().ToLower())
                    {
                        Err.Error = "Invalid Contract Data";
                        Err.ErrorID = (int)ErrorFlags.FaultyContractData;
                        NetworkConnection.DeleteSession(newID);
                        return (RESTStatus.Fail);
                    }
                }

                object dt;
                DateTime dtt;

                dt = sql.ExecSQLScalar("Select ValidFrom FROM Contracts WHERE ContractID=@cid", new SQLParam("@cid", CurrentContractID));
                if (!(dt is DBNull))
                {
                    try
                    {
                        dtt = SQLLib.GetDTUTC(dt);
                        if (DateTime.UtcNow < dtt)
                        {
                            Err.Error = "Contract not started / expired";
                            Err.ErrorID = (int)ErrorFlags.ContractNotStarted_Expired;
                            NetworkConnection.DeleteSession(newID);
                            return (RESTStatus.Fail);
                        }
                    }
                    catch
                    {
                        Err.Error = "Invalid Contract Data";
                        Err.ErrorID = (int)ErrorFlags.FaultyContractData;
                        NetworkConnection.DeleteSession(newID);
                        return (RESTStatus.Fail);
                    }
                }

                dt = sql.ExecSQLScalar("Select ValidTo FROM Contracts WHERE ContractID=@cid", new SQLParam("@cid", CurrentContractID));
                if (!(dt is DBNull))
                {
                    try
                    {
                        dtt = SQLLib.GetDTUTC(dt);
                        if (dtt < DateTime.UtcNow)
                        {
                            Err.Error = "Contract not started / expired";
                            Err.ErrorID = (int)ErrorFlags.ContractNotStarted_Expired;
                            NetworkConnection.DeleteSession(newID);
                            return (RESTStatus.Fail);
                        }
                    }
                    catch
                    {
                        Err.Error = "Invalid Contract Data";
                        Err.ErrorID = (int)ErrorFlags.FaultyContractData;
                        NetworkConnection.DeleteSession(newID);
                        return (RESTStatus.Fail);
                    }
                }
            }
            else
            {
                logon.ContractID = "";
                logon.ContractPassword = "";
            }

            sql.ExecSQL("UPDATE ComputerAccounts SET BIOS=@BIOS, OSVerType=@OSVerType, Is64Bit = @Is64Bit,OSName = @OSName,OSVerMaj = @OSVerMaj," +
                "OSVerMin = @OSVerMin,OSVerBuild = @OSVerBuild,OSSuite = @OSSuite,IsTSE = @IsTSE,CPU = @CPU,ComputerModel = @ComputerModel,ComputerName = @ComputerName," +
                "Language = @Language,DisplayLanguage = @DisplayLanguage,MachineID = @MachineID,LastUpdated = getutcdate(), AgentVersion=@AgentVersion, AgentVersionID=@AgentVersionID," +
                "RunningInHypervisor=@RunningInHypervisor, ContractID=@ContractID, IPAddress=@IPAddress,BIOSType=@BIOSType, NumberOfLogicalProcessors=@NumberOfLogicalProcessors, " +
                "NumberOfProcessors=@NumberOfProcessors, TotalPhysicalMemory=@TotalPhysicalMemory, CPUName=@CPUName, SecureBootState=@SecureBootState, " +
                "SystemRoot=@SystemRoot,SUSID=@SUSID,MeteredConnection=@meteredconnection " +
                "    WHERE MachineID = @MachineID",
                new SQLParam("@Is64Bit", logon.SysInfo.Is64Bit),
                new SQLParam("@OSName", logon.SysInfo.OSName),
                new SQLParam("@OSVerMaj", logon.SysInfo.OSVerMaj),
                new SQLParam("@OSVerMin", logon.SysInfo.OSVerMin),
                new SQLParam("@OSVerBuild", logon.SysInfo.OSVerBuild),
                new SQLParam("@OSSuite", logon.SysInfo.OSSuite),
                new SQLParam("@IsTSE", logon.SysInfo.IsTSE),
                new SQLParam("@CPU", logon.SysInfo.CPU),
                new SQLParam("@ComputerModel", logon.SysInfo.ComputerModel),
                new SQLParam("@ComputerName", logon.SysInfo.ComputerName),
                new SQLParam("@Language", logon.SysInfo.Language),
                new SQLParam("@DisplayLanguage", logon.SysInfo.DisplayLanguage),
                new SQLParam("@MachineID", logon.Username),
                new SQLParam("@AgentVersionID", logon.SysInfo.AgentVersionID),
                new SQLParam("@AgentVersion", logon.SysInfo.AgentVersion),
                new SQLParam("@OSVerType", logon.SysInfo.OSVerType),
                new SQLParam("@RunningInHypervisor", logon.SysInfo.RunningInHypervisor),
                new SQLParam("@BIOS", logon.SysInfo.BIOS),
                new SQLParam("@BIOSType", logon.SysInfo.BIOSType),
                new SQLParam("@NumberOfLogicalProcessors", logon.SysInfo.NumberOfLogicalProcessors),
                new SQLParam("@NumberOfProcessors", logon.SysInfo.NumberOfProcessors),
                new SQLParam("@TotalPhysicalMemory", logon.SysInfo.TotalPhysicalMemory),
                new SQLParam("@CPUName", logon.SysInfo.CPUName),
                new SQLParam("@SecureBootState", logon.SysInfo.SecureBootState),
                new SQLParam("@IPAddress", IPAddress),
                new SQLParam("@SystemRoot", logon.SysInfo.SystemRoot),
                new SQLParam("@SUSID", logon.SysInfo.SUSID),
                new SQLParam("@meteredconnection", logon.SysInfo.IsMeteredConnection),
                new SQLParam("@ContractID", Settings.Default.UseContract == true ? (object)logon.ContractID : DBNull.Value));

            sql.ExecSQL("UPDATE ComputerAccounts SET LastUpdated=getutcdate() WHERE MachineID=@m",
                new SQLParam("@m", logon.Username));

            int Acceptance = Convert.ToInt32(sql.ExecSQLScalar("SELECT Accepted FROM ComputerAccounts WHERE MachineID=@m",
                new SQLParam("@m", logon.Username)));

            switch (Acceptance)
            {
                case 1: //OK
                    break;
                default:
                    Err.Error = "Not accepted";
                    Err.ErrorID = (int)ErrorFlags.NotAccepted;
                    NetworkConnection.DeleteSession(newID);
                    return (RESTStatus.Fail);
            }

            ni.Username = logon.Username;
            ni.Name = logon.SysInfo.ComputerName;
            ni.EMail = "";
            ni.MustChangePassword = false;
            ni.LoggedIn = false;
            ni.ComputerLoggedIn = true;
            ni.Permissions = (Int64)ACLFlags.ComputerLogin;
            Debug.WriteLine("Computer: " + ni.Name + " logged in");
            Err.Error = "OK:" + newID;
            Err.ErrorID = (int)ErrorFlags.NoError;
            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/login/logoff", "Err", "", true, false)]
        public RESTStatus Logoff(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            ni.RWLock.EnterWriteLock();
            NetworkConnection.DeleteSession(ni.ID);
            ni.RWLock.ExitWriteLock();
            Err = new ErrorInfo();
            Err.Error = "OK";
            Err.ErrorID = (int)ErrorFlags.NoError;
            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/login/clone", "Err", "")]
        public RESTStatus CloneSession(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            string newID = NetworkConnection.NewSession();
            NetworkConnectionInfo cloneni = NetworkConnection.GetSession(newID);
            cloneni.Permissions = ni.Permissions;
            cloneni.LoggedIn = true;
            cloneni.Username = ni.Username;
            cloneni.Name = ni.Name;
            cloneni.EMail = ni.EMail;
            cloneni.FromClone = true;
            cloneni.IPAddress = ni.IPAddress;
            cloneni.IsLDAP = ni.IsLDAP;

            if (NetworkConnectionProcessor.InitNi(cloneni) == false)
            {
                NetworkConnection.DeleteSession(newID);
                Err.Error = "System Error";
                Err.ErrorID = (int)ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            Err = new ErrorInfo();
            Err.Error = "OK:" + newID;
            Err.ErrorID = (int)ErrorFlags.NoError;
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/login/user", "Err", "", false, PutIPAddress = true)]
        public RESTStatus Login(SQLLib sql, Logon logon, NetworkConnectionInfo ni, string IPAddress)
        {
            Err = new ErrorInfo();

            if (logon == null)
            {
                Err.Error = "Faulty data";
                Err.ErrorID = (int)ErrorFlags.FaultyData;
                return (RESTStatus.Fail);
            }

            string newID = NetworkConnection.NewSession();
            ni = NetworkConnection.GetSession(newID);
            if (NetworkConnectionProcessor.InitNi(ni) == false)
            {
                NetworkConnection.DeleteSession(newID);
                Err.Error = "System Error";
                Err.ErrorID = (int)ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }
            sql = ni.sql;

            ni.Permissions = 0;
            ni.Error = "";
            ni.ErrorID = ErrorFlags.NoError;

            string PWMD5REQ = Convert.ToBase64String(Encoding.Unicode.GetBytes(logon.Password));

            SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM Users WHERE Username=@u",
                new SQLParam("@u", logon.Username));
            if (dr == null)
            {
                Err.Error = "No DR";
                Err.ErrorID = (int)ErrorFlags.SQLError;
                NetworkConnection.DeleteSession(newID);
                return (RESTStatus.ServerError);
            }
            if (dr.HasRows == false)
            {
                dr.Close();
                Err.Error = "Invalid username/password";
                Err.ErrorID = (int)ErrorFlags.WrongUsernamePassword;
                NetworkConnection.DeleteSession(newID);
                return (RESTStatus.Fail);
            }
            dr.Read();
            if (Convert.ToInt32(dr["UseLDAP"]) == 1)
            {
                if (LDAPLogin(Convert.ToString(dr["LDAPUsername"]), logon.Password) == false)
                {
                    dr.Close();
                    Err.Error = "Invalid username/password";
                    Err.ErrorID = (int)ErrorFlags.WrongUsernamePassword;
                    NetworkConnection.DeleteSession(newID);
                    return (RESTStatus.Fail);
                }
                else
                {
                    ni.IsLDAP = true;
                }
            }
            else
            {
                if (Convert.ToString(dr["Password"]) != PWMD5REQ)
                {
                    dr.Close();
                    Err.Error = "Invalid username/password";
                    Err.ErrorID = (int)ErrorFlags.WrongUsernamePassword;
                    NetworkConnection.DeleteSession(newID);
                    return (RESTStatus.Fail);
                }
                else
                {
                    ni.IsLDAP = false;
                }
            }
            ni.Permissions = Convert.ToInt64(dr["Permissions"]);
            ni.Permissions = ni.Permissions & ~((Int64)ACLFlags.ComputerLogin);
            if (ACL.HasACL(ni.Permissions, ACLFlags.CanLogin) == false)
            {
                dr.Close();
                Err.Error = "Access denied";
                Err.ErrorID = (int)ErrorFlags.AccessDenied;
                NetworkConnection.DeleteSession(newID);
                return (RESTStatus.Fail);
            }

            ni.Username = Convert.ToString(dr["Username"]);
            ni.Name = Convert.ToString(dr["Name"]);
            ni.EMail = Convert.ToString(dr["EMail"]);
            ni.MustChangePassword = Convert.ToBoolean(dr["MustChangePassword"]);
            ni.LoggedIn = true;
            ni.IPAddress = IPAddress;
            Debug.WriteLine(ni.Username + " logged in");
            dr.Close();
            Err.Error = "OK:" + newID;
            Err.ErrorID = (int)ErrorFlags.NoError;

            return (RESTStatus.Success);
        }
    }
}
