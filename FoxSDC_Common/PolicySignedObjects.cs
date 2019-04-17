using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    /*
     
     WARNING: this file is extremly sensitive, and even a slight change can break EVERYTHING!
     
     */

    public class PolicyObjectSigned
    {
        public PolicyObject Policy;
        public byte[] Signature;
    }

    public class PolicyObjectListSigned
    {
        public DateTime TimeStampCheck;
        public byte[] Signature;
        public List<PolicyObjectSigned> Items;
    }

    public class PolicyObject
    {
        public Int64 ID;
        public Int64 Order;
        public string Name;
        public string MachineID;
        public Int64? Grouping;
        public string Data;
        public DateTime DT;
        public Int64 Version;
        public bool Enabled;
        public int Type;
        public DateTime TimeStampCheck;
        public string Condition;
        //Future use
        public string DataAddtions1;
        public string DataAddtions2;
        public string DataAddtions3;
        public string DataAddtions4;
        public string DataAddtions5;
    }

    public class PackageData
    {
        public Int64 ID;
        public string PackageID;
        public Int64 Version;
        public string Title;
        public string Description;
        public Int64 Size;
    }

    public class PackageDataSigned
    {
        public DateTime TimeStampCheck;
        public byte[] Signature;
        public PackageData Package;
    }

}
