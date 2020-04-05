using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public enum SMARTDescriptionEnum : int
    {
        LowIdeal = 1,
        HighIdeal = 2,
        Critical = 3,
        NA = 4
    }

    public class SMARTDescriptionClass
    {
        public string Description;
        public SMARTDescriptionEnum Ideal;
    }

    public class SMARTDescription
    {
        public static bool IsInError(VulpesSMARTInfo i)
        {
            if (i.PredictFailure == true)
                return (true);
            if (i.Attributes == null)
                return (false);
            foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in i.Attributes)
            {
                if (IsAttribInError(kvp.Value) == true)
                    return (true);
            }
            return (false);
        }

        public static bool IsAttribInError(VulpesSMARTAttribute i)
        {
            if (i.FailureImminent == true)
                return (true);
            SMARTDescriptionEnum desc = SMARTDescriptionEnum.NA;
            if (SMARTDescription.Descriptions.ContainsKey(i.ID) == false)
                return (false);
            desc = SMARTDescription.Descriptions[i.ID].Ideal;
            if (desc == SMARTDescriptionEnum.Critical)
            {
                if (i.Vendordata > 0)
                    return (true);
            }
            return (false);
        }

        public static bool ComparePart(VulpesSMARTInfo a, VulpesSMARTInfo b)
        {
            if (a.PNPDeviceID != b.PNPDeviceID)
                return (false);

            return (true);
        }

        public static bool CompareFull(VulpesSMARTInfo a, VulpesSMARTInfo b, List<int> SkipAttributes)
        {
            if (a.PNPDeviceID != b.PNPDeviceID || a.Caption != b.Caption || a.FirmwareRevision != b.FirmwareRevision || a.InterfaceType != b.InterfaceType ||
                a.Model != b.Model || a.Name != b.Name || a.PredictFailure != b.PredictFailure || a.SerialNumber != b.SerialNumber || a.Size != b.Size ||
                a.Status != b.Status)
                return (false);
            if (a.Attributes == null && b.Attributes == null)
                return (true);
            if (a.Attributes.Count != b.Attributes.Count)
                return (false);
            if (SkipAttributes == null)
                SkipAttributes = new List<int>();
            foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in a.Attributes)
            {
                if (SkipAttributes.Contains(kvp.Key) == true)
                    continue;
                if (b.Attributes.ContainsKey(kvp.Key) == false)
                    return (false);
                if (kvp.Value.FailureImminent != b.Attributes[kvp.Key].FailureImminent ||
                    kvp.Value.Flags != b.Attributes[kvp.Key].Flags ||
                    kvp.Value.ID != b.Attributes[kvp.Key].ID ||
                    kvp.Value.Threshold != b.Attributes[kvp.Key].Threshold ||
                    kvp.Value.Value != b.Attributes[kvp.Key].Value ||
                    kvp.Value.Vendordata != b.Attributes[kvp.Key].Vendordata ||
                    kvp.Value.Worst != b.Attributes[kvp.Key].Worst)
                    return (false);
            }

            return (true);
        }

        public static bool CompareFullCriticalOnly(VulpesSMARTInfo a, VulpesSMARTInfo b)
        {
            if (a.PNPDeviceID != b.PNPDeviceID || a.Caption != b.Caption || a.FirmwareRevision != b.FirmwareRevision || a.InterfaceType != b.InterfaceType ||
                a.Model != b.Model || a.Name != b.Name || a.PredictFailure != b.PredictFailure || a.SerialNumber != b.SerialNumber || a.Size != b.Size ||
                a.Status != b.Status)
                return (true);
            if (a.Attributes == null && b.Attributes == null)
                return (true);
            foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in a.Attributes)
            {
                if (b.Attributes.ContainsKey(kvp.Key) == false)
                    continue;
                if (kvp.Value.FailureImminent != b.Attributes[kvp.Key].FailureImminent ||
                    kvp.Value.Flags != b.Attributes[kvp.Key].Flags ||
                    kvp.Value.ID != b.Attributes[kvp.Key].ID ||
                    kvp.Value.Threshold != b.Attributes[kvp.Key].Threshold ||
                    kvp.Value.Value != b.Attributes[kvp.Key].Value ||
                    kvp.Value.Vendordata != b.Attributes[kvp.Key].Vendordata ||
                    kvp.Value.Worst != b.Attributes[kvp.Key].Worst)
                {
                    if (Descriptions.ContainsKey(kvp.Key) == false)
                        continue;
                    if (Descriptions[kvp.Key].Ideal == SMARTDescriptionEnum.Critical)
                        return (false);
                }
            }

            return (true);
        }

        public static Dictionary<int, SMARTDescriptionClass> Descriptions = new Dictionary<int, SMARTDescriptionClass>()
        {
            {0x01,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Read Error Rate"} },
            {0x02,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.HighIdeal, Description="Throughput Performance"} },
            {0x03,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Spin-Up Time"} },
            {0x04,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Start/Stop Count"} },
            {0x05,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="Reallocated Sectors Count"} },
            {0x06,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Read Channel Margin"} },
            {0x07,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Seek Error Rate"} },
            {0x08,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.HighIdeal, Description="Seek Time Performance"} },
            {0x09,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Power-On Hours"} },
            {0x0A,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="Spin Retry Count"} },
            {0x0B,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Recalibration Retries"} },
            {0x0C,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Power Cycle Count"} },
            {0x0D,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Soft Read Error Rate"} },
            {0x16,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Current Helium Level"} },
            {0xAA,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Available Reserved Space"} },
            {0xAB,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="SSD Program Fail Count"} },
            {0xAC,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="SSD Erase Fail Count"} },
            {0xAD,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="SSD Wear Leveling Count"} },
            {0xAE,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Unexpected power loss count"} },
            {0xAF,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Power Loss Protection Failure"} },
            {0xB0,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Erase Fail Count"} },
            {0xB1,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Wear Range Delta"} },
            {0xB3,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Used Reserved Block Count Total"} },
            {0xB4,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Unused Reserved Block Count Total"} },
            {0xB5,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Program Fail Count Total"} },
            {0xB6,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Erase Fail Count"} },
            {0xB7,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="SATA Downshift Error Count"} },
            {0xB8,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="End-to-End error / IOEDC"} },
            {0xB9,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Head Stability"} },
            {0xBA,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Induced Op-Vibration Detection"} },
            {0xBB,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="Reported Uncorrectable Errors"} },
            {0xBC,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Command Timeout"} },
            {0xBD,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="High Fly Writes"} },
            {0xBE,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Airflow Temperature"} },
            {0xBF,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="G-sense Error Rate"} },
            {0xC0,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Unsafe Shutdown Count"} },
            {0xC1,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Load Cycle Count"} },
            {0xC2,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Temperature"} },
            {0xC3,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Hardware ECC Recovered"} },
            {0xC4,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="Reallocation Event Count"} },
            {0xC5,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="Current Pending Sector Count"} },
            {0xC6,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="(Offline) Uncorrectable Sector Count"} },
            {0xC7,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="UltraDMA CRC Error Count"} },
            {0xC8,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Multi-Zone Error Rate"} },
            {0xC9,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="Soft Read Error Rate"} },
            {0xCA,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Data Address Mark errors"} },
            {0xCB,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Run Out Cancel"} },
            {0xCC,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Soft ECC Correction"} },
            {0xCD,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Thermal Asperity Rate"} },
            {0xCE,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Flying Height"} },
            {0xCF,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Spin High Current"} },
            {0xD0,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Spin Buzz"} },
            {0xD1,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Offline Seek Performance"} },
            {0xD2,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Vibration During Write"} },
            {0xD3,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Vibration During Write"} },
            {0xD4,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Shock During Write"} },
            {0xDC,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Disk Shift"} },
            {0xDD,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="G-Sense Error Rate"} },
            {0xDE,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Loaded Hours"} },
            {0xDF,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Load/Unload Retry Count"} },
            {0xE0,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Load Friction"} },
            {0xE1,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Load/Unload Cycle Count"} },
            {0xE2,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Load 'In'-time"} },
            {0xE3,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Torque Amplification Count"} },
            {0xE4,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Power-Off Retract Cycle"} },
            {0xE6,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="GMR Head Amplitude (HDD) / Drive Life Protection Status (SSD)"} },
            {0xE7,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.Critical, Description="Life Left"} },
            {0xE8,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Endurance Remaining"} },
            {0xE9,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Power-On Hours (HDD) / Media Wearout Indicator (SSD)"} },
            {0xF0,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Head Flying Hours"} },
            {0xF1,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Total LBAs Written"} },
            {0xF2,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Total LBAs Read"} },
            {0xF3,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Total LBAs Written Expanded"} },
            {0xF4,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Total LBAs Read Expanded"} },
            {0xF9,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="NAND Writes (1GiB)"} },
            {0xFA,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.LowIdeal, Description="Read Error Retry Rate"} },
            {0xFB,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Minimum Spares Remaining"} },
            {0xFC,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Newly Added Bad Flash Block"} },
            {0xFE,new SMARTDescriptionClass (){  Ideal= SMARTDescriptionEnum.NA, Description="Free Fall Protection"} },
        };
    }
}
