using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_InstallPackage
{
    class Program
    {
        static void Help()
        {
            Console.WriteLine("-install filename [cer file]");
            Console.WriteLine("  Installs the package");
            Console.WriteLine("-test filename [cer file]");
            Console.WriteLine("  Test the package, but don't install");
            Console.WriteLine("-info filename [cer file]");
            Console.WriteLine("  Display info of the package");
            Console.WriteLine("");
            Console.WriteLine("cer file - certificate file, to include in the package checker");
        }

        static List<byte[]> LoadCERFiles(string[] args)
        {
            List<byte[]> lst = new List<byte[]>();

            if (args.Length < 3)
                return (lst);

            for (int i = 2; i < args.Length; i++)
            {
                if (File.Exists(args[i]) == false)
                    continue;
                FileInfo filesz = new FileInfo(args[i]);
                if (filesz.Length > 5242880)
                    continue;
                byte[] data = File.ReadAllBytes(args[i]);
                lst.Add(data);
            }

            return (lst);
        }

        static int Main(string[] args)
        {
            Console.WriteLine("Fox Installer");
            Console.WriteLine("");
            if (args.Length < 2)
            {
                Help();
                return (1);
            }

            ComputerCertificate ccert = new ComputerCertificate();
            if (ccert.GetCertificate() == false)
            {
                Console.WriteLine("Cannot load computer certificate from Fox SDC");
#if DEBUG
                Console.WriteLine("Press any key . . .");
                Console.ReadKey(true);
#endif
                return (2);
            }

            PackageInstaller pkgi = new PackageInstaller();
            PKGStatus pkgres = PKGStatus.NotNeeded;
            PKGRecieptData reciept = null;
            string ErrorText = "";
            bool res = false;
            List<byte[]> cer = new List<byte[]>();

            switch (args[0].ToLower().Trim())
            {
                case "-install":
                    Console.WriteLine("Installing " + args[1].Trim());
                    cer = LoadCERFiles(args);
                    res = pkgi.InstallPackage(args[1].Trim(), cer, PackageInstaller.InstallMode.Install, false, out ErrorText, out pkgres, out reciept);
                    break;
                case "-test":
                    Console.WriteLine("Testing " + args[1].Trim());
                    cer = LoadCERFiles(args);
                    res = pkgi.InstallPackage(args[1].Trim(), cer, PackageInstaller.InstallMode.Test, false, out ErrorText, out pkgres, out reciept);
                    break;
                case "-info":
                    cer = LoadCERFiles(args);
                    res = pkgi.PackageInfo(args[1].Trim(), cer, out ErrorText);
                    Console.WriteLine(ErrorText);
#if DEBUG
                    Console.WriteLine("Press any key . . .");
                    Console.ReadKey(true);
#endif
                    return (res == true ? 0 : 1);
                default:
                    Console.WriteLine("Invalid argurments");
#if DEBUG
                    Console.WriteLine("Press any key . . .");
                    Console.ReadKey(true);
#endif
                    return (1);
            }

            Console.WriteLine("Status: " + pkgres.ToString());

            if (res == false)
            {
                Console.WriteLine(ErrorText);
            }
            else
            {
                if (reciept != null)
                {
                    string recppath = Path.GetDirectoryName(args[1].Trim());
                    if (recppath.EndsWith("\\") == false)
                        recppath += "\\";
                    string recieptfile = recppath + Path.GetFileNameWithoutExtension(args[1].Trim()) + ".foxrecp";
                    string recieptfilesign = recppath + Path.GetFileNameWithoutExtension(args[1].Trim()) + ".sign";
#if DEBUG
                    string recps = JsonConvert.SerializeObject(reciept, Formatting.Indented);
#else
                    string recps = JsonConvert.SerializeObject(reciept);
#endif
                    File.WriteAllText(recieptfile, recps, Encoding.UTF8);
                    byte[] sign = ccert.Sign(Encoding.UTF8.GetBytes(recps));
                    File.WriteAllBytes(recieptfilesign, sign);
                }
            }
#if DEBUG
            Console.WriteLine("Press any key . . .");
            Console.ReadKey(true);
#endif
            return (0);
        }
    }
}
