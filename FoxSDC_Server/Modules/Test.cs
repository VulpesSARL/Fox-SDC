using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server.Modules
{
#if DEBUG
    class Test
    {
        [VulpesRESTfulRet("Test")]
        string TestTxt;

        [VulpesRESTful(VulpesRESTfulVerb.GET, "test.txt", "Test", "", false, true, true)]
        public RESTStatus TestFn(SQLLib sql, HttpListenerRequest request, HttpListenerResponse response, object dummy, NetworkConnectionInfo ni)
        {
            TestTxt = "Beim nächsten Toun ass et " + DateTime.Now.ToString("HH:mm") + " - Biip";
            byte[] buffer = Encoding.UTF8.GetBytes(TestTxt);

            response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
            response.OutputStream.Write(buffer, 0, buffer.Length);

            return (RESTStatus.Success);
        }
    }

#endif
}
