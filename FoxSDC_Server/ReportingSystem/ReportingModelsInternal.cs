using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class ReportingDiskData
    {
        public string Disk;
        public Int64 NotifyValue;
        public Int64 Is;
        public Int64 TotalSZ;
    }

    class ReportingAddRemovePrograms
    {
        public string Action;
        public AddRemoveApp App;
    }

    class ReportingSMART
    {
        public string Action;
        public VulpesSMARTInfo App;
        public List<int> UpdatedAttribs;
    }

    class ReportingStartup
    {
        public string Action;
        public StartupItem App;
    }

    class ReportingSimpleTaskCompletion
    {
        public string Action;
        public SimpleTaskResult App;
    }
}
