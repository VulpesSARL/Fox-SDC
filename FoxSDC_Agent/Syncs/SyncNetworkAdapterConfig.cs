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
    class SyncNetworkAdapterConfig
    {
        static List<NetworkAdapterConfiguration> GetNetAdapterConfig()
        {
            List<NetworkAdapterConfiguration> l = new List<NetworkAdapterConfiguration>();
            foreach (ManagementObject mo in new ManagementObjectSearcher("Select * from Win32_NetworkAdapterConfiguration").Get())
            {
                if (mo["InterfaceIndex"] == null)
                    continue;

                NetworkAdapterConfiguration nc = new NetworkAdapterConfiguration();
                nc.Caption = (string)mo["Caption"];
                nc.Description = (string)mo["Description"];
                nc.DHCPEnabled = mo["DHCPEnabled"] == null ? true : (bool)mo["DHCPEnabled"];
                nc.DHCPLeaseExpires = mo["DHCPLeaseExpires"] == null ? (DateTime?)null : ManagementDateTimeConverter.ToDateTime(Convert.ToString(mo["DHCPLeaseExpires"]));
                nc.DHCPLeaseObtained = mo["DHCPLeaseObtained"] == null ? (DateTime?)null : ManagementDateTimeConverter.ToDateTime(Convert.ToString(mo["DHCPLeaseObtained"]));
                nc.DHCPServer = (string)mo["DHCPServer"];
                nc.DNSDomain = (string)mo["DNSDomain"];
                nc.DefaultIPGateway = mo["DefaultIPGateway"] == null ? null : new List<string>((string[])mo["DefaultIPGateway"]);
                nc.DNSDomainSuffixSearchOrder = mo["DNSDomainSuffixSearchOrder"] == null ? null : new List<string>((string[])mo["DNSDomainSuffixSearchOrder"]);
                nc.DNSServerSearchOrder = mo["DNSServerSearchOrder"] == null ? null : new List<string>((string[])mo["DNSServerSearchOrder"]);
                nc.IPAddress = mo["IPAddress"] == null ? null : new List<string>((string[])mo["IPAddress"]);
                nc.IPSubnet = mo["IPSubnet"] == null ? null : new List<string>((string[])mo["IPSubnet"]);
                nc.DNSHostName = (string)mo["DNSHostName"];
                nc.InterfaceIndex = (int)(uint)mo["InterfaceIndex"];
                nc.IPEnabled = mo["IPEnabled"] == null ? false : (bool)mo["IPEnabled"];
                nc.MACAddress = (string)mo["MACAddress"];
                nc.ServiceName = (string)mo["ServiceName"];
                nc.SettingsID = (string)mo["SettingID"];
                nc.WINSEnableLMHostsLookup = mo["WINSEnableLMHostsLookup"] == null ? false : (bool)mo["WINSEnableLMHostsLookup"];
                nc.WINSHostLookupFile = (string)mo["WINSHostLookupFile"];
                nc.WINSScopeID = (string)mo["WINSScopeID"];
                nc.WINSPrimaryServer = (string)mo["WINSPrimaryServer"];
                nc.WINSSecondaryServer = (string)mo["WINSSecondaryServer"];
                l.Add(nc);
            }
            return (l);
        }

        public static bool DoSyncNetAdapterConfig()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);
                Status.UpdateMessage(0, "Collecting network configuration");

                List<NetworkAdapterConfiguration> lst = GetNetAdapterConfig();

                Status.UpdateMessage(0, "Reporting network configuration");
                ListNetworkAdapterConfiguration lstt = new ListNetworkAdapterConfiguration();
                lstt.Items = lst;
                lstt.MachineID = SystemInfos.SysInfo.MachineID;

                net.ReportNetworkAdapterConfiguration(lstt);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing network config: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }
    }
}
