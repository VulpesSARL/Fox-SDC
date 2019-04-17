using FoxSDC_Common;
using FoxSDC_Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Fox_LicenseGenerator
{
    public class LicensingData
    {
        public string ProductID;
        public DateTime ValidFrom;
        public DateTime? ValidTo;
        public DateTime? SupportValidTo;
        public string LicenseID;
        public string LicenseType;
        public string Features;
        public string UCID;
        public string Owner;
        public string OwnerCustomID;

        public string Vacant1;
        public string Vacant2;
        public string Vacant3;
        public string Vacant4;
        public string Vacant5;
    }

    public class SDCLicensing
    {
        public static LicensingData Data = null;
        public static Int64? NumComputers = null;
        public static bool AllowContract = false;
        public static bool ValidLicense = false;

        public static void LoadLic()
        {
            //Modified for GitHub

            NumComputers = 1000;
            AllowContract = true;
            Data = new LicensingData();
            Data.Features = "";
            Data.LicenseID = Guid.NewGuid().ToString();
            Data.LicenseType = "GitHub Version";
            Data.Owner = "Fox";
            Data.OwnerCustomID = "";
            Data.SupportValidTo = null;
            Data.UCID = UCID.GetUCID();
            Data.ValidTo = null;
            Data.ValidFrom = DateTime.UtcNow.Date;
            Data.Vacant1 = "1000";
            ValidLicense = true;
        }

        static public bool TestExpiry()
        {
            if (Data == null)
                return (false);
            if (DateTime.UtcNow.Date < Data.ValidFrom)
                return (false);
            if (Data.ValidTo != null)
                if (DateTime.UtcNow.Date > Data.ValidTo.Value)
                    return (false);
            return (true);
        }
    }
}
