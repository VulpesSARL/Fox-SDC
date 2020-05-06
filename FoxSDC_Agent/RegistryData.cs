using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    partial class RegistryData
    {
        static public Int64 LastSyncPolicies
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("LastSyncPolicies", "0").ToString(), out i) == false)
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
                    k.SetValue("LastSyncPolicies", value, RegistryValueKind.QWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

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
        }

        static public Int64 PackagesCheckSuccessWait
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("PackagesCheckSuccessWait", "120").ToString(), out i) == false)
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

        static public Int64 PackagesCheckWait
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("PackagesCheckWait", "60").ToString(), out i) == false)
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

        static public Int64 LastSyncReporting
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("LastSyncReporting", "0").ToString(), out i) == false)
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
                    k.SetValue("LastSyncReporting", value, RegistryValueKind.QWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public Int64 LastSyncReporting2
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("LastSyncReporting2", "0").ToString(), out i) == false)
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
                    k.SetValue("LastSyncReporting2", value, RegistryValueKind.QWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public Int64 LastSyncPoliciesWaitTime
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("LastSyncPoliciesWaitTime", "5").ToString(), out i) == false)
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
                    k.SetValue("LastSyncPoliciesWaitTime", value, RegistryValueKind.QWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public Int64 LastSyncReportingWaitTime
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("LastSyncReportingWaitTime", "10").ToString(), out i) == false)
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
                    k.SetValue("LastSyncReportingWaitTime", value, RegistryValueKind.QWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public Int64 LastSyncReportingWaitTime2
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    Int64 i;
                    if (Int64.TryParse(k.GetValue("LastSyncReportingWaitTime2", "60").ToString(), out i) == false)
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
                    k.SetValue("LastSyncReportingWaitTime2", value, RegistryValueKind.QWord);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        static public string MachineID
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (CreateMachineID());

                    string v = k.GetValue("ID", "").ToString();
                    if (v == null)
                    {
                        k.Close();
                        return (CreateMachineID());
                    }
                    if (v.Trim() == "")
                    {
                        k.Close();
                        return (CreateMachineID());
                    }

                    k.Close();

                    Guid g;
                    if (Guid.TryParse(v, out g) == false)
                        return ("");

                    return (v);
                }
                catch
                {
                    return ("");
                }
            }
        }

        public static string MachinePassword
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (CreatePasswordID());

                    string v = k.GetValue("PassID", "").ToString();
                    if (v == null)
                    {
                        k.Close();
                        return (CreatePasswordID());
                    }
                    if (v.Trim() == "")
                    {
                        k.Close();
                        return (CreatePasswordID());
                    }

                    k.Close();

                    Guid g;
                    if (Guid.TryParse(v, out g) == false)
                        return ("");

                    return (v);
                }
                catch
                {
                    return ("");
                }
            }
        }

        public static string CreatePasswordID()
        {
            try
            {
                RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                string GUID = Guid.NewGuid().ToString();
                k.SetValue("PassID", GUID, RegistryValueKind.String);
                k.Close();
                return (GUID);
            }
            catch
            {
                return ("");
            }
        }

        public static string CreateMachineID()
        {
            try
            {
                RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                string GUID = Guid.NewGuid().ToString();
                k.SetValue("ID", GUID, RegistryValueKind.String);
                k.Close();
                return (GUID);
            }
            catch
            {
                return ("");
            }
        }

        public static string InstallPath
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return ("");
                    string v = k.GetValue("InstallPath", "").ToString();
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
                    k.SetValue("InstallPath", value, RegistryValueKind.String);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static string AdministratorName
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
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
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("AdministratorName", value, RegistryValueKind.String);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static string MessageDisclaimer
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
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
            set
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Fox\\SDC");
                    k.SetValue("MessageDisclaimer", value, RegistryValueKind.String);
                    k.Close();
                }
                catch
                {
                }
            }
        }

        public static string UCIDOverride
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return ("");
                    string v = k.GetValue("UCIDOverride", "").ToString();
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

        public static string ServerURL_NoPolicy
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
        }

        public static string ServerURL
        {
            get
            {
                string URL = ServerURL_Policy.Trim();
                if (URL == "")
                    URL = ServerURL_NoPolicy.Trim();
                return (URL);
            }
        }

        public static string ContractID
        {
            get
            {
                string URL = ContractID_Policy.Trim();
                if (URL == "")
                    URL = ContractID_NoPolicy.Trim();
                return (URL);
            }
        }

        public static string ContractPassword
        {
            get
            {
                string URL = ContractPassword_Policy.Trim();
                if (URL == "")
                    URL = ContractPassword_NoPolicy.Trim();
                return (URL);
            }
        }

        public static bool DangerousFunctions
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (false);
                    object v = k.GetValue("Geféierlech Funktioune", null);
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


        public static bool UseOnPrem
        {
            get
            {
                int? u = UseOnPremServer_Policy;
                if (u == null)
                {
                    u = UseOnPremServer_NoPolicy;
                    if (u == null)
                        return (false);
                }

                if (u == 1)
                    return (true);

                u = UseOnPremServer_NoPolicy;
                if (u == null)
                    return (false);

                if (u == 1)
                    return (true);

                return (false);
            }
        }

        public static int? UseOnPremServer_Policy
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Fox\\SDC");
                    if (k == null)
                        return (null);
                    object v = k.GetValue("UseOnPremServer", null);
                    if (v == null)
                    {
                        k.Close();
                        return (null);
                    }
                    int i;
                    if (int.TryParse(v.ToString(), out i) == false)
                    {
                        k.Close();
                        return (null);
                    }
                    k.Close();
                    return (i);
                }
                catch
                {
                    return (null);
                }
            }
        }

        public static int? UseOnPremServer_NoPolicy
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (null);
                    object v = k.GetValue("UseOnPremServer", null);
                    if (v == null)
                    {
                        k.Close();
                        return (null);
                    }
                    int i;
                    if (int.TryParse(v.ToString(), out i) == false)
                    {
                        k.Close();
                        return (null);
                    }
                    k.Close();
                    return (i);
                }
                catch
                {
                    return (null);
                }
            }
        }

        public static string ServerURL_Policy
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Fox\\SDC");
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
        }

        public static string ContractID_Policy
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Fox\\SDC");
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
        }

        public static string ContractID_NoPolicy
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
        }

        public static string ContractPassword_Policy
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Fox\\SDC");
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
        }

        public static string ContractPassword_NoPolicy
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
        }
    }
}
