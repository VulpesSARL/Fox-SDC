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
    public partial class frmSelectGroup : FForm
    {
        public Int64? SelectedElement;
        public string SelectedGroupName;
        public frmSelectGroup()
        {
            InitializeComponent();
        }

        private void frmSelectGroup_Load(object sender, EventArgs e)
        {
            SelectedElement = null;
            SelectedGroupName = "";
            Program.LoadImageList(imageList1);
            GroupFolders.CreateRootFolder(treeAction);
        }

        private void treeAction_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GroupFolders.AfterSelect(treeAction, e, out SelectedElement);
            if (SelectedElement == null)
                SelectedGroupName = "";
            else
                SelectedGroupName = GroupFolders.GetFullPath(treeAction);
        }

        private void treeAction_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            GroupFolders.BeforeExpand(e, false);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (SelectedElement == null)
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
