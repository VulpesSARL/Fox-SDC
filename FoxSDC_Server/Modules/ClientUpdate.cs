using FoxSDC_Common;
using FoxSDC_Server.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class ClientUpdate
    {
        [VulpesRESTfulRet("Version")]
        public NetInt64 Version;

        [VulpesRESTfulRet("EarlyVersion")]
        public NetInt64 EarlyVersion;

        static Int64? CurrentVersion = null;
        static Int64? CurrentEarlyVersion = null;
        static DateTime? DTNormalModified = null;
        static DateTime? DTEarlyModified = null;


        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/update/version", "Version", "")]
        public RESTStatus GetUpdateVersion(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (CurrentVersion == null)
                return (RESTStatus.NotFound);

            Version = new NetInt64();
            Version.Data = CurrentVersion.Value;

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/earlyupdate/version", "EarlyVersion", "")]
        public RESTStatus GetEarlyUpdateVersion(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (CurrentEarlyVersion == null)
                return (RESTStatus.NotFound);

            EarlyVersion = new NetInt64();
            EarlyVersion.Data = CurrentEarlyVersion.Value;

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/update/package", "", "id", true, true, true)]
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

            string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (AppPath.EndsWith("\\") == false)
                AppPath += "\\";
            AppPath += "Packages\\";

            return (ProvideUpdatePackage(AppPath + "SDCA.foxpkg", request, response, ni));
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/earlyupdate/package", "", "id", true, true, true)]
        public RESTStatus GetEarlyPackage(SQLLib sql, HttpListenerRequest request, HttpListenerResponse response, object dummy, NetworkConnectionInfo ni, Int64 id)
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

            string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (AppPath.EndsWith("\\") == false)
                AppPath += "\\";
            AppPath += "Packages\\";

            return (ProvideUpdatePackage(AppPath + "SDCA-Early.foxpkg", request, response, ni));
        }

        RESTStatus ProvideUpdatePackage(string Filename, HttpListenerRequest request, HttpListenerResponse response, NetworkConnectionInfo ni)
        {
            if (File.Exists(Filename) == false)
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

            Downloader.ReadFileChunked(Filename, request, response);

            return (RESTStatus.Success);
        }

        #region Filesystem Checks & Version checks
        public static void ReadCheckVersions()
        {
            string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (AppPath.EndsWith("\\") == false)
                AppPath += "\\";
            AppPath += "Packages\\";

            string NormalUpdate = AppPath + "SDCA.foxpkg";
            string EarlyUpdate = AppPath + "SDCA-Early.foxpkg";

            bool needUpdate = false;

            if (File.Exists(NormalUpdate) == false)
            {
                if (DTNormalModified != null)
                {
                    DTNormalModified = null;
                    needUpdate = true;
                }
            }
            else
            {
                if (DTNormalModified != File.GetLastWriteTimeUtc(NormalUpdate))
                {
                    needUpdate = true;
                    DTNormalModified = File.GetLastWriteTimeUtc(NormalUpdate);
                }
            }

            if (File.Exists(EarlyUpdate) == false)
            {
                if (DTEarlyModified != null)
                {
                    DTEarlyModified = null;
                    needUpdate = true;
                }
            }
            else
            {
                if (DTEarlyModified != File.GetLastWriteTimeUtc(EarlyUpdate))
                {
                    needUpdate = true;
                    DTEarlyModified = File.GetLastWriteTimeUtc(EarlyUpdate);
                }
            }

            if (needUpdate == true)
            {
                ReadVersions();
                needUpdate = false;
            }
        }

        static void ReadVersions()
        {
            string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (AppPath.EndsWith("\\") == false)
                AppPath += "\\";
            AppPath += "Packages\\";

            string NormalUpdate = AppPath + "SDCA.foxpkg";
            string EarlyUpdate = AppPath + "SDCA-Early.foxpkg";

            string Error;

            if (File.Exists(NormalUpdate) == false)
            {
                FoxEventLog.WriteEventLog("No client updates will be provided. The file " + NormalUpdate + " is missing.",
                     System.Diagnostics.EventLogEntryType.Error);
                CurrentVersion = CurrentEarlyVersion = null;
                return;
            }

            PackageInstaller pkg = new PackageInstaller();
            PKGStatus status;
            PKGRecieptData receipt;

            Int64? NormalVersion = null;
            Int64? EarlyVersion = null;

            try
            {
                if (pkg.InstallPackage(NormalUpdate, null, PackageInstaller.InstallMode.TestPackageOnly, false, out Error, out status, out receipt) == false)
                {
                    FoxEventLog.WriteEventLog("No client updates will be provided. The file " + NormalUpdate + " is invalid / invalid signatures.",
                         System.Diagnostics.EventLogEntryType.Error);
                    CurrentVersion = CurrentEarlyVersion = null;
                    return;
                }

                if (pkg.PackageInfo(NormalUpdate, null, out Error) == false)
                {
                    FoxEventLog.WriteEventLog("No client updates will be provided. The file " + NormalUpdate + " is invalid / invalid info.",
                         System.Diagnostics.EventLogEntryType.Error);
                    CurrentVersion = CurrentEarlyVersion = null;
                    return;
                }

                if (pkg.PackageInfoData.PackageID != "Vulpes-SDCA1-Update")
                {
                    FoxEventLog.WriteEventLog("No client updates will be provided. The file " + NormalUpdate + " is has an invalid identifier.",
                         System.Diagnostics.EventLogEntryType.Error);
                    CurrentVersion = CurrentEarlyVersion = null;
                    return;
                }
            }
            catch
            {
                NormalVersion = null;
                EarlyVersion = null;
                CurrentVersion = NormalVersion;
                CurrentEarlyVersion = EarlyVersion;
                FoxEventLog.WriteEventLog("Update packages contains errors. No updates will be provided.", System.Diagnostics.EventLogEntryType.Error);
                return;
            }

            NormalVersion = pkg.PackageInfoData.VersionID;

            if (File.Exists(EarlyUpdate) == true)
            {
                if (pkg.InstallPackage(EarlyUpdate, null, PackageInstaller.InstallMode.TestPackageOnly, false, out Error, out status, out receipt) == true)
                {
                    if (pkg.PackageInfo(EarlyUpdate, null, out Error) == true)
                    {
                        if (pkg.PackageInfoData.PackageID == "Vulpes-SDCA1-Update")
                        {
                            EarlyVersion = pkg.PackageInfoData.VersionID;

                            if (EarlyVersion < NormalVersion)
                            {
                                FoxEventLog.WriteEventLog("The version ID of the file " + EarlyUpdate + " is older than the normal version - not deploying.",
                                     System.Diagnostics.EventLogEntryType.Warning);
                                EarlyVersion = null;
                            }
                            else
                            {
                                FoxEventLog.WriteEventLog("The file " + EarlyUpdate + " will be deployed along with the normal updates.",
                                     System.Diagnostics.EventLogEntryType.Information);
                            }
                        }
                    }
                }
            }

            CurrentVersion = NormalVersion;
            CurrentEarlyVersion = EarlyVersion;

            FoxEventLog.WriteEventLog("The Agent update system is working normally.\r\nVersion: " + CurrentVersion.ToString() + "\r\nEarly Version: " + CurrentEarlyVersion.ToString(), System.Diagnostics.EventLogEntryType.Information);
        }
        #endregion
    }
}