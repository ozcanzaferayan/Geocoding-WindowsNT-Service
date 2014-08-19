using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace GeocodingService
{
    public partial class GeocodingNTService : ServiceBase
    {
        public int eventId = 0;
        Controller c1;
        Controller c2;
        public GeocodingNTService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = "GeocodingSource";
            string logName = "GeocodingLog";
            if (args.Count() > 0)
            {
                 eventSourceName = args[0];
            }
            if (args.Count() > 1) 
            { 
                logName = args[1]; 
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            c1 = new Controller("Konum1");
            c2 = new Controller("Konum2");
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 15000; // 15 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Parallel.Invoke(
            //    () => c1.DoOperation(eventLog1),
            //    () => c2.DoOperation(eventLog1));
            Thread t1 = new Thread(delegate() { c1.DoOperation(eventLog1); });
            Thread t2 = new Thread(delegate() { c2.DoOperation(eventLog1); });
            t1.Start();
            Thread.Sleep(1000);
            t2.Start();
            Thread.Sleep(1000);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }



        
    }
}
