using FoxSDC_Common;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.PolicyObjects
{
    [PolicyObjectAttr(PolicyIDs.InternationalSettings)]
    public class Intl : IPolicyClass
    {
        [StructLayout(LayoutKind.Sequential)]
        class REG_TZI_FORMAT
        {
            public int BIAS;
            public int StandardBias;
            public int DaylightBias;
            public SYSTEMTIME StandardDate;
            public SYSTEMTIME DaylightDate;
        }

        [StructLayout(LayoutKind.Sequential)]
        class SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        static List<InternationalPolicy> ToAdd;
        static List<InternationalPolicy> ToRemove;
        static List<InternationalPolicy> ActivePolicies = new List<InternationalPolicy>();
        static InternationalPolicy RunningPolicy;

        public bool ApplyOrdering(LoadedPolicyObject policy, long Ordering)
        {
            InternationalPolicy t = JsonConvert.DeserializeObject<InternationalPolicy>(policy.PolicyObject.Data);
            foreach (InternationalPolicy w in ToAdd)
            {
                if (policy.PolicyObject.ID == w.ID)
                {
                    w.Order = Ordering;
                }
            }
            foreach (InternationalPolicy w in ActivePolicies)
            {
                if (policy.PolicyObject.ID == w.ID)
                {
                    w.Order = Ordering;
                }
            }
            return (true);
        }

        public bool ApplyPolicy(LoadedPolicyObject policy)
        {
            InternationalPolicy t = JsonConvert.DeserializeObject<InternationalPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToAdd.Add(t);
            return (true);
        }

        void Merge()
        {
            List<InternationalPolicy> rm = new List<InternationalPolicy>();
            foreach (InternationalPolicy ap in ToRemove)
            {
                foreach (InternationalPolicy a in ActivePolicies)
                {
                    if (ap.ID == a.ID)
                        rm.Add(a);
                }
            }

            foreach (InternationalPolicy r in rm)
            {
                ActivePolicies.Remove(r);
            }

            foreach (InternationalPolicy a in ToAdd)
            {
                ActivePolicies.Add(a);
            }

            rm.Clear();
            ToAdd.Clear();
            ToRemove.Clear();

            ActivePolicies.Sort((x, y) => x.Order.CompareTo(y.Order));

            RunningPolicy = new InternationalPolicy();
            foreach (InternationalPolicy p in ActivePolicies)
            {
                if (p.EnableCurrCurrencySymbol != null)
                    RunningPolicy.EnableCurrCurrencySymbol = p.EnableCurrCurrencySymbol;
                if (p.EnableCurrCurrencySymbol == true)
                    RunningPolicy.CurrCurrencySymbol = p.CurrCurrencySymbol;

                if (p.EnableCurrDecimalSymbol != null)
                    RunningPolicy.EnableCurrDecimalSymbol = p.EnableCurrDecimalSymbol;
                if (p.EnableCurrDecimalSymbol == true)
                    RunningPolicy.CurrDecimalSymbol = p.CurrDecimalSymbol;

                if (p.EnableCurrDigitGrouping != null)
                    RunningPolicy.EnableCurrDigitGrouping = p.EnableCurrDigitGrouping;
                if (p.EnableCurrDigitGrouping == true)
                    RunningPolicy.CurrDigitGrouping = p.CurrDigitGrouping;

                if (p.EnableCurrDigitGroupingSymbol != null)
                    RunningPolicy.EnableCurrDigitGroupingSymbol = p.EnableCurrDigitGroupingSymbol;
                if (p.EnableCurrDigitGroupingSymbol == true)
                    RunningPolicy.CurrDigitGroupingSymbol = p.CurrDigitGroupingSymbol;

                if (p.EnableCurrNegativeFormat != null)
                    RunningPolicy.EnableCurrNegativeFormat = p.EnableCurrNegativeFormat;
                if (p.EnableCurrNegativeFormat == true)
                    RunningPolicy.CurrNegativeFormat = p.CurrNegativeFormat;

                if (p.EnableCurrNumDigitsAfterDec != null)
                    RunningPolicy.EnableCurrNumDigitsAfterDec = p.EnableCurrNumDigitsAfterDec;
                if (p.EnableCurrNumDigitsAfterDec == true)
                    RunningPolicy.CurrNumDigitsAfterDec = p.CurrNumDigitsAfterDec;

                if (p.EnableCurrPositiveFormat != null)
                    RunningPolicy.EnableCurrPositiveFormat = p.EnableCurrPositiveFormat;
                if (p.EnableCurrPositiveFormat == true)
                    RunningPolicy.CurrPositiveFormat = p.CurrPositiveFormat;

                if (p.EnableDate1stDayOfWeek != null)
                    RunningPolicy.EnableDate1stDayOfWeek = p.EnableDate1stDayOfWeek;
                if (p.EnableDate1stDayOfWeek == true)
                    RunningPolicy.Date1stDayOfWeek = p.Date1stDayOfWeek;

                if (p.EnableDate2DigitYear != null)
                    RunningPolicy.EnableDate2DigitYear = p.EnableDate2DigitYear;
                if (p.EnableDate2DigitYear == true)
                    RunningPolicy.Date2DigitYear = p.Date2DigitYear;

                if (p.EnableDateLongDate != null)
                    RunningPolicy.EnableDateLongDate = p.EnableDateLongDate;
                if (p.EnableDateLongDate == true)
                    RunningPolicy.DateLongDate = p.DateLongDate;

                if (p.EnableDateShortDate != null)
                    RunningPolicy.EnableDateShortDate = p.EnableDateShortDate;
                if (p.EnableDateShortDate == true)
                    RunningPolicy.DateShortDate = p.DateShortDate;

                if (p.EnableKeyboardLayout1 != null)
                    RunningPolicy.EnableKeyboardLayout1 = p.EnableKeyboardLayout1;
                if (p.EnableKeyboardLayout1 == true)
                    RunningPolicy.KeyboardLayout1 = p.KeyboardLayout1;

                if (p.EnableKeyboardLayout2 != null)
                    RunningPolicy.EnableKeyboardLayout2 = p.EnableKeyboardLayout2;
                if (p.EnableKeyboardLayout2 == true)
                    RunningPolicy.KeyboardLayout2 = p.KeyboardLayout2;

                if (p.EnableKeyboardLayout3 != null)
                    RunningPolicy.EnableKeyboardLayout3 = p.EnableKeyboardLayout3;
                if (p.EnableKeyboardLayout3 == true)
                    RunningPolicy.KeyboardLayout3 = p.KeyboardLayout3;

                if (p.EnableMainFormat != null)
                    RunningPolicy.EnableMainFormat = p.EnableMainFormat;
                if (p.EnableMainFormat == true)
                    RunningPolicy.MainFormat = p.MainFormat;

                if (p.EnableMainLocation != null)
                    RunningPolicy.EnableMainLocation = p.EnableMainLocation;
                if (p.EnableMainLocation == true)
                    RunningPolicy.MainLocation = p.MainLocation;

                if (p.EnableMainSystemLocale != null)
                    RunningPolicy.EnableMainSystemLocale = p.EnableMainSystemLocale;
                if (p.EnableMainSystemLocale == true)
                    RunningPolicy.MainSystemLocale = p.MainSystemLocale;

                if (p.EnableNumDecimalSymbol != null)
                    RunningPolicy.EnableNumDecimalSymbol = p.EnableNumDecimalSymbol;
                if (p.EnableNumDecimalSymbol == true)
                    RunningPolicy.NumDecimalSymbol = p.NumDecimalSymbol;

                if (p.EnableNumDigitGrouping != null)
                    RunningPolicy.EnableNumDigitGrouping = p.EnableNumDigitGrouping;
                if (p.EnableNumDigitGrouping == true)
                    RunningPolicy.NumDigitGrouping = p.NumDigitGrouping;

                if (p.EnableNumDigitGroupingSymbol != null)
                    RunningPolicy.EnableNumDigitGroupingSymbol = p.EnableNumDigitGroupingSymbol;
                if (p.EnableNumDigitGroupingSymbol == true)
                    RunningPolicy.NumDigitGroupingSymbol = p.NumDigitGroupingSymbol;

                if (p.EnableNumDisplayLeading0 != null)
                    RunningPolicy.EnableNumDisplayLeading0 = p.EnableNumDisplayLeading0;
                if (p.EnableNumDisplayLeading0 == true)
                    RunningPolicy.NumDisplayLeading0 = p.NumDisplayLeading0;

                if (p.EnableNumListSeparator != null)
                    RunningPolicy.EnableNumListSeparator = p.EnableNumListSeparator;
                if (p.EnableNumListSeparator == true)
                    RunningPolicy.NumListSeparator = p.NumListSeparator;

                if (p.EnableNumMeasurement != null)
                    RunningPolicy.EnableNumMeasurement = p.EnableNumMeasurement;
                if (p.EnableNumMeasurement == true)
                    RunningPolicy.NumMeasurement = p.NumMeasurement;

                if (p.EnableNumNegativeNumberFormat != null)
                    RunningPolicy.EnableNumNegativeNumberFormat = p.EnableNumNegativeNumberFormat;
                if (p.EnableNumNegativeNumberFormat == true)
                    RunningPolicy.NumNegativeNumberFormat = p.NumNegativeNumberFormat;

                if (p.EnableNumNegativeSignSymbol != null)
                    RunningPolicy.EnableNumNegativeSignSymbol = p.EnableNumNegativeSignSymbol;
                if (p.EnableNumNegativeSignSymbol == true)
                    RunningPolicy.NumNegativeSignSymbol = p.NumNegativeSignSymbol;

                if (p.EnableNumNumOfDigitsAfterDec != null)
                    RunningPolicy.EnableNumNumOfDigitsAfterDec = p.EnableNumNumOfDigitsAfterDec;
                if (p.EnableNumNumOfDigitsAfterDec == true)
                    RunningPolicy.NumNumOfDigitsAfterDec = p.NumNumOfDigitsAfterDec;

                if (p.EnableNumStdDigits != null)
                    RunningPolicy.EnableNumStdDigits = p.EnableNumStdDigits;
                if (p.EnableNumStdDigits == true)
                    RunningPolicy.NumStdDigits = p.NumStdDigits;

                if (p.EnableNumUseNativeDigits != null)
                    RunningPolicy.EnableNumUseNativeDigits = p.EnableNumUseNativeDigits;
                if (p.EnableNumUseNativeDigits == true)
                    RunningPolicy.NumUseNativeDigits = p.NumUseNativeDigits;

                if (p.EnableTelephoneIDN != null)
                    RunningPolicy.EnableTelephoneIDN = p.EnableTelephoneIDN;
                if (p.EnableTelephoneIDN == true)
                    RunningPolicy.TelephoneIDN = p.TelephoneIDN;

                if (p.EnableTimeAM != null)
                    RunningPolicy.EnableTimeAM = p.EnableTimeAM;
                if (p.EnableTimeAM == true)
                    RunningPolicy.TimeAM = p.TimeAM;

                if (p.EnableTimePM != null)
                    RunningPolicy.EnableTimePM = p.EnableTimePM;
                if (p.EnableTimePM == true)
                    RunningPolicy.TimePM = p.TimePM;

                if (p.EnableTimeLongTime != null)
                    RunningPolicy.EnableTimeLongTime = p.EnableTimeLongTime;
                if (p.EnableTimeLongTime == true)
                    RunningPolicy.TimeLongTime = p.TimeLongTime;

                if (p.EnableTimeShortTime != null)
                    RunningPolicy.EnableTimeShortTime = p.EnableTimeShortTime;
                if (p.EnableTimeShortTime == true)
                    RunningPolicy.TimeShortTime = p.TimeShortTime;

                if (p.EnableTimeZone != null)
                    RunningPolicy.EnableTimeZone = p.EnableTimeZone;
                if (p.EnableTimeZone == true)
                    RunningPolicy.TimeZone = p.TimeZone;

                if (p.EnableNumPositiveSignSymbol != null)
                    RunningPolicy.EnableNumPositiveSignSymbol = p.EnableNumPositiveSignSymbol;
                if (p.EnableNumPositiveSignSymbol == true)
                    RunningPolicy.NumPositiveSignSymbol = p.NumPositiveSignSymbol;

                if (p.DeleteOtherKeyboardLayouts != null)
                    RunningPolicy.DeleteOtherKeyboardLayouts = p.DeleteOtherKeyboardLayouts;
                if (p.AutoDST != null)
                    RunningPolicy.AutoDST = p.AutoDST;

                if (p.EnableTimeShortPrefix0Hour != null)
                    RunningPolicy.EnableTimeShortPrefix0Hour = p.EnableTimeShortPrefix0Hour;
                if (p.EnableTimeShortPrefix0Hour == true)
                    RunningPolicy.TimeShortTimePrefix0Hour = p.TimeShortTimePrefix0Hour;

                if (p.EnableTime24Hour != null)
                    RunningPolicy.EnableTime24Hour = p.EnableTime24Hour;
                if (p.EnableTime24Hour == true)
                    RunningPolicy.Time24Hour = p.Time24Hour;

                if (p.EnableDateSeparator != null)
                    RunningPolicy.EnableDateSeparator = p.EnableDateSeparator;
                if (p.EnableDateSeparator == true)
                    RunningPolicy.DateSeparator = p.DateSeparator;

                if (p.EnableDateFirstWeekOfYear != null)
                    RunningPolicy.EnableDateFirstWeekOfYear = p.EnableDateFirstWeekOfYear;
                if (p.EnableDateFirstWeekOfYear == true)
                    RunningPolicy.DateFirstWeekOfYear = p.DateFirstWeekOfYear;

                if (p.EnablePaperSize != null)
                    RunningPolicy.EnablePaperSize = p.EnablePaperSize;
                if (p.EnablePaperSize == true)
                    RunningPolicy.PaperSize = p.PaperSize;
            }
        }

        void ApplyPolicy(InternationalPolicy p)
        {
            if (p.EnableMainSystemLocale == true)
            {
                try
                {
                    CultureInfo c = new CultureInfo(p.MainSystemLocale);
                    RegistryKey RK = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Nls\\Language", true);
                    if (RK == null)
                    {
                        FoxEventLog.WriteEventLog("Cannot open SYSTEM\\CurrentControlSet\\Control\\Nls\\Language - Registry broken?", System.Diagnostics.EventLogEntryType.Error);
                    }
                    else
                    {
                        RK.SetValue("Default", p.MainSystemLocale.ToString("X4"), RegistryValueKind.String);
                        RK.Close();
                    }

                    RK = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Nls\\Locale", true);
                    if (RK == null)
                    {
                        FoxEventLog.WriteEventLog("Cannot open SYSTEM\\CurrentControlSet\\Control\\Nls\\Locale - Registry broken?", System.Diagnostics.EventLogEntryType.Error);
                    }
                    else
                    {
                        RK.SetValue(null, p.MainSystemLocale.ToString("X8"), RegistryValueKind.String);
                        RK.Close();
                    }
                }
                catch
                {
                    FoxEventLog.WriteEventLog("Cannot apply System Locale for ID " + p.MainSystemLocale, System.Diagnostics.EventLogEntryType.Warning);
                }
            }

            if (p.EnableTimeZone == true)
            {
                RegistryKey RK = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
                if (RK == null)
                {
                    FoxEventLog.WriteEventLog("Cannot read SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones - Registry broken?", System.Diagnostics.EventLogEntryType.Error);
                }
                else
                {
                    RegistryKey RK2 = RK.OpenSubKey(p.TimeZone, false);
                    if (RK2 == null)
                    {
                        FoxEventLog.WriteEventLog("Cannot find informations about TimeZone " + p.TimeZone, System.Diagnostics.EventLogEntryType.Warning);
                    }
                    else
                    {
                        object TZIo = RK2.GetValue("TZI", null);
                        if (!(TZIo is byte[]))
                        {
                            FoxEventLog.WriteEventLog("Faulty TZI in TimeZone " + p.TimeZone, System.Diagnostics.EventLogEntryType.Warning);
                        }
                        else
                        {
                            REG_TZI_FORMAT TZI = ClassCopy.CopyClassDatafromBinary<REG_TZI_FORMAT>((byte[])TZIo);

                            RegistryKey RKD = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\TimeZoneInformation", true);
                            if (RKD == null)
                            {
                                FoxEventLog.WriteEventLog("Cannot open SYSTEM\\CurrentControlSet\\Control\\TimeZoneInformation - Registry broken?", System.Diagnostics.EventLogEntryType.Error);
                            }
                            else
                            {
                                RKD.SetValue("Bias", TZI.BIAS, RegistryValueKind.DWord);
                                RKD.SetValue("DaylightBias", TZI.DaylightBias, RegistryValueKind.DWord);
                                if (p.AutoDST == true)
                                    RKD.SetValue("DynamicDaylightTimeDisabled", 0, RegistryValueKind.DWord);
                                else
                                    RKD.SetValue("DynamicDaylightTimeDisabled", 1, RegistryValueKind.DWord);
                                RKD.SetValue("DaylightStart", ClassCopy.CopyClassDataGetBytes(TZI.DaylightDate), RegistryValueKind.Binary);
                                RKD.SetValue("StandardStart", ClassCopy.CopyClassDataGetBytes(TZI.StandardDate), RegistryValueKind.Binary);
                                RKD.SetValue("TimeZoneKeyName", p.TimeZone);
                                RKD.Close();

                                if (string.IsNullOrWhiteSpace(Convert.ToString(RK2.GetValue("MUI_Dlt", ""))) == false)
                                {
                                    RKD.SetValue("DaylightName", Convert.ToString(RK2.GetValue("MUI_Dlt", "")).Trim(), RegistryValueKind.String);
                                }
                                if (string.IsNullOrWhiteSpace(Convert.ToString(RK2.GetValue("MUI_Std", ""))) == false)
                                {
                                    RKD.SetValue("StandardName", Convert.ToString(RK2.GetValue("MUI_Std", "")).Trim(), RegistryValueKind.String);
                                }


                                RKD.Close();
                            }
                        }
                        RK2.Close();
                    }
                    RK.Close();
                }
            }
        }

        void ApplyUserPolicy(InternationalPolicy p)
        {
            RegistryKey intluser = Registry.CurrentUser.OpenSubKey("Control Panel\\International", true);
            if (intluser == null)
            {
                FoxEventLog.WriteEventLog("Cannot open HKCU\\Control Panel\\International - Registry broken?", System.Diagnostics.EventLogEntryType.Error);
                return;
            }

            if (p.EnableMainLocation == true)
            {
                RegistryKey k = intluser.CreateSubKey("Geo");
                k.SetValue("Nation", p.MainLocation, RegistryValueKind.String);
                k.Close();
            }

            if (p.EnableDate2DigitYear == true)
            {
                RegistryKey k = intluser.CreateSubKey("Calendars\\TwoDigitYearMax");
                k.SetValue("1", p.Date2DigitYear, RegistryValueKind.String);
                k.SetValue("10", p.Date2DigitYear, RegistryValueKind.String);
                k.SetValue("11", p.Date2DigitYear, RegistryValueKind.String);
                k.SetValue("12", p.Date2DigitYear, RegistryValueKind.String);
                k.SetValue("2", p.Date2DigitYear, RegistryValueKind.String);
                k.SetValue("9", p.Date2DigitYear, RegistryValueKind.String);
                k.Close();
            }

            if (p.EnableMainFormat == true)
            {
                try
                {
                    CultureInfo c = new CultureInfo(p.MainFormat);
                    RegionInfo regi = new RegionInfo(p.MainFormat);

                    intluser.SetValue("sLanguage", c.ThreeLetterWindowsLanguageName, RegistryValueKind.String);
                    intluser.SetValue("sCountry", regi.NativeName, RegistryValueKind.String);
                    intluser.SetValue("LocaleName", c.IetfLanguageTag, RegistryValueKind.String);
                    intluser.SetValue("Locale", p.MainFormat.ToString("X8"));

                    Console.WriteLine(c.ThreeLetterWindowsLanguageName); //ENG
                    Console.WriteLine(c.IetfLanguageTag); //en-GB
                    Console.WriteLine(regi.NativeName); //United Kingdom
                }
                catch
                {
                    FoxEventLog.WriteEventLog("Cannot apply main region settings for ID " + p.MainFormat, System.Diagnostics.EventLogEntryType.Warning);
                }
            }

            if (p.EnableCurrCurrencySymbol == true)
                intluser.SetValue("sCurrency", p.CurrCurrencySymbol, RegistryValueKind.String);

            if (p.EnableCurrDecimalSymbol == true)
                intluser.SetValue("sMonDecimalSep", p.CurrDecimalSymbol, RegistryValueKind.String);

            if (p.EnableCurrDigitGrouping == true)
            {
                switch (p.CurrDigitGrouping)
                {
                    case 0:
                        intluser.SetValue("sMonGrouping", "0", RegistryValueKind.String);
                        break;
                    case 1:
                        intluser.SetValue("sMonGrouping", "3;0", RegistryValueKind.String);
                        break;
                    case 2:
                        intluser.SetValue("sMonGrouping", "3;0;0", RegistryValueKind.String);
                        break;
                    case 3:
                        intluser.SetValue("sMonGrouping", "3;2;0", RegistryValueKind.String);
                        break;
                    default:
                        FoxEventLog.WriteEventLog("Invalid policy value in Intl Policy \"CurrDigitGrouping\"", System.Diagnostics.EventLogEntryType.Warning);
                        break;
                }
            }

            if (p.EnableCurrDigitGroupingSymbol == true)
                intluser.SetValue("sMonThousandSep", p.CurrDigitGroupingSymbol, RegistryValueKind.String);

            if (p.EnableCurrNegativeFormat == true)
                intluser.SetValue("iNegCurr", p.CurrNegativeFormat, RegistryValueKind.String);

            if (p.EnableCurrNumDigitsAfterDec == true)
                intluser.SetValue("iCurrDigits", p.CurrNumDigitsAfterDec, RegistryValueKind.String);

            if (p.EnableCurrPositiveFormat == true)
                intluser.SetValue("iCurrency", p.CurrPositiveFormat, RegistryValueKind.String);

            if (p.EnableDate1stDayOfWeek == true)
                intluser.SetValue("iFirstDayOfWeek", p.Date1stDayOfWeek, RegistryValueKind.String);

            if (p.EnableDateFirstWeekOfYear == true)
                intluser.SetValue("iFirstWeekOfYear", p.DateFirstWeekOfYear, RegistryValueKind.String);

            if (p.EnableDateFormat == true)
                intluser.SetValue("iDate", p.DateFormat, RegistryValueKind.String);

            if (p.EnableDateLongDate == true)
                intluser.SetValue("sLongDate", p.DateLongDate, RegistryValueKind.String);

            if (p.EnableDateSeparator == true)
                intluser.SetValue("sDate", p.DateSeparator, RegistryValueKind.String);

            if (p.EnableDateShortDate == true)
                intluser.SetValue("sShortDate", p.DateShortDate, RegistryValueKind.String);

            if (p.EnableNumDecimalSymbol == true)
                intluser.SetValue("sDecimal", p.NumDecimalSymbol, RegistryValueKind.String);

            if (p.EnableNumDigitGrouping == true)
            {
                switch (p.NumDigitGrouping)
                {
                    case 0:
                        intluser.SetValue("sGrouping", "0", RegistryValueKind.String);
                        break;
                    case 1:
                        intluser.SetValue("sGrouping", "3;0", RegistryValueKind.String);
                        break;
                    case 2:
                        intluser.SetValue("sGrouping", "3;0;0", RegistryValueKind.String);
                        break;
                    case 3:
                        intluser.SetValue("sGrouping", "3;2;0", RegistryValueKind.String);
                        break;
                    default:
                        FoxEventLog.WriteEventLog("Invalid policy value in Intl Policy \"NumDigitGrouping\"", System.Diagnostics.EventLogEntryType.Warning);
                        break;
                }
            }

            if (p.EnableNumDigitGroupingSymbol == true)
                intluser.SetValue("sThousand", p.NumDigitGroupingSymbol, RegistryValueKind.String);


            if (p.EnableNumDisplayLeading0 == true)
                intluser.SetValue("iLZero", p.NumDisplayLeading0, RegistryValueKind.String);

            if (p.EnableNumListSeparator == true)
                intluser.SetValue("sList", p.NumListSeparator, RegistryValueKind.String);

            if (p.EnableNumMeasurement == true)
                intluser.SetValue("iMeasure", p.NumMeasurement, RegistryValueKind.String);

            if (p.EnableNumNegativeNumberFormat == true)
                intluser.SetValue("iNegNumber", p.NumNegativeNumberFormat, RegistryValueKind.String);

            if (p.EnableNumNegativeSignSymbol == true)
                intluser.SetValue("sNegativeSign", p.NumNegativeSignSymbol, RegistryValueKind.String);

            if (p.EnableNumNumOfDigitsAfterDec == true)
                intluser.SetValue("iDigits", p.NumNumOfDigitsAfterDec, RegistryValueKind.String);

            if (p.EnableNumPositiveSignSymbol == true)
                intluser.SetValue("sPositiveSign", p.NumPositiveSignSymbol, RegistryValueKind.String);

            if (p.EnableNumStdDigits == true)
            {
                switch (p.NumStdDigits)
                {
                    case 0:
                        intluser.SetValue("sNativeDigits", "0123456789", RegistryValueKind.String);
                        break;
                    default:
                        FoxEventLog.WriteEventLog("Invalid policy value in Intl Policy \"NumStdDigits\"", System.Diagnostics.EventLogEntryType.Warning);
                        break;
                }
            }

            if (p.EnableNumUseNativeDigits == true)
                intluser.SetValue("NumShape", p.NumUseNativeDigits, RegistryValueKind.String);

            if (p.EnablePaperSize == true)
                intluser.SetValue("iPaperSize", p.PaperSize, RegistryValueKind.String);

            if (p.EnableTelephoneIDN == true)
                intluser.SetValue("iCountry", p.TelephoneIDN, RegistryValueKind.String);

            if (p.EnableTime24Hour == true)
                intluser.SetValue("iTime", p.Time24Hour, RegistryValueKind.String);

            if (p.EnableTimeAM == true)
                intluser.SetValue("s1159", p.TimeAM, RegistryValueKind.String);

            if (p.EnableTimePM == true)
                intluser.SetValue("s2359", p.TimePM, RegistryValueKind.String);

            if (p.EnableTimeLongTime == true)
                intluser.SetValue("sTimeFormat", p.TimeLongTime, RegistryValueKind.String);

            if (p.EnableTimeShortPrefix0Hour == true)
                intluser.SetValue("iTLZero", p.TimeShortTimePrefix0Hour, RegistryValueKind.String);

            if (p.EnableTimeShortTime == true)
                intluser.SetValue("sTime", p.TimeShortTime, RegistryValueKind.String);

            intluser.Close();
        }

        public bool FinaliseApplyPolicy()
        {
            if (SystemInfos.SysInfo.RunningInWindowsPE == true)
                return (true);

            Merge();
            ApplyPolicy(RunningPolicy);

            return (true);
        }

        public bool FinaliseApplyPolicyUserPart()
        {
            if (SystemInfos.SysInfo.RunningInWindowsPE == true)
                return (true);

            Merge();
            ApplyUserPolicy(RunningPolicy);

            return (true);
        }

        public bool FinaliseUninstallProgramm()
        {
            return (true);
        }

        public bool PreApplyPolicy()
        {
            ToAdd = new List<InternationalPolicy>();
            ToRemove = new List<InternationalPolicy>();
            return (true);
        }

        public bool RemovePolicy(LoadedPolicyObject policy)
        {
            InternationalPolicy t = JsonConvert.DeserializeObject<InternationalPolicy>(policy.PolicyObject.Data);
            t.ID = policy.PolicyObject.ID;
            ToRemove.Add(t);
            return (true);
        }

        public bool UpdatePolicy(LoadedPolicyObject oldpolicy, LoadedPolicyObject newpolicy)
        {
            InternationalPolicy t1 = JsonConvert.DeserializeObject<InternationalPolicy>(oldpolicy.PolicyObject.Data);
            InternationalPolicy t2 = JsonConvert.DeserializeObject<InternationalPolicy>(newpolicy.PolicyObject.Data);

            t1.ID = oldpolicy.PolicyObject.ID;
            t2.ID = newpolicy.PolicyObject.ID;
            ToAdd.Add(t2);
            ToRemove.Add(t1);
            return (true);
        }
    }
}
