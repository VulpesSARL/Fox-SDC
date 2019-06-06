using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public enum ReportingPolicyType : int
    {
        None = 0,
        Disk = 1,
        EventLog = 2,
        AddRemovePrograms = 3,
        Startup = 4,
        SMART = 5,
        SimpleTaskCompleted = 6,

        //Special flags / purposes
        SMARTCritical = 255,
        //License = ?,
        //NotificationTimeout = ? //when a computer didn't report for example - a few days
    }

    public class ReportingPolicyElement
    {
        public ReportingPolicyType Type;
        public bool? ReportToAdmin;
        public bool? UrgentForAdmin;
        /// <summary>
        /// only in contract mode
        /// </summary>
        public bool? ReportToClient;
        /// <summary>
        /// only in contract mode
        /// </summary>
        public bool? UrgentForClient;
        public List<string> ReportingElements;
    }

    public class ReportingPolicyElementDisk
    {
        /// <summary>
        /// Single drive letter, or $ for the system drive
        /// </summary>
        public string DriveLetter;
        /// <summary>
        /// 0 = static size
        /// 1 = percent
        /// </summary>
        public int Method;
        public Int64 MinimumSize;
    }

    public class ReportingPolicyElementEventLog
    {
        /// <summary>
        /// Sources (in OR form)
        /// </summary>
        public List<string> Sources;
        /// <summary>
        /// Application, System or Security
        /// </summary>
        public List<string> Book;
        /// <summary>
        /// 0 = Information
        /// 1 = Error
        /// 2 = Warning
        /// 4 = Information
        /// 8 = Success Audit
        /// 16 = Failure Audit
        /// </summary>
        public List<int> EventLogTypes;
        /// <summary>
        /// EventID
        /// </summary>
        public List<int> CategoryNumbers;
    }

    public class ReportingPolicyElementAddRemovePrograms
    {
        public List<string> Names;
        /// <summary>
        /// 0 = exact ID
        /// 1 = Name (contains)
        /// 2 = Name (starts with)
        /// </summary>
        public int SearchNameIn;
        /// <summary>
        /// 0 = (any)
        /// 1 = 32 Bit
        /// 2 = 64 Bit
        /// </summary>
        public int SearchBits;
        public bool NotifyOnRemove;
        public bool NotifyOnAdd;
        public bool NotifyOnUpdate;
    }

    public class ReportingPolicyElementSMART
    {
        public bool NotifyOnError;
        public bool NotifyOnRemove;
        public bool NotifyOnAdd;
        public bool NotifyOnUpdate;
        public List<int> SkipAttribUpdateReport;
    }

    public class ReportingPolicyElementSimpleTaskCompleted
    {
        public bool Dummy;
    }

    public class ReportingPolicyElementStartup
    {
        public List<string> Names;
        /// <summary>
        /// 0 = exact Name
        /// 1 = Name (contains)
        /// 2 = Name (starts with)
        /// </summary>
        public int SearchNameIn;
        /// <summary>
        /// Semicolon separated
        /// </summary>
        public string SearchLocations;
        public bool NotifyOnRemove;
        public bool NotifyOnAdd;
        public bool NotifyOnUpdate;
    }

}
