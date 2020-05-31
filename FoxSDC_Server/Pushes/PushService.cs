using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Server.Pushes
{
    class PushServiceHelperData
    {
        public string MachineID;
        public Int64 Channel;
        public ManualResetEvent EventLock = new ManualResetEvent(false);
        public List<PushData> ToDo = new List<PushData>();
        public object InnerLock = new object();
        public List<PushDataResponse> Responses = new List<PushDataResponse>();
        public ManualResetEvent ResponseEventLock = new ManualResetEvent(false);
        public ManualResetEvent ResponseEventLock2 = new ManualResetEvent(false);
    }

    static class PushServiceHelper
    {
        static Dictionary<string, PushServiceHelperData> Data = new Dictionary<string, PushServiceHelperData>();
        static object Lock = new object();
        const int WaitPush = 3; //in Minutes
        const int LockWaitOne = 50; //in miliseconds

        public static void DeletePushService(string MachineID, Int64 Channel)
        {
            PushData todo = new PushData();
            todo.Action = "quit";
            string Key = MachineID + "-" + Channel.ToString();

            SendPushService(MachineID, todo, Channel);

            lock (Lock)
            {
                if (Data.ContainsKey(Key) == true)
                    Data.Remove(Key);
            }
        }

        public static void SendPushService(string MachineID, PushData data, Int64 Channel)
        {
            string Key = MachineID + "-" + Channel.ToString();
            PushServiceHelperData dd = null;
            lock (Lock)
            {
                if (Data.ContainsKey(Key) == false)
                    return;
                dd = Data[Key];
            }

            lock (dd.InnerLock)
            {
                dd.ToDo.Add(data);
                dd.EventLock.Set();
            }
        }

        public static bool PushResponse(string MachineID, PushDataResponse response, Int64 Channel, bool Use2ndLock = false)
        {
            string Key = MachineID + "-" + Channel.ToString();
            PushServiceHelperData dd = null;
            lock (Lock)
            {
                if (Data.ContainsKey(Key) == false)
                    return (false);
                dd = Data[Key];
            }

            lock (dd.InnerLock)
            {
                response.MachineID = MachineID;
                dd.Responses.Add(response);
                if (Use2ndLock == false)
                    dd.ResponseEventLock.Set();
                else
                    dd.ResponseEventLock2.Set();
            }

            return (true);
        }

        public static PushDataResponse PopResponse(string MachineID, Int64 Channel, string SpecificID, bool Use2ndLock = false, int Timeout= 60)
        {
            string Key = MachineID + "-" + Channel.ToString();
            PushServiceHelperData dd = null;
            PushDataResponse resp = null;
            lock (Lock)
            {
                if (Data.ContainsKey(Key) == false)
                    return (null);
                dd = Data[Key];
            }

            DateTime now = DateTime.UtcNow;

            do
            {
                if ((DateTime.UtcNow - now).TotalSeconds > 30)
                    break;

                int Counter = 0;
                int Timer = 0;
                lock (dd.InnerLock)
                {
                    Counter = dd.Responses.Count;
                }

                if (Counter == 0)
                {
                    if (Use2ndLock == false)
                    {
                        while (Counter == 0)
                        {
                            lock (dd.InnerLock)
                            {
                                Counter = dd.Responses.Count;
                            }
                            dd.ResponseEventLock.WaitOne(LockWaitOne);
                            Timer++;
                            if (Timer > Timeout)
                                break;
                        }
                    }
                    else
                    {
                        while (Counter == 0)
                        {
                            lock (dd.InnerLock)
                            {
                                Counter = dd.Responses.Count;
                            }
                            dd.ResponseEventLock2.WaitOne(LockWaitOne);
                            Timer++;
                            if (Timer > Timeout)
                                break;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(SpecificID) == true)
                {
                    lock (dd.InnerLock)
                    {
                        if (dd.Responses.Count == 0)
                        {
                            if (Use2ndLock == false)
                                dd.ResponseEventLock.Reset();
                            else
                                dd.ResponseEventLock2.Reset();
                            continue;
                        }
                        resp = dd.Responses[0];
                        dd.Responses.RemoveAt(0);
                        if (dd.Responses.Count == 0)
                            if (Use2ndLock == false)
                                dd.ResponseEventLock.Reset();
                            else
                                dd.ResponseEventLock2.Reset();
                        break;
                    }
                }
                else
                {
                    lock (dd.InnerLock)
                    {
                        if (dd.Responses.Count == 0)
                        {
                            if (Use2ndLock == false)
                                dd.ResponseEventLock.Reset();
                            else
                                dd.ResponseEventLock2.Reset();
                            continue;
                        }
                        foreach (PushDataResponse r in dd.Responses)
                        {
                            if (string.IsNullOrWhiteSpace(r.ReplyID) == false)
                            {
                                if (r.ReplyID.ToLower() == SpecificID.ToLower())
                                {
                                    resp = r;
                                    break;
                                }
                            }
                        }
                        if (resp != null)
                        {
                            dd.Responses.Remove(resp);
                            if (dd.Responses.Count == 0)
                            {
                                if (Use2ndLock == false)
                                    dd.ResponseEventLock.Reset();
                                else
                                    dd.ResponseEventLock2.Reset();
                            }
                            break;
                        }
                        if (Use2ndLock == false)
                            dd.ResponseEventLock.Reset();
                        else
                            dd.ResponseEventLock2.Reset();
                    }
                }
            } while (true);
#if DEBUG
            //StackTrace stacky = new StackTrace();
            //Debug.WriteLine(">> PopResponse: \n  " + JsonConvert.SerializeObject(resp) + "\n       for\n   " + stacky.ToString());
#endif
            return (resp);
        }

        public static PushData WaitForPush(NetworkConnectionInfo ni, Int64 Channel, bool Use2ndLock = false)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
                return (null);
            PushServiceHelperData dd;
            if (ni.PushChannel != null)
                if (ni.PushChannel.Value != Channel)
                    return (null); //Can't use same session for different channels!

            string Key = ni.Username + "-" + Channel.ToString();
            lock (Lock)
            {
                if (Data.ContainsKey(Key) == false)
                {
                    Data.Add(Key, new PushServiceHelperData());
                    if (Use2ndLock == false)
                        Data[Key].ResponseEventLock.Reset();
                    else
                        Data[Key].ResponseEventLock2.Reset();
                }
                Data[Key].MachineID = ni.Username;
                Data[Key].Channel = Channel;
                dd = Data[Key];
                ni.PushChannel = Channel;
            }

            lock (dd.InnerLock)
            {
                if (dd.ToDo.Count > 0)
                {
                    PushData p = dd.ToDo[0];
                    dd.ToDo.Remove(p);
                    return (p);
                }
            }

            dd.EventLock.Reset();
            if (dd.EventLock.WaitOne(WaitPush * 60000) == false)
            {
                PushData p = new PushData();
                p.Action = "repeat";
                return (p);
            }

            lock (dd.InnerLock)
            {
                PushData p = dd.ToDo[0];
                dd.ToDo.Remove(p);
                return (p);
            }
        }
    }


    class PushService
    {
        [VulpesRESTfulRet("PushData")]
        public PushDataRoot ReturnData;

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/httppush/response", "", "")]
        public RESTStatus PushResponse(SQLLib sql, PushDataResponse pdr, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (pdr == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (ni.PushChannel == null)
            {
                ni.Error = "Too early";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (PushServiceHelper.PushResponse(ni.Username, pdr, ni.PushChannel.Value) == false)
            {
                ni.Error = "Cannot push";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/httppush/r2sponse", "", "")]
        public RESTStatus PushResponse2(SQLLib sql, PushDataResponse pdr, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (pdr == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (ni.PushChannel == null)
            {
                ni.Error = "Too early";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (PushServiceHelper.PushResponse(ni.Username, pdr, ni.PushChannel.Value, true) == false)
            {
                ni.Error = "Cannot push";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/httppush/r3sponse", "", "")]
        public RESTStatus PushResponse3(SQLLib sql, PushDataResponse pdr, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (pdr == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (ni.PushChannel == null)
            {
                ni.Error = "Too early";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (PushServiceHelper.PushResponse(ni.Username, pdr, ni.PushChannel.Value, true) == false)
            {
                ni.Error = "Cannot push";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }     

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/httppush/rBsponse", "", "")]
        public RESTStatus PushResponseB(SQLLib sql, PushDataResponse pdr, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (pdr == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (ni.PushChannel == null)
            {
                ni.Error = "Too early";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (PushServiceHelper.PushResponse(ni.Username, pdr, ni.PushChannel.Value, true) == false)
            {
                ni.Error = "Cannot push";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/httppush/service", "PushData", "")]
        public RESTStatus Push0(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            PushData data = PushServiceHelper.WaitForPush(ni, 0);
            if (data == null)
            {
                ni.Error = "Push Service Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            ReturnData = new PushDataRoot();
            ReturnData.Data = data;

            if (Certificates.Sign(ReturnData, SettingsManager.Settings.UseCertificate) == false)
            {
                ni.Error = "Push Service Signing Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/httppush/1service", "PushData", "")]
        public RESTStatus Push1(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            PushData data = PushServiceHelper.WaitForPush(ni, 1);
            if (data == null)
            {
                ni.Error = "Push Service Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            ReturnData = new PushDataRoot();
            ReturnData.Data = data;

            if (Certificates.Sign(ReturnData, SettingsManager.Settings.UseCertificate) == false)
            {
                ni.Error = "Push Service Signing Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/httppush/2service", "PushData", "")]
        public RESTStatus Push2(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            PushData data = PushServiceHelper.WaitForPush(ni, 2);
            if (data == null)
            {
                ni.Error = "Push Service Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            ReturnData = new PushDataRoot();
            ReturnData.Data = data;

            if (Certificates.Sign(ReturnData, SettingsManager.Settings.UseCertificate) == false)
            {
                ni.Error = "Push Service Signing Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }   

        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/httppush/Aservice", "PushData", "")]
        public RESTStatus Push10(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            PushData data = PushServiceHelper.WaitForPush(ni, 10);
            if (data == null)
            {
                ni.Error = "Push Service Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            ReturnData = new PushDataRoot();
            ReturnData.Data = data;

            if (Certificates.Sign(ReturnData, SettingsManager.Settings.UseCertificate) == false)
            {
                ni.Error = "Push Service Signing Error";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }

            return (RESTStatus.Success);
        }

    }
}
