using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_MGMT
{
    enum Direction
    {
        UploadToServer,
        DownloadFromServer
    }

    class UploadDownloadData : ICloneable
    {
        public string MachineID;
        public Direction Direction;
        public string LocalFilename;
        public string RemoteFilename;
        public string MD5CheckSum;
        public Int64 Size;
        public Int64? ProgressSize;
        public string ID = Guid.NewGuid().ToString();
        public bool IgnoreMeteredConnection;
        public bool Failed;
        public string ErrorText;
        public FileStream FileStream;
        public Int64? UploadID = null;
        public Thread DownloadThread = null;


        public object Clone()
        {
            return (new UploadDownloadData()
            {
                Direction = this.Direction,
                LocalFilename = this.LocalFilename,
                MachineID = this.MachineID,
                MD5CheckSum = this.MD5CheckSum,
                ProgressSize = this.ProgressSize,
                RemoteFilename = this.RemoteFilename,
                Size = this.Size,
                ID = this.ID,
                IgnoreMeteredConnection = this.IgnoreMeteredConnection,
                Failed = this.Failed,
                ErrorText = this.ErrorText,
                UploadID = this.UploadID,
                DownloadThread = null,
                FileStream = null
            });
        }
    }

    class UploadDownloadDataThread
    {
        static List<UploadDownloadData> Data = new List<UploadDownloadData>();
        static object DataLock = new object();
        public static bool ThreadClosed = true;
        public static bool CancelThread = false;
        static Thread thread;
        static Network net;
        static bool StopDownload = false;

        static public void StartThread()
        {
            ThreadClosed = false;
            thread = new Thread(new ThreadStart(Thready));
            thread.Start();
            net = Program.net.CloneElement();
        }

        static void DownloadThready(object dd)
        {
            if (!(dd is UploadDownloadData))
                return;

            try
            {
                UploadDownloadData d = (UploadDownloadData)dd;

                Debug.Assert(d.UploadID != null);

                #region Download Code

                HttpWebRequest client = (HttpWebRequest)WebRequest.Create(net.ConnectedURL + "api/agent/filefiledownload/" + d.UploadID.Value.ToString());
                client.Pipelined = false;
                client.ServicePoint.Expect100Continue = false;
                client.AllowAutoRedirect = true;
                if (net.Session != "")
                    client.Headers.Add("Authorization", "Bearer " + net.Session);
#if DEBUG
                client.ReadWriteTimeout = 5000;
                client.Timeout = 5000;
#else
                client.ReadWriteTimeout = 60000;
                client.Timeout = 60000;
#endif
                client.UserAgent = "FoxSDC Client";
                client.Method = "GET";

                Int64 SeekTo = 0;
                FileMode fm = FileMode.Create;

                if (File.Exists(d.RemoteFilename) == true)
                {
                    FileInfo f = new FileInfo(d.RemoteFilename);
                    SeekTo = f.Length;
                    fm = FileMode.Open;
                }

                using (Stream FileStream = File.Open(d.RemoteFilename, fm, FileAccess.ReadWrite, FileShare.Read))
                {
                    FileStream.Seek(SeekTo, SeekOrigin.Begin);
                    HttpWebResponse resp = (HttpWebResponse)client.GetResponse();
                    using (Stream HTTPStream = resp.GetResponseStream())
                    {
                        const int ReadBufferSZ = 2048;
                        d.ProgressSize = 0;

                        StopDownload = false;

                        byte[] data = new byte[ReadBufferSZ];
                        int ReadSZ = HTTPStream.Read(data, 0, ReadBufferSZ);
                        while (ReadSZ > 0)
                        {
                            d.ProgressSize += ReadSZ;
                            FileStream.Write(data, 0, ReadSZ);

                            ReadSZ = HTTPStream.Read(data, 0, ReadBufferSZ);
                            if (StopDownload == true)
                                break;
                        }
                    }
                }

                if (StopDownload == true)
                    return;

                if (MD5Utilities.CalcMD5File(d.RemoteFilename).ToLower() != d.MD5CheckSum.ToLower())
                {
                    File.Delete(d.RemoteFilename);
                    d.Failed = true;
                    d.ErrorText = "MD5 mismatch";
                    d.DownloadThread = null;
                    return;
                }

                net.File_MGMT_CancelUpload(d.UploadID.Value);
                d.DownloadThread = null;
                lock (DataLock)
                {
                    Data.Remove(d);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
            #endregion
        }

        static void Thready()
        {
            do
            {
                UploadDownloadData d = null;
                lock (DataLock)
                {
                    if (Data.Count == 0)
                    {
                        d = null;
                        continue;
                    }
                    foreach (UploadDownloadData dd in Data)
                    {
                        if (dd.Failed == false)
                            d = dd;
                    }
                }

                if (d == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    //check if another download thread is running
                    foreach (UploadDownloadData dd in Data)
                    {
                        if (dd.ID == d.ID)
                            continue;
                        if (dd.DownloadThread != null)
                        {
                            StopDownload = true;
                            dd.DownloadThread.Join(60000);
                            dd.DownloadThread = null;
                        }
                    }

                    //process element
                    switch (d.Direction)
                    {
                        case Direction.DownloadFromServer:
                            if (d.DownloadThread != null)
                            {
                                if (d.DownloadThread.IsAlive == true)
                                {
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            Debug.Assert(d.MD5CheckSum != null);
                            Debug.Assert(d.RemoteFilename != null);
                            if (d.ProgressSize != null)
                                if (d.ProgressSize != d.Size)
                                    d.ProgressSize = null;
                            if (d.UploadID == null)
                            {
                                d.Failed = true;
                                d.ErrorText = "Missing Download ID";
                                continue;
                            }

                            try
                            {
                                // u.RemoteFilename - here
                                // u.LocalFilename - agent
                                if (d.DownloadThread == null)
                                {
                                    StopDownload = false;
                                    d.DownloadThread = new Thread(new ParameterizedThreadStart(DownloadThready));
                                    d.DownloadThread.Start(d);
                                }
                            }
                            catch (Exception ee)
                            {
                                d.Failed = true;
                                d.ErrorText = "Cannot start download: " + ee.Message;
                                continue;
                            }
                            finally
                            {
                            }

                            break;
                        case Direction.UploadToServer:
                            if (string.IsNullOrWhiteSpace(d.MD5CheckSum) == true)
                            {
                                d.MD5CheckSum = MD5Utilities.CalcMD5File(d.LocalFilename);
                            }
                            else
                            {
                                if (d.FileStream == null)
                                {
                                    try
                                    {
                                        d.FileStream = File.Open(d.LocalFilename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                                    }
                                    catch (Exception ee)
                                    {
                                        d.Failed = true;
                                        d.ErrorText = "Cannot open " + d.LocalFilename + ": " + ee.Message;
                                        continue;
                                    }
                                }
                                else
                                {
                                    #region Upload code

                                    if (d.UploadID == null)
                                    {
                                        d.UploadID = net.File_MGMT_NewUploadReq(d.LocalFilename, d.RemoteFilename, d.MachineID, d.MD5CheckSum, d.IgnoreMeteredConnection);
                                        if (d.UploadID == null)
                                        {
                                            d.FileStream.Close();
                                            d.FileStream = null;

                                            d.Failed = true;
                                            d.ErrorText = net.GetLastError();
                                            continue;
                                        }
                                        d.ProgressSize = 0;
                                    }
                                    else
                                    {
                                        int read = 1024 * 1024;
                                        byte[] data = new byte[read];
                                        read = d.FileStream.Read(data, 0, read);
                                        if (data.Length != read)
                                        {
                                            byte[] ddd = new byte[read];
                                            Array.Copy(data, ddd, read);
                                            data = ddd;
                                        }
                                        bool res = net.File_MGMT_AppendUpload(d.MachineID, d.UploadID.Value, data);
                                        if (res == false)
                                        {
                                            d.FileStream.Close();
                                            d.FileStream = null;

                                            d.Failed = true;
                                            d.ErrorText = net.GetLastError();

                                            net.File_MGMT_CancelUpload(d.UploadID.Value);
                                            continue;
                                        }
                                        d.ProgressSize += read;

                                        if (d.ProgressSize == d.Size)
                                        {
                                            d.FileStream.Close();
                                            d.FileStream = null;

                                            lock (DataLock)
                                            {
                                                Data.Remove(d);
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                            break;
                    }
                }
            } while (CancelThread == false);

            lock (DataLock)
            {
                foreach (UploadDownloadData d in Data)
                {
                    if (d.UploadID != null)
                    {
                        switch (d.Direction)
                        {
                            case Direction.UploadToServer:
                                net.File_MGMT_CancelUpload(d.UploadID.Value);
                                break;
                            case Direction.DownloadFromServer:
                                // u.RemoteFilename - here
                                // u.LocalFilename - agent
                                try
                                {
                                    File.Delete(d.RemoteFilename);
                                }
                                catch
                                {

                                }
                                break;
                        }
                    }
                }
            }

            ThreadClosed = true;
        }

        static public List<UploadDownloadData> DataList
        {
            get
            {
                lock (DataLock)
                {
                    List<UploadDownloadData> n = new List<UploadDownloadData>();
                    foreach (UploadDownloadData e in Data)
                        n.Add((UploadDownloadData)e.Clone());
                    return (n);
                }
            }
        }

        static public Int64 DataListRunning
        {
            get
            {
                lock (DataLock)
                {
                    return (Data.Count());
                }
            }
        }

        public static void ResetUploadDownload(string ID)
        {
            lock (DataLock)
            {
                foreach (UploadDownloadData u in Data)
                {
                    if (ID == u.ID)
                    {
                        if (u.Failed == true)
                        {
                            u.ErrorText = "";
                            u.MD5CheckSum = "";
                            u.FileStream = null;
                            if (u.Direction != Direction.DownloadFromServer)
                                u.UploadID = null;
                            u.Failed = false;
                        }
                    }
                    break;
                }
            }
        }

        public static void CancelUploadDownload(string ID)
        {
            UploadDownloadData del = null;

            lock (DataLock)
            {
                foreach (UploadDownloadData u in Data)
                {
                    if (ID == u.ID)
                    {
                        if (u.Direction == Direction.UploadToServer)
                        {
                            del = u;
                            if (u.UploadID != null)
                                Program.net.File_MGMT_CancelUpload(u.UploadID.Value);
                            if (u.FileStream != null)
                            {
                                u.FileStream.Close();
                                u.FileStream = null;
                            }
                        }
                        if (u.Direction == Direction.DownloadFromServer)
                        {
                            // u.RemoteFilename - here
                            // u.LocalFilename - agent
                            del = u;
                            if (u.DownloadThread != null)
                            {
                                StopDownload = true;
                                u.DownloadThread.Join(60000);
                                u.DownloadThread = null;
                                try
                                {
                                    File.Delete(u.RemoteFilename);
                                }
                                catch
                                {

                                }
                            }
                            if (u.UploadID != null)
                                Program.net.File_MGMT_CancelUpload(u.UploadID.Value);
                            if (u.FileStream != null)
                            {
                                u.FileStream.Close();
                                u.FileStream = null;
                            }
                        }
                        break;
                    }
                }
                if (del != null)
                {
                    Data.Remove(del);
                }
            }
        }

        public static bool AddDownloadFromServer(string FromMachineID, Int64 ID, Int64 Size, string RemoteFile, string ToLocalFile, string MD5, out string Error)
        {
            Error = "";

            UploadDownloadData ud = new UploadDownloadData();
            ud.Direction = Direction.DownloadFromServer;
            ud.LocalFilename = ToLocalFile;
            ud.RemoteFilename = RemoteFile;
            ud.Size = Size;
            ud.MD5CheckSum = MD5;
            ud.IgnoreMeteredConnection = true;
            ud.ProgressSize = null;
            ud.ErrorText = "";
            ud.Failed = false;
            ud.MachineID = FromMachineID;
            ud.UploadID = ID;

            lock (DataLock)
            {
                Data.Add(ud);
            }

            return (true);
        }

        public static bool AddUploadToServer(string ForMachineID, string LocalFile, string RemoteFile, bool IgnoreMeteredConnection, out string Error)
        {
            Error = "";
            if (File.Exists(LocalFile) == false)
            {
                Error = "File not found.";
                return (false);
            }

            FileInfo fi = new FileInfo(LocalFile);

            Int64 Size = fi.Length;

            UploadDownloadData ud = new UploadDownloadData();
            ud.Direction = Direction.UploadToServer;
            ud.LocalFilename = LocalFile;
            ud.RemoteFilename = RemoteFile;
            ud.Size = Size;
            ud.IgnoreMeteredConnection = IgnoreMeteredConnection;
            ud.ProgressSize = null;
            ud.ErrorText = "";
            ud.Failed = false;
            ud.MachineID = ForMachineID;

            lock (DataLock)
            {
                Data.Add(ud);
            }

            return (true);
        }
    }
}
