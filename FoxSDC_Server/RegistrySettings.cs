using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    static class Settings
    {
        public static class Default
        {
            static bool Loaded = false;

            public static string DBServer;
            public static string DBDB;
            public static string DBType;
            public static string DBLocalPath;
            public static string ListenOn;
            public static string DataPath;
            public static string EXEPath;
            public static string URLPart;
            public static string WSListenOn;
            public static string WSSSLCert;
            public static string WSPublishURL;
            public static bool CensorSQLInformations;
            public static bool CensorLicInformations;

            public static bool UseContract
            {
                get
                {
                    if (Fox_LicenseGenerator.SDCLicensing.ValidLicense == false)
                        return (false);
                    if (Fox_LicenseGenerator.SDCLicensing.AllowContract == false)
                        return (false);
                    return (_UseContract);
                }
            }

            static bool _UseContract;
            static public void Load()
            {
                if (Loaded == true)
                    return;

                RegistryKey reg = Registry.LocalMachine.OpenSubKey("Software\\Fox\\FoxSDC" + (Program.InstanceID == "" ? "" : "\\" + Program.InstanceID), false);
                if (reg == null)
                    return;

                DBServer = reg.GetValue("DBServer", "").ToString();
                DBDB = reg.GetValue("DBDB", "").ToString();
                DBType = reg.GetValue("DBType", "").ToString();
                DBLocalPath = reg.GetValue("DBLocalPath", "").ToString();
                ListenOn = reg.GetValue("ListenOn", "").ToString();
                WSListenOn = reg.GetValue("WSListenOn", "").ToString();
                WSSSLCert = reg.GetValue("WSSSLCert", "").ToString();
                WSPublishURL = reg.GetValue("WSPublishURL", "").ToString();

                DataPath = reg.GetValue("DataPath", "").ToString();
                string o = reg.GetValue("UseContract", "0").ToString();
                int i;
                if (int.TryParse(o, out i) == false)
                    _UseContract = false;
                else
                    _UseContract = i == 1 ? true : false;

                o = reg.GetValue("CensorSQLInformations", "0").ToString();
                if (int.TryParse(o, out i) == false)
                    CensorSQLInformations = false;
                else
                    CensorSQLInformations = i == 1 ? true : false;

                o = reg.GetValue("CensorLicInformations", "0").ToString();
                if (int.TryParse(o, out i) == false)
                    CensorLicInformations = false;
                else
                    CensorLicInformations = i == 1 ? true : false;

                string ListenOnTest = ListenOn.Split('|')[0];
                if (ListenOnTest.Contains("://+"))
                    ListenOnTest = ListenOnTest.Replace("://+", "://localhost");
                if (ListenOnTest.Contains("://*"))
                    ListenOnTest = ListenOnTest.Replace("://*", "://localhost");

                Uri uri = new Uri(ListenOnTest);
                URLPart = uri.AbsolutePath;

                ListenOnTest = WSListenOn;
                if (ListenOnTest.Contains("://+"))
                    ListenOnTest = ListenOnTest.Replace("://+", "://localhost");
                if (ListenOnTest.Contains("://*"))
                    ListenOnTest = ListenOnTest.Replace("://*", "://localhost");
                uri = new Uri(ListenOnTest);

                uri = new Uri(WSPublishURL);
                reg.Close();

                EXEPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (EXEPath.EndsWith("\\") == false)
                    EXEPath += "\\";

                Loaded = true;
            }
        }
    }
}
