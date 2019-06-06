using FoxSDC_Common;
using FoxSDC_Server.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class SMARTData
    {
        List<VulpesSMARTInfo> LoadData(string MachineID, SQLLib sql)
        {
            List<VulpesSMARTInfo> lst = new List<VulpesSMARTInfo>();

            SqlDataReader dr = sql.ExecSQLReader("Select * from SMARTData WHERE MachineID=@id", new SQLParam("@id", MachineID));
            while (dr.Read())
            {
                VulpesSMARTInfo s = new VulpesSMARTInfo();
                sql.LoadIntoClass(dr, s);
                lst.Add(s);
            }
            dr.Close();

            foreach (VulpesSMARTInfo sm in lst)
            {
                sm.Attributes = new Dictionary<int, VulpesSMARTAttribute>();

                dr = sql.ExecSQLReader("Select * from SMARTDataAttributes WHERE MachineID=@id AND PnPDeviceID=@pnp",
                    new SQLParam("@id", MachineID),
                    new SQLParam("@pnp", sm.PNPDeviceID));
                while (dr.Read())
                {
                    VulpesSMARTAttribute attr = new VulpesSMARTAttribute();
                    sql.LoadIntoClass(dr, attr);
                    sm.Attributes.Add(attr.ID, attr);
                }
                dr.Close();
            }
            return (lst);
        }

        bool TestAttributesKVP(List<VulpesSMARTInfo> lst)
        {
            foreach (VulpesSMARTInfo info in lst)
            {
                if (info.Attributes == null)
                    return (true);
                foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in info.Attributes)
                {
                    if (kvp.Key != kvp.Value.ID)
                        return (false);
                }
            }
            return (true);
        }

        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/reports/smartinfo", "", "")]
        public RESTStatus ReportSmartInfos(SQLLib sql, VulpesSMARTInfoList SMART, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ComputerLogin) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (SMART == null)
            {
                ni.Error = "Invalid Items";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.Fail);
            }

            SMART.MachineID = ni.Username;

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", SMART.MachineID))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.NotFound);
                }
            }

            if (SMART.List == null)
                SMART.List = new List<VulpesSMARTInfo>();

            if (TestAttributesKVP(SMART.List) == false)
            {
                ni.Error = "Invalid Data (KVP)";
                ni.ErrorID = ErrorFlags.InvalidValue;
                return (RESTStatus.NotFound);
            }

            List<VulpesSMARTInfo> InDB = LoadData(SMART.MachineID, sql);

            List<VulpesSMARTInfo> Removed = new List<VulpesSMARTInfo>();
            List<VulpesSMARTInfo> Updated = new List<VulpesSMARTInfo>();
            List<VulpesSMARTInfo> Unchanged = new List<VulpesSMARTInfo>();
            List<VulpesSMARTInfo> Added = new List<VulpesSMARTInfo>();


            foreach (VulpesSMARTInfo SMARTDevice in SMART.List)
            {
                bool Found = false;
                foreach (VulpesSMARTInfo idb in InDB)
                {
                    if (SMARTDescription.ComparePart(SMARTDevice, idb) == true)
                    {
                        if (SMARTDescription.CompareFull(SMARTDevice, idb, null) == true)
                        {
                            Unchanged.Add(SMARTDevice);
                            Found = true;
                            break;
                        }
                        else
                        {
                            Updated.Add(SMARTDevice);
                            Found = true;
                            break;
                        }
                    }
                }
                if (Found == false)
                {
                    Added.Add(SMARTDevice);
                }
            }

            foreach (VulpesSMARTInfo SMARTDevice in InDB)
            {
                bool Found = false;
                foreach (VulpesSMARTInfo i in SMART.List)
                {
                    if (SMARTDescription.ComparePart(SMARTDevice, i) == true)
                        Found = true;
                }
                if (Found == false)
                {
                    Removed.Add(SMARTDevice);
                }
            }

            foreach (VulpesSMARTInfo SMARTDevice in Added)
            {
                if (string.IsNullOrWhiteSpace(SMARTDevice.PNPDeviceID) == true)
                    continue;

                lock (ni.sqllock)
                {
                    sql.InsertMultiData("SMARTData",
                        new SQLData("MachineID", SMART.MachineID),
                        new SQLData("PNPDeviceID", SMARTDevice.PNPDeviceID == null ? "" : SMARTDevice.PNPDeviceID),
                        new SQLData("Caption", SMARTDevice.Caption == null ? "" : SMARTDevice.Caption),
                        new SQLData("FirmwareRevision", SMARTDevice.FirmwareRevision == null ? "" : SMARTDevice.FirmwareRevision),
                        new SQLData("InterfaceType", SMARTDevice.InterfaceType == null ? "" : SMARTDevice.InterfaceType),
                        new SQLData("Model", SMARTDevice.Model == null ? "" : SMARTDevice.Model),
                        new SQLData("Name", SMARTDevice.Name == null ? "" : SMARTDevice.Name),
                        new SQLData("PredictFailure", SMARTDevice.PredictFailure),
                        new SQLData("SerialNumber", SMARTDevice.SerialNumber == null ? "" : SMARTDevice.SerialNumber),
                        new SQLData("Size", SMARTDevice.Size),
                        new SQLData("Status", SMARTDevice.Status == null ? "" : SMARTDevice.Status),
                        new SQLData("DT", DateTime.UtcNow));

                    if (SMARTDevice.VendorSpecific != null)
                        sql.ExecSQL("UPDATE SMARTData SET VendorSpecific=@v WHERE MachineID=@id AND PNPDeviceID=@pnp",
                            new SQLParam("@pnp", SMARTDevice.PNPDeviceID),
                            new SQLParam("@id", SMART.MachineID),
                            new SQLParam("@v", SMARTDevice.VendorSpecific));

                    if (SMARTDevice.VendorSpecificThreshold != null)
                        sql.ExecSQL("UPDATE SMARTData SET VendorSpecificThreshold=@v WHERE MachineID=@id AND PNPDeviceID=@pnp",
                            new SQLParam("@pnp", SMARTDevice.PNPDeviceID),
                            new SQLParam("@id", SMART.MachineID),
                            new SQLParam("@v", SMARTDevice.VendorSpecificThreshold));
                }

                if (SMARTDevice.Attributes == null)
                    SMARTDevice.Attributes = new Dictionary<int, VulpesSMARTAttribute>();
                List<int> HasID = new List<int>();
                foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in SMARTDevice.Attributes)
                {
                    if (HasID.Contains(kvp.Value.ID) == true)
                        continue;
                    sql.InsertMultiData("SMARTDataAttributes",
                        new SQLData("MachineID", SMART.MachineID),
                        new SQLData("PNPDeviceID", SMARTDevice.PNPDeviceID),
                        new SQLData("FailureImminent", kvp.Value.FailureImminent),
                        new SQLData("Flags", kvp.Value.Flags),
                        new SQLData("ID", kvp.Value.ID),
                        new SQLData("Threshold", kvp.Value.Threshold),
                        new SQLData("Value", kvp.Value.Value),
                        new SQLData("Vendordata", kvp.Value.Vendordata),
                        new SQLData("Worst", kvp.Value.Worst));
                    HasID.Add(kvp.Value.ID);
                }
            }

            foreach (VulpesSMARTInfo SMARTDevice in Removed)
            {
                if (string.IsNullOrWhiteSpace(SMARTDevice.PNPDeviceID) == true)
                    continue;

                lock (ni.sqllock)
                {
                    sql.ExecSQL("DELETE FROM SMARTDataAttributes WHERE MachineID=@id AND PNPDeviceID=@pnp",
                        new SQLParam("@id", SMART.MachineID),
                        new SQLParam("@pnp", SMARTDevice.PNPDeviceID));
                    sql.ExecSQL("DELETE FROM SMARTData WHERE MachineID=@id AND PNPDeviceID=@pnp",
                        new SQLParam("@id", SMART.MachineID),
                        new SQLParam("@pnp", SMARTDevice.PNPDeviceID));
                }
            }

            foreach (VulpesSMARTInfo SMARTDevice in Updated)
            {
                if (string.IsNullOrWhiteSpace(SMARTDevice.PNPDeviceID) == true)
                    continue;

                lock (ni.sqllock)
                {
                    sql.ExecSQL("DELETE FROM SMARTDataAttributes WHERE MachineID=@id AND PNPDeviceID=@pnp",
                        new SQLParam("@id", SMART.MachineID),
                        new SQLParam("@pnp", SMARTDevice.PNPDeviceID));
                    sql.ExecSQL("DELETE FROM SMARTData WHERE MachineID=@id AND PNPDeviceID=@pnp",
                        new SQLParam("@id", SMART.MachineID),
                        new SQLParam("@pnp", SMARTDevice.PNPDeviceID));
                }
                lock (ni.sqllock)
                {
                    sql.InsertMultiData("SMARTData",
                      new SQLData("MachineID", SMART.MachineID),
                      new SQLData("PNPDeviceID", SMARTDevice.PNPDeviceID),
                      new SQLData("Caption", SMARTDevice.Caption == null ? "" : SMARTDevice.Caption),
                      new SQLData("FirmwareRevision", SMARTDevice.FirmwareRevision == null ? "" : SMARTDevice.FirmwareRevision),
                      new SQLData("InterfaceType", SMARTDevice.InterfaceType == null ? "" : SMARTDevice.InterfaceType),
                      new SQLData("Model", SMARTDevice.Model == null ? "" : SMARTDevice.Model),
                      new SQLData("Name", SMARTDevice.Name == null ? "" : SMARTDevice.Name),
                      new SQLData("PredictFailure", SMARTDevice.PredictFailure),
                      new SQLData("SerialNumber", SMARTDevice.SerialNumber == null ? "" : SMARTDevice.SerialNumber),
                      new SQLData("Size", SMARTDevice.Size),
                      new SQLData("Status", SMARTDevice.Status == null ? "" : SMARTDevice.Status),
                      new SQLData("DT", DateTime.UtcNow));

                    if (SMARTDevice.VendorSpecific != null)
                        sql.ExecSQL("UPDATE SMARTData SET VendorSpecific=@v WHERE MachineID=@id AND PNPDeviceID=@pnp",
                            new SQLParam("@pnp", SMARTDevice.PNPDeviceID),
                            new SQLParam("@id", SMART.MachineID),
                            new SQLParam("@v", SMARTDevice.VendorSpecific));

                    if (SMARTDevice.VendorSpecificThreshold != null)
                        sql.ExecSQL("UPDATE SMARTData SET VendorSpecificThreshold=@v WHERE MachineID=@id AND PNPDeviceID=@pnp",
                            new SQLParam("@pnp", SMARTDevice.PNPDeviceID),
                            new SQLParam("@id", SMART.MachineID),
                            new SQLParam("@v", SMARTDevice.VendorSpecificThreshold));
                }
                if (SMARTDevice.Attributes == null)
                    SMARTDevice.Attributes = new Dictionary<int, VulpesSMARTAttribute>();
                List<int> HasID = new List<int>();
                foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in SMARTDevice.Attributes)
                {
                    if (HasID.Contains(kvp.Value.ID) == true)
                        continue;
                    sql.InsertMultiData("SMARTDataAttributes",
                        new SQLData("MachineID", SMART.MachineID),
                        new SQLData("PNPDeviceID", SMARTDevice.PNPDeviceID),
                        new SQLData("FailureImminent", kvp.Value.FailureImminent),
                        new SQLData("Flags", kvp.Value.Flags),
                        new SQLData("ID", kvp.Value.ID),
                        new SQLData("Threshold", kvp.Value.Threshold),
                        new SQLData("Value", kvp.Value.Value),
                        new SQLData("Vendordata", kvp.Value.Vendordata),
                        new SQLData("Worst", kvp.Value.Worst));
                    HasID.Add(kvp.Value.ID);
                }
            }

            SmartDataLst l = new SmartDataLst();
            l.Added = Added;
            l.Removed = Removed;
            l.Unchanged = Unchanged;
            l.Updated = Updated;
            l.InDB = InDB;
            l.MachineID = SMART.MachineID;
            Thread t = new Thread(new ParameterizedThreadStart(new DReportingThread(ReportingThread)));
            t.Start(l);

            return (RESTStatus.Success);
        }

        delegate void DReportingThread(object SmartDataListO);

        class SmartDataLst
        {
            public List<VulpesSMARTInfo> Updated;
            public List<VulpesSMARTInfo> Unchanged;
            public List<VulpesSMARTInfo> Removed;
            public List<VulpesSMARTInfo> Added;
            public List<VulpesSMARTInfo> InDB;
            public string MachineID;
        }

        void ReportThings(SQLLib sql, string MachineID, string Method, VulpesSMARTInfo ar, ref Dictionary<string, Int64> AlreadyReported, ReportingPolicyElement RepElementRoot, List<int> UpdatedAttribs = null)
        {
            bool Critical = Method.ToLower() == "error" ? true : false;
            string ID = (Method.ToLower() == "error" ? "ERROR\\\\" : "") + MachineID + "\\\\" + ar.PNPDeviceID;

            bool ReportToAdmin = RepElementRoot.ReportToAdmin.Value;
            bool ReportToClient = RepElementRoot.ReportToClient.Value;
            bool UrgentForAdmin = RepElementRoot.UrgentForAdmin.Value;
            bool UrgentForClient = RepElementRoot.UrgentForClient.Value;

            if (AlreadyReported.ContainsKey(ID) == true)
            {
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.ReportToAdmin) != 0)
                    ReportToAdmin = false;
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.ReportToClient) != 0)
                    ReportToClient = false;
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.UrgentForAdmin) != 0)
                    UrgentForAdmin = false;
                if ((AlreadyReported[ID] & (Int64)ReportingFlags.UrgentForClient) != 0)
                    UrgentForClient = false;
            }

            if (ReportToAdmin == false && ReportToClient == false && UrgentForAdmin == false && UrgentForClient == false)
                return;
            ReportingFlags Flags = (ReportToAdmin == true ? ReportingFlags.ReportToAdmin : 0) |
                (ReportToClient == true ? ReportingFlags.ReportToClient : 0) |
                (UrgentForAdmin == true ? ReportingFlags.UrgentForAdmin : 0) |
                (UrgentForClient == true ? ReportingFlags.UrgentForClient : 0);

            ReportingSMART sm = new ReportingSMART();
            sm.App = ar;
            sm.UpdatedAttribs = UpdatedAttribs;

            ReportingProcessor.ReportSMART(sql, MachineID, Method, sm, Flags, Critical);

            if (AlreadyReported.ContainsKey(ID) == true)
            {
                AlreadyReported[ID] |= (Int64)Flags;
            }
            else
            {
                AlreadyReported.Add(ID, (Int64)Flags);
            }
        }

        void ReportingThread(object SmartDataListO)
        {
            SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for SMART Device Data");
            if (sql == null)
            {
                FoxEventLog.WriteEventLog("Cannot connect to SQL Server for SMART Device Reporting!", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            try
            {
                SmartDataLst SmartDataList = (SmartDataLst)SmartDataListO;
                ComputerData computerdata = Computers.GetComputerDetail(sql, SmartDataList.MachineID);
                if (computerdata == null)
                {
                    FoxEventLog.WriteEventLog("Cannot get any computer data for SMART Device Reporting!", System.Diagnostics.EventLogEntryType.Error);
                    return;
                }

                List<PolicyObject> Pol = Policies.GetPolicyForComputerInternal(sql, SmartDataList.MachineID);
                Dictionary<string, Int64> AlreadyReported = new Dictionary<string, long>();
                foreach (PolicyObject PolO in Pol)
                {
                    if (PolO.Type != PolicyIDs.ReportingPolicy)
                        continue;
                    ReportingPolicyElement RepElementRoot = JsonConvert.DeserializeObject<ReportingPolicyElement>(Policies.GetPolicy(sql, PolO.ID).Data);
                    if (RepElementRoot.Type != ReportingPolicyType.SMART)
                        continue;
                    if (RepElementRoot.ReportToAdmin == null)
                        RepElementRoot.ReportToAdmin = false;
                    if (RepElementRoot.ReportToClient == null)
                        RepElementRoot.ReportToClient = false;
                    if (RepElementRoot.UrgentForAdmin == null)
                        RepElementRoot.UrgentForAdmin = false;
                    if (RepElementRoot.UrgentForClient == null)
                        RepElementRoot.UrgentForClient = false;
                    if (RepElementRoot.ReportToAdmin == false && RepElementRoot.ReportToClient == false && RepElementRoot.UrgentForAdmin == false &&
                        RepElementRoot.UrgentForClient == false)
                        continue;

                    foreach (string Element in RepElementRoot.ReportingElements)
                    {
                        ReportingPolicyElementSMART arprep = JsonConvert.DeserializeObject<ReportingPolicyElementSMART>(Element);
                        if (arprep.NotifyOnAdd == false && arprep.NotifyOnRemove == false && arprep.NotifyOnUpdate == false &&
                            arprep.NotifyOnError == false)
                            continue;

                        if (arprep.NotifyOnAdd == true)
                        {
                            foreach (VulpesSMARTInfo ar in SmartDataList.Added)
                                ReportThings(sql, SmartDataList.MachineID, "Add", ar, ref AlreadyReported, RepElementRoot);
                        }

                        if (arprep.NotifyOnUpdate == true)
                        {
                            foreach (VulpesSMARTInfo ar in SmartDataList.Updated)
                            {
                                foreach (VulpesSMARTInfo indbvsm in SmartDataList.InDB)
                                {
                                    if (indbvsm.PNPDeviceID == ar.PNPDeviceID)
                                    {
                                        if (SMARTDescription.CompareFull(ar, indbvsm, arprep.SkipAttribUpdateReport) == false)
                                        {
                                            if (indbvsm.Attributes != null)
                                            {
                                                List<int> UpdatedAttribs = new List<int>();
                                                if (ar.Attributes == null)
                                                    ar.Attributes = new Dictionary<int, VulpesSMARTAttribute>();
                                                foreach (KeyValuePair<int, VulpesSMARTAttribute> indb in indbvsm.Attributes)
                                                {
                                                    if (ar.Attributes.ContainsKey(indb.Key) == true)
                                                    {
                                                        if (ar.Attributes[indb.Key].FailureImminent != indb.Value.FailureImminent ||
                                                            ar.Attributes[indb.Key].Flags != indb.Value.Flags ||
                                                            ar.Attributes[indb.Key].ID != indb.Value.ID ||
                                                            ar.Attributes[indb.Key].Threshold != indb.Value.Threshold ||
                                                            ar.Attributes[indb.Key].Value != indb.Value.Value ||
                                                            ar.Attributes[indb.Key].Vendordata != indb.Value.Vendordata ||
                                                            ar.Attributes[indb.Key].Worst != indb.Value.Worst)
                                                            UpdatedAttribs.Add(indb.Key);
                                                    }
                                                }
                                                ReportThings(sql, SmartDataList.MachineID, "Update", ar, ref AlreadyReported, RepElementRoot, UpdatedAttribs);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (arprep.NotifyOnRemove == true)
                        {
                            foreach (VulpesSMARTInfo ar in SmartDataList.Removed)
                                ReportThings(sql, SmartDataList.MachineID, "Remove", ar, ref AlreadyReported, RepElementRoot);
                        }

                        if (arprep.NotifyOnError == true)
                        {
                            foreach (VulpesSMARTInfo ar in SmartDataList.Added)
                            {
                                if (SMARTDescription.IsInError(ar) == true)
                                    ReportThings(sql, SmartDataList.MachineID, "Error", ar, ref AlreadyReported, RepElementRoot);
                            }
                            foreach (VulpesSMARTInfo ar in SmartDataList.Updated)
                            {
                                if (SMARTDescription.IsInError(ar) == true)
                                {
                                    foreach (VulpesSMARTInfo indbvsm in SmartDataList.InDB)
                                    {
                                        if (indbvsm.PNPDeviceID == ar.PNPDeviceID)
                                        {
                                            if (SMARTDescription.CompareFullCriticalOnly(indbvsm, ar) == false)
                                            {
                                                ReportThings(sql, SmartDataList.MachineID, "Error", ar, ref AlreadyReported, RepElementRoot);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("SEH in SMART Data Reporting " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                try
                {
                    sql.CloseConnection();
                }
                catch
                {

                }
            }
        }

        [ReportingExplainAttr(ReportingPolicyType.SMART)]
        public class SMARTReporting : IReportingExplain
        {
            public string Explain(string JSON)
            {
                if (JSON == null)
                    return ("SMART Data: no data");

                try
                {
                    ReportingSMART rd = JsonConvert.DeserializeObject<ReportingSMART>(JSON);
                    if (rd.UpdatedAttribs == null)
                        rd.UpdatedAttribs = new List<int>();

                    string res = "Action: " + rd.Action + "\r\n";
                    res += rd.App.Model + "\r\n";
                    res += "SN: " + rd.App.SerialNumber + "\r\n";
                    res += "Firmware: " + rd.App.FirmwareRevision + "\r\n";
                    res += "Size: " + CommonUtilities.NiceSize(rd.App.Size) + "\r\n\r\n";
                    if (rd.App.Attributes == null)
                        rd.App.Attributes = new Dictionary<int, VulpesSMARTAttribute>();
                    if (rd.App.Attributes.Count > 0)
                        res += "Attrib|Name|Ideal|Fail|Value|Threshold|Worst|Raw\r\n";
                    foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in rd.App.Attributes)
                    {
                        string Name = "???";
                        string AttribIdeal = "";
                        if (SMARTDescription.Descriptions.ContainsKey(kvp.Key) == true)
                        {
                            Name = SMARTDescription.Descriptions[kvp.Key].Description;
                            switch (SMARTDescription.Descriptions[kvp.Key].Ideal)
                            {
                                case SMARTDescriptionEnum.Critical: AttribIdeal = "‼"; break;
                                case SMARTDescriptionEnum.LowIdeal: AttribIdeal = "↓"; break;
                                case SMARTDescriptionEnum.HighIdeal: AttribIdeal = "↑"; break;
                            }
                        }

                        res += "0x" + kvp.Value.ID.ToString("X") + "|" + Name + "|" + AttribIdeal + "|" + (kvp.Value.FailureImminent == true ? "!true!" : "false") + "|" +
                            kvp.Value.Value + "|" + kvp.Value.Threshold + "|" + kvp.Value.Worst + "|" + kvp.Value.Vendordata +
                            (SMARTDescription.IsAttribInError(kvp.Value) == true ? " ◄ ◄ ◄ ◄ ◄" : "") +
                            (rd.UpdatedAttribs.Contains(kvp.Key) == true ? "←←" : "") +
                            "\r\n";
                    }
                    return (res);
                }
                catch
                {
                    return ("SMART Report Data faulty: " + JSON);
                }
            }

            public Image GetIcon()
            {
                return (Resources.smart_hdd.ToBitmap());
            }
        }

        [ReportingExplainAttr(ReportingPolicyType.SMARTCritical)]
        public class SMARTCriticalReporting : IReportingExplain
        {
            public string Explain(string JSON)
            {
                if (JSON == null)
                    return ("SMART Data: no data");

                try
                {
                    ReportingSMART rd = JsonConvert.DeserializeObject<ReportingSMART>(JSON);
                    if (rd.UpdatedAttribs == null)
                        rd.UpdatedAttribs = new List<int>();

                    string res = "--- ERROR - FAILURE IMMINENT ---\r\n";
                    res += rd.App.Model + "\r\n";
                    res += "SN: " + rd.App.SerialNumber + "\r\n";
                    res += "Firmware: " + rd.App.FirmwareRevision + "\r\n";
                    res += "Size: " + CommonUtilities.NiceSize(rd.App.Size) + "\r\n\r\n";
                    if (rd.App.Attributes == null)
                        rd.App.Attributes = new Dictionary<int, VulpesSMARTAttribute>();
                    if (rd.App.Attributes.Count > 0)
                        res += "Attrib|Name|Ideal|Fail|Value|Threshold|Worst|Raw\r\n";
                    foreach (KeyValuePair<int, VulpesSMARTAttribute> kvp in rd.App.Attributes)
                    {
                        string Name = "???";
                        string AttribIdeal = "";
                        if (SMARTDescription.Descriptions.ContainsKey(kvp.Key) == true)
                        {
                            Name = SMARTDescription.Descriptions[kvp.Key].Description;
                            switch (SMARTDescription.Descriptions[kvp.Key].Ideal)
                            {
                                case SMARTDescriptionEnum.Critical: AttribIdeal = "‼"; break;
                                case SMARTDescriptionEnum.LowIdeal: AttribIdeal = "↓"; break;
                                case SMARTDescriptionEnum.HighIdeal: AttribIdeal = "↑"; break;
                            }
                        }

                        res += "0x" + kvp.Value.ID.ToString("X") + "|" + Name + "|" + AttribIdeal + "|" + (kvp.Value.FailureImminent == true ? "!true!" : "false") + "|" +
                            kvp.Value.Value + "|" + kvp.Value.Threshold + "|" + kvp.Value.Worst + "|" + kvp.Value.Vendordata +
                            (SMARTDescription.IsAttribInError(kvp.Value) == true ? " ◄ ◄ ◄ ◄ ◄" : "") +
                            (rd.UpdatedAttribs.Contains(kvp.Key) == true ? "←←" : "") +
                            "\r\n";
                    }
                    return (res);
                }
                catch
                {
                    return ("SMART Report Data faulty: " + JSON);
                }
            }

            public Image GetIcon()
            {
                return (Resources.smart_hddcrit.ToBitmap());
            }
        }

        [VulpesRESTfulRet("SMARTRet")]
        VulpesSMARTInfoList SMARTRet;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/reports/smartinfo", "SMARTRet", "id")]
        public RESTStatus GetSMARTInfos(SQLLib sql, object dummy, NetworkConnectionInfo ni, string id)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            lock (ni.sqllock)
            {
                if (Convert.ToInt32(sql.ExecSQLScalar("SELECT COUNT(*) FROM ComputerAccounts WHERE MachineID=@m",
                    new SQLParam("@m", id))) == 0)
                {
                    ni.Error = "Invalid MachineID";
                    ni.ErrorID = ErrorFlags.InvalidValue;
                    return (RESTStatus.NotFound);
                }
            }

            SMARTRet = new VulpesSMARTInfoList();
            SMARTRet.MachineID = id;
            SMARTRet.List = LoadData(id, sql);

            return (RESTStatus.Success);
        }
    }
}
