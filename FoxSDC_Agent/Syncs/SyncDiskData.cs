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
    class SyncDiskData
    {
        static List<DiskDataReport> GetDisks()
        {
            List<DiskDataReport> dd = new List<DiskDataReport>();
            foreach (ManagementObject volume in new ManagementObjectSearcher("Select * from Win32_Volume").Get())
            {
                DiskDataReport d = new DiskDataReport();
                d.MachineID = SystemInfos.SysInfo.MachineID;
                d.Access = (DiskDataAccess)Convert.ToInt32(volume["Access"]);
                d.Automount = Convert.ToBoolean(volume["Automount"]);
                d.Availability = (DiskDataAvailability)Convert.ToInt32(volume["Availability"]);
                d.Capacity = Convert.ToInt64(volume["Capacity"]);
                d.Caption = Convert.ToString(volume["Caption"]);
                d.Compressed = Convert.ToBoolean(volume["Compressed"]);
                d.ConfigManagerErrorCode = Convert.ToInt32(volume["ConfigManagerErrorCode"]);
                d.Description = Convert.ToString(volume["Description"]);
                d.DeviceID = Convert.ToString(volume["DeviceID"]);
                d.DirtyBitSet = Convert.ToBoolean(volume["DirtyBitSet"]);
                d.DriveLetter = Convert.ToString(volume["DriveLetter"]);
                d.DriveType = (DiskDataDriveType)Convert.ToInt32(volume["DriveType"]);
                d.ErrorDescription = Convert.ToString(volume["ErrorDescription"]);
                d.ErrorMethodology = Convert.ToString(volume["ErrorMethodology"]);
                d.FileSystem = Convert.ToString(volume["FileSystem"]);
                d.FreeSpace = Convert.ToInt64(volume["FreeSpace"]);
                d.Label = Convert.ToString(volume["Label"]);
                d.LastErrorCode = Convert.ToInt32(volume["LastErrorCode"]);
                d.MaximumFileNameLength = Convert.ToInt32(volume["MaximumFileNameLength"]);
                d.Name = Convert.ToString(volume["Name"]);
                d.PNPDeviceID = Convert.ToString(volume["PNPDeviceID"]);
                d.SerialNumber = Convert.ToInt64(volume["SerialNumber"]);
                d.Status = Convert.ToString(volume["Status"]);
                dd.Add(d);
            }
            return (dd);
        }

        public static bool DoSyncDiskData()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);
                Status.UpdateMessage(0, "Collecting disk data");

                List<DiskDataReport> lst = GetDisks();

                Status.UpdateMessage(0, "Reporting disk data");
                ListDiskDataReport lstt = new ListDiskDataReport();
                lstt.Items = lst;

                net.ReportDiskData(lstt);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing Disk Data: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }
    }
}
