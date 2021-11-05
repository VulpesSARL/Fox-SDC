using FoxSDC_Server.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if !TXTREPORT
using Telerik.Reporting;
using Telerik.Reporting.Processing;
#endif

/*
 
Note for GitHub Users: if you do not have/want Telerik Reporting, you can configure it to use plain text reports
Go to the Build Options, and define TXTREPORT compilation symbol
 
 */


namespace FoxSDC_Server.ReportingSystem
{
    class RenderReport
    {
#if !TXTREPORT
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
#else
        public static byte[] RenderMachineReport(SQLLib sql, List<string> MachineIDs, DateTime? From, DateTime? To, ReportingFlagsPaper ReportingPaperType, string Output = "PDF")
        {
            StringBuilder sb = new StringBuilder();

            foreach (string MachineID in MachineIDs)
            {
                //this piece is the SAME piece, that's used by Telerik Reporting to get the data
                //(BDO - DataSource)
                ReportingPaper rep = new ReportingPaper();
                List<ReportingPaperElements> items = rep.GetItems(MachineID, From, To, (int)ReportingPaperType);
                if (items.Count == 0)
                    continue;
                sb.AppendLine("Computer:       " + items[0].MachineName);
                sb.AppendLine("Version:        " + items[0].AgentVersion);
                sb.AppendLine("Last updated:   " + items[0].LastUpdated.ToShortDateString() + " " + items[0].LastUpdated.ToShortTimeString());
                sb.AppendLine("Boot:           " + items[0].BIOSBootType);
                sb.AppendLine("ID:             " + items[0].MachineID);
                sb.AppendLine("Total RAM:      " + items[0].TotalPhysicalMemory);
                sb.AppendLine("Make:           " + items[0].VendorMake);
                sb.AppendLine("BIOS:           " + items[0].VendorBIOS);
                sb.AppendLine("OS:             " + items[0].OS);
                sb.AppendLine("OS Ver:         " + items[0].OSVersion);
                sb.AppendLine("OS Rev:         " + items[0].OSWin10Version);
                sb.AppendLine("");
                sb.AppendLine("");
                foreach(ReportingPaperElements item in items)
                {
                    sb.AppendLine(" ===");
                    int IconDesc = (int)(item.Flags & (Int64)ReportingFlags.IconFlags) >> (int)ReportingFlags.IconFlagsShift;
                    string IconDescText = FoxSDC_Common.ReportingStatusPicture.GetPictureDescription((FoxSDC_Common.ReportingStatusPictureEnum)IconDesc);
                    sb.AppendLine(IconDescText);
                    sb.AppendLine(item.ReportedDate.ToShortDateString() + " " + item.ReportedDate.ToShortTimeString());
                    sb.AppendLine(item.Text);
                }
                sb.AppendLine("");
                sb.AppendLine("========================================================");
            }

            return (Encoding.UTF8.GetBytes(sb.ToString()));
        }
#endif
    }
}
