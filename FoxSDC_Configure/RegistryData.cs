using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Configure
{
    class RegistryData
    {
        static public Int64 Verbose
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    int i;
                    if (int.TryParse(k.GetValue("Verbose", "0").ToString(), out i) == false)
                    {
                        k.Close();
                        return (0);
                    }
                    k.Close();
                    return (i);
                }
                catch
                {
                    return (0);
                }
            }
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("Verbose", value, RegistryValueKind.DWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public Int64 EarlyUpdates
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    int i;
                    if (int.TryParse(k.GetValue("EarlyUpdates", "0").ToString(), out i) == false)
                    {
                        k.Close();
                        return (0);
                    }
                    k.Close();
                    return (i);
                }
                catch
                {
                    return (0);
                }
            }
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("EarlyUpdates", value, RegistryValueKind.DWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static string ServerURL
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return ("");
                    string v = k.GetValue("Server", "").ToString();
                    k.Close();
                    if (v == null)
                        return ("");
                    return (v);
                }
                catch
                {
                    return ("");
                }
            }
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("Server", value, RegistryValueKind.String);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static bool UseOnPremServer
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (false);
                    object v = k.GetValue("UseOnPremServer", null);
                    if (v == null)
                    {
                        k.Close();
                        return (false);
                    }
                    int i;
                    if (int.TryParse(v.ToString(), out i) == false)
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
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("UseOnPremServer", value == true ? 1 : 0, RegistryValueKind.DWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static string ContractID
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return ("");
                    string v = k.GetValue("ContractID", "").ToString();
                    k.Close();
                    if (v == null)
                        return ("");
                    return (v);
                }
                catch
                {
                    return ("");
                }
            }
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("ContractID", value, RegistryValueKind.String);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static string ContractPassword
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return ("");
                    string v = k.GetValue("ContractPassword", "").ToString();
                    k.Close();
                    if (v == null)
                        return ("");
                    return (v);
                }
                catch
                {
                    return ("");
                }
            }
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("ContractPassword", value, RegistryValueKind.String);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static bool DisableConfigTool
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Fox\\SDC");
                    if (k == null)
                        return (false);
                    object v = k.GetValue("DisableConfigTool", null);
                    if (v == null)
                    {
                        k.Close();
                        return (false);
                    }
                    int i;
                    if (int.TryParse(v.ToString(), out i) == false)
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
        }
    }
}
