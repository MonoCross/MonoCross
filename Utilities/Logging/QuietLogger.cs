using System;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// Represents a <see cref="BasicLogger"/> that does not write to the Output window.
    /// </summary>
    public class QuietLogger : BaseLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuietLogger"/> class.
        /// </summary>
        /// <param name="logPath">A <see cref="string"/> representing the path in which to save the log files.</param>
        public QuietLogger(string logPath) : base(logPath) { }

        #region Private Help Methods

        // Help methods that use our FileSystem abstraction
        /// <summary>
        /// Appends the specified message to the current log file.
        /// </summary>
        /// <param name="message">The message to append to the current log file.</param>
        /// <param name="messageType">The type of log message.  If this is less than the current logging level, the message will not be appended.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public override void AppendLog(string message, LogMessageType messageType, Exception ex, params object[] args)
        {
            if ((int)LoggingLevel > (int)messageType)
                return;

            message = string.Format(message, args);

            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            string textEntry = string.Format("{0:MM-dd-yyyy HH:mm:ss:ffff} :{1}: [{2}] {3}", DateTime.Now, threadId, messageType, message);

            // throw all logging events to subscriber if there is subscriber(s)
            LogEvent(new LogEventArgs { LogLevel = messageType, Message = message, Exception = ex, });
            AppendText(LogFileName, textEntry);
        }

        #endregion
    }
}
