using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    partial class FoxSDCAService : ServiceBase
    {
        Thread T;

        public FoxSDCAService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            T = new Thread(new ThreadStart(ProgramAgent.SMain));
            T.Start();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            this.RequestAdditionalTime(5 * 60 * 1000);
            Threads.StopAllThreads();
            T.Join(2000);
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            this.RequestAdditionalTime(5 * 60 * 1000);
            Threads.StopAllThreads();
            T.Join();
            base.OnShutdown();
        }
    }
}
