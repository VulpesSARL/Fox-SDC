using FoxSDC_Common;
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
    public partial class frmWriteMessage : FForm
    {
        public frmWriteMessage()
        {
            InitializeComponent();
        }

        private void frmWriteMessage_Load(object sender, EventArgs e)
        {
            lstPriority.Items.Add("Low");
            lstPriority.Items.Add("Normal");
            lstPriority.Items.Add("High");
            lstPriority.SelectedIndex = 1;

            txtDisclaimer.Text = RegistryData.MessageDisclaimer;
            if (txtDisclaimer.Text.Trim() == "")
            {
                panelBottom.Height -= panelDisclaimer.Height;
                panelDisclaimer.Visible = false;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please enter your name.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtSubject.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please enter a subject.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtText.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please enter your text.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            WriteMessage msg = new WriteMessage();
            msg.Name = txtName.Text.Trim();
            msg.Priority = lstPriority.SelectedIndex;
            msg.Subject = txtSubject.Text.Trim();
            msg.Text = txtText.Text.Trim();
            if (Status.WriteMessage(msg) == false)
            {
                MessageBox.Show(this, "Message cannot be sent.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Close();
        }
    }
}
