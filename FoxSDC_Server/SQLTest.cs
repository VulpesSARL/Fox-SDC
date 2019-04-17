using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FoxSDC_Server
{
    class SQLTest
    {
        public const string DefaultPasswordHash = "MQAyADMANAA1ADYANwA4ADkAUgBFAFMARQBUADEAMgAzADQANQA2ADcAOAA5AA=="; //123456789RESET123456789 
        public const Int64 AllPermissions = 0xFFFFFFFFFFFFFFFL;

        static public SQLLib ConnectSQL(string ApplicationName, int TimeOut=600)
        {
            SQLLib sql = null;
            if (Settings.Default.DBType.ToLower() == "mssql")
            {
                sql = new SQLLib();
                sql.ApplicationName = ApplicationName;
                sql.SqlCommandTimeout = TimeOut;
                if (sql.ConnectDatabase(Settings.Default.DBServer, Settings.Default.DBDB, true) == false)
                    return (null);
                sql.SEHError = true;
            }
            if (Settings.Default.DBType.ToLower() == "localdb")
            {
                sql = new SQLLib();
                sql.ApplicationName = ApplicationName;
                sql.SqlCommandTimeout = TimeOut;
                if (sql.ConnectLocalDatabase(Settings.Default.DBLocalPath) == false)
                    return (null);
                sql.SEHError = true;
            }
            return (sql);
        }

        public static bool TestSettings(out string ErrorReason)
        {
            ErrorReason = "";
            try
            {
                if (Settings.Default.DataPath == "")
                {
                    ErrorReason = "Missing DataPath in Config Table";
                    return (false);
                }
            }
            catch
            {
                ErrorReason = "Cannot SELECT Table CONFIG. (DataPath)";
                return (false);
            }

            try
            {
                if (Directory.Exists(Settings.Default.DataPath) == false)
                {
                    ErrorReason = "DataPath does not exist: " + Settings.Default.DataPath;
                    return (false);
                }
            }
            catch
            {
                ErrorReason = "Invalid DataPath: " + Settings.Default.DataPath;
                return (false);
            }

            if (Settings.Default.DataPath.EndsWith("\\") == false)
                Settings.Default.DataPath += "\\";

            try
            {
                StreamWriter txtw = new StreamWriter(File.Open(Settings.Default.DataPath + "TestFile.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None));
                txtw.WriteLine("Test");
                txtw.Close();
                File.Delete(Settings.Default.DataPath + "TestFile.txt");
            }
            catch
            {
                ErrorReason = "Cannot properly read or write to DataPath: " + Settings.Default.DataPath;
                return (false);
            }

            return (true);
        }


        public static bool TestServer(out string ErrorReason, bool SkipSomeSanityChecks = false)
        {
            ErrorReason = "";

            SQLLib sql = null;

            try
            {
                if (Settings.Default.DBType.ToLower() == "mssql")
                {
                    if (Settings.Default.DBServer == "" || Settings.Default.DBDB == "")
                    {
                        ErrorReason = "Servername / DB missing";
                        return (false);
                    }
                    sql = new SQLLib();
                    sql.SqlCommandTimeout = 0; //waiting .... :-)
                    sql.ApplicationName = "Fox SDC Server [test/update connection]";
                    sql.ConnectionPooling = false;
                    sql.SEHError = true;
                    if (sql.ConnectDatabase(Settings.Default.DBServer, Settings.Default.DBDB, true) == false)
                    {
                        ErrorReason = "Cannot connect to the SQL Server";
                        return (false);
                    }
                }
                if (Settings.Default.DBType.ToLower() == "localdb")
                {
                    if (Settings.Default.DBLocalPath == "")
                    {
                        ErrorReason = "LocalDB path missing";
                        return (false);
                    }
                    sql = new SQLLib();
                    sql.SqlCommandTimeout = 0; //waiting .... :-)
                    sql.ApplicationName = "Fox SDC Server [test/update connection]";
                    sql.ConnectionPooling = false;
                    sql.SEHError = true;
                    if (sql.ConnectLocalDatabase(Settings.Default.DBLocalPath) == false)
                    {
                        ErrorReason = "Cannot connect to the SQL Server";
                        return (false);
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorReason = "Cannot connect to the SQL Server [SEH]\n" + ee.ToString();
                Debug.WriteLine(ee.ToString());
                return (false);
            }

            if (sql == null)
            {
                ErrorReason = "sql==null //is the SQL Profile properly defined in registry?\n";
                return (false);
            }

            string ServerVersion = Convert.ToString(sql.ExecSQLScalar("SELECT SERVERPROPERTY('productversion')"));
            string ServerVPart = ServerVersion.Split('.')[0].Trim();
            int ServerVPartI = 0;
            if (int.TryParse(ServerVPart, out ServerVPartI) == false)
            {
                sql.CloseConnection();
                ErrorReason = "Cannot read SQL Version";
                return (false);
            }
            if (ServerVPartI < 11)
            {
                sql.CloseConnection();
                ErrorReason = "Unsupported SQL Version - Minimum V.11 (SQL 2012)";
            }

            try
            {
                sql.ExecSQL("SELECT * FROM Config");
            }
            catch
            {
                sql.CloseConnection();
                ErrorReason = "Cannot SELECT Table CONFIG.";
                return (false);
            }

            try
            {
                if (Convert.ToString(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]='ID'")) != "FOXSDCv1")
                {
                    sql.CloseConnection();
                    ErrorReason = "Invalid ID in Config Table";
                    return (false);
                }
            }
            catch
            {
                sql.CloseConnection();
                ErrorReason = "Cannot SELECT Table CONFIG. (ID)";
                return (false);
            }

            while (DBUpdate.UpdateDB(sql) == false)
            {
                ;
            }

            sql.SEHError = true;

            try
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]='Version'")) != Program.DBVersion)
                {
                    sql.CloseConnection();
                    ErrorReason = "Invalid Version in Config Table";
                    return (false);
                }
            }
            catch
            {
                sql.CloseConnection();
                ErrorReason = "Cannot SELECT Table CONFIG. (Version)";
                return (false);
            }

            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Users WHERE Username='root'")) == 0)
            {
                FoxEventLog.WriteEventLog("Creating user ROOT", EventLogEntryType.Warning);
                sql.InsertMultiData("Users",
                    new SQLData("Username", "root"),
                    new SQLData("Name", "Root User"),
                    new SQLData("Password", DefaultPasswordHash),
                    new SQLData("Permissions", AllPermissions),
                    new SQLData("MustChangePassword", 1));
            }
            if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM Users WHERE Username='root' AND Permissions=@p", new SQLParam("@p", AllPermissions))) == 0)
            {
                FoxEventLog.WriteEventLog("Fixing user ROOT", EventLogEntryType.Warning);
                sql.ExecSQL("UPDATE Users SET Permissions=@p WHERE Username='root'", new SQLParam("@p", AllPermissions));
            }

            try
            {
                Guid g;
                if (Guid.TryParse(Convert.ToString(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]='GUID'")), out g) == false)
                {
                    FoxEventLog.WriteEventLog("Creating GUID", EventLogEntryType.Warning);
                    sql.ExecSQL("DELETE FROM Config WHERE [Key]='GUID'");
                    sql.ExecSQL("INSERT INTO Config VALUES ('GUID', @g)", new SQLParam("@g", Guid.NewGuid().ToString()));
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorReason = "Cannot verify GUID";
                return (false);
            }


            SettingsManager.LoadSettings(sql);

            //try
            //{
            //    string ret = Convert.ToString(sql.ExecSQLScalar("select dbo.FoxCreateSerial(1, 1)"));
            //}
            //catch (Exception ee)
            //{
            //    Debug.WriteLine(ee.ToString());
            //    ErrorReason = "Cannot execute FUNCTIONs in SQL";
            //    return (false);
            //}

            sql.CloseConnection();
            ErrorReason = "";
            return (true);
        }
    }
}
