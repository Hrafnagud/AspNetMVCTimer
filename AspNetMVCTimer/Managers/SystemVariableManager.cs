using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AspNetMVCTimer.Managers
{
    public static class SystemVariableManager
    {
        public static long TIMER_INTERVAL
        {
            get
            {
                try
                {
                    var timerIntervalValue = ConfigurationManager.AppSettings["TIMER_INTERVAL"].ToString();
                    if (string.IsNullOrEmpty(timerIntervalValue))
                    {
                        return 60000;
                    }
                    return Convert.ToInt64(timerIntervalValue);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "\nWeb.Config file => TIMER_INTERVAL key must exist!");
                }
            }
        }
    }
}