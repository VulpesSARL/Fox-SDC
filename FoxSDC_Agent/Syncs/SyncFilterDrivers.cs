using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class SyncFilterDrivers
    {
        static void ProcessFilter(string ClassGUID, string Name, int Type, ref List<FilterDriver> l, RegistryKey currentclass)
        {
            object o;
            o = currentclass.GetValue(Name, null);
            if (o == null)
                return;
            string[] Filters = null;
            if (currentclass.GetValueKind(Name) == RegistryValueKind.String)
                Filters = new string[] { (string)o };
            if (currentclass.GetValueKind(Name) == RegistryValueKind.MultiString)
                Filters = (string[])o;
            if (Filters == null)
                return;
            foreach(string Filter in Filters)
            {
                FilterDriver drv = new FilterDriver();
                drv.ClassGUID = ClassGUID;
                drv.ServiceName = Filter;
                drv.Type = Type;
                l.Add(drv);
            }
        }

        static List<FilterDriver> GetDrivers()
        {
            List<FilterDriver> l = new List<FilterDriver>();

            using (RegistryKey classes = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class"))
            {
                if (classes == null)
                    return (l);
                foreach (string rclass in classes.GetSubKeyNames())
                {
                    using (RegistryKey currentclass = classes.OpenSubKey(rclass))
                    {
                        ProcessFilter(rclass, "UpperFilters", 1, ref l, currentclass);
                        ProcessFilter(rclass, "LowerFilters", 2, ref l, currentclass);
                    }
                }
            }

            return (l);
        }

        public static bool DoSyncFilters()
        {
            try
            {
                Network net;
                net = Utilities.ConnectNetwork(0);
                if (net == null)
                    return (false);
                Status.UpdateMessage(0, "Collecting filters configuration");

                List<FilterDriver> lst =  GetDrivers();

                Status.UpdateMessage(0, "Reporting filters configuration");
                FilterDriverList lstt = new FilterDriverList();
                lstt.List = lst;
                lstt.MachineID = SystemInfos.SysInfo.MachineID;

                net.ReportFiltersList(lstt);

                net.CloseConnection();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Servere error while syncing filters config: " + ee.ToString(), EventLogEntryType.Error);
            }
            Status.UpdateMessage(0);

            return (true);
        }
    }
}
