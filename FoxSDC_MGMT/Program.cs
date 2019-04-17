using FoxSDC_Common;
using FoxSDC_MGMT.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    static class Program
    {
        public static string Title;
        public static string Version = "1.0." + FoxVersion.DTV;
        public static string PreFillServer = "";
        public static Network net;

        public static void LoadImageList(ImageList imageList)
        {
            imageList.Images.Add(Resources.folder); //0
            imageList.Images.Add(Resources.folderopen); //1
            imageList.Images.Add(Resources.Computer); //2
            imageList.Images.Add(Resources.Appwiz); //3
            imageList.Images.Add(Resources.System); //4
            imageList.Images.Add(Resources.Nix); //5
            imageList.Images.Add(Resources.Cert); //6
            imageList.Images.Add(Resources.Disk); //7
            imageList.Images.Add(Resources.link); //8
            imageList.Images.Add(Resources.Box); //9
            imageList.Images.Add(Resources.wsus); //10
            imageList.Images.Add(Resources.eventlog); //11
            imageList.Images.Add(Resources.intl); //12
            imageList.Images.Add(Resources.Report); //13
            imageList.Images.Add(Resources.srv); //14
            imageList.Images.Add(Resources.Wrench); //15
            imageList.Images.Add(Resources.KEYS03); //16
            imageList.Images.Add(Resources.tosrv); //17
            imageList.Images.Add(Resources.tosrv2); //18
            imageList.Images.Add(Resources.fromsrv); //19
            imageList.Images.Add(Resources.fromsrv2); //20
            imageList.Images.Add(Resources.fromtosrv); //21
        }

        [STAThread]
        static int Main(string[] args)
        {
            Title = "Fox SDC " + Version;

            if (args.Length > 1)
            {
                try
                {
                    if (args[0] == "-server")
                    {
                        PreFillServer = args[1].Trim();
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
#if DEBUG
                    MessageBox.Show(null, "An error occoured: " + ee.ToString(), Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
#else
                    MessageBox.Show(null, "An error occoured: " + ee.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
#endif
                    return (-255);
                }
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainDLG());
            Application.Run(new frmExiting());
            return (0);
        }
    }
}
