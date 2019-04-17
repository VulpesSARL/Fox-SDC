using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_InstallPackage
{
    class ComputerCertificate
    {
        X509Certificate2 Cert;

        public bool LoadedCert
        {
            get
            {
                return (Cert == null ? false : true);
            }
        }

        public byte[] Sign(byte[] data)
        {
            return (Certificates.Sign(data, Cert));
        }

        public bool Verify(byte[] data, byte[] signature)
        {
            if (Certificates.Verify(data, signature, Cert) == false)
                return (false);

            return (true);
        }

        public bool GetCertificate()
        {
            Cert = null;
            string ProgramData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (ProgramData.EndsWith("\\") == false)
                ProgramData += "\\";
            ProgramData += "Fox\\SDC Agent\\";

            string myUCID = UCID.GetUCID();

            string CertFile = ProgramData + "cert.pfx";
            if (File.Exists(CertFile) == false)
                return (false);

            Cert = new X509Certificate2(CertFile, myUCID.ToUpper());
            if (Cert == null)
                return (false);

            string TestCN = "CN=Fox SDC Certificate for " + SystemInformation.ComputerName;
            if (Cert.Subject != TestCN)
            {
                Cert = null;
                return (false);
            }

            return (true);
        }


    }
}
