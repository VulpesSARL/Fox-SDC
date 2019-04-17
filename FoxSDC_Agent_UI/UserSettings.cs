using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    public partial class frmUserSettings : FForm
    {
        public frmUserSettings()
        {
            InitializeComponent();
        }

        private void frmUserSettings_Load(object sender, EventArgs e)
        {
            chkBlinkTitle.Checked = RegistryData.ChatBlink == 1 ? true : false;
            chkChatAOT.Checked = RegistryData.ChatAOT == 1 ? true : false;
            switch (RegistryData.ChatAudible)
            {
                case 0:
                    chkAudibleAlert.Checked = false;
                    chkAudibleAlert.Text = "&Audible alert when a chat arrives";
                    break;
                case 1:
                    chkAudibleAlert.Checked = true;
                    chkAudibleAlert.Text = "&Audible alert when a chat arrives";
                    break;
                case 2:
                    chkAudibleAlert.Checked = true;
                    chkAudibleAlert.Text = "Yip when a ch&at arrives";
                    break;
                case 3:
                    chkAudibleAlert.Checked = true;
                    chkAudibleAlert.Text = "Play TEFI chimes when a ch&at arrives";
                    break;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            RegistryData.ChatAOT = chkChatAOT.Checked == true ? 1 : 0;
            RegistryData.ChatBlink = chkBlinkTitle.Checked == true ? 1 : 0;
            if (chkAudibleAlert.Text == "Yip when a ch&at arrives")
                RegistryData.ChatAudible = chkAudibleAlert.Checked == true ? 2 : 0;
            else if (chkAudibleAlert.Text == "Play TEFI chimes when a ch&at arrives")
                RegistryData.ChatAudible = chkAudibleAlert.Checked == true ? 3 : 0;
            else
                RegistryData.ChatAudible = chkAudibleAlert.Checked == true ? 1 : 0;
            if (Program.Chat != null)
                Program.Chat.UpdateAOT();
            this.Close();
        }

        private void cmdCanccel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkAudibleAlert_CheckedChanged(object sender, EventArgs e)
        {
            chkAudibleAlert.Text = "&Audible alert when a chat arrives";
        }
    }
}
