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
    class SyncBitlockerRK
    {
        static List<BitlockerRK> GetRKs()
        {
            List<BitlockerRK> lst = new List<BitlockerRK>();
            uint Error;

            ManagementClass mc;
            try
            {
                mc = new ManagementClass("ROOT\\CIMV2\\Security\\MicrosoftVolumeEncryption", "Win32_EncryptableVolume", null);
                mc.GetInstances();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (lst);
            }

            foreach (ManagementObject mo in mc.GetInstances())
            {
                BitlockerRK rk = new BitlockerRK();
                rk.DeviceID = Convert.ToString(mo["DeviceID"]);
                rk.DriveLetter = Convert.ToString(mo["DriveLetter"]);
                rk.Keys = new List<BitlockerRKKeyElement>();
                rk.Reported = DateTime.UtcNow;

                ManagementBaseObject inparams2 = mo.GetMethodParameters("GetKeyProtectors");
                inparams2["KeyProtectorType"] = 3;
                ManagementBaseObject outparams2 = mo.InvokeMethod("GetKeyProtectors", inparams2, null);
                Error = Convert.ToUInt32(outparams2["returnValue"]);
                if (Error != 0)
                    continue;

                string[] keys = (string[])outparams2["VolumeKeyProtectorID"];

                foreach (string key in keys)
                {
                    ManagementBaseObject inparams = mo.GetMethodParameters("GetKeyProtectorNumericalPassword");
                    inparams["VolumeKeyProtectorID"] = key;
                    ManagementBaseObject outparams = mo.InvokeMethod("GetKeyProtectorNumericalPassword", inparams, null);
                    Error = Convert.ToUInt32(outparams["returnValue"]);
                    if (Error != 0)
                        return (null);

                    BitlockerRKKeyElement rkk = new BitlockerRKKeyElement();
                    rkk.Key = Convert.ToString(outparams["NumericalPassword"]);
                    rkk.VolumeKeyProtectorID = key;
                    rk.Keys.Add(rkk);
                }
                lst.Add(rk);
            }

            return (lst);
        }

        public static bool DoSyncBitlockerRK()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(1);
                if (net == null)
                    return (false);
                Status.UpdateMessage(1, "Collecting Bitlocker Recovery Keys");

                List<BitlockerRK> lst = GetRKs();

                if (lst.Count != 0)
                {
                    Status.UpdateMessage(1, "Reporting Bitlocker Recovery Keys");
                    BitlockerRKList lstt = new BitlockerRKList();
                    lstt.List = lst;
                    lstt.MachineID = SystemInfos.SysInfo.MachineID;
                    net.ReportBitlockerRKList(lstt);
                }

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing Bitlocker RKs: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(1);

            return (true);
        }
    }
}
