using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class MaintenanceTasks
    {
        static Thread MTT = null;
        static bool StopThread = false;

        public static void StartMaintenanceTreads()
        {
            MTT = new Thread(new ThreadStart(MThread));
            MTT.Start();
        }

        public static void StopThreads()
        {
            StopThread = true;
            if (MTT != null)
                MTT.Join();
        }

        static void Pause()
        {
            for (int i = 0; i < 60 * 60; i++)
            {
                Thread.Sleep(1000);
                if (StopThread == true)
                    break;
            }
        }

        static void MThread()
        {
            do
            {
                using (SQLLib sql = SQLTest.ConnectSQL("Fox SDC Server for Maintenance", 0))
                {
                    if (sql == null)
                    {
                        Pause();
                        if (StopThread == true)
                            break;
                    }

                    try
                    {
                        if (SettingsManager.Settings.KeepEventLogDays > 0)
                        {
                            DateTime now = DateTime.UtcNow;
                            Int64 res = Convert.ToInt64(sql.ExecSQLScalar(@"DECLARE @Deleted_Rows INT
                            DECLARE @Deleted_Rows_Total INT
                            SET @Deleted_Rows = 1
                            SET @Deleted_Rows_Total = 0

                            WHILE (@Deleted_Rows > 0)
                            BEGIN
                                delete top (10000) from EventLog where TimeGenerated<DATEADD(day, @d, getutcdate())
                                SET @Deleted_Rows = @@ROWCOUNT
                                SET @Deleted_Rows_Total = @Deleted_Rows_Total + @Deleted_Rows
                            END

                            Select @Deleted_Rows_Total",
                                new SQLParam("@d", 0 - SettingsManager.Settings.KeepEventLogDays)));
                            Int64 secs = Convert.ToInt64((DateTime.UtcNow - now).TotalSeconds);
                            FoxEventLog.WriteEventLog("Eventlog data Maintenance completed: " + res.ToString() + " entr" + (res == 1 ? "y" : "ies") + " deleted\nTime needed: " + secs + " second" + (secs == 1 ? "" : "s"), System.Diagnostics.EventLogEntryType.Information);
                        }
                        if (SettingsManager.Settings.KeepNonPresentDisks > 0)
                        {
                            DateTime now = DateTime.UtcNow;
                            int res = sql.ExecSQLNQ("delete from DiskData where DevicePresent!=1 AND LastUpdated<DATEADD(day, @d, getutcdate())",
                                new SQLParam("@d", 0 - SettingsManager.Settings.KeepNonPresentDisks));
                            Int64 secs = Convert.ToInt64((DateTime.UtcNow - now).TotalSeconds);
                            FoxEventLog.WriteEventLog("Nonpresent disk data Maintenance completed: " + res.ToString() + " entr" + (res == 1 ? "y" : "ies") + " deleted\nTime needed: " + secs + " second" + (secs == 1 ? "" : "s"), System.Diagnostics.EventLogEntryType.Information);
                        }
                        if (SettingsManager.Settings.KeepReports > 0)
                        {
                            DateTime now = DateTime.UtcNow;
                            int res = sql.ExecSQLNQ("delete from Reporting where Reported<DATEADD(day, @d, getutcdate())",
                                new SQLParam("@d", 0 - SettingsManager.Settings.KeepReports));
                            Int64 secs = Convert.ToInt64((DateTime.UtcNow - now).TotalSeconds);
                            FoxEventLog.WriteEventLog("Report Maintenance completed: " + res.ToString() + " entr" + (res == 1 ? "y" : "ies") + " deleted\nTime needed: " + secs + " second" + (secs == 1 ? "" : "s"), System.Diagnostics.EventLogEntryType.Information);
                        }
                        if (SettingsManager.Settings.KeepBitlockerRK > 0)
                        {
                            DateTime now = DateTime.UtcNow;
                            int res = sql.ExecSQLNQ("delete from BitlockerRK where Reported<DATEADD(day, @d, getutcdate())",
                                new SQLParam("@d", 0 - SettingsManager.Settings.KeepBitlockerRK));
                            Int64 secs = Convert.ToInt64((DateTime.UtcNow - now).TotalSeconds);
                            FoxEventLog.WriteEventLog("Old BitlockerRK Maintenance completed: " + res.ToString() + " entr" + (res == 1 ? "y" : "ies") + " deleted\nTime needed: " + secs + " second" + (secs == 1 ? "" : "s"), System.Diagnostics.EventLogEntryType.Information);
                        }
                        if (SettingsManager.Settings.KeepChatLogs > 0)
                        {
                            DateTime now = DateTime.UtcNow;
                            int res = sql.ExecSQLNQ("delete from Chats where DT<DATEADD(day, @d, getutcdate())",
                                new SQLParam("@d", 0 - SettingsManager.Settings.KeepChatLogs));
                            Int64 secs = Convert.ToInt64((DateTime.UtcNow - now).TotalSeconds);
                            FoxEventLog.WriteEventLog("Old Chat Logs Maintenance completed: " + res.ToString() + " entr" + (res == 1 ? "y" : "ies") + " deleted\nTime needed: " + secs + " second" + (secs == 1 ? "" : "s"), System.Diagnostics.EventLogEntryType.Information);
                        }
                    }
                    catch (Exception ee)
                    {
                        FoxEventLog.WriteEventLog("Cannot delete Eventlog data\n" + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    }

                    sql.CloseConnection();
                }
                Pause();
                if (StopThread == true)
                    break;
            } while (StopThread == false);
        }
    }
}
