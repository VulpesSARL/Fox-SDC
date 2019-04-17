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
        public static Network NetScreen;
        public static MainDLG maindlg;

        public static string Title = "Fox SDC Remote Screen " + FoxVersion.DTS;

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 3)
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            maindlg = new MainDLG(args[1], args[0], args[2]);

            Application.Run(maindlg);
        }
    }
}
