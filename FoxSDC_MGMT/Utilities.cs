using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    class Utilities
    {
        public static void ConnectToScreen(IWin32Window parent, string MID)
        {
            string Exec = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "FoxSDC_ManageScreen.exe");
            string SessionID = Program.net.CloneSession();
            if (string.IsNullOrWhiteSpace(SessionID) == true)
            {
                MessageBox.Show(parent, "Cannot get a new SessionID from the Server", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = Exec;
                p.StartInfo.Arguments = "\"" + Program.net.ConnectedURL + "\" \"" + MID + "\" \"" + SessionID + "\"";
                p.StartInfo.UseShellExecute = false;
                p.Start();
            }
            catch (Exception ee)
            {
                MessageBox.Show(parent, "Cannot start the process " + Exec + " - " + ee.Message, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Debug.WriteLine(ee.ToString());
                return;
            }
        }
    }
}
