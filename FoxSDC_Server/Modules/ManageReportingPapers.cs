using FoxSDC_Common;
using FoxSDC_Server.Properties;
using FoxSDC_Server.ReportingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
    class ManageReportingPapers
    {
        readonly List<string> SupportedNames = new List<string> { "COMPUTERREPORT" };

        [VulpesRESTfulRet("RetPaper")]
        public NetByte RetPaper;

        [VulpesRESTfulRet("RetListPaper")]
        public NetStringList RetListPaper;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/rep/listpaper", "RetListPaper", "")]
        public RESTStatus GetPaperList(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            RetListPaper = new NetStringList();
            RetListPaper.Items = SupportedNames;

            return (RESTStatus.Success);
        }


        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/rep/paper", "RetPaper", "Paper")]
        public RESTStatus GetPaperTemplate(SQLLib sql, object dummy, NetworkConnectionInfo ni, string Paper)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (string.IsNullOrWhiteSpace(Paper) == true)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            Paper = Paper.ToUpper().Trim();

            if (SupportedNames.Contains(Paper) == false)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            RetPaper = new NetByte();
            RetPaper.Data = null;

            lock (ni.sqllock)
            {
                object o = sql.ExecSQLScalar("SELECT [data] from ReportPapers WHERE ID=@id",
            new SQLParam("@id", Paper));
                if (o is DBNull || o is null)
                {
                    switch (Paper)
                    {
                        case "COMPUTERREPORT":
                            RetPaper.Data = Resources.Computer_Report;
                            break;
                    }
                }
                else
                {
                    RetPaper.Data = (byte[])o;
                }
            }

            return (RESTStatus.Success);
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/rep/testpaper", "RetPaper", "Paper")]
        public RESTStatus TestPaperTemplate(SQLLib sql, object dummy, NetworkConnectionInfo ni, string Paper)
        {
#if !TXTREPORT
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (string.IsNullOrWhiteSpace(Paper) == true)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            Paper = Paper.ToUpper().Trim();

            if (SupportedNames.Contains(Paper) == false)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            RetPaper = new NetByte();
            RetPaper.Data = null;

            lock (ni.sqllock)
            {
                switch (Paper)
                {
                    case "COMPUTERREPORT":
                        RetPaper.Data = RenderReport.RenderReportData(RenderReport.GetReportPaperData(sql, Paper, Resources.Computer_Report), new Dictionary<string, object>(), "PDF");
                        break;
                }
            }

            return (RESTStatus.Success);
#else
            ni.Error = "Unsupported functionality";
            ni.ErrorID = ErrorFlags.SystemError;
            return (RESTStatus.Fail);
#endif
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/rep/addpaper", "", "")]
        public RESTStatus AddPaperTemplate(SQLLib sql, ReportPaper req, NetworkConnectionInfo ni)
        {
#if !TXTREPORT
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (req == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(req.Name) == true)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            req.Name = req.Name.ToUpper().Trim();

            if (SupportedNames.Contains(req.Name) == false)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (req.data == null)
            {
                lock (ni.sqllock)
                {
                    sql.ExecSQL("DELETE from ReportPapers WHERE ID=@id",
                    new SQLParam("@id", req.Name));
                }
                return (RESTStatus.Success);
            }
            else
            {
                if (RenderReport.RenderReportData(req.data, new Dictionary<string, object>()) == null)
                {
                    ni.Error = "Rendering of report failed";
                    ni.ErrorID = ErrorFlags.InvalidData;
                    return (RESTStatus.Fail);
                }

                lock (ni.sqllock)
                {
                    if (sql.ExecSQL("if exists(select * from ReportPapers where [ID]=@ID) update ReportPapers set [Data]=@data,DT=getutcdate() where [ID]=@ID else insert into ReportPapers ([ID],[Data]) values(@ID,@data)",
                    new SQLParam("@ID", req.Name),
                    new SQLParam("@data", req.data)) == false)
                    {
                        ni.Error = "SQL Error";
                        ni.ErrorID = ErrorFlags.SQLError;
                        return (RESTStatus.Fail);
                    }
                }
            }
            return (RESTStatus.Success);
#else
            ni.Error = "Unsupported functionality";
            ni.ErrorID = ErrorFlags.SystemError;
            return (RESTStatus.Fail);
#endif
        }

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.POST, "api/mgmt/rep/getpaper", "RetPaper", "")]
        public RESTStatus GetPaperData(SQLLib sql, ReportPaperRequest req, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }

            if (req == null)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (string.IsNullOrWhiteSpace(req.Name) == true)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            if (SupportedNames.Contains(req.Name) == false)
            {
                ni.Error = "Invalid data";
                ni.ErrorID = ErrorFlags.InvalidData;
                return (RESTStatus.Fail);
            }

            RetPaper = new NetByte();

            switch (req.Name)
            {
                case "COMPUTERREPORT":
                    if (req.MachineIDs == null)
                    {
                        ni.Error = "Invalid data";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.Fail);
                    }
                    if (req.MachineIDs.Count == 0)
                    {
                        ni.Error = "Invalid data";
                        ni.ErrorID = ErrorFlags.InvalidData;
                        return (RESTStatus.Fail);
                    }

                    List<string> MachinesOK = new List<string>();

                    foreach (string m in req.MachineIDs)
                    {
                        string Query = "";
                        List<SQLParam> SQLParams = new List<SQLParam>();

                        lock (ni.sqllock)
                        {
                            if (Computers.MachineExists(sql, m) == false)
                                continue;
                        }

                        if (req.From == null && req.To == null)
                        {
                            Query = "Select count(*) from Reporting where machineid=@mid";
                            SQLParams.Add(new SQLParam("@mid", m));
                        }
                        if (req.From != null && req.To == null)
                        {
                            Query = "Select count(*) from Reporting where machineid=@mid AND Reported>=@d1";
                            SQLParams.Add(new SQLParam("@mid", m));
                            SQLParams.Add(new SQLParam("@d1", req.From.Value));
                        }
                        if (req.From == null && req.To != null)
                        {
                            Query = "Select count(*) from Reporting where machineid=@mid AND Reported<=@d1";
                            SQLParams.Add(new SQLParam("@mid", m));
                            SQLParams.Add(new SQLParam("@d1", req.To.Value));
                        }
                        if (req.From != null && req.To != null)
                        {
                            Query = "Select count(*) from Reporting where machineid=@mid and Reported between @d1 and @d2";
                            SQLParams.Add(new SQLParam("@mid", m));
                            SQLParams.Add(new SQLParam("@d1", req.From.Value));
                            SQLParams.Add(new SQLParam("@d2", req.To.Value));
                        }

                        lock (ni.sqllock)
                        {
                            if (Convert.ToInt32(sql.ExecSQLScalar(Query, SQLParams.ToArray())) == 0)
                                continue;
                        }
                        MachinesOK.Add(m);
                    }

                    if (MachinesOK.Count == 0)
                    {
                        ni.Error = "Machines has no report";
                        ni.ErrorID = ErrorFlags.NoData;
                        return (RESTStatus.Fail);
                    }

                    lock (ni.sqllock)
                    {
                        RetPaper.Data = RenderReport.RenderMachineReport(sql, MachinesOK, req.From, req.To, ReportingFlagsPaper.ReReport, "PDF");
                    }
                    break;
            }
            return (RESTStatus.Success);
        }
    }
}
