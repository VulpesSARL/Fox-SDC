using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net.Security;

namespace FoxSDC_Common
{
    public enum MessageTypes
    {
        Nix,
        PlainTextMessage,
        StatusMessage,
        CertificateAcceptanceMessage,
        NotConnected
    }

    public enum MessageResponse
    {
        Button1,
        Button2,
        Button3
    }

    public enum MessageInvoke
    {
        ReloadPolicies,
        ReloadReports
    }

    [ServiceContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
    public interface IPipeCommunication
    {
        [OperationContract]
        void GetMessage(out Int64 ID, out MessageTypes MessageType, out string Message, out Int64 Number);
        [OperationContract]
        void ResponseMessage(Int64 ID, MessageResponse Response);
        [OperationContract]
        void InvokeMessage(MessageInvoke what);

        [OperationContract]
        List<PackageIDData> GetOptionalSoftware();

        [OperationContract]
        void SetOptionalSoftware(string PackageID);

        [OperationContract]
        bool WriteMessage(WriteMessage msg);

        [OperationContract]
        string GetServerURL();

        [OperationContract]
        ServerInfo GetServerInfo();

        [OperationContract]
        string GetAgentVersion();

        [OperationContract]
        string GetUCID();

        [OperationContract]
        string GetContract();

        [OperationContract]
        string Ping();

#if ENABLECHAT
        [OperationContract]
        PushChatMessage PopChatMessage();

        [OperationContract]
        bool SendChatMessage(string Message);

        [OperationContract]
        bool ConfirmChatMessage(Int64 ID);
#endif
    }

    public class PipeCommunication : ClientBase<IPipeCommunication>
    {
        public PipeCommunication()
            : base(new ServiceEndpoint(ContractDescription.GetContract(typeof(IPipeCommunication)),
            new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/sdc/FoxSDC-Agent-Comm")))
        {
            
        }

        public void GetMessage(out Int64 ID, out MessageTypes MessageType, out string Message, out Int64 Number)
        {
            Channel.GetMessage(out ID, out MessageType, out Message, out Number);
        }

        public void ResponseMessage(Int64 ID, MessageResponse Response)
        {
            Channel.ResponseMessage(ID, Response);
        }

        public void InvokeMessage(MessageInvoke what)
        {
            Channel.InvokeMessage(what);
        }

        public List<PackageIDData> GetOptionalSoftware()
        {
            return (Channel.GetOptionalSoftware());
        }

        public void SetOptionalSoftware(string PackageID)
        {
            Channel.SetOptionalSoftware(PackageID);
        }

        public bool WriteMessage(WriteMessage msg)
        {
            return(Channel.WriteMessage(msg));
        }

        public ServerInfo GetServerInfo()
        {
            return (Channel.GetServerInfo());
        }

        public string GetServerURL()
        {
            return (Channel.GetServerURL());
        }

        public string GetAgentVersion()
        {
            return (Channel.GetAgentVersion());
        }

        public string GetUCID()
        {
            return (Channel.GetUCID());
        }

        public string GetContract()
        {
            return (Channel.GetContract());
        }

        public string Ping()
        {
            return (Channel.Ping());
        }

#if ENABLECHAT
        public PushChatMessage PopChatMessage()
        {
            return (Channel.PopChatMessage());
        }

        public bool SendChatMessage(string Message)
        {
            return (Channel.SendChatMessage(Message));
        }

        public bool ConfirmChatMessage(Int64 ID)
        {
            return (Channel.ConfirmChatMessage(ID));
        }
#endif
    }
}
