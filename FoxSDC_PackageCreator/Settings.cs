using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_PackageCreator
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
            public string LastCertificate;
            public string LastExtCertificate;
            public string LastSelectedCertificateType;
            public int WinH;
            public int WinW;
            public int WinX;
            public int WinY;
            public int WinState;

            public void Load()
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Fox\\FoxSDC Package Creator", false);
                if (reg == null)
                    return;
                LastCertificate = reg.GetValue("LastCertificate", "").ToString();
                LastExtCertificate = reg.GetValue("LastExtCertificate", "").ToString();
                LastSelectedCertificateType = reg.GetValue("LastSelectedCertificateType", "").ToString();

                string tmp;

                tmp = reg.GetValue("WinH", "0").ToString();
                int.TryParse(tmp, out WinH);
                tmp = reg.GetValue("WinW", "0").ToString();
                int.TryParse(tmp, out WinW);
                tmp = reg.GetValue("WinX", "0").ToString();
                int.TryParse(tmp, out WinX);
                tmp = reg.GetValue("WinY", "0").ToString();
                int.TryParse(tmp, out WinY);

                reg.Close();
            }

            public void Save()
            {
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Fox\\FoxSDC Package Creator");
                if (reg == null)
                    return;

                reg.SetValue("LastCertificate", LastCertificate == null ? "" : LastCertificate, RegistryValueKind.String);
                reg.SetValue("LastExtCertificate", LastExtCertificate == null ? "" : LastExtCertificate, RegistryValueKind.String);
                reg.SetValue("LastSelectedCertificateType", LastSelectedCertificateType == null ? "" : LastSelectedCertificateType, RegistryValueKind.String);
                reg.SetValue("WinH", WinH, RegistryValueKind.DWord);
                reg.SetValue("WinW", WinW, RegistryValueKind.DWord);
                reg.SetValue("WinX", WinX, RegistryValueKind.DWord);
                reg.SetValue("WinY", WinY, RegistryValueKind.DWord);

                reg.Close();
            }
        }
    }
}
