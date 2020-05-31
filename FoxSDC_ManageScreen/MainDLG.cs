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
using WebSocketSharp;

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
        string WSURL;
        WebSocket ws;
        Thread UpdateScreenThreadHandle;

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
#if DEBUG
            txtStatus.Visible = true;
#else
            txtStatus.Visible = false;
#endif

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

            Connect();
        }

        void Connect(bool Reset = false)
        {
            MouseEventsEnabled = false;
            KeyboardEventsEnabled = false;

#if DEBUG
            UpdateText("Connecting");
#endif

            if (Reset == true)
            {
                ThreadRunning = false;
                if (UpdateScreenThreadHandle != null)
                {
                    if (UpdateScreenThreadHandle.Join(10000) == false)
                    {
                        MessageBox.Show(this, "Thread did not close. Reset not performed", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    try
                    {
                        ws.Close();
                    }
                    catch
                    {

                    }
                }
            }

            try
            {
                string WSURL = Program.Net.GetWebsocketURL();
                PushConnectNetworkResult res = Program.Net.PushCreateWSScreenconnection(ComputerID);
                if (res == null)
                    return;
                if (res.Result != 0)
                    return;
                Debug.WriteLine("WS SOCKET: " + res.ConnectedGUID + " Create connection");
                this.WSURL = WSURL + "websocket/mgmt-" + Uri.EscapeUriString(res.ConnectedGUID);
                Debug.WriteLine("WS URL: " + this.WSURL);

                ws = new WebSocket(this.WSURL);
                ws.OnMessage += Ws_OnMessage;
                ws.SetCookie(new WebSocketSharp.Net.Cookie("MGMT-SessionID", Program.Net.Session));
                ws.Connect();

                SendData(SendDataType.RefreshScreen, 0, 0, 0, 0);

                ThreadRunning = true;
                UpdateScreenThreadHandle = new Thread(new ThreadStart(UpdateScreenThread));
                UpdateScreenThreadHandle.Start();

                MouseEventsEnabled = true;
                KeyboardEventsEnabled = true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "SEH: " + ee.ToString(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void SendData(SendDataType Type, int F1, int F2, int F3, int F4)
        {
#if DEBUG
            UpdateText("SendData");
#endif

            SendDataData s = new SendDataData();
            s.Header1 = 0x46;
            s.Header2 = 0x52;
            s.Header3 = 0x53;
            s.Header4 = 0x1;

            s.DataType = (int)Type;
            s.Flag1 = F1;
            s.Flag2 = F2;
            s.Flag3 = F3;
            s.Flag4 = F4;

            byte[] data = CommonUtilities.Serialize<SendDataData>(s);
            try
            {
                ws.Send(data);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
            }
        }

        int RecvMode = 0;
        List<byte> RecvBaseData = null;
        List<byte> RecvScreenData = null;
        List<byte> RecvBlocksData = null;
        PushScreenData2 RecvPushScreenData;
        int PushScreenData2SZ = Marshal.SizeOf(typeof(PushScreenData2));
        bool Ws_OnMessageRunning = false;
        object ScreenLocker = new object();
        object Ws_OnMessageLocker = new object();

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            lock (Ws_OnMessageLocker)
            {
                try
                {
                    lock (ScreenLocker)
                    {
                        Ws_OnMessageRunning = true;
                    }

                    List<byte> Recv = new List<byte>(e.RawData);

                    while (Recv.Count > 0)
                    {
                        Recheck:
                        switch (RecvMode)
                        {
                            case 0:
#if DEBUG
                                UpdateText("Ws_OnMessage 0");
#endif
                                if (RecvBaseData == null)
                                    RecvBaseData = new List<byte>();
                                while (Recv.Count > 0)
                                {
                                    RecvBaseData.Add(Recv[0]);
                                    Recv.RemoveAt(0);

                                    #region Header Test

                                    if (RecvBaseData.Count == 1)
                                    {
                                        if (RecvBaseData[0] != 0x46)
                                        {
                                            RecvBaseData = null;
                                            continue;
                                        }
                                    }

                                    if (RecvBaseData.Count == 2)
                                    {
                                        if (RecvBaseData[1] != 0x52)
                                        {
                                            RecvBaseData = null;
                                            continue;
                                        }
                                    }

                                    if (RecvBaseData.Count == 3)
                                    {
                                        if (RecvBaseData[2] != 0x53)
                                        {
                                            RecvBaseData = null;
                                            continue;
                                        }
                                    }

                                    if (RecvBaseData.Count == 4)
                                    {
                                        if (RecvBaseData[3] != 0x1)
                                        {
                                            RecvBaseData = null;
                                            continue;
                                        }
                                    }

                                    #endregion

                                    if (RecvBaseData.Count == PushScreenData2SZ)
                                    {
                                        RecvPushScreenData = CommonUtilities.Deserialize<PushScreenData2>(RecvBaseData.ToArray());
                                        RecvMode = 1;

                                        //Debug.WriteLine("Ws_OnMessage() DataSZ=" + RecvPushScreenData.DataSZ.ToString() + " NumChangedBlocks=" + RecvPushScreenData.NumChangedBlocks.ToString());
                                        RecvScreenData = null;
                                        RecvBlocksData = null;
                                        goto Recheck;
                                    }
                                }
                                break;
                            case 1:
#if DEBUG
                                UpdateText("Ws_OnMessage 1");
#endif
                                if (RecvPushScreenData.DataSZ == 0)
                                {
                                    RecvMode = 2;
                                    goto Recheck;
                                }
                                if (RecvScreenData == null)
                                    RecvScreenData = new List<byte>();
                                while (Recv.Count > 0)
                                {
                                    RecvScreenData.Add(Recv[0]);
                                    Recv.RemoveAt(0);
                                    if (RecvScreenData.Count == RecvPushScreenData.DataSZ)
                                    {
                                        RecvMode = 2;
                                        goto Recheck;
                                    }
                                }
                                break;
                            case 2:
#if DEBUG
                                UpdateText("Ws_OnMessage 2");
#endif
                                if (RecvPushScreenData.NumChangedBlocks == 0)
                                {
                                    RecvMode = 3;
                                    goto Recheck;
                                }
                                if (RecvBlocksData == null)
                                    RecvBlocksData = new List<byte>();
                                while (Recv.Count > 0)
                                {
                                    RecvBlocksData.Add(Recv[0]);
                                    Recv.RemoveAt(0);
                                    if (RecvBlocksData.Count == RecvPushScreenData.NumChangedBlocks * 8)
                                    {
                                        RecvMode = 3;
                                        goto Recheck;
                                    }
                                }
                                break;
                            case 3:
#if DEBUG
                                UpdateText("Ws_OnMessage 3");
#endif
                                //Debug.WriteLine("Ws_OnMessage() Complete!");
                                try
                                {
                                    PushScreenData pd = new PushScreenData();
                                    pd.BlockX = RecvPushScreenData.BlockX;
                                    pd.BlockY = RecvPushScreenData.BlockY;
                                    pd.CursorX = RecvPushScreenData.CursorX;
                                    pd.CursorY = RecvPushScreenData.CursorY;
                                    pd.Data = RecvScreenData.ToArray();
                                    pd.DataType = RecvPushScreenData.DataType;
                                    pd.FailedCode = RecvPushScreenData.FailedCode;
                                    pd.X = RecvPushScreenData.X;
                                    pd.Y = RecvPushScreenData.Y;
                                    if (RecvBlocksData != null && RecvBlocksData.Count > 0)
                                    {
                                        pd.ChangedBlocks = new List<long>();
                                        for (int i = 0; i < RecvBlocksData.Count; i += 8)
                                        {
                                            pd.ChangedBlocks.Add(BitConverter.ToInt64(RecvBlocksData.ToArray(), i));
                                        }
                                    }
                                    UpdateScreen(pd);
                                }
                                catch (Exception ee)
                                {
                                    Debug.WriteLine(ee.ToString());
                                }
                                RecvMode = 0;
                                RecvBaseData = null;
                                RecvScreenData = null;
                                RecvBlocksData = null;
                                break;
                        }
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                }
                finally
                {
                    lock (ScreenLocker)
                    {
                        Ws_OnMessageRunning = false;
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        delegate void DUpdateText(string Text);

#if DEBUG
        void UpdateText(string Text)
        {
            /*
            if (this.InvokeRequired == true)
            {
                this.Invoke(new DUpdateText(UpdateText), Text);
                return;
            }
            try
            {
                txtStatus.Text = Text;
                Application.DoEvents();
            }
            catch
            {

            }*/
        }
#endif

        bool ThreadRunning = true;
        void UpdateScreenThread()
        {
            do
            {
                lock (ScreenLocker)
                {
                    if (Ws_OnMessageRunning == false)
                    {
                        if (RecvBaseData == null && RecvMode == 0)
                        {
                            if (ScreenData == null)
                                SendData(SendDataType.RefreshScreen, 0, 0, 0, 0);
                            else
                                SendData(SendDataType.DeltaScreen, 0, 0, 0, 0);
#if DEBUG
                            UpdateText("Send Update");
#endif
                        }
#if DEBUG
                        else
                        {
                            UpdateText("RecvBaseData LOCKED (RecvMode: " + RecvMode.ToString() + ")");
                        }
#endif
                    }
#if DEBUG
                    else
                    {
                        UpdateText("Ws_OnMessageRunning LOCKED (RecvMode: " + RecvMode.ToString() + ")");
                    }
#endif
                }
                Thread.Sleep(slowRefreshToolStripMenuItem.Checked == true ? 5000 : 100);
            } while (ThreadRunning == true);
        }

        delegate void UpdateScreenDG(PushScreenData screen);

        void UpdateScreen(PushScreenData screen)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new UpdateScreenDG(UpdateScreen), screen);
                return;
            }

            if (screen == null)
                return;
            if (screen.X == 0 || screen.Y == 0)
                return;
            if (screen.FailedCode != 0)
                return;
            if (screen.Data == null)
                return;
            if (screen.ChangedBlocks == null && screen.DataType == 1)
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
                            if (screen.ChangedBlocks == null || screen.ChangedBlocks.Count == 0)
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
            SendData(SendDataType.Mouse, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(e.Button));
        }

        private void picDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            LastButtons |= ~e.Button;
            SendData(SendDataType.Mouse, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(LastButtons));
        }

        private void PicDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if (MouseEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            LastButtons = e.Button;
            SendData(SendDataType.Mouse, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(e.Button));
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            LastButtons = e.Button;
            SendData(SendDataType.Mouse, (int)(e.X / SqueezeRatio), (int)(e.Y / SqueezeRatio), e.Delta, ConvertButtons(e.Button));
        }

        private void MainDLG_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            SendData(SendDataType.Keyboard, (int)e.KeyCode, 0, 0, 0);
        }

        private void MainDLG_KeyUp(object sender, KeyEventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;
            if (disableInputHereToolStripMenuItem.Checked == true)
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            SendData(SendDataType.Keyboard, (int)e.KeyCode, 0, 0x2, 0);
        }

        private void CTRLALTDELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            SendData(SendDataType.Keyboard, 0xFFFFFFF, 0xFFFFFFF, 0xFFFFFFF, 0);
        }

        private void windowsKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            SendData(SendDataType.Keyboard, (int)(Keys.LWin), 0, 0, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.LWin), 0, 0x2, 0);
        }

        private void MainDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            ThreadRunning = false;
            try
            {
                SendData(SendDataType.Disconnect, 0, 0, 0, 0);
                Thread.Sleep(1000);
            }
            catch
            {

            }
        }

        private void sendLayoutToRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            InputLanguage lang = InputLanguage.CurrentInputLanguage;
            SendData(SendDataType.Keyboard, 0xFFFFFFF, 0xFFFFFFE, (lang.Handle.ToInt32() & 0x7FFF0000) >> 16, 0);
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
            lock (Ws_OnMessageLocker)
            {
                RecvMode = 0;
                RecvBaseData = null;
                RecvScreenData = null;
                RecvBlocksData = null;
                SendData(SendDataType.ResetStream, 0, 0, 0, 0);
            }
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
            refreshScreenToolStripMenuItem_Click(sender, e);
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

            SendData(SendDataType.Keyboard, (int)(Keys.LControlKey), 0, 0, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.LShiftKey), 0, 0, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.Escape), 0, 0, 0);

            SendData(SendDataType.Keyboard, (int)(Keys.Escape), 0, 0x2, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.LShiftKey), 0, 0x2, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.LControlKey), 0, 0x2, 0);
        }

        private void cTRLALTDELETEVKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KeyboardEventsEnabled == false)
                return;

            if (disableInputHereToolStripMenuItem.Checked == true)
                return;

            SendData(SendDataType.Keyboard, (int)(Keys.LControlKey), 0, 0, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.Alt), 0, 0, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.Delete), 0, 0, 0);

            SendData(SendDataType.Keyboard, (int)(Keys.Delete), 0, 0x2, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.Alt), 0, 0x2, 0);
            SendData(SendDataType.Keyboard, (int)(Keys.LControlKey), 0, 0x2, 0);
        }

        private void typeClipboardAsTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ClipbaordText = "";
            try
            {
                ClipbaordText = Clipboard.GetText(TextDataFormat.Text);
            }
            catch
            {
                return;
            }

            foreach (char c in ClipbaordText)
            {
                SendData(SendDataType.Keyboard, 0xFFFFFFF, 0xFFFFFFD, c, 0);
            }
        }

        private void resetConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to reset the connection?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
            Connect(true);
        }

        private void slowRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slowRefreshToolStripMenuItem.Checked = !slowRefreshToolStripMenuItem.Checked;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 0, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 1, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 2, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 3, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 4, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 5, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 6, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 7, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            SendData(SendDataType.SetScreen, 8, 0, 0, 0);
            refreshScreenToolStripMenuItem_Click(sender, e);
        }
    }
}
