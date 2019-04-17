using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FoxSDC_Common;

namespace FoxSDC_MGMT
{
    public partial class ctlPaperManager : UserControl
    {
        string Title;
        string DisplayText;
        public ctlPaperManager(string Title, string DisplayText)
        {
            this.Title = Title;
            this.DisplayText = DisplayText;
            InitializeComponent();
        }

        private void ctlPaperManager_Load(object sender, EventArgs e)
        {
            lblName.Text = DisplayText;
        }

        private void cmdChange_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Title = "Change report paper " + DisplayText;
            o.Filter = "Telerik Report files|*.trdp";
            o.Multiselect = false;
            o.ShowReadOnly = false;
            o.CheckFileExists = true;
            if (o.ShowDialog(this) != DialogResult.OK)
                return;
            byte[] data = null;
            try
            {
                data = File.ReadAllBytes(o.FileName);
            }
            catch
            {
                MessageBox.Show(this, "Cannot read the file " + o.FileName, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (data == null)
                return;
            if (Program.net.PaperSaveTemplate(Title, data) == false)
            {
                MessageBox.Show(this, "Cannot save the report paper on the server: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MessageBox.Show(this, "Report paper saved successfully.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to delete the report paper " + DisplayText + " from the server and use the default one?", Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
            if (Program.net.PaperSaveTemplate(Title, null) == false)
            {
                MessageBox.Show(this, "Cannot delete the report paper from the server: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            byte[] pabeia = Program.net.PaperTest(Title);
            if (pabeia == null)
            {
                MessageBox.Show(this, "Cannot get the report paper from the server: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SaveFileDialog o = new SaveFileDialog();
            o.Title = "Test report paper " + DisplayText;
            o.Filter = "Portable Document Format|*.pdf";
            o.OverwritePrompt = true;
            o.CheckPathExists = true;
            if (o.ShowDialog(this) != DialogResult.OK)
                return;
            try
            {
                File.WriteAllBytes(o.FileName, pabeia);
            }
            catch
            {
                MessageBox.Show(this, "Cannot save data to the file " + o.FileName, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void cmdDownload_Click(object sender, EventArgs e)
        {
            byte[] pabeia = Program.net.PaperGetTemplate(Title);
            if (pabeia == null)
            {
                MessageBox.Show(this, "Cannot get the report paper from the server: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SaveFileDialog o = new SaveFileDialog();
            o.Title = "Extract report paper " + DisplayText;
            o.Filter = "Telerik Report files|*.trdp";
            o.OverwritePrompt = true;
            o.CheckPathExists = true;
            if (o.ShowDialog(this) != DialogResult.OK)
                return;
            try
            {
                File.WriteAllBytes(o.FileName, pabeia);
            }
            catch
            {
                MessageBox.Show(this, "Cannot save data to the file " + o.FileName, Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
    }
}
