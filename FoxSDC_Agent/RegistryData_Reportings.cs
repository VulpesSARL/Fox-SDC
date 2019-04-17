using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    partial class RegistryData
    {
        static bool ReadRepRegistry(string Name)
        {
            try
            {
                RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC\\Reportings");
                if (k == null)
                    return (false);
                Int64 i;
                if (Int64.TryParse(k.GetValue(Name, "0").ToString(), out i) == false)
                {
                    k.Close();
                    return (false);
                }
                k.Close();
                return (i == 1 ? true : false);
            }
            catch
            {
                return (false);
            }
        }

        static void WriteRepRegistry(string Name, bool Value)
        {
            try
            {
                RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC\\Reportings");
                k.SetValue(Name, Value == true ? 1 : 0, RegistryValueKind.DWord);
                k.Close();
            }
            catch
            {
            }
        }

        static public bool DisableEventLogSync
        {
            get
            {
                return (ReadRepRegistry("DisableEventLogSync"));
            }
            set
            {
                WriteRepRegistry("DisableEventLogSync", value);
            }
        }

        static public bool DisableAddRemoveProgramsSync
        {
            get
            {
                return (ReadRepRegistry("DisableAddRemoveProgramsSync"));
            }
            set
            {
                WriteRepRegistry("DisableAddRemoveProgramsSync", value);
            }
        }

        static public bool DisableDiskDataSync
        {
            get
            {
                return (ReadRepRegistry("DisableDiskDataSync"));
            }
            set
            {
                WriteRepRegistry("DisableDiskDataSync", value);
            }
        }

        static public bool DisableNetadapterSync
        {
            get
            {
                return (ReadRepRegistry("DisableNetadapterSync"));
            }
            set
            {
                WriteRepRegistry("DisableNetadapterSync", value);
            }
        }

        static public bool DisableDeviceManagerSync
        {
            get
            {
                return (ReadRepRegistry("DisableDeviceManagerSync"));
            }
            set
            {
                WriteRepRegistry("DisableDeviceManagerSync", value);
            }
        }

        static public bool DisableFilterDriverSync
        {
            get
            {
                return (ReadRepRegistry("DisableFilterDriverSync"));
            }
            set
            {
                WriteRepRegistry("DisableFilterDriverSync", value);
            }
        }

        static public bool DisableWinLicenseSync
        {
            get
            {
                return (ReadRepRegistry("DisableWinLicenseSync"));
            }
            set
            {
                WriteRepRegistry("DisableWinLicenseSync", value);
            }
        }

        static public bool EnableBitlockerRKSync
        {
            get
            {
                return (ReadRepRegistry("EnableBitlockerRKSync"));
            }
            set
            {
                WriteRepRegistry("EnableBitlockerRKSync", value);
            }
        }

    }
}
