using System;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// Represents the method that will handle the OnLogEvent event.
    /// </summary>
    /// <param name="logEventArgs">Contains information about the message that was logged.</param>
    public delegate void LogEvent(LogEventArgs logEventArgs);

    /// <summary>
    /// Provides data for the OnLogEvent event.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventArgs"/> class.
        /// </summary>
        public LogEventArgs()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message that was logged.</param>
        /// <param name="logLevel">The log level of the message.</param>
        public LogEventArgs(string message, LogMessageType logLevel)
        {
            Message = message;
            LogLevel = logLevel;
        }

        /// <summary>
        /// Gets or sets the logging level of the message that was logged.
        /// </summary>
        public LogMessageType LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the message that was logged.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Exception that was logged (if available)
        /// </summary>
        public Exception Exception { get; set; }
    }
}
