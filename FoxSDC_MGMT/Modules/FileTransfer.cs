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
    public partial class ctlFileTransfer : UserControl
    {
        string MID;
        public ctlFileTransfer(string MID)
        {
            this.MID = MID;
            InitializeComponent();
        }

        Dictionary<string, ComputerData> MIDs = new Dictionary<string, ComputerData>();

        void LoadList()
        {
            lstFiles.Items.Clear();
            List<FileUploadData> datas = Program.net.File_MGMT_GetFullFileList(MID);
            if (datas == null)
                return;
            foreach (FileUploadData data in datas)
            {
                ListViewItem l = new ListViewItem(data.RemoteFileLocation);
                string DirectionText;
                l.Tag = data;
                switch (data.Direction)
                {
                    case 0:
                        l.ImageIndex = 17;
                        DirectionText = "Server to Agent";
                        break;
                    case 1:
                        l.ImageIndex = 19;
                        DirectionText = "Agent to Server";
                        break;
                    case 2:
                        l.ImageIndex = 18;
                        DirectionText = "Server to Management";
                        break;
                    case 3:
                        l.ImageIndex = 20;
                        DirectionText = "Management to Server";
                        break;
                    default:
                        DirectionText = "??? 0x" + data.Direction.ToString("X");
                        break;
                }

                l.SubItems.Add(CommonUtilities.NiceSize(data.Size));
                l.SubItems.Add(CommonUtilities.NiceSize(data.ProgressSize));
                if (data.RequestOnly == true)
                    DirectionText += " (Pending)";
                l.SubItems.Add(DirectionText);

                if (MIDs.ContainsKey(data.MachineID) == false)
                    MIDs.Add(data.MachineID, Program.net.GetComputerDetail(data.MachineID));

                l.SubItems.Add(MIDs[data.MachineID].Computername);
                l.SubItems.Add(MIDs[data.MachineID].GroupingPath);

                lstFiles.Items.Add(l);
            }
        }

        private void ctlFileTransfer_Load(object sender, EventArgs e)
        {
            Program.LoadImageList(imageList1);
            LoadList();
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            LoadList();
        }

        private void cmdToClient_Click(object sender, EventArgs e)
        {
            frmAddFilesToRemoteClient frm = new frmAddFilesToRemoteClient(MID);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;

            LoadList();
        }

        private void cmdCancelTrans_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem i in lstFiles.SelectedItems)
            {
                FileUploadData t = (FileUploadData)i.Tag;
                Program.net.File_MGMT_CancelUpload(t.ID);
            }
            LoadList();
        }

        private void cmdTransferFromAgent_Click(object sender, EventArgs e)
        {
            frmAddFilesFromRemoteClient frm = new frmAddFilesFromRemoteClient(MID);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;

            LoadList();
        }

        private void cmdTransferToHere_Click(object sender, EventArgs e)
        {
            frmAddFilesToLocal frm = new frmAddFilesToLocal(MID);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;

            LoadList();
        }
    }
}
