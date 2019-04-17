using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class Status
    {
        public static Int64 MessageID = 0;
        public static MessageTypes MessageType = MessageTypes.Nix;
        public static string Message = "";
        public static Int64 CustomNumber = 0;

        static Dictionary<int, string> ChannelMessages = new Dictionary<int, string>();
        static object ChannelMessagesLock = new object();

        [Obsolete]
        public static void UpdateMessage(string Message)
        {
            Status.MessageID += 1;
            Status.Message = Message;
            Status.CustomNumber = 0;
            Status.MessageType = MessageTypes.PlainTextMessage;
        }

        public static void UpdateMessage(int Channel, string Message)
        {
            lock (ChannelMessagesLock)
            {
                if (ChannelMessages.ContainsKey(Channel) == false)
                    ChannelMessages.Add(Channel, "");
            }

            lock (ChannelMessagesLock)
            {
                ChannelMessages[Channel] = Message;
            }

            string CumulMessage = "";

            foreach (KeyValuePair<int, string> kvp in ChannelMessages.OrderBy(s => s.Key))
            {
                if (string.IsNullOrWhiteSpace(kvp.Value) == true)
                    continue;
                CumulMessage += kvp.Value.ToString() + "\r\n";
            }

            CumulMessage = CumulMessage.Trim();

            Status.MessageID += 1;
            Status.Message = CumulMessage;
            Status.CustomNumber = 0;
            Status.MessageType = MessageTypes.PlainTextMessage;
        }

        public static void UpdateMessage(string Message, int Percent)
        {
            Status.MessageID += 1;
            Status.Message = Message;
            Status.CustomNumber = Percent;
            Status.MessageType = MessageTypes.StatusMessage;
        }

        public static void RequestCertificateConfirm(string Message, Int64 ID)
        {
            Status.MessageID += 1;
            Status.Message = Message;
            Status.CustomNumber = ID;
            Status.MessageType = MessageTypes.CertificateAcceptanceMessage;
        }

        [Obsolete]
        public static void UpdateMessage()
        {
            Status.MessageType = MessageTypes.Nix;
        }

        public static void UpdateMessage2()
        {
            Status.MessageType = MessageTypes.Nix;
        }

        public static void UpdateMessage(int Channel)
        {
            UpdateMessage(Channel, "");
        }
    }
}
