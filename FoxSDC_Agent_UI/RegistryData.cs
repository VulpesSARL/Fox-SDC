using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent_UI
{
    class RegistryData
    {
        #region User settings
        static public int ChatAudible
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Fox\\SDC", false);
                    if (k == null)
                        return (0);
                    int i;
                    if (int.TryParse(k.GetValue("ChatAudible", "0").ToString(), out i) == false)
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
                    RegistryKey k = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("ChatAudible", value, RegistryValueKind.DWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public int ChatBlink
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Fox\\SDC", false);
                    if (k == null)
                        return (1);
                    int i;
                    if (int.TryParse(k.GetValue("ChatBlink", "1").ToString(), out i) == false)
                    {
                        k.Close();
                        return (1);
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
                    RegistryKey k = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("ChatBlink", value, RegistryValueKind.DWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public int ChatAOT
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Fox\\SDC", false);
                    if (k == null)
                        return (0);
                    int i;
                    if (int.TryParse(k.GetValue("ChatAOT", "0").ToString(), out i) == false)
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
                    RegistryKey k = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("ChatAOT", value, RegistryValueKind.DWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        #endregion

        static public int ShowClientEnhancedMenu
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC", false);
                    if (k == null)
                        return (0);
                    int i;
                    if (int.TryParse(k.GetValue("ShowClientEnhancedMenu", "0").ToString(), out i) == false)
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
        }

        public static string AdministratorName
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC", false);
                    if (k == null)
                        return ("Administrator");
                    string v = k.GetValue("AdministratorName", "Administrator").ToString();
                    k.Close();
                    if (v == null)
                        return ("Administrator");
                    return (v);
                }
                catch
                {
                    return ("Administrator");
                }
            }
        }

        public static string MessageDisclaimer
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC", false);
                    if (k == null)
                        return ("");
                    string v = k.GetValue("MessageDisclaimer", "").ToString();
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
        }
    }
}
