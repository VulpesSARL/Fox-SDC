using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_RemoteConnect
{
    static class Program
    {
        public static string Title = "Fox SDC Remote Connect";
        public static Network net;

        public static string Connection = "";
        public static string SessionID;
        public static string Server;
        public static string MachineID;
        public static string Username;
        public static string Password;
        public static int LocalPort;
        public static string RemoteServer;
        public static int RemotePort;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "-?":
                    case "/?":
                        MessageBox.Show(null, "Usage:\nFoxSDCRemoteConnect -server Server MachineID Username {Password|*} LocalPort RemoteServer RemotePort\nFoxSDCRemoteConnect -direct <<Used internally>>", Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    case "-server":
                        if (args.Length < 6)
                        {
                            MessageBox.Show(null, "Too few argurments", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        Connection = "server";
                        Server = args[1];
                        MachineID = args[2];
                        Username = args[3];
                        Password = args[4];
                        if (int.TryParse(args[5], out LocalPort) == false)
                        {
                            MessageBox.Show(null, "Invalid argurments", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        RemoteServer = args[6];
                        if (int.TryParse(args[7], out RemotePort) == false)
                        {
                            MessageBox.Show(null, "Invalid argurments", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        break;
                    case "-direct":
                        if (args.Length < 5)
                        {
                            MessageBox.Show(null, "Too few argurments", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        Connection = "direct";
                        Server = args[1];
                        MachineID = args[2];
                        SessionID = args[3];
                        if (int.TryParse(args[4], out LocalPort) == false)
                        {
                            MessageBox.Show(null, "Invalid argurments", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        RemoteServer = args[5];
                        if (int.TryParse(args[6], out RemotePort) == false)
                        {
                            MessageBox.Show(null, "Invalid argurments", Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        break;
                }
            }
            Application.Run(new MainDLG());
        }
    }
}
