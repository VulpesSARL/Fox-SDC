using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class WebServerHandler
    {
        static WebServer http;

        static void httpCallback(HttpListenerRequest request, HttpListenerResponse response)
        {
            Debug.WriteLine("REQ: " + request.HttpMethod + " " + request.RawUrl);
            RESTful.RunRESTful(request, response);
        }

        public static void RunWebServer()
        {
            http = new WebServer(Settings.Default.ListenOn.Split('|'), httpCallback);
            http.Run();
        }

        public static void EndWebServer()
        {
            http.Stop();
        }
    }
}
