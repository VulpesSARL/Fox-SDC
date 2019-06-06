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

namespace FoxSDC_MGMT
{
    public partial class frmPropertiesWindow : FForm
    {
        object data;
        object transformeddata;

        public frmPropertiesWindow(ComputerData cd)
        {
            InitializeComponent();
            data = cd;
        }

        public frmPropertiesWindow(DiskDataReport cd)
        {
            InitializeComponent();
            data = cd;
        }

        public frmPropertiesWindow(AddRemoveAppReport cd)
        {
            InitializeComponent();
            data = cd;
        }

        public frmPropertiesWindow(StartupItemFull cd)
        {
            InitializeComponent();
            data = cd;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPropertiesWindow_Load(object sender, EventArgs e)
        {
            if (data is ComputerData)
            {
                this.Text = "Computer informations";
                transformeddata = new PComputerData();
                ClassCopy.CopyClassData(data, transformeddata);
                ((PComputerData)transformeddata).IOSVerType = ((ComputerData)data).OSVerType;
                PropertiesG.SelectedObject = transformeddata;
            }
            if (data is AddRemoveAppReport)
            {
                this.Text = "Software informations";
                transformeddata = new PAddRemovePrograms();
                ClassCopy.CopyClassData(data, transformeddata);
                PropertiesG.SelectedObject = transformeddata;
            }
            if(data is DiskDataReport)
            {
                this.Text = "Disk informations";
                transformeddata = new PDiskDataReport();
                ClassCopy.CopyClassData(data, transformeddata);
                PropertiesG.SelectedObject = transformeddata;
            }
            if(data is StartupItemFull)
            {
                this.Text = "Startup informations";
                transformeddata = new PStartupItemFull();
                ClassCopy.CopyClassData(data, transformeddata);
                PropertiesG.SelectedObject = transformeddata;
            }
        }
    }
}
