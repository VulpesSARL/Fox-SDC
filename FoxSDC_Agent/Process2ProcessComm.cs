using FoxSDC_Agent.Redirs;
using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static FoxSDC_Common.PackageInstaller;

namespace FoxSDC_Agent
{
    internal class DataHolder
    {
        public string Action;
        public string TODO;
        public string Result;
        public string GUID;

        public object ResultLock = new object();
        public List<string> ResultStacked = new List<string>();
        public ManualResetEvent ResultMR = new ManualResetEvent(false);

        public object TODOLock = new object();
        public List<string> TODOStacked = new List<string>();
        public ManualResetEvent TODOMR = new ManualResetEvent(false);
    }

    internal class DataHInstallPackageTODO
    {
        public string Filename;
        public List<byte[]> CerCertificates;
        public InstallMode Mode;
        public bool ZipIsMetaOnly;
        public string OtherDLL = "";
    }

    internal class DataHRunasUserTODO
    {
        public string Username;
        public string Password;
        public string Filename;
        public string Args;
    }

    public class DataHRunasUserResult
    {
        public Int64 Result;
    }


    internal class DataHInstallPackageResult
    {
        public bool Return;
        public string ErrorText;
        public PKGStatus res;
        public PKGRecieptData Reciept;
        public string TempDLLFilename;
    }

    internal class DataHRunConredir
    {
        public string File;
        public string Args;
        public string TODOPipeGUID;
    }

    public class DataHRunConredirRunningData
    {
        public string SessionID;
        public Process proc;
        public Process2ProcessComm p2pGetResultStacked;
        public Process2ProcessComm p2pSetTODOStacked;
    }


    [ServiceContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
    interface IProcessComm
    {
        [OperationContract]
        bool Ping();

        [OperationContract]
        string GetTODO();

        [OperationContract]
        bool SetGUID(string GUID);

        [OperationContract]
        void SetResult(string Result);

        [OperationContract]
        string GetAction();

        [OperationContract]
        void SetResultStacked(string Result);

        [OperationContract]
        string GetTODOStacked();
    }

    /// <summary>
    /// Client to Server
    /// </summary>
    class ProcessPipeComm : ClientBase<IProcessComm>
    {
        public ProcessPipeComm(string GUID)
            : base(new ServiceEndpoint(ContractDescription.GetContract(typeof(IProcessComm)),
            new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/sdcp2p/FoxSDC-Agent-P2PC-" + GUID)))
        {
            ((NetNamedPipeBinding)(this.Endpoint.Binding)).MaxReceivedMessageSize = 1048576;
            ((NetNamedPipeBinding)(this.Endpoint.Binding)).MaxBufferPoolSize = 1048576;
            ((NetNamedPipeBinding)(this.Endpoint.Binding)).MaxBufferSize = 1048576;

            if (Channel.SetGUID(GUID) == false)
                throw new Exception("ProcessPipeComm GUID not found");
        }

        public bool Ping()
        {
            return (Channel.Ping());
        }

        public string GetTODO()
        {
            return (Channel.GetTODO());
        }

        public void SetResult(string Result)
        {
            Channel.SetResult(Result);
        }

        public string GetAction()
        {
            return (Channel.GetAction());
        }

        public void SetResultStacked(string res)
        {
            Channel.SetResultStacked(res);
        }

        public string GetTODOStacked()
        {
            return (Channel.GetTODOStacked());
        }
    }

    /// <summary>
    /// Server to Client ("Main" Class)
    /// </summary>
    class ProcessPipeCommReal : IProcessComm
    {
        public static Dictionary<string, DataHolder> Data = new Dictionary<string, DataHolder>();

        string GUID = "";

        public ProcessPipeCommReal()
        {
            Debug.WriteLine("SRV: ProcessPipeCommReal created");
        }

        public bool SetGUID(string guid)
        {
            if (Data.ContainsKey(guid) == false)
                return (false);
            GUID = guid;
            return (true);
        }

        public bool Ping()
        {
            return (true);
        }

        public string GetAction()
        {
            return (Data[GUID].Action);
        }

        public string GetTODO()
        {
            return (Data[GUID].TODO);
        }

        public void SetResult(string Result)
        {
            Data[GUID].Result = Result;
        }

        public void SetResultStacked(string Result)
        {
            lock (Data[GUID].ResultLock)
            {
                Data[GUID].ResultStacked.Add(Result);
                Data[GUID].ResultMR.Set();
            }
        }

        public string GetTODOStacked()
        {
            if (Data.ContainsKey(GUID) == false)
                return (null);
            DataHolder h = Data[GUID];
            int Counter;
            lock (h.TODOLock)
            {
                Counter = h.TODOStacked.Count;
            }

            if (Counter == 0)
                h.TODOMR.WaitOne(30000);

            string r = "";

            lock (h.TODOLock)
            {
                if (h.TODOStacked.Count == 0)
                {
                    h.TODOMR.Reset();
                    return (null);
                }
                r = h.TODOStacked[0];
                h.TODOStacked.RemoveAt(0);
                if (h.TODOStacked.Count == 0)
                    h.TODOMR.Reset();
            }

            if (r == "")
                return (null);
            return (r);
        }
    }

    /// <summary>
    /// To be used on both client & server
    /// </summary>
    public class Process2ProcessComm
    {
        string GUID;
        ServiceHost serviceHost;
        ProcessPipeComm ConnectedPipe;
        public Process2ProcessComm()
        {
            GUID = Guid.NewGuid().ToString();
        }

        public string GetGUID()
        {
            return (GUID);
        }

        public static bool InvokeRunConsoleRedir(string Filename, string Args, out Int64 W32Res, out DataHRunConredirRunningData RunningData)
        {
            DataHRunConredir d = new DataHRunConredir();
            RunningData = new DataHRunConredirRunningData();

            d.File = Filename;
            d.Args = Args;
            W32Res = 0;

            RunningData.p2pSetTODOStacked = new Process2ProcessComm();
            RunningData.p2pSetTODOStacked.SetTODO("nothing", "");
            d.TODOPipeGUID = RunningData.p2pSetTODOStacked.GetGUID();
            RunningData.p2pGetResultStacked = new Process2ProcessComm();
            RunningData.p2pGetResultStacked.SetTODO("CONREDIR", d);
            if (RunningData.p2pGetResultStacked.StartPipe() == false)
            {
                FoxEventLog.WriteEventLog("Cannot start P2PC<1> for CONREDIR " + RunningData.p2pGetResultStacked.GetGUID(), EventLogEntryType.Error);
                return (false);
            }
            if (RunningData.p2pSetTODOStacked.StartPipe() == false)
            {
                RunningData.p2pGetResultStacked.ClosePipe();
                FoxEventLog.WriteEventLog("Cannot start P2PC<2> for CONREDIR " + RunningData.p2pSetTODOStacked.GetGUID(), EventLogEntryType.Error);
                return (false);
            }

            try
            {
                RunningData.proc = new Process();
                RunningData.proc.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                RunningData.proc.StartInfo.Arguments = "-pipeaction " + RunningData.p2pGetResultStacked.GetGUID();
                RunningData.proc.StartInfo.UseShellExecute = true; //new console window!
                RunningData.proc.Start();
            }
            catch (Win32Exception ee)
            {
                W32Res = 0x00000000FFFFFFFF & ee.NativeErrorCode;
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Cannot start P2PC for CONREDIR " + RunningData.p2pGetResultStacked.GetGUID(), EventLogEntryType.Error);
                return (false);
            }
            catch (Exception ee)
            {
                W32Res = 0x8000ffff;
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Cannot start P2PC for CONREDIR " + RunningData.p2pGetResultStacked.GetGUID(), EventLogEntryType.Error);
                return (false);
            }

            return (true);
        }

        public static bool InvokeRunAsUser(string Filename, string Args, string Username, string Password, int SessionID, out DataHRunasUserResult ores, out Int64 W32Res)
        {
            ores = null;
            W32Res = 0xe9;
            DataHRunasUserTODO d = new DataHRunasUserTODO();
            d.Filename = Filename;
            d.Args = Args;
            d.Username = Username;
            d.Password = Password;

            Process2ProcessComm p2p = new Process2ProcessComm();
            p2p.SetTODO("RUNUSER", d);
            if (p2p.StartPipe() == false)
            {
                FoxEventLog.WriteEventLog("Cannot start P2PC for RUNUSER " + p2p.GetGUID(), EventLogEntryType.Error);
                return (false);
            }

            if (SessionID == 0)
            {
                try
                {
                    Process proc = new Process();
                    proc.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                    proc.StartInfo.Arguments = "-pipeaction " + p2p.GetGUID();
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start();
                    proc.WaitForExit();
                }
                catch (Win32Exception ee)
                {
                    W32Res = 0x00000000FFFFFFFF & ee.NativeErrorCode;
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.WriteEventLog("Cannot start P2PC for SYS RUNUSER " + p2p.GetGUID(), EventLogEntryType.Error);
                    return (false);
                }
                catch (Exception ee)
                {
                    W32Res = 0x8000ffff;
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.WriteEventLog("Cannot start P2PC for SYS RUNUSER " + p2p.GetGUID(), EventLogEntryType.Error);
                    return (false);
                }
            }
            else
            {
                if (ProgramAgent.CPP.StartAppAsUserWait(Assembly.GetExecutingAssembly().Location, "-pipeaction " + p2p.GetGUID(), SessionID) == false)
                {
                    W32Res = ProgramAgent.CPP.WGetLastError();
                    FoxEventLog.WriteEventLog("Cannot start P2PC Process for RUNUSER " + p2p.GetGUID() + " Error: 0x" + ProgramAgent.CPP.WGetLastError().ToString("X"), EventLogEntryType.Error);
                    p2p.ClosePipe();
                    return (false);
                }
            }

            ores = p2p.GetResult<DataHRunasUserResult>();
            p2p.ClosePipe();
            if (ores == null)
            {
                FoxEventLog.WriteEventLog("P2PC didn't return any data for RUNUSER " + p2p.GetGUID(), EventLogEntryType.Error);
                return (false);
            }

            return (true);
        }

        public static bool InvokeInstallPackage(string Filename, List<byte[]> CerCertificates, InstallMode Mode, bool ZipIsMetaOnly,
            out string ErrorText, out PKGStatus res, out PKGRecieptData Reciept, string OtherDLL = "")
        {
            ErrorText = "Internal issues";
            res = PKGStatus.Failed;
            Reciept = null;

            DataHInstallPackageTODO inst = new DataHInstallPackageTODO();
            inst.Filename = Filename;
            inst.CerCertificates = CerCertificates;
            inst.Mode = Mode;
            inst.ZipIsMetaOnly = ZipIsMetaOnly;
            inst.OtherDLL = OtherDLL;

            Process2ProcessComm p2p = new Process2ProcessComm();
            p2p.SetTODO("INSTALL", inst);
            if (p2p.StartPipe() == false)
            {
                FoxEventLog.WriteEventLog("Cannot start P2PC for INSTALL " + p2p.GetGUID(), EventLogEntryType.Error);
                return (false);
            }

            Process p = new Process();
            p.StartInfo.Arguments = "-pipeaction " + p2p.GetGUID();
            p.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
            p.StartInfo.UseShellExecute = false;
            if (p.Start() == false)
            {
                FoxEventLog.WriteEventLog("Cannot start P2PC Process for INSTALL " + p2p.GetGUID(), EventLogEntryType.Error);
                p2p.ClosePipe();
                return (false);
            }
            p.WaitForExit();

            DataHInstallPackageResult ores = p2p.GetResult<DataHInstallPackageResult>();
            p2p.ClosePipe();
            if (ores == null)
            {
                FoxEventLog.WriteEventLog("P2PC didn't return any data for INSTALL " + p2p.GetGUID(), EventLogEntryType.Error);
                return (false);
            }

            ErrorText = ores.ErrorText;
            res = ores.res;
            Reciept = ores.Reciept;
            if (ores.TempDLLFilename != null)
            {
                try
                {
                    File.Delete(ores.TempDLLFilename);
                }
                catch
                {

                }
            }

            return (ores.Return);
        }

        public bool ConnectPipe(string pGUID)
        {
            try
            {
                ConnectedPipe = new ProcessPipeComm(pGUID);
                if (ConnectedPipe.Ping() == false)
                    return (false);
            }
            catch
            {
                return (false);
            }
            return (true);
        }

        public T GetResult<T>()
        {
            if (ProcessPipeCommReal.Data.ContainsKey(GUID) == false)
                return (default(T));
            if (ProcessPipeCommReal.Data[GUID].Result == null)
                return (default(T));
            return (JsonConvert.DeserializeObject<T>(ProcessPipeCommReal.Data[GUID].Result));
        }

        public T GetResultStacked<T>()
        {
            if (ProcessPipeCommReal.Data.ContainsKey(GUID) == false)
                return (default(T));
            DataHolder h = ProcessPipeCommReal.Data[GUID];
            int Counter;
            lock (h.ResultLock)
            {
                Counter = h.ResultStacked.Count;
            }

            if (Counter == 0)
                h.ResultMR.WaitOne(30000);

            string r = "";

            lock (h.ResultLock)
            {
                if (h.ResultStacked.Count == 0)
                {
                    h.ResultMR.Reset();
                    return (default(T));
                }
                r = h.ResultStacked[0];
                h.ResultStacked.RemoveAt(0);
                if (h.ResultStacked.Count == 0)
                    h.ResultMR.Reset();
            }

            if (r == "")
                return (default(T));
            return (JsonConvert.DeserializeObject<T>(r));
        }


        public bool StartPipe()
        {
            NetNamedPipeBinding NETPB = new NetNamedPipeBinding();
            NETPB.MaxReceivedMessageSize = 1048576;
            NETPB.MaxBufferPoolSize = 1048576;
            NETPB.MaxBufferSize = 1048576;

            serviceHost = new ServiceHost(typeof(ProcessPipeCommReal), new Uri[] { new Uri("net.pipe://localhost/sdcp2p/") });
            serviceHost.AddServiceEndpoint(typeof(IProcessComm), NETPB, "FoxSDC-Agent-P2PC-" + GUID);
            serviceHost.OpenTimeout = new TimeSpan(0, 1, 0);
            serviceHost.Open();

            Console.WriteLine("Pipe: FoxSDC-Agent-P2PC-" + GUID + " created");
            return (true);
        }

        public bool ClosePipe()
        {
            if (serviceHost != null)
            {
                Console.WriteLine("Pipe: FoxSDC-Agent-P2PC-" + GUID + " closed");
                serviceHost.Close();
            }
            if (ProcessPipeCommReal.Data.ContainsKey(GUID) == true)
            {
                lock (ProcessPipeCommReal.Data)
                {
                    ProcessPipeCommReal.Data.Remove(GUID);
                }
            }
            return (true);
        }

        public T GetTODO<T>()
        {
            string todo = ConnectedPipe.GetTODO();
            return (JsonConvert.DeserializeObject<T>(todo));
        }

        public string GetTODOStacked()
        {
            return (ConnectedPipe.GetTODOStacked());
        }

        public string GetAction()
        {
            return (ConnectedPipe.GetAction());
        }

        public void SetResult(object res)
        {
            ConnectedPipe.SetResult(JsonConvert.SerializeObject(res));
        }

        public void SetResultStacked(string res)
        {
            ConnectedPipe.SetResultStacked(res);
        }

        public void SetResultStacked(object res)
        {
            ConnectedPipe.SetResultStacked(JsonConvert.SerializeObject(res));
        }

        public void SetTODOStacked(string Data)
        {
            lock (ProcessPipeCommReal.Data[GUID].TODOLock)
            {
                ProcessPipeCommReal.Data[GUID].TODOStacked.Add(Data);
                ProcessPipeCommReal.Data[GUID].TODOMR.Set();
            }
        }

        public void SetTODO(string Action, object todo)
        {
            if (ProcessPipeCommReal.Data.ContainsKey(GUID) == false)
            {
                lock (ProcessPipeCommReal.Data)
                {
                    ProcessPipeCommReal.Data.Add(GUID, new DataHolder());
                }
            }

            ProcessPipeCommReal.Data[GUID].Action = Action;
            ProcessPipeCommReal.Data[GUID].GUID = GUID;
            ProcessPipeCommReal.Data[GUID].TODO = JsonConvert.SerializeObject(todo);
        }
    }

    public class Process2ProcessCommClient
    {
        public static void RunPipeClient()
        {
            Process2ProcessComm p2p = new Process2ProcessComm();
            if (p2p.ConnectPipe(ProgramAgent.PipeGUID) == false)
            {
                FoxEventLog.VerboseWriteEventLog("Cannot connect to pipe " + ProgramAgent.PipeGUID, EventLogEntryType.Error);
                return;
            }

            string Action = p2p.GetAction();
            if (Action == null)
            {
                FoxEventLog.VerboseWriteEventLog("Got no action data from pipe " + ProgramAgent.PipeGUID, EventLogEntryType.Error);
                return;
            }

            switch (Action.ToLower())
            {
                case "install":
                    {
                        PackageInstaller inst = new PackageInstaller();
                        DataHInstallPackageTODO todo = p2p.GetTODO<DataHInstallPackageTODO>();
                        if (todo == null)
                        {
                            FoxEventLog.VerboseWriteEventLog("Got no todo data from pipe " + ProgramAgent.PipeGUID, EventLogEntryType.Error);
                            return;
                        }
                        DataHInstallPackageResult result = new DataHInstallPackageResult();

                        result.Return = inst.InstallPackage(todo.Filename, todo.CerCertificates, todo.Mode, todo.ZipIsMetaOnly,
                            out result.ErrorText, out result.res, out result.Reciept, todo.OtherDLL);

                        if (inst.ScriptTempDLLFilename != null)
                            FoxEventLog.VerboseWriteEventLog("Script DLL file = " + inst.ScriptTempDLLFilename, EventLogEntryType.Information);

                        result.TempDLLFilename = inst.ScriptTempDLLFilename;

                        p2p.SetResult(result);
                        break;
                    }
                case "runuser":
                    {
                        DataHRunasUserTODO todo = p2p.GetTODO<DataHRunasUserTODO>();
                        DataHRunasUserResult res = new DataHRunasUserResult();

                        try
                        {
                            Process p = new Process();
                            p.StartInfo.FileName = todo.Filename;
                            p.StartInfo.Arguments = todo.Args;
                            p.StartInfo.UserName = todo.Username;
                            p.StartInfo.Password = Utilities.MakeSecString(todo.Password);
                            p.StartInfo.UseShellExecute = false;
                            p.Start();
                        }
                        catch (Win32Exception ee)
                        {
                            FoxEventLog.VerboseWriteEventLog("RUNUSER: Cannot run " + todo.Filename + " as user " + todo.Username + ": " + ee.ToString(), EventLogEntryType.Warning);
                            res.Result = 0x00000000FFFFFFFF & ee.NativeErrorCode;
                            Debug.WriteLine(ee.ToString());
                        }
                        catch (Exception ee)
                        {
                            FoxEventLog.VerboseWriteEventLog("RUNUSER: Cannot run " + todo.Filename + " as user " + todo.Username + ": " + ee.ToString(), EventLogEntryType.Warning);
                            res.Result = 0x8000ffff;
                            Debug.WriteLine(ee.ToString());
                        }

                        p2p.SetResult(res);
                        break;
                    }
                case "conredir":
                    {
                        DataHRunConredir R = p2p.GetTODO<DataHRunConredir>();
                        MainSTDIORedir.RunPipeConsoleEnd(p2p, R);
                        break;
                    }
                default:
                    FoxEventLog.VerboseWriteEventLog("Action " + Action + " from pipe " + ProgramAgent.PipeGUID + "?? häh???", EventLogEntryType.Warning);
                    return;
            }
        }
    }
}
