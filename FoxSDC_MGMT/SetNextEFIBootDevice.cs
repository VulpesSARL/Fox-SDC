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
    public partial class frmSetNextEFIBootDevice : FForm
    {
        class ListThingy
        {
            public int ID;
            public string Desc;
            public override string ToString()
            {
                return (Desc);
            }
        }

        string MachineID;

        public frmSetNextEFIBootDevice(string MachineID)
        {
            this.MachineID = MachineID;
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (lstDevices.SelectedIndex == -1)
                return;
            if (Program.net.SetEFINextBootDevice(MachineID, (lstDevices.SelectedItem as ListThingy).ID) == false)
            {
                MessageBox.Show(this, "Setting device to " + (lstDevices.SelectedItem as ListThingy).Desc + " did not work", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Close();
        }

        private void frmSetNextEFIBootDevice_Load(object sender, EventArgs e)
        {
            Dictionary<int, string> dict = Program.net.GetEFIBootDevices(MachineID);
            if (dict == null)
                return;
            foreach (KeyValuePair<int, string> kvp in dict)
            {
                lstDevices.Items.Add(new ListThingy() { ID = kvp.Key, Desc = kvp.Value });
            }
        }
    }
}
