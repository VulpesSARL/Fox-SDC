using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class WindowsPE
    {
        static bool? Isrunning;
        static public bool IsRunningInWindowsPE
        {
            get
            {
                if (Isrunning == null)
                    Isrunning = intIsRunningInWindowsPE();
                return (Isrunning.Value);
            }
        }

        static bool intIsRunningInWindowsPE()
        {
            using (RegistryKey minint = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\MiniNT"))
            {
                if (minint == null)
                    return (false);
            }
            using (RegistryKey minint = Registry.LocalMachine.OpenSubKey("Software\\Fox MiniNT5 Test"))
            {
                if (minint == null)
                    return (false);
                if (minint.GetValue("").ToString() != "Test")
                    return (false);
            }
            return (true);
        }
    }
}
