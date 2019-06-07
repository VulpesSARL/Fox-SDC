using FoxSDC_Common;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class SyncSimpleTasks
    {
        static void CompleteTask(Network net, SimpleTask st, int Code, string Text)
        {
            SimpleTaskResult res = new SimpleTaskResult();
            res.ID = st.ID;
            res.MachineID = SystemInfos.SysInfo.MachineID;
            res.Name = st.Name;
            res.Result = Code;
            res.Text = Text;
            net.CompleteSimpleTask(res);
        }

        static bool ProcessSimpleTask(Network net, SimpleTask st, out Int64 NewID)
        {
            NewID = -1;
            Status.UpdateMessage(0, "Running Simple Task: " + st.Name);
            try
            {
                switch (st.Type)
                {
                    case 1:
                        #region Run
                        {
                            SimpleTaskRunProgramm rt = JsonConvert.DeserializeObject<SimpleTaskRunProgramm>(st.Data);

                            if (string.IsNullOrWhiteSpace(rt.Executable) == true)
                            {
                                CompleteTask(net, st, 0xFFF1, "Simple Task Data is invalid.");
                                break;
                            }

                            Process proc = new Process();
                            if (string.IsNullOrWhiteSpace(rt.User) == false)
                            {
                                PushRunningSessionList sessions = ProgramAgent.CPP.GetActiveTSSessions();
                                int SessionID = -1;
                                foreach(PushRunningSessionElement session in sessions.Data)
                                {
                                    if (rt.User.ToLower() == (session.Domain + "\\" + session.User).ToLower())
                                    {
                                        SessionID = session.SessionID;
                                        break;
                                    }
                                }

                                if (SessionID == -1)
                                {
                                    Int64? nid = net.PutSimpleTaskAside(st.ID);
                                    if (nid == null)
                                    {
                                        return (false);
                                    }
                                    else
                                    {
                                        NewID = nid.Value;
                                        return (true);
                                    }
                                }

                                int processid = ProgramAgent.CPP.StartAppAsUserID(Environment.ExpandEnvironmentVariables(rt.Executable), rt.Parameters, SessionID);
                                if (processid == -1)
                                {
                                    CompleteTask(net, st, 0xFFF3, "Cannot start process " + rt.Executable);
                                    break;
                                }
                                proc = Process.GetProcessById(processid);
                            }
                            else
                            {
                                try
                                {
                                    proc.StartInfo.UseShellExecute = false;
                                    proc.StartInfo.FileName = Environment.ExpandEnvironmentVariables(rt.Executable);
                                    proc.StartInfo.Arguments = rt.Parameters;
                                    proc.Start();
                                }
                                catch
                                {
                                    CompleteTask(net, st, 0xFFF3, "Cannot start process " + rt.Executable);
                                    break;
                                }
                            }

                            int Counter = 0;
                            do
                            {
                                Thread.Sleep(1000);
                                if (proc.HasExited == true)
                                    break;
                                Counter++;
                                if (Counter % 120 == 0)
                                    net.Ping();
                            } while (Counter < 3600);

                            if (proc.HasExited == false)
                            {
                                proc.Kill();
                                CompleteTask(net, st, 0xFFF2, "Process has been killed, took too long.");
                                break;
                            }
                            
                            CompleteTask(net, st, proc.ExitCode, "Process completed successfully.");
                            break;
                        }
                    #endregion
                    case 2:
                        #region Registry
                        {
                            SimpleTaskRegistry reg = JsonConvert.DeserializeObject<SimpleTaskRegistry>(st.Data);

                            RegistryKey regroot = null;

                            switch (reg.Root)
                            {
                                case 0:
                                    regroot = Registry.LocalMachine;
                                    break;
                                case 1:
                                    regroot = Registry.Users;
                                    break;
                                default:
                                    {
                                        CompleteTask(net, st, 0xFFF2, "Registry Root is invalid 0x" + reg.Action.ToString("X") + ".");
                                        break;
                                    }
                            }

                            if (regroot == null)
                                break;

                            RegistryValueKind regtype = RegistryValueKind.Unknown;

                            switch (reg.ValueType)
                            {
                                case 0:
                                    regtype = RegistryValueKind.String;
                                    break;
                                case 1:
                                    regtype = RegistryValueKind.DWord;
                                    break;
                                case 2:
                                    regtype = RegistryValueKind.QWord;
                                    break;
                                case 3:
                                    regtype = RegistryValueKind.MultiString;
                                    break;
                                case 4:
                                    regtype = RegistryValueKind.ExpandString;
                                    break;
                                case 5:
                                    regtype = RegistryValueKind.Binary;
                                    break;
                                default:
                                    {
                                        CompleteTask(net, st, 0xFFF3, "Registry Value Type is invalid 0x" + reg.Action.ToString("X") + ".");
                                        break;
                                    }
                            }

                            if (regtype == RegistryValueKind.Unknown)
                                break;

                            if (string.IsNullOrWhiteSpace(reg.Folder) == true)
                            {
                                CompleteTask(net, st, 0xFFF4, "Registry Root Folder is missing.");
                                break;
                            }

                            reg.Folder = reg.Folder.Trim();
                            if (reg.Folder.StartsWith("\\") == true)
                                reg.Folder = reg.Folder.Substring(1, reg.Folder.Length - 1);
                            if (reg.Folder.EndsWith("\\") == true)
                                reg.Folder = reg.Folder.Substring(0, reg.Folder.Length - 1);

                            if (string.IsNullOrWhiteSpace(reg.Folder) == true)
                            {
                                CompleteTask(net, st, 0xFFF5, "Registry Root Folder is missing.");
                                break;
                            }

                            if (reg.Root == 1) //HKEY_USERS
                            {
                                string User = reg.Folder.Split('\\')[0].Trim();
                                using (RegistryKey r = regroot.OpenSubKey(User))
                                {
                                    //user not loaded (logged in)
                                    if (r == null)
                                    {
                                        Int64? nid = net.PutSimpleTaskAside(st.ID);
                                        if (nid == null)
                                        {
                                            return (false);
                                        }
                                        else
                                        {
                                            NewID = nid.Value;
                                            return (true);
                                        }
                                    }
                                }
                            }

                            bool Success = false;

                            switch (reg.Action)
                            {
                                case 0: //Add / Update key
                                    {
                                        using (RegistryKey k = regroot.CreateSubKey(reg.Folder))
                                        {
                                            if (k != null)
                                            {
                                                switch (regtype)
                                                {
                                                    case RegistryValueKind.String:
                                                    case RegistryValueKind.ExpandString:
                                                        k.SetValue(reg.Valuename, reg.Data, regtype);
                                                        Success = true;
                                                        break;
                                                    case RegistryValueKind.MultiString:
                                                        k.SetValue(reg.Valuename, reg.Data.Split(new string[] { "\\0" }, StringSplitOptions.None), regtype);
                                                        Success = true;
                                                        break;
                                                    case RegistryValueKind.DWord:
                                                        {
                                                            int dword;
                                                            if (reg.Data.ToLower().StartsWith("0x") == true)
                                                            {
                                                                if (int.TryParse(reg.Data.Substring(2, reg.Data.Length - 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out dword) == false)
                                                                {
                                                                    CompleteTask(net, st, 0xFFF7, "Invalid data.");
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (int.TryParse(reg.Data, out dword) == false)
                                                                {
                                                                    CompleteTask(net, st, 0xFFF7, "Invalid data.");
                                                                    break;
                                                                }
                                                            }
                                                            k.SetValue(reg.Valuename, dword, regtype);
                                                            Success = true;
                                                        }
                                                        break;
                                                    case RegistryValueKind.QWord:
                                                        {
                                                            Int64 dword;
                                                            if (reg.Data.ToLower().StartsWith("0x") == true)
                                                            {
                                                                if (Int64.TryParse(reg.Data.Substring(2, reg.Data.Length - 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out dword) == false)
                                                                {
                                                                    CompleteTask(net, st, 0xFFF7, "Invalid data.");
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Int64.TryParse(reg.Data, out dword) == false)
                                                                {
                                                                    CompleteTask(net, st, 0xFFF7, "Invalid data.");
                                                                    break;
                                                                }
                                                            }
                                                            k.SetValue(reg.Valuename, dword, regtype);
                                                            Success = true;
                                                        }
                                                        break;
                                                    case RegistryValueKind.Binary:
                                                        {
                                                            if (reg.Data.Length % 2 != 0)
                                                            {
                                                                CompleteTask(net, st, 0xFFF7, "Invalid data.");
                                                                break;
                                                            }
                                                            List<byte> bytedata = new List<byte>();
                                                            for (int i = 0; i < reg.Data.Length; i += 2)
                                                            {
                                                                int btmp;
                                                                if (int.TryParse(reg.Data.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out btmp) == false)
                                                                {
                                                                    CompleteTask(net, st, 0xFFF7, "Invalid data.");
                                                                    break;
                                                                }
                                                                bytedata.Add((byte)btmp);
                                                            }
                                                            k.SetValue(reg.Valuename, bytedata.ToArray(), regtype);
                                                            Success = true;
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                CompleteTask(net, st, 0xFFF6, "Cannot open registry " + reg.Folder + ".");
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case 1: //Delete value
                                    {
                                        using (RegistryKey k = regroot.OpenSubKey(reg.Folder))
                                        {
                                            if (k != null)
                                            {
                                                k.DeleteValue(reg.Valuename, false);
                                                Success = true;
                                            }
                                            else
                                            {
                                                CompleteTask(net, st, 0xFFF6, "Cannot open registry " + reg.Folder + ".");
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case 2: //Delete directory
                                    {
                                        string f = reg.Folder.Substring(0, reg.Folder.LastIndexOf('\\'));
                                        string v = reg.Folder.Substring(reg.Folder.LastIndexOf('\\') + 1);

                                        using (RegistryKey k = regroot.OpenSubKey(f))
                                        {
                                            if (k != null)
                                            {
                                                k.DeleteSubKeyTree(v, false);
                                                Success = true;
                                            }
                                            else
                                            {
                                                CompleteTask(net, st, 0xFFF6, "Cannot open registry " + f + ".");
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        CompleteTask(net, st, 0xFFF1, "Registry Action is invalid 0x" + reg.Action.ToString("X") + ".");
                                        break;
                                    }
                            }

                            if (Success == true)
                                CompleteTask(net, st, 0, "Registry process completed successfully.");
                            break;
                        }
                    #endregion
                    default:
                        {
                            CompleteTask(net, st, 0xFFFF, "Does not know how to process Simple Task Type 0x" + st.Type.ToString("X") + ".");
                            break;
                        }
                }
            }
            catch (Exception ee)
            {
                CompleteTask(net, st, 0xFFF0, "SEH while processing Simple Task: " + ee.ToString());
                return (false);
            }

            Status.UpdateMessage(0, "Completed Simple Task: " + st.Name);
            return (true);
        }

        public static bool DoSyncSimpleTasks()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);

                Status.UpdateMessage(0, "Checking Simple Tasks");

                Int64 Aside = -1;

                SimpleTaskDataSigned st = null;
                do
                {
                    st = net.GetSimpleTaskSigned();
                    if (st == null)
                        break;
                    if (ApplicationCertificate.Verify(st) == false)
                    {
                        FoxEventLog.WriteEventLog("One or more Simple Tasks are tampered - no tasks will be processed.", System.Diagnostics.EventLogEntryType.Error);
                        break;
                    }
                    if (Aside != -1)
                        if (Aside == st.STask.ID)
                            break;

                    Int64 AID;
                    if (ProcessSimpleTask(net, st.STask, out AID) == false)
                        break;
                    if (AID != -1)
                        Aside = AID;

                } while (st != null);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while running Simple Tasks: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }
    }
}
