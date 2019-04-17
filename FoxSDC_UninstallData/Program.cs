using FoxSDC_Agent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_UninstallData
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main()
        {
            FoxEventLog.Shutup = true;
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (dir.EndsWith("\\") == false)
                dir += "\\";

            ProgramAgent.Init();

            if (ProgramAgent.LoadDLL() == false)
                return (1);

            if (SystemInfos.CollectSystemInfo() != 0)
                return (1);

            if (ApplicationCertificate.LoadCertificate() == false)
            {
                FoxEventLog.WriteEventLog("Cannot load certificate", System.Diagnostics.EventLogEntryType.Error);
                return (1);
            }

            if (FilesystemData.LoadCertificates() == false)
                return (1);
            if (FilesystemData.LoadPolicies() == false)
                return (1);
            FilesystemData.LoadLocalPackageData();
            FilesystemData.LoadLocalPackages();
            FilesystemData.LoadUserPackageData();
            FilesystemData.LoadEventLogList();

            FoxEventLog.Shutup = false;

            if (SyncPolicy.ApplyPolicy(SyncPolicy.ApplyPolicyFunction.Uninstall) == false)
                return (5);

            ServiceController svc = new ServiceController("FoxSDCA");

            try
            {
                svc.Stop();
            }
            catch
            {

            }

            int i = 0;

            do
            {
                i++;
                if (i > 120 * 4)
                    break;
                svc.Refresh();
                Thread.Sleep(1000);
            } while (svc.Status != ServiceControllerStatus.Stopped);

            #region Kill Processes

            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainModule.FileName.ToLower() == dir.ToLower() + "foxsdc_agent_ui.exe")
                    {
                        proc.Kill();
                    }
                }
                catch
                {

                }
            }

            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainModule.FileName.ToLower() == dir.ToLower() + "foxsdc_applyusersettings.exe")
                    {
                        proc.Kill();
                    }
                }
                catch
                {

                }
            }

            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainModule.FileName.ToLower() == dir.ToLower() + "foxsdc_agent.exe")
                    {
                        proc.Kill();
                    }
                }
                catch
                {

                }
            }

            #endregion

            try
            {
                RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                reg.DeleteValue("FoxSDCAgent", false);
                reg.DeleteValue("FoxSDCAgentApply", false);
                reg.Close();
            }
            catch
            {

            }

            try
            {
                Process.Start(Environment.ExpandEnvironmentVariables("%systemroot%\\system32\\msiexec.exe"), "/x {A6F066EE-E795-4C65-8FE4-2D93AB52BC36} /passive");
            }
            catch
            {

            }
            return (0);
        }
    }
}
