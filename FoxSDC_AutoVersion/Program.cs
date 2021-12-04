using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_AutoVersion
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("FoxSDC_AutoVersion Source.cs Namespace");
                Console.WriteLine(" or ");
                Console.WriteLine("FoxSDC_AutoVersion -foxps FoxPSFile.FoxPS FoxSDC_Agent.exe");
                return (1);
            }

            if (args[0].ToLower() != "-foxps")
            {
                int Revision = 1;
                DateTime WasCompiled = DateTime.UtcNow;

                if (File.Exists(args[0]) == true)
                {
                    IEnumerable<string> ielines = File.ReadLines(args[0]);
                    string[] lines = ielines.ToArray();
                    if (lines[0].StartsWith("//REV:") == true)
                    {
                        string[] splitty = lines[0].Split(':');
                        if (int.TryParse(splitty[1], out Revision) == false)
                            Revision = 0;
                        Revision++;
                        if (splitty.Length > 2)
                        {
                            if (DateTime.TryParseExact(splitty[2], "yyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out WasCompiled) == false)
                            {
                                WasCompiled = DateTime.UtcNow;
                                Console.WriteLine("Cannot read DT from file");
                            }
                        }
                    }
                }

                if (WasCompiled.Date < DateTime.UtcNow.Date)
                    Revision = 1;

                WasCompiled = DateTime.UtcNow;

                if (Revision > 99)
                    Revision = 1;

                string data = "";
                data = "//REV:" + Revision.ToString() + ":" + WasCompiled.ToString("yyMMddHHmmss") + "\r\n" +
                    "using System;\r\n" +
                    "namespace " + args[1].Trim() + "\r\n" +
                    "{\r\n" +
                    "\tpublic class FoxVersion\r\n" +
                    "\t{\r\n" +
                    "\t\tpublic const Int64 Version = " + DateTime.UtcNow.ToString("yyMMdd") + Revision.ToString("00") + ";\r\n" +
                    "\t\tpublic const string VersionS = \"" + DateTime.UtcNow.ToString("yyMMdd") + Revision.ToString("00") + "\";\r\n" +
                    "\t\tpublic const string DTV = \"" + DateTime.UtcNow.ToString("yyMM.dd") + Revision.ToString("00") + "\";\r\n" +
                    "\t\tpublic static readonly DateTime DT = new DateTime(" + DateTime.UtcNow.Year + ", " + DateTime.UtcNow.Month + ", " +
                        DateTime.UtcNow.Day + ", " + DateTime.UtcNow.Hour + ", " + DateTime.UtcNow.Minute + ", " + DateTime.UtcNow.Second + ")" + ";\r\n" +
                    "\t\tpublic const int Revision = " + Revision.ToString() + ";\r\n" +
                    "\t}\r\n" +
                    "}\r\n";
                try
                {
                    File.WriteAllText(args[0], data, Encoding.UTF8);
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                    return (-1);
                }
            }
            else
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("Missing argurments");
                    return (1);
                }

                if (File.Exists(args[1]) == false)
                {
                    Console.WriteLine("Cannot find " + args[1]);
                    return (4);
                }
                if (File.Exists(args[2]) == false)
                {
                    Console.WriteLine("Cannot find " + args[2]);
                    return (5);
                }

                string data;

                try
                {
                    data = File.ReadAllText(args[1], Encoding.UTF8);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Cannot read the file: " + ee.Message);
                    return (6);
                }

                PKGRootData r;
                try
                {
                    r = JsonConvert.DeserializeObject<PKGRootData>(data);
                    if (r.HeaderID != "FoxPackageScriptV1")
                    {
                        Console.WriteLine("The file is not a valid Package Script");
                        return (7);
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                    Console.WriteLine("The file cannot be parsed.");
                    return (8);
                }

                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(args[2]);
                if (string.IsNullOrWhiteSpace(ver.ProductName) == true)
                {
                    Console.WriteLine("Product Name is blank.");
                    return (10);
                }
                if (ver.ProductName.IndexOf("Version:") == -1)
                {
                    Console.WriteLine("Cannot read version ID.");
                    return (10);
                }
                string VersionID = ver.ProductName.Substring(ver.ProductName.IndexOf("Version:") + 8).Trim();
                if (string.IsNullOrEmpty(VersionID) == true)
                {
                    Console.WriteLine("VersionID turns out be empty.");
                    return (11);
                }
                Int64 V;
                if (Int64.TryParse(VersionID, out V) == false)
                {
                    Console.WriteLine("Cannot interpret version ID.");
                    return (11);
                }

                r.VersionID = V;

                try
                {
                    data = JsonConvert.SerializeObject(r, Formatting.Indented);
                    File.WriteAllText(args[1], data, Encoding.UTF8);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Cannot write the package file: " + ee.Message);
                    return (11);
                }

                Console.WriteLine("Package version updated successfully to " + V.ToString() + "!");
            }

            return (0);
        }

    }
}

