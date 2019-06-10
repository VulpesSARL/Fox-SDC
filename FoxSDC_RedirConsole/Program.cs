using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_RedirConsole
{
    class Program
    {
        static Network net = null;
        static Thread RecvData;
        static string StdIOSession;
        static string MachineID;
        static bool End = false;
        static bool res;

        static IntPtr StdInHandle;
        static IntPtr StdOutHandle;

        static bool EnableMouseInput = false;
        static bool LoopPing = false;

        static Push_Stdio_StdOut LastBuffer = new Push_Stdio_StdOut();

        static void RunDirect(string Server, string MID, string SessionID, string StdIOSessionID)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Connecting to server: " + Server);

            net = new Network();
            if (net.SetSessionID(Server, SessionID) == false)
            {
                Console.WriteLine("Failed!");
                Console.ForegroundColor = c;
                net = null;
                return;
            }
#if DEBUG
            Console.WriteLine("MachineID:      " + MID);
            Console.WriteLine("SessionID:      " + SessionID);
            Console.WriteLine("STDIOSessionID: " + StdIOSessionID);
#endif
            Console.WriteLine("Connected.");
            Console.ForegroundColor = c;
            Thread.Sleep(1000);
            StdIOSession = StdIOSessionID;
            MachineID = MID;
        }

        static void RunRemoteApp(string Server, string MID, string Username, string Password, string Program, string Args)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

#if DEBUG
            Console.WriteLine("MachineID:      " + MID);
#endif

            if (Password == "*")
            {
                IntPtr hwnd = ConsoleUtilities.GetConsoleWindow();
                frmPassword pwd = new frmPassword(Server, Username);
                if (pwd.ShowDialog(new WindowWrapper(hwnd)) != System.Windows.Forms.DialogResult.OK)
                {
                    Console.ForegroundColor = c;
                    return;
                }
                Password = pwd.Password;
            }

            Console.WriteLine("Connecting to server: " + Server);

            net = new Network();
            if (net.Connect(Server) == false)
            {
                Console.ForegroundColor = c;
                net = null;
                return;
            }

            if (net.Login(Username, Password) == false)
            {
                if (net.LoginError == null)
                    Console.WriteLine("Login failed: ???");
                else
                    Console.WriteLine("Login failed: " + net.LoginError.Error);
                Console.ForegroundColor = c;
                net = null;
                return;
            }

            if (LoopPing == false)
            {
                if (net.PushPing(MID) == false)
                {
                    Console.WriteLine("Remote machine does not respond to \"Ping\"");
                    Console.ForegroundColor = c;
                    net.CloseConnection();
                    net = null;
                    return;
                }
            }
            else
            {
                Console.CancelKeyPress += Console_CancelKeyPress_BreakPing;
                do
                {
                    if (net.PushPing(MID) == false)
                    {
                        Console.WriteLine("Remote machine does not respond to \"Ping\" - Retrying");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        break;
                    }
                } while (LoopPing == true);
                Console.CancelKeyPress -= Console_CancelKeyPress_BreakPing;
                if (LoopPing == false)
                {
                    Console.ForegroundColor = c;
                    net.CloseConnection();
                    net = null;
                    return;
                }
            }

            PushRunTask rt = new PushRunTask();
            rt.Executable = Program;
            rt.Args = Args;
            rt.Option = PushRunTaskOption.SystemUserConsoleRedir;

            PushRunTaskResult rtres = net.PushRunFile(MID, rt);
            if (rtres == null)
            {
                Console.WriteLine("Cannot start application: ???");
                Console.ForegroundColor = c;
                net.CloseConnection();
                net = null;
                return;
            }
            if (rtres.Result != 0)
            {
                string errorMessage = new Win32Exception((int)(rtres.Result)).Message;
                Console.WriteLine("Cannot start application: 0x" + rtres.Result.ToString("X") + " " + errorMessage);
                Console.ForegroundColor = c;
                net.CloseConnection();
                net = null;
                return;
            }

            StdIOSession = rtres.SessionID;
            MachineID = MID;

#if DEBUG
            Console.WriteLine("SessionID:      " + net.Session);
            Console.WriteLine("STDIOSessionID: " + StdIOSession);
#endif

            Console.WriteLine("Connected.");
            Console.ForegroundColor = c;
            Thread.Sleep(1000);
        }

        private static void Console_CancelKeyPress_BreakPing(object sender, ConsoleCancelEventArgs e)
        {
            LoopPing = false;
            e.Cancel = true;
        }

        static void RunRedir()
        {
            if (net == null)
                return;

            Console.CancelKeyPress += Console_CancelKeyPress;

            StdInHandle = ConsoleUtilities.GetStdHandle((int)ConsoleUtilities.StdHandle.STD_INPUT_HANDLE);
            StdOutHandle = ConsoleUtilities.GetStdHandle((int)ConsoleUtilities.StdHandle.STD_OUTPUT_HANDLE);

            int OldFlags;
            ConsoleUtilities.GetConsoleMode(StdInHandle, out OldFlags);
            int NewFlags = OldFlags & ~(int)(ConsoleUtilities.ConsoleModes.ENABLE_ECHO_INPUT | ConsoleUtilities.ConsoleModes.ENABLE_LINE_INPUT | ConsoleUtilities.ConsoleModes.ENABLE_PROCESSED_INPUT);
            ConsoleUtilities.SetConsoleMode(StdInHandle, NewFlags);

            RecvData = new Thread(new ThreadStart(RecvDataThread));
            RecvData.Start();

            do
            {
                ConsoleUtilities.INPUT_RECORD_Flat[] ir = new ConsoleUtilities.INPUT_RECORD_Flat[128];
                int read;
                if (ConsoleUtilities.ReadConsoleInput(StdInHandle, ir, 128, out read) == false)
                {
                    Debug.WriteLine("Err ReadConsoleInput()");
                    continue;
                }
                if (read == 0)
                {
                    Debug.WriteLine("Nix ReadConsoleInput()");
                    continue;
                }

                List<ConsoleUtilities.INPUT_RECORD_Flat> ir2 = new List<ConsoleUtilities.INPUT_RECORD_Flat>();
                for (int i = 0; i < read; i++)
                {
                    switch (BitConverter.ToInt16(ir[i].d, 0))
                    {
                        case 0x1: //KEY_EVENT 
                            ir2.Add(ir[i]);
                            break;
                        case 0x2: //MOUSE_EVENT
                            if (EnableMouseInput == true)
                                ir2.Add(ir[i]);
                            break;
                    }
                }

                net.PushPushStdIO(MachineID, StdIOSession, ir2.ToArray());

            } while (End == false);

            ConsoleUtilities.SetConsoleMode(StdInHandle, OldFlags);
        }

        static void RecvDataThread()
        {
            Network n = net.CloneElement();

            do
            {
                Push_Stdio_StdOut o = n.PushPopStdIO(MachineID, StdIOSession);
                if (o == null)
                {
                    Debug.WriteLine("Push_Stdio_StdOut==null");
                    Thread.Sleep(500);
                    continue;
                }
                switch (o.State)
                {
                    case PushStdoutState.Timeout:
                        Debug.WriteLine("Push_Stdio_StdOut.State==Timeout");
                        break;
                    case PushStdoutState.InternalError:
                        Debug.WriteLine("Push_Stdio_StdOut.State==InternalError");
                        Thread.Sleep(500);
                        break;
                    case PushStdoutState.End:
                        Debug.WriteLine("Push_Stdio_StdOut.State==End");
                        ConsoleColor c = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Remote process terminated.");
                        Thread.Sleep(500);
                        Console.ForegroundColor = c;
                        End = true;
                        return;
                    case PushStdoutState.OK:
                        ProcessConsoleData(o);
                        break;
                }
            } while (End == false);
        }

        private static void ProcessConsoleData(Push_Stdio_StdOut std)
        {
            if (std.WindowsX != LastBuffer.WindowsX || std.WindowsY != LastBuffer.WindowsY)
            {
                ConsoleUtilities.CONSOLE_SCREEN_BUFFER_INFO_EX ex = new ConsoleUtilities.CONSOLE_SCREEN_BUFFER_INFO_EX();
                ex.cbSize = 96;
                res = ConsoleUtilities.GetConsoleScreenBufferInfoEx(StdOutHandle, ref ex);
                ex.dwMaximumWindowSize.X = ex.dwSize.X = (short)std.WindowsX;
                ex.dwMaximumWindowSize.Y = ex.dwSize.Y = (short)std.WindowsY;
                ex.srWindow.Top = ex.srWindow.Left = 0;
                ex.srWindow.Right = (short)std.WindowsX;
                ex.srWindow.Bottom = (short)std.WindowsY;
                res = ConsoleUtilities.SetConsoleScreenBufferInfoEx(StdOutHandle, ref ex);
            }

            if (std.CursorX != LastBuffer.CursorX || std.CursorY != LastBuffer.CursorY)
            {
                ConsoleUtilities.COORD c = new ConsoleUtilities.COORD();
                c.X = (short)std.CursorX;
                c.Y = (short)std.CursorY;
                res = ConsoleUtilities.SetConsoleCursorPosition(StdOutHandle, c);
            }

            if (std.Data == null)
                std.Data = new ConsoleUtilities.CHAR_INFO2[1];
            if (LastBuffer.Data == null)
                LastBuffer.Data = new ConsoleUtilities.CHAR_INFO2[1];

            bool DataModified = false;

            if (std.Data.Length != LastBuffer.Data.Length)
                DataModified = true;
            if (DataModified == false)
            {
                for (int i = 0; i < std.Data.Length; i++)
                {
                    if (LastBuffer.Data[i].b[0] != std.Data[i].b[0] ||
                        LastBuffer.Data[i].b[1] != std.Data[i].b[1] ||
                        LastBuffer.Data[i].b[2] != std.Data[i].b[2] ||
                        LastBuffer.Data[i].b[3] != std.Data[i].b[3])
                    {
                        DataModified = true;
                        break;
                    }
                }
            }

            if (DataModified == true)
            {
                ConsoleUtilities.COORD c = new ConsoleUtilities.COORD();
                c.X = (short)std.WindowsX;
                c.Y = (short)std.WindowsY;
                ConsoleUtilities.COORD p = new ConsoleUtilities.COORD();
                p.X = p.Y = 0;
                ConsoleUtilities.SMALL_RECT r = new ConsoleUtilities.SMALL_RECT();
                r.Left = r.Top = 0;
                r.Right = (short)std.WindowsX;
                r.Bottom = (short)std.WindowsY;
                res = ConsoleUtilities.WriteConsoleOutput(StdOutHandle, std.Data, c, p, ref r);
            }

            LastBuffer = std;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Push_Stdio_StdIn s = new Push_Stdio_StdIn();
            s.data = null;
            s.SessionID = StdIOSession;
            switch (e.SpecialKey)
            {
                case ConsoleSpecialKey.ControlC:
                    s.State = PushStdInState.CTRL_C;
                    break;
                case ConsoleSpecialKey.ControlBreak:
                    s.State = PushStdInState.CTRL_BREAK;
                    break;
            }
            net.PushPushStdIO(MachineID, s);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(VulpesBranding.REDIRTitle);

            if (Console.IsErrorRedirected == true || Console.IsInputRedirected == true || Console.IsOutputRedirected == true)
            {
                Console.WriteLine("");
                Console.WriteLine("This program does not work on a redirected console.");
                return;
            }

            if (args.Length < 1)
            {
                Console.WriteLine("");
                Console.WriteLine(" -server");
                Console.WriteLine("    Server MachineID Username {Password|*} RemoteProgram Args");
                Console.WriteLine(" -direct");
                Console.WriteLine("    used internally only.");
                return;
            }

            RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Fox\\SDC_MGMT");
            if (reg != null)
            {
                object o = reg.GetValue("EnableConMouse", 0);
                int dword;
                if (int.TryParse(o.ToString(), out dword) == true)
                {
                    if (dword == 1)
                        EnableMouseInput = true;
                }

                o = reg.GetValue("LoopPing", 0);
                if (int.TryParse(o.ToString(), out dword) == true)
                {
                    if (dword == 1)
                        LoopPing = true;
                }

                reg.Close();
            }

            switch (args[0].ToLower())
            {
                case "-direct":
                    if (args.Length < 5)
                    {
                        Console.WriteLine("To few args.");
                        return;
                    }
                    RunDirect(args[1], args[2], args[3], args[4]);
                    if (net == null)
                        break;
                    RunRedir();
                    break;
                case "-server":
                    if (args.Length < 6)
                    {
                        Console.WriteLine("To few args.");
                        return;
                    }
                    string aargs = "";
                    for (int i = 5; i < args.Length; i++)
                    {
                        aargs += args[i] + " ";
                    }
                    RunRemoteApp(args[1], args[2], args[3], args[4], args[5], aargs);
                    if (net == null)
                        break;
                    RunRedir();
                    break;
            }

            if (net != null)
                net.CloseConnection();

        }
    }
}
