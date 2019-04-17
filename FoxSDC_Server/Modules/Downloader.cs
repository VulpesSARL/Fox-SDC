using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class Downloader
    {
        [VulpesRESTfulRet("NewID")]
        public NewUploadDataID NewID;

        #region Uploader

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/upload/request", "", "")]
        public RESTStatus RequestUpload(SQLLib sql, UploadRequest request, NetworkConnectionInfo ni)
        {
            if (ni.Upload != null)
            {
                ni.Error = "Upload is already running";
                ni.ErrorID = ErrorFlags.UploadAlreadyRunning;
                return (RESTStatus.Fail);
            }

            if (request.FileType < 0 || request.FileType > FlagsConst.Document_Type_Max)
            {
                ni.Error = "Invalid type";
                ni.ErrorID = ErrorFlags.InvalidType;
                return (RESTStatus.Fail);
            }

            switch (request.FileType)
            {
                case FlagsConst.Document_Type_Package:
                    if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
                    {
                        ni.Error = "Access denied";
                        ni.ErrorID = ErrorFlags.AccessDenied;
                        return (RESTStatus.Denied);
                    }
                    break;
                    //case FlagsConst.Document_Type_Invoice:
                    //case FlagsConst.Document_Type_Voucher:
                    //case FlagsConst.Document_Type_ClientData:
                    //    {
                    //        ni.Error = "Cannot use this function";
                    //        return (false);
                    //    }
            }

            if (request.FileSize < 0 || request.FileSize > Consts.MaxFilesize)
            {
                ni.Error = "File too large";
                ni.ErrorID = ErrorFlags.FileTooLarge;
                return (RESTStatus.Fail);
            }

            UploadRunner run = new UploadRunner();
            run.Counter = 0;
            run.CurrentPosition = 0;
            run.Size = request.FileSize;
            run.FileType = request.FileType;
            if (request.FileType == FlagsConst.Document_Type_Package)
            {
                run.TempFilename = "TMP_" + Guid.NewGuid().ToString() + ".data";
                run.Data = File.Open(Settings.Default.DataPath + run.TempFilename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            }
            else
            {
                run.TempFilename = "";
                run.Data = new System.IO.MemoryStream();
            }
            run.TempName = request.TempName;
            ni.Upload = run;
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/upload/cancel", "", "")]
        public RESTStatus CancelUpload(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.Upload == null)
                return (RESTStatus.Success);
            if (ni.Upload.Data != null)
            {
                ni.Upload.Data.Close();
                ni.Upload.Data.Dispose();
                if (ni.Upload.TempFilename != "")
                {
                    try
                    {
                        CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + ni.Upload.TempFilename);
                    }
                    catch
                    {

                    }
                }
            }
            ni.Upload = null;
            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/upload/data", "", "")]
        public RESTStatus UploadData(SQLLib sql, UploadData data, NetworkConnectionInfo ni)
        {
            if (ni.Upload == null)
            {
                ni.Error = "Upload is not running";
                ni.ErrorID = ErrorFlags.UploadNotRunning;
                return (RESTStatus.Fail);
            }

            if (data.Size < 0)
            {
                ni.Error = "Negative SZ";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (data.Size > Consts.MaxFileChunk)
            {
                ni.Error = "Chunk too large";
                ni.ErrorID = ErrorFlags.ChunkTooLarge;
                return (RESTStatus.Fail);
            }

            if (data == null)
            {
                ni.Error = "No data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.Fail);
            }

            if (data.data.Length != data.Size)
            {
                ni.Error = "Faulty sizes";
                ni.ErrorID = ErrorFlags.FaultySizes;
                return (RESTStatus.Fail);
            }

            if (ni.Upload.CurrentPosition + data.Size > ni.Upload.Size)
            {
                ni.Error = "Too many data";
                ni.ErrorID = ErrorFlags.TooManyData;
                return (RESTStatus.Fail);
            }

            if (ni.Upload.Counter + 1 != data.Counter)
            {
                ni.Error = "Invalid count";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            if (MD5Utilities.CalcMD5(data.data).ToLower() != data.MD5.ToLower())
            {
                ni.Error = "MD5 error";
                ni.ErrorID = ErrorFlags.CheckSumError;
                return (RESTStatus.Fail);
            }

            ni.Upload.Data.Write(data.data, 0, data.data.Length);
            ni.Upload.Counter++;
            ni.Upload.CurrentPosition += data.Size;

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/upload/finalise", "NewID", "")]
        public RESTStatus FinaliseUpload(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.Upload == null)
            {
                ni.Error = "Upload is not running";
                ni.ErrorID = ErrorFlags.UploadNotRunning;
                return (RESTStatus.Fail);
            }

            if (ni.Upload.Data == null)
            {
                ni.Error = "No data";
                ni.ErrorID = ErrorFlags.NoData;
                return (RESTStatus.Fail);
            }

            if (ni.Upload.Size != ni.Upload.CurrentPosition)
            {
                ni.Error = "Upload not completed";
                ni.ErrorID = ErrorFlags.UploadNotCompleted;
                return (RESTStatus.Fail);
            }

            switch (ni.Upload.FileType)
            {
                case FlagsConst.Document_Type_Package:
                    {
                        ni.Upload.Data.Close();
                        PackageInstaller pki = new PackageInstaller();
                        string ErrorText;
                        PKGStatus res;
                        PKGRecieptData reciept;
                        List<byte[]> CER = PolicyCertificates.GetPolicyCertificates(sql);
                        if (pki.InstallPackage(Settings.Default.DataPath + ni.Upload.TempFilename, CER, PackageInstaller.InstallMode.TestPackageOnly, false, out ErrorText, out res, out reciept) == false)
                        {
                            try
                            {
                                CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + ni.Upload.TempFilename);
                            }
                            catch
                            {

                            }
                            ni.Error = "Package contains errors: " + ErrorText;
                            ni.ErrorID = ErrorFlags.FaultyData;
                            ni.Upload = null;
                            return (RESTStatus.Fail);
                        }


                        string GUID = Guid.NewGuid().ToString();
                        string Filename = "PKG_" + GUID + ".foxpkg";
                        string FilenameMeta = "PKG_" + GUID + ".meta.foxpkg";

                        File.Move(Settings.Default.DataPath + ni.Upload.TempFilename, Settings.Default.DataPath + Filename);

                        if (pki.CreateMetaDataPackage(Settings.Default.DataPath + Filename, Settings.Default.DataPath + FilenameMeta, out ErrorText) == false)
                        {
                            try
                            {
                                CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + Filename);
                            }
                            catch { }
                            try
                            {
                                CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + FilenameMeta);
                            }
                            catch { }
                            ni.Error = "Cannot create meta Package: " + ErrorText;
                            ni.ErrorID = ErrorFlags.FaultyData;
                            ni.Upload = null;
                            return (RESTStatus.Fail);
                        }


                        if (pki.PackageInfo(Settings.Default.DataPath + Filename, CER, out ErrorText) == false)
                        {
                            try
                            {
                                CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + Filename);
                            }
                            catch { }
                            try
                            {
                                CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + FilenameMeta);
                            }
                            catch { }
                            ni.Error = "Cannot read Package Info: " + ErrorText;
                            ni.ErrorID = ErrorFlags.FaultyData;
                            ni.Upload = null;
                            return (RESTStatus.Fail);
                        }

                        Int64? NewID = null;

                        lock (ni.sqllock)
                        {
                            NewID = sql.InsertMultiDataID("Packages",
                                new SQLData("PackageID", pki.PackageInfoData.PackageID),
                                new SQLData("Version", pki.PackageInfoData.VersionID),
                                new SQLData("Title", pki.PackageInfoData.Title),
                                new SQLData("Description", pki.PackageInfoData.Description),
                                new SQLData("Filename", Filename),
                                new SQLData("MetaFilename", FilenameMeta),
                                new SQLData("Size", ni.Upload.Size));
                        }

                        if (NewID == null)
                        {
                            try
                            {
                                CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + Filename);
                            }
                            catch { }
                            try
                            {
                                CommonUtilities.SpecialDeleteFile(Settings.Default.DataPath + FilenameMeta);
                            }
                            catch { }
                            ni.Error = "Error storing data";
                            return (RESTStatus.Fail);
                        }

                        this.NewID = new NewUploadDataID();
                        this.NewID.NewID = NewID.Value;
                        this.NewID.FileType = ni.Upload.FileType;

                        ni.Upload = null;


                        break;
                    }
                //case FlagsConst.Document_Type_Invoice:
                //case FlagsConst.Document_Type_Voucher:
                //case FlagsConst.Document_Type_ClientData:
                //    {
                //        ni.Error = "Cannot use this function";
                //        return (false);
                //    }
                default:
                    {
                        ni.Error = "Invalid type";
                        ni.ErrorID = ErrorFlags.InvalidType;
                        return (RESTStatus.Fail);
                    }
            }

            return (RESTStatus.Success);
        }

        #endregion

        #region Downloader
        public static void ReadFileChunked(string FSFilename, HttpListenerRequest Request, HttpListenerResponse Response)
        {
            FileInfo f;
            try
            {
                f = new FileInfo(FSFilename);
            }
            catch
            {
                Response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                Response.StatusCode = 500;
                Response.StatusDescription = "Server Error";
                return;
            }
            Int64 size = f.Length;
            Int64 from = 0;
            Int64 to = 0;

            if (Request.Headers["HTTP_RANGE"] != "" && Request.Headers["HTTP_RANGE"] != null)
            {
                string[] split1 = Request.Headers["HTTP_RANGE"].Split(new char[] { '=' }, StringSplitOptions.None);
                if (split1.Length < 2)
                    return;
                if (split1[0].Trim().ToLower() != "bytes")
                    return;
                string[] split2 = split1[1].Split(new char[] { '-' }, StringSplitOptions.None);
                if (split2.Length < 2)
                    return;
                if (split2[0].Trim() != "")
                {
                    if (Int64.TryParse(split2[0], out from) == false)
                        return;
                }
                else
                {
                    return;
                }
                if (split2[1].Trim() != "")
                {
                    if (Int64.TryParse(split2[1], out to) == false)
                        return;
                }
                else
                {
                    to = size - 1;
                }
                to++;
                if (to > size)
                    to = size;
                Int64 size2 = size - 1;
                Int64 new_length = size - from;
                Response.StatusCode = 206;
                Response.StatusDescription = "Partial Content";
                //Response.AddHeader("Content-Length", new_length.ToString());
                Response.AddHeader("Content-Range", "bytes " + from.ToString() + "-" + size2.ToString() + "/" + size.ToString());
            }
            else
            {
                to = size;
                Int64 size2 = to - 1;
                //Response.AddHeader("Content-Length", size.ToString());
            }

            Response.SendChunked = true;
            Response.AddHeader("Accept-Ranges", "bytes");

            Int64 chunksize = 10240;
            try
            {
                using (FileStream file = File.Open(FSFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    file.Seek(from, SeekOrigin.Begin);
                    while (file.Position < to /* (given) EOF*/)
                    {
                        Int64 nchunk = chunksize;
                        if (nchunk > to - file.Position)
                            nchunk = to - file.Position;
                        if (nchunk > 0)
                        {
                            byte[] buffer = new byte[nchunk];
                            file.Read(buffer, 0, (int)nchunk);
                            try
                            {
                                Response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                            catch (Exception ee)
                            {
                                Debug.WriteLine(ee.ToString());
                                file.Close();
                                return;
                            }
                        }
                    }
                    file.Close();
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return;
            }
        }

        #endregion
    }
}
