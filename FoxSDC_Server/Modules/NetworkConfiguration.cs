using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class NetworkConfiguration
    {

        void InsertSupplData(SQLLib sql, string MachineID, int IfIndex, int Type, List<string> lst)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                sql.InsertMultiData("NetworkConfigSuppl",
                    new SQLData("MachineID", MachineID),
                    new SQLData("InterfaceIndex", IfIndex),
                    new SQLData("Type", Type),
                    new SQLData("Order", i),
                    new SQLData("Data", lst[i] == null ? "" : lst[i].Trim()));
            }
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/netadapterconfig", "", "")]
        public RESTStatus ReportNetadapterConfig(SQLLib sql, ListNetworkAdapterConfiguration netadapters, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (netadapters == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            netadapters.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", netadapters.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.NotFound);
                }
            }

            lock (ni.sqllock)
            {
                sql.ExecSQL("DELETE FROM NetworkConfigSuppl WHERE MachineID=@id", new SQLParam("@id", netadapters.MachineID));
                sql.ExecSQL("DELETE FROM NetworkConfig WHERE MachineID=@id", new SQLParam("@id", netadapters.MachineID));
            }

            if (netadapters.Items == null)
                netadapters.Items = new List<NetworkAdapterConfiguration>();

            List<int> IFIndex = new List<int>();

            foreach (NetworkAdapterConfiguration ncfg in netadapters.Items)
            {
                if (IFIndex.Contains(ncfg.InterfaceIndex) == true)
                    continue;
                IFIndex.Add(ncfg.InterfaceIndex);

                lock (ni.sqllock)
                {
                    sql.InsertMultiData("NetworkConfig",
                        new SQLData("MachineID", netadapters.MachineID),
                        new SQLData("InterfaceIndex", ncfg.InterfaceIndex),
                        new SQLData("IPEnabled", ncfg.IPEnabled),
                        new SQLData("MACAddress", ncfg.MACAddress == null ? "" : ncfg.MACAddress.Trim()),
                        new SQLData("ServiceName", ncfg.ServiceName == null ? "" : ncfg.ServiceName.Trim()),
                        new SQLData("SettingsID", ncfg.SettingsID == null ? "" : ncfg.SettingsID.Trim()),
                        new SQLData("Description", ncfg.Description == null ? "" : ncfg.Description.Trim()),
                        new SQLData("DHCPEnabled", ncfg.DHCPEnabled),
                        new SQLData("DHCPServer", ncfg.DHCPServer == null ? "" : ncfg.DHCPServer.Trim()),
                        new SQLData("DNSDomain", ncfg.DNSDomain == null ? "" : ncfg.DNSDomain.Trim()),
                        new SQLData("DNSHostName", ncfg.DNSHostName == null ? "" : ncfg.DNSHostName.Trim()),
                        new SQLData("Caption", ncfg.Caption == null ? "" : ncfg.Caption.Trim()),
                        new SQLData("DHCPLeaseExpires", ncfg.DHCPLeaseExpires),
                        new SQLData("DHCPLeaseObtained", ncfg.DHCPLeaseObtained),
                        new SQLData("WINSEnableLMHostsLookup", ncfg.WINSEnableLMHostsLookup),
                        new SQLData("WINSHostLookupFile", ncfg.WINSHostLookupFile == null ? "" : ncfg.WINSHostLookupFile.Trim()),
                        new SQLData("WINSPrimaryServer", ncfg.WINSPrimaryServer == null ? "" : ncfg.WINSPrimaryServer.Trim()),
                        new SQLData("WINSSecondaryServer", ncfg.WINSSecondaryServer == null ? "" : ncfg.WINSSecondaryServer.Trim()),
                        new SQLData("WINSScopeID", ncfg.WINSScopeID == null ? "" : ncfg.WINSScopeID.Trim()));
                }

                if (ncfg.IPAddress == null)
                    ncfg.IPAddress = new List<string>();
                if (ncfg.IPSubnet == null)
                    ncfg.IPSubnet = new List<string>();
                if (ncfg.DefaultIPGateway == null)
                    ncfg.DefaultIPGateway = new List<string>();
                if (ncfg.DNSDomainSuffixSearchOrder == null)
                    ncfg.DNSDomainSuffixSearchOrder = new List<string>();
                if (ncfg.DNSServerSearchOrder == null)
                    ncfg.DNSServerSearchOrder = new List<string>();

                lock (ni.sqllock)
                    InsertSupplData(sql, netadapters.MachineID, ncfg.InterfaceIndex, 1, ncfg.IPAddress);
                lock (ni.sqllock)
                    InsertSupplData(sql, netadapters.MachineID, ncfg.InterfaceIndex, 2, ncfg.IPSubnet);
                lock (ni.sqllock)
                    InsertSupplData(sql, netadapters.MachineID, ncfg.InterfaceIndex, 3, ncfg.DefaultIPGateway);
                lock (ni.sqllock)
                    InsertSupplData(sql, netadapters.MachineID, ncfg.InterfaceIndex, 4, ncfg.DNSDomainSuffixSearchOrder);
                lock (ni.sqllock)
                    InsertSupplData(sql, netadapters.MachineID, ncfg.InterfaceIndex, 5, ncfg.DNSServerSearchOrder);
            }


            return (RESTStatus.Success);
        }

        [VulpesRESTfulRet("LstNetData")]
        public ListNetworkAdapterConfiguration LstNetData;


        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/netdata", "LstNetData", "id")]
        public RESTStatus ListNetData(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (string.IsNullOrWhiteSpace(id) == true)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.NotFound);
            }

            lock (ni.sqllock)
            {
                if (Computers.MachineExists(sql, id) == false)
                {
                    ni.Error = "Invalid data";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.NotFound);
                }
            }

            LstNetData = new ListNetworkAdapterConfiguration();
            LstNetData.Items = new List<NetworkAdapterConfiguration>();
            LstNetData.MachineID = id;

            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("SELECT * FROM networkconfig WHERE MachineID=@mid", new SQLParam("@mid", id));
                while (dr.Read())
                {
                    NetworkAdapterConfiguration n = new NetworkAdapterConfiguration();
                    sql.LoadIntoClass(dr, n);
                    LstNetData.Items.Add(n);
                }
                dr.Close();
            }

            foreach (NetworkAdapterConfiguration n in LstNetData.Items)
            {
                n.IPAddress = new List<string>();
                n.IPSubnet = new List<string>();
                n.DefaultIPGateway = new List<string>();
                n.DNSDomainSuffixSearchOrder = new List<string>();
                n.DNSServerSearchOrder = new List<string>();

                lock (ni.sqllock)
                {
                    SqlDataReader dr = sql.ExecSQLReader("select * from NetworkConfigSuppl WHERE MachineID=@mid AND InterfaceIndex=@i order by Type,[Order]",
                    new SQLParam("@mid", id),
                    new SQLParam("@i", n.InterfaceIndex));
                    while (dr.Read())
                    {
                        switch (Convert.ToInt32(dr["Type"]))
                        {
                            case 1:
                                n.IPAddress.Add(Convert.ToString(dr["Data"])); break;
                            case 2:
                                n.IPSubnet.Add(Convert.ToString(dr["Data"])); break;
                            case 3:
                                n.DefaultIPGateway.Add(Convert.ToString(dr["Data"])); break;
                            case 4:
                                n.DNSDomainSuffixSearchOrder.Add(Convert.ToString(dr["Data"])); break;
                            case 5:
                                n.DNSServerSearchOrder.Add(Convert.ToString(dr["Data"])); break;
                        }
                    }
                    dr.Close();
                }
            }
            return (RESTStatus.Success);
        }

    }
}
