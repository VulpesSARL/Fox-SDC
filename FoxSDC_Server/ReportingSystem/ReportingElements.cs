using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    //Type: ReportingPolicyType
    [Flags]
    public enum ReportingFlags : Int64
    {
        ReportToAdmin = 0x1,
        UrgentForAdmin = 0x2,
        ReportToClient = 0x4,
        UrgentForClient = 0x8,
        ClientReported = 0x10,
        AdminReported = 0x20,
        UrgentClientReported = 0x40,
        UrgentAdminReported = 0x80,

        IconFlags = 0x7800000000000000L,
        IconFlagsShift = 59
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class ReportingExplainAttr : Attribute
    {
        public ReportingPolicyType Type;
        public ReportingExplainAttr(ReportingPolicyType Type)
        {
            this.Type = Type;
        }
    }

    interface IReportingExplain
    {
        Image GetIcon();
        string Explain(string JSON);
    }

    class ReportingProcessor
    {
        public static void ReportEventLog(SQLLib sql, string MachineID, EventLogReportFull EV, ReportingFlags Flags)
        {
            Flags &= ~(ReportingFlags.AdminReported | ReportingFlags.ClientReported | ReportingFlags.UrgentAdminReported | ReportingFlags.UrgentClientReported);

            sql.InsertMultiData("Reporting",
                new SQLData("MachineID", MachineID),
                new SQLData("Type", ReportingPolicyType.EventLog),
                new SQLData("Data", JsonConvert.SerializeObject(EV)),
                new SQLData("Flags", Flags));
        }

        public static void ReportDiskData(SQLLib sql, string MachineID, string Disk, Int64 NotifyValue, Int64 Is, Int64 TotalSZ, ReportingFlags Flags)
        {
            Flags &= ~(ReportingFlags.AdminReported | ReportingFlags.ClientReported | ReportingFlags.UrgentAdminReported | ReportingFlags.UrgentClientReported);

            ReportingDiskData rep = new ReportingDiskData();
            rep.Disk = Disk;
            rep.Is = Is;
            rep.NotifyValue = NotifyValue;
            rep.TotalSZ = TotalSZ;

            sql.InsertMultiData("Reporting",
                new SQLData("MachineID", MachineID),
                new SQLData("Type", ReportingPolicyType.Disk),
                new SQLData("Data", JsonConvert.SerializeObject(rep)),
                new SQLData("Flags", Flags));
        }

        public static void ReportAddRemoveApps(SQLLib sql, string MachineID, string Method, AddRemoveApp AR, ReportingFlags Flags)
        {
            Flags &= ~(ReportingFlags.AdminReported | ReportingFlags.ClientReported | ReportingFlags.UrgentAdminReported | ReportingFlags.UrgentClientReported);

            ReportingAddRemovePrograms a = new ReportingAddRemovePrograms();
            a.Action = Method;
            a.App = AR;

            sql.InsertMultiData("Reporting",
                new SQLData("MachineID", MachineID),
                new SQLData("Type", ReportingPolicyType.AddRemovePrograms),
                new SQLData("Data", JsonConvert.SerializeObject(a)),
                new SQLData("Flags", Flags));
        }

        public static void ReportSMART(SQLLib sql, string MachineID, string Method, ReportingSMART AR, ReportingFlags Flags, bool Critical)
        {
            Flags &= ~(ReportingFlags.AdminReported | ReportingFlags.ClientReported | ReportingFlags.UrgentAdminReported | ReportingFlags.UrgentClientReported);
            
            AR.Action = Method;

            sql.InsertMultiData("Reporting",
                new SQLData("MachineID", MachineID),
                new SQLData("Type", Critical == false ? ReportingPolicyType.SMART : ReportingPolicyType.SMARTCritical),
                new SQLData("Data", JsonConvert.SerializeObject(AR)),
                new SQLData("Flags", Flags));
        }

        public static void ReportStartup(SQLLib sql, string MachineID, string Method, StartupItem AR, ReportingFlags Flags)
        {
            Flags &= ~(ReportingFlags.AdminReported | ReportingFlags.ClientReported | ReportingFlags.UrgentAdminReported | ReportingFlags.UrgentClientReported);

            ReportingStartup a = new ReportingStartup();
            a.Action = Method;
            a.App = AR;

            sql.InsertMultiData("Reporting",
                new SQLData("MachineID", MachineID),
                new SQLData("Type", ReportingPolicyType.Startup),
                new SQLData("Data", JsonConvert.SerializeObject(a)),
                new SQLData("Flags", Flags));
        }

        public static void ReportSimpleTaskCompletion(SQLLib sql, string MachineID, string Method, SimpleTaskResult AR, ReportingFlags Flags)
        {
            Flags &= ~(ReportingFlags.AdminReported | ReportingFlags.ClientReported | ReportingFlags.UrgentAdminReported | ReportingFlags.UrgentClientReported);

            ReportingSimpleTaskCompletion a = new  ReportingSimpleTaskCompletion ();
            a.Action = Method;
            a.App = AR;

            sql.InsertMultiData("Reporting",
                new SQLData("MachineID", MachineID),
                new SQLData("Type", ReportingPolicyType.SimpleTaskCompleted),
                new SQLData("Data", JsonConvert.SerializeObject(a)),
                new SQLData("Flags", Flags));
        }

        static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(ReportingExplainAttr), true).Length > 0)
                {
                    yield return (type);
                }
            }
        }

        public static IReportingExplain FindExplainer(int ID)
        {
            IReportingExplain Explain = null;

            foreach (Type type in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
            {
                ReportingExplainAttr atttr = type.GetCustomAttribute<ReportingExplainAttr>();
                if (atttr.Type != (ReportingPolicyType)ID)
                    continue;
                Explain = (IReportingExplain)Activator.CreateInstance(type);
                break;
            }

            return (Explain);
        }

    }
}
