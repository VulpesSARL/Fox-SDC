using FoxSDC_PackageCreator.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_PackageCreator
{
    static class Program
    {
        public static string AppTitle = VulpesBranding.PKGTitle;
        public static string AppPath = "";

        public static void LoadImageList(ImageList imageList)
        {
            imageList.Images.Add(Resources.folder); //0
            imageList.Images.Add(Resources.folderopen); //1            
        }

        [STAThread]
        static void Main()
        {
            AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Debug.WriteLine("Application path: " + AppPath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainDLG());
        }
    }
}
