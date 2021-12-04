using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_ManageScreen
{
    static class Program
    {
        public static Network Net;
        public static MainDLG maindlg;

        public static string Title = "Fox SDC Remote Screen " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 3)
                return;

#if DEBUG
            MessageBox.Show(null, "Attach Debugger here!", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            maindlg = new MainDLG(args[1], args[0], args[2]);

            Application.Run(maindlg);
        }
    }
}
