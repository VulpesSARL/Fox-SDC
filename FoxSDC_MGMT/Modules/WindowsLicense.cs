using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Common;

namespace FoxSDC_MGMT
{
    public partial class ctlWindowsLicense : UserControl
    {
        string MID;
        public ctlWindowsLicense(string MID)
        {
            this.MID = MID;
            InitializeComponent();
        }

        private void ctlWindowsLicense_Load(object sender, EventArgs e)
        {
            PropertyGrid wlg = new PropertyGrid();
            wlg.HelpVisible = false;
            wlg.Dock = DockStyle.Fill;
            WindowsLic winlic = Program.net.GetWindowsLicData(MID);
            if (winlic != null)
            {
                PWindowsLic winlictransformeddata = new PWindowsLic();
                ClassCopy.CopyClassData(winlic, winlictransformeddata);
                wlg.SelectedObject = winlictransformeddata;
            }
            this.Controls.Add(wlg);
        }
    }
}
