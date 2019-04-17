using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class Utilities
    {
        public static string FilterOutBadChars(string Input)
        {
            if (Input == null)
                return (null);
            string Temp = Input.Trim();
            Temp = Temp.Replace("\\", "");
            Temp = Temp.Replace("%", "");
            Temp = Temp.Replace("\\", "");
            Temp = Temp.Replace("$", "");
            //Temp = Temp.Replace("/", "");
            Temp = Temp.Trim();
            return (Temp);
        }

        public static bool ContainsInvalidCharacters(string Input)
        {
            if (Input.Contains("\\") == true) return (true);
            if (Input.Contains("*") == true) return (true);
            if (Input.Contains("?") == true) return (true);
            if (Input.Contains("%") == true) return (true);
            if (Input.Contains("/") == true) return (true);
            if (Input.Contains("$") == true) return (true);
            if (Input.Contains("|") == true) return (true);
            if (Input.Contains("<") == true) return (true);
            if (Input.Contains(">") == true) return (true);
            return (false);
        }

        public static string FilterOutBadCharsKeepNumbersOnly(string Input)
        {
            string Temp = "";
            if (Input.Trim() == "")
                return ("");
            for (int i = 0; i < Input.Length; i++)
                if (Input[i] >= 0x30 && Input[i] <= 0x39)
                    Temp += Input[i];
            return (Temp);
        }

        public static bool IsNumeric(string s)
        {
            foreach (char c in s)
                if (char.IsNumber(c) == false)
                    return (false);
            return (true);
        }

        public static bool TestSign(out string ErrorText)
        {
            try
            {
                ErrorText = "";

                if (SettingsManager.Settings.UseCertificate == "")
                {
                    ErrorText = "No certificate specified";
                    return (false);
                }

                RSACryptoServiceProvider csp = null;
                X509Store my = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                my.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 cert in my.Certificates)
                {
                    if (cert.Subject.Contains(SettingsManager.Settings.UseCertificate))
                    {
                        csp = (RSACryptoServiceProvider)cert.PrivateKey;
                        break;
                    }
                }

                if (csp == null)
                {
                    ErrorText = "Cannot find CERT";
                    return (false);
                }

                byte[] signature = csp.SignData(Encoding.ASCII.GetBytes("Test"), new SHA1CryptoServiceProvider());
                if (signature == null)
                {
                    ErrorText = "Signing retured no data";
                    return (false);
                }
                return (true);
            }
            catch (Exception ee)
            {
                ErrorText = "SEH: " + ee.Message;
                return (false);
            }

        }
    }
}
