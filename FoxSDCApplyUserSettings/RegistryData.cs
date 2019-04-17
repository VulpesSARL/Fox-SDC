using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDCApplyUserSettings
{
    class RegistryData
    {
        static public Int64 Verbose
        {
            get
            {
                try
                {
                    RegistryKey k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Fox\\SDC");
                    if (k == null)
                        return (0);
                    int i;
                    if (int.TryParse(k.GetValue("Verbose", "0").ToString(), out i) == false)
                    {
                        k.Close();
                        return (0);
                    }
                    k.Close();
                    return (i);
                }
                catch
                {
                    return (0);
                }
            }
        }
    }
}
