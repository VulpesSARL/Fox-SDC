using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class Win10Version
    {
        public static string GetWin10Version(string osversion)
        {
            string OSWin10Edition = "";
            string[] OSVersion = osversion.Split('.');//10.0.15063
            if (OSVersion.Length == 3)
            {
                int OSVersionMajor;
                if (int.TryParse(OSVersion[0], out OSVersionMajor) == true)
                {
                    if (OSVersionMajor == 10)
                    {
                        int OSVersionMinor;
                        if (int.TryParse(OSVersion[2], out OSVersionMinor) == true)
                        {
                            switch (OSVersionMinor)
                            {
                                case 10240: OSWin10Edition = "1507 (RTM)"; break;
                                case 10586: OSWin10Edition = "1511"; break;
                                case 14393: OSWin10Edition = "1607"; break;
                                case 15063: OSWin10Edition = "1703"; break;
                                case 16299: OSWin10Edition = "1709"; break;
                                case 17134: OSWin10Edition = "1803"; break;
                                case 17763: OSWin10Edition = "1809"; break;
                                default: OSWin10Edition = "??? " + OSVersionMinor.ToString(); break;
                            }
                        }
                    }
                }
            }
            return (OSWin10Edition);
        }
    }
}
