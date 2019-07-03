using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    partial class DownloadSystemFSData
    {
        static object FileLock = new object();
        static bool StopService = false;
        static Thread RunningThread;
        static Thread RunningDLThread;

        static bool CancelAndDeleteDL = false;
        static Network downloadnet = null;
        static bool UnlockTimer = false;

        static void Wait(int Secs)
        {
            UnlockTimer = false;
            for (int i = 0; i < Secs; i++)
            {
                if (StopService == true)
                    break;
                if (UnlockTimer == true)
                    break;
                Thread.Sleep(1000);
            }
        }

        static bool roughDT(DateTime dt1, DateTime dt2)
        {
            return (dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day &&
                dt1.Hour == dt2.Hour && dt1.Minute == dt2.Minute && dt1.Second == dt2.Second);
        }

        static void ClearDataFSD()
        {
            FilesystemData.FileTransferStatus.RemoteFileLocation = "";
            FilesystemData.FileTransferStatus.ServerID = null;
            FilesystemData.FileTransferStatus.ProgressSize = 0;
            FilesystemData.FileTransferStatus.Size = 0;
            FilesystemData.FileTransferStatus.MD5CheckSum = "";
            FilesystemData.FileTransferStatus.OverrideMeteredConnection = false;
            FilesystemData.FileTransferStatus.RequestOnly = false;
            FilesystemData.FileTransferStatus.Direction = -1;
            FilesystemData.FileTransferStatus.LastModfied = new DateTime(1900, 1, 1, 0, 0, 0);
        }

        static void DownloadThreadRunner()
        {
            try
            {
                int Direction = -1;
                lock (FileLock)
                {
                    if (FilesystemData.FileTransferStatus.ServerID == null)
                        return;
                    Direction = FilesystemData.FileTransferStatus.Direction;
                }

                #region Client to Server

                if (Direction == 1)
                {
                    string LocalFilename = "";
                    string MD5;
                    Int64 ServerID = 0;
                    Int64 CurrentSZ;
                    Int64 TotalSZ;
                    bool OverrideMetered;
                    bool ReqOnly;
                    DateTime LastModified;
                    try
                    {
                        lock (FileLock)
                        {
                            LocalFilename = FilesystemData.FileTransferStatus.RemoteFileLocation;
                            ServerID = FilesystemData.FileTransferStatus.ServerID.Value;
                            CurrentSZ = FilesystemData.FileTransferStatus.ProgressSize;
                            TotalSZ = FilesystemData.FileTransferStatus.Size;
                            MD5 = FilesystemData.FileTransferStatus.MD5CheckSum;
                            OverrideMetered = FilesystemData.FileTransferStatus.OverrideMeteredConnection;
                            ReqOnly = FilesystemData.FileTransferStatus.RequestOnly;
                            LastModified = FilesystemData.FileTransferStatus.LastModfied;
                        }

                        //won't start when in metered connection!
                        if (OverrideMetered == false)
                        {
                            try
                            {
                                if (MeteredConnection.IsMeteredConnection() == true)
                                {
                                    FoxEventLog.VerboseWriteEventLog("Upload paused = metered connection detected", System.Diagnostics.EventLogEntryType.Information);
                                    return;
                                }
                            }
                            catch
                            {

                            }
                        }

                        Int64 ReallyCurrentSZ = 0;

                        try
                        {
                            if (File.Exists(LocalFilename) == true)
                            {
                                FileInfo fi = new FileInfo(LocalFilename);
                                ReallyCurrentSZ = fi.Length;
                            }
                            else
                            {
                                FoxEventLog.WriteEventLog("File " + LocalFilename + " does not exist for upload.", System.Diagnostics.EventLogEntryType.Error);
                                return;
                            }
                        }
                        catch (Exception ee)
                        {
                            FoxEventLog.WriteEventLog("Checking upload file " + LocalFilename + " ID: " + ServerID.ToString() + " failed:\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                            return;
                        }

                        downloadnet = Utilities.ConnectNetwork(7);
                        if (downloadnet == null)
                            return;

                        if (ReqOnly == true)
                        {
                            Status.UpdateMessage(7, "Checking " + LocalFilename);
                            MD5 = MD5Utilities.CalcMD5File(LocalFilename);
                            FileInfo fi = new FileInfo(LocalFilename);
                            lock (FileLock)
                            {
                                FilesystemData.FileTransferStatus.MD5CheckSum = MD5;
                                FilesystemData.FileTransferStatus.LastModfied = fi.LastWriteTimeUtc;
                                FilesystemData.WriteFileTransferStatus();
                            }

                            Int64? NewID = downloadnet.File_Agent_NewUploadReq(LocalFilename, OverrideMetered, MD5);
                            if (NewID == null)
                            {
                                FoxEventLog.VerboseWriteEventLog("Cannot create a new upload req for " + LocalFilename, System.Diagnostics.EventLogEntryType.Warning);
                                return;
                            }

                            if (downloadnet.File_Agent_CancelUpload(ServerID) == false)
                            {
                                FoxEventLog.VerboseWriteEventLog("Cannot delete temp upload req for " + LocalFilename + " ID: " + ServerID.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                                return;
                            }

                            lock (FileLock)
                            {
                                //wait for the new upload req
                                ClearDataFSD();
                                FilesystemData.WriteFileTransferStatus();
                                UnlockTimer = true;
                                return;
                            }
                        }
                        else
                        {
                            FileStream fs = null;
                            do
                            {
                                try
                                {
                                    if (CancelAndDeleteDL == true)
                                        return;

                                    FileInfo fi = new FileInfo(LocalFilename);
                                    if (roughDT(fi.LastWriteTimeUtc, LastModified) == false || fi.Length != TotalSZ)
                                    {
                                        if (downloadnet.File_Agent_CancelUpload(ServerID) == false)
                                        {
                                            FoxEventLog.VerboseWriteEventLog("Cannot cancel upload req for " + LocalFilename + " ID: " + ServerID.ToString() + ", due file changes", System.Diagnostics.EventLogEntryType.Warning);
                                            lock (FileLock)
                                            {
                                                ClearDataFSD();
                                                FilesystemData.WriteFileTransferStatus();
                                            }
                                            return;
                                        }
                                        else
                                        {
                                            FoxEventLog.VerboseWriteEventLog("Cannot upload req for " + LocalFilename + " ID: " + ServerID.ToString() + ": File has been changed!", System.Diagnostics.EventLogEntryType.Warning);
                                        }
                                        return;
                                    }

                                    try
                                    {
                                        if (fs == null)
                                            fs = File.Open(LocalFilename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                                    }
                                    catch (Exception ee)
                                    {
                                        FoxEventLog.WriteEventLog("Cannot upload file " + LocalFilename + " ID: " + ServerID.ToString() + " failed:\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                                        return;
                                    }

                                    try
                                    {
                                        fs.Seek(CurrentSZ, SeekOrigin.Begin);
                                    }
                                    catch
                                    {
                                        FoxEventLog.WriteEventLog("Cannot properly seek in file " + LocalFilename + " ID: " + ServerID.ToString() + " Pos: 0x" + CurrentSZ.ToString("X"), System.Diagnostics.EventLogEntryType.Warning);
                                        return;
                                    }

                                    byte[] data = null;

                                    try
                                    {
                                        int read = 1024 * 1024;
                                        data = new byte[read];
                                        read = fs.Read(data, 0, read);
                                        if (data.Length != read)
                                        {
                                            byte[] ddd = new byte[read];
                                            Array.Copy(data, ddd, read);
                                            data = ddd;
                                        }
                                    }
                                    catch
                                    {
                                        FoxEventLog.WriteEventLog("Cannot read file " + LocalFilename + " ID: " + ServerID.ToString() + " Pos: 0x" + CurrentSZ.ToString("X"), System.Diagnostics.EventLogEntryType.Warning);
                                        return;
                                    }

                                    Status.UpdateMessage(7, "Uploading " + LocalFilename + "\r\n" + CommonUtilities.NiceSize(CurrentSZ) + " of " + CommonUtilities.NiceSize(TotalSZ));
                                    bool res = downloadnet.File_Agent_AppendUpload(ServerID, data);
                                    if (res == false)
                                    {
                                        FoxEventLog.VerboseWriteEventLog("Cannot upload append req for " + LocalFilename + " ID: " + ServerID.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                                        return;
                                    }

                                    CurrentSZ += data.Length;
                                    lock (FileLock)
                                    {
                                        FilesystemData.FileTransferStatus.ProgressSize = CurrentSZ;
                                        FilesystemData.WriteFileTransferStatus();
                                    }

                                    if (CurrentSZ == TotalSZ)
                                    {
                                        FoxEventLog.WriteEventLog("Upload file success " + LocalFilename + " ID: " + ServerID.ToString(), System.Diagnostics.EventLogEntryType.Information);
                                        lock (FileLock)
                                        {
                                            ClearDataFSD();
                                            FilesystemData.WriteFileTransferStatus();
                                        }
                                        UnlockTimer = true;
                                        return;
                                    }
                                }
                                finally
                                {
                                    if (fs != null)
                                    {
                                        fs.Close();
                                        fs = null;
                                    }
                                }
                            } while (true);
                        }
                    }
                    catch (Exception ee)
                    {
                        FoxEventLog.WriteEventLog("Downloading file " + LocalFilename + " ID: " + ServerID.ToString() + " crashed:\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    }
                    finally
                    {
                        Status.UpdateMessage(7);
                    }
                }

                #endregion
                #region Server to Client

                if (Direction == 0)
                {
                    string LocalFilename;
                    string MD5;
                    Int64 ServerID;
                    Int64 CurrentSZ;
                    Int64 TotalSZ;
                    bool OverrideMetered;
                    lock (FileLock)
                    {
                        LocalFilename = FilesystemData.FileTransferStatus.RemoteFileLocation;
                        ServerID = FilesystemData.FileTransferStatus.ServerID.Value;
                        CurrentSZ = FilesystemData.FileTransferStatus.ProgressSize;
                        TotalSZ = FilesystemData.FileTransferStatus.Size;
                        MD5 = FilesystemData.FileTransferStatus.MD5CheckSum;
                        OverrideMetered = FilesystemData.FileTransferStatus.OverrideMeteredConnection;
                    }

                    //won't start when in metered connection!
                    if (OverrideMetered == false)
                    {
                        try
                        {
                            if (MeteredConnection.IsMeteredConnection() == true)
                            {
                                FoxEventLog.VerboseWriteEventLog("Download paused = metered connection detected", System.Diagnostics.EventLogEntryType.Information);
                                return;
                            }
                        }
                        catch
                        {

                        }
                    }

                    Int64 ReallyCurrentSZ = 0;

                    try
                    {
                        string Dir = Path.GetDirectoryName(LocalFilename);
                        if (Directory.Exists(Dir) == false)
                            Directory.CreateDirectory(Dir);
                        if (File.Exists(LocalFilename) == true)
                        {
                            FileInfo fi = new FileInfo(LocalFilename);
                            ReallyCurrentSZ = fi.Length;
                        }
                        else
                        {
                            ReallyCurrentSZ = 0;
                        }
                    }
                    catch (Exception ee)
                    {
                        FoxEventLog.WriteEventLog("Checking download file " + LocalFilename + " ID: " + ServerID.ToString() + " failed:\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    }

                    if (ReallyCurrentSZ != CurrentSZ)
                    {
                        FoxEventLog.WriteEventLog("File Size does not match: deleting the file " + LocalFilename, System.Diagnostics.EventLogEntryType.Warning);
                        try
                        {
                            File.Delete(LocalFilename);
                        }
                        catch (Exception ee)
                        {
                            FoxEventLog.WriteEventLog("Cannot delete " + LocalFilename + "\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                        }
                        lock (FileLock)
                        {
                            ClearDataFSD();
                            FilesystemData.WriteFileTransferStatus();
                            return;
                        }
                    }

                    downloadnet = Utilities.ConnectNetwork(7);
                    if (downloadnet == null)
                        return;
                    try
                    {
                        downloadnet.DownloadNotify += Downloadnet_DownloadNotify;
                        if (downloadnet.DownloadFile("api/agent/filefiledownload/" + ServerID.ToString(), LocalFilename, TotalSZ) == false)
                        {
                            FileInfo fi = new FileInfo(LocalFilename);
                            lock (FileLock)
                            {
                                FilesystemData.FileTransferStatus.ProgressSize = fi.Length;
                                FilesystemData.WriteFileTransferStatus();
                            }
                        }
                        else
                        {
                            if (downloadnet.StopDownload == false)
                            {
                                Status.UpdateMessage(7, "Checking " + FilesystemData.FileTransferStatus.RemoteFileLocation + "...");
                                string CalcMD5 = MD5Utilities.CalcMD5File(LocalFilename);
                                if (MD5.ToLower() != CalcMD5.ToLower())
                                {
                                    FoxEventLog.WriteEventLog("File MD5 does not match: deleting the file " + LocalFilename, System.Diagnostics.EventLogEntryType.Warning);
                                    try
                                    {
                                        File.Delete(LocalFilename);
                                    }
                                    catch (Exception ee)
                                    {
                                        FoxEventLog.WriteEventLog("Cannot delete " + LocalFilename + "\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                                    }
                                    lock (FileLock)
                                    {
                                        ClearDataFSD();
                                        FilesystemData.WriteFileTransferStatus();
                                        return;
                                    }
                                }
                                else
                                {
                                    //success!
                                    FoxEventLog.WriteEventLog("File download success: " + LocalFilename, System.Diagnostics.EventLogEntryType.Information);
                                    downloadnet.File_Agent_CancelUpload(ServerID);
                                    lock (FileLock)
                                    {
                                        ClearDataFSD();
                                        FilesystemData.WriteFileTransferStatus();
                                    }
                                    UnlockTimer = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (CancelAndDeleteDL == true)
                                {
                                    FoxEventLog.VerboseWriteEventLog("File " + LocalFilename + " canceled by server req.", System.Diagnostics.EventLogEntryType.Information);
                                    try
                                    {
                                        File.Delete(LocalFilename);
                                    }
                                    catch (Exception ee)
                                    {
                                        FoxEventLog.WriteEventLog("Cannot delete " + LocalFilename + "\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                                    }
                                    lock (FileLock)
                                    {
                                        ClearDataFSD();
                                        FilesystemData.WriteFileTransferStatus();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        FoxEventLog.WriteEventLog("Downloading file " + LocalFilename + " ID: " + ServerID.ToString() + " crashed:\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    }
                    finally
                    {
                        downloadnet.DownloadNotify -= Downloadnet_DownloadNotify;
                        Status.UpdateMessage(7);
                    }
                }

                #endregion
            }
            finally
            {
                try
                {
                    if (downloadnet != null)
                        downloadnet.CloseConnection();
                }
                catch
                {

                }
                downloadnet = null;
            }
        }

        private static void Downloadnet_DownloadNotify(long CurrentSZ, long TotalSZ)
        {
            Status.UpdateMessage(7, "Downloading " + FilesystemData.FileTransferStatus.RemoteFileLocation + "\r\n" + CommonUtilities.NiceSize(CurrentSZ) + " of " + CommonUtilities.NiceSize(TotalSZ));
        }

        static void ThreadRunner()
        {
            lock (FileLock)
            {
                FilesystemData.LoadFileTransferStatus();
            }

            while (StopService == false)
            {
                try
                {
                    Network net = Utilities.ConnectNetwork(7);
                    if (net == null)
                    {
                        Wait(60);
                        continue;
                    }

                    NetInt64ListSigned lst = net.File_Agent_GetFileList();
                    if (lst == null)
                    {
                        net.CloseConnection();
                        Wait(2 * 60);
                        continue;
                    }

                    if (ApplicationCertificate.Verify(lst) == false)
                    {
                        FoxEventLog.WriteEventLog("One or more DownloadFS Lists are tampered - no download / uploads will be processed.", System.Diagnostics.EventLogEntryType.Error);
                        net.CloseConnection();
                        Wait(2 * 60);
                        continue;
                    }

                    if (lst.data.data.Count == 0)
                    {
                        net.CloseConnection();
                        Wait(2 * 60);
                        continue;
                    }

                    Int64 RunningID = 0;
                    lock (FileLock)
                    {
                        if (FilesystemData.FileTransferStatus.ServerID != null)
                        {
                            bool Found = false;
                            foreach (Int64 l in lst.data.data)
                            {
                                if (l == FilesystemData.FileTransferStatus.ServerID.Value)
                                {
                                    Found = true;
                                    RunningID = l;
                                    break;
                                }
                            }

                            if (Found == false)
                                RunningID = lst.data.data[0];
                        }
                        else
                        {
                            RunningID = lst.data.data[0];
                        }
                    }

                    Debug.Assert(RunningID != 0);
                    FileUploadDataSigned fud = net.File_Agent_GetFileAnyData(RunningID);
                    if (ApplicationCertificate.Verify(fud) == false)
                    {
                        FoxEventLog.WriteEventLog("One or more DownloadFS Elements are tampered - no download / uploads will be processed.", System.Diagnostics.EventLogEntryType.Error);
                        net.CloseConnection();
                        Wait(2 * 60);
                        continue;
                    }

                    net.CloseConnection();

                    string LocalFilename = "";
                    bool WaitandContinue = false;

                    lock (FileLock)
                    {
                        //may be needed for deleting the file (if canceled by the server)
                        LocalFilename = FilesystemData.FileTransferStatus.RemoteFileLocation;

                        if (FilesystemData.FileTransferStatus.ServerID != null)
                        {
                            if (RunningID != FilesystemData.FileTransferStatus.ServerID)
                            {
                                CancelAndDeleteDL = true;
                            }
                            else
                            {
                                if (FilesystemData.FileTransferStatus.Size != fud.Data.Size ||
                                    FilesystemData.FileTransferStatus.RemoteFileLocation != fud.Data.RemoteFileLocation ||
                                    FilesystemData.FileTransferStatus.MD5CheckSum.ToLower() != fud.Data.MD5CheckSum.ToLower() ||
                                    FilesystemData.FileTransferStatus.RequestOnly != fud.Data.RequestOnly)
                                {
                                    CancelAndDeleteDL = true;
                                }
                                else
                                {
                                    if (RunningDLThread == null)
                                    {
                                        RunningDLThread = new Thread(new ThreadStart(DownloadThreadRunner));
                                        RunningDLThread.Start();
                                    }
                                    else
                                    {
                                        if (RunningDLThread.IsAlive == false)
                                        {
                                            RunningDLThread = new Thread(new ThreadStart(DownloadThreadRunner));
                                            RunningDLThread.Start();
                                        }
                                    }

                                }

                                WaitandContinue = true;
                            }
                        }
                    }

                    if (WaitandContinue == true)
                    {
                        Wait(2 * 60);
                        continue;
                    }

                    if (CancelAndDeleteDL == true)
                    {
                        if (RunningDLThread != null)
                        {
                            RunningDLThread.Join();
                        }

                        if (string.IsNullOrWhiteSpace(LocalFilename) == false)
                        {
                            try
                            {
                                if (FilesystemData.FileTransferStatus.Direction == 0)
                                {
                                    if (File.Exists(LocalFilename) == true)
                                        File.Delete(LocalFilename);
                                }
                            }
                            catch (Exception ee)
                            {
                                FoxEventLog.WriteEventLog("Cannot check / delete " + LocalFilename + "\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                            }
                        }
                    }

                    CancelAndDeleteDL = false;

                    lock (FileLock)
                    {
                        FilesystemData.FileTransferStatus.Size = fud.Data.Size;
                        FilesystemData.FileTransferStatus.RemoteFileLocation = fud.Data.RemoteFileLocation;
                        FilesystemData.FileTransferStatus.MD5CheckSum = fud.Data.MD5CheckSum.ToLower();
                        FilesystemData.FileTransferStatus.OverrideMeteredConnection = fud.Data.OverrideMeteredConnection;
                        FilesystemData.FileTransferStatus.Direction = fud.Data.Direction;
                        FilesystemData.FileTransferStatus.ProgressSize = 0;
                        FilesystemData.FileTransferStatus.ServerID = RunningID;
                        FilesystemData.FileTransferStatus.RequestOnly = fud.Data.RequestOnly;
                        FilesystemData.FileTransferStatus.LastModfied = fud.Data.FileLastModified;
                        FilesystemData.WriteFileTransferStatus();
                    }

                    RunningDLThread = new Thread(new ThreadStart(DownloadThreadRunner));
                    RunningDLThread.Start();

                    Wait(2 * 60);
                    continue;
                }
                catch (Exception ee)
                {
                    FoxEventLog.WriteEventLog("Internal crash in DownloadFS System: " + ee.Message, System.Diagnostics.EventLogEntryType.Error);
                    Wait(10);
                }
                Wait(2 * 60);
            };
        }

        public static void StartThread()
        {
            RunningThread = new Thread(new ThreadStart(ThreadRunner));
            RunningThread.Start();
        }

        public static void StopThread()
        {
            if (downloadnet != null)
                downloadnet.StopDownload = true;
            StopService = true;
            if (RunningThread != null)
                RunningThread.Join();
        }
    }
}
