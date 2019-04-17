using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    /// <summary>
    /// Unique Computer ID
    /// </summary>
    public class UCID
    {
        public static string GetUCID()
        {
            string BIOS = "";

            ManagementClass mc = new ManagementClass("Win32_BIOS");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementBaseObject mo in moc)
            {
                BIOS = GetFromMO(mo, "Manufacturer") + GetFromMO(mo, "IdentificationCode") + GetFromMO(mo, "SerialNumber");
            }

            string CPUID = "";
            mc = new ManagementClass("Win32_Processor");
            moc = mc.GetInstances();
            foreach (ManagementBaseObject mo in moc)
            {
                if (CPUID == "")
                    CPUID = GetFromMO(mo, "UniqueId");
                if (CPUID == "")
                    CPUID = GetFromMO(mo, "ProcessorId");
                if (CPUID == "")
                {
                    CPUID = GetFromMO(mo, "Name");
                    if (CPUID == "")
                        CPUID = GetFromMO(mo, "Manufacturer");
                    CPUID += GetFromMO(mo, "MaxClockSpeed");
                }
            }

            string BASEBOARD = "";
            mc = new ManagementClass("Win32_Baseboard");
            moc = mc.GetInstances();
            foreach (ManagementBaseObject mo in moc)
            {
                BASEBOARD = GetFromMO(mo, "SerialNumber");
            }

            string ID = "BIOS>>" + BIOS + "CPU>>" + CPUID + "BB>>" + BASEBOARD;
            return (MD5Utilities.CalcMD5(ID));
        }

        static string GetFromMO(ManagementBaseObject mo, string Key)
        {
            try
            {
                return (Convert.ToString(mo[Key]));
            }
            catch
            {
                return ("");
            }
        }
    }
}
