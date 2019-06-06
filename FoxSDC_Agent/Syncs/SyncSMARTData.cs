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
    class SyncSMARTData
    {
        static VulpesSMARTInfo GetElementByPNPID(List<VulpesSMARTInfo> lst, string PNP)
        {
            foreach (VulpesSMARTInfo l in lst)
            {
                if (l.PNPDeviceID.ToUpper() == PNP.ToUpper())
                    return (l);
            }
            return (null);
        }

        static List<VulpesSMARTInfo> GetSMARTInfo()
        {
            List<VulpesSMARTInfo> lst = new List<VulpesSMARTInfo>();

            foreach (ManagementObject volume in new ManagementObjectSearcher("Select * from Win32_DiskDrive").Get())
            {
                VulpesSMARTInfo smi = new VulpesSMARTInfo();
                smi.Caption = Convert.ToString(volume["Caption"]);
                smi.FirmwareRevision = Convert.ToString(volume["FirmwareRevision"]);
                smi.InterfaceType = Convert.ToString(volume["InterfaceType"]);
                smi.Model = Convert.ToString(volume["Model"]);
                smi.Name = Convert.ToString(volume["Name"]);
                smi.PNPDeviceID = Convert.ToString(volume["PNPDeviceID"]);
                smi.SerialNumber = Convert.ToString(volume["SerialNumber"]).Trim();
                smi.Size = Convert.ToInt64(volume["Size"]);
                smi.Status = Convert.ToString(volume["Status"]);
                lst.Add(smi);
            }

            //foreach (ManagementObject volume in new ManagementObjectSearcher("ROOT\\WMI", "Select * from MSStorageDriver_FailurePredictFunction").Get())
            //{
            //    object ret = volume.InvokeMethod("EnableDisableHardwareFailurePrediction", new object[] { true });
            //}

            foreach (ManagementObject volume in new ManagementObjectSearcher("ROOT\\WMI", "Select * from MSStorageDriver_FailurePredictStatus").Get())
            {
                string inst = Convert.ToString(volume["InstanceName"]);
                if (inst.EndsWith("_0") == true)
                    inst = inst.Substring(0, inst.Length - 2);
                VulpesSMARTInfo smi = GetElementByPNPID(lst, inst);
                if (smi == null)
                    continue;
                smi.PredictFailure = Convert.ToBoolean(volume["PredictFailure"]);
            }

            foreach (ManagementObject volume in new ManagementObjectSearcher("ROOT\\WMI", "Select * from MSStorageDriver_FailurePredictData").Get())
            {
                string inst = Convert.ToString(volume["InstanceName"]);
                if (inst.EndsWith("_0") == true)
                    inst = inst.Substring(0, inst.Length - 2);
                VulpesSMARTInfo smi = GetElementByPNPID(lst, inst);
                if (smi == null)
                    continue;

                smi.VendorSpecific = (byte[])volume["VendorSpecific"];

                for (int i = 0; i < 30; i++)
                {
                    if (smi.VendorSpecific[i * 12 + 2] == 0) //ID
                        continue;

                    smi.Attributes.Add(smi.VendorSpecific[i * 12 + 2], new VulpesSMARTAttribute()
                    {
                        ID = smi.VendorSpecific[i * 12 + 2],
                        Flags = smi.VendorSpecific[i * 12 + 4],
                        FailureImminent = (smi.VendorSpecific[i * 12 + 4] & 0x1) == 0x1,
                        Value = smi.VendorSpecific[i * 12 + 5],
                        Worst = smi.VendorSpecific[i * 12 + 6],
                        Vendordata = BitConverter.ToInt32(smi.VendorSpecific, i * 12 + 7)
                    });
                }
            }

            foreach (ManagementObject volume in new ManagementObjectSearcher("ROOT\\WMI", "Select * from MSStorageDriver_FailurePredictThresholds").Get())
            {
                string inst = Convert.ToString(volume["InstanceName"]);
                if (inst.EndsWith("_0") == true)
                    inst = inst.Substring(0, inst.Length - 2);
                VulpesSMARTInfo smi = GetElementByPNPID(lst, inst);
                if (smi == null)
                    continue;

                smi.VendorSpecificThreshold = (byte[])volume["VendorSpecific"];
                for (int i = 0; i < 30; i++)
                {
                    int id = smi.VendorSpecificThreshold[i * 12 + 2];

                    if (smi.Attributes.ContainsKey(id) == true)
                        smi.Attributes[id].Threshold = smi.VendorSpecificThreshold[i * 12 + 3];
                }
            }

            return (lst);
        }

        public static bool DoSyncSMART()
        {
            try
            {
                Status.UpdateMessage(1, "Collecting SMART Informations");
                Network net;
                net = Utilities.ConnectNetwork(1);
                if (net == null)
                    return (false);
                Status.UpdateMessage(1, "Collecting SMART Informations");

                List<VulpesSMARTInfo> SMART;
                try
                {
                    SMART = GetSMARTInfo();
                }
                catch (ManagementException ee)
                {
                    if(ee.ErrorCode== ManagementStatus.NotSupported)
                    {
                        SMART = new List<VulpesSMARTInfo>();
                        SMART.Add(new VulpesSMARTInfo()
                        {
                            Attributes = null,
                            Caption = "Unsupported",
                            FirmwareRevision = "AAAA",
                            InterfaceType = "IDE",
                            Model = "Unsupported",
                            Name = "Unsupported",
                            PNPDeviceID = "LEGACY\\UNSUPPORTED",
                            PredictFailure = false,
                            SerialNumber = "00000000000000",
                            Size = 1024 * 1024,
                            Status = "OK",
                            VendorSpecific = null,
                            VendorSpecificThreshold = null
                        });
                    }
                    else
                    {
                        throw;
                    }
                }
                catch
                {
                    throw;
                }
                if (SMART == null)
                {
                    Status.UpdateMessage(1);
                    net.CloseConnection();
                    return (false);
                }

                Status.UpdateMessage(1, "Collecting SMART Informations (Sending data ...)");
                net.ReportSMARTInfos(SystemInfos.SysInfo.MachineID, SMART);
                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing SMART Informations: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(1);
            return (true);
        }
    }
}
