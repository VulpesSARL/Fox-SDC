using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_MGMT
{
#pragma warning disable 649

    public class ApproveListElement
    {
        public string Computername;
        public string MachineID;
    }

    public class SessionElement
    {
        public int SessionID;
        public string User;
        public string Domain;

        public override string ToString()
        {
            string cUser = "";
            if (string.IsNullOrWhiteSpace(Domain) == true && string.IsNullOrWhiteSpace(User) == true)
                cUser = "";
            else if (string.IsNullOrWhiteSpace(Domain) == true && string.IsNullOrWhiteSpace(User) == false)
                cUser = " - " + User;
            else
                cUser = " - " + Domain + "\\" + User;

            return (SessionID.ToString() + cUser);
        }

        public SessionElement()
        {

        }

        public SessionElement(PushRunningSessionElement e)
        {
            SessionID = e.SessionID;
            User = e.User;
            Domain = e.Domain;
        }
    }

    [RefreshProperties(RefreshProperties.All)]
    public class PComputerData
    {
        string _MachineID;

        [CategoryAttribute("Identification"), ReadOnlyAttribute(true), Browsable(true)]
        public string MachineID
        {
            get { return _MachineID; }
            set { _MachineID = value; }
        }
        string _UCID;

        [CategoryAttribute("Identification"), ReadOnlyAttribute(true), Browsable(true)]
        public string UCID
        {
            get { return _UCID; }
            set { _UCID = value; }
        }

        string _Comments;

        [CategoryAttribute("Identification"), ReadOnlyAttribute(true), Browsable(true)]
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }

        string _IPAddress;

        [CategoryAttribute("Identification"), ReadOnlyAttribute(true), Browsable(true)]
        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }

        string _Computername;

        [CategoryAttribute("Identification"), ReadOnlyAttribute(true), Browsable(true)]
        public string Computername
        {
            get { return _Computername; }
            set { _Computername = value; }
        }
        string _OS;

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public string OS
        {
            get { return _OS; }
            set { _OS = value; }
        }
        string _OSVersion;

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public string OSVersion
        {
            get { return _OSVersion; }
            set { _OSVersion = value; }
        }

        string _OSWin10Edition;

        string _OSSuite;

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public string OSSuite
        {
            get { return _OSSuite; }
            set { _OSSuite = value; }
        }
        string _Make;

        [CategoryAttribute("Vendor"), ReadOnlyAttribute(true), Browsable(true)]
        public string Make
        {
            get { return _Make; }
            set { _Make = value; }
        }

        string _BIOS;

        [CategoryAttribute("Vendor"), ReadOnlyAttribute(true), Browsable(true)]
        public string BIOS
        {
            get { return _BIOS; }
            set { _BIOS = value; }
        }

        string _Language;

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }
        string _CPU;

        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public string CPU
        {
            get { return _CPU; }
            set { _CPU = value; }
        }
        DateTime _LastUpdated;

        [CategoryAttribute("Updated"), ReadOnlyAttribute(true), Browsable(true)]
        public DateTime LastUpdated
        {
            get { return _LastUpdated; }
            set { _LastUpdated = value; }
        }
        bool _IsTSE;

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public bool IsTSE
        {
            get { return _IsTSE; }
            set { _IsTSE = value; }
        }
        bool _Is64Bit;

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public bool Is64Bit
        {
            get { return _Is64Bit; }
            set { _Is64Bit = value; }
        }

        bool _Approved;


        [CategoryAttribute("Approved"), ReadOnlyAttribute(true), Browsable(true)]
        public bool Approved
        {
            get { return _Approved; }
            set { _Approved = value; }
        }

        string _AgentVersion;

        [CategoryAttribute("Agent Version"), ReadOnlyAttribute(true), Browsable(true)]
        public string AgentVersion
        {
            get { return _AgentVersion; }
            set { _AgentVersion = value; }
        }
        Int64 _AgentVersionID;

        [CategoryAttribute("Agent Version"), ReadOnlyAttribute(true), Browsable(true)]
        public Int64 AgentVersionID
        {
            get { return _AgentVersionID; }
            set { _AgentVersionID = value; }
        }

        string _GroupingPath;

        [CategoryAttribute("Location"), ReadOnlyAttribute(true), Browsable(true)]
        public string GroupingPath
        {
            get { return _GroupingPath; }
            set { _GroupingPath = value; }
        }

        [CategoryAttribute("Location"), ReadOnlyAttribute(true), Browsable(true)]
        public string MeteredConnection
        {
            get { return (BMeteredConnection == null ? "Unknown" : (BMeteredConnection == true ? "Yes" : "No")); }
        }

        bool _RunningInHypervisor;

        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public bool RunningInHypervisor
        {
            get { return _RunningInHypervisor; }
            set { _RunningInHypervisor = value; }
        }

        public int IOSVerType;

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public string OSVerType
        {
            get { return (CommonUtilities.DecodeOSVerType(IOSVerType)); }
        }

        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public string SecureBootState { get => _SecureBootState; set => _SecureBootState = value; }
        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public string BIOSType { get => _BIOSType; set => _BIOSType = value; }
        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public int NumberOfLogicalProcessors { get => _NumberOfLogicalProcessors; set => _NumberOfLogicalProcessors = value; }
        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public int NumberOfProcessors { get => _NumberOfProcessors; set => _NumberOfProcessors = value; }
        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public long TotalPhysicalMemory { get => _TotalPhysicalMemory; set => _TotalPhysicalMemory = value; }
        [CategoryAttribute("Architecture"), ReadOnlyAttribute(true), Browsable(true)]
        public string CPUName { get => _CPUName; set => _CPUName = value; }
        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public string SystemRoot { get => _SystemRoot; set => _SystemRoot = value; }
        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public string OSWin10Edition { get => _OSWin10Edition; set => _OSWin10Edition = value; }

        [CategoryAttribute("Identification"), ReadOnlyAttribute(true), Browsable(true)]
        public string SUSID { get => _SUSID; set => _SUSID = value; }

        [CategoryAttribute("Operating System"), ReadOnlyAttribute(true), Browsable(true)]
        public bool? RunningInWindowsPE { get => _RunningInWindowsPE; set => _RunningInWindowsPE = value; }


        [RefreshProperties(RefreshProperties.All)]
        [CategoryAttribute("Users"), ReadOnlyAttribute(true), Browsable(true)]
        public string OneUser { get => _OneUser; set => _OneUser = value; }

        [RefreshProperties(RefreshProperties.All)]
        [CategoryAttribute("Users"), ReadOnlyAttribute(true), Browsable(true)]
        public int NumberOfUsers { get => _NumberOfUsers; set => _NumberOfUsers = value; }

        [RefreshProperties(RefreshProperties.All)]
        [CategoryAttribute("Users"), ReadOnlyAttribute(true), Browsable(true)]
        public List<string> LoggedInUsers { get => _LoggedInUser; set => _LoggedInUser = value; }

        string _SUSID;
        string _BIOSType;
        int _NumberOfLogicalProcessors;
        int _NumberOfProcessors;
        Int64 _TotalPhysicalMemory;
        string _CPUName;
        string _SecureBootState;
        string _SystemRoot;
        bool? _RunningInWindowsPE;
        public bool? BMeteredConnection;

        List<string> _LoggedInUser = new List<string>();
        int _NumberOfUsers;
        string _OneUser;
    }

    public class PAddRemovePrograms
    {
        string _MachineID;

        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public string MachineID
        {
            get { return _MachineID; }
            set { _MachineID = value; }
        }
        string _Computername;

        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public string Computername
        {
            get { return _Computername; }
            set { _Computername = value; }
        }
        string _ProductID;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public string ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }
        bool _IsWOWBranch;

        [CategoryAttribute("Flags"), ReadOnlyAttribute(true), Browsable(true)]
        public bool IsWOWBranch
        {
            get { return _IsWOWBranch; }
            set { _IsWOWBranch = value; }
        }
        bool _IsMSI;

        [CategoryAttribute("Flags"), ReadOnlyAttribute(true), Browsable(true)]
        public bool IsMSI
        {
            get { return _IsMSI; }
            set { _IsMSI = value; }
        }
        bool _IsSystemComponent;

        [CategoryAttribute("Flags"), ReadOnlyAttribute(true), Browsable(true)]
        public bool IsSystemComponent
        {
            get { return _IsSystemComponent; }
            set { _IsSystemComponent = value; }
        }
        string _Name;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        string _DisplayVersion;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public string DisplayVersion
        {
            get { return _DisplayVersion; }
            set { _DisplayVersion = value; }
        }
        string _UninstallString;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public string UninstallString
        {
            get { return _UninstallString; }
            set { _UninstallString = value; }
        }
        int _VersionMajor;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public int VersionMajor
        {
            get { return _VersionMajor; }
            set { _VersionMajor = value; }
        }
        int _VersionMinor;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public int VersionMinor
        {
            get { return _VersionMinor; }
            set { _VersionMinor = value; }
        }
        string _Language;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }
        string _DisplayLanguage;

        [CategoryAttribute("Software"), ReadOnlyAttribute(true), Browsable(true)]
        public string DisplayLanguage
        {
            get { return _DisplayLanguage; }
            set { _DisplayLanguage = value; }
        }
        DateTime _DT;

        [CategoryAttribute("Reporting"), ReadOnlyAttribute(true), Browsable(true)]
        public DateTime DT
        {
            get { return _DT; }
            set { _DT = value; }
        }
        string _HKCUUser;

        [CategoryAttribute("User"), ReadOnlyAttribute(true), Browsable(true)]
        public string HKCUUser
        {
            get { return _HKCUUser; }
            set { _HKCUUser = value; }
        }
        string _Username;

        [CategoryAttribute("User"), ReadOnlyAttribute(true), Browsable(true)]
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
    }

    public class PDiskDataReport
    {
        string _MachineID;

        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public string MachineID
        {
            get { return _MachineID; }
            set { _MachineID = value; }
        }
        string _Computername;

        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public string Computername
        {
            get { return _Computername; }
            set { _Computername = value; }
        }
        string _DeviceID;


        [CategoryAttribute("Device ID"), ReadOnlyAttribute(true), Browsable(true)]
        public string DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }
        DateTime _LastUpdated;

        [CategoryAttribute("Reporting"), ReadOnlyAttribute(true), Browsable(true)]
        public DateTime LastUpdated
        {
            get { return _LastUpdated; }
            set { _LastUpdated = value; }
        }
        bool _DevicePresent;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public bool DevicePresent
        {
            get { return _DevicePresent; }
            set { _DevicePresent = value; }
        }
        DiskDataAccess _Access;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public DiskDataAccess Access
        {
            get { return _Access; }
            set { _Access = value; }
        }
        bool _Automount;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public bool Automount
        {
            get { return _Automount; }
            set { _Automount = value; }
        }
        DiskDataAvailability _Availability;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public DiskDataAvailability Availability
        {
            get { return _Availability; }
            set { _Availability = value; }
        }
        Int64 _Capacity;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public Int64 Capacity
        {
            get { return _Capacity; }
            set { _Capacity = value; }
        }
        string _Caption;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }
        bool _Compressed;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public bool Compressed
        {
            get { return _Compressed; }
            set { _Compressed = value; }
        }
        int _ConfigManagerErrorCode;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public int ConfigManagerErrorCode
        {
            get { return _ConfigManagerErrorCode; }
            set { _ConfigManagerErrorCode = value; }
        }
        string _Description;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        bool _DirtyBitSet;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public bool DirtyBitSet
        {
            get { return _DirtyBitSet; }
            set { _DirtyBitSet = value; }
        }
        string _DriveLetter;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public string DriveLetter
        {
            get { return _DriveLetter; }
            set { _DriveLetter = value; }
        }
        DiskDataDriveType _DriveType;

        [CategoryAttribute("Device ID"), ReadOnlyAttribute(true), Browsable(true)]
        public DiskDataDriveType DriveType
        {
            get { return _DriveType; }
            set { _DriveType = value; }
        }
        string _ErrorDescription;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string ErrorDescription
        {
            get { return _ErrorDescription; }
            set { _ErrorDescription = value; }
        }
        string _ErrorMethodology;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string ErrorMethodology
        {
            get { return _ErrorMethodology; }
            set { _ErrorMethodology = value; }
        }
        string _FileSystem;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public string FileSystem
        {
            get { return _FileSystem; }
            set { _FileSystem = value; }
        }
        Int64 _FreeSpace;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public Int64 FreeSpace
        {
            get { return _FreeSpace; }
            set { _FreeSpace = value; }
        }
        string _Label;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public string Label
        {
            get { return _Label; }
            set { _Label = value; }
        }
        int _LastErrorCode;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public int LastErrorCode
        {
            get { return _LastErrorCode; }
            set { _LastErrorCode = value; }
        }
        int _MaximumFileNameLength;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public int MaximumFileNameLength
        {
            get { return _MaximumFileNameLength; }
            set { _MaximumFileNameLength = value; }
        }
        string _Name;

        [CategoryAttribute("Name"), ReadOnlyAttribute(true), Browsable(true)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        string _PNPDeviceID;

        [CategoryAttribute("Device ID"), ReadOnlyAttribute(true), Browsable(true)]
        public string PNPDeviceID
        {
            get { return _PNPDeviceID; }
            set { _PNPDeviceID = value; }
        }
        Int64 _SerialNumber;

        [CategoryAttribute("Filesystem"), ReadOnlyAttribute(true), Browsable(true)]
        public Int64 SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }
        string _Status;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }

    public class PWindowsLic
    {
        string _MachineID;
        DateTime _Reported;
        string _Name;
        string _Description;
        Int64 _GracePeriodRemaining;
        string _PartialProductKey;
        string _ProductKeyID;
        string _ProductKeyID2;
        string _LicenseFamily;
        string _ProductKeyChannel;
        Int64 _LicenseStatus;
        string _LicenseStatusText;

        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public string MachineID { get => _MachineID; set => _MachineID = value; }
        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public DateTime Reported { get => _Reported; set => _Reported = value; }
        [CategoryAttribute("License"), ReadOnlyAttribute(true), Browsable(true)]
        public string Name { get => _Name; set => _Name = value; }
        [CategoryAttribute("License"), ReadOnlyAttribute(true), Browsable(true)]
        public string Description { get => _Description; set => _Description = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public long GracePeriodRemaining { get => _GracePeriodRemaining; set => _GracePeriodRemaining = value; }
        [CategoryAttribute("License"), ReadOnlyAttribute(true), Browsable(true)]
        public string PartialProductKey { get => _PartialProductKey; set => _PartialProductKey = value; }
        [CategoryAttribute("License"), ReadOnlyAttribute(true), Browsable(true)]
        public string ProductKeyID { get => _ProductKeyID; set => _ProductKeyID = value; }
        [CategoryAttribute("License"), ReadOnlyAttribute(true), Browsable(true)]
        public string ProductKeyID2 { get => _ProductKeyID2; set => _ProductKeyID2 = value; }
        [CategoryAttribute("License"), ReadOnlyAttribute(true), Browsable(true)]
        public string LicenseFamily { get => _LicenseFamily; set => _LicenseFamily = value; }
        [CategoryAttribute("License"), ReadOnlyAttribute(true), Browsable(true)]
        public string ProductKeyChannel { get => _ProductKeyChannel; set => _ProductKeyChannel = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public long LicenseStatus { get => _LicenseStatus; set => _LicenseStatus = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string LicenseStatusText { get => _LicenseStatusText; set => _LicenseStatusText = value; }
    }

    public class PPnPDevice
    {
        int _Index;
        int _Availability;
        string _Caption;
        string _ClassGuid;
        List<string> _CompatibleID;
        int _ConfigManagerErrorCode;
        bool _ConfigManagerUserConfig;
        string _CreationClassName;
        string _Description;
        bool? _ErrorCleared;
        string _ErrorDescription;
        List<string> _HardwareID;
        DateTime? _InstallDate;
        int? _LastErrorCode;
        string _Manufacturer;
        string _Name;
        string _PNPClass;
        string _PNPDeviceID;
        bool _Present;
        string _Service;
        string _Status;
        int? _StatusInfo;
        string _ConfigManagerErrorCodeText;

        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public int? StatusInfo { get => _StatusInfo; set => _StatusInfo = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string Status { get => _Status; set => _Status = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string Service { get => _Service; set => _Service = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public bool Present { get => _Present; set => _Present = value; }
        [CategoryAttribute("ID"), ReadOnlyAttribute(true), Browsable(true)]
        public string PNPDeviceID { get => _PNPDeviceID; set => _PNPDeviceID = value; }
        [CategoryAttribute("ID"), ReadOnlyAttribute(true), Browsable(true)]
        public string PNPClass { get => _PNPClass; set => _PNPClass = value; }
        [CategoryAttribute("Name"), ReadOnlyAttribute(true), Browsable(true)]
        public string Name { get => _Name; set => _Name = value; }
        [CategoryAttribute("Name"), ReadOnlyAttribute(true), Browsable(true)]
        public string Manufacturer { get => _Manufacturer; set => _Manufacturer = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public int? LastErrorCode { get => _LastErrorCode; set => _LastErrorCode = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public DateTime? InstallDate { get => _InstallDate; set => _InstallDate = value; }
        [CategoryAttribute("ID"), ReadOnlyAttribute(true), Browsable(true)]
        public List<string> HardwareID { get => _HardwareID; set => _HardwareID = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string ErrorDescription { get => _ErrorDescription; set => _ErrorDescription = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public bool? ErrorCleared { get => _ErrorCleared; set => _ErrorCleared = value; }
        [CategoryAttribute("Name"), ReadOnlyAttribute(true), Browsable(true)]
        public string Description { get => _Description; set => _Description = value; }
        [CategoryAttribute("ID"), ReadOnlyAttribute(true), Browsable(true)]
        public string CreationClassName { get => _CreationClassName; set => _CreationClassName = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public bool ConfigManagerUserConfig { get => _ConfigManagerUserConfig; set => _ConfigManagerUserConfig = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public int ConfigManagerErrorCode { get => _ConfigManagerErrorCode; set => _ConfigManagerErrorCode = value; }
        [CategoryAttribute("ID"), ReadOnlyAttribute(true), Browsable(true)]
        public List<string> CompatibleID { get => _CompatibleID; set => _CompatibleID = value; }
        [CategoryAttribute("ID"), ReadOnlyAttribute(true), Browsable(true)]
        public string ClassGuid { get => _ClassGuid; set => _ClassGuid = value; }
        [CategoryAttribute("Name"), ReadOnlyAttribute(true), Browsable(true)]
        public string Caption { get => _Caption; set => _Caption = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public int Availability { get => _Availability; set => _Availability = value; }
        [CategoryAttribute("ID"), ReadOnlyAttribute(true), Browsable(true)]
        public int Index { get => _Index; set => _Index = value; }
        [CategoryAttribute("Status"), ReadOnlyAttribute(true), Browsable(true)]
        public string ConfigManagerErrorCodeText { get => _ConfigManagerErrorCodeText; set => _ConfigManagerErrorCodeText = value; }
    }

    public class PStartupItemFull
    {
        string _Computername;
        string _MachineID;
        string _Key;
        string _Item;
        string _Location;
        string _HKCUUser;
        string _Username;
        DateTime _DT;

        [CategoryAttribute("Reporting"), ReadOnlyAttribute(true), Browsable(true)]
        public DateTime DT { get => _DT; set => _DT = value; }
        [CategoryAttribute("User"), ReadOnlyAttribute(true), Browsable(true)]
        public string Username { get => _Username; set => _Username = value; }
        [CategoryAttribute("User"), ReadOnlyAttribute(true), Browsable(true)]
        public string HKCUUser { get => _HKCUUser; set => _HKCUUser = value; }
        [CategoryAttribute("Element"), ReadOnlyAttribute(true), Browsable(true)]
        public string Location { get => _Location; set => _Location = value; }
        [CategoryAttribute("Element"), ReadOnlyAttribute(true), Browsable(true)]
        public string Item { get => _Item; set => _Item = value; }
        [CategoryAttribute("Element"), ReadOnlyAttribute(true), Browsable(true)]
        public string Key { get => _Key; set => _Key = value; }
        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public string MachineID { get => _MachineID; set => _MachineID = value; }
        [CategoryAttribute("Computer"), ReadOnlyAttribute(true), Browsable(true)]
        public string Computername { get => _Computername; set => _Computername = value; }
    }

}
