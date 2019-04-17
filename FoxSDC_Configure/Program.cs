using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Configure
{
    static class Program
    {
        static public string AppPath;

        [STAThread]
        static void Main()
        {
            AppPath = Assembly.GetExecutingAssembly().Location;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (RegistryData.DisableConfigTool == true)
            {
                MessageBox.Show(null, "This configuration tool has been disabled by your system administrator.\nPlease contact your system administrator for more informations.", "Configure Fox Software Deployment & Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Application.Run(new MainDLG());
        }
    }
}
