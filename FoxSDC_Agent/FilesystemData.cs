using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    public class FilesystemCertificateData
    {
        public byte[] Certificate;
        public string FSFilename;
    }

    public class FilesystemData
    {
        static public List<FilesystemCertificateData> LoadedCertificates;
        static public List<LoadedPolicyObject> LoadedPolicyObjects;
        static public List<PackagesToInstall> LocalPackages;
        static public List<LocalPackageData> LocalPackageDataList;
        static public List<PackageIDData> AvailableUserPackages;
        static public List<PackageIDData> UserPackagesToInstall;
        static public FileSystemTransferStatus FileTransferStatus;
        static public HashSet<string> SyncedEventLog;

        public static void WriteUserPackageList()
        {
            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                Directory.CreateDirectory(PackagesFolder);

#if DEBUG
            Formatting frm = Formatting.Indented;
#else
            Formatting frm = Formatting.None;
#endif
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(AvailableUserPackages, frm));
            File.WriteAllBytes(PackagesFolder + "AvailableUserPackages.json", data);
            byte[] sign = ApplicationCertificate.Sign(data);
            if (sign == null)
            {
                FoxEventLog.WriteEventLog("Cannot sign AV UserPackages data list for saving", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            else
            {
                File.WriteAllBytes(PackagesFolder + "AvailableUserPackages.sign", sign);
            }


            data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(UserPackagesToInstall, frm));
            File.WriteAllBytes(PackagesFolder + "UserPackagesToInstall.json", data);
            sign = ApplicationCertificate.Sign(data);
            if (sign == null)
            {
                FoxEventLog.WriteEventLog("Cannot sign user choosen package list for saving", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            else
            {
                File.WriteAllBytes(PackagesFolder + "UserPackagesToInstall.sign", sign);
            }
        }

        static public void LoadUserPackageData()
        {
            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                Directory.CreateDirectory(PackagesFolder);
            if (AvailableUserPackages == null)
                AvailableUserPackages = new List<PackageIDData>();
            if (UserPackagesToInstall == null)
                UserPackagesToInstall = new List<PackageIDData>();
            try
            {
                if (File.Exists(PackagesFolder + "AvailableUserPackages.json") == true && File.Exists(PackagesFolder + "AvailableUserPackages.sign") == true)
                {
                    byte[] list = File.ReadAllBytes(PackagesFolder + "AvailableUserPackages.json");
                    byte[] sign = File.ReadAllBytes(PackagesFolder + "AvailableUserPackages.sign");
                    if (ApplicationCertificate.Verify(list, sign) == false)
                    {
                        File.Delete(PackagesFolder + "AvailableUserPackages.json");
                        File.Delete(PackagesFolder + "AvailableUserPackages.sign");
                        FoxEventLog.WriteEventLog("AvailableUserPackages list signature is invalid: deleting the files!", EventLogEntryType.Warning);
                        AvailableUserPackages = new List<PackageIDData>();
                    }
                    else
                    {
                        AvailableUserPackages = JsonConvert.DeserializeObject<List<PackageIDData>>(Encoding.UTF8.GetString(list));
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Error decoding AvailableUserPackages", EventLogEntryType.Warning);
                AvailableUserPackages = new List<PackageIDData>();
            }

            try
            {
                if (File.Exists(PackagesFolder + "UserPackagesToInstall.json") == true && File.Exists(PackagesFolder + "UserPackagesToInstall.sign") == true)
                {
                    byte[] list = File.ReadAllBytes(PackagesFolder + "UserPackagesToInstall.json");
                    byte[] sign = File.ReadAllBytes(PackagesFolder + "UserPackagesToInstall.sign");
                    if (ApplicationCertificate.Verify(list, sign) == false)
                    {
                        File.Delete(PackagesFolder + "UserPackagesToInstall.json");
                        File.Delete(PackagesFolder + "UserPackagesToInstall.sign");
                        FoxEventLog.WriteEventLog("UserPackagesToInstall list signature is invalid: deleting the files!", EventLogEntryType.Warning);
                        UserPackagesToInstall = new List<PackageIDData>();
                    }
                    else
                    {
                        UserPackagesToInstall = JsonConvert.DeserializeObject<List<PackageIDData>>(Encoding.UTF8.GetString(list));
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Error decoding UserPackagesToInstall", EventLogEntryType.Warning);
                UserPackagesToInstall = new List<PackageIDData>();
            }
        }

        public static bool CanInstallOptionalPackage(PackagesToInstall pkg)
        {
            foreach (PackageIDData pp in UserPackagesToInstall)
            {
                if (pp.PackageID.ToLower() == pkg.PackageID.ToLower())
                    return (true);
            }
            return (false);
        }

        public static void WritePackageDataList()
        {
            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                Directory.CreateDirectory(PackagesFolder);
#if DEBUG
            Formatting frm = Formatting.Indented;
#else
            Formatting frm = Formatting.None;
#endif
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(LocalPackageDataList, frm));
            File.WriteAllBytes(PackagesFolder + "Install.json", data);
            byte[] sign = ApplicationCertificate.Sign(data);
            if (sign == null)
            {
                FoxEventLog.WriteEventLog("Cannot sign package data list for saving", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            else
            {
                File.WriteAllBytes(PackagesFolder + "Install.sign", sign);
            }
        }

        static public void LoadLocalPackageData()
        {
            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                Directory.CreateDirectory(PackagesFolder);
            if (LocalPackageDataList == null)
                LocalPackageDataList = new List<LocalPackageData>();
            try
            {
                if (File.Exists(PackagesFolder + "Install.json") == true && File.Exists(PackagesFolder + "Install.sign") == true)
                {
                    byte[] list = File.ReadAllBytes(PackagesFolder + "Install.json");
                    byte[] sign = File.ReadAllBytes(PackagesFolder + "Install.sign");
                    if (ApplicationCertificate.Verify(list, sign) == false)
                    {
                        File.Delete(PackagesFolder + "Install.json");
                        File.Delete(PackagesFolder + "Install.sign");
                        FoxEventLog.WriteEventLog("Package data list signature is invalid: deleting the files!", EventLogEntryType.Warning);
                        LocalPackageDataList = new List<LocalPackageData>();
                    }
                    else
                    {
                        LocalPackageDataList = JsonConvert.DeserializeObject<List<LocalPackageData>>(Encoding.UTF8.GetString(list));
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Error decoding package data list data", EventLogEntryType.Warning);
                LocalPackages = new List<PackagesToInstall>();
            }
        }

        static public List<LocalPackageData> FindLocalPackageFromList(string PackageID)
        {
            List<LocalPackageData> l = new List<LocalPackageData>();
            if (LocalPackageDataList == null)
                LocalPackageDataList = new List<LocalPackageData>();
            foreach (LocalPackageData pp in LocalPackageDataList)
            {
                if (pp.PackageID.ToLower() == PackageID.ToLower())
                {
                    l.Add(pp);
                }
            }
            return (l);
        }

        static public LocalPackageData FindLocalPackageFromList(string PackageID, long Version)
        {
            if (LocalPackageDataList == null)
                LocalPackageDataList = new List<LocalPackageData>();
            foreach (LocalPackageData pp in LocalPackageDataList)
            {
                if (pp.PackageID.ToLower() == PackageID.ToLower() && pp.Version == Version)
                {
                    return (pp);
                }
            }
            return (null);
        }

        static public LocalPackageData FindLocalPackageFromListLatest(string PackageID)
        {
            if (LocalPackageDataList == null)
                LocalPackageDataList = new List<LocalPackageData>();
            List<LocalPackageData> l = FindLocalPackageFromList(PackageID);
            LocalPackageData latest = null;
            foreach (LocalPackageData ll in l)
            {
                if (latest == null)
                {
                    latest = ll;
                }
                else
                {
                    if (ll.Version > latest.Version)
                        latest = ll;
                }
            }
            return (latest);
        }

        static public void AddUpdateLocalPackage(LocalPackageData pkg)
        {
            LocalPackageData currentpkg = null;
            foreach (LocalPackageData p in LocalPackageDataList)
            {
                if (p.Version == pkg.Version && p.PackageID.ToLower() == pkg.PackageID.ToLower())
                {
                    currentpkg = p;
                    break;
                }
            }
            bool AddNeeded = false;
            if (currentpkg == null)
            {
                AddNeeded = true;
                currentpkg = new LocalPackageData();
            }

            currentpkg.InstallStatus = pkg.InstallStatus;
            currentpkg.LastChecked = pkg.LastChecked;
            currentpkg.PackageID = pkg.PackageID;
            if (!(pkg.PKGRecieptFilename == null && currentpkg.PKGRecieptFilename != null))
                currentpkg.PKGRecieptFilename = pkg.PKGRecieptFilename;
            currentpkg.Version = pkg.Version;
            currentpkg.ServerHasPackage = pkg.ServerHasPackage;
            if (AddNeeded == true)
                LocalPackageDataList.Add(currentpkg);
        }

        static public List<byte[]> LoadedCertificatesArray
        {
            get
            {
                List<byte[]> b = new List<byte[]>();
                if (LoadedCertificates == null)
                    return (null);
                foreach (FilesystemCertificateData cer in LoadedCertificates)
                    b.Add(cer.Certificate);
                return (b);
            }
        }

        public static void WritePackageList()
        {
            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                Directory.CreateDirectory(PackagesFolder);
#if DEBUG
            Formatting frm = Formatting.Indented;
#else
            Formatting frm = Formatting.None;
#endif
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(LocalPackages, frm));
            File.WriteAllBytes(PackagesFolder + "List.json", data);
            byte[] sign = ApplicationCertificate.Sign(data);
            if (sign == null)
            {
                FoxEventLog.WriteEventLog("Cannot sign package list for saving", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            else
            {
                File.WriteAllBytes(PackagesFolder + "List.sign", sign);
            }
        }

        public static void WriteFileTransferStatus()
        {
            if (Directory.Exists(SystemInfos.ProgramData) == false)
                Directory.CreateDirectory(SystemInfos.ProgramData);
#if DEBUG
            Formatting frm = Formatting.Indented;
#else
            Formatting frm = Formatting.None;
#endif
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(FileTransferStatus, frm));
            File.WriteAllBytes(SystemInfos.ProgramData + "FileTransfer.json", data);
            byte[] sign = ApplicationCertificate.Sign(data);
            if (sign == null)
            {
                FoxEventLog.WriteEventLog("Cannot sign File Transfer status file for saving", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            else
            {
                File.WriteAllBytes(SystemInfos.ProgramData + "FileTransfer.sign", sign);
            }
        }

        public static void LoadFileTransferStatus()
        {
            if (FileTransferStatus == null)
                FileTransferStatus = new FileSystemTransferStatus();
            try
            {
                if (File.Exists(SystemInfos.ProgramData + "FileTransfer.sign") == true && File.Exists(SystemInfos.ProgramData + "FileTransfer.json") == true)
                {
                    byte[] list = File.ReadAllBytes(SystemInfos.ProgramData + "FileTransfer.json");
                    byte[] sign = File.ReadAllBytes(SystemInfos.ProgramData + "FileTransfer.sign");
                    if (ApplicationCertificate.Verify(list, sign) == false)
                    {
                        File.Delete(SystemInfos.ProgramData + "FileTransfer.json");
                        File.Delete(SystemInfos.ProgramData + "FileTransfer.sign");
                        FoxEventLog.WriteEventLog("File Transfer status file signature is invalid: deleting the files!", EventLogEntryType.Warning);
                        FileTransferStatus = new FileSystemTransferStatus();
                    }
                    else
                    {
                        FileTransferStatus = JsonConvert.DeserializeObject<FileSystemTransferStatus>(Encoding.UTF8.GetString(list));
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("File Transfer status file", EventLogEntryType.Warning);
                SyncedEventLog = new HashSet<string>();
            }
        }

        public static void WriteEventLogList()
        {
            if (Directory.Exists(SystemInfos.ProgramData) == false)
                Directory.CreateDirectory(SystemInfos.ProgramData);
#if DEBUG
            Formatting frm = Formatting.Indented;
#else
            Formatting frm = Formatting.None;
#endif
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(SyncedEventLog, frm));
            File.WriteAllBytes(SystemInfos.ProgramData + "SyncedEventLog.json", data);
            byte[] sign = ApplicationCertificate.Sign(data);
            if (sign == null)
            {
                FoxEventLog.WriteEventLog("Cannot sign synced Event Log for saving", System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            else
            {
                File.WriteAllBytes(SystemInfos.ProgramData + "SyncedEventLog.sign", sign);
            }
        }

        public static void LoadEventLogList()
        {
            if (SyncedEventLog == null)
                SyncedEventLog = new HashSet<string>();
            try
            {
                if (File.Exists(SystemInfos.ProgramData + "SyncedEventLog.sign") == true && File.Exists(SystemInfos.ProgramData + "SyncedEventLog.json") == true)
                {
                    byte[] list = File.ReadAllBytes(SystemInfos.ProgramData + "SyncedEventLog.json");
                    byte[] sign = File.ReadAllBytes(SystemInfos.ProgramData + "SyncedEventLog.sign");
                    if (ApplicationCertificate.Verify(list, sign) == false)
                    {
                        File.Delete(SystemInfos.ProgramData + "SyncedEventLog.json");
                        File.Delete(SystemInfos.ProgramData + "SyncedEventLog.sign");
                        FoxEventLog.WriteEventLog("Event Log List signature is invalid: deleting the files!", EventLogEntryType.Warning);
                        SyncedEventLog = new HashSet<string>();
                    }
                    else
                    {
                        SyncedEventLog = JsonConvert.DeserializeObject<HashSet<string>>(Encoding.UTF8.GetString(list));
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Error decoding Event Log List", EventLogEntryType.Warning);
                SyncedEventLog = new HashSet<string>();
            }
        }

        static public void RegisterPackage(PackagesToInstall Package)
        {
            if (LocalPackages == null)
                LocalPackages = new List<PackagesToInstall>();
            LocalPackages.Add(Package);
            WritePackageList();
        }

        static public bool HasLocalPackage(PackagesToInstall Package)
        {
            foreach (PackagesToInstall pp in LocalPackages)
            {
                if (Package == pp)
                    return (true);
            }
            return (false);
        }

        static public void RemovePackage(PackagesToInstall Package, bool ForceRemove = false)
        {
            if (LocalPackages == null)
                return;
            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                Directory.CreateDirectory(PackagesFolder);

            PackagesToInstall pkg = null;
            foreach (PackagesToInstall pp in LocalPackages)
            {
                if (Package == pp)
                {
                    pkg = pp;
                    break;
                }
            }
            if (pkg == null)
                return;

            LocalPackageData rmlpkg = null;
            foreach (LocalPackageData lpkg in LocalPackageDataList)
            {
                if (lpkg.PackageID == pkg.PackageID && lpkg.Version == pkg.Version)
                {
                    rmlpkg = lpkg;
                    break;
                }
            }

            if (ForceRemove == false)
            {
                if (rmlpkg != null)
                {
                    if (rmlpkg.PKGRecieptFilename != null)
                    {
                        FoxEventLog.VerboseWriteEventLog("Not removing Package: " + pkg.PackageID + " V" + pkg.Version.ToString() + ". Reciept file is registred.", EventLogEntryType.Warning);
                        if (pkg.Filename != null)
                        {
                            if (File.Exists(PackagesFolder + pkg.Filename) == true)
                                CommonUtilities.SpecialDeleteFile(PackagesFolder + pkg.Filename);
                            pkg.Filename = null;
                        }
                        rmlpkg.ServerHasPackage = false;
                        return;
                    }
                }
            }

            if (pkg.MetaFilename != null)
                if (File.Exists(PackagesFolder + pkg.MetaFilename) == true)
                    CommonUtilities.SpecialDeleteFile(PackagesFolder + pkg.MetaFilename);
            if (pkg.Filename != null)
                if (File.Exists(PackagesFolder + pkg.Filename) == true)
                    CommonUtilities.SpecialDeleteFile(PackagesFolder + pkg.Filename);
            LocalPackages.Remove(pkg);
            WritePackageList();

            if (rmlpkg != null)
            {
                if (rmlpkg.PKGRecieptFilename != null)
                {
                    CommonUtilities.SpecialDeleteFile(PackagesFolder + rmlpkg.PKGRecieptFilename);
                    CommonUtilities.SpecialDeleteFile(PackagesFolder + rmlpkg.PKGRecieptFilename + ".sign");
                }
                LocalPackageDataList.Remove(rmlpkg);
                WritePackageDataList();
            }
        }

        static public void LoadLocalPackages()
        {
            string PackagesFolder = SystemInfos.ProgramData + "Packages\\";
            if (Directory.Exists(PackagesFolder) == false)
                Directory.CreateDirectory(PackagesFolder);
            if (LocalPackages == null)
                LocalPackages = new List<PackagesToInstall>();
            try
            {
                if (File.Exists(PackagesFolder + "List.json") == true && File.Exists(PackagesFolder + "List.sign") == true)
                {
                    byte[] list = File.ReadAllBytes(PackagesFolder + "List.json");
                    byte[] sign = File.ReadAllBytes(PackagesFolder + "List.sign");
                    if (ApplicationCertificate.Verify(list, sign) == false)
                    {
                        File.Delete(PackagesFolder + "List.json");
                        File.Delete(PackagesFolder + "List.sign");
                        FoxEventLog.WriteEventLog("Package list signature is invalid: deleting the files!", EventLogEntryType.Warning);
                        LocalPackages = new List<PackagesToInstall>();
                    }
                    else
                    {
                        LocalPackages = JsonConvert.DeserializeObject<List<PackagesToInstall>>(Encoding.UTF8.GetString(list));
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Error decoding package list data", EventLogEntryType.Warning);
                LocalPackages = new List<PackagesToInstall>();
            }

            try
            {
                foreach (PackagesToInstall pkg in LocalPackages)
                {
                    if (pkg.Filename != null)
                    {
                        if (File.Exists(PackagesFolder + pkg.Filename) == false)
                        {
                            FoxEventLog.WriteEventLog("Referenced package " + pkg.Filename + " does not exist", EventLogEntryType.Warning);
                            pkg.Filename = null;
                        }
                    }
                    if (pkg.MetaFilename != null)
                    {
                        if (File.Exists(PackagesFolder + pkg.MetaFilename) == false)
                        {
                            FoxEventLog.WriteEventLog("Referenced meta package " + pkg.Filename + " does not exist", EventLogEntryType.Warning);
                            pkg.MetaFilename = null;
                        }
                    }
                }

                List<PackagesToInstall> RemovePackages = new List<PackagesToInstall>();
                foreach (PackagesToInstall pkg in LocalPackages)
                {
                    if (pkg.MetaFilename == null)
                        RemovePackages.Add(pkg);
                }

                foreach (PackagesToInstall pkg in RemovePackages)
                {
                    LocalPackages.Remove(pkg);
                    if (pkg.MetaFilename != null)
                        File.Delete(PackagesFolder + pkg.MetaFilename);
                    if (pkg.Filename != null)
                        File.Delete(PackagesFolder + pkg.Filename);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                FoxEventLog.WriteEventLog("Error processing package list data", EventLogEntryType.Error);
                LocalPackages = new List<PackagesToInstall>();
            }
        }


        static public bool ContainsLoadedCert(byte[] cerdata)
        {
            if (LoadedCertificates == null)
                LoadedCertificates = new List<FilesystemCertificateData>();
            bool FoundCert = false;
            foreach (FilesystemCertificateData data in LoadedCertificates)
            {
                if (data.Certificate.SequenceEqual(cerdata) == true)
                {
                    FoundCert = true;
                    break;
                }
            }

            return (FoundCert);
        }

        static public bool ContainsPolicy(PolicyObject obj, bool CheckData)
        {
            if (LoadedPolicyObjects == null)
                LoadedPolicyObjects = new List<LoadedPolicyObject>();

            bool ElementFound = false;
            foreach (LoadedPolicyObject pol in LoadedPolicyObjects)
            {
                if (pol.PolicyObject.ID != obj.ID)
                    continue;
                if (pol.PolicyObject.Name != obj.Name)
                    continue;
                if (pol.PolicyObject.Type != obj.Type)
                    continue;
                if (pol.PolicyObject.Version != obj.Version)
                    continue;
                if (pol.PolicyObject.DT != obj.DT)
                    continue;
                if (CheckData == true)
                {
                    if (pol.PolicyObject.Data != obj.Data)
                        continue;
                }
                ElementFound = true;
                pol.Processed = true;
                break;
            }

            return (ElementFound);
        }

        static public bool ReOrderPolicies()
        {
            LoadedPolicyObjects.Sort((x, y) => x.PolicyObject.Order.CompareTo(y.PolicyObject.Order));
            return (true);
        }

        static public bool UpdatePolicyOrder(PolicyObject obj, Int64 Order)
        {
            if (LoadedPolicyObjects == null)
                LoadedPolicyObjects = new List<LoadedPolicyObject>();

            foreach (LoadedPolicyObject pol in LoadedPolicyObjects)
            {
                if (pol.PolicyObject.ID != obj.ID)
                    continue;
                if (pol.PolicyObject.Name != obj.Name)
                    continue;
                if (pol.PolicyObject.Type != obj.Type)
                    continue;
                if (pol.PolicyObject.Version != obj.Version)
                    continue;
                if (pol.PolicyObject.DT != obj.DT)
                    continue;
                pol.PolicyObject.Order = Order;

                byte[] ppol = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pol.PolicyObject));
                byte[] sign = ApplicationCertificate.Sign(ppol);
                if (sign == null)
                {
                    FoxEventLog.WriteEventLog("Cannot sign policy for saving", System.Diagnostics.EventLogEntryType.Error);
                    return (false);
                }


                try
                {
                    File.WriteAllBytes(pol.Filename, ppol);
                }
                catch
                {
                    FoxEventLog.WriteEventLog("Cannot save policy", System.Diagnostics.EventLogEntryType.Error);
                    return (false);
                }

                try
                {
                    File.WriteAllBytes(pol.SignFilename, sign);
                }
                catch
                {
                    try
                    {
                        File.Delete(pol.Filename);
                    }
                    catch
                    {

                    }
                    FoxEventLog.WriteEventLog("Cannot save policy signature", System.Diagnostics.EventLogEntryType.Error);
                    return (false);
                }

                break;
            }
            return (true);
        }

        static public void ResetProcessedPolicyObjectStatus()
        {
            foreach (LoadedPolicyObject pol in LoadedPolicyObjects)
                pol.Processed = false;
        }

        static public bool InstallCertificate(byte[] data)
        {
            if (ContainsLoadedCert(data) == true)
                return (true);

            string CertFolder = SystemInfos.ProgramData + "Certificates\\";
            FilesystemCertificateData cer = new FilesystemCertificateData();

            for (int i = 1; i < 100; i++)
            {
                if (File.Exists(CertFolder + "Certificate" + i.ToString("00") + ".cer") == false)
                {
                    File.WriteAllBytes(CertFolder + "Certificate" + i.ToString("00") + ".cer", data);
                    byte[] d = ApplicationCertificate.Sign(data);
                    if (d != null)
                        File.WriteAllBytes(CertFolder + "Certificate" + i.ToString("00") + ".sign", d);
                    FoxEventLog.WriteEventLog("Certificate " + Certificates.GetCN(data) + " installed as ID=" + i.ToString("00"), System.Diagnostics.EventLogEntryType.Information);
                    cer.FSFilename = "Certificate" + i.ToString("00") + ".cer";
                    cer.Certificate = data;
                    break;
                }
            }

            LoadedCertificates.Add(cer);
            FoxEventLog.WriteEventLog("Certificate " + Certificates.GetCN(data) + " loaded", System.Diagnostics.EventLogEntryType.Information);

            return (true);
        }

        static public bool InstallPolicy(PolicyObject data, Int64 Order)
        {
            if (data == null)
                return (false);
            data.Order = Order;
            string PoliciesFolder = SystemInfos.ProgramData + "Policies\\";
            string Filename = data.ID.ToString("X8") + "-" + Guid.NewGuid().ToString();

            byte[] pol = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            byte[] sign = ApplicationCertificate.Sign(pol);
            if (sign == null)
            {
                FoxEventLog.WriteEventLog("Cannot sign policy for saving", System.Diagnostics.EventLogEntryType.Error);
                return (false);
            }

            LoadedPolicyObject lobj = new LoadedPolicyObject();
            lobj.PolicyObject = data;
            lobj.Filename = PoliciesFolder + Filename + ".pol";
            lobj.SignFilename = PoliciesFolder + Filename + ".sign";
            lobj.Processed = true;

            try
            {
                File.WriteAllBytes(lobj.Filename, pol);
            }
            catch
            {
                FoxEventLog.WriteEventLog("Cannot save policy", System.Diagnostics.EventLogEntryType.Error);
                return (false);
            }

            try
            {
                File.WriteAllBytes(lobj.SignFilename, sign);
            }
            catch
            {
                try
                {
                    File.Delete(lobj.Filename);
                }
                catch
                {

                }
                FoxEventLog.WriteEventLog("Cannot save policy signature", System.Diagnostics.EventLogEntryType.Error);
                return (false);
            }

            LoadedPolicyObjects.Add(lobj);

            return (true);
        }

        static public bool DeletePolicy(LoadedPolicyObject obj)
        {
            LoadedPolicyObjects.Remove(obj);
            try
            {
                File.Delete(obj.Filename);
            }
            catch
            {

            }
            try
            {
                File.Delete(obj.SignFilename);
            }
            catch
            {

            }
            Debug.WriteLine(obj.Filename + " deleted");
            Debug.WriteLine(obj.SignFilename + " deleted");

            return (true);
        }

        static public bool LoadPolicies()
        {
            LoadedPolicyObjects = new List<LoadedPolicyObject>();
            string PoliciesFolder = SystemInfos.ProgramData + "Policies\\";
            if (Directory.Exists(PoliciesFolder) == false)
                Directory.CreateDirectory(PoliciesFolder);

            foreach (string file in Directory.EnumerateFiles(PoliciesFolder, "*.pol", SearchOption.TopDirectoryOnly))
            {
                string signfile = file.Substring(0, file.Length - 4) + ".sign";

                if (File.Exists(file) == false)
                {
                    if (File.Exists(signfile) == true)
                    {
                        FoxEventLog.WriteEventLog("Found lonely file \"" + file + "\" - deleting the file", System.Diagnostics.EventLogEntryType.Warning);
                        File.Delete(signfile);
                    }
                }
                else
                {
                    if (File.Exists(signfile) == false)
                    {
                        FoxEventLog.WriteEventLog("Found \"" + file + "\" but no signature - deleting the file", System.Diagnostics.EventLogEntryType.Warning);
                        File.Delete(file);
                    }
                    else
                    {
                        FileInfo fileinfo;
                        fileinfo = new FileInfo(file);
                        if (fileinfo.Length > 33554432)
                        {
                            FoxEventLog.WriteEventLog("File \"" + file + "\" too large (>32MB) - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(file);
                            File.Delete(signfile);
                            continue;
                        }
                        fileinfo = new FileInfo(signfile);
                        if (fileinfo.Length > 33554432)
                        {
                            FoxEventLog.WriteEventLog("File \"" + signfile + "\" too large (>32MB) - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(file);
                            File.Delete(signfile);
                            continue;
                        }

                        byte[] pol = File.ReadAllBytes(file);
                        byte[] sign = File.ReadAllBytes(signfile);

                        if (ApplicationCertificate.Verify(pol, sign) == false)
                        {
                            FoxEventLog.WriteEventLog("File \"" + file + "\" is not proper signed - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(file);
                            File.Delete(signfile);
                            continue;
                        }

                        try
                        {
                            PolicyObject obj = JsonConvert.DeserializeObject<PolicyObject>(Encoding.UTF8.GetString(pol));
                            if (ContainsPolicy(obj, true) == true)
                            {
                                FoxEventLog.WriteEventLog("File \"" + file + "\" is already loaded from a different file - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                                File.Delete(file);
                                File.Delete(signfile);
                                continue;
                            }
                            LoadedPolicyObject lobj = new LoadedPolicyObject();
                            lobj.PolicyObject = obj;
                            lobj.Filename = file;
                            lobj.SignFilename = signfile;
                            lobj.Processed = false;
                            LoadedPolicyObjects.Add(lobj);
                            Debug.WriteLine(file + " loaded");
                        }
                        catch
                        {
                            FoxEventLog.WriteEventLog("File \"" + file + "\" cannot be loaded properly - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(file);
                            File.Delete(signfile);
                            continue;
                        }
                    }
                }
            }

            return (true);
        }

        static public bool LoadCertificates(bool Shutup = false)
        {
            LoadedCertificates = new List<FilesystemCertificateData>();
            string CertFolder = SystemInfos.ProgramData + "Certificates\\";
            if (Directory.Exists(CertFolder) == false)
                Directory.CreateDirectory(CertFolder);

            for (int i = 1; i < 100; i++)
            {
                if (File.Exists(CertFolder + "Certificate" + i.ToString("00") + ".cer") == false)
                {
                    if (File.Exists(CertFolder + "Certificate" + i.ToString("00") + ".sign") == true)
                    {
                        FoxEventLog.WriteEventLog("Found lonely file \"Certificate" + i.ToString("00") + ".sign\" - deleting the file", System.Diagnostics.EventLogEntryType.Warning);
                        File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".sign");
                    }
                }
                else
                {
                    if (File.Exists(CertFolder + "Certificate" + i.ToString("00") + ".sign") == false)
                    {
                        FoxEventLog.WriteEventLog("Found \"Certificate" + i.ToString("00") + ".cer\" but no signature - deleting the file", System.Diagnostics.EventLogEntryType.Warning);
                        File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".cer");
                    }
                    else
                    {
                        FileInfo file;
                        file = new FileInfo(CertFolder + "Certificate" + i.ToString("00") + ".cer");
                        if (file.Length > 5242880)
                        {
                            FoxEventLog.WriteEventLog("File \"Certificate" + i.ToString("00") + ".cer\" too large (>5MB) - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".cer");
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".sign");
                            continue;
                        }
                        file = new FileInfo(CertFolder + "Certificate" + i.ToString("00") + ".sign");
                        if (file.Length > 5242880)
                        {
                            FoxEventLog.WriteEventLog("File \"Certificate" + i.ToString("00") + ".sign\" too large (>5MB) - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".cer");
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".sign");
                            continue;
                        }

                        byte[] cert = File.ReadAllBytes(CertFolder + "Certificate" + i.ToString("00") + ".cer");
                        byte[] sign = File.ReadAllBytes(CertFolder + "Certificate" + i.ToString("00") + ".sign");

                        if (ApplicationCertificate.Verify(cert, sign) == false)
                        {
                            FoxEventLog.WriteEventLog("File \"Certificate" + i.ToString("00") + ".cer\" and \"Certificate" + i.ToString("00") + ".sign\" do not match - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".cer");
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".sign");
                            continue;
                        }

                        if (ContainsLoadedCert(cert) == true)
                        {
                            FoxEventLog.WriteEventLog("File \"Certificate" + i.ToString("00") + ".cer\" this certificate is already loaded from a different file - deleting the files", System.Diagnostics.EventLogEntryType.Warning);
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".cer");
                            File.Delete(CertFolder + "Certificate" + i.ToString("00") + ".sign");
                            continue;
                        }

                        Debug.WriteLine(CertFolder + "Certificate" + i.ToString("00") + ".cer loaded");
                        FilesystemCertificateData fscer = new FilesystemCertificateData();
                        fscer.FSFilename = "Certificate" + i.ToString("00") + ".cer";
                        fscer.Certificate = cert;
                        LoadedCertificates.Add(fscer);
                        if (Shutup == false)
                            FoxEventLog.WriteEventLog("Certificate " + Certificates.GetCN(cert) + " loaded", System.Diagnostics.EventLogEntryType.Information);
                    }
                }
            }

            return (true);
        }
    }
}
