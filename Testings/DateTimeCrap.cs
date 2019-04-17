using FoxSDC_Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testings
{
    [TestClass]
    public class DateTimeCrap
    {
        [TestMethod]
        public void TestDT1()
        {
            SchedulerPlanning Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55));
            Assert.AreEqual(new DateTime(2018, 3, 9, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 54), Plan));
            Assert.AreEqual(null, Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 56), Plan));
        }

        [TestMethod]
        public void TestDT2()
        {
            SchedulerPlanning Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), 1);
            Assert.AreEqual(new DateTime(2018, 3, 9, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 54), Plan));
            Assert.AreEqual(new DateTime(2018, 3, 10, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 56), Plan));
            Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), 5);
            Assert.AreEqual(new DateTime(2018, 3, 14, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 56), Plan));
        }

        [TestMethod]
        public void TestDT3()
        {
            SchedulerPlanning Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), 1, new List<DayOfWeek> { DayOfWeek.Monday });
            Assert.AreEqual(new DateTime(2018, 3, 12, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 54), Plan));
            Assert.AreEqual(new DateTime(2018, 3, 12, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 56), Plan));
            Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), 1, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday });
            Assert.AreEqual(new DateTime(2018, 3, 9, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 54), Plan));
            Assert.AreEqual(new DateTime(2018, 3, 9, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 55), Plan));
            Assert.AreEqual(new DateTime(2018, 3, 12, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 57), Plan));
            Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), 4, new List<DayOfWeek> { DayOfWeek.Thursday });
            Assert.AreEqual(new DateTime(2018, 3, 15, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 57, 54), Plan));
            Assert.AreEqual(new DateTime(2018, 4, 19, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 15, 22, 57, 56), Plan));
        }

        [TestMethod]
        public void TestDT4()
        {
            SchedulerPlanning Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), new List<int> { 1 }, new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            Assert.AreEqual(new DateTime(2018, 4, 1, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 56, 54), Plan));
            Assert.AreEqual(new DateTime(2018, 4, 1, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 3, 9, 22, 56, 56), Plan));
            Assert.AreEqual(new DateTime(2018, 4, 1, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 4, 1, 22, 56, 56), Plan));
            Assert.AreEqual(new DateTime(2018, 5, 1, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 4, 1, 22, 57, 56), Plan));
            Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), new List<int> { 29 }, new List<int> { 2 });
            Assert.AreEqual(new DateTime(2020, 2, 29, 22, 57, 55), Scheduler.GetNextRunDate(new DateTime(2018, 4, 1, 22, 57, 56), Plan));
            Plan = Scheduler.CreatePlan(new DateTime(2018, 3, 9, 22, 57, 55), new List<int> { 30 }, new List<int> { 2 });
            Assert.AreEqual(null, Scheduler.GetNextRunDate(new DateTime(2018, 4, 1, 22, 57, 56), Plan));
        }
    }
}
