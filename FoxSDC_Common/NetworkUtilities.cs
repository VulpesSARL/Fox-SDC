using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class NetworkUtilities
    {
        public static bool PortAvailable(int Port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            bool isAvailable = true;

            foreach (IPEndPoint tcpi in tcpConnInfoArray)
            {
                if (tcpi.Port == Port)
                {
                    isAvailable = false;
                    break;
                }
            }
            return (isAvailable);
        }
    }
}
