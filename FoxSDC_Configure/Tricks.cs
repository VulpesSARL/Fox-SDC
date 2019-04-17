using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Configure
{
    class Tricks
    {
        public static bool RestartAsAdmin(string Parameters)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = Program.AppPath;
                proc.StartInfo.Verb = "runas";
                proc.StartInfo.Arguments = Parameters;
                if (proc.Start() == false)
                    return (false);
                Process.GetCurrentProcess().Kill();
            }
            catch
            {
                return (false);
            }
            return (true);
        }

        public static bool RestartAsAdmin()
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = Program.AppPath;
                proc.StartInfo.Verb = "runas";
                if (proc.Start() == false)
                    return (false);
                Process.GetCurrentProcess().Kill();
            }
            catch
            {
                return (false);
            }
            return (true);
        }

        public static bool IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return (principal.IsInRole(WindowsBuiltInRole.Administrator));
        }

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public static void UACToButton(Button button, bool add)
        {
            const Int32 BCM_SETSHIELD = 0x160C;
            if (add == true)
            {
                button.FlatStyle = FlatStyle.System;
                if (button.Text.Trim() == "")
                    button.Text = " ";
                SendMessage(button.Handle, BCM_SETSHIELD, 0, 1);
            }
            else
            {
                SendMessage(button.Handle, BCM_SETSHIELD, 0, 0);
            }
        }
    }
}
