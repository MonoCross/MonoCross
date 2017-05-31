using System;
using System.Collections.Generic;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// Defines the MonoCross abstract logging utility.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Occurs when a message has been logged.
        /// </summary>
        event LogEvent OnLogEvent;

        /// <summary>
        /// Gets the path in which log files are saved.
        /// </summary>
        string LogPath { get; }

        /// <summary>
        /// Gets or sets the level of verbosity for the logger.
        /// </summary>
        /// <value>The logging level as a <see cref="LoggingLevel"/> value.</value>
        LoggingLevel LoggingLevel { get; set; }

        /// <summary>
        /// Logs an informational message.  This is a level 4 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Info(string message, params object[] args);
        /// <summary>
        /// Logs an informational message.  This is a level 4 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        void Info(Exception ex);
        /// <summary>
        /// Logs an informational message.  This is a level 4 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Info(string message, Exception ex, params object[] args);

        /// <summary>
        /// Logs a debug message.  This is a level 2 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Debug(string message, params object[] args);
        /// <summary>
        /// Logs a debug message.  This is a level 2 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        void Debug(Exception ex);
        /// <summary>
        /// Logs a debug message.  This is a level 2 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Debug(string message, Exception ex, params object[] args);

        /// <summary>
        /// Logs a warning message.  This is a level 5 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Warn(string message, params object[] args);
        /// <summary>
        /// Logs a warning message.  This is a level 5 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        void Warn(Exception ex);
        /// <summary>
        /// Logs a warning message.  This is a level 5 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Warn(string message, Exception ex, params object[] args);

        /// <summary>
        /// Logs an error message.  This is a level 6 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Error(string message, params object[] args);
        /// <summary>
        /// Logs an error message.  This is a level 6 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        void Error(Exception ex);
        /// <summary>
        /// Logs an error message.  This is a level 6 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Error(string message, Exception ex, params object[] args);

        /// <summary>
        /// Logs a fatal error message.  This is a level 7 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Fatal(string message, params object[] args);
        /// <summary>
        /// Logs a fatal error message.  This is a level 7 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        void Fatal(Exception ex);
        /// <summary>
        /// Logs a fatal error message.  This is a level 7 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Fatal(string message, Exception ex, params object[] args);

        /// <summary>
        /// Logs a metric message.  This is a level 3 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Metric(string message, params object[] args);
        /// <summary>
        /// Logs a metric message.  This is a level 3 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="milliseconds">The number of milliseconds that an operation took.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Metric(string message, double milliseconds, params object[] args);

        /// <summary>
        /// Logs a message about the current platform.  This is a level 1 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Platform(string message, params object[] args);
        /// <summary>
        /// Logs a message about the current platform.  This is a level 1 logging operation.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log.</param>
        void Platform(Exception ex);
        /// <summary>
        /// Logs a message about the current platform.  This is a level 1 logging operation.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the message to log.</param>
        /// <param name="ex">The <see cref="Exception"/> to append to the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void Platform(string message, Exception ex, params object[] args);

        /// <summary>
        /// Returns a list of log files that currently exist in the log path.
        /// </summary>
        IEnumerable<string> GetLogFiles();

        /// <summary>
        /// Deletes any log files that are older than the specified number of days.
        /// </summary>
        /// <param name="days">The number of days.  If a log file's age is equal to or greater than this, it will be deleted.</param>
        void DeleteLogFiles(int days);
    }
}
