using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_RemoteConnect
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

            public void Load()
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Fox\\SDC_RemoteConnect", false);
                if (reg == null)
                    return;
                LastUsername = reg.GetValue("LastUsername", "").ToString();
                LastServer = reg.GetValue("LastServer", "").ToString();
                reg.Close();
            }

            public void Save()
            {
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Fox\\SDC_RemoteConnect");
                if (reg == null)
                    return;

                reg.SetValue("LastUsername", LastUsername == null ? "" : LastUsername, RegistryValueKind.String);
                reg.SetValue("LastServer", LastServer == null ? "" : LastServer, RegistryValueKind.String);

                reg.Close();
            }
        }
    }
}
