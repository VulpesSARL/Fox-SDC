using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    [Flags]
    public enum ACLFlags : long
    {
        CanLogin = 0x1,
        ChangeServerSettings = 0x2,
        UploadData = 0x4,
        DownloadData = 0x8,
        ComputerLogin = 0x10
    }

    public enum ErrorFlags : long
    {
        NoError = 0,
        InvalidUsername = 1,
        InvalidPassword = 2,
        SQLError = 3,
        WrongUsernamePassword = 4,
        AccessDenied = 5,
        PWPolicyNotMet = 6,
        InvalidOldPassword = 7,
        UploadAlreadyRunning = 8,
        InvalidType = 9,
        FileTooLarge = 10,
        UploadNotRunning = 11,
        InvalidValue = 12,
        ChunkTooLarge = 13,
        NoData = 14,
        FaultySizes = 15,
        TooManyData = 16,
        CheckSumError = 17,
        UploadNotCompleted = 18,
        DownloadAlreadyRunning = 19,
        InvalidID = 20,
        FileSystemError = 21,
        InvalidData = 22,
        UCIDissues = 23,
        NotAccepted = 24,
        MachineUCIDmissmatch = 25,
        SocketError = 26,
        DuplicateElement = 27,
        SEHERROR = 28,
        SystemError = 29,
        CannotSign = 30,
        FaultyData = 31,
        FaultyContractData = 32,
        ContractNumComputersExhausted = 33,
        ContractNotStarted_Expired = 34,
        LicensingError = 35
    }

    public static class ACL
    {
        public static bool HasACL(Int64 Permissions, ACLFlags flag)
        {
            return ((Permissions & (Int64)flag) == (Int64)flag ? true : false);
        }
    }

    public enum DiskDataAccess : int
    {
        Unknown = 0,
        Readonly = 1,
        Writeonly = 2,
        ReadWrite = 3,
        WORM = 4
    }

    public enum DiskDataAvailability : int
    {
        Other = 1,
        Unknown = 2,
        Running = 3,
        Warning = 4,
        InTest = 5,
        NotApplicable = 6,
        PowerOff = 7,
        Offline = 8,
        OffDuty = 9,
        Degraded = 10,
        NotInstalled = 11,
        InstallError = 12,
        PWR_Unknown = 13,
        PWR_LowPower = 14,
        PWR_Standby = 15,
        PowerCycle = 16,
        PWR_Warning = 17,
        Paused = 18,
        NotReady = 19,
        NotConfigured = 20,
        Quiesced = 21
    }

    public enum DiskDataDriveType : int
    {
        Unknown = 0,
        NoRootDirectory = 1,
        RemovableDisk = 2,
        LocalDisk = 3,
        NetworkDrive = 4,
        CD = 5,
        RAMDisk = 6
    }
}
