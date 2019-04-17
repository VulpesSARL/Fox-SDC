using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using FoxSDC_Common;
using Newtonsoft.Json;

namespace FoxSDC_MGMT.Policies
{
    public partial class ctlTesting : UserControl, PolicyElementInterface
    {
        PolicyObject Pol;
        PolicyTesting Test;

        public ctlTesting()
        {
            InitializeComponent();
        }

        public bool SetData(PolicyObject obj)
        {
            Pol = obj;
            Debug.WriteLine("SetData: " + obj.Data);

            Test = JsonConvert.DeserializeObject<PolicyTesting>(obj.Data);
            if (Test == null)
                Test = new PolicyTesting();

            return (true);
        }

        public string GetData()
        {
            string data = "";
            PolicyTesting n = new PolicyTesting();

            n.CheckBox1 = checkBox1.Checked;
            n.CheckBox2 = checkBox2.Checked;
            n.CheckBox3 = checkBox3.Checked;
            n.Text = textBox1.Text;

            data = JsonConvert.SerializeObject(n);

            Debug.WriteLine("GetData: \"" + data + "\"");
            return (data);
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string data = GetData();
            Program.net.EditPolicy(Pol.ID, data);
        }

        private void ctlTesting_Load(object sender, EventArgs e)
        {
            lblTitle.Text = Pol.Name;
            checkBox1.Checked = Test.CheckBox1;
            checkBox2.Checked = Test.CheckBox2;
            checkBox3.Checked = Test.CheckBox3;
            textBox1.Text = Test.Text;
        }
    }
}
