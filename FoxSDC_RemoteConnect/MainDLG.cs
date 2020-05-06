using FoxSDC_Common;
using FoxSDC_RemoteConnect.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_RemoteConnect
{
    public partial class MainDLG : FForm
    {
        string MID = "";
        NetworkConnection netc = null;
        bool Closing2 = false;

        Int64 RXStat = 0;
        Int64 TXStat = 0;

        class LSTComputerData
        {
            public string Name;
            public string Location;
            public string ID;
            public LSTComputerData(string Name, string Location, string ID)
            {
                this.Name = Name;
                this.Location = Location;
                this.ID = ID;
            }
            public override string ToString()
            {
                return (Name + (Location == "" ? "" : " [" + Location + "]"));
            }
        }

        delegate void DUpdatePing(string Text, bool EnableControls);

        public MainDLG()
        {
            InitializeComponent();
        }

        private void MainDLG_Load(object sender, EventArgs e)
        {
            chkUseWebSockets.Checked = true;
            panelLogin.Enabled = true;
            panelConnectData.Enabled = false;
            lblRX.Visible = lblTX.Visible = lblRXTXErr.Visible = false;
            txtUsername.Text = Settings.Default.LastUsername;
            txtServer.Text = Settings.Default.LastServer;
            lblPing.Text = "";
            this.Text = Program.Title;
            UpdateRXTXStat();
            if (Program.Connection == "server")
            {
                if (Program.Password == "*")
                {
                    frmPassword frm = new frmPassword(Program.Server, Program.Username);
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        this.Close();
                        return;
                    }
                    Program.Password = frm.Password;
                }
                txtServer.Text = Program.Server;
                txtUsername.Text = Program.Username;
                txtPassword.Text = Program.Password;
                cmdConnect_Click(sender, e);
                if (panelConnectData.Enabled == false)
                {
                    this.Close();
                    return;
                }
                MID = Program.MachineID;
                txtListenOn.Text = Program.LocalPort.ToString();
                txtConnectTo.Text = Program.RemoteServer;
                txtConnectPort.Text = Program.RemotePort.ToString();
                cmdStart_Click(sender, e);
            }
            if (Program.Connection == "direct")
            {
                txtServer.Text = Program.Server;
                cmdConnect_Click(sender, e);
                if (panelConnectData.Enabled == false)
                {
                    this.Close();
                    return;
                }
                MID = Program.MachineID;
                ComputerData d = Program.net.GetComputerDetail(MID);
                LSTComputerData c = new LSTComputerData("<<MID>> " + d.Computername, "", Program.MachineID);
                lstComputer.Items.Add(c);
                lstComputer.SelectedItem = c;
                txtListenOn.Text = Program.LocalPort.ToString();
                txtConnectTo.Text = Program.RemoteServer;
                txtConnectPort.Text = Program.RemotePort.ToString();
                cmdStart_Click(sender, e);
            }
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (txtServer.Text.Trim() == "")
            {
                MessageBox.Show(this, "Enter a server.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Program.Connection != "direct")
            {
                if (txtUsername.Text.Trim() == "")
                {
                    MessageBox.Show(this, "Enter an username.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtPassword.Text == "")
                {
                    MessageBox.Show(this, "Enter a password.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            Program.net = new Network();

            if (Program.net.Connect(txtServer.Text) == false)
            {
                MessageBox.Show(this, "Connection to server " + txtServer.Text + " did not work.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Program.Connection != "direct")
            {
                if (Program.net.Login(txtUsername.Text, txtPassword.Text) == false)
                {
                    if (Program.net.LoginError == null)
                        MessageBox.Show(this, "Login failed: " + "???", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Login failed: " + Program.net.LoginError.Error, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Program.net.CloseConnection();
                    return;
                }

                if (Program.net.NeedChangePassword() == true)
                {
                    MessageBox.Show(this, "You need to change your password.\nPlease login with the main manager application, change the password, and try again here.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Program.net.CloseConnection();
                    return;
                }
            }
            else
            {
                Program.net.SetSessionID(Program.Server, Program.SessionID);
            }

            if (Program.Connection == "")
            {
                Settings.Default.LastServer = txtServer.Text;
                Settings.Default.LastUsername = txtUsername.Text;
                Settings.Default.Save();
            }

            Program.net.GetInfo(); //Get more data of the logged in user

            List<ComputerData> computers = Program.net.GetComputerList(true, null);
            if (computers == null)
            {
                MessageBox.Show(this, "Cannot get a list of computers.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Program.net.CloseConnection();
                return;
            }
            if (computers.Count == 0)
            {
                MessageBox.Show(this, "There're no computers in the list. Approve at least one computer and try again here.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Program.net.CloseConnection();
                return;
            }

            foreach (ComputerData d in computers)
            {
                lstComputer.Items.Add(new LSTComputerData(d.Computername, d.GroupingPath, d.MachineID));
            }

            panelLogin.Enabled = false;
            panelConnectData.Enabled = true;

            lblPing.Text = "Status pending ..";
            picStatus.Image = Resources.Help.ToBitmap();
        }

        void UpdatePing(string Text, bool EnableControls)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new DUpdatePing(UpdatePing), Text, EnableControls);
                return;
            }
            lblPing.Text = Text;
            if (EnableControls == true)
                picStatus.Image = Resources.NetCon.ToBitmap();
            else
                picStatus.Image = Resources.NetNotCon.ToBitmap();
        }

        private void bgwPinger_DoWork(object sender, DoWorkEventArgs e)
        {
            if (MID == "")
                return;
            Network net = Program.net.CloneElement();
            bool res = net.PushPing(MID);
            UpdatePing(res == true ? "Online" : "Not connected", res);
        }

        private void lstComputer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstComputer.SelectedItem == null)
            {
                MID = "";
                lblPing.Text = "Status pending ..";
                picStatus.Image = Resources.Help.ToBitmap();
                return;
            }
            LSTComputerData c = (LSTComputerData)lstComputer.SelectedItem;
            MID = c.ID;
            lblPing.Text = "Status pending ..";
            picStatus.Image = Resources.Help.ToBitmap();
        }

        private void timerPinger_Tick(object sender, EventArgs e)
        {
            if (bgwPinger.IsBusy == false)
                bgwPinger.RunWorkerAsync();
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            LSTComputerData c = (LSTComputerData)lstComputer.SelectedItem;
            int LocalPort;
            if (int.TryParse(txtListenOn.Text, out LocalPort) == false)
            {
                MessageBox.Show(this, "Invalid Listen on Number.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (LocalPort < 1 && LocalPort > 65535)
            {
                MessageBox.Show(this, "Invalid Listen on Number Range (must beween 1-65535).", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtConnectTo.Text.Trim() == "")
            {
                MessageBox.Show(this, "Connect to missing", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            int RemotePort;
            if (int.TryParse(txtConnectPort.Text, out RemotePort) == false)
            {
                MessageBox.Show(this, "Invalid Remote Port Number.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (RemotePort < 1 && LocalPort > 65535)
            {
                MessageBox.Show(this, "Invalid Remote Port Number Range (must beween 1-65535).", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            netc = new NetworkConnection(chkUseWebSockets.Checked);
            netc.OnRXTX += Netc_OnRXTX;
            netc.OnStatus += Netc_OnStatus;
            if (netc.StartConnection(Program.net.CloneElement(), c.ID, LocalPort, txtConnectTo.Text.Trim(), RemotePort) == false)
            {
                MessageBox.Show(this, "Connection failed.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            panelConnectData.Enabled = false;
        }

        delegate void DNetc_OnStatus(NetworkConnection.StatusID Res);

        private void Netc_OnStatus(NetworkConnection.StatusID Res)
        {
            if (Closing2 == true)
                return;
            if (this.InvokeRequired == true)
            {
                this.Invoke(new DNetc_OnStatus(Netc_OnStatus), Res);
                return;
            }

            switch (Res)
            {
                case NetworkConnection.StatusID.Success:
                    lblRXTXErr.Visible = false;
                    break;
                default:
                    lblRXTXErr.Visible = true;
                    break;
            }
        }

        delegate void DNetc_OnRXTX(bool RX, bool TX, Int64 PlusRX, Int64 PlusTX);

        private void Netc_OnRXTX(bool RX, bool TX, Int64 PlusRX, Int64 PlusTX)
        {
            if (Closing2 == true)
                return;
            if (this.InvokeRequired == true)
            {
                this.Invoke(new DNetc_OnRXTX(Netc_OnRXTX), RX, TX, PlusRX, PlusTX);
                return;
            }
            lblRX.Visible = RX;
            lblTX.Visible = TX;
            RXStat += PlusRX;
            TXStat += PlusTX;
            UpdateRXTXStat();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateRXTXStat()
        {
            lblRXTXStat.Text = "RX: " + CommonUtilities.NiceSize(RXStat) + "\nTX: " + CommonUtilities.NiceSize(TXStat);
        }

        private void MainDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            Closing2 = true;
            if (netc != null)
                netc.StopConnection();
        }
    }
}
