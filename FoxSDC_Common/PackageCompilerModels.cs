using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
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

    public class PKGFile
    {
        public string ID;
        public string FolderName;
        public string FileName;
        public string SrcFile;
        public bool InstallThisFile;
        public bool KeepInMeta;
    }
}
