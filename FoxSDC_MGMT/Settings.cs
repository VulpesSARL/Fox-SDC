using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_MGMT
{
    static class Settings
    {
        static bool SettingsLoaded = false;
        static def _def;

        public static def Default
        {
            get
            {
                if (SettingsLoaded == false)
                {
                    _def = new def();
                    _def.Load();
                    SettingsLoaded = true;
                }
                return (_def);
            }
        }


        public class def
        {
            public string LastUsername;
            public string LastServer;
            public bool EnableDebug;
            public bool ShowActiveUsers;

            public void Load()
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Fox\\SDC_MGMT", false);
                if (reg == null)
                    return;
                LastUsername = reg.GetValue("LastUsername", "").ToString();
                LastServer = reg.GetValue("LastServer", "").ToString();
                EnableDebug = reg.GetValue("EnableDebug", "0").ToString() == "1" ? true : false;
                ShowActiveUsers = reg.GetValue("ShowActiveUsers", "0").ToString() == "1" ? true : false;
                reg.Close();
            }

            public void Save()
            {
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Fox\\SDC_MGMT");
                if (reg == null)
                    return;

                reg.SetValue("LastUsername", LastUsername == null ? "" : LastUsername, RegistryValueKind.String);
                reg.SetValue("LastServer", LastServer == null ? "" : LastServer, RegistryValueKind.String);
                reg.SetValue("ShowActiveUsers", ShowActiveUsers == true ? 1 : 0, RegistryValueKind.DWord);

                reg.Close();
            }
        }
    }
}