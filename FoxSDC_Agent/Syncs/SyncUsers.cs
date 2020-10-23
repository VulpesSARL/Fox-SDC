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
    class SyncUsers
    {
        public static bool DoSyncUsers()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);
                Status.UpdateMessage(0, "Collecting users");

                Dictionary<string, string> lst = Userregistries.GetUsers();
                if (lst == null)
                {
                    net.CloseConnection();
                    Status.UpdateMessage(0);
                    return (true);
                }

                Status.UpdateMessage(0, "Reporting users");
                UsersList lstt = new UsersList();
                lstt.Users = lst;
                lstt.MachineID = SystemInfos.SysInfo.MachineID;

                net.ReportUsers(lstt);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing user list: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }
    }
}
