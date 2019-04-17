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
    public partial class frmAskText : FForm
    {
        string Title;
        string Label;
        string DefaultText;
        bool AllowBlank;
        public string ReturnedText;
        public frmAskText(string title, string label, string defaultText, bool allowblank)
        {
            Title = title;
            Label = label;
            DefaultText = defaultText;
            AllowBlank = allowblank;
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmAskText_Load(object sender, EventArgs e)
        {
            txtText.Text = DefaultText;
            lblLabel.Text = Label;
            this.Text = Title;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            ReturnedText = txtText.Text.Trim();
            if (ReturnedText == "" && AllowBlank == false)
            {
                MessageBox.Show(this, "Please enter some text.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
