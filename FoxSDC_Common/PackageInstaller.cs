using FoxSDC_Common.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FoxSDC_Common
{
    public class PackageInstaller
    {
        public delegate void StatusUpdate(string Text);
        public event StatusUpdate OnStatusUpdate;
        public bool AbortProcess = false;
        public List<string> RequiredDependencies = null;
        public PKGRootData PackageInfoData = null;

        public string ScriptTempDLLFilename = null;

        void UpdateText(string Text)
        {
            if (OnStatusUpdate != null)
                OnStatusUpdate(Text);
        }

        byte[] GetZipData(ZipArchiveEntry entry)
        {
            Stream str = entry.Open();
            MemoryStream memstr = new MemoryStream();
            str.CopyTo(memstr);
            str.Close();
            return (memstr.GetBuffer());
        }

        Stream GetZipStreamData(ZipArchiveEntry entry)
        {
            return (entry.Open());
        }

        bool OutputToFile(Alphaleonis.Win32.Filesystem.KernelTransaction kt, string InternalFilename, string ExternalFilename, ZipArchive ZipArch, out string ErrorText)
        {
            ErrorText = "";
            ZipArchiveEntry entry = ZipArch.GetEntry(InternalFilename);
            if (entry == null)
                return (false);

            try
            {
                using (FileStream extfile = Alphaleonis.Win32.Filesystem.File.OpenTransacted(kt, ExternalFilename, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    int lasterr = Marshal.GetLastWin32Error();
                    if (lasterr != 0)
                    {
                        ErrorText = "Cannot copy to " + ExternalFilename + " 0x" + lasterr.ToString("X");
                        return (false);
                    }
                    using (Stream intfile = entry.Open())
                    {
                        intfile.CopyTo(extfile);
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorText = "Cannot copy to " + ExternalFilename + " " + ee.Message;
                Debug.WriteLine(ee.ToString());
                return (false);
            }

            return (true);
        }

        bool TestSignatureBySignFile(string Filename, ZipArchive ZipArch, List<byte[]> CertificatesData)
        {
            ZipArchiveEntry entry = ZipArch.GetEntry(Filename);
            if (entry == null)
                return (false);

            bool SignValid = false;

            for (int i = 1; i < 9; i++)
            {
                //Debug.WriteLine("Check " + Filename + " with Signature " + Filename + ".sign" + (i == 1 ? "" : i.ToString()));
                UpdateText("Checking " + Filename);
                ZipArchiveEntry entry2 = ZipArch.GetEntry(Filename + ".sign" + (i == 1 ? "" : i.ToString()));
                if (entry2 == null)
                    break;
                byte[] signdata = GetZipData(entry2);
                foreach (byte[] cer in CertificatesData)
                {
                    using (Stream data = GetZipStreamData(entry))
                    {
                        SignValid = Certificates.Verify(data, signdata, cer);
                    }
                    if (SignValid == true)
                        break;
                }
                if (SignValid == true)
                    break;
            }

            return (SignValid);
        }

        bool TestSignatureByInternalSigning(PKGRootData Package, string Filename, ZipArchive ZipArch, List<byte[]> CertificatesData)
        {
            ZipArchiveEntry entry = ZipArch.GetEntry(Filename);
            if (entry == null)
                return (false);

            bool SignValid = false;

            if (Package.Signatures.ContainsKey(Filename.ToLower()) == false)
                return (false);

            foreach (byte[] signdata in Package.Signatures[Filename.ToLower()])
            {
                //Debug.WriteLine("Check " + Filename + " with Signature " + Filename + ".sign" + (i == 1 ? "" : i.ToString()));
                UpdateText("Checking " + Filename);

                foreach (byte[] cer in CertificatesData)
                {
                    using (Stream data = GetZipStreamData(entry))
                    {
                        SignValid = Certificates.Verify(data, signdata, cer);
                    }
                    if (SignValid == true)
                        break;
                }
                if (SignValid == true)
                    break;
            }

            return (SignValid);
        }

        bool CheckScript(PKGRootData Package, ZipArchive ZipArch, bool WithFiles, bool MetaOnly, out string ErrorText)
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
                if (WithFiles == true)
                {
                    foreach (PKGFile f in Package.Files)
                    {
                        UpdateText("Checking " + f.SrcFile);
                        //Debug.WriteLine("Checking " + f.SrcFile);
                        if (MetaOnly == true)
                            if (f.KeepInMeta == false)
                                continue;

                        ZipArchiveEntry entry = ZipArch.GetEntry(f.SrcFile);
                        if (entry == null)
                        {
                            ErrorText = "Cannot find file " + f.SrcFile;
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
                }
                return (true);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorText = "SEH Error";
                return (false);
            }
        }

        public bool PackageInfo(string Filename, List<byte[]> CerCertificates, out string ErrorText)
        {
            ErrorText = "";
            bool ScriptSignFailed = false;
            UpdateText("Initializing");
            if (CerCertificates == null)
                CerCertificates = new List<byte[]>();
            CerCertificates.Add(Resources.Vulpes_Main);

            try
            {
                if (File.Exists(Filename) == false)
                {
                    ErrorText = "File does not exist";
                    return (false);
                }

                using (FileStream ZipFile = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (ZipArchive ZipArch = new ZipArchive(ZipFile, ZipArchiveMode.Read, true, Encoding.UTF8))
                    {
                        ZipArchiveEntry entry;
                        entry = ZipArch.GetEntry("Script.json");
                        if (entry == null)
                        {
                            ZipFile.Close();
                            ErrorText = "Missing Script.json";
                            return (false);
                        }

                        if (TestSignatureBySignFile("Script.json", ZipArch, CerCertificates) == false)
                        {
                            ScriptSignFailed = true;
                        }

                        byte[] packagedata;

                        packagedata = GetZipData(entry);
                        PKGRootData Package;
                        string script = Encoding.UTF8.GetString(packagedata);
                        try
                        {
                            Package = JsonConvert.DeserializeObject<PKGRootData>(script);
                            if (Package.HeaderID != "FoxPackageScriptV1")
                            {
                                ErrorText = "Script.json is not the format wanted (Header)!";
                                ZipFile.Close();
                                return (false);
                            }
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());
                            ErrorText = "Script.json is not the format wanted (Format)!";
                            ZipFile.Close();
                            return (false);
                        }

                        if (CheckScript(Package, ZipArch, false, false, out ErrorText) == false)
                            return (false);

                        ErrorText = "Package Details:\n";
                        ErrorText += "Name:         " + Package.Title + "\n";
                        ErrorText += "ID:           " + Package.PackageID + "\n";
                        ErrorText += "Description:  " + Package.Description + "\n";
                        ErrorText += "Version ID:   " + Package.VersionID.ToString() + "\n";
                        if (ScriptSignFailed == true)
                            ErrorText += "!QUICK SIGNATURE CHECK FAILED!\n";

                        ZipFile.Close();

                        PackageInfoData = Package;
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorText = "Internal error, likely a corrupted ZIP file";
                return (false);
            }
            return (true);
        }

        bool InitPackageZipFile(string Filename, List<byte[]> CerCertificates, bool ZipIsMetaOnly, out string ErrorText,
            out PKGRunningPackageData RunningPKG, string OtherDLL)
        {
            ErrorText = "";
            RunningPKG = null;
            UpdateText("Initializing");
            Debug.WriteLine("Testing " + Filename);


            using (FileStream ZipFile = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ZipArchive ZipArch = new ZipArchive(ZipFile, ZipArchiveMode.Read, true, Encoding.UTF8))
                {
                    ZipArchiveEntry entry;
                    entry = ZipArch.GetEntry("Script.json");
                    if (entry == null)
                    {
                        ZipFile.Close();
                        ErrorText = "Missing Script.json";
                        return (false);
                    }

                    if (TestSignatureBySignFile("Script.json", ZipArch, CerCertificates) == false)
                    {
                        ZipFile.Close();
                        ErrorText = "Script.json Signature check failed";
                        return (false);
                    }

                    byte[] packagedata;

                    packagedata = GetZipData(entry);
                    PKGRootData Package;
                    string ScriptSource = Encoding.UTF8.GetString(packagedata);
                    try
                    {
                        Package = JsonConvert.DeserializeObject<PKGRootData>(ScriptSource);
                        if (Package.HeaderID != "FoxPackageScriptV1")
                        {
                            ErrorText = "Script.json is not the format wanted (Header)!";
                            ZipFile.Close();
                            return (false);
                        }
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        ErrorText = "Script.json is not the format wanted (Format)!";
                        ZipFile.Close();
                        return (false);
                    }

                    if (CheckScript(Package, ZipArch, true, ZipIsMetaOnly, out ErrorText) == false)
                        return (false);

                    foreach (PKGFile file in Package.Files)
                    {
                        if (ZipIsMetaOnly == true)
                            if (file.KeepInMeta == false)
                                continue;
                        if (TestSignatureByInternalSigning(Package, file.SrcFile, ZipArch, CerCertificates) == false)
                        {
                            ZipFile.Close();
                            ErrorText = file.SrcFile + " Signature check failed";
                            return (false);
                        }

                        if (AbortProcess == true)
                        {
                            ZipFile.Close();
                            ErrorText = "Aborted";
                            return (false);
                        }
                    }

                    Assembly asm = PackageCompiler.CompileScript(Package.Script, out ErrorText, OtherDLL, out ScriptTempDLLFilename);
                    if (asm == null)
                    {
                        ErrorText = "Script contains errors: \r\n" + ErrorText;
                        ZipFile.Close();
                        return (false);
                    }

                    object o = asm.CreateInstance("FoxSDC_Package.PackageScriptTemplate", false);
                    if (o == null)
                    {
                        ErrorText = "Cannot find entrypoint for the script";
                        ZipFile.Close();
                        return (false);
                    }

                    PKGScript script = null;
                    try
                    {
                        script = (PKGScript)o;
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        ErrorText = "Cannot bind function";
                        ZipFile.Close();
                        return (false);
                    }

                    RunningPKG = new PKGRunningPackageData();
                    RunningPKG.Files = Package.Files;
                    RunningPKG.PackageID = Package.PackageID;
                    RunningPKG.RebootRequired = false;
                    RunningPKG.Title = Package.Title;
                    RunningPKG.CerCertificates = CerCertificates;
                    RunningPKG.script = script;
                    RunningPKG.asm = asm;
                    RunningPKG.scriptsource = Package.Script;
                    RunningPKG.Description = Package.Description;
                    RunningPKG.RecieptData = null;
                    RunningPKG.Filename = Filename;
                    RunningPKG.NoReciept = Package.NoReceipt;
                }
            }
            return (true);
        }

        void RollbackFiles(PKGRunningPackageData RunningPKG, out string ErrorText)
        {
            ErrorText = "";
            try
            {
                RunningPKG.script.Rollback(RunningPKG);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorText = "Script Error:\r\n" + ee.ToString();
            }
        }

        public enum InstallMode
        {
            Install,
            Test,
            TestPackageOnly,
            Update,
            UpdateTest,
            ApplyUserSettingsTest
        }

        public bool CreateMetaDataPackage(string Filename, string OutputFilename, out string ErrorText)
        {
            ErrorText = "";
            using (FileStream ZipFile = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ZipArchive ZipArch = new ZipArchive(ZipFile, ZipArchiveMode.Read, true, Encoding.UTF8))
                {
                    ZipArchiveEntry entry;
                    entry = ZipArch.GetEntry("Script.json");
                    if (entry == null)
                    {
                        ZipFile.Close();
                        ErrorText = "Missing Script.json";
                        return (false);
                    }

                    PKGRootData Package = new PKGRootData();
                    byte[] packagedata = GetZipData(entry);
                    string ScriptSource = Encoding.UTF8.GetString(packagedata);
                    try
                    {
                        Package = JsonConvert.DeserializeObject<PKGRootData>(ScriptSource);
                        if (Package.HeaderID != "FoxPackageScriptV1")
                        {
                            ErrorText = "Script.json is not the format wanted (Header)!";
                            ZipFile.Close();
                            return (false);
                        }
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        ErrorText = "Script.json is not the format wanted (Format)!";
                        ZipFile.Close();
                        return (false);
                    }

                    using (FileStream ZipFileOut = File.Open(OutputFilename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                    {
                        using (ZipArchive ZipArchOut = new ZipArchive(ZipFileOut, ZipArchiveMode.Create, true, Encoding.UTF8))
                        {
                            entry = ZipArch.GetEntry("Script.json");
                            if (entry == null)
                            {
                                ZipFile.Close();
                                ErrorText = "Missing Script.json";
                                return (false);
                            }

                            ZipArchiveEntry newentry = ZipArchOut.CreateEntry("Script.json");
                            using (Stream newentrystream = newentry.Open())
                            {
                                using (Stream currententry = entry.Open())
                                {
                                    currententry.CopyTo(newentrystream);
                                }
                            }

                            for (int i = 1; i < 9; i++)
                            {
                                ZipArchiveEntry entry2 = ZipArch.GetEntry("Script.json.sign" + (i == 1 ? "" : i.ToString()));
                                if (entry2 == null)
                                    continue;

                                newentry = ZipArchOut.CreateEntry("Script.json.sign" + (i == 1 ? "" : i.ToString()));
                                using (Stream newentrystream = newentry.Open())
                                {
                                    using (Stream currententry = entry2.Open())
                                    {
                                        currententry.CopyTo(newentrystream);
                                    }
                                }
                            }

                            foreach (PKGFile PFile in Package.Files)
                            {
                                if (PFile.KeepInMeta == false)
                                    continue;

                                entry = ZipArch.GetEntry(PFile.SrcFile);
                                if (entry == null)
                                {
                                    ZipFile.Close();
                                    ErrorText = "Missing " + PFile.SrcFile;
                                    return (false);
                                }
                                ZipArchiveEntry newentry2 = ZipArchOut.CreateEntry(PFile.SrcFile);

                                using (Stream newentrystream2 = newentry2.Open())
                                {
                                    using (Stream currententry2 = entry.Open())
                                    {
                                        currententry2.CopyTo(newentrystream2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (true);
        }

        public bool ApplyUserSettings(string Filename, List<byte[]> CerCertificates, out string ErrorText, out PKGStatus res)
        {
            ErrorText = "";
            res = PKGStatus.Unknown;

            if (CerCertificates == null)
                CerCertificates = new List<byte[]>();
            CerCertificates.Add(Resources.Vulpes_Main);

            PKGRunningPackageData RunningPKG;
            PKGRecieptData RunningReciept = new PKGRecieptData();
            RunningReciept.HeaderID = "FoxRecieptScriptV1";

            if (InitPackageZipFile(Filename, CerCertificates, true, out ErrorText, out RunningPKG, "") == false)
                return (false);

            using (FileStream ZipFile = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ZipArchive ZipArch = new ZipArchive(ZipFile, ZipArchiveMode.Read, true, Encoding.UTF8))
                {
                    RunningReciept.scriptsource = RunningPKG.scriptsource;
                    RunningReciept.PackageID = RunningPKG.PackageID;
                    RunningReciept.InstalledOn = DateTime.Now;
                    RunningReciept.Description = RunningPKG.Description;

                    try
                    {
                        res = RunningPKG.script.ApplyUserSettings(RunningPKG);
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        ErrorText = "Script Error:\r\n" + ee.ToString();
                        return (false);
                    }
                }
            }

            return (true);
        }

        public bool InstallPackage(string Filename, List<byte[]> CerCertificates, InstallMode Mode, bool ZipIsMetaOnly,
            out string ErrorText, out PKGStatus res, out PKGRecieptData Reciept, string OtherDLL = "")
        {
            ErrorText = "";
            Reciept = null;
            PKGRunningPackageData RunningPKG;
            PKGRecieptData RunningReciept = new PKGRecieptData();
            RunningReciept.HeaderID = "FoxRecieptScriptV1";
            res = PKGStatus.Unknown;

            if (CerCertificates == null)
                CerCertificates = new List<byte[]>();
            CerCertificates.Add(Resources.Vulpes_Main);

            if (InitPackageZipFile(Filename, CerCertificates, ZipIsMetaOnly, out ErrorText, out RunningPKG, OtherDLL) == false)
                return (false);

            RunningPKG.ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (RunningPKG.ApplicationPath.EndsWith("\\") == false)
                RunningPKG.ApplicationPath += "\\";

            using (FileStream ZipFile = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ZipArchive ZipArch = new ZipArchive(ZipFile, ZipArchiveMode.Read, true, Encoding.UTF8))
                {
                    RunningReciept.scriptsource = RunningPKG.scriptsource;
                    RunningReciept.PackageID = RunningPKG.PackageID;
                    RunningReciept.InstalledOn = DateTime.Now;
                    RunningReciept.Description = RunningPKG.Description;

                    if (Mode == InstallMode.TestPackageOnly || Mode == InstallMode.ApplyUserSettingsTest)
                        return (true);

                    if (ZipIsMetaOnly == true)
                    {
                        if (Mode == InstallMode.Install || Mode == InstallMode.Update)
                        {
                            Debug.WriteLine("Cannot install/update a Meta-Package");
                            ErrorText = "Cannot install/update a Meta-Package";
                            return (false);
                        }
                    }

                    try
                    {
                        UpdateText("Running pre install script");

                        PKGInstallState inst = PKGInstallState.NotSet;
                        switch (Mode)
                        {
                            case InstallMode.Install:
                                inst = PKGInstallState.Install; break;
                            case InstallMode.Test:
                                inst = PKGInstallState.Test; break;
                            case InstallMode.TestPackageOnly:
                                inst = PKGInstallState.Test; break;
                            case InstallMode.Update:
                                inst = PKGInstallState.Update; break;
                            case InstallMode.UpdateTest:
                                inst = PKGInstallState.UpdateTest; break;
                        }

                        res = RunningPKG.script.CheckInstallationStatus(RunningPKG, inst);
                        if (res == PKGStatus.DependencyFailed)
                        {
                            RequiredDependencies = RunningPKG.script.GetDependencies(RunningPKG);
                            ErrorText = RunningPKG.ErrorText;
                            return (false);
                        }
                        if (res == PKGStatus.NotNeeded)
                        {
                            ErrorText = RunningPKG.ErrorText;
                            return (true); //no installation
                        }
                        if (res == PKGStatus.Failed)
                        {
                            ErrorText = RunningPKG.ErrorText;
                            return (false);
                        }

                        if (Mode == InstallMode.Test || Mode == InstallMode.UpdateTest)
                        {
                            ErrorText = "";
                            res = PKGStatus.Success;
                            return (true);
                        }

                        res = RunningPKG.script.PreInstall(RunningPKG);
                        if (res != PKGStatus.Success)
                        {
                            ErrorText = RunningPKG.ErrorText;
                            return (false);
                        }
                    }
                    catch (Exception ee)
                    {
                        Debug.WriteLine(ee.ToString());
                        ErrorText = "Script Error:\r\n" + ee.ToString();
                        return (false);
                    }

                    RunningReciept.CreatedFolders = new List<string>();
                    RunningReciept.InstalledFiles = new List<string>();

                    int lasterr;
                    CommonUtilities.ResetLastError();
                    List<string> DeleteFiles = new List<string>();

                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        Alphaleonis.Win32.Filesystem.KernelTransaction kt = new Alphaleonis.Win32.Filesystem.KernelTransaction();
                        lasterr = Marshal.GetLastWin32Error();
                        if (lasterr != 0)
                        {
                            ErrorText = "Cannot start transaction 0x" + lasterr.ToString("X");
                            return (false);
                        }

                        foreach (PKGFile file in RunningPKG.Files)
                        {
                            string fldr = Environment.ExpandEnvironmentVariables(file.FolderName);
                            if (fldr.EndsWith("\\") == false)
                                fldr += "\\";
                            string fullfilename = fldr + file.FileName;
                            string renamedfile = fldr + "FOX-" + CommonUtilities.NewGUID + ".PENDING";

                            bool RestartMove = false;

                            try
                            {
                                if (Alphaleonis.Win32.Filesystem.File.ExistsTransacted(kt, fullfilename) == true)
                                {
                                    try
                                    {
                                        Alphaleonis.Win32.Filesystem.File.MoveTransacted(kt, fullfilename, renamedfile);

                                        lasterr = Marshal.GetLastWin32Error();
                                        if (lasterr != 0)
                                        {
                                            string SubErr;
                                            RollbackFiles(RunningPKG, out SubErr);
                                            kt.Rollback();
                                            ErrorText = "Cannot move file " + fullfilename + " => " + renamedfile + " 0x" + lasterr.ToString("X") + "\r\n" + SubErr;
                                            return (false);
                                        }
                                        DeleteFiles.Add(renamedfile);
                                    }
                                    catch
                                    {
                                        RestartMove = true;
                                        string tmp = fullfilename;
                                        fullfilename = renamedfile;
                                        renamedfile = tmp;
                                    }
                                }
                            }
                            catch (Exception ee)
                            {
                                Debug.WriteLine(ee.ToString());

                                UpdateText("Rolling back");

                                string SubErr;
                                RollbackFiles(RunningPKG, out SubErr);
                                kt.Rollback();
                                ErrorText = "Move Error:\r\n" + ee.ToString() + "\r\n" + SubErr;
                                return (false);
                            }

                            UpdateText("Installing " + fullfilename);
                            try
                            {
                                string mkdir = Path.GetDirectoryName(fldr);
                                if (Alphaleonis.Win32.Filesystem.Directory.ExistsTransacted(kt, mkdir) == false)
                                {
                                    try
                                    {
                                        Alphaleonis.Win32.Filesystem.Directory.CreateDirectoryTransacted(kt, mkdir);
                                        RunningReciept.CreatedFolders.Add(fldr);

                                        //lasterr = Marshal.GetLastWin32Error();
                                        //if (lasterr != 0)
                                        //{
                                        //    string SubErr;
                                        //    RollbackFiles(RunningPKG, out SubErr);
                                        //    kt.Rollback();
                                        //    ErrorText = "Cannot create folder " + mkdir + " 0x" + lasterr.ToString("X") + "\r\n" + SubErr;
                                        //    return (false);
                                        //}
                                    }
                                    catch (Exception ee)
                                    {
                                        Debug.WriteLine(ee.ToString());
                                    }
                                }
                            }
                            catch (Exception ee)
                            {
                                Debug.WriteLine(ee.ToString());
                            }

                            CommonUtilities.ResetLastError();
                            try
                            {
                                if (OutputToFile(kt, file.SrcFile, fullfilename, ZipArch, out ErrorText) == false)
                                {
                                    UpdateText("Rolling back");

                                    string SubErr;
                                    RollbackFiles(RunningPKG, out SubErr);
                                    kt.Rollback();
                                    ErrorText += "\r\n" + SubErr;
                                    return (false);
                                }
                            }
                            catch (Exception ee)
                            {
                                Debug.WriteLine(ee.ToString());

                                UpdateText("Rolling back");

                                string SubErr;
                                RollbackFiles(RunningPKG, out SubErr);
                                kt.Rollback();
                                ErrorText = "File Output Error:\r\n" + ee.ToString() + "\r\n" + SubErr;
                                return (false);
                            }

                            RunningReciept.InstalledFiles.Add(RestartMove == true ? renamedfile : fullfilename);

                            if (RestartMove == true)
                            {
                                try
                                {
                                    Alphaleonis.Win32.Filesystem.File.CopyMoveCore(false, kt, renamedfile, null, true, null, Alphaleonis.Win32.Filesystem.MoveOptions.DelayUntilReboot, null, null, null, Alphaleonis.Win32.Filesystem.PathFormat.RelativePath);
                                    Alphaleonis.Win32.Filesystem.File.CopyMoveCore(false, kt, fullfilename, renamedfile, true, null, Alphaleonis.Win32.Filesystem.MoveOptions.DelayUntilReboot, null, null, null, Alphaleonis.Win32.Filesystem.PathFormat.RelativePath);

                                    lasterr = Marshal.GetLastWin32Error();
                                    if (lasterr != 0)
                                    {
                                        string SubErr;
                                        RollbackFiles(RunningPKG, out SubErr);
                                        kt.Rollback();
                                        ErrorText = "Cannot pending-move file " + fullfilename + " => " + renamedfile + " 0x" + lasterr.ToString("X") + "\r\n" + SubErr;
                                        return (false);
                                    }
                                }
                                catch (Exception ee)
                                {
                                    Debug.WriteLine(ee.ToString());

                                    UpdateText("Rolling back");

                                    string SubErr;
                                    RollbackFiles(RunningPKG, out SubErr);
                                    kt.Rollback();
                                    ErrorText = "Restart Move Error:\r\n" + ee.ToString() + "\r\n" + SubErr;
                                    return (false);
                                }
                            }
                        }

                        try
                        {
                            res = RunningPKG.script.PostInstall(RunningPKG);
                            if (res != PKGStatus.Success)
                            {
                                ErrorText = "PostInstall failed.";
                                UpdateText("Rolling back");
                                RollbackFiles(RunningPKG, out ErrorText);
                                kt.Rollback();
                                return (false);
                            }
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());

                            UpdateText("Rolling back");

                            string SubErr;
                            RollbackFiles(RunningPKG, out SubErr);
                            kt.Rollback();
                            ErrorText = "PostInstall Script Error:\r\n" + ee.ToString() + "\r\n" + SubErr;
                            return (false);
                        }

                        try
                        {
                            kt.Commit();
                        }
                        catch (Exception ee)
                        {
                            Debug.WriteLine(ee.ToString());

                            UpdateText("Rolling back");

                            string SubErr;
                            RollbackFiles(RunningPKG, out SubErr);
                            kt.Rollback();
                            ErrorText = "Commit Error:\r\n" + ee.ToString() + "\r\n" + SubErr;
                            return (false);
                        }
                    }

                    if (RunningPKG.NoReciept == false)
                        Reciept = RunningReciept;
                    else
                        Reciept = null;

                    //Clean up mess here
                    foreach (string file in DeleteFiles)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {
                            CommonUtilities.PendingMove(file, null);
                        }
                    }

                    res = PKGStatus.Success;
                }
            }
            return (true);
        }
    }
}
