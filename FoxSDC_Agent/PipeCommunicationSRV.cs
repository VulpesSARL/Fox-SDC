using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class PipeComm : IPipeCommunication
    {
        public void GetMessage(out long ID, out MessageTypes MessageType, out string Message, out Int64 Number)
        {
            ID = Status.MessageID;
            MessageType = Status.MessageType;
            Message = Status.Message;
            Number = Status.CustomNumber;
        }

        public PushChatMessage PopChatMessage()
        {
            return (Push.PushMain10.PopMessage());
        }

        public void ResponseMessage(long ID, MessageResponse Response)
        {
            if (SyncPolicy.RequestCertPolicyMessageID == ID)
            {
                if (Response == MessageResponse.Button2) //Nö
                {
                    Status.UpdateMessage2();
                    return;
                }
                if (Response == MessageResponse.Button3) //?!?
                    return;
                if (Response == MessageResponse.Button1) //Yes
                {
                    FilesystemData.InstallCertificate(SyncPolicy.RequestCertPolicyCERData);
                    InvokeMessage(MessageInvoke.ReloadPolicies);
                }
            }
        }

        public void InvokeMessage(MessageInvoke what)
        {
            switch (what)
            {
                case MessageInvoke.ReloadPolicies:
                    Threads.InvokePolicySync();
                    break;
                case MessageInvoke.ReloadReports:
                    Threads.InvokeReportingSync();
                    break;
            }
        }

        public List<PackageIDData> GetOptionalSoftware()
        {
            return (FilesystemData.AvailableUserPackages);
        }

        public ServerInfo GetServerInfo()
        {
            return (Utilities.ServerInfo);
        }

        public string GetServerURL()
        {
            return (Utilities.URL);
        }

        public string GetAgentVersion()
        {
            return (FoxVersion.DTS);
        }

        public string GetContract()
        {
            return (SystemInfos.ContractID);
        }

        public void SetOptionalSoftware(string PackageID)
        {
            if (PackageID == null)
                return;
            PackageIDData FoundPackage = null;

            foreach (PackageIDData pkg in FilesystemData.AvailableUserPackages)
            {
                if (PackageID.ToLower() == pkg.PackageID.ToLower())
                {
                    FoundPackage = pkg;
                    break;
                }
            }

            if (FoundPackage == null)
                return;

            bool Found = false;
            foreach (PackageIDData pkg in FilesystemData.UserPackagesToInstall)
            {
                if (pkg.PackageID.ToLower() == FoundPackage.PackageID.ToLower())
                {
                    Found = true;
                    break;
                }
            }

            if (Found == true)
                return;

            FilesystemData.UserPackagesToInstall.Add(FoundPackage);
            FilesystemData.WriteUserPackageList();
        }

        public string GetUCID()
        {
            return (SystemInfos.SysInfo.UCID);
        }

        public string Ping()
        {
            return ("Ping");
        }

        public bool WriteMessage(WriteMessage msg)
        {
            Network net = Utilities.ConnectNetwork(-1);
            if (net == null)
                return (false);
            bool res = net.WriteMessage(msg);
            net.CloseConnection();
            return (res);
        }

        public bool SendChatMessage(string Message)
        {
            Network net = Utilities.ConnectNetwork(-1);
            if (net == null)
                return (false);
            bool res = net.ReportChatMessage("", Message);
            net.CloseConnection();
            return (res);
        }

        public bool ConfirmChatMessage(Int64 ID)
        {
            Network net = Utilities.ConnectNetwork(-1);
            if (net == null)
                return (false);
            bool res = net.ConfirmChat(ID);
            net.CloseConnection();
            return (res);
        }
    }

    class PipeCommunicationSRV
    {
        public static void StartPipeSrv()
        {
            ServiceHost serviceHost = new ServiceHost(typeof(PipeComm), new Uri[] { new Uri("net.pipe://localhost/sdc/") });
            serviceHost.AddServiceEndpoint(typeof(IPipeCommunication), new NetNamedPipeBinding(), "FoxSDC-Agent-Comm");
            serviceHost.OpenTimeout = new TimeSpan(0, 1, 0);
            serviceHost.Open();

            Console.WriteLine("Named pipe on these endpoints:");
            foreach (ServiceEndpoint serviceEndpoint in serviceHost.Description.Endpoints)
            {
                Console.WriteLine("  " + serviceEndpoint.ListenUri.AbsoluteUri);
            }
        }
    }
}
