﻿using FoxSDC_Agent_UI.Properties;
using FoxSDC_Common;
using System;
using System.Media;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    public partial class frmChat : FForm
    {
#if ENABLECHAT
        PushChatMessage initialchat;
#endif

        public frmChat(PushChatMessage initialchat)
        {
#if ENABLECHAT
            this.initialchat = initialchat;
#endif
            InitializeComponent();
        }

#if ENABLECHAT
        void AppendText(PushChatMessage chat)
        {
            txtRecvText.Text += chat.DT.ToLocalTime().ToLongDateString() + " " + chat.DT.ToLocalTime().ToLongTimeString() +
                " - " + chat.Name + ":\r\n" + chat.Text + "\r\n";
            txtRecvText.SelectionStart = txtRecvText.Text.Length;
            txtRecvText.SelectionLength = 0;
            txtRecvText.ScrollToCaret();
        }

        public void UpdateAOT()
        {
            this.TopMost = RegistryData.ChatAOT == 1 ? true : false;
        }

        void GetNotice()
        {
            if (RegistryData.ChatBlink == 1)
                WindowFlashing.FlashWindowEx(this);
            switch (RegistryData.ChatAudible)
            {
                case 1: //beep
                    {
                        SystemSounds.Beep.Play();
                        break;
                    }
                case 2: //yip (hidden)
                    {
                        SoundPlayer player = new SoundPlayer(Resources.YIP);
                        player.Play();
                        break;
                    }
                case 3: //TEFI (hidden)
                    {
                        SoundPlayer player = new SoundPlayer(Resources.TEFI_Chimes);
                        player.Play();
                        break;
                    }
            }
        }
#endif

        private void frmChat_Load(object sender, EventArgs e)
        {
#if ENABLECHAT
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            if (initialchat != null)
            {
                AppendText(initialchat);
                Status.ConfirmChatMessage(initialchat.ID);
                GetNotice();
            }
            timPull.Enabled = true;
            UpdateAOT();
#endif
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
#if ENABLECHAT
            if (txtSendText.Text.Trim() == "")
                return;
            Status.SendChatMessage(txtSendText.Text.Trim());
            PushChatMessage c = new PushChatMessage();
            c.DT = DateTime.UtcNow;
            c.Name = "You";
            c.Text = txtSendText.Text.Trim();
            c.ID = 0;
            AppendText(c);
            txtSendText.Text = "";
#endif
        }

        private void timPull_Tick(object sender, EventArgs e)
        {
#if ENABLECHAT
            PushChatMessage msg = Status.PopChatMessage();
            if (msg == null)
                return;
            AppendText(msg);
            Status.ConfirmChatMessage(msg.ID);
            GetNotice();
#endif
        }

        private void frmChat_FormClosing(object sender, FormClosingEventArgs e)
        {
#if ENABLECHAT
            Program.Chat = null;
#endif
        }
    }
}
