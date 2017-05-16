using System;
using System.Linq;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// Helper methods for building log messages.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Gets the file name in the format: YYYMMDD
        /// </summary>
        /// <param name="suffix">The suffix after the date.</param>
        /// <param name="extension">The extension after the suffix.</param>
        /// <returns>A <see cref="string"/> representation of the file date.</returns>
        public static string GetFileNameYYYMMDD(string suffix, string extension)
        {
            var name = DateTime.Now.ToString("yyyy_MM_dd");

            if (!string.IsNullOrEmpty(suffix))
                name = string.Join("_", new[] { name, suffix.Trim('_'), });

            return name + extension;
        }

        /// <summary>
        /// Builds a log entry for an exception message.
        /// </summary>
        /// <param name="ex">The exception to format.</param>
        /// <param name="logMessageType">Level of the log message.</param>
        /// <returns>A <see cref="string"/> representation of the exception formatted for logging.</returns>
        public static string BuildExceptionMessage(Exception ex, LogMessageType logMessageType)
        {
            string exMessage =
                string.Format("Date: {0}, [Exception]\n {1}\n Message: {2}\n Stack: {3}",
                DateTime.Now,
                logMessageType,
                ex.Message,
                ex.StackTrace);

#if !NETCF
            // Include the Data collection
            exMessage += "\n Data:";
            exMessage = ex.Data.Keys.Cast<object>().Aggregate(exMessage, (current, item) => current + string.Format(" key:{0}, value:{1};", item, ex.Data[item]));
#endif
            // Are there any inner exceptions?
            while (ex.InnerException != null)
            {
                exMessage += BuildInnerExceptionMessage(ex.InnerException);
                ex = ex.InnerException;
            }

            return exMessage;
        }

        private static string BuildInnerExceptionMessage(Exception ex)
        {
            string inExMessage =
                string.Format("\n  [Inner Exception]\n  Message: {0}\n  Stack: {1}",
                ex.Message,
                ex.StackTrace);
#if !NETCF
            // Include the Data collection
            inExMessage += "\n  Data:";
            inExMessage = ex.Data.Keys.Cast<object>().Aggregate(inExMessage, (current, item) => current + string.Format(" key:{0}, value:{1};", item, ex.Data[item]));
#endif
            return inExMessage;
        }
    }
}
