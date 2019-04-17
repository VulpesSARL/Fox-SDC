using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmExiting : FForm
    {
        bool CanClose = false;

        public frmExiting()
        {
            InitializeComponent();
        }

        private void frmExiting_Load(object sender, EventArgs e)
        {
            this.Text = "Exiting - " + Program.Title;
        }

        private void TimExitTest_Tick(object sender, EventArgs e)
        {
            if (UploadDownloadDataThread.ThreadClosed == true)
            {
                CanClose = true;
                this.Close();
            }
        }

        private void frmExiting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CanClose == false)
                e.Cancel = true;
        }
    }
}
