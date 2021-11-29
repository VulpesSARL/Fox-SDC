using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    public static class FoxEventLog
    {
        public static bool Shutup = false;
        const string Title = "Fox SDC Agent";
        const int EVTMaxLength = 32700;
        public static void WriteEventLog(string Message, EventLogEntryType type)
        {
            if (Shutup == true)
                return;
            try
            {
                if (EventLog.SourceExists(Title) == false)
                {
                    EventLog.CreateEventSource(Title, "Application");
                    return;
                }

                EventLog ev = new EventLog();
                ev.Source = Title;
                if (Message.Length > EVTMaxLength)
                    Message = Message.Substring(0, EVTMaxLength - 6) + "<snip>";
                ev.WriteEntry(Message, type);
                Debug.WriteLine("EVT: " + Message);
            }
            catch (Exception ee)
            {
                Debug.WriteLine("Event Log didn't work " + ee.ToString());
            }
        }

        public static void WriteEventLog(string Message, EventLogEntryType type, bool ByPassShutup)
        {
            if (ByPassShutup == false && Shutup == true)
                return;
            bool shut = Shutup;
            Shutup = false;
            WriteEventLog(Message, type);
            Shutup = shut;
        }


        public static void VerboseWriteEventLog(string Message, EventLogEntryType type)
        {
            if (Shutup == true)
                return;
            try
            {
#if !DEBUG
                if (RegistryData.Verbose != 1)
                    return;
#endif

                if (EventLog.SourceExists(Title) == false)
                {
                    EventLog.CreateEventSource(Title, "Application");
                    return;
                }

                EventLog ev = new EventLog();
                ev.Source = Title;
                if (Message.Length > EVTMaxLength)
                    Message = Message.Substring(0, EVTMaxLength) + "<snip>";
                ev.WriteEntry(Message, type, 32767);
                Debug.WriteLine("EVT: " + Message);
            }
            catch (Exception ee)
            {
                Debug.WriteLine("Event Log didn't work " + ee.ToString());
            }
        }

        public static void RegisterEventLog()
        {
            try
            {
                if (EventLog.SourceExists(Title) == false)
                {
                    EventLog.CreateEventSource(Title, "Application");
                    Console.WriteLine(Title + " Created");
                    Thread.Sleep(1000);
                }
                else
                {
                    //Console.WriteLine(Title + " Exists");
                }
            }
            catch(Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

    }
}
