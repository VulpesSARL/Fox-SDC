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
    public partial class ctlDevicesFilter : UserControl
    {
        string MachineID;
        public ctlDevicesFilter(string MachineID)
        {
            this.MachineID = MachineID;
            InitializeComponent();
        }

        private void ctlDevicesFilter_Load(object sender, EventArgs e)
        {
            List<FilterDriver> cfg = Program.net.GetDevicesFilters(MachineID);
            if (cfg == null)
                return;
            foreach (FilterDriver c in cfg)
            {
                string Classname = c.ClassGUID;
                if (Win10DevClasses.Classes.ContainsKey(c.ClassGUID) == true)
                    Classname = Win10DevClasses.Classes[c.ClassGUID];

                ListViewItem l = new ListViewItem(Classname);
                l.SubItems.Add(c.ClassGUID);
                switch (c.Type)
                {
                    case 1: l.SubItems.Add("UpperFilter"); break;
                    case 2: l.SubItems.Add("LowerFilter"); break;
                    default: l.SubItems.Add("Unknown devices " + c.Type.ToString()); break;
                }
                l.SubItems.Add(c.ServiceName);
                lstFilters.Items.Add(l);
            }
        }
    }
}
