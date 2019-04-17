using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    public class ApplicationCertificate
    {
        static X509Certificate2 Cert = null;
        public static string CN
        {
            get
            {
                return ("Fox SDC Certificate for " + SystemInfos.SysInfo.ComputerName);
            }
        }

        static string CertFile
        {
            get
            {
                return (SystemInfos.ProgramData + "cert.pfx");
            }
        }

        public static string GetCN()
        {
            try
            {
                return (Cert.Subject);
            }
            catch
            {
                return (null);
            }
        }

        public static bool CNMatch()
        {
            if (GetCN() != "CN=" + CN)
                return (false);
            return (true);
        }

        public static bool LoadCertificate()
        {
            if (File.Exists(CertFile) == false)
                return (MakeOpenCert());

            return (OpenCert());
        }

        static bool TestCert()
        {
            byte[] res = Sign(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            return (res == null ? false : true);
        }

        static bool MakeOpenCert()
        {
            byte[] UnusedCERData;
            byte[] data = CertificateCreation.GenerateRootCertificate(CN, SystemInfos.SysInfo.UCID.ToUpper(), out UnusedCERData);
            if (data == null)
                return (false);
            try
            {
                File.WriteAllBytes(CertFile, data);
            }
            catch
            {
                return (false);
            }

            return (OpenCert());
        }

        static bool OpenCert(string Password, out Int64 HResult)
        {
            HResult = 0;

            try
            {
                Cert = new X509Certificate2(CertFile, Password);
                if (TestCert() == false)
                {
                    HResult = 0xD;
                    return (false);
                }
            }
            catch (Exception ee)
            {
                HResult = ee.HResult & 0xFFFFFFFF;
                return (false);
            }
            return (true);
        }

        static bool OpenCert()
        {
            Int64 HResult;
            if (OpenCert(SystemInfos.SysInfo.UCID.ToUpper(), out HResult) == false)
            {
                if (HResult == 0x80070056) //Wrong Password
                {
                    if (string.IsNullOrWhiteSpace(RegistryData.UCIDOverride) == true)
                    {
                        if (OpenCert(UCID.GetUCIDLegacy().ToUpper(), out HResult) == false)
                        {
                            return (false);
                        }
                        else
                        {
                            FoxEventLog.WriteEventLog("Using LEGACY UCID to load agent certificate.", System.Diagnostics.EventLogEntryType.Warning);
                        }
                    }
                    else
                    {
                        return (false);
                    }
                }
                else
                {
                    return (false);
                }
            }

            return (true);
        }

        public static bool Verify(byte[] data, byte[] signature)
        {
            if (Certificates.Verify(data, signature, Cert) == false)
                return (false);

            return (true);
        }

        public static bool Verify(PolicyObjectSigned data)
        {
            bool SignedOK = false;
            foreach (FilesystemCertificateData cer in FilesystemData.LoadedCertificates)
            {
                if (Certificates.Verify(data, cer.Certificate) == true)
                {
                    SignedOK = true;
                    break;
                }
            }

            return (SignedOK);
        }

        public static bool Verify(NetInt64ListSigned data)
        {
            bool SignedOK = false;
            foreach (FilesystemCertificateData cer in FilesystemData.LoadedCertificates)
            {
                if (Certificates.Verify(data, cer.Certificate) == true)
                {
                    SignedOK = true;
                    break;
                }
            }

            return (SignedOK);
        }

        public static bool Verify(FileUploadDataSigned data)
        {
            bool SignedOK = false;
            foreach (FilesystemCertificateData cer in FilesystemData.LoadedCertificates)
            {
                if (Certificates.Verify(data, cer.Certificate) == true)
                {
                    SignedOK = true;
                    break;
                }
            }

            return (SignedOK);
        }

        public static bool Verify(PushDataRoot data)
        {
            bool SignedOK = false;
            foreach (FilesystemCertificateData cer in FilesystemData.LoadedCertificates)
            {
                if (Certificates.Verify(data, cer.Certificate) == true)
                {
                    SignedOK = true;
                    break;
                }
            }

            return (SignedOK);
        }

        public static byte[] Sign(byte[] data)
        {
            return (Certificates.Sign(data, Cert));
        }
    }
}
