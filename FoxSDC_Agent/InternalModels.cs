using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    public class LoadedPolicyObject
    {
        public string Filename;
        public string SignFilename;
        public PolicyObject PolicyObject;
    }

    class DownloadQueueElement
    {
        public string URL;
        public string LocalFile;
        public object SupplementalData;
    }

    public class PackagesToInstall
    {
        public Int64 ID;
        public string PackageID;
        public Int64 Version;
        public bool Optional;
        public bool InstallUpdates;
        public string Title;
        /// <summary>
        /// Filename only, no path
        /// </summary>
        public string Filename;
        /// <summary>
        /// Filename only, no path
        /// </summary>
        public string MetaFilename;

        public override bool Equals(object obj)
        {
            if (!(obj is PackagesToInstall))
                return (false);
            return ((PackagesToInstall)obj == this);
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() ^ ID.GetHashCode() ^ PackageID.GetHashCode() ^ Version.GetHashCode());
        }

        public static bool operator ==(PackagesToInstall aa, PackagesToInstall bb)
        {
            if (aa is null)
                return (false);
            if (bb is null)
                return (false);
            if (aa.ID == bb.ID && aa.PackageID == bb.PackageID && aa.Version == bb.Version)
                return (true);

            return (false);
        }

        public static bool operator !=(PackagesToInstall a, PackagesToInstall b)
        {
            return (!(a == b));
        }
    }

    public class LocalPackageData
    {
        public string PackageID;
        public Int64 Version;
        public DateTime LastChecked;
        public string PKGRecieptFilename;
        public PKGStatus InstallStatus;
        public bool DownloadFailed;
        public bool Downloaded;
        public bool ServerHasPackage;
    }

    public class FileSystemTransferStatus
    {
        public Int64? ServerID;
        public Int64 Size;
        public string RemoteFileLocation;
        public Int64 ProgressSize;
        public bool OverrideMeteredConnection;
        public bool ExecuteWhenDone;
        public string MD5CheckSum;
        public int Direction;
        public bool RequestOnly;
        public DateTime LastModfied;
    }
}
