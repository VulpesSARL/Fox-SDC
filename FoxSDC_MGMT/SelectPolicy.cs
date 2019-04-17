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
    public partial class frmSelectPolicy : FForm
    {
        public Int64? SelectedGroupElement;
        public Int64? SelectedPolicyElement;
        public frmSelectPolicy()
        {
            InitializeComponent();
        }

        private void frmSelectGroup_Load(object sender, EventArgs e)
        {
            SelectedGroupElement = null;
            SelectedPolicyElement = null;
            Program.LoadImageList(imageList1);
            GroupFolders.CreateRootFolder(treeAction);
        }

        private void treeAction_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GroupFolders.AfterSelect(treeAction, e, out SelectedGroupElement, out SelectedPolicyElement);
        }

        private void treeAction_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            GroupFolders.BeforeExpand(e, true);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (SelectedPolicyElement == null)
                return;
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
