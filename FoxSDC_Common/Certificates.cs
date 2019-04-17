using FoxSDC_Common.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public enum InternalCertificate
    {
        Main,
        Licensing
    }

    public class Certificates
    {
        const int TimeStampPlusMinus = 10;
        public static string GetCN(byte[] CustomCert)
        {
            try
            {
                X509Certificate2 cert = new X509Certificate2(CustomCert);
                return (cert.Subject);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        public static byte[] GetCertificate(InternalCertificate CER)
        {
            switch (CER)
            {
                case InternalCertificate.Licensing:
                    return (Resources.Vulpes_Licensing);
                case InternalCertificate.Main:
                    return (Resources.Vulpes_Main);
            }
            return (null);
        }

        public static bool Verify(byte[] data, byte[] signature, byte[] CustomCert)
        {
            try
            {
                X509Certificate2 cert = new X509Certificate2(CustomCert);
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

                bool res = csp.VerifyData(data, new SHA1CryptoServiceProvider(), signature);

                return (res);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        public static bool Verify(Stream data, byte[] signature, byte[] CustomCert)
        {
            try
            {
                X509Certificate2 cert = new X509Certificate2(CustomCert);
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

                HashAlgorithm hash = new SHA1Managed();
                byte[] hdata = hash.ComputeHash(data);

                bool res = csp.VerifyHash(hdata, CryptoConfig.MapNameToOID("SHA1"), signature);

                return (res);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        public static bool Verify(Stream data, byte[] signature, InternalCertificate CER)
        {
            try
            {
                X509Certificate2 cert;
                switch (CER)
                {
                    case InternalCertificate.Main:
                        cert = new X509Certificate2(Resources.Vulpes_Main);
                        break;
                    case InternalCertificate.Licensing:
                        cert = new X509Certificate2(Resources.Vulpes_Licensing);
                        break;
                    default:
                        return (false);
                }
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

                HashAlgorithm hash = new SHA1Managed();
                byte[] hdata = hash.ComputeHash(data);

                bool res = csp.VerifyHash(hdata, CryptoConfig.MapNameToOID("SHA1"), signature);

                return (res);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        public static bool Verify(byte[] data, byte[] signature, X509Certificate2 cert)
        {
            try
            {
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

                bool res = csp.VerifyData(data, new SHA1CryptoServiceProvider(), signature);

                return (res);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        public static bool Verify(byte[] data, byte[] signature, InternalCertificate CER)
        {
            try
            {
                X509Certificate2 cert;
                switch (CER)
                {
                    case InternalCertificate.Main:
                        cert = new X509Certificate2(Resources.Vulpes_Main);
                        break;
                    case InternalCertificate.Licensing:
                        cert = new X509Certificate2(Resources.Vulpes_Licensing);
                        break;
                    default:
                        return (false);
                }
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

                bool res = csp.VerifyData(data, new SHA1CryptoServiceProvider(), signature);

                return (res);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }

        public static byte[] Sign(byte[] data, X509Certificate2 cert)
        {
            byte[] signature = null;

            try
            {
                RSACryptoServiceProvider csp = null;

                csp = (RSACryptoServiceProvider)cert.PrivateKey;

                if (csp == null)
                    return (new byte[] { });

                signature = csp.SignData(data, new SHA1CryptoServiceProvider());

                return (signature);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        public static bool CertificateExists(string CN, StoreLocation Location)
        {
            try
            {
                X509Store my = new X509Store(StoreName.My, Location);
                my.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 cert in my.Certificates)
                {
                    if (cert.Subject.Contains(CN))
                        return (true);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }


            return (false);
        }

        public static byte[] Sign(byte[] data, string CN, StoreLocation Location)
        {
            byte[] signature = null;

            try
            {
                X509Store my = new X509Store(StoreName.My, Location);
                my.Open(OpenFlags.ReadOnly);

                RSACryptoServiceProvider csp = null;
                foreach (X509Certificate2 cert in my.Certificates)
                {
                    if (cert.Subject.Contains(CN))
                    {
                        csp = (RSACryptoServiceProvider)cert.PrivateKey;
                        break;
                    }
                }

                if (csp == null)
                    return (new byte[] { });

                signature = csp.SignData(data, new SHA1CryptoServiceProvider());

                return (signature);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        public static List<string> GetCertificates(StoreLocation Location)
        {
            try
            {
                List<string> S = new List<string>();

                X509Store my = new X509Store(StoreName.My, Location);
                my.Open(OpenFlags.ReadOnly);
                foreach (X509Certificate2 cert in my.Certificates)
                {
                    S.Add(cert.Subject);
                }
                return (S);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        public static byte[] Sign(Stream data, string CN, StoreLocation location)
        {
            byte[] signature = null;

            try
            {
                X509Store my = new X509Store(StoreName.My, location);
                my.Open(OpenFlags.ReadOnly);

                RSACryptoServiceProvider csp = null;
                foreach (X509Certificate2 cert in my.Certificates)
                {
                    if (cert.Subject.Contains(CN))
                    {
                        csp = (RSACryptoServiceProvider)cert.PrivateKey;
                        break;
                    }
                }

                if (csp == null)
                    return (new byte[] { });

                signature = csp.SignData(data, new SHA1CryptoServiceProvider());

                return (signature);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }
        }

        public static bool Sign(PolicyObjectSigned policy, string CN)
        {
            if (policy == null)
                return (false);
            if (policy.Policy == null)
                return (false);
            policy.Policy.TimeStampCheck = DateTime.Now.ToUniversalTime();
            string JSON = JsonConvert.SerializeObject(policy.Policy, Formatting.None);
            policy.Signature = Sign(Encoding.UTF8.GetBytes(JSON), CN, StoreLocation.LocalMachine);
            if (policy.Signature == null)
                return (false);
            return (true);
        }

        public static bool Sign(NetInt64ListSigned intlist, string CN)
        {
            if (intlist == null)
                return (false);
            if (intlist.data == null)
                return (false);
            intlist.data.TimeStampCheck = DateTime.Now.ToUniversalTime();
            string JSON = JsonConvert.SerializeObject(intlist.data, Formatting.None);
            intlist.Signature = Sign(Encoding.UTF8.GetBytes(JSON), CN, StoreLocation.LocalMachine);
            if (intlist.Signature == null)
                return (false);
            return (true);
        }

        public static bool Sign(FileUploadDataSigned data, string CN)
        {
            if (data == null)
                return (false);
            if (data.Data == null)
                return (false);
            data.Data.TimeStampCheck = DateTime.Now.ToUniversalTime();
            string JSON = JsonConvert.SerializeObject(data.Data, Formatting.None);
            data.Signature = Sign(Encoding.UTF8.GetBytes(JSON), CN, StoreLocation.LocalMachine);
            if (data.Signature == null)
                return (false);
            return (true);
        }

        public static bool Sign(PushDataRoot pushdata, string CN)
        {
            if (pushdata == null)
                return (false);
            if (pushdata.Data == null)
                return (false);
            pushdata.Data.TimeStampCheck = DateTime.Now.ToUniversalTime();
            string JSON = JsonConvert.SerializeObject(pushdata.Data, Formatting.None);
            pushdata.Signature = Sign(Encoding.UTF8.GetBytes(JSON), CN, StoreLocation.LocalMachine);
            if (pushdata.Signature == null)
                return (false);
            return (true);
        }

        public static bool Verify(PolicyObjectSigned policy, byte[] cer)
        {
            if (policy == null)
                return (false);
            if (policy.Policy == null)
                return (false);
            if (policy.Policy.TimeStampCheck > DateTime.Now.ToUniversalTime().AddMinutes(TimeStampPlusMinus) &&
               policy.Policy.TimeStampCheck < DateTime.Now.ToUniversalTime().AddMinutes(-TimeStampPlusMinus))
            {
                Debug.WriteLine("Timestamp missmatch -+" + TimeStampPlusMinus.ToString() + " minutes");
                return (false);
            }

            string JSON = JsonConvert.SerializeObject(policy.Policy, Formatting.None);
            if (Verify(Encoding.UTF8.GetBytes(JSON), policy.Signature, cer) == false)
            {
                Debug.WriteLine("Verify failed");
                return (false);
            }

            return (true);
        }

        public static bool Verify(PushDataRoot pushdata, byte[] cer)
        {
            if (pushdata == null)
                return (false);
            if (pushdata.Data == null)
                return (false);
            if (pushdata.Data.TimeStampCheck > DateTime.Now.ToUniversalTime().AddMinutes(TimeStampPlusMinus) &&
               pushdata.Data.TimeStampCheck < DateTime.Now.ToUniversalTime().AddMinutes(-TimeStampPlusMinus))
            {
                Debug.WriteLine("Timestamp missmatch -+" + TimeStampPlusMinus.ToString() + " minutes");
                return (false);
            }

            string JSON = JsonConvert.SerializeObject(pushdata.Data, Formatting.None);
            if (Verify(Encoding.UTF8.GetBytes(JSON), pushdata.Signature, cer) == false)
            {
                Debug.WriteLine("Verify failed");
                return (false);
            }

            return (true);
        }

        public static bool Verify(PolicyObjectListSigned policylist, byte[] cer)
        {
            if (policylist == null)
                return (false);
            if (policylist.Items == null)
                return (false);
            if (policylist.TimeStampCheck > DateTime.Now.ToUniversalTime().AddMinutes(TimeStampPlusMinus) &&
                policylist.TimeStampCheck < DateTime.Now.ToUniversalTime().AddMinutes(-TimeStampPlusMinus))
            {
                Debug.WriteLine("Timestamp missmatch -+" + TimeStampPlusMinus.ToString() + " minutes");
                return (false);
            }

            string JSON = policylist.TimeStampCheck.ToString("s") + "-" + policylist.Items.ToString();
            if (Verify(Encoding.UTF8.GetBytes(JSON), policylist.Signature, cer) == false)
            {
                Debug.WriteLine("Verify failed");
                return (false);
            }

            return (true);
        }

        public static bool Verify(PackageDataSigned package, byte[] cer)
        {
            if (package == null)
                return (false);
            if (package.Package == null)
                return (false);
            if (package.TimeStampCheck > DateTime.Now.ToUniversalTime().AddMinutes(TimeStampPlusMinus) &&
                package.TimeStampCheck < DateTime.Now.ToUniversalTime().AddMinutes(-TimeStampPlusMinus))
            {
                Debug.WriteLine("Timestamp missmatch -+" + TimeStampPlusMinus.ToString() + " minutes");
                return (false);
            }

            string JSON = package.TimeStampCheck.ToString("s") + "-" + JsonConvert.SerializeObject(package.Package);
            if (Verify(Encoding.UTF8.GetBytes(JSON), package.Signature, cer) == false)
            {
                Debug.WriteLine("Verify failed");
                return (false);
            }

            return (true);
        }

        public static bool Verify(NetInt64ListSigned int64lst, byte[] cer)
        {
            if (int64lst == null)
                return (false);
            if (int64lst.data == null)
                return (false);
            if (int64lst.data.TimeStampCheck > DateTime.Now.ToUniversalTime().AddMinutes(TimeStampPlusMinus) &&
                int64lst.data.TimeStampCheck < DateTime.Now.ToUniversalTime().AddMinutes(-TimeStampPlusMinus))
            {
                Debug.WriteLine("Timestamp missmatch -+" + TimeStampPlusMinus.ToString() + " minutes");
                return (false);
            }
            string JSON = JsonConvert.SerializeObject(int64lst.data, Formatting.None);
            if (Verify(Encoding.UTF8.GetBytes(JSON), int64lst.Signature, cer) == false)
            {
                Debug.WriteLine("Verify failed");
                return (false);
            }
            return (true);
        }

        public static bool Verify(FileUploadDataSigned FUD, byte[] cer)
        {
            if (FUD == null)
                return (false);
            if (FUD.Data == null)
                return (false);
            if (FUD.Data.TimeStampCheck > DateTime.Now.ToUniversalTime().AddMinutes(TimeStampPlusMinus) &&
                FUD.Data.TimeStampCheck < DateTime.Now.ToUniversalTime().AddMinutes(-TimeStampPlusMinus))
            {
                Debug.WriteLine("Timestamp missmatch -+" + TimeStampPlusMinus.ToString() + " minutes");
                return (false);
            }
            string JSON = JsonConvert.SerializeObject(FUD.Data, Formatting.None);
            if (Verify(Encoding.UTF8.GetBytes(JSON), FUD.Signature, cer) == false)
            {
                Debug.WriteLine("Verify failed");
                return (false);
            }
            return (true);
        }

        public static bool Sign(PolicyObjectListSigned policylist, string CN)
        {
            if (policylist == null)
                return (false);
            if (policylist.Items == null)
                return (false);
            policylist.TimeStampCheck = DateTime.Now.ToUniversalTime();
            string JSON = policylist.TimeStampCheck.ToString("s") + "-" + policylist.Items.ToString();
            policylist.Signature = Sign(Encoding.UTF8.GetBytes(JSON), CN, StoreLocation.LocalMachine);
            if (policylist.Signature == null)
                return (false);
            return (true);
        }

        public static bool Sign(PackageDataSigned package, string CN)
        {
            if (package == null)
                return (false);
            if (package.Package == null)
                return (false);
            package.TimeStampCheck = DateTime.Now.ToUniversalTime();
            string JSON = package.TimeStampCheck.ToString("s") + "-" + JsonConvert.SerializeObject(package.Package);
            package.Signature = Sign(Encoding.UTF8.GetBytes(JSON), CN, StoreLocation.LocalMachine);
            if (package.Signature == null)
                return (false);
            return (true);
        }

        public static byte[] ExtractMainCER()
        {
            return (Resources.Vulpes_Main);
        }

        public static X509Certificate2 ExtractMainCERX509()
        {
            return (new X509Certificate2(Resources.Vulpes_Main));
        }

        public static string GetFox()
        {
            return (Resources.Vulpes);
        }
    }
}
