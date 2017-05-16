using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MonoCross.Utilities.Storage;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// 
    /// </summary>
    internal class ConsoleWritelineLogger : ILog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicLogger"/> class.
        /// </summary>
        /// <param name="logPath">A <see cref="String"/> representing the Log path value.</param>
        internal ConsoleWritelineLogger(string logPath) { }

        #region ILog Members

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Info(string message)
        {
			Console.WriteLine("BasicLogger Info: " + message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Warn(string message)
        {
			Console.WriteLine("BasicLogger Warn: " + message);
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Debug(string message)
        {
			Console.WriteLine("BasicLogger Debug: " + message);
        }

        /// <summary>
        /// logs an error message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Error(string message)
        {
			Console.WriteLine("BasicLogger Error: " + message);
        }

        /// <summary>
        /// Logs an error message from the specified exception.
        /// </summary>
        /// <param name="ex">A <see cref="Exception"/> representing the Ex value.</param>
        public void Error(Exception ex)
        {
			Console.WriteLine("BasicLogger Error: " + ex.ToString());
        }

        /// <summary>
        /// Logs a fatal error message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Fatal(string message)
        {
			Console.WriteLine("BasicLogger Fatal: " + message);
        }

        /// <summary>
        /// Logs a fatal error message from the specified exception.
        /// </summary>
        /// <param name="ex">A <see cref="Exception"/> representing the Ex value.</param>
        public void Fatal(Exception ex)
        {
			Console.WriteLine("BasicLogger  Fatal (Exception:)" + ex.ToString());
        }

        #endregion

    }
}
