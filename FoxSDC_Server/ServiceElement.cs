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

namespace FoxSDC_Server
{
    partial class FoxSDCASrvService : ServiceBase
    {
        Thread T;

        public FoxSDCASrvService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            T = new Thread(new ThreadStart(Program.SMain));
            T.Start();
        }

        protected override void OnStop()
        {
            Program.ServiceRunning = false;
            T.Join();
        }
    }
}
