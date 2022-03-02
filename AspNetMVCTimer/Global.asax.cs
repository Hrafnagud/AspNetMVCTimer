using AspNetMVCTimer.Managers;
using AspNetMVCTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AspNetMVCTimer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static long TimerInterval = SystemVariableManager.TIMER_INTERVAL;
        private Thread TimerJobThread = null;

        private System.Timers.Timer timer;
        public System.Timers.Timer Timer{
            get
            {
                if (timer == null)
                {
                    timer = new System.Timers.Timer();
                    timer.AutoReset = false;
                    timer.Enabled = false;
                    timer.Interval = TimerInterval;
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(CheckIntervalElapsed);
                }
                return timer;
            }
        }

        private void CheckIntervalElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Timer.Enabled = false;
                if (TimerJobThread == null || !TimerJobThread.IsAlive)
                {
                    TimerJobThread = new Thread(DoTimerJob);
                    TimerJobThread.IsBackground = true;
                    TimerJobThread.Name = "DoTimerJob";
                    TimerJobThread.Start();
                }
            }
            catch (Exception ex)
            {

            }

            finally
            {
                Timer.Interval = TimerInterval;
                Timer.Enabled = true;
            }
        }

        private void DoTimerJob()
        {
            try
            {
                //Periodical operations to be performed. (You can also split methods. No need to assemble all processes into this method.)
                Record myRecord = new Record()
                {
                    Message = $"Adding this record..\t{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}"
                };
                TimerTestDBEntities myCotnext = new TimerTestDBEntities();
                myCotnext.Records.Add(myRecord);
                myCotnext.SaveChanges();
            }
            catch (Exception)
            {

            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Call event when project started.
            CheckIntervalElapsed(null, null);
        }

        protected void Application_Error()
        {
            //Unhandled or unwanted errors will be directed to this method!
        }
    }
}
