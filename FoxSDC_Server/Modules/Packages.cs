using FoxSDC_Common;
using FoxSDC_Server.Modules;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class Packages
    {
        [VulpesRESTfulRet("PackageData")]
        PackageData PackageData;

        [VulpesRESTfulRet("PackageDataSigned")]
        PackageDataSigned PackageDataSigned;

        [VulpesRESTfulRet("PackageDataList")]
        PackageDataList PackageDataList;

        public static bool PackageExists(SQLLib sql, Int64 id)
        {
            if (Convert.ToInt32(sql.ExecSQLScalar("Select count(*) FROM Packages WHERE ID=@id",
                new SQLParam("@id", id))) == 0)
                return (false);
            return (true);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/packages", "PackageData", "id")]
        public RESTStatus GetPackage(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (PackageExists(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidID;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("Select * FROM Packages WHERE ID=@id",
                new SQLParam("@id", id));
                dr.Read();

                PackageData = new PackageData();
                sql.LoadIntoClass(dr, PackageData);
                dr.Close();
            }
            return (RESTStatus.Success);
        }


        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/pkgsigned", "PackageDataSigned", "id")]
        public RESTStatus GetPackageSigned(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (PackageExists(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidID;
                    return (RESTStatus.NotFound);
                }
            }

            PackageData pp = null;
            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("Select * FROM Packages WHERE ID=@id",
                new SQLParam("@id", id));
                dr.Read();

                pp = new PackageData();
                sql.LoadIntoClass(dr, pp);
                dr.Close();
            }

            PackageDataSigned = new PackageDataSigned();
            PackageDataSigned.Package = pp;
            if (Certificates.Sign(PackageDataSigned, SettingsManager.Settings.UseCertificate) == false)
            {
                FoxEventLog.WriteEventLog("Cannot sign package element with Certificate " + SettingsManager.Settings.UseCertificate, System.Diagnostics.EventLogEntryType.Warning);
                ni.Error = "Cannot sign package element with Certificate " + SettingsManager.Settings.UseCertificate;
                ni.ErrorID = ErrorFlags.CannotSign;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }


        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/packages", "PackageDataList", "")]
        public RESTStatus GetPackages(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            PackageDataList = new PackageDataList();
            PackageDataList.Items = new List<FoxSDC_Common.PackageData>();

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("Select * FROM Packages");

                while (dr.Read())
                {
                    PackageData pd = new PackageData();
                    sql.LoadIntoClass(dr, pd);
                    PackageDataList.Items.Add(pd);
                }

                dr.Close();
            }
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.DELETE, "api/mgmt/packages", "", "id")]
        public RESTStatus DeletePackage(SQLLib sql, object dummy, NetworkConnectionInfo ni, Int64 id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (PackageExists(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidID;
                    return (RESTStatus.NotFound);
                }
            }

            string Filename = null;
            string MetaFilename = null;

            lock (ni.sqllock)
            {
                Filename = Convert.ToString(sql.ExecSQLScalar("SELECT Filename FROM Packages WHERE ID=@id",
                new SQLParam("@id", id)));
            }
            lock (ni.sqllock)
            {
                MetaFilename = Convert.ToString(sql.ExecSQLScalar("SELECT MetaFilename FROM Packages WHERE ID=@id",
                new SQLParam("@id", id)));
            }

            CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + Filename);
            CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + MetaFilename);

            lock (ni.sqllock)
            {
                sql.ExecSQL("DELETE FROM Packages WHERE ID=@id", new SQLParam("@id", id));
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/packagedata", "", "id", true, true, true)]
        public RESTStatus GetPackage(SQLLib sql, HttpListenerRequest request, HttpListenerResponse response, object dummy, NetworkConnectionInfo ni, Int64 id)
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
                if (PackageExists(sql, id) == false)
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

            string Filename = null;
            lock (ni.sqllock)
            {
                Filename = Convert.ToString(sql.ExecSQLScalar("SELECT Filename FROM Packages WHERE ID=@id",
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

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/agent/packagemetadata", "", "id", true, true, true)]
        public RESTStatus GetPackageMeta(SQLLib sql, HttpListenerRequest request, HttpListenerResponse response, object dummy, NetworkConnectionInfo ni, Int64 id)
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

            if (PackageExists(sql, id) == false)
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

            string MetaFilename = Convert.ToString(sql.ExecSQLScalar("SELECT MetaFilename FROM Packages WHERE ID=@id",
                new SQLParam("@id", id)));

            if (File.Exists(Settings.Default.DataPath + MetaFilename) == false)
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

            Downloader.ReadFileChunked(Settings.Default.DataPath + MetaFilename, request, response);

            return (RESTStatus.Success);
        }
    }
}
