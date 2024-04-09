using Castle.Facilities.Logging;
using System;
using System.Linq;
using Abp.Extensions;

namespace Abp.Castle.Logging.Log4Net
{
    public static class LoggingFacilityExtensions
    {
        public static LoggingFacility UseAbpLog4Net(this LoggingFacility loggingFacility)
        {
            return loggingFacility.LogUsing<Log4NetLoggerFactory>();
        }

        public static void Error(this log4net.ILog log, Exception ex)
        {
            if (ex is AggregateException aggregateException)
            {
                var msg = string.Join(" ", aggregateException.InnerExceptions.Select(e => e.GetFullMessage()));
                log.Error(msg, ex);
                return;
            }

            log.Error(ex.GetFullMessage(), ex);
        }
    }
}