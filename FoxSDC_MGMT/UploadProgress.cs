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
    public partial class frmUploadProgress : FForm
    {
        string Filename;
        int Type;

        public frmUploadProgress(string filename, int type)
        {
            Filename = filename;
            Type = type;
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to cancel the upload?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
            {
                progress.CancelUpload();
            }
        }

        private void frmUploadProgress_Load(object sender, EventArgs e)
        {
            if (progress.UploadFile(Filename, Type) == false)
            {
                MessageBox.Show(this, "An error occured: " + progress.LastError, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                this.Close();
            }
        }

        private void progress_Success(FoxSDC_Common.NewUploadDataID nid)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void progress_Error()
        {
            MessageBox.Show(this, "An error occured: " + progress.LastError, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.Close();
        }

        private void progress_Cancel()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
