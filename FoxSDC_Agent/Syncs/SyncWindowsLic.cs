using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class SyncWindowsLic
    {
        static WindowsLic GetWindowsLicense()
        {
            WindowsLic lic = new WindowsLic();
            try
            {
                ManagementScope Scope;
                Scope = new ManagementScope("\\\\.\\root\\CIMV2", null);

                Scope.Connect();
                ObjectQuery Query = new ObjectQuery("SELECT * FROM SoftwareLicensingProduct Where PartialProductKey<>null AND ApplicationId='55c92734-d682-4d71-983e-d6ec3f16059f' AND LicenseIsAddon=False");
                EnumerationOptions eo = new EnumerationOptions();
                eo.Timeout = new TimeSpan(0, 0, 3, 0);
                ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query, eo);

                foreach (ManagementObject WmiObject in Searcher.Get())
                {
                    lic.Name = (String)WmiObject["Name"];
                    lic.Description = (String)WmiObject["Description"];
                    lic.GracePeriodRemaining = (UInt32)WmiObject["GracePeriodRemaining"];
                    lic.PartialProductKey = (String)WmiObject["PartialProductKey"];
                    lic.ProductKeyID = (String)WmiObject["ProductKeyID"];
                    try
                    {
                        lic.ProductKeyID2 = (String)WmiObject["ProductKeyID2"];
                    }
                    catch
                    {
                        lic.ProductKeyID2 = "";
                    }
                    lic.LicenseFamily = (String)WmiObject["LicenseFamily"];
                    try
                    {
                        lic.ProductKeyChannel = (String)WmiObject["ProductKeyChannel"];
                    }
                    catch
                    {
                        lic.ProductKeyChannel = "";
                    }
                    lic.LicenseStatus = (UInt32)WmiObject["LicenseStatus"];
                    switch ((UInt32)WmiObject["LicenseStatus"])
                    {
                        case 0:
                            lic.LicenseStatusText = "Unlicensed"; break;
                        case 1:
                            lic.LicenseStatusText = "Licensed"; break;
                        case 2:
                            lic.LicenseStatusText = "Out-Of-Box Grace Period"; break;
                        case 3:
                            lic.LicenseStatusText = "Out-Of-Tolerance Grace Period"; break;
                        case 4:
                            lic.LicenseStatusText = "On-Genuine Grace Period"; break;
                        case 5:
                            lic.LicenseStatusText = "Notification"; break;
                        default:
                            lic.LicenseStatusText = lic.LicenseStatus.ToString(); break;
                    }
                }
            }
            catch (ManagementException ee)
            {
                if (ee.ErrorCode == ManagementStatus.CallCanceled)
                {
                    //drop message silently
                    Debug.WriteLine(ee.ToString());
                    return (null);
                }
                else
                {
                    Debug.WriteLine(ee.ToString());
                    FoxEventLog.WriteEventLog("Cannot get Windows License Info " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    return (null);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Cannot get Windows License Info " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                return (null);
            }
            return (lic);
        }

        public static bool DoSyncWindowsLic()
        {
            try
            {
                Status.UpdateMessage(1, "Collecting Windows License Data");
                Network net;
                net = Utilities.ConnectNetwork(1);
                if (net == null)
                    return (false);
                Status.UpdateMessage(1, "Collecting Windows License Data");

                WindowsLic Lic = GetWindowsLicense();
                if (Lic == null)
                {
                    Status.UpdateMessage(1);
                    return (false);
                }

                Status.UpdateMessage(1, "Collecting Windows License Data (Sending data ...)");
                net.ReportWindowsLicData(Lic);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing Windows License Status: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(1);
            return (true);
        }

    }
}
