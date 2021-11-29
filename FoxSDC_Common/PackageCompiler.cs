using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace FoxSDC_Common
{
    public interface PKGScript
    {
        List<string> GetDependencies(PKGRunningPackageData Package);
        PKGStatus CheckInstallationStatus(PKGRunningPackageData Package, PKGInstallState State);
        PKGStatus PreInstall(PKGRunningPackageData Package);
        PKGStatus PostInstall(PKGRunningPackageData Package);
        PKGStatus Rollback(PKGRunningPackageData Package);
        PKGStatus ApplyUserSettings(PKGRunningPackageData Package);
    }

    public class PKGRunningPackageData
    {
        public string Title;
        public string PackageID;
        public List<PKGFile> Files;
        public bool RebootRequired;
        public List<byte[]> CerCertificates;
        public PKGScript script;
        public Assembly asm;
        public string ErrorText;
        public string scriptsource;
        public PKGRecieptData RecieptData;
        public string Description;
        public string Filename;
        public bool NoReciept;
        public string ApplicationPath;
        public long VersionID;

        public void SetInstallPath(string path)
        {
            Environment.SetEnvironmentVariable("INSTALLPATH", Environment.ExpandEnvironmentVariables(path), EnvironmentVariableTarget.Process);
        }
    }

    public class PKGRecieptData
    {
        public string HeaderID;
        public List<string> InstalledFiles = new List<string>();
        public List<string> CreatedFolders = new List<string>();
        public string scriptsource;
        public DateTime InstalledOn;
        public string PackageID;
        public string Description;
    }

    public class PKGRename
    {
        public string Source;
        public string Destination;
    }

    public enum PKGStatus
    {
        Unknown,
        NotNeeded,
        Failed,
        Success,
        DependencyFailed,
    }

    public enum PKGInstallState
    {
        NotSet,
        Install,
        Uninstall,
        Test,
        Update,
        UpdateTest
    }

    public class PKGRootData
    {
        public string HeaderID;
        public string Title;
        public string Description;
        public string PackageID;
        public string Script;
        public string Outputfile;
        public bool NoReceipt;
        public Int64 VersionID;
        public List<PKGFile> Files = new List<PKGFile>();
        public List<PKGFolder> Folders = new List<PKGFolder>();
        public Dictionary<string, List<byte[]>> Signatures = new Dictionary<string, List<byte[]>>();
    }

    public class PKGFolder
    {
        public string FolderName;
    }

    public class PKGCompilerArgs
    {
        public bool UseExtSign;
        public string SignCert;
        public StoreLocation SignLocation;
        public string SignExtCert;
        public SecureString PIN;
    }

    public class PKGFile
    {
        public string ID;
        public string FolderName;
        public string FileName;
        public string SrcFile;
        public bool InstallThisFile;
        public bool KeepInMeta;
    }
    public class PackageCompiler
    {
        public delegate void StatusUpdate(string Text);
        public event StatusUpdate OnStatusUpdate;
        public bool AbortProcess = false;

        void UpdateText(string Text)
        {
            if (OnStatusUpdate != null)
                OnStatusUpdate(Text);
        }

        public static Assembly CompileScript(string Script, out string ErrorText, string OtherDLL, out string PathToAssembly)
        {
            PathToAssembly = "";
            ErrorText = "";
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = true;
            cp.IncludeDebugInformation = false;
            cp.TreatWarningsAsErrors = false;
            cp.IncludeDebugInformation = true;
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (dir.EndsWith("\\") == false)
                dir += "\\";
            if (OtherDLL == null || OtherDLL == "")
                cp.ReferencedAssemblies.Add(dir + "FoxSDC_Common.dll");
            else
                cp.ReferencedAssemblies.Add(dir + OtherDLL);

            CompilerResults cr = provider.CompileAssemblyFromSource(cp, Script);
            if (cr.Errors.Count > 0)
            {
                foreach (CompilerError ce in cr.Errors)
                {
                    Debug.WriteLine(ce.ToString());
                    ErrorText += ce.ToString() + "\r\n";
                }
                return (null);
            }
            PathToAssembly = null;

            return (cr.CompiledAssembly);
        }

        public bool CompilePackage(string PackageFilename, PKGCompilerArgs Args, out string ErrorText)
        {
            string data = "";
            UpdateText("Loading data");
            try
            {
                data = File.ReadAllText(PackageFilename, Encoding.UTF8);
            }
            catch (Exception ee)
            {
                ErrorText = "Cannot read the file: " + ee.Message;
                return (false);
            }

            PKGRootData r;
            try
            {
                r = JsonConvert.DeserializeObject<PKGRootData>(data);
                if (r.HeaderID != "FoxPackageScriptV1")
                {
                    ErrorText = "The file is not a valid Package Script";
                    return (false);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorText = "The file cannot be parsed.";
                return (false);
            }

            bool res = CheckScript(r, out ErrorText);
            if (res == false)
                return (false);

            return (CompilePackage(r, Args, out ErrorText));
        }

        public bool CompilePackage(PKGRootData Package, PKGCompilerArgs Args, out string ErrorText)
        {
            try
            {
                ErrorText = "";
                AbortProcess = false;
                UpdateText("Preparing data");

                PKGRootData ZipDef = new PKGRootData();
                ZipDef.Description = Package.Description;
                ZipDef.HeaderID = Package.HeaderID;
                ZipDef.Outputfile = "";
                ZipDef.PackageID = Package.PackageID;
                ZipDef.Script = Package.Script;
                ZipDef.Title = Package.Title;
                ZipDef.VersionID = Package.VersionID;
                ZipDef.Folders = new List<PKGFolder>(); //not needed here
                ZipDef.Files = new List<PKGFile>();
                ZipDef.Signatures = new Dictionary<string, List<byte[]>>();

                using (Stream filestream = File.Open(Package.Outputfile, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    using (ZipArchive ZipFile = new ZipArchive(filestream, ZipArchiveMode.Create, true, Encoding.UTF8))
                    {
                        ZipArchiveEntry entry;
                        byte[] signature;

                        foreach (PKGFile file in Package.Files)
                        {
                            if (AbortProcess == true)
                            {
                                filestream.Close();
                                ErrorText = "Aborted";
                                return (false);
                            }

                            UpdateText("Compressing " + file.SrcFile);

                            PKGFile inzipfile = new PKGFile();
                            inzipfile.ID = file.ID;
                            inzipfile.SrcFile = Guid.NewGuid().ToString();
                            inzipfile.FileName = file.FileName;
                            inzipfile.FolderName = file.FolderName;
                            inzipfile.KeepInMeta = file.KeepInMeta;
                            inzipfile.InstallThisFile = file.InstallThisFile;

                            Stream FileStream = File.Open(file.SrcFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                            entry = ZipFile.CreateEntry(inzipfile.SrcFile, CompressionLevel.Optimal);
                            using (Stream ZipStream = entry.Open())
                            {
                                FileStream.CopyTo(ZipStream);
                            }
                            FileStream.Seek(0, SeekOrigin.Begin);

                            if (AbortProcess == true)
                            {
                                filestream.Close();
                                ErrorText = "Aborted";
                                return (false);
                            }

                            UpdateText("Signing " + file.SrcFile);

                            signature = null;
                            if (Args.UseExtSign == false)
                            {
                                signature = Certificates.Sign(FileStream, Args.SignCert, Args.SignLocation);
                            }
                            else
                            {
                                SmartCards sm = new SmartCards();
                                signature = sm.SignData(Args.SignExtCert, FileStream, Args.PIN);
                            }
                            FileStream.Close();
                            if (signature == null)
                            {
                                ErrorText = "Signing failed";
                                filestream.Close();
                                return (false);
                            }

                            UpdateText("Storing Signature " + file.SrcFile);

                            ZipDef.Files.Add(inzipfile);
                            ZipDef.Signatures.Add(inzipfile.SrcFile.ToLower(), new List<byte[]> { signature });
                        }

                        UpdateText("Finalizing");

                        byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ZipDef, Formatting.None));
                        signature = null;
                        if (Args.UseExtSign == false)
                        {
                            signature = Certificates.Sign(data, Args.SignCert, Args.SignLocation);
                        }
                        else
                        {
                            SmartCards sm = new SmartCards();
                            signature = sm.SignData(Args.SignExtCert, data, Args.PIN);
                        }
                        entry = ZipFile.CreateEntry("Script.json", CompressionLevel.Optimal);
                        using (Stream ZipStream = entry.Open())
                        {
                            ZipStream.Write(data, 0, data.Length);
                        }

                        entry = ZipFile.CreateEntry("Script.json.sign", CompressionLevel.Optimal);
                        using (Stream ZipStream = entry.Open())
                        {
                            ZipStream.Write(signature, 0, signature.Length);
                        }
                        return (true);
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorText = "SEH Error";
                return (false);
            }
        }

        public bool CheckScript(PKGRootData Package, out string ErrorText)
        {
            try
            {
                ErrorText = "";
                AbortProcess = false;
                UpdateText("Checking data");
                if (Package.PackageID.Trim() == "")
                {
                    ErrorText = "Invalid element (PackageID=\"\")";
                    return (false);
                }
                if (Package.Title.Trim() == "")
                {
                    ErrorText = "Invalid element (Title=\"\")";
                    return (false);
                }
                foreach (PKGFile f in Package.Files)
                {
                    UpdateText("Checking " + f.SrcFile);
                    Debug.WriteLine("Checking " + f.SrcFile);
                    if (File.Exists(f.SrcFile) == false)
                    {
                        ErrorText = "File " + f.SrcFile + " does not exist.";
                        return (false);
                    }
                    if (f.FileName.Trim() == "")
                    {
                        ErrorText = "Invalid element (Filename=\"\")";
                        return (false);
                    }
                    if (f.FolderName.Trim() == "")
                    {
                        ErrorText = "Invalid element (FolderName=\"\")";
                        return (false);
                    }
                    if (AbortProcess == true)
                    {
                        ErrorText = "Aborted";
                        return (false);
                    }
                }
                UpdateText("");
                return (true);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorText = "SEH Error";
                return (false);
            }
        }
    }
}
