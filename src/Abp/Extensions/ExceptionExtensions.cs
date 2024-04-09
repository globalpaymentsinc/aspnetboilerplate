using System;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/> class.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Uses <see cref="ExceptionDispatchInfo.Capture"/> method to re-throws exception
        /// while preserving stack trace.
        /// </summary>
        /// <param name="exception">Exception to be re-thrown</param>
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        public static void Error(this Castle.Core.Logging.ILogger log, Exception ex)
        {
            if (ex is AggregateException aggregateException)
            {
                var msg = string.Join(" ", aggregateException.InnerExceptions.Select(e => e.GetFullMessage()));
                log.Error(msg, ex);
                return;
            }

            log.Error(ex.GetFullMessage(), ex);
        }

        public static string GetFullMessage(this Exception ex)
        {
            return ex.InnerException == null 
                ? ex.Message 
                : ex.Message + " --> " + ex.InnerException.GetFullMessage();
        }
    }
}