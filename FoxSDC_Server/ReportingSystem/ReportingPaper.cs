using FoxSDC_Common;
using FoxSDC_Server.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    public enum ReportingFlagsPaper
    {
        UrgentAdmin = 1,
        UrgentClient = 2,
        ReportAdmin = 3,
        ReportClient = 4,
        ReReport = 5
    }

    public class ReportingPaperElements
    {
        public Int64 ID { get; set; }
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public string AgentVersion { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string OSWin10Version { get; set; }
        public string VendorMake { get; set; }
        public string VendorBIOS { get; set; }
        public string BIOSBootType { get; set; }
        public string UCID { get; set; }
        public string TotalPhysicalMemory { get; set; }
        public DateTime LastUpdated { get; set; }

        public Image IconPicture { get; set; }
        public Image StatusPicture { get; set; }

        public DateTime ReportedDate { get; set; }
        public string Text { get; set; }
        public int ReportingType { get; set; }
        public Int64 Flags { get; set; }
        public string ContractID { get; set; }
    }

    [System.ComponentModel.DataObject]
    public class ReportingPaper : List<ReportingPaperElements>
    {
        void CopyBaseData(ReportingPaperElements From, ReportingPaperElements To)
        {
            To.AgentVersion = From.AgentVersion;
            To.BIOSBootType = From.BIOSBootType;
            To.LastUpdated = From.LastUpdated;
            To.MachineID = From.MachineID;
            To.MachineName = From.MachineName;
            To.OS = From.OS;
            To.OSVersion = From.OSVersion;
            To.OSWin10Version = From.OSWin10Version;
            To.TotalPhysicalMemory = From.TotalPhysicalMemory;
            To.UCID = From.UCID;
            To.VendorBIOS = From.VendorBIOS;
            To.VendorMake = From.VendorMake;
            To.ContractID = From.ContractID;
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        public List<ReportingPaperElements> GetItems(string MachineID, DateTime? From, DateTime? To, int ReportingPaper)
        {
            List<ReportingPaperElements> lst = new List<ReportingPaperElements>();

            if (string.IsNullOrWhiteSpace(MachineID) == true)
            {
                ReportingPaperElements funnybasicinfo = new ReportingPaperElements();
                funnybasicinfo.AgentVersion = "Fox SDC 950228180442";
                funnybasicinfo.BIOSBootType = "Legacy";
                funnybasicinfo.LastUpdated = new DateTime(1995, 3, 5, 19, 35, 21);
                funnybasicinfo.MachineID = "8878FF7A-2506-4A63-B0F1-A0C28AFD6480";
                funnybasicinfo.MachineName = "FOX-PC1-NT3";
                funnybasicinfo.OS = "Windows NT 3.51 Service Pack 5";
                funnybasicinfo.OSVersion = "3.51.1057";
                funnybasicinfo.OSWin10Version = "";
                funnybasicinfo.TotalPhysicalMemory = CommonUtilities.NiceSize(67108864);
                funnybasicinfo.UCID = "F3489E4C92654D12A6EF6C4C2EAA4A5E";
                funnybasicinfo.VendorBIOS = "Award BIOS 6";
                funnybasicinfo.VendorMake = "Vulpes 486 PC";
                funnybasicinfo.ContractID = "Company XXX Contract";

                lst.Add(new ReportingPaperElements());
                CopyBaseData(funnybasicinfo, lst[0]);
                lst[0].ID = 10005;
                lst[0].ReportedDate = new DateTime(1995, 3, 5, 18, 20, 54);
                lst[0].IconPicture = Resources.Vulpes.ToBitmap();
                lst[0].StatusPicture = ReportingStatusPicture.GetPicture(ReportingStatusPictureEnum.Good);
                lst[0].Text = "Reporting test ...\nNew line\n\nAnother line";

                lst.Add(new ReportingPaperElements());
                CopyBaseData(funnybasicinfo, lst[1]);
                lst[1].ID = 10004;
                lst[1].ReportedDate = new DateTime(1995, 3, 5, 18, 19, 12);
                lst[1].IconPicture = Resources.EventLog.ToBitmap();
                lst[1].StatusPicture = ReportingStatusPicture.GetPicture(ReportingStatusPictureEnum.Info);
                lst[1].Text = "The Event log service was started.";

                lst.Add(new ReportingPaperElements());
                CopyBaseData(funnybasicinfo, lst[2]);
                lst[2].ID = 10003;
                lst[2].ReportedDate = new DateTime(1995, 3, 5, 18, 15, 35);
                lst[2].IconPicture = Resources.EventLog.ToBitmap();
                lst[2].StatusPicture = ReportingStatusPicture.GetPicture(ReportingStatusPictureEnum.Stop);
                lst[2].Text = "The system has rebooted without cleanly shutting down first. This error could be caused if the system stopped responding, crashed, or lost power unexpectedly.";

                return (lst);
            }

            Settings.Default.Load();

            SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for Reporting");
            if (sql == null)
                return (lst);

            //Get some ComputerInfo
            SqlDataReader dr = sql.ExecSQLReader("select * from ComputerAccounts Where MachineID=@m",
                new SQLParam("@m", MachineID));
            if (dr.HasRows == false)
            {
                dr.Close();
                sql.CloseConnection();
                return (lst);
            }

            dr.Read();
            ReportingPaperElements basicinfo = new ReportingPaperElements();
            basicinfo.AgentVersion = Convert.ToString(dr["AgentVersion"]);
            basicinfo.BIOSBootType = Convert.ToString(dr["BIOSType"]);
            basicinfo.LastUpdated = SQLLib.GetDTUTC(dr["LastUpdated"]);
            basicinfo.MachineID = Convert.ToString(dr["MachineID"]);
            basicinfo.MachineName = Convert.ToString(dr["ComputerName"]);
            basicinfo.OS = Convert.ToString(dr["OSName"]);
            basicinfo.OSVersion = Convert.ToString(dr["OSVerMaj"]) + "." + Convert.ToString(dr["OSVerMin"]) + "." + Convert.ToString(dr["OSVerBuild"]);
            basicinfo.OSWin10Version = Win10Version.GetWin10Version(basicinfo.OSVersion);
            basicinfo.TotalPhysicalMemory = CommonUtilities.NiceSize(Convert.ToInt64(dr["TotalPhysicalMemory"]));
            basicinfo.UCID = Convert.ToString(dr["UCID"]);
            basicinfo.VendorBIOS = Convert.ToString(dr["BIOS"]);
            basicinfo.VendorMake = Convert.ToString(dr["ComputerModel"]);
            basicinfo.ContractID = Convert.ToString(dr["ContractID"]);
            dr.Close();

            string Query = "";
            List<SQLParam> SQLParams = new List<SQLParam>();
            switch ((ReportingFlagsPaper)ReportingPaper)
            {
                case ReportingFlagsPaper.UrgentAdmin:
                    Query = "Select * from Reporting where machineid=@mid and (Flags & @f1)!=0 AND (Flags & @f2)=0 order by Reported desc";
                    SQLParams.Add(new SQLParam("@mid", MachineID));
                    SQLParams.Add(new SQLParam("@f1", ReportingFlags.UrgentForAdmin));
                    SQLParams.Add(new SQLParam("@f2", ReportingFlags.UrgentAdminReported));
                    break;
                case ReportingFlagsPaper.UrgentClient:
                    Query = "Select * from Reporting where machineid=@mid and (Flags & @f1)!=0 AND (Flags & @f2)=0 order by Reported desc";
                    SQLParams.Add(new SQLParam("@mid", MachineID));
                    SQLParams.Add(new SQLParam("@f1", ReportingFlags.UrgentForClient));
                    SQLParams.Add(new SQLParam("@f2", ReportingFlags.UrgentClientReported));
                    break;
                case ReportingFlagsPaper.ReportAdmin:
                    Query = "Select * from Reporting where machineid=@mid and (Flags & @f1)!=0 AND (Flags & @f2)=0 order by Reported desc";
                    SQLParams.Add(new SQLParam("@mid", MachineID));
                    SQLParams.Add(new SQLParam("@f1", ReportingFlags.ReportToAdmin));
                    SQLParams.Add(new SQLParam("@f2", ReportingFlags.AdminReported));
                    break;
                case ReportingFlagsPaper.ReportClient:
                    Query = "Select * from Reporting where machineid=@mid and (Flags & @f1)!=0 AND (Flags & @f2)=0 order by Reported desc";
                    SQLParams.Add(new SQLParam("@mid", MachineID));
                    SQLParams.Add(new SQLParam("@f1", ReportingFlags.ReportToClient));
                    SQLParams.Add(new SQLParam("@f2", ReportingFlags.ClientReported));
                    break;
                case ReportingFlagsPaper.ReReport:
                    if (From == null && To == null)
                    {
                        Query = "Select * from Reporting where machineid=@mid order by Reported desc";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                    }
                    if (From != null && To == null)
                    {
                        Query = "Select * from Reporting where machineid=@mid AND Reported>=@d1 order by Reported desc";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                        SQLParams.Add(new SQLParam("@d1", From.Value));
                    }
                    if (From == null && To != null)
                    {
                        Query = "Select * from Reporting where machineid=@mid AND Reported<=@d1 order by Reported desc";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                        SQLParams.Add(new SQLParam("@d1", To.Value));
                    }
                    if (From != null && To != null)
                    {
                        Query = "Select * from Reporting where machineid=@mid and Reported between @d1 and @d2 order by Reported desc";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                        SQLParams.Add(new SQLParam("@d1", From.Value));
                        SQLParams.Add(new SQLParam("@d2", To.Value));
                    }
                    break;
                default:
                    return (lst);
            }

            List<Int64> ReportedIDs = new List<long>();

            dr = sql.ExecSQLReader(Query, SQLParams.ToArray());
            while (dr.Read())
            {
                ReportingPaperElements r = new ReportingPaperElements();
                CopyBaseData(basicinfo, r);
                ReportedIDs.Add(Convert.ToInt64(dr["ID"]));
                r.ID = Convert.ToInt64(dr["ID"]);
                r.Flags = Convert.ToInt64(dr["Flags"]);
                r.ReportingType = Convert.ToInt32(dr["Type"]);
                r.ReportedDate = SQLLib.GetDTUTC(dr["Reported"]);
                r.StatusPicture = ReportingStatusPicture.GetPicture((int)((r.Flags & (Int64)ReportingFlags.IconFlags) >> (int)ReportingFlags.IconFlagsShift));
                IReportingExplain explain = ReportingProcessor.FindExplainer(r.ReportingType);
                if (explain != null)
                {
                    r.Text = explain.Explain(Convert.ToString(dr["Data"]));
                    r.IconPicture = explain.GetIcon();
                }
                else
                {
                    r.Text = "Missing module for Type=" + r.ReportingType.ToString() + "; Text=" + Convert.ToString(dr["Data"]);
                    r.IconPicture = Resources.Nix.ToBitmap();
                }
                lst.Add(r);
            }
            dr.Close();

            if (ReportedIDs.Count > 0 && (ReportingFlagsPaper)ReportingPaper != ReportingFlagsPaper.ReReport)
            {
                string SQLIn = "";
                foreach (Int64 i in ReportedIDs)
                {
                    SQLIn += i.ToString() + ",";
                }

                if (SQLIn.EndsWith(",") == true)
                    SQLIn = SQLIn.Substring(0, SQLIn.Length - 1);

                Query = "";
                SQLParams = new List<SQLParam>();

                switch ((ReportingFlagsPaper)ReportingPaper)
                {
                    case ReportingFlagsPaper.UrgentAdmin:
                        Query = "update Reporting set Flags = Flags | @f2 where machineid=@mid and Flags & @f1!=0 AND Flags & @f2=0";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                        SQLParams.Add(new SQLParam("@f1", ReportingFlags.UrgentForAdmin));
                        SQLParams.Add(new SQLParam("@f2", ReportingFlags.UrgentAdminReported));
                        break;
                    case ReportingFlagsPaper.UrgentClient:
                        Query = "update Reporting set Flags = Flags | @f2 where machineid=@mid and Flags & @f1!=0 AND Flags & @f2=0";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                        SQLParams.Add(new SQLParam("@f1", ReportingFlags.UrgentForClient));
                        SQLParams.Add(new SQLParam("@f2", ReportingFlags.UrgentClientReported));
                        break;
                    case ReportingFlagsPaper.ReportAdmin:
                        Query = "update Reporting set Flags = Flags | @f2 where machineid=@mid and Flags & @f1!=0 AND Flags & @f2=0";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                        SQLParams.Add(new SQLParam("@f1", ReportingFlags.ReportToAdmin));
                        SQLParams.Add(new SQLParam("@f2", ReportingFlags.AdminReported));
                        break;
                    case ReportingFlagsPaper.ReportClient:
                        Query = "update Reporting set Flags = Flags | @f2 where machineid=@mid and Flags & @f1!=0 AND Flags & @f2=0";
                        SQLParams.Add(new SQLParam("@mid", MachineID));
                        SQLParams.Add(new SQLParam("@f1", ReportingFlags.ReportToClient));
                        SQLParams.Add(new SQLParam("@f2", ReportingFlags.ClientReported));
                        break;
                }

                sql.ExecSQLNQ(Query, SQLParams.ToArray());
            }
            return (lst);
        }
    }
}
