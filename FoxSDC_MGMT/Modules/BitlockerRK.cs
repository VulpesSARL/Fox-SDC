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
    public partial class ctlBitlockerRK : UserControl
    {
        string MachineID;

        public ctlBitlockerRK(string MachineID)
        {
            this.MachineID = MachineID;
            InitializeComponent();
        }

        private void ctlBitlockerRK_Load(object sender, EventArgs e)
        {
        }

        private void cmdGetKeys_Click(object sender, EventArgs e)
        {
            if (tvRK.Nodes.Count == 0)
                if (MessageBox.Show(this, "Really request keys?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
            tvRK.Nodes.Clear();
            List<BitlockerRK> rk = Program.net.GetBitlockerRK(MachineID);
            if (rk == null || rk.Count == 0)
            {
                MessageBox.Show(this, "No keys on server for this machine.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (BitlockerRK rkk in rk)
            {
                string d ="";
                if (rkk.DriveLetter.Trim() == "")
                    d = rkk.DeviceID + " ";
                else
                    d = rkk.DriveLetter.Trim() + " (" + rkk.DeviceID + ") ";
                d += rkk.Reported.ToLongDateString() + " " + rkk.Reported.ToLongTimeString();
                TreeNode n = tvRK.Nodes.Add(d);
                foreach(BitlockerRKKeyElement ke in rkk.Keys)
                {
                    n.Nodes.Add(ke.VolumeKeyProtectorID + " = " + ke.Key);
                }
            }
        }
    }
}
