using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class PolicyIDs
    {
        public const int Test = 1;
        public const int SignCertificate = 2;
        public const int PackageCertificate = 3;
        public const int LinkedPolicy = 4;
        public const int PackagePolicy = 5;
        public const int WSUS = 6;
        public const int InternationalSettings = 7;
        public const int ReportingPolicy = 8;
        public const int ClientSettings = 9;
        public const int PortMapping = 10;
        public static readonly List<int> HiddenPolicies = new List<int> { ReportingPolicy };
        public static string HiddenPoliciesSQLINClause
        {
            get
            {
                string s = "";
                foreach (int i in HiddenPolicies)
                {
                    s += i.ToString() + ",";
                }
                if (s.EndsWith(",") == true)
                    s = s.Substring(0, s.Length - 1);
                return (s);
            }
        }
    }


    public class PolicyTesting
    {
        public string Text;
        public bool CheckBox1;
        public bool CheckBox2;
        public bool CheckBox3;
    }

    public class PolicySigningCertificates
    {
        public string UUCerFile;
        public string UUSignFile;
    }

    public class PolicyPackageCertificates
    {
        public string UUCerFile;
    }

    public class PackagePolicy
    {
        public List<Int64> Packages;
        public bool OptionalInstallation;
        public bool InstallUpdates;
    }

    public class WSUSPolicy
    {
        public Int64 ID;
        public bool? ConfigureWSUS;
        public bool? SpecifyWUOptions;
        public int WUOptions;
        public Int64 Order;
        public bool? InstallDuringMaintenance;
        public bool? SpecifyScheduleInstall;
        public bool? InstallMicrosoftUpdates;
        public int ScheduleInstallDay;
        public int ScheduleInstallHour;
        public bool? SpecifyWUServer;
        public string WUServer;
        public bool? SpecifyStatusServer;
        public string StatusServer;
        public bool? SpecifyClientSideTargeting;
        public string Target;
        public bool? SpecifyDetectionFreq;
        public int DetectionFreq;
        public bool? DontAutoRestart;
        public bool? SpecifyAlwaysAutoRestart;
        public int AlwaysAutoRestartDelay;
        public bool? SpecifyDeadline;
        public int DeadLine;
        public bool? DontAutoRestartDuringActiveHours;
        public int ActiveHoursFrom;
        public int ActiveHoursTo;
        public bool? NoMSServer;
        public int DownloadMode;
        public bool? EnableDownloadMode;
        public bool? DisableDualScan;
    }

    public class PortMappingPolicy
    {
        public Int64 ID;
        public Int64 Order;

        public int ClientPort;
        public bool BindTo0000;
        public bool NoBindIfSDCServerIsDetected;
        public bool EditHOSTS;
        public string HOSTSEntry;
        public string ServerServer;
        public int ServerPort;
    }

    public class InternationalPolicy
    {
        //Docs https://technet.microsoft.com/en-us/library/cc978632.aspx 

        public Int64 ID;
        public Int64 Order;

        /// <summary>
        /// HKEY_CURRENT_USER\Control Panel\International "Locale" REG_SZ "00000809" (Hex!)
        /// HKEY_CURRENT_USER\Control Panel\International "LocaleName" REG_SZ "en-GB"
        /// HKEY_CURRENT_USER\Control Panel\International "sCountry" REG_SZ "United Kingdom"
        /// HKEY_CURRENT_USER\Control Panel\International "sLanguage" REG_SZ "ENG"
        /// MainFormat = Classic LCID
        /// </summary>
        public int MainFormat;
        /// <summary>
        /// HKEY_CURRENT_USER\Control Panel\International\Geo "Nation" REG_SZ "242" (Decimal!)
        /// Doc https://msdn.microsoft.com/en-us/library/windows/desktop/dd374073(v=vs.85).aspx
        /// </summary>
        public int MainLocation;
        /// <summary>
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\Language\  "Default" REG_SZ "0809" (Hex!)
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\Locale\   "(Default)" REG_SZ "00000809" (Hex!)
        ///  Doc https://msdn.microsoft.com/en-us/library/cc233982.aspx
        /// </summary>
        public int MainSystemLocale;
        /// <summary>
        /// Registry Key from HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones
        /// Write to HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation
        /// Doc https://blogs.msdn.microsoft.com/bclteam/2007/06/07/exploring-windows-time-zones-with-system-timezoneinfo-josh-free/
        /// </summary>
        public string TimeZone;
        public bool? AutoDST;
        /// <summary>
        /// 44 for UK; 352 for LU
        /// Reg: iCountry
        /// </summary>
        public string TelephoneIDN;

        /// <summary>
        /// Reg: iDigits
        /// </summary>
        public int NumNumOfDigitsAfterDec;
        /// <summary>
        /// Reg: sDecimal
        /// </summary>
        public string NumDecimalSymbol;
        /// <summary>
        /// Reg: sThousand
        /// '
        /// </summary>
        public string NumDigitGroupingSymbol;
        /// <summary>
        /// Reg: sGrouping
        /// 0 = "0"
        /// 1 = "3;0"
        /// 2 = "3;0;0"
        /// 3 = "3;2;0"
        /// </summary>
        public int NumDigitGrouping;
        /// <summary>
        /// Reg: sNegativeSign
        /// </summary>
        public string NumNegativeSignSymbol;
        /// <summary>
        /// Reg: sPositiveSign
        /// -nix-
        /// </summary>
        public string NumPositiveSignSymbol;
        /// <summary>
        /// Reg: iNegNumber
        /// </summary>
        public int NumNegativeNumberFormat;
        /// <summary>
        /// Reg: iLZero
        /// </summary>
        public int NumDisplayLeading0;
        /// <summary>
        /// Reg: sList
        /// ; or ,
        /// </summary>
        public string NumListSeparator;
        /// <summary>
        /// Reg: iMeasure
        /// </summary>
        public int NumMeasurement;
        /// <summary>
        /// Reg: sNativeDigits 
        /// 0123456789
        /// </summary>
        public int NumStdDigits;
        /// <summary>
        /// Reg: NumShape
        /// </summary>
        public int NumUseNativeDigits;

        /// <summary>
        /// Reg: sCurrency
        /// </summary>
        public string CurrCurrencySymbol;
        /// <summary>
        /// Reg: iCurrency
        /// </summary>
        public int CurrPositiveFormat;
        /// <summary>
        /// Reg: iNegCurr
        /// </summary>
        public int CurrNegativeFormat;
        /// <summary>
        /// Reg: sMonDecimalSep
        /// </summary>
        public string CurrDecimalSymbol;
        /// <summary>
        /// Reg: iCurrDigits
        /// </summary>
        public int CurrNumDigitsAfterDec;
        /// <summary>
        /// Reg: sMonThousandSep
        /// </summary>
        public string CurrDigitGroupingSymbol;
        /// <summary>
        /// Reg: sMonGrouping
        /// 0 = "0"
        /// 1 = "3;0"
        /// 2 = "3;0;0"
        /// 3 = "3;2;0"
        /// </summary>
        public int CurrDigitGrouping;

        /// <summary>
        /// Reg: sTime
        /// :
        /// </summary>
        public string TimeSeparator;
        /// <summary>
        /// Reg: sShortTime
        /// </summary>
        public string TimeShortTime;
        /// <summary>
        /// Reg: sTimeFormat
        /// </summary>
        public string TimeLongTime;
        /// <summary>
        /// Reg: s1159
        /// </summary>
        public string TimeAM;
        /// <summary>
        /// Reg: s2359
        /// </summary>
        public string TimePM;
        /// <summary>
        /// Reg: iTLZero
        /// </summary>
        public int TimeShortTimePrefix0Hour;

        /// <summary>
        /// Reg: iTime
        /// </summary>
        public int Time24Hour;

        /// <summary>
        /// Reg: sDate
        /// /
        /// </summary>
        public string DateSeparator;

        /// <summary>
        /// Reg: sShortDate
        /// </summary>
        public string DateShortDate;
        /// <summary>
        /// Reg: sLongDate
        /// </summary>
        public string DateLongDate;
        /// <summary>
        /// Reg: sYearMonth
        /// </summary>
        public string DateYearMonth;
        /// <summary>
        /// Reg: iFirstDayOfWeek
        /// </summary>
        public int Date1stDayOfWeek;
        /// <summary>
        /// Reg: .\Calendars\TwoDigitYearMax
        /// Values: 1, 10, 11, 12, 2, 9
        /// Data: full year: 2039
        /// </summary>
        public int Date2DigitYear;
        /// <summary>
        /// Reg: iFirstWeekOfYear
        /// </summary>
        public int DateFirstWeekOfYear;
        /// <summary>
        /// Reg: iDate
        /// </summary>
        public int DateFormat;

        /// <summary>
        /// Reg: iPaperSize
        /// </summary>
        public int PaperSize;

        public string KeyboardLayout1;
        public string KeyboardLayout2;
        public string KeyboardLayout3;

        public bool? DeleteOtherKeyboardLayouts;

        public bool? EnableMainFormat;
        public bool? EnableMainLocation;
        public bool? EnableMainSystemLocale;
        public bool? EnableTimeZone;
        public bool? EnableTelephoneIDN;
        public bool? EnableNumNumOfDigitsAfterDec;
        public bool? EnableNumDecimalSymbol;
        public bool? EnableNumDigitGroupingSymbol;
        public bool? EnableNumDigitGrouping;
        public bool? EnableNumNegativeSignSymbol;
        public bool? EnableNumPositiveSignSymbol;
        public bool? EnableNumNegativeNumberFormat;
        public bool? EnableNumDisplayLeading0;
        public bool? EnableNumListSeparator;
        public bool? EnableNumMeasurement;
        public bool? EnableNumStdDigits;
        public bool? EnableNumUseNativeDigits;
        public bool? EnableCurrCurrencySymbol;
        public bool? EnableCurrPositiveFormat;
        public bool? EnableCurrNegativeFormat;
        public bool? EnableCurrDecimalSymbol;
        public bool? EnableCurrNumDigitsAfterDec;
        public bool? EnableCurrDigitGroupingSymbol;
        public bool? EnableCurrDigitGrouping;
        public bool? EnableTimeSeparator;
        public bool? EnableTimeShortTime;
        public bool? EnableTimeLongTime;
        public bool? EnableTimeAM;
        public bool? EnableTimePM;
        public bool? EnableDateShortDate;
        public bool? EnableDateLongDate;
        public bool? EnableDateFormat;
        public bool? EnableDate1stDayOfWeek;
        public bool? EnableDate2DigitYear;
        public bool? EnableKeyboardLayout1;
        public bool? EnableKeyboardLayout2;
        public bool? EnableKeyboardLayout3;

        public bool? EnableTimeShortPrefix0Hour;
        public bool? EnableTime24Hour;
        public bool? EnableDateSeparator;
        public bool? EnableDateFirstWeekOfYear;
        public bool? EnablePaperSize;
    }

    public class ClientSettingsPolicy
    {
        public Int64 ID;
        public Int64 Order;

        public bool? DisableEventLogSync;
        public bool? DisableAddRemoveProgramsSync;
        public bool? DisableDiskDataSync;
        public bool? DisableNetadapterSync;
        public bool? DisableDeviceManagerSync;
        public bool? DisableFilterDriverSync;
        public bool? DisableWinLicenseSync;
        public bool? DisableUsersSync;
        public bool? DisableStartupSync;
        public bool? EnableBitlockerRKSync;
        public bool? DisableSMARTSync;
        public bool? DisableSimpleTasks;

        public bool? EnableAdditionalEventLogs;
        public string AdditionalEventLogs;
    }
}
