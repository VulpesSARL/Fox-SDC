using FoxSDC_Agent_UI;
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

namespace MyUCID
{
    public partial class MainDLG : FForm
    {
        public MainDLG()
        {
            InitializeComponent();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtUCID.Text);
            }
            catch
            {

            }
        }

        private void MainDLG_Load(object sender, EventArgs e)
        {
            txtUCID.Text = UCID.GetUCID();
        }
    }
}
