using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_SigningTool
{
    public partial class frmCreateCER : FForm
    {
        public frmCreateCER()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {

            X509V1CertificateGenerator certGen = new X509V1CertificateGenerator();

            IDictionary attrs = new Hashtable();
            attrs[X509Name.CN] = txtCN.Text;
            attrs[X509Name.O] = txtCompany.Text;
            attrs[X509Name.C] = txtCC.Text;
            attrs[X509Name.ST] = txtProvince.Text;
            attrs[X509Name.OU] = txtOU.Text;
            attrs[X509Name.L] = txtCity.Text;



            IList ord = new ArrayList();
            ord.Add(X509Name.CN);
            ord.Add(X509Name.O);
            ord.Add(X509Name.C);
            ord.Add(X509Name.ST);
            ord.Add(X509Name.OU);
            ord.Add(X509Name.L);


            X509Name CN = new X509Name(ord, attrs);
            Org.BouncyCastle.X509.X509Certificate newCert;

            certGen.SetSerialNumber(BigInteger.ProbablePrime(120, new Random()));
            certGen.SetIssuerDN(CN);
            certGen.SetNotAfter(new DateTime(DT.Value.Year, DT.Value.Month, DT.Value.Day, 0, 0, 0, 0));
            certGen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
            certGen.SetSubjectDN(CN);
            CryptoApiRandomGenerator randomGenerator = new CryptoApiRandomGenerator();

            SecureRandom random;
            AsymmetricCipherKeyPair keypair;

            if (txtSPKI.Text == "")
            {
                RsaKeyPairGenerator keypairgen = new RsaKeyPairGenerator();
                keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048));

                keypair = keypairgen.GenerateKeyPair();
                certGen.SetPublicKey(keypair.Public);
                random = new SecureRandom(randomGenerator);
                ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256WITHRSA", keypair.Private, random);
                newCert = certGen.Generate(signatureFactory);
            }
            else
            {
                RsaPublicKeyStructure rsaPubStructure = RsaPublicKeyStructure.GetInstance(Asn1Object.FromByteArray(File.ReadAllBytes(txtSPKI.Text)));

                AsymmetricKeyParameter extpublickey = (AsymmetricKeyParameter)(new RsaKeyParameters(false, rsaPubStructure.Modulus, rsaPubStructure.PublicExponent));
                certGen.SetPublicKey(extpublickey);

                RsaKeyPairGenerator keypairgen = new RsaKeyPairGenerator();
                keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048));
                keypair = keypairgen.GenerateKeyPair();
                random = new SecureRandom(randomGenerator);
                ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256WITHRSA", keypair.Private, random);
                newCert = certGen.Generate(signatureFactory);
            }


            byte[] PlainCer = newCert.GetEncoded();

            SaveFileDialog save = new SaveFileDialog();
            if (lstOutput.SelectedIndex == 0)
            {
                save.Filter = "Certificate|*.cer";
                save.DefaultExt = ".cer";
                save.Title = "Save CER file";
            }
            if (lstOutput.SelectedIndex == 1)
            {
                save.Filter = "Certificate|*.der";
                save.DefaultExt = ".der";
                save.Title = "Save DER file";
            }
            if (lstOutput.SelectedIndex == 2)
            {
                save.Filter = "Certificate|*.p12";
                save.DefaultExt = ".p12";
                save.Title = "Save P12 file";
            }
            if (save.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            if (lstOutput.SelectedIndex == 0)
            {
                File.WriteAllBytes(save.FileName, PlainCer);
            }
            if (lstOutput.SelectedIndex == 1)
            {
                TextWriter txt = new StreamWriter(save.FileName, false, Encoding.ASCII);
                PemWriter text = new PemWriter(txt);
                text.WriteObject(newCert);
                txt.Close();
            }
            if (lstOutput.SelectedIndex == 2)
            {
                Pkcs12Store pkcs = new Pkcs12Store();
                pkcs.SetCertificateEntry(txtCN.Text, new X509CertificateEntry(newCert));
                AsymmetricKeyEntry keyentry = new AsymmetricKeyEntry(keypair.Private);
                pkcs.SetKeyEntry(txtCN.Text, keyentry, new[] { new X509CertificateEntry(newCert) });
                MemoryStream mem = new MemoryStream();
                pkcs.Save(mem, txtPassword.Text.ToCharArray(), random);
                PlainCer = newCert.GetEncoded();

                File.WriteAllBytes(save.FileName, mem.GetBuffer());
            }
            this.Close();
        }

        private void frmCreateCER_Load(object sender, EventArgs e)
        {
            DT.Value = new DateTime(2099, 1, 1, 0, 0, 0, 0);
            txtCity.Text = "Diddeleng";
            txtCC.Text = "LU";
            txtCN.Text = "Vulpes Licensing";
            txtCompany.Text = "Vulpes";
            txtOU.Text = "Vulpes IT";
            txtProvince.Text = "Luxembourg";
            lstOutput.Items.Add("CER (binary)");
            lstOutput.Items.Add("DER (text)");
            lstOutput.Items.Add("PKCS#12");
            lstOutput.SelectedIndex = 0;
        }

        private void cmdSearchSPKI_Click(object sender, EventArgs e)
        {
            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Filter = "SPKI files|*.spki";
            cmdlg.Title = "Search SPKI";
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != DialogResult.OK)
                return;
            txtSPKI.Text = cmdlg.FileName;
        }
    }
}
