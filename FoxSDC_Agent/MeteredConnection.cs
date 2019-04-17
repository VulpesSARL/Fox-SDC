using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace FoxSDC_Agent
{
    class MeteredConnection
    {
        static public bool? IsMeteredConnection()
        {
            try
            {
                ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (internetConnectionProfile == null)
                    return (null);
                if (internetConnectionProfile.GetConnectionCost().NetworkCostType == NetworkCostType.Unknown
                    || internetConnectionProfile.GetConnectionCost().NetworkCostType == NetworkCostType.Unrestricted)
                {
                    return (false);
                }
                else
                {
                    return (true);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }
    }
}
