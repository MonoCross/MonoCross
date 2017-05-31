using System;
using System.Collections.Generic;
using System.Threading;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// <see cref="ILog"/> implementation that stores no log files and outputs messages using <see cref="System.Diagnostics.Debug.WriteLine(string)"/>
    /// </summary>
    public class DiagnosticDebugLogger : ILog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticDebugLogger"/> class.
        /// </summary>
        public DiagnosticDebugLogger()
        {
        }

        // Help methods that use our FileSystem abstraction
        private void AppendLog(String message, LogMessageType messageType, Exception ex, params object[] args)
        {
            if ((int)LoggingLevel > (int)messageType)
                return;

#if NETCF
            int threadId = Thread.CurrentThread.ManagedThreadId;
#else
            int threadId = Environment.CurrentManagedThreadId;
#endif
            message = string.Format(message, args);

            string textEntry = string.Format("{0:MM-dd-yyyy HH:mm:ss:ffff} :{1}: [{2}] {3}", DateTime.Now, threadId, messageType, message);
            // throw all logging events to subscriber if there is subscriber(s)
            LogEvent(new LogEventArgs { LogLevel = messageType, Message = message, Exception = ex, });
            System.Diagnostics.Debug.WriteLine(textEntry);
        }


        /// <summary>
        /// Appends the specified <see cref="Exception"/> to the current log file.
        /// </summary>
        /// <param name="ex">The exception to append to the current log file.</param>
        /// <param name="messageType">The type of log message.  If this is less than the current logging level, the exception will not be appended.</param>
        private void AppendLog(Exception ex, LogMessageType messageType)
        {
            AppendLog(LogHelper.BuildExceptionMessage(ex, messageType), messageType, ex);
        }

        /// <summary>
        /// Called when a message has been logged.
        /// </summary>
        /// <param name="logEventArgs">Contains information about the message that was logged.</param>
        public void LogEvent(LogEventArgs logEventArgs)
        {
            // throw all logging events to subscriber if there is subscriber(s)
            if (OnLogEvent != null) OnLogEvent(logEventArgs);
        }

        #region ILog members

        /// <summary>
        /// Occurs when a message has been logged.
        /// </summary>
        public event LogEvent OnLogEvent;

        /// <summary>
        /// Gets the path in which log files are saved.
        /// </summary>
        public string LogPath
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the level of verbosity for the logger.
        /// </summary>
        /// <value>
        /// The logging level as a <see cref="LoggingLevel" /> value.
        /// </value>
        public LoggingLevel LoggingLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Logs an informational message.  This is a level 4 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Info(string message, params object[] args)
        {
            AppendLog(message, LogMessageType.Info, null, args);
        }

        /// <summary>
        /// Logs an informational message.  This is a level 4 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        public virtual void Info(Exception ex)
        {
            AppendLog(ex, LogMessageType.Info);
        }

        /// <summary>
        /// Logs an informational message.  This is a level 4 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Info(string message, Exception ex, params object[] args)
        {
            AppendLog(message, LogMessageType.Info, null, args);
            AppendLog(ex, LogMessageType.Info);
        }

        /// <summary>
        /// Logs a warning message.  This is a level 5 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Warn(string message, params object[] args)
        {
            AppendLog(message, LogMessageType.Warn, null, args);
        }

        /// <summary>
        /// Logs a warning message.  This is a level 5 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        public virtual void Warn(Exception ex)
        {
            AppendLog(ex, LogMessageType.Warn);
        }

        /// <summary>
        /// Logs a warning message.  This is a level 5 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Warn(string message, Exception ex, params object[] args)
        {
            AppendLog(message, LogMessageType.Warn, null, args);
            AppendLog(ex, LogMessageType.Warn);
        }

        /// <summary>
        /// Logs a debug message.  This is a level 2 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Debug(string message, params object[] args)
        {
            AppendLog(message, LogMessageType.Debug, null, args);
        }

        /// <summary>
        /// Logs a debug message.  This is a level 2 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        public virtual void Debug(Exception ex)
        {
            AppendLog(ex, LogMessageType.Debug);
        }

        /// <summary>
        /// Logs a debug message.  This is a level 2 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Debug(string message, Exception ex, params object[] args)
        {
            AppendLog(message, LogMessageType.Debug, null, args);
            AppendLog(ex, LogMessageType.Debug);
        }

        /// <summary>
        /// Logs an error message.  This is a level 6 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Error(string message, params object[] args)
        {
            AppendLog(message, LogMessageType.Error, null, args);
        }

        /// <summary>
        /// Logs an error message.  This is a level 6 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        public virtual void Error(Exception ex)
        {
            AppendLog(ex, LogMessageType.Error);
        }

        /// <summary>
        /// Logs an error message.  This is a level 6 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Error(string message, Exception ex, params object[] args)
        {
            AppendLog(message, LogMessageType.Error, null, args);
            AppendLog(ex, LogMessageType.Error);
        }

        /// <summary>
        /// Logs a fatal error message.  This is a level 7 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Fatal(string message, params object[] args)
        {
            AppendLog(message, LogMessageType.Fatal, null, args);
        }

        /// <summary>
        /// Logs a fatal error message.  This is a level 7 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        public virtual void Fatal(Exception ex)
        {
            AppendLog(ex, LogMessageType.Fatal);
        }

        /// <summary>
        /// Logs a fatal error message.  This is a level 7 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Fatal(string message, Exception ex, params object[] args)
        {
            AppendLog(message, LogMessageType.Fatal, null, args);
            AppendLog(ex, LogMessageType.Fatal);
        }

        /// <summary>
        /// Logs a metric message.  This is a level 3 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Metric(string message, params object[] args)
        {
            AppendLog(message, LogMessageType.Metric, null, args);
        }

        /// <summary>
        /// Logs a metric message.  This is a level 3 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="milliseconds">The number of milliseconds that an operation took.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Metric(string message, double milliseconds, params object[] args)
        {
            string msg = string.Format("{0} : Milliseconds: {1}", message, milliseconds);
            AppendLog(msg, LogMessageType.Metric, null, args);
        }

        /// <summary>
        /// Logs a message about the current platform.  This is a level 1 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Platform(string message, params object[] args)
        {
            AppendLog(message, LogMessageType.Platform, null, args);
        }

        /// <summary>
        /// Logs a message about the current platform.  This is a level 1 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        public virtual void Platform(Exception ex)
        {
            AppendLog(ex, LogMessageType.Platform);
        }

        /// <summary>
        /// Logs a message about the current platform.  This is a level 1 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void Platform(string message, Exception ex, params object[] args)
        {
            AppendLog(message, LogMessageType.Platform, null, args);
            AppendLog(ex, LogMessageType.Platform);
        }

        /// <summary>
        /// Returns a list of log files that current exist in the log path.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetLogFiles()
        {
            return new string[] { };
        }

        /// <summary>
        /// Deletes any log files that are older than the specified number of days.
        /// </summary>
        /// <param name="days">The number of days.  If a log file's age is equal to or greater than this, it will be deleted.</param>
        public void DeleteLogFiles(int days)
        {
            AppendLog("No logs to delete!", LogMessageType.Info, null);
        }

        #endregion
    }
}
