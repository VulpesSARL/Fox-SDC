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
using Newtonsoft.Json;

namespace FoxSDC_MGMT.Policies
{
    public partial class ctlPortMapping : UserControl, PolicyElementInterface
    {
        PortMappingPolicy Mapping;
        PolicyObject Pol;

        public ctlPortMapping()
        {
            InitializeComponent();
        }

        private void ctlPortMapping_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;
            chkCreateHOSTS_CheckedChanged(sender, e);
            UpdateStatus();
        }

        private void chkCreateHOSTS_CheckedChanged(object sender, EventArgs e)
        {
            txtHostsEntry.Enabled = chkCreateHOSTS.Checked;
        }

        void UpdateStatus()
        {
            chkCreateHOSTS.Checked = Mapping.EditHOSTS;
            txtHostsEntry.Text = Mapping.HOSTSEntry;
            chkMap0000.Checked = Mapping.BindTo0000;
            chkDontMapOnServer.Checked = Mapping.NoBindIfSDCServerIsDetected;
            txtCliPort.Text = Mapping.ClientPort.ToString();
            txtToPort.Text = Mapping.ServerPort.ToString();
            txtToServer.Text = Mapping.ServerServer;
        }

        public bool SetData(PolicyObject obj)
        {
            Pol = obj;

            Mapping = JsonConvert.DeserializeObject<PortMappingPolicy>(obj.Data);
            if (Mapping == null)
                Mapping = new PortMappingPolicy();

            UpdateStatus();
            return (true);
        }

        public string GetData()
        {
            Mapping.EditHOSTS = chkCreateHOSTS.Checked;
            Mapping.HOSTSEntry = txtHostsEntry.Text;
            Mapping.BindTo0000 = chkMap0000.Checked;
            Mapping.NoBindIfSDCServerIsDetected = chkDontMapOnServer.Checked;
            Mapping.ServerServer = txtToServer.Text;
            int.TryParse(txtCliPort.Text, out Mapping.ClientPort);
            int.TryParse(txtToPort.Text, out Mapping.ServerPort);
            return (JsonConvert.SerializeObject(Mapping));
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string d = GetData();
            Program.net.EditPolicy(Pol.ID, d);
        }
    }
}
