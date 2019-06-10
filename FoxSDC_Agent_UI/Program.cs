using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    static class Program
    {
        public static string AppTitle = VulpesBranding.AgentUITitle;
        public static MainDLG MainDLG = null;
        public static frmChat Chat = null;
        public static frmNT3Icon NT3 = null;

        [STAThread]
        static void Main(string[] args)
        {
            bool QuitApp = false;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/reportnow":
                        {
                            Status.InvokeUpdate(MessageInvoke.ReloadReports);
                            QuitApp = true;
                            break;
                        }
                    case "/updatenow":
                        {
                            Status.InvokeUpdate(MessageInvoke.ReloadPolicies);
                            QuitApp = true;
                            break;
                        }
                }
            }

            if (QuitApp == true)
                return;

            using (Mutex mutex = new Mutex(false, "Local\\" + VulpesBranding.AgentUIMutex))
            {
                if (!mutex.WaitOne(0, false))
                {
                    Debug.WriteLine("Agent UI - double instance");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainDLG = new MainDLG();
                Application.Run(MainDLG);
                Application.Exit();
            }
        }
    }
}
