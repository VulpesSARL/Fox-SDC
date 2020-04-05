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
    class NetRedirPWS
    {
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
            Res.ConnectedGUID = MainNetRedirWS.StartNetRedir(net, req.SessionID, req.Address, req.Port);

            return (Res);
        }

        public static PushConnectNetworkResult CloseConnection(string ReqString, Network net)
        {
            PushConnectNetworkResult Res = new PushConnectNetworkResult();

            bool res = MainNetRedirWS.CloseConnection(ReqString);
            if (res == true)
                Res.Result = 0;
            else
                Res.Result = 1;

            return (Res);
        }
    }
}
