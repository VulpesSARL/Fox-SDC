using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class DBUpdate
    {
        public const int DBVersion = 33;
        static public bool UpdateDB(SQLLib sql)
        {
            try
            {
                sql.SEHError = true;
                int Version = Convert.ToInt32(sql.ExecSQLScalar("SELECT Value FROM Config WHERE [Key]='Version'"));
                switch (Version)
                {
                    case 0:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [Users](
                                [Username] [nvarchar](100) NOT NULL,
                                [Password] [nvarchar](100) NOT NULL,
                                [Name] [nvarchar](500) NOT NULL,
                                [Permissions] [bigint] NOT NULL,
                                [MustChangePassword] [bit] NOT NULL CONSTRAINT [DF_Users_MustChangePassword]  DEFAULT ((0)),
                                [EMail] [nvarchar](500) NULL,
                             CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
                            (
                                [Username] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        Version = 1;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 1:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE ComputerAccounts
                            (
                            MachineID nvarchar(100) NOT NULL,
                            Accepted bit NOT NULL,
                            UCID nvarchar(100) NOT NULL,
                            Password nvarchar(100) NOT NULL,
                            Is64Bit bit NOT NULL,
                            OSName nvarchar(500) NOT NULL,
                            OSVerMaj int NOT NULL,
                            OSVerMin int NOT NULL,
                            OSVerBuild int NOT NULL,
                            OSVerType int NOT NULL,
                            OSSuite nvarchar(500) NOT NULL,
                            IsTSE bit NOT NULL,
                            CPU nvarchar(500) NOT NULL,
                            ComputerModel nvarchar(500) NOT NULL,
                            ComputerName nvarchar(500) NOT NULL,
                            Language nvarchar(100) NOT NULL,
                            DisplayLanguage nvarchar(500) NOT NULL,
                            LastUpdated datetime NOT NULL,
                            AgentVersion nvarchar(200) NOT NULL,
                            AgentVersionID bigint NOT NULL,
                            RunningInHypervisor bit NOT NULL,
                            BIOS nvarchar(500) NOT NULL
                            )  ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD CONSTRAINT DF_ComputerAccounts_Accepted DEFAULT 0 FOR Accepted");
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD CONSTRAINT PK_ComputerAccounts PRIMARY KEY CLUSTERED (MachineID) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD CONSTRAINT DF_ComputerAccounts_LastUpdated DEFAULT getutcdate() FOR LastUpdated");
                        Version = 2;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 2:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [Grouping](
                                [ID] [bigint] IDENTITY(1,1) NOT NULL,
                                [Name] [nvarchar](500) NOT NULL,
                                [ParentID] [bigint] NULL,
                             CONSTRAINT [PK_Grouping] PRIMARY KEY CLUSTERED 
                            (
                                [ID] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [Grouping] WITH CHECK ADD CONSTRAINT [FK_Grouping_Grouping] FOREIGN KEY([ParentID]) REFERENCES [Grouping] ([ID])");
                        sql.ExecSQL("ALTER TABLE [Grouping] CHECK CONSTRAINT [FK_Grouping_Grouping]");
                        Version = 3;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 3:
                        sql.BeginTransaction();
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD Grouping bigint NULL");
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD CONSTRAINT FK_ComputerAccounts_Grouping FOREIGN KEY (Grouping) REFERENCES Grouping (ID) ON UPDATE NO ACTION   ON DELETE NO ACTION");
                        sql.ExecSQL("ALTER TABLE ComputerAccounts CHECK CONSTRAINT FK_ComputerAccounts_Grouping");
                        Version = 4;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 4:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [AddRemovePrograms](
                                  [MachineID] [nvarchar](100) NOT NULL,
                                  [ProductID] [nvarchar](300) NOT NULL,
                                  [IsWOWBranch] [bit] NOT NULL,
                                  [IsMSI] [bit] NOT NULL,
                                  [IsSystemComponent] [bit] NOT NULL,
                                  [Name] [nvarchar](2000) NOT NULL,
                                  [DisplayVersion] [nvarchar](1000) NOT NULL,
                                  [UninstallString] [nvarchar](2000) NOT NULL,
                                  [VersionMajor] [int] NOT NULL,
                                  [VersionMinor] [int] NOT NULL,
                                  [Language] [nvarchar](50) NULL,
                                  [DisplayLanguage] [nvarchar](300) NULL,
                                  [DT] [datetime] NOT NULL,
                            CONSTRAINT [PK_AddRemovePrograms] PRIMARY KEY CLUSTERED 
                            (
                                  [MachineID] ASC,
                                  [ProductID] ASC,
                                  [IsWOWBranch] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE AddRemovePrograms ADD CONSTRAINT FK_AddRemovePrograms_MachineID FOREIGN KEY (MachineID) REFERENCES ComputerAccounts (MachineID) ON UPDATE NO ACTION ON DELETE NO ACTION");
                        sql.ExecSQL(@"ALTER TABLE AddRemovePrograms CHECK CONSTRAINT FK_AddRemovePrograms_MachineID");
                        Version = 5;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 5:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE Policies
                            (
                            ID bigint NOT NULL IDENTITY (1, 1),
                            Type int NOT NULL,
                            Name nvarchar(500) NOT NULL,
                            Grouping bigint NULL,
                            MachineID nvarchar(100) NULL,
                            DataBlob nvarchar(MAX) NOT NULL,
                            DT datetime NOT NULL,
                            Version bigint NOT NULL,
                            Enabled bit NOT NULL
                            )  ON [PRIMARY]
                             TEXTIMAGE_ON [PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE Policies ADD CONSTRAINT PK_Policies PRIMARY KEY CLUSTERED (ID) WITH(STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE Policies ADD CONSTRAINT FK_Policies_Grouping FOREIGN KEY (Grouping) REFERENCES Grouping (ID) ON UPDATE NO ACTION ON DELETE NO ACTION");
                        sql.ExecSQL(@"ALTER TABLE Policies CHECK CONSTRAINT FK_Policies_Grouping");
                        sql.ExecSQL(@"ALTER TABLE Policies ADD CONSTRAINT FK_Policies_MachineID FOREIGN KEY (MachineID) REFERENCES ComputerAccounts (MachineID) ON UPDATE NO ACTION ON DELETE NO ACTION");
                        sql.ExecSQL(@"ALTER TABLE Policies CHECK CONSTRAINT FK_Policies_MachineID");
                        sql.ExecSQL(@"ALTER TABLE Policies WITH CHECK ADD CONSTRAINT CK_Policies CHECK ((Grouping IS NULL AND MachineID IS NULL OR Grouping IS NOT NULL AND MachineID IS NULL OR Grouping IS NULL AND MachineID IS NOT NULL))");
                        sql.ExecSQL(@"ALTER TABLE Policies CHECK CONSTRAINT CK_Policies");

                        Version = 6;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 6:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE DiskData
                            (
                            MachineID nvarchar(100) NOT NULL,
                            DevicePresent bit not NULL,
                            LastUpdated datetime NOT NULL,
                            DeviceID nvarchar(400) NOT NULL,
                            Access int NOT NULL,
                            Automount bit not NULL,
                            Availability int NOT NULL,
                            Capacity bigint NOT NULL,
                            Caption nvarchar(500) NOT NULL,
                            Compressed bit NOT NULL,
                            ConfigManagerErrorCode int NOT NULL,
                            Description nvarchar(500) NOT NULL,
                            DirtyBitSet bit NOT NULL,
                            DriveLetter nvarchar(300) NOT NULL,
                            DriveType int NOT NULL,
                            ErrorDescription nvarchar(500) NOT NULL,
                            ErrorMethodology nvarchar(500) NOT NULL,
                            FileSystem nvarchar(100) NOT NULL,
                            FreeSpace bigint NOT NULL,
                            Label nvarchar(100) NOT NULL,
                            LastErrorCode int NOT NULL,
                            MaximumFileNameLength int NOT NULL,
                            Name nvarchar(500) NOT NULL,
                            PNPDeviceID nvarchar(500) NOT NULL,
                            SerialNumber bigint NOT NULL,
                            Status nvarchar(500) NOT NULL
                            )  ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE DiskData ADD CONSTRAINT PK_DiskData PRIMARY KEY CLUSTERED (MachineID ASC, DeviceID ASC) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE DiskData ADD CONSTRAINT DF_DiskData_LastUpdated DEFAULT getutcdate() FOR LastUpdated");
                        sql.ExecSQL("ALTER TABLE DiskData ADD CONSTRAINT FK_DiskData_MachineID FOREIGN KEY (MachineID) REFERENCES ComputerAccounts (MachineID) ON UPDATE NO ACTION ON DELETE NO ACTION");
                        sql.ExecSQL("ALTER TABLE DiskData CHECK CONSTRAINT FK_DiskData_MachineID");
                        Version = 7;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 7:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE Packages(
                            [ID] [bigint] IDENTITY(1,1) NOT NULL,
                            [PackageID] [nvarchar](200) NOT NULL,
                            [Version] [bigint] NOT NULL,
                            [Title] [nvarchar](500) NOT NULL,
                            [Description] [nvarchar](max) NOT NULL,
                            [Filename] [nvarchar](200) NOT NULL,
                            [MetaFilename] [nvarchar](200) NOT NULL,
                            [Size] [bigint] NOT NULL,
                            [Uploaded] [datetime] NOT NULL DEFAULT getutcdate(),
                            CONSTRAINT [PK_Packages] PRIMARY KEY CLUSTERED 
                        (
                            [ID] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
                        Version = 8;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 8:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE Contracts
                            (
                            ContractID nvarchar(200) NOT NULL,
                            ContractPassword nvarchar(200) NOT NULL,
                            ValidFrom datetime NULL,
                            ValidTo datetime NULL,
                            MaxComputers int NULL,
                            PricePerComputer decimal(28, 10) NULL,
                            PriceTotal decimal(28, 10) NULL,
                            Vacant1 int NULL,
                            Vacant2 int NULL,
                            Vacant3 int NULL,
                            Vacant4 int NULL,
                            Vacant5 int NULL,
                            Vacant6 nvarchar(200) NULL,
                            Vacant7 nvarchar(200) NULL,
                            Vacant8 nvarchar(200) NULL,
                            Vacant9 nvarchar(200) NULL,
                            Vacant10 nvarchar(200) NULL,
                            Vacant11 decimal(28, 10) NULL,
                            Vacant12 decimal(28, 10) NULL,
                            Vacant13 datetime NULL,
                            Vacant14 datetime NULL
                            )  ON [PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE Contracts ADD CONSTRAINT
                            PK_Contracts PRIMARY KEY CLUSTERED 
                            (
                            ContractID
                            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE ComputerAccounts ADD ContractID nvarchar(200) NULL");
                        sql.ExecSQL(@"ALTER TABLE ComputerAccounts ADD CONSTRAINT FK_ComputerAccounts_ContractID FOREIGN KEY (ContractID) REFERENCES Contracts (ContractID) ON UPDATE NO ACTION ON DELETE NO ACTION");
                        Version = 9;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 9:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"ALTER TABLE Contracts ADD Disabled bit NOT NULL CONSTRAINT DF_Contracts_Disabled DEFAULT 0");
                        Version = 10;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 10:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE EventLog
                            (
                            ID bigint NOT NULL IDENTITY(1, 1),
                            MachineID nvarchar(100) NOT NULL,
                            LogID nvarchar(100) NOT NULL,
                            EventLog nvarchar(100) NOT NULL,
                            Source nvarchar(100) NOT NULL,
                            Category nvarchar(100) NOT NULL,
                            CategoryNumber int NOT NULL,
                            EventLogType int NOT NULL,
                            InstanceID bigint NOT NULL,
                            TimeGenerated datetime NOT NULL,
                            TimeWritten datetime NOT NULL,
                            Reported datetime NOT NULL,
                            JSONReplacementStrings nvarchar(max) NULL,
                            Data varbinary(MAX) NULL,
                            Message nvarchar(MAX) NOT NULL
                            )  ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE EventLog ADD CONSTRAINT PK_EventLog PRIMARY KEY CLUSTERED (ID) WITH(STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE EventLog ADD CONSTRAINT FK_EventLog_MachineID FOREIGN KEY (MachineID) REFERENCES ComputerAccounts (MachineID) ON UPDATE NO ACTION ON DELETE NO ACTION");

                        Version = 11;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 11:
                        sql.ExecSQL("CREATE INDEX TimeGeneratedFilter ON EventLog ([TimeGenerated] DESC) INCLUDE([ID], [MachineID], [LogID], [EventLog],[Source],[Category],[CategoryNumber],[EventLogType],[InstanceID],[TimeWritten],[Reported],[JSONReplacementStrings],[Data],[Message]) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
                        Version = 12;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 12:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE RSOP
                            (
                            MachineID nvarchar(100) NOT NULL,
                            RSOPData nvarchar(MAX) NOT NULL
                            ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE RSOP ADD CONSTRAINT PK_RSOP PRIMARY KEY CLUSTERED (MachineID) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE RSOP ADD CONSTRAINT FK_RSOP_MachineID FOREIGN KEY (MachineID) REFERENCES ComputerAccounts (MachineID) ON UPDATE NO ACTION ON DELETE NO ACTION");
                        Version = 13;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 13:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE WindowsLic
                            (
                            MachineID nvarchar(100) NOT NULL,
                            Name nvarchar(200) NOT NULL,
                            Description nvarchar(400) NOT NULL,
                            GracePeriodRemaining BIGINT NOT NULL,
                            PartialProductKey nvarchar(400) NOT NULL,
                            ProductKeyID nvarchar(400) NOT NULL,
                            ProductKeyID2 nvarchar(400) NOT NULL,
                            LicenseFamily nvarchar(400) NOT NULL,
                            ProductKeyChannel nvarchar(400) NOT NULL,
                            LicenseStatus BIGINT NOT NULL,
                            LicenseStatusText nvarchar(200) NOT NULL
                            ) ON[PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE WindowsLic ADD CONSTRAINT PK_WindowsLic PRIMARY KEY CLUSTERED (MachineID) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]");
                        sql.ExecSQL(@"ALTER TABLE WindowsLic ADD CONSTRAINT FK_WindowsLic_MachineID FOREIGN KEY (MachineID) REFERENCES ComputerAccounts (MachineID) ON UPDATE NO ACTION ON DELETE NO ACTION");

                        Version = 14;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 14:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"ALTER TABLE WindowsLic ADD Reported DateTime NOT NULL CONSTRAINT DF_WindowsLic_Reported DEFAULT getutcdate()");
                        sql.ExecSQL(@"ALTER TABLE RSOP ADD Reported DateTime NOT NULL CONSTRAINT DF_RSOP_Reported DEFAULT getutcdate()");
                        Version = 15;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 15:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"ALTER TABLE ComputerAccounts ADD IPAddress nvarchar(200) NULL");
                        Version = 16;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 16:
                        sql.BeginTransaction();
                        sql.ExecSQL("DROP INDEX TimeGeneratedFilter ON EventLog");
                        sql.ExecSQL("CREATE INDEX EventLogIndex ON EventLog ([TimeGenerated] DESC, [Source] ASC) INCLUDE([ID], [MachineID], [LogID], [EventLog],[Category],[CategoryNumber],[EventLogType],[InstanceID],[TimeWritten],[Reported],[JSONReplacementStrings],[Data],[Message]) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
                        Version = 17;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 17:
                        sql.BeginTransaction();
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD BIOSType nvarchar(100) NULL, NumberOfLogicalProcessors int NULL, NumberOfProcessors int NULL, TotalPhysicalMemory bigint NULL, CPUName nvarchar(500) NULL, SecureBootState nvarchar(100) NULL");
                        Version = 18;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 18:
                        sql.BeginTransaction();
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD SystemRoot nvarchar(100) NULL");
                        Version = 19;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 19:
                        sql.BeginTransaction();
                        sql.ExecSQL("DROP INDEX EventLogIndex ON EventLog");
                        sql.ExecSQL("CREATE NONCLUSTERED INDEX LogIDIndex ON EventLog ([MachineID], [LogID]) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
                        sql.ExecSQL("CREATE NONCLUSTERED INDEX TGIndex ON EventLog ([MachineID], [TimeGenerated]) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
                        sql.ExecSQL("CREATE NONCLUSTERED INDEX GiantEventLogIndex ON [EventLog] ([MachineID],[TimeGenerated] desc) INCLUDE ([ID],[LogID],[EventLog],[Category],[Source],[CategoryNumber],[EventLogType],[InstanceID],[TimeWritten],[Reported],[JSONReplacementStrings],[Data],[Message])");
                        sql.ExecSQL("CREATE NONCLUSTERED INDEX SourceIndex ON EventLog ([Source] asc) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
                        Version = 20;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 20:
                        sql.BeginTransaction();
                        sql.ExecSQL("DROP TABLE RSOP");
                        Version = 21;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 21:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE Reporting
                            (
                            ID bigint NOT NULL IDENTITY (1, 1),
                            Reported datetime NOT NULL,
                            MachineID nvarchar(100) NOT NULL,
                            Type int NOT NULL,
                            Data nvarchar(MAX) NOT NULL,
                            Flags bigint NOT NULL
                            )  ON [PRIMARY]
                                TEXTIMAGE_ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE Reporting ADD CONSTRAINT DF_Reporting_Reported DEFAULT getutcdate() FOR Reported");
                        sql.ExecSQL("ALTER TABLE Reporting ADD CONSTRAINT DF_Reporting_Flags DEFAULT 0 FOR Flags");
                        sql.ExecSQL("ALTER TABLE Reporting ADD CONSTRAINT PK_Reporting PRIMARY KEY CLUSTERED (ID) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE Reporting ADD CONSTRAINT FK_Reporting_ComputerAccounts FOREIGN KEY (MachineID) REFERENCES dbo.ComputerAccounts (MachineID) ON UPDATE NO ACTION ON DELETE NO ACTION");
                        Version = 22;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 22:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [ReportPapers](
                                [ID] [nvarchar](100) NOT NULL,
                                [DT] [datetime] NOT NULL,
                                [Data] [varbinary](max) NOT NULL,
                             CONSTRAINT [PK_ReportPapers] PRIMARY KEY CLUSTERED 
                            (
                                [ID] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
                        sql.ExecSQL("ALTER Table ReportPapers ADD CONSTRAINT PK_ReportPapers_DT DEFAULT getutcdate() FOR DT");
                        Version = 23;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 23:
                        sql.BeginTransaction();
                        sql.ExecSQL("ALTER Table ComputerAccounts ADD Comments NVARCHAR(200) NULL");
                        Version = 24;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 24:
                        sql.BeginTransaction();
                        sql.ExecSQL("ALTER Table Contracts ADD EMail NVARCHAR(200) NULL");
                        Version = 25;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 25:
                        sql.BeginTransaction();
                        sql.ExecSQL("ALTER Table ComputerAccounts ADD SUSID NVARCHAR(200) NULL");
                        Version = 26;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 26:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [NetworkConfig](
                                [MachineID] [nvarchar](100) NOT NULL,
                                [InterfaceIndex] [int] NOT NULL,
                                [IPEnabled] [bit] NOT NULL,
                                [MACAddress] [varchar](100) NOT NULL,
                                [ServiceName] [varchar](100) NOT NULL,
                                [SettingsID] [varchar](100) NOT NULL,
                                [Description] [nvarchar](200) NOT NULL,
                                [DHCPEnabled] [bit] NOT NULL,
                                [DHCPServer] [varchar](100) NOT NULL,
                                [DNSDomain] [nvarchar](200) NOT NULL,
                                [DNSHostName] [nvarchar](200) NOT NULL,
                                [Caption] [nvarchar](200) NOT NULL,
                                [DHCPLeaseExpires] [datetime] NULL,
                                [DHCPLeaseObtained] [datetime] NULL,
                                [WINSEnableLMHostsLookup] [bit] NOT NULL,
                                [WINSHostLookupFile] [nvarchar](100) NOT NULL,
                                [WINSPrimaryServer] [varchar](100) NOT NULL,
                                [WINSSecondaryServer] [varchar](100) NOT NULL,
                                [WINSScopeID] [varchar](100) NOT NULL,
                             CONSTRAINT [PK_NetworkConfig] PRIMARY KEY CLUSTERED 
                            (
                                [MachineID] ASC,
                                [InterfaceIndex] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [NetworkConfig] WITH CHECK ADD CONSTRAINT [FK_NetworkConfig_ComputerAccounts] FOREIGN KEY([MachineID]) REFERENCES [ComputerAccounts] ([MachineID])");
                        sql.ExecSQL("ALTER TABLE [NetworkConfig] CHECK CONSTRAINT [FK_NetworkConfig_ComputerAccounts]");
                        sql.ExecSQL(@"CREATE TABLE [NetworkConfigSuppl](
                                [MachineID] [nvarchar](100) NOT NULL,
                                [InterfaceIndex] [int] NOT NULL,
                                [Type] [int] NOT NULL,
                                [Order] [int] NOT NULL,
                                [Data] [nvarchar](100) NOT NULL,
                             CONSTRAINT [PK_NetworkConfigSuppl] PRIMARY KEY CLUSTERED 
                            (
                                [MachineID] ASC,
                                [InterfaceIndex] ASC,
                                [Type] ASC,
                                [Order] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [NetworkConfigSuppl] WITH CHECK ADD CONSTRAINT [FK_NetworkConfigSuppl_NetworkConfig] FOREIGN KEY([MachineID], [InterfaceIndex]) REFERENCES [NetworkConfig] ([MachineID], [InterfaceIndex])");
                        sql.ExecSQL("ALTER TABLE [NetworkConfigSuppl] CHECK CONSTRAINT [FK_NetworkConfigSuppl_NetworkConfig]");
                        Version = 27;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 27:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [DevicesConfig](
                                [MachineID] [nvarchar](100) NOT NULL,
                                [Index] [int] NOT NULL,
                                [Availability] int NOT NULL,
                                [Caption] nvarchar(300) NOT NULL,
                                [ClassGuid] nvarchar(300) NOT NULL,
                                [CompatibleID] nvarchar(MAX) NOT NULL,
                                [ConfigManagerErrorCode] int NOT NULL,
                                [ConfigManagerUserConfig] bit NOT NULL,
                                [CreationClassName] nvarchar(200) NOT NULL,
                                [Description] nvarchar(500) NOT NULL,
                                [ErrorCleared] bit NULL,
                                [ErrorDescription] nvarchar(500) NOT NULL,
                                [HardwareID] nvarchar(MAX) NOT NULL,
                                [InstallDate] [datetime] NULL,
                                [LastErrorCode] int NULL,
                                [Manufacturer] nvarchar(500) NOT NULL,
                                [Name] nvarchar(500) NOT NULL,
                                [PNPClass] nvarchar(500) NOT NULL,
                                [PNPDeviceID] nvarchar(500) NOT NULL,
                                [Present] bit NOT NULL,
                                [Service] nvarchar(200) NOT NULL,
                                [Status] nvarchar(200) NOT NULL,
                                [StatusInfo] int NULL,
                             CONSTRAINT [PK_DevicesConfig] PRIMARY KEY CLUSTERED 
                            (
                                [MachineID] ASC,
                                [Index] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [DevicesConfig] WITH CHECK ADD CONSTRAINT [FK_DevicesConfig_ComputerAccounts] FOREIGN KEY([MachineID]) REFERENCES [ComputerAccounts] ([MachineID])");
                        sql.ExecSQL("ALTER TABLE [DevicesConfig] CHECK CONSTRAINT [FK_DevicesConfig_ComputerAccounts]");
                        Version = 28;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 28:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [DevicesFilter](
                                [MachineID] [nvarchar](100) NOT NULL,
                                [Index] [int] NOT NULL,
                                [ClassGUID] [nvarchar](100) NOT NULL,
                                [ServiceName] [nvarchar](100) NOT NULL,
                                [Type] [int] NOT NULL,
                            CONSTRAINT [PK_DevicesFilter] PRIMARY KEY CLUSTERED 
                            (
                                [MachineID] ASC,
                                [Index] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [DevicesFilter] WITH CHECK ADD CONSTRAINT [FK_DevicesFilter_ComputerAccounts] FOREIGN KEY([MachineID]) REFERENCES [ComputerAccounts] ([MachineID])");
                        sql.ExecSQL("ALTER TABLE [DevicesFilter] CHECK CONSTRAINT [FK_DevicesFilter_ComputerAccounts]");
                        Version = 29;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 29:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [BitlockerRK](
                                [MachineID] [nvarchar](100) NOT NULL,
                                [DeviceID] [nvarchar](200) NOT NULL,
                                [Reported] [datetime] NOT NULL,
                                [DriveLetter] [nvarchar](50) NOT NULL,
                                [Keys] [nvarchar](max) NOT NULL,
                            CONSTRAINT [PK_BitlockerRK] PRIMARY KEY CLUSTERED 
                            (
                                [MachineID] ASC,
                                [DeviceID] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [BitlockerRK] WITH CHECK ADD CONSTRAINT [FK_BitlockerRK_ComputerAccounts] FOREIGN KEY([MachineID]) REFERENCES [ComputerAccounts] ([MachineID])");
                        sql.ExecSQL("ALTER TABLE [BitlockerRK] CHECK CONSTRAINT [FK_BitlockerRK_ComputerAccounts]");
                        Version = 30;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 30:
                        sql.BeginTransaction();
                        sql.ExecSQL("ALTER TABLE ComputerAccounts ADD MeteredConnection bit NULL");
                        Version = 31;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 31:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [Chats](
                            [ID][bigint] NOT NULL IDENTITY(1, 1),
                            [MachineID][nvarchar](100) NOT NULL,
                            [DT][datetime] NOT NULL,
                            [Read][bit] NOT NULL,
                            [ToClient][bit] NOT NULL,
                            [Name][nvarchar](100) NOT NULL,
                            [Text][nvarchar](max) NOT NULL,
                         CONSTRAINT[PK_Chats] PRIMARY KEY CLUSTERED
                        (
                            [ID] ASC,
                            [MachineID] ASC
                        )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
                        ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [Chats] WITH CHECK ADD CONSTRAINT[FK_Chats_ComputerAccounts] FOREIGN KEY([MachineID]) REFERENCES [ComputerAccounts]([MachineID])");
                        sql.ExecSQL("ALTER TABLE [Chats] CHECK CONSTRAINT[FK_Chats_ComputerAccounts]");
                        Version = 32;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                    case 32:
                        sql.BeginTransaction();
                        sql.ExecSQL(@"CREATE TABLE [FileTransfers](
                                [ID] [bigint] IDENTITY(1,1) NOT NULL,
                                [MachineID] [nvarchar](100) NOT NULL,
                                [RemoteFileLocation] [nvarchar](500) NOT NULL,
                                [ServerFile] [nvarchar](100) NOT NULL,
                                [Direction] [int] NOT NULL,
                                [MD5Sum] [nvarchar](50) NOT NULL,
                                [Size] [bigint] NOT NULL,
                                [ProgressSize] [bigint] NOT NULL,
                                [DTUpdated] [datetime] NOT NULL,
                                [FileLastModified] [datetime2](7) NOT NULL,
                                [RequestOnly] [bit] NOT NULL,
                                [OverrideMeteredConnection] [bit] NOT NULL,
                                [MGMT_Computer] [nvarchar](100) NULL,
                             CONSTRAINT [PK_FileTransfers] PRIMARY KEY CLUSTERED 
                            (
                                [ID] ASC,
                                [MachineID] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]");
                        sql.ExecSQL("ALTER TABLE [FileTransfers] WITH CHECK ADD  CONSTRAINT [FK_FileTransfers_ComputerAccounts] FOREIGN KEY([MachineID]) REFERENCES [ComputerAccounts] ([MachineID])");
                        sql.ExecSQL("ALTER TABLE [FileTransfers] CHECK CONSTRAINT [FK_FileTransfers_ComputerAccounts]");
                        Version = 33;
                        sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                        sql.CommitTransaction();
                        FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                        return (false);
                        /*case 1:
                            sql.BeginTransaction();
                            sql.ExecSQL("...............");
                            Version = 2;
                            sql.ExecSQL("UPDATE CONFIG SET [Value]=@ver WHERE [Key]='Version'", new SQLParam("@ver", Version));
                            sql.CommitTransaction();
                            FoxEventLog.WriteEventLog("Updated DB to Version " + Version.ToString(), EventLogEntryType.Information);
                            return (false);*/
                }
                return (true);
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("DB Update Error: " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                Debug.WriteLine(ee.ToString());
                sql.RollBackTransaction();
                return (true);
            }
            finally
            {
                sql.SEHError = false;
            }
        }
    }
}
