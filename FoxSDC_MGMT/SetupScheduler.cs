using FoxSDC_Common;
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
    public partial class frmSetupScheduler : FForm
    {
        public SchedulerPlanning Plan;
        string Title;
        public frmSetupScheduler(SchedulerPlanning Plan, string Title)
        {
            this.Plan = Plan;
            this.Title = Title;
            InitializeComponent();
        }

        private void frmSetupScheduler_Load(object sender, EventArgs e)
        {
            this.Text = Title;
            lstMethod.Items.Add("None");
            lstMethod.Items.Add("Once");
            lstMethod.Items.Add("Daily");
            lstMethod.Items.Add("Weekly");
            lstMethod.Items.Add("Monthly");
            DTStart.Format = DateTimePickerFormat.Custom;
            DTStart.CustomFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            for (int i = 0; i < 7; i++)
                lstDayOfWeek.Items.Add(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)i));
            for (int i = 0; i < 32; i++)
                lstDays.Items.Add((i + 1).ToString());
            for (int i = 0; i < 12; i++)
                lstMonths.Items.Add(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(i + 1));
            panelDaily.Location = panelWeekly.Location = panelMonthly.Location;

            txtRecurWeeks.Text = "1";
            txtRecurDays.Text = "1";

            if (Plan == null)
            {
                lstMethod.SelectedIndex = 0;
            }
            else
            {
                lstMethod.SelectedIndex = (int)Plan.Planning;
                if (Plan.Planning != SchedulerPlanningType.None)
                    DTStart.Value = Plan.StartDate.ToLocalTime();
                switch (Plan.Planning)
                {
                    case SchedulerPlanningType.Daily:
                        txtRecurDays.Text = (Plan.Recurrence == null ? 1 : Plan.Recurrence.Value).ToString();
                        break;
                    case SchedulerPlanningType.Weekly:
                        txtRecurWeeks.Text = (Plan.Recurrence == null ? 1 : Plan.Recurrence.Value).ToString();
                        if (Plan.RecurInWeekDays != null)
                        {
                            foreach (DayOfWeek dob in Plan.RecurInWeekDays)
                            {
                                lstDayOfWeek.SetItemChecked((int)dob, true);
                            }
                        }
                        break;
                    case SchedulerPlanningType.Monthly:
                        if (Plan.RecurInDay != null)
                        {
                            foreach (int d in Plan.RecurInDay)
                            {
                                lstDays.SetItemChecked(d - 1, true);
                            }
                        }
                        if (Plan.RecurInMonths != null)
                        {
                            foreach (int m in Plan.RecurInMonths)
                            {
                                lstMonths.SetItemChecked(m - 1, true);
                            }
                        }
                        break;
                }
            }
        }

        private void lstMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lstMethod.SelectedIndex)
            {
                case 0:
                    panelDaily.Visible = panelWeekly.Visible = panelMonthly.Visible = false;
                    DTStart.Enabled = false;
                    break;
                case 1:
                    panelDaily.Visible = panelWeekly.Visible = panelMonthly.Visible = false;
                    DTStart.Enabled = true;
                    break;
                case 2:
                    panelWeekly.Visible = panelMonthly.Visible = false;
                    panelDaily.Visible = true;
                    DTStart.Enabled = true;
                    break;
                case 3:
                    panelDaily.Visible = panelMonthly.Visible = false;
                    panelWeekly.Visible = true;
                    DTStart.Enabled = true;
                    break;
                case 4:
                    panelDaily.Visible = panelWeekly.Visible = false;
                    panelMonthly.Visible = true;
                    DTStart.Enabled = true;
                    break;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            SchedulerPlanning P = new SchedulerPlanning();
            P.RecurInDay = new List<int>();
            P.RecurInMonths = new List<int>();
            P.RecurInWeekDays = new List<DayOfWeek>();
            P.StartDate = DTStart.Value.ToUniversalTime();
            int Tmp;
            switch (lstMethod.SelectedIndex)
            {
                case 0:
                    P.Planning = SchedulerPlanningType.None;
                    break;
                case 1:
                    P.Planning = SchedulerPlanningType.OneTime;
                    break;
                case 2:
                    P.Planning = SchedulerPlanningType.Daily;
                    if (int.TryParse(txtRecurDays.Text, out Tmp) == false)
                    {
                        MessageBox.Show(this, "Invalid recurrence number.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    if (Tmp < 1)
                    {
                        MessageBox.Show(this, "Recurrence must be more than 0.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    P.Recurrence = Tmp;
                    break;
                case 3:
                    P.Planning = SchedulerPlanningType.Weekly;
                    if (int.TryParse(txtRecurWeeks.Text, out Tmp) == false)
                    {
                        MessageBox.Show(this, "Invalid recurrence number.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    if (Tmp < 1)
                    {
                        MessageBox.Show(this, "Recurrence must be more than 0.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    P.Recurrence = Tmp;
                    for (int i = 0; i < 7; i++)
                    {
                        if (lstDayOfWeek.GetItemChecked(i) == true)
                            P.RecurInWeekDays.Add((DayOfWeek)i);
                    }
                    if (P.RecurInWeekDays.Count == 0)
                    {
                        MessageBox.Show(this, "Please select at least one day of week.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    break;
                case 4:
                    P.Planning = SchedulerPlanningType.Monthly;

                    for (int i = 0; i < 32; i++)
                        if (lstDays.GetItemChecked(i) == true)
                            P.RecurInDay.Add(i + 1);
                    for (int i = 0; i < 12; i++)
                        if (lstMonths.GetItemChecked(i) == true)
                            P.RecurInMonths.Add(i + 1);
                    if (P.RecurInDay.Count == 0)
                    {
                        MessageBox.Show(this, "Please select at least one day.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    if (P.RecurInMonths.Count == 0)
                    {
                        MessageBox.Show(this, "Please select at least one month.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    break;
            }
            DateTime? TestDT = Scheduler.GetNextRunDate(DateTime.Now, P);
            if (TestDT == null && P.Planning != SchedulerPlanningType.None)
            {
                MessageBox.Show(this, "This event will never occour.", Program.Title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            Plan = P;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtRecurDays_TextChanged(object sender, EventArgs e)
        {
            int tmp;
            if(int.TryParse(txtRecurDays.Text,out tmp)==false)
            {
                lblDays.Text = "days?";
            }
            else
            {
                lblDays.Text = "day" + (tmp == 1 ? "" : "s");
            }
        }

        private void txtRecurWeeks_TextChanged(object sender, EventArgs e)
        {
            int tmp;
            if (int.TryParse(txtRecurWeeks.Text, out tmp) == false)
            {
                lblWeeks.Text = "weeks?";
            }
            else
            {
                lblWeeks.Text = "week" + (tmp == 1 ? "" : "s");
            }
        }
    }
}
