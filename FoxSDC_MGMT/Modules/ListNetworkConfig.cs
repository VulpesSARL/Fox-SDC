using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Common;

namespace FoxSDC_MGMT
{
    public partial class ctlListNetworkConfig : UserControl
    {
        string MachineID;
        public ctlListNetworkConfig(string MachineID)
        {
            this.MachineID = MachineID;
            InitializeComponent();
        }

        private void ctlListNetworkConfig_Load(object sender, EventArgs e)
        {
            List<NetworkAdapterConfiguration> cfg = Program.net.GetNetAdapterConfig(MachineID);
            if (cfg == null)
                return;
            foreach (NetworkAdapterConfiguration a in cfg)
            {
                TreeNode tn = TVConfigs.Nodes.Add(a.Description);

                if (a.IPAddress.Count > 0)
                {
                    TreeNode stn = tn.Nodes.Add("IP Addresses");
                    for (int i = 0; i < a.IPAddress.Count; i++)
                    {
                        stn.Nodes.Add(a.IPAddress[i] + " - " + a.IPSubnet[i]);
                    }
                }
                if (a.DefaultIPGateway.Count > 0)
                {
                    TreeNode stn = tn.Nodes.Add("Default gateways");
                    for (int i = 0; i < a.DefaultIPGateway.Count; i++)
                    {
                        stn.Nodes.Add(a.DefaultIPGateway[i]);
                    }
                }
                if (a.DNSServerSearchOrder.Count > 0)
                {
                    TreeNode stn = tn.Nodes.Add("DNS Server");
                    for (int i = 0; i < a.DNSServerSearchOrder.Count; i++)
                    {
                        stn.Nodes.Add(a.DNSServerSearchOrder[i]);
                    }
                }
                if (a.DNSDomainSuffixSearchOrder.Count > 0)
                {
                    TreeNode stn = tn.Nodes.Add("DNS Domain Suffix Search Order");
                    for (int i = 0; i < a.DNSDomainSuffixSearchOrder.Count; i++)
                    {
                        stn.Nodes.Add(a.DNSDomainSuffixSearchOrder[i]);
                    }
                }
                if (a.DHCPEnabled == true)
                {
                    TreeNode stn = tn.Nodes.Add("DHCP");
                    stn.Nodes.Add("DHCP Server: " + a.DHCPServer);
                    stn.Nodes.Add("Lease obtained: " + (a.DHCPLeaseObtained == null ? "N/A" : a.DHCPLeaseObtained.Value.ToLongDateString() + " " + a.DHCPLeaseObtained.Value.ToLongTimeString()));
                    stn.Nodes.Add("Lease expires: " + (a.DHCPLeaseExpires == null ? "N/A" : a.DHCPLeaseExpires.Value.ToLongDateString() + " " + a.DHCPLeaseExpires.Value.ToLongTimeString()));
                }

                if (a.DNSHostName + "." + a.DNSDomain != ".")
                    tn.Nodes.Add("DNS Name: " + a.DNSHostName + "." + a.DNSDomain);
                if (a.MACAddress != "")
                    tn.Nodes.Add("MAC Address: " + a.MACAddress);
                tn.Nodes.Add("Service Name: " + a.ServiceName);
                tn.Nodes.Add("Setting ID: " + a.SettingsID);
                if (a.WINSEnableLMHostsLookup == true)
                {
                    TreeNode stn = tn.Nodes.Add("WINS");
                    stn.Nodes.Add("Host Lookup file: " + a.WINSHostLookupFile);
                    stn.Nodes.Add("Scope ID: " + a.WINSScopeID);
                    stn.Nodes.Add("Primary Server: " + a.WINSPrimaryServer);
                    stn.Nodes.Add("Secondary Server: " + a.WINSSecondaryServer);
                }
            }
        }
    }
}
