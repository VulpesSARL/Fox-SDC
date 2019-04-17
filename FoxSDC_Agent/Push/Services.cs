using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Push
{
    class Services
    {

        static PushServicesInfo ProcessData(ManagementObject srv, bool IsDriver)
        {
            PushServicesInfo i = new PushServicesInfo();
            i.Description = Convert.ToString(srv["Description"]);
            i.DisplayName = Convert.ToString(srv["DisplayName"]);
            i.Name = Convert.ToString(srv["Name"]);
            i.ServiceType = Convert.ToString(srv["ServiceType"]);
            i.PathName = Convert.ToString(srv["PathName"]);
            i.StartMode = Convert.ToString(srv["StartMode"]);
            i.State = Convert.ToString(srv["State"]);
            i.StartName = Convert.ToString(srv["StartName"]);
            i.Started = Convert.ToBoolean(srv["Started"]);
            i.DesktopInteract = Convert.ToBoolean(srv["DesktopInteract"]);
            i.AcceptPause = Convert.ToBoolean(srv["AcceptPause"]);
            i.AcceptStop = Convert.ToBoolean(srv["AcceptStop"]);
            i.ServiceSpecificExitCode = Convert.ToInt64(srv["ServiceSpecificExitCode"]);
            i.ExitCode = Convert.ToInt64(srv["ExitCode"]);
            i.ErrorControl = Convert.ToString(srv["ErrorControl"]);

            if (IsDriver == false)
            {
                i.DelayedAutoStart = Convert.ToBoolean(srv["DelayedAutoStart"]);
                i.ProcessId = Convert.ToInt64(srv["ProcessId"]);
            }
            else
            {
                i.DelayedAutoStart = false;
                i.ProcessId = 0;
            }
            return (i);
        }

        static Int64 InvokeCommand(string Service, string Function)
        {
            using (ManagementClass mc = new ManagementClass("ROOT\\CIMV2", "Win32_Service", null))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    if (Convert.ToString(mo["Name"]).ToLower() == Service.ToLower())
                    {
                        return (Convert.ToUInt32(mo.InvokeMethod(Function, null)));
                    }
                }
            }

            using (ManagementClass mc = new ManagementClass("ROOT\\CIMV2", "Win32_SystemDriver", null))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    if (Convert.ToString(mo["Name"]).ToLower() == Service.ToLower())
                    {
                        return (Convert.ToUInt32(mo.InvokeMethod(Function, null)));
                    }
                }
            }

            return (0x8000FFFF);
        }

        public static PushServicesInfoList GetServices()
        {
            PushServicesInfoList s = new PushServicesInfoList();
            s.Data = new List<PushServicesInfo>();

            foreach (ManagementObject srv in new ManagementObjectSearcher("Select * from Win32_SystemDriver").Get())
            {
                s.Data.Add(ProcessData(srv, true));
            }

            foreach (ManagementObject srv in new ManagementObjectSearcher("Select * from Win32_Service").Get())
            {
                s.Data.Add(ProcessData(srv, false));
            }

            return (s);
        }

        public static PushServiceControlState ServiceControl(string ReqString)
        {
            PushServiceControlState scm = new PushServiceControlState();
            PushServiceControlReq req;

            try
            {
                req = JsonConvert.DeserializeObject<PushServiceControlReq>(ReqString);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                scm.ResultCode = 0x8000FFFF;
                return (scm);
            }

            if (string.IsNullOrWhiteSpace(req.Service) == true)
            {
                scm.ResultCode = 0x8000FFFF;
                return (scm);
            }

            req.Service = req.Service.Trim();

            try
            {
                switch (req.Control)
                {
                    case 1: //Stop
                        scm.ResultCode = InvokeCommand(req.Service, "StopService");
                        break;
                    case 2: //Start
                        scm.ResultCode = InvokeCommand(req.Service, "StartService");
                        break;
                    case 3: //Pause
                        scm.ResultCode = InvokeCommand(req.Service, "PauseService");
                        break;
                    case 4: //Resume
                        scm.ResultCode = InvokeCommand(req.Service, "ResumeService");
                        break;
                    default:
                        scm.ResultCode = 0x8000FFFF;
                        break;
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                scm.ResultCode = 0x8000FFFF;
            }

            return (scm);
        }
    }
}
