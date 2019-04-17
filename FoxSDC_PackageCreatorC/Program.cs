using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_PackageCreatorC
{
    class Program
    {
        static void Help()
        {
            Console.WriteLine("Fox SDC Console Package Compiler");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("FoxSDC_PackageCreatorC Command .....");
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("  ListCerts");
            Console.WriteLine("     List the installed certificates available to use and CSPs");
            Console.WriteLine("");
            Console.WriteLine("  Compile Filename CertMethod Cert");
            Console.WriteLine("     Compiles a Package Script");
            Console.WriteLine("        Filename:   the FOXPS file used to compile");
            Console.WriteLine("        CertMethod: CERT - uses a local installed certificate");
            Console.WriteLine("                    CSP  - uses a CSP driver (e.g. smart card)");
            Console.WriteLine("        Cert:       Name of the certificate or CSP to use");
            Console.WriteLine("");
        }

        enum CertMethod
        {
            CSP,
            CERT,
            UNKNOWN
        }

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Help();
                return (1);
            }

            switch (args[0].ToLower())
            {
                case "-?":
                case "/?":
                case "/h":
                case "-h":
                case "?":
                    Help();
                    return (1);
                case "listcerts":
                    {
                        Console.WriteLine("Available certificates:");
                        foreach (string s in Certificates.GetCertificates(StoreLocation.CurrentUser))
                        {
                            Console.WriteLine("  " + s);
                        }
                        Console.WriteLine("Available CSPs:");

                        foreach (string s in SmartCards.GetCSPProviders())
                        {
                            Console.WriteLine("  " + s);
                        }
                        return (0);
                    }
                case "compile":
                    {
                        if (args.Length < 4)
                        {
                            Console.WriteLine("Insufficient argurments.");
                            return (1);
                        }
                        string filename = args[1];
                        CertMethod certmeth = CertMethod.UNKNOWN;
                        switch (args[2].ToLower())
                        {
                            case "cert":
                                certmeth = CertMethod.CERT;
                                break;
                            case "csp":
                                certmeth = CertMethod.CSP;
                                break;
                        }
                        if (certmeth == CertMethod.UNKNOWN)
                        {
                            Console.WriteLine("Unknown certificate method.");
                            return (1);
                        }
                        string certname = args[3];

                        PKGCompilerArgs pkgcompiler = new PKGCompilerArgs();
                        pkgcompiler.UseExtSign = certmeth == CertMethod.CSP ? true : false;
                        pkgcompiler.SignCert = certname;
                        pkgcompiler.SignExtCert = certname;
                        pkgcompiler.SignLocation = StoreLocation.CurrentUser;
                        pkgcompiler.PIN = null;

                        string ErrorText;
                        PackageCompiler PackageCompiler = new PackageCompiler();
                        PackageCompiler.OnStatusUpdate += PackageCompiler_OnStatusUpdate;
                        bool res = PackageCompiler.CompilePackage(filename, pkgcompiler, out ErrorText);
                        if (res == false)
                        {
                            Console.WriteLine("\n" + ErrorText + "\n\nFAILED!\n");
                            return (5);
                        }
                        Console.WriteLine("Success");
                        return(0);
                    }
                default:
                    {
                        Console.WriteLine("Unsupported command");
                        break;
                    }
            }

            return (1);
        }

        private static void PackageCompiler_OnStatusUpdate(string Text)
        {
            Console.WriteLine(Text);
        }
    }
}
