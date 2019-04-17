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

namespace FoxSDC_PackageCreator
{
    public partial class frmPropertiesFolder : FForm
    {
        PKGFolder Folder;

        public frmPropertiesFolder(PKGFolder folder)
        {
            Folder = folder;
            InitializeComponent();
        }

        private void frmPropertiesFolder_Load(object sender, EventArgs e)
        {
            txtName.Text = Folder.FolderName;
            txtName.ReadOnly = true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            //what?!

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
