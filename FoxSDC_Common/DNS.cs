using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class DNS
    {
        [DllImport("Dnsapi.dll", EntryPoint = "DnsQuery_W", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        static extern Int32 DnsQuery(String lpstrName, Int16 wType, Int32 options, IntPtr pExtra, ref IntPtr ppQueryResultsSet, IntPtr pReserved);

        [DllImport("Dnsapi.dll", SetLastError = true)]
        static extern void DnsRecordListFree(IntPtr pRecordList, Int32 freeType);

        [StructLayout(LayoutKind.Sequential)]
        struct DnsRecordPtr
        {
            public IntPtr pNext;
            public String pName;
            public Int16 wType;
            public Int16 wDataLength;
            public Int32 flags;
            public Int32 dwTtl;
            public Int32 dwReserved;
            public IntPtr pNameHost;
        }

        public static string ResolveIP(string IP)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(IP);
                return (ReverseIPLookup(ip));
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return ("");
            }
        }
        static String ReverseIPLookup(IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                string domain = String.Join(".", ipAddress.GetAddressBytes().Reverse().Select(b => b.ToString())) + ".in-addr.arpa";
                return (DnsGetPtrRecord(domain));
            }
            if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                string domain = String.Join(".", ipAddress.GetAddressBytes().Reverse().Select(b => b.ToString("x2")));
                string domain2 = "";
                foreach (string d in domain.Split('.'))
                {
                    if (d.Length == 1)
                        domain2 += d + ".";
                    else
                        domain2 += d.Substring(1, 1) + "." + d.Substring(0, 1) + ".";
                }
                domain2 += "ip6.arpa";
                return (DnsGetPtrRecord(domain2));
            }

            return (ipAddress.ToString());
        }

        static String DnsGetPtrRecord(String domain)
        {
            const Int16 DNS_TYPE_PTR = 0x000C;
            const Int32 DNS_QUERY_STANDARD = 0x00000000;
            const Int32 DNS_ERROR_RCODE_NAME_ERROR = 9003;
            IntPtr queryResultSet = IntPtr.Zero;
            try
            {
                int dnsStatus = DnsQuery(
                  domain,
                  DNS_TYPE_PTR,
                  DNS_QUERY_STANDARD,
                  IntPtr.Zero,
                  ref queryResultSet,
                  IntPtr.Zero
                );
                if (dnsStatus == DNS_ERROR_RCODE_NAME_ERROR)
                    return (null);
                if (dnsStatus != 0)
                    throw new Win32Exception(dnsStatus);
                DnsRecordPtr dnsRecordPtr;
                for (IntPtr pointer = queryResultSet; pointer != IntPtr.Zero; pointer = dnsRecordPtr.pNext)
                {
                    dnsRecordPtr = (DnsRecordPtr)Marshal.PtrToStructure(pointer, typeof(DnsRecordPtr));
                    if (dnsRecordPtr.wType == DNS_TYPE_PTR)
                        return Marshal.PtrToStringUni(dnsRecordPtr.pNameHost);
                }
                return (null);
            }
            finally
            {
                const Int32 DnsFreeRecordList = 1;
                if (queryResultSet != IntPtr.Zero)
                    DnsRecordListFree(queryResultSet, DnsFreeRecordList);
            }
        }
    }
}
