using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class SmartCards
    {
        public IntPtr ParentWindowHandle = IntPtr.Zero;
        public string LastError = null;

        static public List<string> GetCSPProviders()
        {
            List<string> lst = new List<string>();

            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography\\Defaults\\Provider", false);
            if (key == null)
                return (null);
            foreach (string s in key.GetSubKeyNames())
                lst.Add(s);
            key.Close();
            return (lst);
        }

        public byte[] SignData(string Provider, Stream data, SecureString pin)
        {
            try
            {
                if (Provider == null)
                {
                    LastError = "No Provider";
                    return (null);
                }
                if (Provider.Trim() == "")
                {
                    LastError = "No Provider";
                    return (null);
                }
                if (data == null)
                {
                    LastError = "No Data";
                    return (null);
                }
                CspParameters csp = new CspParameters(1, Provider);
                csp.ParentWindowHandle = ParentWindowHandle;
                csp.Flags = CspProviderFlags.UseDefaultKeyContainer;
                if (pin != null)
                    csp.KeyPassword = pin;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                byte[] sig = rsa.SignData(data, new SHA1CryptoServiceProvider());
                rsa.Clear();
                return (sig);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                LastError = ee.Message;
                return (null);
            }
        }

        public byte[] SignData(string Provider, byte[] data, SecureString pin)
        {
            try
            {
                if (Provider == null)
                {
                    LastError = "No Provider";
                    return (null);
                }
                if (Provider.Trim() == "")
                {
                    LastError = "No Provider";
                    return (null);
                }
                if (data == null)
                {
                    LastError = "No Data";
                    return (null);
                }
                CspParameters csp = new CspParameters(1, Provider);
                csp.ParentWindowHandle = ParentWindowHandle;
                csp.Flags = CspProviderFlags.UseDefaultKeyContainer;
                csp.KeyPassword = pin;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                byte[] sig = rsa.SignData(data, new SHA1CryptoServiceProvider());
                rsa.Clear();
                return (sig);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                LastError = ee.Message;
                return (null);
            }
        }

        public bool VerifyData(string Provider, byte[] data, byte[] sign)
        {
            try
            {
                if (data == null)
                {
                    LastError = "No Data";
                    return (false);
                }
                if (sign == null)
                {
                    LastError = "No Signature";
                    return (false);
                }
                CspParameters csp = new CspParameters(1, Provider);
                csp.Flags = CspProviderFlags.UseDefaultKeyContainer;
                csp.ParentWindowHandle = ParentWindowHandle;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                bool res = rsa.VerifyData(data, new SHA1CryptoServiceProvider(), sign);
                rsa.Clear();
                return (res);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                LastError = ee.Message;
                return (false);
            }
        }
    }
}
