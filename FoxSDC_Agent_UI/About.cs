using FoxSDC_Agent_UI.Properties;
using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    public partial class frmAbout : FForm
    {
        int ClickCounter = 0;
        public frmAbout()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            string AgentVersion = Status.GetAgentVersion();
            string ServerURL = "";
            string ServerVersion = "";
            string UCID = "";
            string Contract = "";
            string Computername = Environment.MachineName;
            string LicOwner = "";
            string LicID = "";
            string LicCustomID = "";
            if (AgentVersion == null)
            {
                AgentVersion = "Error";
                ServerURL = ServerVersion = "";
            }
            else
            {
                ServerURL = Status.GetServerURL();
                UCID = Status.GetUCID();
                Contract = Status.GetContract();
                ServerInfo srvi = Status.GetServerInfo();
                if (srvi != null)
                {
                    ServerVersion = srvi.ServerVersion;
                    LicOwner = srvi.LicOwner;
                    LicID = srvi.LicLicenseID;
                    LicCustomID = srvi.LicOwnerCustomID;
                }
            }

            lblAgentVer.Text = AgentVersion;
            lblServer.Text = ServerURL;
            lblServerVer.Text = ServerVersion;
            lblComputername.Text = Computername;
            lblUCID.Text = UCID;
            lblContract.Text = Contract;
            lblLicID.Text = LicID;
            lblLicOwner.Text = LicOwner;
            lblLicCustomID.Text = LicCustomID;

            lblMemoriam.Text = FoxSDC_Common.Memoriam.InMemoriam;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string d = "";
            d += lblTitle1.Text + "\r\n";
            d += lblTitle2.Text + "\r\n";
            d += "Agent Version: " + lblAgentVer.Text + "\r\n";
            d += "UCID: " + lblUCID.Text + "\r\n";
            d += "Name: " + lblComputername.Text + "\r\n";
            d += "Contract: " + lblContract.Text + "\r\n";
            d += "Server: " + lblServer.Text + "\r\n";
            d += "Server Version: " + lblServerVer.Text + "\r\n";
            d += "License owner: " + lblLicOwner.Text + "\r\n";
            d += "License ID: " + lblLicID.Text + "\r\n";
            d += "License Custom ID: " + lblLicCustomID.Text + "\r\n";
            try
            {
                Clipboard.SetText(d);
            }
            catch
            {

            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ClickCounter++;
            Debug.WriteLine("Click: " + ClickCounter);
            if (ClickCounter == 10 || Control.ModifierKeys == (Keys.Control | Keys.Shift))
            {
                SoundPlayer player = new SoundPlayer(Resources.YIP);
                player.Play();
            }
            if (ClickCounter == 50)
            {
                SoundPlayer player = new SoundPlayer(Resources.TEFI_Chimes);
                player.Play();
                ClickCounter = 0;
            }
        }
    }
}
