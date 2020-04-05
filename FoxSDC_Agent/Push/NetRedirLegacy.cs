using FoxSDC_Agent.Redirs;
using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Push
{
    class NetRedirPLegacy
    {
        public static PushConnectNetworkData PullNetData(string ReqString, Network net)
        {
            PushConnectNetworkData Res = new PushConnectNetworkData();
            PushConnectNetworkData req;
            try
            {
                req = JsonConvert.DeserializeObject<PushConnectNetworkData>(ReqString);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                Res.Result = 0x16;
                return (Res);
            }

            return (MainNetRedirLegacy.ProcessDataRecv(req.GUID));
        }

        public static PushConnectNetworkResult StartNet(string ReqString, Network net)
        {
            PushConnectNetworkResult Res = new PushConnectNetworkResult();
            PushConnectNetwork req;
            try
            {
                req = JsonConvert.DeserializeObject<PushConnectNetwork>(ReqString);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                Res.Result = 0x16;
                return (Res);
            }

            Res.Result = 0;
            Res.ConnectedGUID = MainNetRedirLegacy.StartNetRedir(req.Address, req.Port, net);

            return (Res);
        }

        public static PushConnectNetworkResult SendData(string ReqString, Network net)
        {
            PushConnectNetworkResult Res = new PushConnectNetworkResult();

            bool res = MainNetRedirLegacy.ProcessData(ReqString);
            if (res == true)
                Res.Result = 0;
            else
                Res.Result = 1;

            return (Res);
        }

        public static PushConnectNetworkResult CloseConnection(string ReqString, Network net)
        {
            PushConnectNetworkResult Res = new PushConnectNetworkResult();

            bool res = MainNetRedirLegacy.CloseConnection(ReqString);
            if (res == true)
                Res.Result = 0;
            else
                Res.Result = 1;

            return (Res);
        }
    }
}
