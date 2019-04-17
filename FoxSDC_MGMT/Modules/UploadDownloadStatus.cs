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
using System.Diagnostics;

namespace FoxSDC_MGMT
{
    public partial class ctlUploadDownloadStatus : UserControl
    {
        public ctlUploadDownloadStatus()
        {
            InitializeComponent();
        }

        Dictionary<string, ComputerData> MIDs = new Dictionary<string, ComputerData>();

        private void ctlUploadDownloadStatus_Load(object sender, EventArgs e)
        {
            Program.LoadImageList(imageList1);
            UpdateList();
            tim.Enabled = true;
        }

        void UpdateList()
        {
            List<string> ProcessedIDs = new List<string>();

            foreach (UploadDownloadData ud in UploadDownloadDataThread.DataList)
            {
                bool Found = false;
                foreach (ListViewItem i in lstEntries.Items)
                {
                    if ((string)i.Tag == ud.ID)
                    {
                        Found = true;
                        i.SubItems[6].Text = ud.ProgressSize == null ? "N/A" : CommonUtilities.NiceSize(ud.ProgressSize.Value);
                        i.SubItems[7].Text = ud.Failed == false ? "" : ud.ErrorText;
                        break;
                    }
                }
                if (Found == false)
                {
                    ListViewItem l = new ListViewItem();
                    switch (ud.Direction)
                    {
                        case Direction.DownloadFromServer:
                            l.SubItems[0].Text = "Download";
                            l.ImageIndex = 19;
                            break;
                        case Direction.UploadToServer:
                            l.SubItems[0].Text = "Upload";
                            l.ImageIndex = 17;
                            break;
                        default:
                            l.SubItems[0].Text = "???";
                            break;
                    }

                    if (MIDs.ContainsKey(ud.MachineID) == false)
                        MIDs.Add(ud.MachineID, Program.net.GetComputerDetail(ud.MachineID));

                    l.Tag = ud.ID;
                    l.SubItems.Add(MIDs[ud.MachineID].Computername);
                    l.SubItems.Add(MIDs[ud.MachineID].GroupingPath);
                    l.SubItems.Add(ud.LocalFilename);
                    l.SubItems.Add(ud.RemoteFilename);
                    l.SubItems.Add(CommonUtilities.NiceSize(ud.Size));
                    l.SubItems.Add(ud.ProgressSize == null ? "N/A" : CommonUtilities.NiceSize(ud.ProgressSize.Value));
                    l.SubItems.Add(ud.Failed == false ? "" : ud.ErrorText);
                    lstEntries.Items.Add(l);
                }

                ProcessedIDs.Add(ud.ID);
            }

            List<ListViewItem> itr = new List<ListViewItem>();
            foreach (ListViewItem i in lstEntries.Items)
            {
                if (ProcessedIDs.Contains((string)i.Tag) == false)
                    itr.Add(i);
            }

            foreach (ListViewItem i in itr)
                lstEntries.Items.Remove(i);
        }

        private void tim_Tick(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void cancelUploaddownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem l in lstEntries.SelectedItems)
            {
                UploadDownloadDataThread.CancelUploadDownload((string)l.Tag);
            }
        }

        private void resetFailedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem l in lstEntries.SelectedItems)
            {
                UploadDownloadDataThread.ResetUploadDownload((string)l.Tag);
            }
        }
    }
}
