using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace FoxSDC_MGMT
{
    public partial class ctlFirstPage : UserControl
    {
        public ctlFirstPage()
        {
            InitializeComponent();
        }

        private void FirstPage_Load(object sender, EventArgs e)
        {
            link.Text = VulpesBranding.MGMTMainPageURL;
            lblCompany.Text = VulpesBranding.MGMTMainPageCompany;

            txtServerData.Text = "";
            txtServerData.Text += "Server: " + Program.net.ConnectedURL + "\r\n";
            txtServerData.Text += "Name: " + Program.net.serverinfo.Name + "\r\n";
            txtServerData.Text += "\r\n";
            txtServerData.Text += "Management Version: " + FoxVersion.DTS + "\r\n";
            txtServerData.Text += "\r\n";
            txtServerData.Text += "Server Version: " + Program.net.serverinfo.ServerVersion + "\r\n";
            txtServerData.Text += "Server GUID: " + Program.net.serverinfo.ServerGUID + "\r\n";
            txtServerData.Text += "\r\n";
            txtServerData.Text += "SQL Server: " + Program.net.serverinfo.SQLServer + "\r\n";
            txtServerData.Text += "SQL Service: " + Program.net.serverinfo.SQLService + "\r\n";
            txtServerData.Text += "SQL Version: " + Program.net.serverinfo.SQLProductVersion + "\r\n";
            txtServerData.Text += "SQL Name: " + Program.net.serverinfo.SQLProductName + "\r\n";
            txtServerData.Text += "SQL Product Level: " + Program.net.serverinfo.SQLProductLevel + "\r\n";
            txtServerData.Text += "SQL Edition: " + Program.net.serverinfo.SQLEdition + "\r\n";
            txtServerData.Text += "SQL Collation: " + Program.net.serverinfo.SQLCollation + "\r\n";
            txtServerData.Text += "\r\n";
            txtServerData.Text += "License ID: " + Program.net.serverinfo.LicLicenseID + "\r\n";
            txtServerData.Text += "License Type: " + Program.net.serverinfo.LicLicenseType + "\r\n";
            txtServerData.Text += "License Owner: " + Program.net.serverinfo.LicOwner + "\r\n";
            txtServerData.Text += "License Custom ID: " + Program.net.serverinfo.LicOwnerCustomID + "\r\n";
            txtServerData.Text += "License support valid to: " + (Program.net.serverinfo.LicSupportValidTo == null ? "∞" : Program.net.serverinfo.LicSupportValidTo.Value.ToLongDateString() + " " + Program.net.serverinfo.LicSupportValidTo.Value.ToLongTimeString()) + "\r\n";
            txtServerData.Text += "License valid to: " + (Program.net.serverinfo.LicValidTo == null ? "∞" : Program.net.serverinfo.LicValidTo.Value.ToLongDateString() + " " + Program.net.serverinfo.LicValidTo.Value.ToLongTimeString()) + "\r\n";
            if (string.IsNullOrWhiteSpace(FoxSDC_Common.Memoriam.InMemoriam) == false)
                txtServerData.Text += "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" + FoxSDC_Common.Memoriam.InMemoriam;
        }

        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = VulpesBranding.MGMTMainPageURL;
                p.StartInfo.UseShellExecute = true;
                p.Start();
            }
            catch
            {

            }
        }
    }
}
