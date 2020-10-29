using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Redirs
{

    [ServiceContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
    public interface IPipeScreenData
    {
        [OperationContract]
        NetBool SetKeyboard2(int Flags, int ScanCode, int VirtualKey);
        [OperationContract]
        NetBool SetMousePosition2(int X, int Y, int Delta, int Flags);
        [OperationContract]
        PushScreenData GetFullscreen();
        [OperationContract]
        PushScreenData GetDeltaScreen();
        [OperationContract]
        bool Ping();
        [OperationContract]
        bool CloseSession();
        [OperationContract]
        NetBool SetScreen(int ScreenNumber);
    }

    /// <summary>
    /// Client to Server
    /// </summary>
    class PipeScreenDataComm : ClientBase<IPipeScreenData>
    {
        public PipeScreenDataComm(string GUID)
            : base(new ServiceEndpoint(ContractDescription.GetContract(typeof(IPipeScreenData)),
            new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/sdcscreen/FoxSDC-Agent-SCREEN-" + GUID + "/FoxSDC-Agent-SCREEN-" + GUID)))
        {
            ((NetNamedPipeBinding)(this.Endpoint.Binding)).MaxReceivedMessageSize = 1048576;
            ((NetNamedPipeBinding)(this.Endpoint.Binding)).MaxBufferPoolSize = 1048576;
            ((NetNamedPipeBinding)(this.Endpoint.Binding)).MaxBufferSize = 1048576;
        }

        public bool Ping()
        {
            return (Channel.Ping());
        }
        public PushScreenData GetDeltaScreen()
        {
            return (Channel.GetDeltaScreen());
        }
        public PushScreenData GetFullscreen()
        {
            return (Channel.GetFullscreen());
        }
        public NetBool SetMousePosition(int X, int Y, int Delta, int Flags)
        {
            return (Channel.SetMousePosition2(X, Y, Delta, Flags));
        }
        public NetBool SetScreen(int ScreenNumber)
        {
            return (Channel.SetScreen(ScreenNumber));
        }
        public NetBool SetKeyboard(int Flags, int ScanCode, int VirtualKey)
        {
            return (Channel.SetKeyboard2(Flags, ScanCode, VirtualKey));
        }
        public bool CloseSession()
        {
            return (Channel.CloseSession());
        }
    }

    /// <summary>
    /// Server to Client ("Main" Class)
    /// </summary>
    class PipeScreenData : IPipeScreenData
    {
        public object ScreenLock = new object();
        public int ScreenX;
        public int ScreenY;
        public byte[] ScreenRawBytes = null;
        public int UseDeltaScreenAlgo = 1;
        int NextSquare = 0;

        public bool CloseSession()
        {
            MainScreenSystemClient.Timeout = true;
            return (true);
        }

        public bool Ping()
        {
            return (true);
        }

        public NetBool SetScreen(int ScreenNumber)
        {
            NetBool nb = new NetBool();
            nb.Data = true;
            ProgramAgent.CPP.SetScreenNumber(ScreenNumber);
            return (nb);
        }

        public PushScreenData GetDeltaScreen()
        {
            try
            {
                MainScreenSystemClient.LastCalled = DateTime.Now;
                PushScreenData data = new PushScreenData();
                CPPFrameBufferData screen = ProgramAgent.CPP.GetFrameBufferData();
                if (screen == null)
                {
                    data.X = data.Y = 0;
                    data.FailedCode = -1;
                    return (data);
                }

                if (screen.Failed == true)
                {
                    Debug.WriteLine("Screendata failed @ " + screen.FailedAt.ToString() + " 0x" + screen.Win32Error.ToString("X"));

                    data.X = data.Y = 0;
                    data.FailedCode = screen.FailedAt;
                    return (data);
                }

                lock (ScreenLock)
                {
                    //resolution has changed - send whole image over
                    if (screen.X != ScreenX || screen.Y != ScreenY)
                    {
                        try
                        {
                            Bitmap bmp = new Bitmap(screen.X, screen.Y, 4 * screen.X, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(screen.Data, 0));

                            MemoryStream mem = new MemoryStream();
                            ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);

                            bmp.Save(mem, codec, myEncoderParameters);
                            mem.Seek(0, SeekOrigin.Begin);
                            data.Data = new byte[mem.Length];
                            mem.Read(data.Data, 0, (int)mem.Length);

                            NextSquare = 0;
                            ScreenX = screen.X;
                            ScreenY = screen.Y;
                            ScreenRawBytes = screen.Data;
                            return (data);
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());
                            data.X = data.Y = 0;
                            data.FailedCode = -2;

                            //next time, transfer whole screen
                            ScreenX = ScreenY = 0;
                            NextSquare = 0;
                            return (data);
                        }
                    }
                }

                switch (UseDeltaScreenAlgo)
                {
                    case 0:
                        {
                            int BlockX = 64;
                            int BlockY = 64;
                            int VScreenX = screen.X + (BlockX - (screen.X % BlockX));
                            int VScreenY = screen.Y + (BlockY - (screen.Y % BlockY));

                            byte[] deltadata = new byte[screen.Data.Length];
                            int XSZ = screen.X * 4;
                            data.ChangedBlocks = new List<long>();
                            data.DataType = 1;
                            data.BlockX = BlockX;
                            data.BlockY = BlockY;
                            data.X = screen.X;
                            data.Y = screen.Y;

                            lock (ScreenLock)
                            {
                                for (int y = 0; y < VScreenY; y += BlockY)
                                {
                                    for (int x = 0; x < VScreenX; x += BlockX)
                                    {
                                        bool Changed = false;

                                        for (int by = 0; by < BlockY; by++)
                                        {
                                            for (int bx = 0; bx < BlockX; bx++)
                                            {
                                                int pos = ((by + y) * XSZ) + ((bx + x) * 4);
                                                if (pos >= screen.Data.Length)
                                                    continue;
                                                if (BitConverter.ToInt32(screen.Data, pos) != BitConverter.ToInt32(ScreenRawBytes, pos))
                                                {
                                                    Changed = true;
                                                    break;
                                                }
                                            }
                                            if (Changed == true)
                                                break;
                                        }

                                        if (Changed == true)
                                        {
                                            for (int by = 0; by < BlockY; by++)
                                            {
                                                for (int bx = 0; bx < BlockX; bx++)
                                                {
                                                    int pos = ((by + y) * XSZ) + ((bx + x) * 4);
                                                    if (pos >= screen.Data.Length)
                                                        continue;
                                                    deltadata[pos + 0] = screen.Data[pos + 0];
                                                    deltadata[pos + 1] = screen.Data[pos + 1];
                                                    deltadata[pos + 2] = screen.Data[pos + 2];
                                                    deltadata[pos + 3] = screen.Data[pos + 3];
                                                }
                                            }
                                            data.ChangedBlocks.Add(((Int64)x << 32) | (Int64)y);
                                        }
                                    }
                                }
                            }

                            try
                            {
                                Bitmap bmp = new Bitmap(screen.X, screen.Y, 4 * screen.X, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(deltadata, 0));

                                MemoryStream mem = new MemoryStream();
                                ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
                                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                                myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);

                                bmp.Save(mem, codec, myEncoderParameters);
                                mem.Seek(0, SeekOrigin.Begin);
                                data.Data = new byte[mem.Length];
                                mem.Read(data.Data, 0, (int)mem.Length);

                                //string Filename = "C:\\Temp\\SSDCSC-" + DateTime.Now.ToString("yyyyddMMHHmmss") + ".jpg";
                                //File.WriteAllBytes(Filename, data.Data);
                            }
                            catch (Exception ee)
                            {
                                Debug.WriteLine(ee.ToString());
                                data.X = data.Y = 0;
                                data.FailedCode = -2;
                                return (data);
                            }

                            lock (ScreenLock)
                            {
                                ScreenX = screen.X;
                                ScreenY = screen.Y;
                                ScreenRawBytes = screen.Data;
                            }

                            data.CursorX = screen.CursorX;
                            data.CursorY = screen.CursorY;

                            break;
                        }
                    case 1:
                        {
                            int BlockX = 64;
                            int BlockY = 64;
                            int VScreenX = screen.X + (BlockX - (screen.X % BlockX));
                            int VScreenY = screen.Y + (BlockY - (screen.Y % BlockY));
                            byte[] deltadata = new byte[BlockX * BlockY * 4];

                            do
                            {
                                lock (ScreenLock)
                                {
                                    if (NextSquare > (VScreenX / BlockX) * (VScreenY / BlockY))
                                    {
                                        NextSquare = 0;
                                        //spun around, nothing changed?
                                        data.CursorX = screen.CursorX;
                                        data.CursorY = screen.CursorY;
                                        data.DataType = 3;
                                        data.BlockX = BlockX;
                                        data.BlockY = BlockY;
                                        data.X = screen.X;
                                        data.Y = screen.Y;
                                        break;
                                    }
                                }

                                bool Changed = false;
                                int XSZ = screen.X * 4;

                                int y = NextSquare / (VScreenX / BlockX);
                                int x = NextSquare % (VScreenX / BlockX);
                                x *= BlockX;
                                y *= BlockY;

                                lock (ScreenLock)
                                {
                                    for (int by = 0; by < BlockY; by++)
                                    {
                                        for (int bx = 0; bx < BlockX; bx++)
                                        {
                                            int pos = ((by + y) * XSZ) + ((bx + x) * 4);
                                            if (pos >= screen.Data.Length)
                                                continue;
                                            if (BitConverter.ToInt32(screen.Data, pos) != BitConverter.ToInt32(ScreenRawBytes, pos))
                                            {
                                                Changed = true;
                                                break;
                                            }
                                        }
                                        if (Changed == true)
                                            break;
                                    }

                                    if (Changed == true)
                                    {
                                        for (int by = 0; by < BlockY; by++)
                                        {
                                            for (int bx = 0; bx < BlockX; bx++)
                                            {
                                                int pos = ((by + y) * XSZ) + ((bx + x) * 4);
                                                if (pos >= screen.Data.Length)
                                                    continue;
                                                deltadata[((by * BlockX * 4) + bx * 4) + 0] = screen.Data[pos + 0];
                                                deltadata[((by * BlockX * 4) + bx * 4) + 1] = screen.Data[pos + 1];
                                                deltadata[((by * BlockX * 4) + bx * 4) + 2] = screen.Data[pos + 2];
                                                deltadata[((by * BlockX * 4) + bx * 4) + 3] = screen.Data[pos + 3];

                                                if (pos >= ScreenRawBytes.Length)
                                                    continue;
                                                ScreenRawBytes[pos + 0] = screen.Data[pos + 0];
                                                ScreenRawBytes[pos + 1] = screen.Data[pos + 1];
                                                ScreenRawBytes[pos + 2] = screen.Data[pos + 2];
                                                ScreenRawBytes[pos + 3] = screen.Data[pos + 3];
                                            }
                                        }
                                        data.ChangedBlocks = new List<long>();
                                        data.ChangedBlocks.Add(NextSquare);
                                        NextSquare++;
                                    }
                                    else
                                    {
                                        NextSquare++;
                                        continue;
                                    }
                                }

                                if (Changed == true)
                                {
                                    try
                                    {
                                        Bitmap bmp = new Bitmap(BlockX, BlockY, 4 * BlockX, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(deltadata, 0));

                                        MemoryStream mem = new MemoryStream();
                                        ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
                                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                                        myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);

                                        bmp.Save(mem, codec, myEncoderParameters);
                                        mem.Seek(0, SeekOrigin.Begin);
                                        data.Data = new byte[mem.Length];
                                        mem.Read(data.Data, 0, (int)mem.Length);

                                        //string Filename = "C:\\Temp\\SSDCSC-" + DateTime.Now.ToString("yyyyddMMHHmmss") + ".jpg";
                                        //File.WriteAllBytes(Filename, data.Data);
                                    }
                                    catch (Exception ee)
                                    {
                                        Debug.WriteLine(ee.ToString());
                                        data.X = data.Y = 0;
                                        data.FailedCode = -2;
                                        return (data);
                                    }

                                    data.CursorX = screen.CursorX;
                                    data.CursorY = screen.CursorY;
                                    data.DataType = 2;
                                    data.BlockX = BlockX;
                                    data.BlockY = BlockY;
                                    data.X = screen.X;
                                    data.Y = screen.Y;
                                    break;
                                }

                            } while (true);

                            break;
                        }
                }

                return (data);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.VerboseWriteEventLog(ee.ToString(), EventLogEntryType.Error);
                throw (ee);
            }
        }

        public PushScreenData GetFullscreen()
        {
            try
            {
                MainScreenSystemClient.LastCalled = DateTime.Now;
                PushScreenData data = new PushScreenData();
                CPPFrameBufferData screen = ProgramAgent.CPP.GetFrameBufferData();
                if (screen == null)
                {
                    data.X = data.Y = 0;
                    data.FailedCode = -1;
                    return (data);
                }

                if (screen.Failed == true)
                {
                    Debug.WriteLine("Screendata failed @ " + screen.FailedAt.ToString() + " 0x" + screen.Win32Error.ToString("X"));

                    data.X = data.Y = 0;
                    data.FailedCode = screen.FailedAt;
                    return (data);
                }

                data.X = screen.X;
                data.Y = screen.Y;
                data.CursorX = screen.CursorX;
                data.CursorY = screen.CursorY;
                data.DataType = 0;

                lock (ScreenLock)
                {
                    ScreenX = screen.X;
                    ScreenY = screen.Y;
                    ScreenRawBytes = screen.Data;
                    NextSquare = 0;
                }

                try
                {
                    Bitmap bmp = new Bitmap(screen.X, screen.Y, 4 * screen.X, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(screen.Data, 0));

                    MemoryStream mem = new MemoryStream();
                    ImageCodecInfo codec = GetEncoder(ImageFormat.Jpeg);
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);

                    bmp.Save(mem, codec, myEncoderParameters);
                    mem.Seek(0, SeekOrigin.Begin);
                    data.Data = new byte[mem.Length];
                    mem.Read(data.Data, 0, (int)mem.Length);
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    data.X = data.Y = 0;
                    data.FailedCode = -2;
                    return (data);
                }

                return (data);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.VerboseWriteEventLog(ee.ToString(), EventLogEntryType.Error);
                throw (ee);
            }
        }

        public NetBool SetMousePosition2(int X, int Y, int Delta, int Flags)
        {
            MainScreenSystemClient.LastCalled = DateTime.Now;
            NetBool nb = new NetBool();

            try
            {
                ProgramAgent.CPP.MoveMouse(X, Y, Delta, Flags);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                nb.Data = false;
                return (nb);
            }

            return (nb);
        }

        ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return (codec);
                }
            }
            return (null);
        }

        public NetBool SetKeyboard2(int Flags, int ScanCode, int VirtualKey)
        {
            MainScreenSystemClient.LastCalled = DateTime.Now;
            NetBool nb = new NetBool();

            try
            {
                if (Flags == 0xFFFFFFF && ScanCode == 0xFFFFFFF && VirtualKey == 0xFFFFFFF)
                    ProgramAgent.CPP.SendCTRLALTDELETE();
                else if (VirtualKey == 0xFFFFFFF && ScanCode == 0xFFFFFFE)
                    ProgramAgent.CPP.SetKeyboardLayout(Flags);
                else if (VirtualKey == 0xFFFFFFF && ScanCode == 0xFFFFFFD)
                    ProgramAgent.CPP.TypeKeyboardChar((char)Flags);
                else
                    ProgramAgent.CPP.SetKeyboard(VirtualKey, ScanCode, Flags);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                nb.Data = false;
                return (nb);
            }

            return (nb);
        }
    }

    /// <summary>
    /// Both client & server
    /// note: in this P2P comm - it's inverted ... - the helper-agent starts the pipe server here,
    /// and the main agent connects to it
    /// </summary>
    class MainScreenRedir
    {
        public MainScreenRedir(string GUID)
        {
            this.GUID = GUID;
        }

        ServiceHost serviceHost;
        public PipeScreenDataComm Pipe1;
        public PipeScreenDataComm Pipe2;
        string GUID;
        public void StartPipe()
        {
            NetNamedPipeBinding NETPB = new NetNamedPipeBinding();
            NETPB.MaxReceivedMessageSize = 1048576 * 20;
            NETPB.MaxBufferPoolSize = 1048576 * 20;
            NETPB.MaxBufferSize = 1048576 * 20;

            serviceHost = new ServiceHost(typeof(PipeScreenData), new Uri[] { new Uri("net.pipe://localhost/sdcscreen/FoxSDC-Agent-SCREEN-" + GUID) });
            serviceHost.AddServiceEndpoint(typeof(IPipeScreenData), NETPB, "FoxSDC-Agent-SCREEN-" + GUID);
            serviceHost.OpenTimeout = new TimeSpan(0, 1, 0);
            serviceHost.Open();

            Console.WriteLine("Pipe: FoxSDC-Agent-SCREEN-" + GUID + " created");
        }
    }

    class MainScreenSystem
    {
        static Int64 CurrentConsoleSessionID = -1;
        static string CurrentConnectionGUID = "";
        static MainScreenRedir Pipe;
        static DateTime? LastCalled = null;
        static int PipeProcessID;

        static bool StartApp(string GUID, out int ProcessID)
        {
            ProcessID = 0;
#if !DEBUG || DEBUGSERVICE
            if (ProgramAgent.CPP.StartAppInWinLogon(Assembly.GetExecutingAssembly().Location, "-screen " + GUID, out ProcessID) == false)
            {
                Debug.WriteLine("Cannot start: 0x" + ProgramAgent.CPP.WGetLastError().ToString("X"));
                FoxEventLog.WriteEventLog("Cannot start Screen Capture Program: 0x" + ProgramAgent.CPP.WGetLastError().ToString("X"), EventLogEntryType.Error);
                return (false);
            }
#else
            //Crude: since we cannot copy the token as normal user / nor admin user
            try
            {
                Process proc = Process.Start(Assembly.GetExecutingAssembly().Location, "-screen " + GUID);
                ProcessID = proc.Id;
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
#endif
            return (true);
        }

        static object CheckConnectionLocker = new object();

        static bool ProcessExists(int id)
        {
            return (Process.GetProcesses().Any(x => x.Id == id));
        }

        static bool CheckConnection()
        {
            if (LastCalled != null)
            {
                if ((DateTime.Now - LastCalled.Value).TotalMinutes > 9) //timeout
                    LastCalled = null;
            }

            if (LastCalled == null)
            {
                LastCalled = DateTime.Now;
                CurrentConsoleSessionID = -1;
                CurrentConnectionGUID = "";
            }
            else
            {
                LastCalled = DateTime.Now;
            }

            lock (CheckConnectionLocker)
            {
                FullReset:

                Int64 Con = ProgramAgent.CPP.GetConsoleSessionID();

                if (CurrentConsoleSessionID != Con)
                {
                    if (CurrentConnectionGUID != "")
                    {
                        FoxEventLog.VerboseWriteEventLog("Console Session ID changed from " + CurrentConsoleSessionID.ToString() + " to " + Con.ToString() + " - Closing Pipe #" + CurrentConnectionGUID, EventLogEntryType.Information);
                        try
                        {
                            if (Pipe != null)
                                Pipe.Pipe1.CloseSession();
                            Pipe = null;
                        }
                        catch
                        {

                        }
                        CurrentConnectionGUID = "";
                    }
                }

                CurrentConsoleSessionID = Con;

                if (CurrentConnectionGUID == "")
                {
                    CurrentConnectionGUID = Guid.NewGuid().ToString();
                    FoxEventLog.VerboseWriteEventLog("Starting Screen Capture for Session ID " + CurrentConsoleSessionID.ToString() + " Pipe #" + CurrentConnectionGUID, EventLogEntryType.Information);
                    if (StartApp(CurrentConnectionGUID, out PipeProcessID) == false)
                        return (false);
                    Pipe = new MainScreenRedir(CurrentConnectionGUID);
                    Pipe.Pipe1 = new PipeScreenDataComm(CurrentConnectionGUID);
                    Pipe.Pipe2 = new PipeScreenDataComm(CurrentConnectionGUID);
                }

                int Counter = 0;
                bool Connected = false;
                do
                {
                    if (ProcessExists(PipeProcessID) == false)
                    {
                        FoxEventLog.VerboseWriteEventLog("Screen Capture Process ID " + PipeProcessID.ToString() + " for Session ID " + CurrentConsoleSessionID.ToString() + " Pipe #" + CurrentConnectionGUID + " gone", EventLogEntryType.Warning);
                        Connected = false;
                        if (ProgramAgent.CPP.GetConsoleSessionID() != CurrentConsoleSessionID)
                            goto FullReset;
                        break;
                    }

                    try
                    {
                        if (Pipe.Pipe1.Ping() == true)
                        {
                            Connected = true;
                            break;
                        }
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                    }
                    Thread.Sleep(100);
                    Counter++;
                    if (Counter > 10 * 30)
                        break;
                    Pipe.Pipe1 = new PipeScreenDataComm(CurrentConnectionGUID);
                    Pipe.Pipe2 = new PipeScreenDataComm(CurrentConnectionGUID);
                } while (true);

                if (Connected == false)
                {
                    //Restart the app
                    CurrentConnectionGUID = Guid.NewGuid().ToString();
                    FoxEventLog.VerboseWriteEventLog("Starting Screen Capture for Session ID " + CurrentConsoleSessionID.ToString() + " (2nd try!) Pipe #" + CurrentConnectionGUID, EventLogEntryType.Information);
                    if (StartApp(CurrentConnectionGUID, out PipeProcessID) == false)
                        return (false);
                    Pipe = new MainScreenRedir(CurrentConnectionGUID);
                    Pipe.Pipe1 = new PipeScreenDataComm(CurrentConnectionGUID);
                    Pipe.Pipe2 = new PipeScreenDataComm(CurrentConnectionGUID);
                    Connected = false;
                    do
                    {
                        if (ProcessExists(PipeProcessID) == false)
                        {
                            FoxEventLog.VerboseWriteEventLog("Screen Capture Process ID " + PipeProcessID.ToString() + " for Session ID " + CurrentConsoleSessionID.ToString() + " Pipe #" + CurrentConnectionGUID + " gone", EventLogEntryType.Warning);
                            Connected = false;
                            if (ProgramAgent.CPP.GetConsoleSessionID() != CurrentConsoleSessionID)
                                goto FullReset;
                            break;
                        }

                        try
                        {
                            if (Pipe.Pipe1.Ping() == true)
                            {
                                Connected = true;
                                break;
                            }
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());
                        }
                        Thread.Sleep(100);
                        Counter++;
                        if (Counter > 10 * 30)
                            break;
                        Pipe.Pipe1 = new PipeScreenDataComm(CurrentConnectionGUID);
                        Pipe.Pipe2 = new PipeScreenDataComm(CurrentConnectionGUID);
                    } while (true);
                    if (Connected == false)
                    {
                        FoxEventLog.WriteEventLog("Starting Screen Capture for Session ID " + CurrentConsoleSessionID.ToString() + " failed!", EventLogEntryType.Error);
                        CurrentConnectionGUID = "";
                        return (false);
                    }
                }

                return (true);
            }
        }

        static public NetBool SetKeyboard(int Flags, int ScanCode, int VirtualKey)
        {
            try
            {
                if (Flags == 0xFFFFFFF && ScanCode == 0xFFFFFFF && VirtualKey == 0xFFFFFFF)
                {
                    ProgramAgent.CPP.SendCTRLALTDELETE();
                    return (new NetBool() { Data = true });
                }
            }
            catch
            {

            }

            if (CheckConnection() == false)
                return (new NetBool() { Data = false });

            NetBool b;
            try
            {
                b = Pipe.Pipe1.SetKeyboard(Flags, ScanCode, VirtualKey);
            }
            catch
            {
                return (new NetBool() { Data = false });
            }

            return (b);
        }

        static public NetBool SetMousePosition(int X, int Y, int Delta, int Flags)
        {
            if (CheckConnection() == false)
                return (new NetBool() { Data = false });

            NetBool b;
            try
            {
                b = Pipe.Pipe1.SetMousePosition(X, Y, Delta, Flags);
            }
            catch
            {
                return (new NetBool() { Data = false });
            }
            return (b);
        }

        static public NetBool SetScreen(int ScreenNumber)
        {
            if (CheckConnection() == false)
                return (new NetBool() { Data = false });

            Pipe.Pipe1.SetScreen(ScreenNumber);
            return (new NetBool() { Data = true });
        }

        static public PushScreenData GetFullscreen()
        {
            if (CheckConnection() == false)
                return (new PushScreenData() { FailedCode = 0x50, X = 0, Y = 0 });

            PushScreenData b;
            try
            {
                b = Pipe.Pipe2.GetFullscreen();
            }
            catch
            {
                return (new PushScreenData() { FailedCode = 0x51, X = 0, Y = 0 });
            }

            return (b);
        }

        static byte[] ConvertListInt64ToByte(List<Int64> l)
        {
            byte[] b = new byte[l.Count * 8];

            for (int i = 0; i < l.Count; i++)
            {
                Array.Copy(BitConverter.GetBytes(l[i]), 0, b, i * 8, 8);
            }
            return (b);
        }

        static byte[] ProcessPushScreenData(PushScreenData b)
        {
            PushScreenData2 b2 = new PushScreenData2();
            b2.Header1 = 0x46;
            b2.Header2 = 0x52;
            b2.Header3 = 0x53;
            b2.Header4 = 0x1;
            b2.BlockX = b.BlockX;
            b2.BlockY = b.BlockY;
            b2.CursorX = b.CursorX;
            b2.CursorY = b.CursorY;
            b2.DataSZ = b.Data == null ? 0 : b.Data.LongLength;
            b2.DataType = b.DataType;
            b2.FailedCode = b.FailedCode;
            b2.NumChangedBlocks = b.ChangedBlocks == null ? 0 : b.ChangedBlocks.Count;
            b2.X = b.X;
            b2.Y = b.Y;

            byte[] data = CommonUtilities.Combine(CommonUtilities.Serialize<PushScreenData2>(b2),
                b.Data == null ? new byte[0] : b.Data,
                b.ChangedBlocks == null ? new byte[0] : ConvertListInt64ToByte(b.ChangedBlocks));

            return (data);
        }

        static public byte[] GetFullscreen2()
        {
            PushScreenData b;

            if (CheckConnection() == false)
                b = new PushScreenData() { FailedCode = 0x50, X = 0, Y = 0 };

            try
            {
                b = Pipe.Pipe2.GetFullscreen();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                b = new PushScreenData() { FailedCode = 0x51, X = 0, Y = 0 };
            }

            return (ProcessPushScreenData(b));
        }

        static public PushScreenData GetDeltaScreen()
        {
            if (CheckConnection() == false)
                return (new PushScreenData() { FailedCode = 0x52, X = 0, Y = 0 });

            PushScreenData b;
            try
            {
                b = Pipe.Pipe2.GetDeltaScreen();
            }
            catch
            {
                return (new PushScreenData() { FailedCode = 0x53, X = 0, Y = 0 });
            }

            return (b);
        }

        static public byte[] GetDeltaScreen2()
        {
            PushScreenData b;

            if (CheckConnection() == false)
                b = new PushScreenData() { FailedCode = 0x52, X = 0, Y = 0 };

            try
            {
                b = Pipe.Pipe2.GetDeltaScreen();
            }
            catch
            {
                b = new PushScreenData() { FailedCode = 0x53, X = 0, Y = 0 };
            }

            return (ProcessPushScreenData(b));
        }

    }

    class MainScreenSystemClient
    {
        public static bool Timeout = false;
        public static DateTime? LastCalled = null;
        public static void RunPipeClient()
        {
            MainScreenRedir MR = new MainScreenRedir(ProgramAgent.PipeGUID);
            MR.StartPipe();

            LastCalled = DateTime.Now;
            do
            {
                if (LastCalled != null)
                {
                    if ((DateTime.Now - LastCalled.Value).TotalMinutes > 10)
                        Timeout = true;
                }
                Thread.Sleep(1000);
            } while (Timeout == false);
            Thread.Sleep(1000);
        }
    }
}
