using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class NetString
    {
        public string Data;
    }

    public class NetDictIntString
    {
        public Dictionary<int, string> Dict;
    }

    public class NetStringList
    {
        public List<string> Items;
    }

    public class NetBool
    {
        public bool Data;
    }

    public class NetByte
    {
        public byte[] Data;
    }

    public class NetInt64
    {
        public Int64 Data;
    }

    public class NetInt32
    {
        public Int32 Data;
    }

    public class NetInt64Null
    {
        public Int64? Data;
    }

    public class NewUploadDataID
    {
        public Int64 NewID;
        public int FileType;
    }

    public class UploadData
    {
        public Int64 Counter;
        public Int64 Size;
        public byte[] data;
        public string MD5;
        public bool Done;
    }

    public class ServerSettings
    {
        public string UseCertificate;
        public Int64 KeepEventLogDays;
        public Int64 KeepBitlockerRK;
        public Int64 KeepNonPresentDisks;
        public Int64 KeepReports;
        public Int64 KeepChatLogs;

        public string EMailServer;
        public int EMailPort;
        public bool EMailUseSSL;
        public string EMailFrom;
        public string EMailFromFriendly;
        public string EMailAdminTo;
        public string EMailUsername;
        public string EMailPassword;

        public string EMailAdminText;
        public string EMailClientText;
        public bool EMailAdminIsHTML;
        public bool EMailClientIsHTML;
        public string EMailAdminSubject;
        public string EMailClientSubject;

        public SchedulerPlanning EMailAdminScheduling;
        public SchedulerPlanning EMailClientScheduling;

        public DateTime? LastScheduleRanClient;
        public DateTime? LastScheduleRanAdmin;

        public string AdminIPAddresses;
        public string AdministratorName;
        public string MessageDisclaimer;
    }

    public class ClientSettings
    {
        public string AdministratorName;
        public string MessageDisclaimer;
    }

    public class UploadRequest
    {
        /// <summary>
        /// 0 - ????
        /// </summary>
        public int FileType;
        public Int64 FileSize;
        public string TempName;
    }

    public class ServerInfo
    {
        public string Name;
        public bool InitSuccess;
        public int ProtocolVersion;
        public Int64 Permissions;
        public string ServerVersion;
        public string SQLServer;
        public string SQLService;
        public string SQLProductVersion;
        public string SQLProductLevel;
        public string SQLEdition;
        public string SQLCollation;
        public string SQLProductName;
        public string ServerGUID;

        public DateTime LicValidFrom;
        public DateTime? LicValidTo;
        public DateTime? LicSupportValidTo;
        public string LicLicenseID;
        public string LicLicenseType;
        public string LicFeatures;
        public string LicUCID;
        public string LicOwner;
        public string LicOwnerCustomID;

        public string LicVacant1;
        public string LicVacant2;
        public string LicVacant3;
        public string LicVacant4;
        public string LicVacant5;

    }

    public class ChangePassword
    {
        public string OldPassword;
        public string NewPassword;
    }

    public class Logon
    {
        public string Username;
        public string Password;
    }

    public class ErrorInfo
    {
        public string Error;
        public int ErrorID;
    }

    public class BaseSystemInfo
    {
        public bool Is64Bit;
        public string OSName;
        public int OSVerMaj;
        public int OSVerMin;
        public int OSVerBuild;
        public int OSVerType;
        public string OSSuite;
        public bool IsTSE;
        public string CPU;
        public string ComputerModel;
        public string ComputerName;
        public string MachineID;
        public string Language;
        public string DisplayLanguage;
        public string UCID;
        public string LegacyUCID;
        public string AgentVersion;
        public Int64 AgentVersionID;
        public bool RunningInHypervisor;
        public string BIOS;
        public string BIOSType;
        public int NumberOfLogicalProcessors;
        public int NumberOfProcessors;
        public Int64 TotalPhysicalMemory;
        public string CPUName;
        public string SecureBootState;
        public string SystemRoot;
        public string SUSID;
        public bool? IsMeteredConnection;
        public bool? RunningInWindowsPE;
    }

    public class ComputerLogon
    {
        public string Username;
        public string Password;
        public string ContractID;
        public string ContractPassword;
        public BaseSystemInfo SysInfo;
    }

    public class RecoveryLogon
    {
        public string ContractID;
        public string ContractPassword;
        public string UCID;
        public string MoreMachineHash;
    }

    public class RecoveryData
    {
        public bool Worked;
        public string MachineID;
        public string MachinePassword;
    }

    public class ContractInfosList
    {
        public List<ContractInfos> Items;
    }

    public class ContractInfos
    {
        public string ContractID;
        public string ContractPassword;
        public DateTime? ValidFrom;
        public DateTime? ValidTo;
        public int? MaxComputers;
        public decimal? PricePerComputer;
        public decimal? PriceTotal;
        public int? Vacant1;
        public int? Vacant2;
        public int? Vacant3;
        public int? Vacant4;
        public int? Vacant5;
        public string Vacant6;
        public string Vacant7;
        public string Vacant8;
        public string Vacant9;
        public string Vacant10;
        public decimal? Vacant11;
        public decimal? Vacant12;
        public DateTime? Vacant13;
        public DateTime? Vacant14;
        public bool Disabled;
        public string EMail;

        public List<ComputerData> IncludedComputers;
    }

    public class ComputerData
    {
        public string MachineID;
        public string UCID;
        public string Computername;
        public string Comments;
        public string OS;
        public string OSVersion;
        public string OSSuite;
        public int OSVerType;
        public string Make;
        public string Language;
        public string CPU;
        public DateTime LastUpdated;
        public bool IsTSE;
        public bool Is64Bit;
        public bool Approved;
        public string GroupingPath;
        public string AgentVersion;
        public Int64 AgentVersionID;
        public bool RunningInHypervisor;
        public string BIOS;
        public string ContractID;
        public string IPAddress;
        public string BIOSType;
        public int NumberOfLogicalProcessors;
        public int NumberOfProcessors;
        public Int64 TotalPhysicalMemory;
        public string CPUName;
        public string SecureBootState;
        public string SystemRoot;
        public string SUSID;
        public bool? IsMeteredConnection;
        public bool? RunningInWindowsPE;
    }

    public class ComputerDataList
    {
        public List<ComputerData> List;
    }

    public class ApproveComputer
    {
        public bool State;
        public Int64 Group;
    }

    public class GroupElementList
    {
        public List<GroupElement> List;
    }

    public class GroupElement
    {
        public Int64 ID;
        public string Name;
        public Int64? ParentID;
    }

    public class CreateGroup
    {
        public Int64? ToParent;
        public string Name;
    }

    public class AddRemoveApp
    {
        public string MachineID;
        public string ProductID;
        public bool IsWOWBranch;
        public bool IsMSI;
        public bool IsSystemComponent;
        public string Name;
        public string DisplayVersion;
        public string UninstallString;
        public int VersionMajor;
        public int VersionMinor;
        public string Language;
        public string DisplayLanguage;
        public string HKCUUser;
    }

    public class ListAddRemoveApps
    {
        public List<AddRemoveApp> Items;
        public string MachineID;
        public List<string> SIDUsers;
    }

    public class ListAddRemoveAppsReport
    {
        public List<AddRemoveAppReport> Items;
    }

    public class ListStartupItems
    {
        public List<StartupItem> Items;
        public string MachineID;
        public List<string> SIDUsers;
    }

    public class StartupItem
    {
        public string Key;
        public string Item;
        public string Location;
        public string HKCUUser;
    }

    public class StartupItemFull
    {
        public string Computername;
        public string MachineID;
        public string Key;
        public string Item;
        public string Location;
        public string HKCUUser;
        public string Username;
        public DateTime DT;
    }

    public class ListStartupItemReport
    {
        public List<StartupItemFull> Items;
    }

    public class ListEventLogReport
    {
        public List<EventLogReport> Items;
        public string MachineID;
    }

    public class EventLogReport
    {
        public string MachineID;
        public string LogID;
        public string EventLog;
        public string Source;
        public string Category;
        public int CategoryNumber;
        public int EventLogType;
        public Int64 InstanceID;
        public DateTime TimeGenerated;
        public DateTime TimeWritten;
        public string JSONReplacementStrings;
        public byte[] Data;
        public string Message;
    }

    public class EventLogReportFull
    {
        public string MachineID;
        public string LogID;
        public string EventLog;
        public string Source;
        public string Category;
        public int CategoryNumber;
        public int EventLogType;
        public Int64 InstanceID;
        public DateTime TimeGenerated;
        public DateTime TimeWritten;
        public DateTime Reported;
        public string JSONReplacementStrings;
        public byte[] Data;
        public string Message;
    }

    public class EventLogReportFullList
    {
        public List<EventLogReportFull> Data;
    }

    public class AddRemoveAppReport
    {
        public string MachineID;
        public string Computername;
        public string ProductID;
        public bool IsWOWBranch;
        public bool IsMSI;
        public bool IsSystemComponent;
        public string Name;
        public string DisplayVersion;
        public string UninstallString;
        public int VersionMajor;
        public int VersionMinor;
        public string Language;
        public string DisplayLanguage;
        public DateTime DT;
        public string HKCUUser;
        public string Username;
    }

    public class NewPolicyReq
    {
        public string Name;
        public int Type;
        public string MachineID;
        public Int64? Grouping;
        public string Data;
    }

    public class EditPolicy
    {
        public Int64 ID;
        public string Name;
        public string MachineID;
        public Int64? Grouping;
        public string Data;
        public bool DataOnly;
    }

    public class PolicyEnableDisableRequest
    {
        public bool Enable;
    }

    public class PolicyObjectList
    {
        public List<PolicyObject> Items;
    }

    public class DiskDataReport
    {
        public string MachineID;
        public string Computername;
        public string DeviceID;
        public DateTime LastUpdated;
        public bool DevicePresent;
        public DiskDataAccess Access;
        public bool Automount;
        public DiskDataAvailability Availability;
        public Int64 Capacity;
        public string Caption;
        public bool Compressed;
        public int ConfigManagerErrorCode;
        public string Description;
        public bool DirtyBitSet;
        public string DriveLetter;
        public DiskDataDriveType DriveType;
        public string ErrorDescription;
        public string ErrorMethodology;
        public string FileSystem;
        public Int64 FreeSpace;
        public string Label;
        public int LastErrorCode;
        public int MaximumFileNameLength;
        public string Name;
        public string PNPDeviceID;
        public Int64 SerialNumber;
        public string Status;
    }

    public class ListDiskDataReport
    {
        public string MachineID;
        public List<DiskDataReport> Items;
    }

    public class NetworkAdapterConfiguration
    {
        public int InterfaceIndex;
        public List<string> IPAddress;
        public List<string> IPSubnet;
        public List<string> DefaultIPGateway;
        public bool IPEnabled;
        public string MACAddress;
        public string ServiceName;
        public string SettingsID;
        public string Description;
        public bool DHCPEnabled;
        public string DHCPServer;
        public string DNSDomain;
        public string DNSHostName;
        public List<string> DNSDomainSuffixSearchOrder;
        public List<string> DNSServerSearchOrder;
        public string Caption;
        public DateTime? DHCPLeaseExpires;
        public DateTime? DHCPLeaseObtained;
        public bool WINSEnableLMHostsLookup;
        public string WINSHostLookupFile;
        public string WINSPrimaryServer;
        public string WINSScopeID;
        public string WINSSecondaryServer;
    }

    public class ListNetworkAdapterConfiguration
    {
        public string MachineID;
        public List<NetworkAdapterConfiguration> Items;
    }

    public class PackageDataList
    {
        public List<PackageData> Items;
    }

    public class PackageIDData
    {
        public string Title;
        public string PackageID;
    }

    public class EventLogSearch
    {
        public string MachineID;
        public DateTime? FromDate;
        public DateTime? ToDate;
        public string Source;
        public int? EventLogType;
        public int QTY;
        public string EventLogBook;
        public int? CategoryNumber;
    }

    public class WindowsLic
    {
        public string MachineID;
        public DateTime Reported;
        public string Name;
        public string Description;
        public Int64 GracePeriodRemaining;
        public string PartialProductKey;
        public string ProductKeyID;
        public string ProductKeyID2;
        public string LicenseFamily;
        public string ProductKeyChannel;
        public Int64 LicenseStatus;
        public string LicenseStatusText;
    }

    public class ReportPaper
    {
        public string Name;
        public byte[] data;
    }

    public class ReportPaperRequest
    {
        public string Name;
        public List<string> MachineIDs;
        public DateTime? From;
        public DateTime? To;

    }

    public class WUUpdateInfo
    {
        public string Name;
        public string Description;
        public string Link;
        public string ID;
    }

    public class WUUpdateInfoList
    {
        public List<WUUpdateInfo> List;
    }

    public class WUStatus
    {
        public string Text;
    }

    public class WriteMessage
    {
        public string Text;
        public string Subject;
        public int Priority;
        public string Name;
    }

    public class UsersList
    {
        public Dictionary<string, string> Users;
        public string MachineID;
    }

    public class PnPDeviceList
    {
        public string MachineID;
        public List<PnPDevice> List;
    }

    public class PnPDevice
    {
        public int Index;
        public int Availability;
        public string Caption;
        public string ClassGuid;
        public List<string> CompatibleID;
        public int ConfigManagerErrorCode;
        public bool ConfigManagerUserConfig;
        public string CreationClassName;
        public string Description;
        public bool? ErrorCleared;
        public string ErrorDescription;
        public List<string> HardwareID;
        public DateTime? InstallDate;
        public int? LastErrorCode;
        public string Manufacturer;
        public string Name;
        public string PNPClass;
        public string PNPDeviceID;
        public bool Present;
        public string Service;
        public string Status;
        public int? StatusInfo;
    }

    public class FilterDriver
    {
        public string ClassGUID;
        public int Index;
        public string ServiceName;
        public int Type;
    }

    public class FilterDriverList
    {
        public string MachineID;
        public List<FilterDriver> List;
    }

    public class BitlockerRKKeyElement
    {
        public string VolumeKeyProtectorID;
        public string Key;
    }

    public class BitlockerRK
    {
        public string DeviceID;
        public string DriveLetter;
        public DateTime Reported;
        public List<BitlockerRKKeyElement> Keys;
    }

    public class BitlockerRKList
    {
        public string MachineID;
        public List<BitlockerRK> List;
    }

    public class FileUploadData
    {
        public string MachineID;
        public Int64 ID;
        public string RemoteFileLocation;
        /// <summary>
        /// 0 = Server to Client
        /// 1 = Client to Server
        /// 2 = Server to Management
        /// 3 = Management to Server
        /// </summary>
        public int Direction;
        public string MD5CheckSum;
        public Int64 Size;
        public Int64 ProgressSize;
        public DateTime LastUpdated;
        public DateTime FileLastModified;
        public bool OverrideMeteredConnection;
        public bool RequestOnly;
        public DateTime TimeStampCheck;
    }

    public class FileUploadDataSigned
    {
        public FileUploadData Data;
        public byte[] Signature;
    }

    public class FileUploadAppendData
    {
        public Int64 ID;
        public byte[] Data;
        public string MD5;
        public int Size;
        public string MachineID;
    }

    public class FileUploadDataList
    {
        public List<FileUploadData> List;
    }

    public class NetInt64List2
    {
        public List<Int64> data;
        public DateTime TimeStampCheck;
    }

    public class NetInt64List
    {
        public List<Int64> data;
    }

    public class NetInt64ListSigned
    {
        public NetInt64List2 data;
        public byte[] Signature;
    }

    public class VulpesSMARTInfoList
    {
        public List<VulpesSMARTInfo> List;
        public string MachineID;
    }

    public class VulpesSMARTInfo
    {
        public string PNPDeviceID;
        public string Name;
        public string Model;
        public string InterfaceType;
        public string FirmwareRevision;
        public string SerialNumber;
        public Int64 Size;
        public string Status;
        public string Caption;
        public bool? PredictFailure;
        public byte[] VendorSpecific;
        public byte[] VendorSpecificThreshold;
        public Dictionary<int, VulpesSMARTAttribute> Attributes = new Dictionary<int, VulpesSMARTAttribute>();
    }

    public class VulpesSMARTAttribute
    {
        public int ID;
        public int Flags;
        public bool FailureImminent;
        public int Value;
        public int Worst;
        public int Vendordata;
        public int Threshold;
    }

    public class SimpleTaskLite
    {
        public string MachineID;
        public string ComputerName;
        public Int64 ID;
        public int Type;
        public string Name;
    }

    public class SimpleTaskLiteList
    {
        public List<SimpleTaskLite> List;
    }

    public class SimpleTask
    {
        public string MachineID;
        public Int64 ID;
        public int Type;
        public string Name;
        public string Data;
        public DateTime TimeStampCheck;
    }

    public class SimpleTaskResult
    {
        public Int64 ID;
        public string Name;
        public string MachineID;
        public int Result;
        public string Text;
    }

    public class SimpleTaskRunProgramm
    {
        public string Executable;
        public string Parameters;
        public string User;
    }

    public class SimpleTaskRegistry
    {
        public int Action;
        public int Root;
        public string Folder;
        public string Valuename;
        public int ValueType;
        public string Data;
    }

    public class SimpleTaskNix
    {
        public string Dummy;
    }

    public class UserInfo
    {
        public string Username;
        public string Name;
    }

    public class UserDetails
    {
        public string Username;
        public string Name;
        public Int64 Permissions;
        public bool MustChangePassword;
        public string EMail;
        public bool UseLDAP;
        public string LDAPUsername;
    }

    public class UserDetailsPassword
    {
        public string Username;
        public string Name;
        public Int64 Permissions;
        public bool MustChangePassword;
        public string EMail;
        public bool UseLDAP;
        public string LDAPUsername;
        public string NewPassword;
    }

    public class UserDetailsList
    {
        public List<UserDetails> List;
    }
}
