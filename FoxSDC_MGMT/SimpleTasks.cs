﻿using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public partial class frmSimpleTasks : FForm
    {
        SimpleTask editst = null;
        List<string> MachineIDs = null;
        Int64? GroupID = null;
        string PathName = "";

        public frmSimpleTasks()
        {
            MachineIDs = null;
            editst = null;
            InitializeComponent();
        }

        public frmSimpleTasks(SimpleTask editst)
        {
            this.editst = editst;
            MachineIDs = null;
            InitializeComponent();
        }

        public frmSimpleTasks(List<string> MachineIDs)
        {
            this.MachineIDs = MachineIDs;
            editst = null;
            InitializeComponent();
        }

        public frmSimpleTasks(Int64 GroupID, string PathName)
        {
            this.PathName = PathName;
            this.GroupID = GroupID;
            InitializeComponent();
        }

        private void frmSimpleTasks_Load(object sender, EventArgs e)
        {
            lstPCs.ShowCheckBoxes = true;
            lstPCs.ListOnly = true;
            lblTZ.Text = "(" + TimeZoneInfo.Local.StandardName + ")";

            lstType.Items.Add("Run a program");
            lstType.Items.Add("Edit Registry");
            lstType.Items.Add("Reset Windows Update Client");

            lstRegAction.Items.Add("Add / Update value");
            lstRegAction.Items.Add("Delete value");
            lstRegAction.Items.Add("Delete tree");

            lstRegValueType.Items.Add("REG_SZ");
            lstRegValueType.Items.Add("REG_DWORD");
            lstRegValueType.Items.Add("REG_QWORD");
            lstRegValueType.Items.Add("REG_MULTI_SZ");
            lstRegValueType.Items.Add("REG_EXPAND_SZ");
            lstRegValueType.Items.Add("REG_BINARY");

            lstRegRoot.Items.Add("HKEY_LOCAL_MACHINE");
            lstRegRoot.Items.Add("HKEY_USERS");

            panelRunApp.Dock = DockStyle.Fill;
            panelReg.Dock = DockStyle.Fill;
            panelRunApp.Visible = panelRunApp.Enabled = false;
            panelReg.Visible = panelReg.Enabled = false;

            lstType.SelectedIndex = 0;
            lstRegAction.SelectedIndex = 0;
            lstRegValueType.SelectedIndex = 0;
            lstRegRoot.SelectedIndex = 0;

            this.Text = "New Simple Task";
            lblIntoGroup.Visible = false;

            if (MachineIDs != null)
            {
                foreach (string MID in MachineIDs)
                    lstPCs.SelectMachineID(MID);
            }

            if (GroupID != null)
            {
                lstPCs.Visible = false;
                lblIntoGroup.Visible = true;
                lblIntoGroup.Text = "All computers located in the group:\n\n" + PathName;
            }

            if (editst != null)
            {
                lstPCs.SelectMachineID(editst.MachineID);

                txtName.Text = editst.Name;
                if (editst.ExecAfter == null)
                {
                    chkDontExec.Checked = false;
                }
                else
                {
                    chkDontExec.Checked = true;
                    DTExecBefore.Value = TimeZoneInfo.ConvertTimeFromUtc(editst.ExecAfter.Value, TimeZoneInfo.Local);
                }

                switch (editst.Type)
                {
                    case 1://run app
                        {
                            lstType.SelectedIndex = 0;
                            SimpleTaskRunProgramm stapp = JsonConvert.DeserializeObject<SimpleTaskRunProgramm>(editst.Data);
                            txtRunArgs.Text = stapp.Parameters;
                            txtRunExec.Text = stapp.Executable;
                            txtRunUser.Text = stapp.User;
                            break;
                        }
                    case 2://edit registry
                        {
                            lstType.SelectedIndex = 1;
                            SimpleTaskRegistry streg = JsonConvert.DeserializeObject<SimpleTaskRegistry>(editst.Data);
                            lstRegAction.SelectedIndex = streg.Action;
                            lstRegRoot.SelectedIndex = streg.Root;
                            txtRegFolder.Text = streg.Folder;
                            txtRegValueName.Text = streg.Valuename;
                            lstRegValueType.SelectedIndex = streg.ValueType;
                            txtRegValue.Text = streg.Data;
                            break;
                        }
                    case 3://Reset WU
                        {
                            lstType.SelectedIndex = 2;
                            break;
                        }
                }
                this.Text = "Edit Simple Task";
                if (editst.ID == -1)
                    editst = null;
            }

            DTExecBefore.CustomFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            DTExecBefore.Format = DateTimePickerFormat.Custom;
            chkDontExec_CheckedChanged(null, null);
        }

        private void lstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelRunApp.Visible = panelRunApp.Enabled = false;
            panelReg.Visible = panelReg.Enabled = false;
            switch (lstType.SelectedIndex)
            {
                case 0:
                    panelRunApp.Visible = panelRunApp.Enabled = true;
                    break;
                case 1:
                    panelReg.Visible = panelReg.Enabled = true;
                    break;
                case 2:
                    //no settings
                    break;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SelectComputers(Int64 GroupID)
        {
            List<GroupElement> gel = Program.net.GetGroups(GroupID);
            if (gel != null)
            {
                foreach (GroupElement g in gel)
                {
                    SelectComputers(g.ID);
                }
            }

            List<ComputerData> cdlst = Program.net.GetComputerList(true, GroupID);
            if (cdlst != null)
            {
                foreach (ComputerData pc in cdlst)
                {
                    lstPCs.SelectMachineID(pc.MachineID);
                }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please type in a name.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (lstPCs.CheckedItems.Count == 0 && GroupID == null)
            {
                MessageBox.Show(this, "Please select a least one computer.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DateTime? DontExecBefore = null;
            if (chkDontExec.Checked == true)
            {
                DontExecBefore = DTExecBefore.Value;
                if (DTExecBefore.Value.Year < 2010)
                {
                    MessageBox.Show(this, "Invalid date", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (GroupID != null)
            {
                lstPCs.LoadList();
                lstPCs.UncheckItems();

                SelectComputers(GroupID.Value);
            }

            switch (lstType.SelectedIndex)
            {
                case 0: //run app
                    {
                        if (txtRunExec.Text.Trim() == "")
                        {
                            MessageBox.Show(this, "Please enter at least the executable filename.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        SimpleTaskRunProgramm stapp = new SimpleTaskRunProgramm();
                        stapp.Executable = txtRunExec.Text.Trim();
                        stapp.Parameters = txtRunArgs.Text.Trim();
                        stapp.User = txtRunUser.Text.Trim();

                        RemovePreviousThing();

                        foreach (ListViewItem lst in lstPCs.CheckedItems)
                        {
                            ComputerData cd = (ComputerData)lst.Tag;
                            Int64? ID = Program.net.SetSimpleTask(txtName.Text.Trim(), cd.MachineID, DontExecBefore, 1, stapp);
                            if (ID == null)
                            {
                                MessageBox.Show(this, "Cannot create SimpleTask on Server: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }

                        if (GroupID != null)
                            MessageBox.Show(this, "Task" + (lstPCs.GetSelectedCount == 1 ? "" : "s") + " created for " + lstPCs.GetSelectedCount + " PC" + (lstPCs.GetSelectedCount == 1 ? "" : "s"), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
                case 1: //edit registry
                    {
                        if (txtRegFolder.Text.Trim() == "")
                        {
                            MessageBox.Show(this, "Please enter a folder.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        SimpleTaskRegistry streg = new SimpleTaskRegistry();
                        streg.Action = lstRegAction.SelectedIndex;
                        streg.Root = lstRegRoot.SelectedIndex;
                        streg.Folder = txtRegFolder.Text.Trim();
                        streg.Valuename = txtRegValueName.Text.Trim();
                        streg.ValueType = lstRegValueType.SelectedIndex;
                        streg.Data = txtRegValue.Text;

                        RemovePreviousThing();

                        foreach (ListViewItem lst in lstPCs.CheckedItems)
                        {
                            ComputerData cd = (ComputerData)lst.Tag;
                            Int64? ID = Program.net.SetSimpleTask(txtName.Text.Trim(), cd.MachineID, DontExecBefore, 2, streg);
                            if (ID == null)
                            {
                                MessageBox.Show(this, "Cannot create SimpleTask on Server: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }

                        if (GroupID != null)
                            MessageBox.Show(this, "Task" + (lstPCs.GetSelectedCount == 1 ? "" : "s") + " created for " + lstPCs.GetSelectedCount + " PC" + (lstPCs.GetSelectedCount == 1 ? "" : "s"), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
                case 2: //reset WU
                    {
                        SimpleTaskNix streg = new SimpleTaskNix();
                        streg.Dummy = "";

                        RemovePreviousThing();

                        foreach (ListViewItem lst in lstPCs.CheckedItems)
                        {
                            ComputerData cd = (ComputerData)lst.Tag;
                            Int64? ID = Program.net.SetSimpleTask(txtName.Text.Trim(), cd.MachineID, DontExecBefore, 3, streg);
                            if (ID == null)
                            {
                                MessageBox.Show(this, "Cannot create SimpleTask on Server: " + Program.net.GetLastError(), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }

                        if (GroupID != null)
                            MessageBox.Show(this, "Task" + (lstPCs.GetSelectedCount == 1 ? "" : "s") + " created for " + lstPCs.GetSelectedCount + " PC" + (lstPCs.GetSelectedCount == 1 ? "" : "s"), Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
            }
        }

        void RemovePreviousThing()
        {
            if (editst != null)
            {
                Program.net.DeleteSimpleTask(editst.ID);
            }
        }

        private void lstRegAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lstRegAction.SelectedIndex)
            {
                case 0:
                    txtRegValueName.Enabled = lstRegValueType.Enabled = txtRegValue.Enabled = true;
                    break;
                case 1:
                    txtRegValueName.Enabled = true;
                    lstRegValueType.Enabled = txtRegValue.Enabled = false;
                    break;
                case 2:
                    txtRegValueName.Enabled = lstRegValueType.Enabled = txtRegValue.Enabled = false;
                    break;
            }
        }

        private void chkDontExec_CheckedChanged(object sender, EventArgs e)
        {
            DTExecBefore.Enabled = chkDontExec.Checked;
        }
    }
}
