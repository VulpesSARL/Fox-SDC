using FoxSDC_Server.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace FoxSDC_Server.ReportingSystem
{
    class RenderReport
    {
        //Telerik Supported output formats: https://docs.telerik.com/reporting/configuring-rendering-extensions

        public static byte[] GetReportPaperData(SQLLib sql, string Paper, byte[] Default)
        {
            object d = sql.ExecSQLScalar("select [data] from ReportPapers where [ID]=@ID",
                new SQLParam("@id", Paper));
            if (d is DBNull || d is null)
                return (Default);
            return ((byte[])d);
        }

        public static ReportSource RenderReportData(byte[] ReportBinaryData, Dictionary<string, object> Parameters)
        {
            ReportPackager pack = new ReportPackager();
            Telerik.Reporting.Report doc = pack.Unpackage(new MemoryStream(ReportBinaryData));

            InstanceReportSource instance = new InstanceReportSource();
            instance.ReportDocument = doc;

            if (Parameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in Parameters)
                    instance.Parameters.Add(new Telerik.Reporting.Parameter(kvp.Key, kvp.Value));
            }

            return (instance);
        }

        public static byte[] RenderReportData(byte[] ReportBinaryData, Dictionary<string, object> Parameters, string Output = "PDF")
        {
            try
            {
                ReportSource instance = RenderReportData(ReportBinaryData, Parameters);

                ReportProcessor reportprocessor = new ReportProcessor();
                RenderingResult result = reportprocessor.RenderReport(Output, instance, null);

                return (result.DocumentBytes);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        public static byte[] RenderMachineReport(SQLLib sql, string MachineID, DateTime? From, DateTime? To, ReportingFlagsPaper ReportingPaper, string Output = "PDF")
        {
            byte[] ReportFile = GetReportPaperData(sql, "COMPUTERREPORT", Resources.Computer_Report);

            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("MachineID", MachineID);
            Params.Add("From", From);
            Params.Add("To", To);
            Params.Add("ReportingPaper", (int)ReportingPaper);

            return (RenderReportData(ReportFile, Params, Output));
        }

        public static byte[] RenderMachineReport(SQLLib sql, List<string> MachineIDs, DateTime? From, DateTime? To, ReportingFlagsPaper ReportingPaper, string Output = "PDF")
        {
            byte[] ReportFile = GetReportPaperData(sql, "COMPUTERREPORT", Resources.Computer_Report);

            ReportBook RepBook = new ReportBook();

            foreach (string MachineID in MachineIDs)
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("MachineID", MachineID);
                Params.Add("From", From);
                Params.Add("To", To);
                Params.Add("ReportingPaper", (int)ReportingPaper);

                RepBook.ReportSources.Add(RenderReportData(ReportFile, Params));
            }

            InstanceReportSource instance = new InstanceReportSource();
            instance.ReportDocument = RepBook;

            ReportProcessor reportprocessor = new ReportProcessor();
            RenderingResult result = reportprocessor.RenderReport(Output, instance, null);

            return (result.DocumentBytes);
        }

    }
}
