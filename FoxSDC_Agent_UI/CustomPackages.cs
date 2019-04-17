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
    public partial class frmCustomPackages : FForm
    {
        class LocalPackageIDData
        {
            public string Title;
            public string PackageID;

            public override string ToString()
            {
                return (Title);
            }
        }


        public frmCustomPackages()
        {
            InitializeComponent();
        }

        private void frmCustomPackages_Load(object sender, EventArgs e)
        {
            List<PackageIDData> packages = Status.GetOptionalSoftware();
            if (packages == null)
            {
                MessageBox.Show(this, "Error communicating with the service. Please contact your administrator about this error.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmdOK.Enabled = false;
                return;
            }

            if (packages.Count == 0)
            {
                MessageBox.Show(this, "The server has no optional software for you to install. If you think, this is an error, please contact your administrator.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdOK.Enabled = false;
                return;
            }

            foreach (PackageIDData pkg in packages)
            {
                LocalPackageIDData lpkg = new LocalPackageIDData();
                lpkg.PackageID = pkg.PackageID;
                lpkg.Title = pkg.Title;
                lstPackages.Items.Add(lpkg, false);
            }
            this.Show();
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstPackages.Items.Count; i++)
            {
                if (lstPackages.GetItemCheckState(i) == CheckState.Checked)
                {
                    LocalPackageIDData pkg = (LocalPackageIDData)lstPackages.Items[i];
                    Status.SetOptionalSoftware(pkg.PackageID);
                }
            }
            this.Close();
        }
    }
}
