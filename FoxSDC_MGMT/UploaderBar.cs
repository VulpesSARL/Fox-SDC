using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    class UploaderBar : ProgressBar
    {
        FileStream file;
        string lasterror = "";
        Int64 filesz;
        Int64 currentsz;
        Thread RunnerThread;
        Int64 counter;
        Network net;
        bool CancelThread;

        public delegate void voidy();
        public delegate void newd(NewUploadDataID nid);

        public event voidy Error;
        public event voidy Cancel;
        public event newd Success;

        void OnError()
        {
            if (Error != null)
                Error();
        }

        void OnCancel()
        {
            if (Cancel != null)
                Cancel();
        }

        void OnSuccess(NewUploadDataID newid)
        {
            if (Success != null)
                Success(newid);
        }

        public string LastError
        {
            get
            {
                return (lasterror);
            }
        }

        void Reset()
        {
            this.Minimum = 0;
            this.Maximum = 1000;
            this.Value = 0;
        }

        public UploaderBar()
        {
            Reset();
        }

        void UpdatePromille()
        {
            this.Value = (int)(((double)currentsz / (double)filesz) * 1000d);
        }

        public void CancelUpload()
        {
            CancelThread = true;
        }

        public bool UploadFile(string Filename, int Type, string Pretitle = "")
        {
            if (RunnerThread != null)
                if (RunnerThread.IsAlive == true)
                    return (false);

            Reset();

            if (File.Exists(Filename) == false)
            {
                lasterror = "File does not exist";
                return (false);
            }

            FileInfo fi = new FileInfo(Filename);


            string ClonedSession = Program.net.CloneSession();
            if (ClonedSession == "")
            {
                lasterror = "No Session";
                return (false);
            }

            this.net = new Network();
            this.net.SetSessionID(Program.net.ConnectedURL, ClonedSession);

            if (this.net.NeedChangePassword() == true)
            {
                lasterror = "Password needs to be changed";
                this.net.CloseConnection();
                return (false);
            }

            string PT = Pretitle;

            if (PT == "")
                PT = Path.GetFileName(Filename);

            if (this.net.UploadRequest(Type, fi.Length, PT) == false)
            {
                lasterror = this.net.GetLastError();
                this.net.CloseConnection();
                return (false);
            }

            file = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            filesz = fi.Length;
            currentsz = 0;
            counter = 0;

            CancelThread = false;
            RunnerThread = new Thread(new ThreadStart(UploaderThread));
            RunnerThread.Start();

            return (true);
        }

        void UploaderThread()
        {
            while (file.Position < filesz)
            {
                Int64 blksz;
                Int64 nchunk = 512 * 1024;
                if (nchunk > filesz - file.Position)
                    nchunk = filesz - file.Position;

                byte[] data = new byte[nchunk];
                blksz = file.Read(data, 0, data.Length);
                currentsz += blksz;
                counter++;
                UploadData ud = new UploadData();
                ud.Counter = counter;
                ud.data = data;
                ud.Size = blksz;
                ud.MD5 = MD5Utilities.CalcMD5(data);
                if (this.net.UploadData(ud) == false)
                {
                    lasterror = this.net.GetLastError();
                    this.net.CloseConnection();
                    file.Close();
                    this.BeginInvoke(new voidy(Reset));
                    this.BeginInvoke(new voidy(OnError));
                    return;
                }
                if (CancelThread == true)
                {
                    lasterror = "Canceled";
                    this.net.UploadCancel();
                    this.net.CloseConnection();
                    file.Close();
                    this.BeginInvoke(new voidy(Reset));
                    this.BeginInvoke(new voidy(OnCancel));
                    return;
                }
                this.BeginInvoke(new voidy(UpdatePromille));
            }
            file.Close();
            NewUploadDataID nid = this.net.UploadFinalise();
            if (nid == null)
            {
                lasterror = this.net.GetLastError();
                this.net.CloseConnection();
                this.BeginInvoke(new voidy(Reset));
                this.BeginInvoke(new voidy(OnError));
                return;
            }
            this.net.CloseConnection();
            this.BeginInvoke(new voidy(Reset));
            this.BeginInvoke(new newd(OnSuccess), nid);
        }

    }
}
