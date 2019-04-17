using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent_UI
{
    class Status
    {
        static PipeCommunication PC = null;
        static Int64 ID = 0;
        static MessageTypes MsgType = MessageTypes.NotConnected;
        static string Message = "";
        static Int64 CustomNumber = 0;
        static bool PingOK = false;
        const string CannotConnect = "(there is no connection to the agent service -- make sure that the service is running)";
        public static bool HasIssues = false;

        static void ConnectThread()
        {
            PC = new PipeCommunication();
        }

        static void PingThread()
        {
            PingOK = false;
            if (PC == null)
                return;
            try
            {
                if (PC.Ping() != "Ping")
                    PingOK = false;
                else
                    PingOK = true;
            }
            catch
            {
                PingOK = false;
            }
        }

        static bool Connect()
        {
            PC = null;
            Thread t = new Thread(ConnectThread);
            t.Start();
            if (t.Join(5000) == false)
            {
                t.Abort();
                PC = null;
                return (false);
            }
            return (true);
        }

        static bool Ping()
        {
            PingOK = false;
            Thread t = new Thread(PingThread);
            t.Start();
            if (t.Join(5000) == false)
            {
                t.Abort();
                PC = null;
                return (false);
            }
            if (PingOK == true)
            {
                return (true);
            }
            else
            {
                PC = null;
                return (false);
            }
        }

        static public List<PackageIDData> GetOptionalSoftware()
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (null);
                    }
                }

                return (PC.GetOptionalSoftware());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        static public ServerInfo GetServerInfo()
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (null);
                    }
                }

                return (PC.GetServerInfo());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        static public string GetServerURL()
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (null);
                    }
                }

                return (PC.GetServerURL());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        static public string GetAgentVersion()
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (null);
                    }
                }

                return (PC.GetAgentVersion());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        static public PushChatMessage PopChatMessage()
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (null);
                    }
                }

                return (PC.PopChatMessage());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        static public bool SendChatMessage(string Message)
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (false);
                    }
                }

                return (PC.SendChatMessage(Message));
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        public static bool ConfirmChatMessage(Int64 ID)
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (false);
                    }
                }

                return (PC.ConfirmChatMessage(ID));
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        static public string GetUCID()
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (null);
                    }
                }

                return (PC.GetUCID());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        static public string GetContract()
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (null);
                    }
                }

                return (PC.GetContract());
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        static public void SetOptionalSoftware(string PackageID)
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return;
                    }
                }

                PC.SetOptionalSoftware(PackageID);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return;
            }
        }

        static public bool WriteMessage(WriteMessage msg)
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        return (false);
                    }
                }

                return (PC.WriteMessage(msg));
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        static public void InvokeUpdate(MessageInvoke what)
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        MsgType = MessageTypes.NotConnected;
                        ID = 0;
                        MsgType = MessageTypes.Nix;
                        Message = "";
                        CustomNumber = 0;
                        return;
                    }
                }

                PC.InvokeMessage(what);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        static public void ResponseMessage(MainDLG dlg, MessageResponse Response)
        {
            try
            {
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        MsgType = MessageTypes.NotConnected;
                        ID = 0;
                        MsgType = MessageTypes.Nix;
                        Message = "";
                        CustomNumber = 0;
                        return;
                    }
                }

                PC.ResponseMessage(ID, Response);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        static public void UpdateStatus(MainDLG dlg)
        {
            try
            {
                HasIssues = false;
                Ping();
                if (PC == null)
                {
                    if (Connect() == false)
                    {
                        Debug.WriteLine("SDC_Agent_UI Thread abort");
                        PC = null;
                        MsgType = MessageTypes.NotConnected;
                        ID = 0;
                        MsgType = MessageTypes.Nix;
                        Message = "";
                        CustomNumber = 0;
                    }
                }

                PC.GetMessage(out ID, out MsgType, out Message, out CustomNumber);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                PC = null;
                MsgType = MessageTypes.NotConnected;
                ID = 0;
                Message = "(there is no connection to the agent service -- make sure that the service is running)";
                CustomNumber = 0;
                HasIssues = true;
            }

            switch (MsgType)
            {
                case MessageTypes.Nix:
                    dlg.cmdButton1.Enabled = dlg.cmdButton2.Enabled = dlg.cmdButton3.Enabled = false;
                    dlg.cmdButton1.Visible = dlg.cmdButton2.Visible = dlg.cmdButton3.Visible = false;
                    if (dlg.txtMessage.Text != "")
                    {
                        dlg.txtMessage.Text = "";
                        dlg.txtMessage.SelectionStart = 0;
                        dlg.txtMessage.SelectionLength = 0;
                    }
                    dlg.progress.Visible = false;
                    break;
                case MessageTypes.NotConnected:
                    dlg.cmdButton1.Enabled = dlg.cmdButton2.Enabled = dlg.cmdButton3.Enabled = false;
                    dlg.cmdButton1.Visible = dlg.cmdButton2.Visible = dlg.cmdButton3.Visible = false;
                    if (dlg.txtMessage.Text != CannotConnect)
                    {
                        dlg.txtMessage.Text = CannotConnect;
                        dlg.txtMessage.SelectionStart = 0;
                        dlg.txtMessage.SelectionLength = 0;
                    }
                    dlg.progress.Visible = false;
                    break;
                case MessageTypes.PlainTextMessage:
                    dlg.cmdButton1.Enabled = dlg.cmdButton2.Enabled = dlg.cmdButton3.Enabled = false;
                    dlg.cmdButton1.Visible = dlg.cmdButton2.Visible = dlg.cmdButton3.Visible = false;
                    if (dlg.txtMessage.Text != Message)
                    {
                        dlg.txtMessage.Text = Message;
                        dlg.txtMessage.SelectionStart = 0;
                        dlg.txtMessage.SelectionLength = 0;
                    }
                    dlg.progress.Visible = false;
                    break;
                case MessageTypes.StatusMessage:
                    dlg.cmdButton1.Enabled = dlg.cmdButton2.Enabled = dlg.cmdButton3.Enabled = false;
                    dlg.cmdButton1.Visible = dlg.cmdButton2.Visible = dlg.cmdButton3.Visible = false;
                    if (dlg.txtMessage.Text != Message)
                    {
                        dlg.txtMessage.Text = Message;
                        dlg.txtMessage.SelectionStart = 0;
                        dlg.txtMessage.SelectionLength = 0;
                    }
                    dlg.progress.Visible = true;
                    if (CustomNumber < 0)
                        CustomNumber = 0;
                    if (CustomNumber > 100)
                        CustomNumber = 100;
                    dlg.progress.Value = (int)CustomNumber;
                    break;
                case MessageTypes.CertificateAcceptanceMessage:
                    dlg.cmdButton1.Text = "Yes";
                    dlg.cmdButton2.Text = "No";
                    dlg.cmdButton1.Enabled = dlg.cmdButton2.Enabled = true;
                    dlg.cmdButton1.Visible = dlg.cmdButton2.Visible = true;
                    dlg.cmdButton3.Enabled = false;
                    dlg.cmdButton3.Visible = false;
                    if (dlg.txtMessage.Text != Message)
                    {
                        SystemSounds.Exclamation.Play();
                        dlg.txtMessage.Text = Message;
                        dlg.txtMessage.SelectionStart = 0;
                        dlg.txtMessage.SelectionLength = 0;
                        dlg.Visible = true;
                    }
                    dlg.progress.Visible = false;
                    break;
            }
        }
    }
}
