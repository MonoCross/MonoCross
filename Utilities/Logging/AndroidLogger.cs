using System;
using System.Threading;
using Android.Util;

namespace MonoCross.Utilities.Logging
{
    public class AndroidLogger : BaseLogger
    {
        public const string Tag = "MonoCross";
        public const string TagMetric = "MonoCross_metric";
        public const string TagPlatform = "MonoCross_gui";

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidLogger"/> class.
        /// </summary>
        /// <param name="logPath">A <see cref="String"/> representing the Log path value.</param>
        public AndroidLogger(string logPath)
            : base(logPath)
        {
        }

        #region Private Help Methods

        // Help methods that use our FileSystem abstraction
        public override void AppendLog(String message, LogMessageType messageType, Exception exception, params object[] args)
        {
            if ((int)LoggingLevel > (int)messageType)
                return;

            var threadId = Thread.CurrentThread.ManagedThreadId;

            if (args != null && args.Length > 0)
            {
                try
                {
                    message = string.Format(message, args);
                }
                catch (FormatException)
                {
                    Log.Warn(TagPlatform, "Invalid string format: " + message);
                }
            }

            var logEntry = string.Format(":{0}: {1}", threadId, message);

            switch (messageType)
            {
                case LogMessageType.Info:
                    Log.Info(Tag, logEntry);
                    break;
                case LogMessageType.Warn:
                    Log.Warn(Tag, logEntry);
                    break;
                case LogMessageType.Debug:
                    Log.Debug(Tag, logEntry);
                    break;
                case LogMessageType.Error:
                    Log.Error(Tag, logEntry);
                    break;
                case LogMessageType.Fatal:
                    Log.Wtf(Tag, logEntry);
                    break;
                case LogMessageType.Metric:
                    Log.Debug(TagMetric, logEntry);
                    break;
                case LogMessageType.Platform:
                    Log.Verbose(TagPlatform, logEntry);
                    break;
            }

            // throw all logging events to subscriber if there is subscriber(s)
            LogEvent(new LogEventArgs { LogLevel = messageType, Message = message, Exception = exception, });

            AppendText(LogFileName, $"{DateTime.Now:MM-dd-yyyy HH:mm:ss:ffff} :{threadId}: [{messageType}] {message}");
        }

        #endregion
    }
}