using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    public enum VulpesRESTfulVerb
    {
        POST,
        GET,
        PUT,
        PATCH,
        DELETE,
        HEAD
    }

    public enum RESTStatus
    {
        Success,
        Fail,
        NotFound,
        Created,
        NoContent,
        ServerError,
        Denied
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class VulpesRESTfulRet : Attribute
    {
        public string Name;
        public VulpesRESTfulRet(string Name)
        {
            this.Name = Name;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VulpesRESTProtected : Attribute
    {
        public VulpesRESTProtected()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class VulpesRESTful : Attribute
    {
        public VulpesRESTfulVerb Verb;
        public string URL;
        public bool RequireNi;
        public string ReturnValueName;
        public string GetValues;
        public bool UseLock;
        public bool RawDataProcessing;
        public bool PutIPAddress;
        public bool WithQueryString;
        public VulpesRESTful(VulpesRESTfulVerb verb, string URL, string ReturnValueName, string GetValues, bool RequireNi = true, bool UseLock = true, bool RawDataProcessing = false, bool PutIPAddress = false, bool WithQueryString = false)
        {
            this.Verb = verb;
            this.URL = URL;
            this.RequireNi = RequireNi;
            this.ReturnValueName = ReturnValueName;
            this.GetValues = GetValues;
            this.UseLock = UseLock;
            this.RawDataProcessing = RawDataProcessing;
            this.PutIPAddress = PutIPAddress;
            this.WithQueryString = WithQueryString;
        }
    }

    class RESTful
    {
        class RESTfulElement
        {
            public VulpesRESTfulVerb Verb;
            public string URL;
            public MethodInfo Method;
            public bool RequireNi;
            public string ReturnValueName;
            public string GetValues;
            public bool UseLock;
            public bool RawDataProcessing;
            public bool PutIPAddress;
            public bool WithQueryString;
        }

        static List<RESTfulElement> RESTfulElements;
        static List<RESTfulElement> RESTfulProtected;

        static IEnumerable<MethodInfo> GetTypesWithHelpAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    Debug.WriteLine(method.ToString());
                    if (method.GetCustomAttributes(typeof(VulpesRESTful), true).Length > 0)
                    {
                        yield return (method);
                    }
                }
            }
        }

        public static bool RegisterRESTfulClasses()
        {
            RESTfulElements = new List<RESTfulElement>();
            RESTfulProtected = new List<RESTfulElement>();

            foreach (MethodInfo method in GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly()))
            {
                VulpesRESTful vr = method.GetCustomAttribute<VulpesRESTful>();
                RESTfulElement re = new RESTfulElement();
                re.Method = method;
                re.URL = vr.URL;
                re.Verb = vr.Verb;
                re.RequireNi = vr.RequireNi;
                re.ReturnValueName = vr.ReturnValueName;
                re.GetValues = vr.GetValues;
                re.UseLock = vr.UseLock;
                re.RawDataProcessing = vr.RawDataProcessing;
                re.PutIPAddress = vr.PutIPAddress;
                re.WithQueryString = vr.WithQueryString;

                if (re.Method.ReturnType != typeof(RESTStatus))
                    throw new Exception("Return type != RESTStatus (here: " + method.ToString() + "  )");

                if (re.URL.StartsWith("/") == true || re.URL.EndsWith("/") == true)
                    throw new Exception("URL in VulpesRESTful must not start/end with '/' (here: " + method.ToString() + "  )");

                foreach (RESTfulElement ele in RESTfulElements)
                {
                    if (ele.GetValues == re.GetValues && ele.URL == re.URL && ele.Verb == re.Verb)
                        throw new Exception("Multiple registration for " + ele.Verb.ToString() + " " + ele.URL + " " + ele.GetValues);
                }

                RESTfulElements.Add(re);
                if (method.GetCustomAttribute<VulpesRESTProtected>() != null)
                    RESTfulProtected.Add(re);
            }

#if DEBUG
            foreach (RESTfulElement ele in RESTfulElements)
            {
                if (RESTfulProtected.Contains(ele) == true)
                    Console.Write("[P] ");
                Console.WriteLine(ele.Verb.ToString() + " " + ele.URL + " " + ele.GetValues);
            }
#endif

            return (true);
        }

        public static string GetClientIP(HttpListenerRequest Request)
        {
            string IP = Request.RemoteEndPoint.Address.ToString();
            IP += " \"" + DNS.ResolveIP(IP) + "\"";
            string RealClientIP = Request.Headers["X-Forwarded-For"];

            if (RealClientIP != null)
            {
                if (RealClientIP != "")
                {
                    IP += " (X-FWD: ";
                    bool FirstEntry = true;
                    foreach (string rcliip in RealClientIP.Split(','))
                    {
                        IP += (FirstEntry == true ? "" : ", ") + rcliip.Trim() + " \"" + DNS.ResolveIP(rcliip.Trim()) + "\"";
                        FirstEntry = true;
                    }
                    IP += ")";
                }
            }
            return (IP);
        }

        public static bool RunRESTful(HttpListenerRequest request, HttpListenerResponse response)
        {
            byte[] data = new byte[0];
            try
            {
                StreamReader read = new StreamReader(request.InputStream, Encoding.UTF8);
                string OriginalXML = read.ReadToEnd();

                VulpesRESTfulVerb verb;
                switch (request.HttpMethod.ToLower())
                {
                    case "get": verb = VulpesRESTfulVerb.GET; break;
                    case "put": verb = VulpesRESTfulVerb.PUT; break;
                    case "delete": verb = VulpesRESTfulVerb.DELETE; break;
                    case "head": verb = VulpesRESTfulVerb.HEAD; break;
                    case "post": verb = VulpesRESTfulVerb.POST; break;
                    case "patch": verb = VulpesRESTfulVerb.PATCH; break;
                    default:
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 501;
                        response.StatusDescription = "Not implemented";
                        //data = Encoding.UTF8.GetBytes("501 - Not implemented.");
                        return (true);
                }

                string URL = request.Url.LocalPath;
                Console.WriteLine(verb.ToString() + " " + URL);
                string CuttedParameters = "";
                if (URL.ToLower().StartsWith(Settings.Default.URLPart.ToLower()) == true)
                    URL = URL.Substring(Settings.Default.URLPart.Length, URL.Length - Settings.Default.URLPart.Length);
                string Auth = request.Headers["Authorization"];
                if (Auth == null)
                    Auth = "";
                Auth = Auth.ToLower();
                if (Auth.StartsWith("bearer ") == true)
                {
                    Auth = Auth.Substring(6, Auth.Length - 6).Trim();
                }
                else
                {
                    Auth = "";
                }

                string ClientIPAddress = GetClientIP(request);
                NetworkConnectionInfo ni = NetworkConnection.GetSession(Auth);
                if (ni != null)
                {
                    ni.IPAddress = ClientIPAddress;
                }
                RESTfulElement CurrentR = null;
                foreach (RESTfulElement r in RESTfulElements)
                {
                    if (URL.ToLower().StartsWith(r.URL.ToLower()) == true && r.Verb == verb)
                    {
                        CuttedParameters = URL.Substring(r.URL.Length, URL.Length - r.URL.Length);
                        if (CuttedParameters.StartsWith("/") == true && r.GetValues != "")
                            CuttedParameters = CuttedParameters.Substring(1, CuttedParameters.Length - 1);
                        else
                            if (CuttedParameters.StartsWith("/") == true && r.GetValues == "")
                            continue;
                        else
                                if (CuttedParameters == "" && r.GetValues != "")
                            continue;

                        if (r.RequireNi == true && ni == null)
                        {
                            response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                            response.StatusCode = 403;
                            response.StatusDescription = "Forbidden";
                            //data = Encoding.UTF8.GetBytes("403 - Forbidden.");
                            return (true);
                        }
                        else
                        {
                            CurrentR = r;
                            break;
                        }
                    }
                }

                if (CurrentR == null)
                {
                    response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                    response.StatusCode = 404;
                    response.StatusDescription = "Not found";
                    //data = Encoding.UTF8.GetBytes("404 - Not found.");
                    return (true);
                }

                if (RESTfulProtected.Contains(CurrentR) == true)
                {
                    if (string.IsNullOrWhiteSpace(SettingsManager.Settings.AdminIPAddresses) == false)
                    {
                        IPAddress ip = request.RemoteEndPoint.Address;
                        if (ip.ToString() != IPAddress.Loopback.ToString() && ip.ToString() != IPAddress.IPv6Loopback.ToString())
                        {
                            bool IPInRange = false;
                            foreach (string s in SettingsManager.Settings.AdminIPAddresses.Split(','))
                            {
                                if (s.Contains("/") == true)
                                {
                                    IPNetwork ipn;
                                    if (IPNetwork.TryParse(s, out ipn) == true)
                                    {
                                        IPNetwork ipnn = IPNetwork.Parse(ip, IPAddress.Broadcast);
                                        if (ipn.Overlap(ipnn) == true)
                                        {
                                            IPInRange = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (IPInRange == false)
                            {
                                response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                                response.StatusCode = 403;
                                response.StatusDescription = "Forbidden";
                                return (true);
                            }
                        }
                    }
                }

                ParameterInfo Param = CurrentR.Method.GetParameters()[1];

                List<object> Params = new List<object>();
                Params.Add(ni == null ? null : ni.sql);
                if (CurrentR.RawDataProcessing == true)
                {
                    Params.Add(request);
                    Params.Add(response);
                }
                Params.Add(JsonConvert.DeserializeObject(OriginalXML, Param.ParameterType));
                Params.Add(ni);
                if (CurrentR.PutIPAddress == true)
                {
                    Params.Add(ClientIPAddress);
                }
                if (CurrentR.WithQueryString == true)
                {
                    Params.Add(request.QueryString);
                }

                if (CurrentR.GetValues != "")
                {
                    ParameterInfo[] paramsi = CurrentR.Method.GetParameters();
                    if (paramsi.Length < 4)
                    {
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 404;
                        response.StatusDescription = "Not found";
                        //data = Encoding.UTF8.GetBytes("404 - Not found.");
                        return (true);
                    }

                    int CurrentParam = 3 + (CurrentR.RawDataProcessing == true ? 2 : 0);
                    string[] CuttedParametersList = CuttedParameters.Split('/');
                    string[] ParamsDef = CurrentR.GetValues.Split('/');

                    if (CuttedParametersList.Length != ParamsDef.Length)
                    {
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 404;
                        response.StatusDescription = "Not found";
                        //data = Encoding.UTF8.GetBytes("404 - Not found.");
                        return (true);
                    }

                    foreach (string g in ParamsDef)
                    {
                        if (paramsi.Length < CurrentParam + 1)
                        {
                            response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                            response.StatusCode = 404;
                            response.StatusDescription = "Not found";
                            //data = Encoding.UTF8.GetBytes("404 - Not found.");
                            return (true);
                        }
                        if (g != paramsi[CurrentParam].Name)
                        {
                            response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                            response.StatusCode = 500;
                            response.StatusDescription = "Server Error";
#if DEBUG
                            data = Encoding.UTF8.GetBytes("500 - Server Error. Bad param definition");
#else
                            //data = Encoding.UTF8.GetBytes("500 - Server Error.");
#endif
                            return (true);
                        }
                        try
                        {
                            Params.Add(Convert.ChangeType(CuttedParametersList[CurrentParam - (CurrentR.RawDataProcessing == true ? 5 : 3)], paramsi[CurrentParam].ParameterType));
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());
                            response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                            response.StatusCode = 404;
                            response.StatusDescription = "Not found";
#if DEBUG
                            data = Encoding.UTF8.GetBytes("404 - Not found. " + ee.ToString());
#else
                            //data = Encoding.UTF8.GetBytes("404 - Not found.");
#endif
                            return (true);
                        }
                        CurrentParam++;
                    }
                }

                object instance = Activator.CreateInstance(CurrentR.Method.DeclaringType);
                RESTStatus res;
                if (ni == null)
                {
                    res = (RESTStatus)(CurrentR.Method.Invoke(instance, Params.ToArray()));
                }
                else
                {
                    if (CurrentR.UseLock == false)
                    {
                        res = (RESTStatus)(CurrentR.Method.Invoke(instance, Params.ToArray()));
                    }
                    else
                    {
                        try
                        {
                            ni.RWLock.EnterReadLock();
                            res = (RESTStatus)(CurrentR.Method.Invoke(instance, Params.ToArray()));
                        }
                        catch(Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());
                            throw;
                        }
                        finally
                        {
                            ni.RWLock.ExitReadLock();
                        }
                    }
                }
                if (CurrentR.RawDataProcessing == true)
                {
                    data = new byte[0];
                    return (true);
                }

                switch (res)
                {
                    case RESTStatus.Created:
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 201;
                        response.StatusDescription = "Created";
                        if (CurrentR.ReturnValueName != "")
                        {
                            foreach (FieldInfo f in CurrentR.Method.DeclaringType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                            {
                                VulpesRESTfulRet ret = f.GetCustomAttribute<VulpesRESTfulRet>();
                                if (ret != null)
                                {
                                    if (ret.Name == CurrentR.ReturnValueName)
                                    {
                                        data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(f.GetValue(instance)));
                                        break;
                                    }
                                }
                            }
                        }
                        return (true);
                    case RESTStatus.Fail:
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 490;
                        response.StatusDescription = "Did not work";
                        if (CurrentR.ReturnValueName != "")
                        {
                            foreach (FieldInfo f in CurrentR.Method.DeclaringType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                            {
                                VulpesRESTfulRet ret = f.GetCustomAttribute<VulpesRESTfulRet>();
                                if (ret != null)
                                {
                                    if (ret.Name == CurrentR.ReturnValueName)
                                    {
                                        data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(f.GetValue(instance)));
                                        break;
                                    }
                                }
                            }
                        }
                        //if (data.Length == 0)
                        //    data = Encoding.UTF8.GetBytes("490 - Did not work.");
                        return (true);
                    case RESTStatus.NoContent:
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 204;
                        response.StatusDescription = "No Content";
                        return (true);
                    case RESTStatus.NotFound:
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 404;
                        response.StatusDescription = "Not found";
                        //data = Encoding.UTF8.GetBytes("404 - Not found.");
                        return (true);
                    case RESTStatus.ServerError:
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 500;
                        response.StatusDescription = "Server Error";
                        //data = Encoding.UTF8.GetBytes("500 - Server Error.");
                        return (true);
                    case RESTStatus.Denied:
                        response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                        response.StatusCode = 403;
                        response.StatusDescription = "Forbidden";
                        //data = Encoding.UTF8.GetBytes("403 - Forbidden.");
                        return (true);
                    case RESTStatus.Success:
                        if (CurrentR.ReturnValueName != "")
                        {
                            foreach (FieldInfo f in CurrentR.Method.DeclaringType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                            {
                                VulpesRESTfulRet ret = f.GetCustomAttribute<VulpesRESTfulRet>();
                                if (ret != null)
                                {
                                    if (ret.Name == CurrentR.ReturnValueName)
                                    {
                                        data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(f.GetValue(instance)));
                                        break;
                                    }
                                }
                            }
                        }
                        return (true);
                }

            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                try
                {
                    response.AddHeader("Content-Type", "text/plain; charset=UTF-8");
                    response.StatusCode = 500;
                    response.StatusDescription = "Server Error";
#if DEBUG
                    data = Encoding.UTF8.GetBytes("500 - Server Error. " + ee.ToString());
#else
                    //data = Encoding.UTF8.GetBytes("500 - Server Error.");
#endif
                }
                catch (Exception eee)
                {
                    Debug.WriteLine(eee.ToString());
                }
                return (true);
            }
            finally
            {
                if (data.Length > 0)
                {
                    response.ContentLength64 = data.LongLength;
                    Stream output = response.OutputStream;
                    output.Write(data, 0, data.Length);
                }
            }

            return (true);
        }
    }
}
