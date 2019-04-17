using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class CertificateCreation
    {
        public static byte[] GenerateRootCertificate(string certName, string Password, out byte[] PlainCer)
        {
            PlainCer = null;

            X509V1CertificateGenerator certGen = new X509V1CertificateGenerator();

            X509Name CN = new X509Name("CN=" + certName);

            RsaKeyPairGenerator keypairgen = new RsaKeyPairGenerator();
            keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048));

            AsymmetricCipherKeyPair keypair = keypairgen.GenerateKeyPair();

            certGen.SetSerialNumber(BigInteger.ProbablePrime(120, new Random()));
            certGen.SetIssuerDN(CN);
            certGen.SetNotAfter(new DateTime(2099, 1, 1));
            certGen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
            certGen.SetSubjectDN(CN);
            certGen.SetPublicKey(keypair.Public);

            CryptoApiRandomGenerator randomGenerator = new CryptoApiRandomGenerator();
            SecureRandom random = new SecureRandom(randomGenerator);

            ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256WITHRSA", keypair.Private, random);

            Org.BouncyCastle.X509.X509Certificate newCert = certGen.Generate(signatureFactory);

            Pkcs12Store pkcs = new Pkcs12Store();
            pkcs.SetCertificateEntry(certName, new X509CertificateEntry(newCert));
            AsymmetricKeyEntry keyentry = new AsymmetricKeyEntry(keypair.Private);
            pkcs.SetKeyEntry(certName, keyentry, new[] { new X509CertificateEntry(newCert) });
            MemoryStream mem = new MemoryStream();
            pkcs.Save(mem, Password.ToCharArray(), random);
            PlainCer = newCert.GetEncoded();

            return (mem.GetBuffer());
        }
    }
}
