using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class Contract
    {
        [VulpesRESTfulRet("ContractInfos")]
        public ContractInfosList CI;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/getcontractinfos", "ContractInfos", "")]
        public RESTStatus GetContractInfos(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            CI = new ContractInfosList();
            CI.Items = new List<ContractInfos>();

            if (Settings.Default.UseContract == false)
                return (RESTStatus.Success);
            lock (ni.sqllock)
            {
                SqlDataReader dr = sql.ExecSQLReader("select * from contracts order by ContractID");
                while (dr.Read())
                {
                    ContractInfos i = new ContractInfos();
                    i.IncludedComputers = new List<ComputerData>();
                    sql.LoadIntoClass(dr, i);
                    CI.Items.Add(i);
                }
                dr.Close();
            }

            foreach (ContractInfos i in CI.Items)
            {
                List<string> Machines = new List<string>();
                lock (ni.sqllock)
                {
                    SqlDataReader dr = sql.ExecSQLReader("select MachineID from ComputerAccounts where ContractID=@c",
                        new SQLParam("@c", i.ContractID));
                    while (dr.Read())
                    {
                        Machines.Add(Convert.ToString(dr["MachineID"]));
                    }
                    dr.Close();
                }

                foreach (string M in Machines)
                {
                    ComputerData c = Computers.GetComputerDetail(sql, M);
                    i.IncludedComputers.Add(c);
                }
            }

            return (RESTStatus.Success);
        }
    }
}
