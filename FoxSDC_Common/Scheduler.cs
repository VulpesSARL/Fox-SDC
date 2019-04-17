using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public enum SchedulerPlanningType : int
    {
        None = 0,
        OneTime = 1,
        Daily = 2,
        Weekly = 3,
        Monthly = 4
    }

    public class SchedulerPlanning
    {
        public SchedulerPlanningType Planning;
        public DateTime StartDate;
        /// <summary>
        /// Daily = n days</br>
        /// Weekly = n weeks
        /// </summary>
        public int? Recurrence;
        public List<DayOfWeek> RecurInWeekDays;
        public List<int> RecurInMonths;
        public List<int> RecurInDay;
    }

    public class Scheduler
    {
        public static string Explain(SchedulerPlanning Plan)
        {
            if (Plan == null)
                Plan = Scheduler.Nix;

            string s = "<Invalid>";

            switch (Plan.Planning)
            {
                case SchedulerPlanningType.None:
                    s = "No schedule";
                    break;
                case SchedulerPlanningType.OneTime:
                    s = "Once at " + Plan.StartDate.ToLocalTime().ToLongDateString() + " " + Plan.StartDate.ToLocalTime().ToLongTimeString();
                    break;
                case SchedulerPlanningType.Daily:
                    if (Plan.Recurrence == null)
                        break;
                    s = "Starting from " + Plan.StartDate.ToLocalTime().ToLongDateString() + " " + Plan.StartDate.ToLocalTime().ToLongTimeString() + ", every " +
                        Plan.Recurrence.ToString() + " day" + (Plan.Recurrence == 1 ? "" : "s");
                    break;
                case SchedulerPlanningType.Weekly:
                    if (Plan.Recurrence == null)
                        break;
                    if (Plan.RecurInWeekDays == null || Plan.RecurInWeekDays.Count == 0)
                        break;
                    s = "Starting from " + Plan.StartDate.ToLocalTime().ToLongDateString() + " " + Plan.StartDate.ToLocalTime().ToLongTimeString() + ", every " +
                        Plan.Recurrence.ToString() + " week" + (Plan.Recurrence == 1 ? "" : "s") + " on ";
                    foreach (DayOfWeek dow in Plan.RecurInWeekDays)
                    {
                        s += Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(dow) + ", ";
                    }
                    if (s.EndsWith(", ") == true)
                        s = s.Substring(0, s.Length - 2);
                    break;
                case SchedulerPlanningType.Monthly:
                    if (Plan.RecurInDay == null || Plan.RecurInDay.Count == 0)
                        break;
                    if (Plan.RecurInMonths == null || Plan.RecurInMonths.Count == 0)
                        break;
                    s = "Starting from " + Plan.StartDate.ToLocalTime().ToLongDateString() + " " + Plan.StartDate.ToLocalTime().ToLongTimeString() + ", every ";
                    foreach (int Month in Plan.RecurInMonths)
                        s += Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(Month) + ", ";
                    if (s.EndsWith(", ") == true)
                        s = s.Substring(0, s.Length - 2);
                    s += " on these day" + (Plan.RecurInDay.Count == 1 ? "" : "s") + " ";
                    foreach (int days in Plan.RecurInDay)
                        s += days.ToString() + ", ";
                    if (s.EndsWith(", ") == true)
                        s = s.Substring(0, s.Length - 2);
                    break;
            }

            return (s);
        }

        public static SchedulerPlanning Nix
        {
            get
            {
                SchedulerPlanning p = new SchedulerPlanning();
                p.Planning = SchedulerPlanningType.None;
                return (p);
            }
        }

        public static SchedulerPlanning CreatePlan()
        {
            SchedulerPlanning p = new SchedulerPlanning();
            p.Planning = SchedulerPlanningType.None;
            return (p);
        }

        public static SchedulerPlanning CreatePlan(DateTime OneTime)
        {
            SchedulerPlanning p = new SchedulerPlanning();
            p.Planning = SchedulerPlanningType.OneTime;
            p.StartDate = OneTime;
            return (p);
        }

        public static SchedulerPlanning CreatePlan(DateTime StartTime, int RecurrenceDays)
        {
            SchedulerPlanning p = new SchedulerPlanning();
            p.Planning = SchedulerPlanningType.Daily;
            p.StartDate = StartTime;
            p.Recurrence = RecurrenceDays;
            return (p);
        }

        public static SchedulerPlanning CreatePlan(DateTime StartTime, int RecurrenceWeeks, List<DayOfWeek> Days)
        {
            SchedulerPlanning p = new SchedulerPlanning();
            p.Planning = SchedulerPlanningType.Weekly;
            p.StartDate = StartTime;
            p.Recurrence = RecurrenceWeeks;
            p.RecurInWeekDays = Days;
            return (p);
        }

        public static SchedulerPlanning CreatePlan(DateTime StartTime, List<int> Days, List<int> Months)
        {
            SchedulerPlanning p = new SchedulerPlanning();
            p.Planning = SchedulerPlanningType.Monthly;
            p.StartDate = StartTime;
            p.RecurInMonths = Months;
            p.RecurInDay = Days;
            return (p);
        }

        static DateTime? SetToNextDay(DateTime DT, List<DayOfWeek> RecurInWeekDays)
        {
            if (RecurInWeekDays == null || RecurInWeekDays.Count == 0)
                return (null);
            do
            {
                DT = DT.AddDays(1);
                bool CorrectDOW = false;
                foreach (DayOfWeek dow in RecurInWeekDays)
                {
                    if (DT.DayOfWeek == dow)
                    {
                        CorrectDOW = true;
                        break;
                    }
                }
                if (CorrectDOW == true)
                    return (DT);
            } while (true);
        }

        static DateTime? SetToNextDay(DateTime DT, List<int> RecurInDay, List<int> RecurInMonths)
        {
            if (RecurInDay == null || RecurInDay.Count == 0)
                return (null);
            if (RecurInMonths == null || RecurInMonths.Count == 0)
                return (null);

            DateTime BackupDT = DT;
            do
            {
                DT = DT.AddDays(1);
                if ((DT - BackupDT).TotalDays > 365 * 5)
                    return (null);
                bool Found = false;
                foreach (int Month in RecurInMonths)
                {
                    if (DT.Month == Month)
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found == false)
                    continue;
                Found = false;
                foreach (int Day in RecurInDay)
                {
                    if (DT.Day == Day)
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found == false)
                    continue;
                return (DT);
            } while (true);
        }

        static int GetWeekNumber(DateTime DT)
        {
            return (new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(DT, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday));
        }

        public static DateTime? GetNextRunDate(DateTime LastRunTime, SchedulerPlanning Plan)
        {
            if (Plan.Planning == SchedulerPlanningType.None)
                return (null);
            if (Plan.Planning == SchedulerPlanningType.OneTime)
            {
                if (Plan.StartDate >= LastRunTime)
                    return (Plan.StartDate);
                return (null);
            }
            if (Plan.Planning == SchedulerPlanningType.Daily)
            {
                if (Plan.StartDate >= LastRunTime)
                    return (Plan.StartDate);
                if (Plan.Recurrence == null || Plan.Recurrence < 1)
                    return (null);
                DateTime DT = Plan.StartDate;
                while (LastRunTime >= DT)
                {
                    DT = DT.AddDays(Plan.Recurrence.Value);
                }
                return (DT);
            }
            if (Plan.Planning == SchedulerPlanningType.Weekly)
            {
                if (Plan.StartDate >= LastRunTime)
                {
                    return (SetToNextDay(Plan.StartDate.AddDays(-1), Plan.RecurInWeekDays));
                }
                if (Plan.Recurrence == null || Plan.Recurrence < 1)
                    return (null);
                DateTime DT = Plan.StartDate;
                if (SetToNextDay(DT, Plan.RecurInWeekDays) == null)
                    return (null);
                DT = SetToNextDay(DT, Plan.RecurInWeekDays).Value;
                int CurrentWeekNumber = GetWeekNumber(DT);
                while (LastRunTime >= DT)
                {
                    DateTime nDT = SetToNextDay(DT, Plan.RecurInWeekDays).Value;
                    if (CurrentWeekNumber != GetWeekNumber(nDT))
                    {
                        switch (nDT.DayOfWeek)
                        {
                            case DayOfWeek.Monday: nDT = nDT.AddDays(7); break;
                            case DayOfWeek.Tuesday: nDT = nDT.AddDays(6); break;
                            case DayOfWeek.Wednesday: nDT = nDT.AddDays(5); break;
                            case DayOfWeek.Thursday: nDT = nDT.AddDays(4); break;
                            case DayOfWeek.Friday: nDT = nDT.AddDays(3); break;
                            case DayOfWeek.Saturday: nDT = nDT.AddDays(2); break;
                            case DayOfWeek.Sunday: nDT = nDT.AddDays(1); break;
                        }
                        nDT = nDT.AddDays(7 * (Plan.Recurrence.Value - 1));
                        nDT = nDT.AddDays(-1);
                        nDT = SetToNextDay(nDT, Plan.RecurInWeekDays).Value;
                        DT = nDT;
                    }
                    else
                    {
                        DT = nDT;
                    }
                }
                return (DT);
            }
            if (Plan.Planning == SchedulerPlanningType.Monthly)
            {
                if (Plan.StartDate >= LastRunTime)
                {
                    return (SetToNextDay(Plan.StartDate.AddDays(-1), Plan.RecurInDay, Plan.RecurInMonths));
                }
                DateTime DT = Plan.StartDate;
                if (SetToNextDay(DT, Plan.RecurInDay, Plan.RecurInMonths) == null)
                    return (null);
                while (LastRunTime >= DT)
                {
                    DT = SetToNextDay(DT, Plan.RecurInDay, Plan.RecurInMonths).Value;
                }
                return (DT);
            }

            return (null);
        }
    }
}
