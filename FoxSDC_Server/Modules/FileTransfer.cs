using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class FileTransfer
    {
        static bool FileExists(SQLLib sql, Int64 id)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("Select count(*) FROM FileTransfers WHERE ID=@id",
                new SQLParam("@id", id))) == 0)
                return (false);
            return (true);
        }

        static bool FileExistsCompleted(SQLLib sql, Int64 id)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("Select count(*) FROM FileTransfers WHERE ID=@id AND Size=ProgressSize",
                new SQLParam("@id", id))) == 0)
                return (false);
            return (true);
        }

        static FileUploadData FillUploadData(SqlDataReader dr)
        {
            FileUploadData d = new FileUploadData();
            d.Direction = Convert.ToInt32(dr["Direction"]);
            d.FileLastModified = SQLLib.GetDTUTC(dr["FileLastModified"]);
            d.ID = Convert.ToInt64(dr["ID"]);
            d.LastUpdated = SQLLib.GetDTUTC(dr["DTUpdated"]);
            d.MachineID = Convert.ToString(dr["MachineID"]);
            d.MD5CheckSum = Convert.ToString(dr["MD5Sum"]);
            d.OverrideMeteredConnection = Convert.ToInt32(dr["OverrideMeteredConnection"]) == 1 ? true : false;
            d.ProgressSize = Convert.ToInt64(dr["ProgressSize"]);
            d.RemoteFileLocation = Convert.ToString(dr["RemoteFileLocation"]);
            d.Size = Convert.ToInt64(dr["Size"]);
            d.RequestOnly = Convert.ToInt32(dr["RequestOnly"]) == 1 ? true : false;
            return (d);
        }

        #region Server -> Agent

        [VulpesRESTfulRet("UploadedData")]
        FileUploadDataSigned UploadedData;

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/fileanydata", "UploadedData", "ID")]
        public RESTStatus GetFileDataAny(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 ID)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM FileTransfers WHERE ID=@id AND MachineID=@mid AND Direction in (0,1)",
                    new SQLParam("@mid", ni.Username),
                    new SQLParam("@id", ID));

                if (dr.HasRows == false)
                {
                    ni.Error = "Not found";
                    ni.ErrorID = ErrorFlags.InvalidID;
                    dr.Close();
                    return (RESTStatus.NotFound);
                }

                dr.Read();

                UploadedData = new FileUploadDataSigned();
                UploadedData.Data = FillUploadData(dr);
                dr.Close();
            }

            if (Certificates.Sign(UploadedData, SettingsManager.Settings.UseCertificate) == false)
            {
                FoxEventLog.WriteEventLog("Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                ni.Error = "Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate;
                ni.ErrorID = ErrorFlags.CannotSign;
                return (RESTStatus.ServerError);
            }


            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/filedata", "UploadedData", "ID")]
        public RESTStatus GetFileData(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 ID)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM FileTransfers WHERE ID=@id AND MachineID=@mid AND Size=ProgressSize AND Direction=0 AND RequestOnly=0",
                    new SQLParam("@mid", ni.Username),
                    new SQLParam("@id", ID));

                if (dr.HasRows == false)
                {
                    ni.Error = "Not found";
                    ni.ErrorID = ErrorFlags.InvalidID;
                    dr.Close();
                    return (RESTStatus.NotFound);
                }

                dr.Read();

                UploadedData = new FileUploadDataSigned();
                UploadedData.Data = FillUploadData(dr);
                dr.Close();
            }

            if (Certificates.Sign(UploadedData, SettingsManager.Settings.UseCertificate) == false)
            {
                FoxEventLog.WriteEventLog("Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                ni.Error = "Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate;
                ni.ErrorID = ErrorFlags.CannotSign;
                return (RESTStatus.ServerError);
            }


            return (RESTStatus.Success);
        }

        public static void DeleteAllFiles(SQLLib sql, string MachineID)
        {
            SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM FileTransfers WHERE MachineID=@m",
                new SQLParam("@m", MachineID));
            while (dr.Read())
            {
                string Filename = Settings.Default.DataPath + Convert.ToString(dr["ServerFile"]);

                if (File.Exists(Filename) == true)
                {
                    try
                    {
                        CommonUtilities.SpecialDeleteFile(Filename);
                    }
                    catch
                    { }
                }
            }
            dr.Close();

            sql.ExecSQL("DELETE FROM FileTransfers WHERE MachineID=@m", new SQLParam("@m", MachineID));
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/filecancelupload", "", "ID")]
        public RESTStatus CancelUploadAgentToServer(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 ID)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM FileTransfers WHERE ID=@id AND MachineID=@mid AND Direction in (0,1)",
                new SQLParam("@mid", ni.Username),
                new SQLParam("@id", ID))) == 0)
                {
                    ni.Error = "Invalid ID";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }
            }

            string Filename = null;

            lock (ni.sqllock)
            {
                Filename = Settings.Default.DataPath + Convert.ToString(sql.ExecSQLScalar("SELECT ServerFile FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", ni.Username),
                    new SQLParam("@id", ID)));
            }

            if (File.Exists(Filename) == true)
            {
                try
                {
                    CommonUtilities.SpecialDeleteFile(Filename);
                }
                catch
                { }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQLScalar("DELETE FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", ni.Username),
                    new SQLParam("@id", ID));
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/agent/fileappendupload", "", "")]
        public RESTStatus AppendUploadAgentToServer(SQLLib sql, FileUploadAppendData upload, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (upload == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (upload.Data.Length != upload.Size)
            {
                ni.Error = "Invalid SZ";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(upload.MD5) == true)
            {
                ni.Error = "No MD5";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (upload.MD5.ToLower() != MD5Utilities.CalcMD5(upload.Data).ToLower())
            {
                ni.Error = "MD5 Error";
                ni.ErrorID = ErrorFlags.CheckSumError;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM FileTransfers WHERE ID=@id AND MachineID=@mid AND [Size]!=[ProgressSize] AND Direction=1 AND RequestOnly=0",
                new SQLParam("@mid", ni.Username),
                new SQLParam("@id", upload.ID))) == 0)
                {
                    ni.Error = "Invalid ID";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }
            }
            Int64 TotalSZ = 0;
            Int64 ProgressSize = 0;
            lock (ni.sqllock)
            {
                TotalSZ = Convert.ToInt64(sql.ExecSQLScalar("SELECT [Size] FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                new SQLParam("@mid", ni.Username),
                new SQLParam("@id", upload.ID)));
                ProgressSize = Convert.ToInt64(sql.ExecSQLScalar("SELECT ProgressSize FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", ni.Username),
                    new SQLParam("@id", upload.ID)));
            }

            if (ProgressSize + upload.Size > TotalSZ)
            {
                ni.Error = "Too many data";
                ni.ErrorID = ErrorFlags.ChunkTooLarge;
                return (RESTStatus.Fail);
            }

            string Filename = null;
            lock (ni.sqllock)
            {
                Filename = Settings.Default.DataPath + Convert.ToString(sql.ExecSQLScalar("SELECT ServerFile FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                   new SQLParam("@mid", ni.Username),
                   new SQLParam("@id", upload.ID)));
            }

            if (File.Exists(Filename) == false)
            {
                if (ProgressSize > 0)
                {
                    ni.Error = "FS Error - Missing";
                    ni.ErrorID = ErrorFlags.FileSystemError;
                    return (RESTStatus.Fail);
                }
            }

            using (FileStream str = File.Open(Filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                str.Seek(0, SeekOrigin.End);
                str.Write(upload.Data, 0, upload.Size);
            }

            FileInfo fi = new FileInfo(Filename);
            if (ProgressSize + upload.Size != fi.Length)
            {
                ni.Error = "FS Error - Final SZ Error";
                ni.ErrorID = ErrorFlags.FileSystemError;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE FileTransfers SET ProgressSize=@psz, DTUpdated=getutcdate() WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", ni.Username),
                    new SQLParam("@psz", fi.Length),
                    new SQLParam("@id", upload.ID));
            }

            if (fi.Length == TotalSZ)
            {
                string MD5 = MD5Utilities.CalcMD5File(Filename);
                string MD5DB = "";

                lock (ni.sqllock)
                {
                    MD5DB = Convert.ToString(sql.ExecSQLScalar("SELECT MD5Sum FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                      new SQLParam("@mid", ni.Username),
                      new SQLParam("@id", upload.ID)));
                }
                if (MD5.ToLower() != MD5DB.ToLower())
                {
                    ni.Error = "Final MD5 error";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }

                lock (ni.sqllock)
                {
                    sql.ExecSQL("UPDATE FileTransfers SET Direction=2 WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", ni.Username),
                    new SQLParam("@id", upload.ID));
                }
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("Int64List")]
        NetInt64ListSigned Int64List;

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/fileidlist", "Int64List", "")]
        public RESTStatus GetFileList(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            Int64List = new NetInt64ListSigned();
            Int64List.data = new NetInt64List2();
            Int64List.data.data = new List<long>();

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT ID FROM FileTransfers WHERE MachineID=@mid AND ((Size=ProgressSize AND Direction=0) OR (Direction=1)) ORDER BY RequestOnly ASC, ID ASC",
                new SQLParam("@mid", ni.Username));
                while (dr.Read())
                {
                    Int64List.data.data.Add(Convert.ToInt64(dr["ID"]));
                }
                dr.Close();
            }

            if (Certificates.Sign(Int64List, SettingsManager.Settings.UseCertificate) == false)
            {
                FoxEventLog.WriteEventLog("Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                ni.Error = "Cannot sign policy with Certificate " + SettingsManager.Settings.UseCertificate;
                ni.ErrorID = ErrorFlags.CannotSign;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("NewID")]
        NetInt64 NewID;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/agent/filenewupload", "NewID", "")]
        public RESTStatus NewUploadAgentToServer(SQLLib sql, FileUploadData uploadreq, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (uploadreq == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            uploadreq.MachineID = ni.Username;
            uploadreq.ProgressSize = 0;
            if (uploadreq.Size <= 0)
            {
                ni.Error = "Wrong SZ";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrEmpty(uploadreq.MD5CheckSum) == true)
            {
                ni.Error = "No MD5";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrEmpty(uploadreq.RemoteFileLocation) == true)
            {
                ni.Error = "No Filename";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            NewID = new NetInt64();

            string GUID = Guid.NewGuid().ToString();
            string ServerFilename = "FILE_" + GUID + ".dat";

            Int64? id = null;

            lock (ni.sqllock)
            {
                id = sql.InsertMultiDataID("FileTransfers",
                    new SQLData("MachineID", uploadreq.MachineID),
                    new SQLData("RemoteFileLocation", uploadreq.RemoteFileLocation),
                    new SQLData("ServerFile", ServerFilename),
                    new SQLData("Direction", 1),
                    new SQLData("MD5Sum", uploadreq.MD5CheckSum),
                    new SQLData("Size", uploadreq.Size),
                    new SQLData("ProgressSize", 0),
                    new SQLData("DTUpdated", DateTime.UtcNow),
                    new SQLData("RequestOnly", 0),
                    new SQLData("OverrideMeteredConnection", uploadreq.OverrideMeteredConnection),
                    new SQLData("FileLastModified", uploadreq.FileLastModified));
            }

            if (id == null)
            {
                ni.Error = "DB error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.Fail);
            }

            NewID.Data = id.Value;

            return (RESTStatus.Success);
        }

        #endregion

        #region Server -> Management

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/filenewupload", "NewID", "")]
        public RESTStatus NewUploadMgmtToServer(SQLLib sql, FileUploadData uploadreq, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (uploadreq == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                if (Computers.MachineExists(sql, uploadreq.MachineID) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }
            }

            uploadreq.ProgressSize = 0;
            if (uploadreq.Size <= 0)
            {
                ni.Error = "Wrong SZ";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrEmpty(uploadreq.MD5CheckSum) == true)
            {
                ni.Error = "No MD5";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrEmpty(uploadreq.RemoteFileLocation) == true)
            {
                ni.Error = "No Filename";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            NewID = new NetInt64();

            string GUID = Guid.NewGuid().ToString();
            string ServerFilename = "FILE_" + GUID + ".dat";

            Int64? id = null;
            lock (ni.sqllock)
            {
                id = sql.InsertMultiDataID("FileTransfers",
                    new SQLData("MachineID", uploadreq.MachineID),
                    new SQLData("RemoteFileLocation", uploadreq.RemoteFileLocation),
                    new SQLData("ServerFile", ServerFilename),
                    new SQLData("Direction", 3),
                    new SQLData("MD5Sum", uploadreq.MD5CheckSum),
                    new SQLData("Size", uploadreq.Size),
                    new SQLData("ProgressSize", 0),
                    new SQLData("DTUpdated", DateTime.UtcNow),
                    new SQLData("RequestOnly", 0),
                    new SQLData("OverrideMeteredConnection", uploadreq.OverrideMeteredConnection),
                    new SQLData("FileLastModified", uploadreq.FileLastModified));
            }

            if (id == null)
            {
                ni.Error = "DB error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.Fail);
            }

            NewID.Data = id.Value;

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/filenewrequpload", "NewID", "")]
        public RESTStatus NewUploadReqAgentToServer(SQLLib sql, FileUploadData uploadreq, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (uploadreq == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                if (Computers.MachineExists(sql, uploadreq.MachineID) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }
            }

            if (string.IsNullOrEmpty(uploadreq.RemoteFileLocation) == true)
            {
                ni.Error = "No Filename";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            NewID = new NetInt64();

            Int64? id = null;
            lock (ni.sqllock)
            {
                id = sql.InsertMultiDataID("FileTransfers",
                    new SQLData("MachineID", uploadreq.MachineID),
                    new SQLData("RemoteFileLocation", uploadreq.RemoteFileLocation),
                    new SQLData("ServerFile", ""),
                    new SQLData("Direction", 1),
                    new SQLData("MD5Sum", ""),
                    new SQLData("Size", 0),
                    new SQLData("ProgressSize", 0),
                    new SQLData("DTUpdated", DateTime.UtcNow),
                    new SQLData("RequestOnly", 1),
                    new SQLData("OverrideMeteredConnection", uploadreq.OverrideMeteredConnection),
                    new SQLData("FileLastModified", DateTime.UtcNow));
            }
            if (id == null)
            {
                ni.Error = "DB error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.Fail);
            }

            NewID.Data = id.Value;

            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("UploadedData")]
        FileUploadData UploadedData2;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/filedata", "UploadedData2", "ID")]
        public RESTStatus GetFileDataMgmt(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 ID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT COUNT(*) FROM FileTransfers WHERE ID=@id",
                    new SQLParam("@id", ID));
                if (dr.HasRows == false)
                {
                    ni.Error = "Not found";
                    ni.ErrorID = ErrorFlags.InvalidID;
                    dr.Close();
                    return (RESTStatus.NotFound);
                }

                dr.Read();

                UploadedData2 = FillUploadData(dr);

                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/filecancelupload", "", "ID")]
        public RESTStatus CancelUpload(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 ID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM FileTransfers WHERE ID=@id",
                new SQLParam("@id", ID))) == 0)
                {
                    ni.Error = "Invalid ID";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }
            }


            string Filename = null;
            lock (ni.sqllock)
            {
                Filename = Settings.Default.DataPath + Convert.ToString(sql.ExecSQLScalar("SELECT ServerFile FROM FileTransfers WHERE ID=@id",
                    new SQLParam("@id", ID)));
            }

            if (File.Exists(Filename) == true)
            {
                try
                {
                    CommonUtilities.SpecialDeleteFile(Filename);
                }
                catch
                { }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQLScalar("DELETE FROM FileTransfers WHERE ID=@id",
                    new SQLParam("@id", ID));
            }
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/fileappendupload", "", "")]
        public RESTStatus AppendUpload(SQLLib sql, FileUploadAppendData upload, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (upload == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                if (Computers.MachineExists(sql, upload.MachineID) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.Fail);
                }
            }

            if (upload.Data.Length != upload.Size)
            {
                ni.Error = "Invalid SZ";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(upload.MD5) == true)
            {
                ni.Error = "No MD5";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (upload.MD5.ToLower() != MD5Utilities.CalcMD5(upload.Data).ToLower())
            {
                ni.Error = "MD5 Error";
                ni.ErrorID = ErrorFlags.CheckSumError;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM FileTransfers WHERE ID=@id AND MachineID=@mid AND [Size]!=[ProgressSize] AND Direction=3",
                new SQLParam("@mid", upload.MachineID),
                new SQLParam("@id", upload.ID))) == 0)
                {
                    ni.Error = "Invalid ID";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }
            }

            Int64 TotalSZ; Int64 ProgressSize;
            lock (ni.sqllock)
            {
                TotalSZ = Convert.ToInt64(sql.ExecSQLScalar("SELECT [Size] FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", upload.MachineID),
                    new SQLParam("@id", upload.ID)));
            }

            lock (ni.sqllock)
            {
                ProgressSize = Convert.ToInt64(sql.ExecSQLScalar("SELECT ProgressSize FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", upload.MachineID),
                    new SQLParam("@id", upload.ID)));
            }

            if (ProgressSize + upload.Size > TotalSZ)
            {
                ni.Error = "Too many data";
                ni.ErrorID = ErrorFlags.ChunkTooLarge;
                return (RESTStatus.Fail);
            }

            string Filename = null;

            lock (ni.sqllock)
            {
                Filename = Settings.Default.DataPath + Convert.ToString(sql.ExecSQLScalar("SELECT ServerFile FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", upload.MachineID),
                    new SQLParam("@id", upload.ID)));
            }

            if (File.Exists(Filename) == false)
            {
                if (ProgressSize > 0)
                {
                    ni.Error = "FS Error - Missing";
                    ni.ErrorID = ErrorFlags.FileSystemError;
                    return (RESTStatus.Fail);
                }
            }

            using (FileStream str = File.Open(Filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                str.Seek(0, SeekOrigin.End);
                str.Write(upload.Data, 0, upload.Size);
            }

            FileInfo fi = new FileInfo(Filename);
            if (ProgressSize + upload.Size != fi.Length)
            {
                ni.Error = "FS Error - Final SZ Error";
                ni.ErrorID = ErrorFlags.FileSystemError;
                return (RESTStatus.Fail);
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("UPDATE FileTransfers SET ProgressSize=@psz, DTUpdated=getutcdate() WHERE ID=@id AND MachineID=@mid",
                    new SQLParam("@mid", upload.MachineID),
                    new SQLParam("@psz", fi.Length),
                    new SQLParam("@id", upload.ID));
            }

            if (fi.Length == TotalSZ)
            {
                string MD5 = MD5Utilities.CalcMD5File(Filename);
                string MD5DB = "";
                lock (ni.sqllock)
                {
                    MD5DB = Convert.ToString(sql.ExecSQLScalar("SELECT MD5Sum FROM FileTransfers WHERE ID=@id AND MachineID=@mid",
                        new SQLParam("@mid", upload.MachineID),
                        new SQLParam("@id", upload.ID)));
                }
                if (MD5.ToLower() != MD5DB.ToLower())
                {
                    ni.Error = "Final MD5 error";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }

                lock (ni.sqllock)
                {
                    sql.ExecSQL("UPDATE FileTransfers SET Direction=0 WHERE ID=@id AND MachineID=@mid",
                        new SQLParam("@mid", upload.MachineID),
                        new SQLParam("@id", upload.ID));
                }
            }
            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("Int64List2")]
        NetInt64List Int64List2;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/fileidlist", "Int64List", "MachineID")]
        public RESTStatus GetFileListMgmt(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            Int64List2 = new NetInt64List();
            Int64List2.data = new List<long>();

            lock (ni.sqllock)
            {
                SqlDataReader dr;
                if (string.IsNullOrWhiteSpace(MachineID) == true)
                {
                    if (Computers.MachineExists(sql, MachineID) == false)
                    {
                        ni.Error = "Invalid data";
                        ni.ErrorID = ErrorFlags.InvalidValue;
                        return (RESTStatus.Fail);
                    }

                    dr = sql.ExecSQLReader("SELECT ID FROM FileTransfers WHERE MachineID=@mid ORDER BY ID",
                        new SQLParam("@mid", MachineID));
                }
                else
                {
                    dr = sql.ExecSQLReader("SELECT ID FROM FileTransfers ORDER BY ID");
                }

                while (dr.Read())
                {
                    Int64List2.data.Add(Convert.ToInt64(dr["ID"]));
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("FileUploadDataList")]
        FileUploadDataList FileUploadDataList;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/filefulllist", "FileUploadDataList", "MachineID")]
        public RESTStatus GetFullFileListMgmt(SQLLib sql, object dummy, NetworkConnectionInfo ni, string MachineID)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            FileUploadDataList = new FileUploadDataList();
            FileUploadDataList.List = new List<FileUploadData>();

            lock (ni.sqllock)
            {
                SqlDataReader dr;
                if (string.IsNullOrWhiteSpace(MachineID) == true)
                {
                    if (Computers.MachineExists(sql, MachineID) == false)
                    {
                        ni.Error = "Invalid data";
                        ni.ErrorID = ErrorFlags.InvalidValue;
                        return (RESTStatus.Fail);
                    }

                    dr = sql.ExecSQLReader("SELECT * FROM FileTransfers WHERE MachineID=@mid ORDER BY ID",
                        new SQLParam("@mid", MachineID));
                }
                else
                {
                    dr = sql.ExecSQLReader("SELECT * FROM FileTransfers ORDER BY ID");
                }

                while (dr.Read())
                {
                    FileUploadDataList.List.Add(FillUploadData(dr));
                }
                dr.Close();
            }

            return (RESTStatus.Success);
        }

        #endregion

        #region Server -> Agent or Management

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/filefiledownload", "", "id", true, true, true)]
        public RESTStatus GetFile(SQLLib sql, HttpListenerRequest request, HttpListenerResponse response, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false && ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;

                response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                response.StatusCode = 403;
                response.StatusDescription = "Forbidden";
                byte[] data = Encoding.UTF8.GetBytes("403 - Forbidden.");
                response.ContentLength64 = data.LongLength;
                Stream output = response.OutputStream;
                output.Write(data, 0, data.Length);

                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (FileExistsCompleted(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidID;

                    response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                    response.StatusCode = 404;
                    response.StatusDescription = "Not found";
                    byte[] data = Encoding.UTF8.GetBytes("404 - Not found.");
                    response.ContentLength64 = data.LongLength;
                    Stream output = response.OutputStream;
                    output.Write(data, 0, data.Length);

                    return (RESTStatus.NotFound);
                }
            }

            if (ni.HasAcl(ACLFlags.ComputerLogin) == true)
            {
                lock (ni.sqllock)
                {
                    if (Convert.ToString(sql.ExecSQLScalar("SELECT MachineID FROM FileTransfers WHERE ID=@id AND Size=ProgressSize AND (Direction=0 OR Direction=2)",
                    new SQLParam("@id", id))) != ni.Username)
                    {
                        ni.Error = "Invalid data";
                        ni.ErrorID = ErrorFlags.InvalidID;

                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 404;
                        response.StatusDescription = "Not found";
                        byte[] data = Encoding.UTF8.GetBytes("404 - Not found.");
                        response.ContentLength64 = data.LongLength;
                        Stream output = response.OutputStream;
                        output.Write(data, 0, data.Length);

                        return (RESTStatus.NotFound);
                    }
                }
            }

            string Filename = null;
            lock (ni.sqllock)
            {
                Filename = Convert.ToString(sql.ExecSQLScalar("SELECT ServerFile FROM FileTransfers WHERE ID=@id",
                    new SQLParam("@id", id)));
            }

            if (File.Exists(Settings.Default.DataPath + Filename) == false)
            {
                ni.Error = "Cannot find local file";
                ni.ErrorID = ErrorFlags.FileSystemError;

                response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                response.StatusCode = 500;
                response.StatusDescription = "Server Error";
                byte[] data = Encoding.UTF8.GetBytes("500 - Server Error.");
                response.ContentLength64 = data.LongLength;
                Stream output = response.OutputStream;
                output.Write(data, 0, data.Length);

                return (RESTStatus.ServerError);
            }

            Downloader.ReadFileChunked(Settings.Default.DataPath + Filename, request, response);

            return (RESTStatus.Success);
        }

        #endregion

    }
}
