using FoxSDC_Common;
using FoxSDC_ManageScreen.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_ManageScreen
{
    public partial class MainDLG : Form
    {
        string ComputerID;
        string ServerURL;
        string SessionID;
        Bitmap ScreenData;
        bool MouseEventsEnabled = false;
        bool KeyboardEventsEnabled = false;
        int SnapCounter = 0;
        bool SqueezePicture = false;
        float SqueezeRatio = 1;
        public MainDLG(string ComputerID, string ServerURL, string SessionID)
        {
            this.ComputerID = ComputerID;
            this.ServerURL = ServerURL;
            this.SessionID = SessionID;
            InitializeComponent();
        }

        delegate void DUpdatePic(Bitmap data, int CurX, int CurY);
        delegate void DSetCursor(Cursor cur);

        void UpdatePic(Bitmap data, int CurX, int CurY)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new DUpdatePic(UpdatePic), data, CurX, CurY);
                return;
            }

            Bitmap bmp = new Bitmap(data);

            Graphics gr = Graphics.FromImage(bmp);
            gr.DrawImage(Resources.CUR_Normal.ToBitmap(), CurX, CurY);

            picDisplay.Image = bmp;
            if (SqueezePicture == false)
            {
                picDisplay.Size = new Size() { Width = data.Width, Height = data.Height };
                picDisplay.SizeMode = PictureBoxSizeMode.Normal;
                panel1.AutoScroll = true;
                SqueezeRatio = 1;
            }
            else
            {
                float PercW = (float)panel1.Width / (float)data.Width;
                float PercH = (float)panel1.Height / (float)data.Height;
                float Perc = 0;
                if (PercH < PercW)
                    Perc = PercH;
                else
                    Perc = PercW;

                float X = (float)data.Width * Perc;
                float Y = (float)data.Height * Perc;

                SqueezeRatio = Perc;
                picDisplay.Size = new Size() { Width = (int)X, Height = (int)Y };
                picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
                panel1.AutoScroll = false;
            }
        }

        void SetCursor(Cursor cur)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new DSetCursor(SetCursor), cur);
                return;
            }
            panel1.Cursor = cur;
        }

        private void MainDLG_Load(object sender, EventArgs e)
        {
            this.Text = Program.Title + " ??? ???";
            Program.Net = new FoxSDC_Common.Network();
            if (Program.Net.SetSessionID(ServerURL, SessionID) == false)
            {
                panel1.Enabled = false;
                MessageBox.Show(this, "Cannot get session.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Text = Program.Title + " Server: " + Program.Net.serverinfo.Name + " ???";
            ComputerData cd = Program.Net.GetComputerDetail(ComputerID);
            if (cd == null)
            {
                panel1.Enabled = false;
                MessageBox.Show(this, "Cannot get computer info.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Text = Program.Title + " Server: " + Program.Net.serverinfo.Name + " Computer: " + cd.Computername;
            Program.NetScreen = new Network();
            Program.NetScreen.SetSessionID(Program.Net.ConnectedURL, Program.Net.CloneSession());
            MouseEventsEnabled = true;
            KeyboardEventsEnabled = true;
            Thread t = new Thread(new ThreadStart(UpdateScreenThread));
            t.Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool ThreadRunning = true;
        void UpdateScreenThread()
        {
            do
            {
                UpdateScreen();
                Thread.Sleep(100);
            } while (ThreadRunning == true);
        }

        void UpdateScreen()
        {
            PushScreenData screen;
            if (ScreenData == null)
            {
                screen = Program.NetScreen.PushGetScreenDataFull(ComputerID);
            }
            else
            {
                screen = Program.NetScreen.PushGetScreenDataDelta(ComputerID);
            }
            if (screen == null)
                return;
            if (screen.X == 0 || screen.Y == 0)
                return;
            try
            {
                switch (screen.DataType)
                {
                    case 0:
                        {
                            Bitmap bmp = new Bitmap(new MemoryStream(screen.Data));
                            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            ScreenData = bmp;
                            UpdatePic(bmp, screen.CursorX, screen.CursorY);
                            picDisplay.Refresh();
                            break;
                        }
                    case 1:
                        {
                            if (screen.ChangedBlocks.Count == 0)
                                return;
                            Bitmap bmpdelta = new Bitmap(new MemoryStream(screen.Data));
                            bmpdelta.RotateFlip(RotateFlipType.RotateNoneFlipY);

                            Graphics graa = Graphics.FromImage(ScreenData);

                            foreach (Int64 Blocks in screen.ChangedBlocks)
                            {
                                int X = (int)((Blocks >> 32) & 0x7FFFFFFF);
                                int Y = (int)(Blocks & 0x7FFFFFFF);

                                Y = screen.Y - Y;

                                graa.DrawImage(bmpdelta, new Rectangle(X, Y - screen.BlockY, screen.BlockX, screen.BlockY), new Rectangle(X, Y - screen.BlockY, screen.BlockX, screen.BlockY), GraphicsUnit.Pixel);
                            }
                            UpdatePic(ScreenData, screen.CursorX, screen.CursorY);
                            break;
                        }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        int ConvertButtons(MouseButtons e)
        {
            int i = 0;

            if (e == MouseButtons.Left)
                i |= (int)MouseDataFlags.LeftButton;
            if (e == MouseButtons.Right)
                i |= (int)MouseDataFlags.RightButton;
            if (e == MouseButtons.Middle)
                i |= (int)MouseDataFlags.MiddleButton;
            if (e == MouseButtons.XButton1)
                i |= (int)MouseDataFlags.XButton1;
            if (e == MouseButtons.XButton2)
                i |= (int)MouseDataFlags.XButton2;
            return (i);
        }

        MouseButtons LastButtons = 0;

        private void picDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            LastButtons = e.Button;
            Program.Net.PushSetMouse(ComputerID, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(e.Button));
        }

        private void picDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            LastButtons |= ~e.Button;
            Program.Net.PushSetMouse(ComputerID, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(LastButtons));
        }

        private void PicDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if (MouseEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            LastButtons = e.Button;
            Program.Net.PushSetMouse(ComputerID, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(e.Button));
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            LastButtons = e.Button;
            Program.Net.PushSetMouse(ComputerID, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(e.Button));
        }

        private void MainDLG_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            Program.Net.PushSetKeyboard(ComputerID, (int)e.KeyCode, 0, 0);
        }

        private void MainDLG_KeyUp(object sender, KeyEventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            Program.Net.PushSetKeyboard(ComputerID, (int)e.KeyCode, 0, 0x2);
        }

        private void CTRLALTDELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            Program.Net.PushSetKeyboard(ComputerID, 0xFFFFFFF, 0xFFFFFFF, 0xFFFFFFF);
        }

        private void windowsKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LWin), 0, 0);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LWin), 0, 0x2);
        }

        private void MainDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            ThreadRunning = false;
        }

        private void sendLayoutToRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            InputLanguage lang = InputLanguage.CurrentInputLanguage;
            Program.Net.PushSetKeyboard(ComputerID, 0xFFFFFFF, 0xFFFFFFE, (lang.Handle.ToInt32() & 0x7FFF0000) >> 16);
        }

        private void optionsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            InputLanguage lang = InputLanguage.CurrentInputLanguage;
            sendLayoutToRemoteToolStripMenuItem.Text = "&Send keyboard layout to remote (" + lang.LayoutName + ")";
        }

        private void disableInputHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableInputHereToolStripMenuItem.Checked = !disableInputHereToolStripMenuItem.Checked;
        }

        private void refreshScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenData = null;
        }

        private void snapshotToDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Filename = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (Filename.EndsWith("\\") == false)
                Filename += "\\";
            Filename += "Fox-SDC Manage Screen-" + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") +
                DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "-" +
                SnapCounter.ToString("0000") + ".png";
            SnapCounter++;
            picDisplay.Image.Save(Filename, ImageFormat.Png);
        }

        private void squeezePictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            squeezePictureToolStripMenuItem.Checked = !squeezePictureToolStripMenuItem.Checked;
            SqueezePicture = squeezePictureToolStripMenuItem.Checked;
        }

        private void timPing_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Program.Net != null)
                    Program.Net.Ping();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        private void CTRLSHIFTESCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LControlKey), 0, 0);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LShiftKey), 0, 0);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.Escape), 0, 0);

            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.Escape), 0, 0x2);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LShiftKey), 0, 0x2);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LControlKey), 0, 0x2);
        }

        private void cTRLALTDELETEVKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LControlKey), 0, 0);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.Alt), 0, 0);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.Delete), 0, 0);

            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.Delete), 0, 0x2);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.Alt), 0, 0x2);
            Program.Net.PushSetKeyboard(ComputerID, (int)(Keys.LControlKey), 0, 0x2);
        }
    }
}
