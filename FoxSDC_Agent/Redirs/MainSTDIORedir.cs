using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Redirs
{
    class StdIORedir
    {
        Thread AgentT;
        Thread ConsoleT;
        Network net = null;
        Process consoleproc;
        string Filename;
        string Args;
        string SessionID;

        Thread TScreen;

        Process2ProcessComm consoleendGetTODOStacked;
        Process2ProcessComm consoleendSetResultStacked;

        IntPtr consolestdin;
        IntPtr consolestdout;

        DataHRunConredirRunningData runningdata;
        Push_Stdio_StdOut lastbuffer = new Push_Stdio_StdOut();

        bool BufferChanged(Push_Stdio_StdOut b)
        {
            if (b.CursorX != lastbuffer.CursorX)
                return (true);
            if (b.CursorY != lastbuffer.CursorY)
                return (true);
            if (b.SessionID != lastbuffer.SessionID)
                return (true);
            if (b.WindowsX != lastbuffer.WindowsX)
                return (true);
            if (b.WindowsY != lastbuffer.WindowsY)
                return (true);
            if (lastbuffer.Data == null && b.Data == null)
                return (false); //all match

            if (lastbuffer.Data == null)
                return (true);
            if (b.Data == null)
                return (true);

            if (lastbuffer.Data.Length != b.Data.Length)
                return (true);
            for (int i = 0; i < lastbuffer.Data.Length; i++)
            {
                if (lastbuffer.Data[i].b[0] != b.Data[i].b[0] ||
                    lastbuffer.Data[i].b[1] != b.Data[i].b[1] ||
                    lastbuffer.Data[i].b[2] != b.Data[i].b[2] ||
                    lastbuffer.Data[i].b[3] != b.Data[i].b[3])
                    return (true);
            }

            return (false); //all match
        }

        void PullScreen()
        {
            try
            {
                do
                {
                    ConsoleUtilities.CONSOLE_SCREEN_BUFFER_INFO bufferinfo;
                    if (ConsoleUtilities.GetConsoleScreenBufferInfo(consolestdout, out bufferinfo) == false)
                    {
                        Debug.WriteLine("GetConsoleScreenBufferInfo() failed");
                        Thread.Sleep(1000);
                        continue;
                    }

                    Push_Stdio_StdOut std = new Push_Stdio_StdOut();
                    std.WindowsY = (bufferinfo.srWindow.Bottom - bufferinfo.srWindow.Top) + 1;
                    std.WindowsX = (bufferinfo.srWindow.Right - bufferinfo.srWindow.Left) + 1;

                    ConsoleUtilities.SMALL_RECT r = bufferinfo.srWindow;
                    ConsoleUtilities.COORD csz = new ConsoleUtilities.COORD();
                    csz.X = (short)std.WindowsX;
                    csz.Y = (short)std.WindowsY;
                    ConsoleUtilities.COORD ccoord = new ConsoleUtilities.COORD();
                    ccoord.X = 0; ccoord.Y = 0;

                    ConsoleUtilities.CHAR_INFO2[] info = new ConsoleUtilities.CHAR_INFO2[csz.X * csz.Y];
                    if (ConsoleUtilities.ReadConsoleOutput(consolestdout, info, csz, ccoord, ref r) == false)
                    {
                        Debug.WriteLine("ReadConsoleOutput() failed");
                        Thread.Sleep(1000);
                        continue;
                    }

                    std.CursorX = bufferinfo.dwCursorPosition.X - bufferinfo.srWindow.Left;
                    std.CursorY = bufferinfo.dwCursorPosition.Y - bufferinfo.srWindow.Top;
                    std.SessionID = SessionID;
                    std.State = PushStdoutState.OK;
                    std.Data = info;

                    if (BufferChanged(std) == true)
                    {
                        consoleendSetResultStacked.SetResultStacked(std);
                        lastbuffer = std;
                        Thread.Sleep(10); //a trick to "skip" some pages, due some intense scrolling
                    }
                    else
                    {
                        Thread.Sleep(100); //polling principle :-(
                    }
                } while (consoleproc.HasExited == false);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
            try
            {
                Push_Stdio_StdOut std = new Push_Stdio_StdOut();
                std.State = PushStdoutState.End;
                consoleendSetResultStacked.SetResultStacked(std);
            }
            catch
            {

            }
        }

        public void PushStdinFromAgent(Push_Stdio_StdIn data)
        {
            runningdata.p2pSetTODOStacked.SetTODOStacked(JsonConvert.SerializeObject(data));
        }

        public void PushStdinToConsole(Push_Stdio_StdIn data)
        {
            try
            {
                if (consoleproc.HasExited == true)
                    return;
                switch (data.State)
                {
                    case PushStdInState.Normal:
                        int written;
                        ConsoleUtilities.WriteConsoleInput(consolestdin, data.data, data.data.Length, out written);
                        break;
                    case PushStdInState.CTRL_C:
                        ConsoleUtilities.GenerateConsoleCtrlEvent(ConsoleUtilities.CTRL_C_EVENT, consoleproc.Id);
                        break;
                    case PushStdInState.CTRL_BREAK:
                        ConsoleUtilities.GenerateConsoleCtrlEvent(ConsoleUtilities.CTRL_BREAK_EVENT, consoleproc.Id);
                        break;
                }

            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        void TellMGMTSessionTerminated()
        {
            Push_Stdio_StdOut std = new Push_Stdio_StdOut();
            std.SessionID = SessionID;
            std.State = PushStdoutState.End;
            net.ResponsePushData1(std, "stdio", 0, "stdio-" + SessionID);
            Thread.Sleep(1000);
        }

        void InternalThreadConsoleEnd()
        {
            try
            {
                do
                {
                    string s = consoleendGetTODOStacked.GetTODOStacked();
                    if (string.IsNullOrWhiteSpace(s) == true)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    try
                    {
                        Push_Stdio_StdIn p = JsonConvert.DeserializeObject<Push_Stdio_StdIn>(s);
                        PushStdinToConsole(p);
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        Thread.Sleep(10);
                    }
                } while (consoleproc.HasExited == false);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        void InternalThreadAgentEnd()
        {
            Int64 W32Res;
            if (Process2ProcessComm.InvokeRunConsoleRedir(Filename, Args, out W32Res, out runningdata) == false)
            {
                TellMGMTSessionTerminated();
                MainSTDIORedir.RemoveSession(SessionID);
                return;
            }

            do
            {
                Push_Stdio_StdOut r = runningdata.p2pGetResultStacked.GetResultStacked<Push_Stdio_StdOut>();
                if (r == null)
                {
                    if (runningdata.proc.HasExited == true)
                    {
                        TellMGMTSessionTerminated();
                        MainSTDIORedir.RemoveSession(SessionID);
                        break;
                    }
                }
                else
                {
                    net.ResponsePushData1(r, "stdio", 0, "stdio-" + SessionID);
                }
            } while (true);

            runningdata.p2pGetResultStacked.ClosePipe();
            runningdata.p2pSetTODOStacked.ClosePipe();
        }

        public StdIORedir(string Filename, string Args, string SessionID, Network net)
        {
            this.Filename = Filename;
            this.Args = Args;
            this.SessionID = SessionID;
            this.net = net;
            AgentT = new Thread(new ThreadStart(InternalThreadAgentEnd));
            AgentT.Start();
        }

        public void RunConsoleEnd()
        {
            Console.Clear();
            Console.WriteLine("Fox SDC Redirector on " + Environment.MachineName);

            try
            {
                consoleproc = new Process();
                consoleproc.StartInfo.Arguments = Args;
                consoleproc.StartInfo.FileName = Filename;
                consoleproc.StartInfo.UseShellExecute = false;
                consoleproc.Start();
                consolestdin = ConsoleUtilities.GetStdHandle((int)ConsoleUtilities.StdHandle.STD_INPUT_HANDLE);
                consolestdout = ConsoleUtilities.GetStdHandle((int)ConsoleUtilities.StdHandle.STD_OUTPUT_HANDLE);
                TScreen = new Thread(new ThreadStart(PullScreen));
                ConsoleT = new Thread(new ThreadStart(InternalThreadConsoleEnd));
                TScreen.Start();
                ConsoleT.Start();
                consoleproc.WaitForExit();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                Push_Stdio_StdOut std = new Push_Stdio_StdOut();
                std.SessionID = SessionID;
                std.State = PushStdoutState.End;
                try
                {
                    consoleendSetResultStacked.SetResultStacked(std);
                }
                catch (Exception eee)
                {
                    Debug.WriteLine(eee.ToString());
                }
            }
        }

        public StdIORedir(Process2ProcessComm p2pTODO, Process2ProcessComm p2pResult, DataHRunConredir R)
        {
            this.consoleendSetResultStacked = p2pResult;
            this.consoleendGetTODOStacked = p2pTODO;
            this.Filename = R.File;
            this.Args = R.Args;
        }
    }


    class MainSTDIORedir
    {
        static object Locker = new object();
        static Dictionary<string, StdIORedir> Redirs = new Dictionary<string, StdIORedir>();

        public static void RunPipeConsoleEnd(Process2ProcessComm resp2p, DataHRunConredir R)
        {
            Process2ProcessComm todop2p = new Process2ProcessComm();
            if (todop2p.ConnectPipe(R.TODOPipeGUID) == false)
            {
                Debug.WriteLine("Cannot get 2nd Pipe");
                return;
            }

            StdIORedir rr = new StdIORedir(todop2p, resp2p, R);
            rr.RunConsoleEnd();
        }

        public static string StartRedir(string Filename, string Args, Network net)
        {
            string SessionID = Guid.NewGuid().ToString();
            StdIORedir R = new StdIORedir(Filename, Args, SessionID, net.CloneElement());
            lock (Locker)
            {
                Redirs.Add(SessionID, R);
            }

            return (SessionID);
        }

        public static void RemoveSession(string Session)
        {
            lock (Locker)
            {
                if (Redirs.ContainsKey(Session) == true)
                    Redirs.Remove(Session);
            }
        }

        public static void ProcessStdInAgent(string Data)
        {
            Push_Stdio_StdIn pdata = null;
            StdIORedir redir = null;
            try
            {
                pdata = JsonConvert.DeserializeObject<Push_Stdio_StdIn>(Data);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }

            if (pdata == null)
                return;

            lock (Locker)
            {
                if (Redirs.ContainsKey(pdata.SessionID) == false)
                    return;
                redir = Redirs[pdata.SessionID];
            }

            redir.PushStdinFromAgent(pdata);
        }

    }
}
