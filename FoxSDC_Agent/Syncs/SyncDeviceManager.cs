using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class SyncDeviceManager
    {
        static List<PnPDevice> GetPnPDevices()
        {
            List<PnPDevice> l = new List<PnPDevice>();
            foreach (ManagementObject mo in new ManagementObjectSearcher("Select * from Win32_PnPEntity").Get())
            {
                PnPDevice dev = new PnPDevice();

                dev.Availability = mo["Availability"] == null ? 0 : (int)(uint)mo["Availability"];
                dev.Caption = (string)mo["Caption"];
                dev.ClassGuid = (string)mo["ClassGuid"];
                dev.CompatibleID = mo["CompatibleID"] == null ? null : new List<string>((string[])mo["CompatibleID"]);
                dev.ConfigManagerErrorCode = (int)(uint)mo["ConfigManagerErrorCode"];
                dev.ConfigManagerUserConfig = (bool)mo["ConfigManagerUserConfig"];
                dev.CreationClassName = (string)mo["CreationClassName"];
                dev.Description = (string)mo["Description"];
                dev.ErrorCleared = mo["ErrorCleared"] == null ? (bool?)null : (bool)mo["ErrorCleared"];
                dev.ErrorDescription = (string)mo["ErrorDescription"];
                dev.HardwareID = mo["HardwareID"] == null ? null : new List<string>((string[])mo["HardwareID"]);
                dev.InstallDate = mo["InstallDate"] == null ? (DateTime?)null : ManagementDateTimeConverter.ToDateTime(Convert.ToString(mo["InstallDate"]));
                dev.LastErrorCode = mo["LastErrorCode"] == null ? (int?)null : (int)mo["LastErrorCode"];
                dev.Manufacturer = (string)mo["Manufacturer"];
                dev.Name = (string)mo["Name"];
                dev.PNPDeviceID = (string)mo["PNPDeviceID"];
                dev.Service = (string)mo["Service"];
                dev.Status = (string)mo["Status"];
                dev.StatusInfo = mo["StatusInfo"] == null ? (int?)null : (int)mo["StatusInfo"];
                try //Windows 7 compatibility
                {
                    dev.PNPClass = (string)mo["PNPClass"];
                }
                catch
                {
                    dev.PNPClass = "";
                }
                try
                {
                    dev.Present = (bool)mo["Present"];
                }
                catch
                {
                    dev.Present = true;
                }
                l.Add(dev);
            }

            return (l);
        }

        public static bool DoSyncDeviceManager()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);
                Status.UpdateMessage(0, "Collecting device configuration");

                List<PnPDevice> lst = GetPnPDevices();

                Status.UpdateMessage(0, "Reporting device configuration");
                PnPDeviceList lstt = new PnPDeviceList();
                lstt.List = lst;
                lstt.MachineID = SystemInfos.SysInfo.MachineID;

                net.ReportDevicesList(lstt);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing devices config: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }

    }
}
