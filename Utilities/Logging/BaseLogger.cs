using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// Represents a base class for logging utilities.  This class is abstract.
    /// </summary>
    public abstract class BaseLogger : ILog
    {
        private static readonly object PadLock = new object();

        /// <summary>
        /// Gets the extension of the saved log files.
        /// </summary>
        protected virtual string FileExt { get { return ".log"; } }

        /// <summary>
        /// Gets the name of the saved log files.
        /// </summary>
        protected virtual string FileType { get { return Device.Platform + "Logger"; } }

        /// <summary>
        /// Gets the path in which log files are saved.
        /// </summary>
        public virtual string LogPath
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the name of the log file when it is saved.
        /// </summary>
        public virtual string LogFileName
        {
            get
            {
                return LogPath.AppendPath(LogHelper.GetFileNameYYYMMDD(FileType, FileExt));
            }
        }

        /// <summary>
        /// Gets or sets the level of verbosity for the logger.
        /// </summary>
        /// <value>The logging level as a <see cref="LoggingLevel"/> value.</value>
        public virtual LoggingLevel LoggingLevel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLogger"/> class.
        /// </summary>
        /// <param name="logPath">A <see cref="String"/> representing the path in which to save the log files.</param>
        public BaseLogger(string logPath)
        {
            InitLogger(logPath);
        }

        /// <summary>
        /// Initializes the logger and sets the save path to the specified string.
        /// </summary>
        /// <param name="logPath">A <see cref="String"/> representing the path in which to save the log files.</param>
        protected virtual void InitLogger(string logPath)
        {
#if DEBUG
            LoggingLevel = 0;
#else
            LoggingLevel = LoggingLevel.Warn;
#endif

            LogPath = logPath;
            Device.File.EnsureDirectoryExists(LogFileName);

            // Delete log files 3 weeks old and older.
            DeleteLogFiles(21);
        }

        #region ILog Members

        /// <summary>
        /// Occurs when a message has been logged.
        /// </summary>
        public event LogEvent OnLogEvent;

        /// <summary>
        /// Called when a message has been logged.
        /// </summary>
        /// <param name="logEventArgs">Contains information about the message that was logged.</param>
        public void LogEvent(LogEventArgs logEventArgs)
        {
            // throw all logging events to subscriber if there is subscriber(s)
            var logEvent = OnLogEvent;
            if (logEvent != null) logEvent(logEventArgs);
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
            message = string.Format("{0} : Milliseconds: {1}", message, milliseconds);
            AppendLog(message, LogMessageType.Metric, null, args);
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
        public virtual IEnumerable<string> GetLogFiles()
        {
            return Device.File.GetFileNames(LogPath).Where(name => name.EndsWith(FileExt));
        }

        /// <summary>
        /// Deletes any log files that are older than the specified number of days.
        /// </summary>
        /// <param name="days">The number of days.  If a log file's age is equal to or greater than this, it will be deleted.</param>
        public void DeleteLogFiles(int days)
        {
#if NETFX_CORE
            var files = GetLogFiles().Select(file =>
            {
                var task = Windows.Storage.StorageFile.GetFileFromPathAsync(Path.Combine(LogPath, file)).AsTask();
                task.Wait();
                return task.Result;
            }).Where(fi => DateTimeOffset.UtcNow.Subtract(fi.DateCreated.ToUniversalTime()).Days >= days);
            foreach (var file in files)
                Device.File.Delete(file.Path);
#elif SILVERLIGHT
            var store = ((FileSystem.SLFile)Device.File)._store;
            var files = GetLogFiles().Where(file => DateTime.UtcNow.Subtract(store.GetLastWriteTime(file).UtcDateTime).Days >= days);
            foreach (var file in files)
                Device.File.Delete(file);
#else
            var files = GetLogFiles().Select(file => new FileInfo(file)).Where(fi => DateTime.UtcNow.Subtract(fi.LastWriteTime.ToUniversalTime()).Days >= days);
            foreach (var file in files)
                Device.File.Delete(file.FullName);
#endif
        }

        #endregion

        #region Private Help Methods

        // Help methods that use our FileSystem abstraction
        /// <summary>
        /// Appends the specified message to the current log file.
        /// </summary>
        /// <param name="message">The message to append to the current log file.</param>
        /// <param name="messageType">The type of log message.  If this is less than the current logging level, the message will not be appended.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public virtual void AppendLog(String message, LogMessageType messageType, Exception exception, params object[] args)
        {
            if ((int)LoggingLevel > (int)messageType)
                return;

            int threadId;
#if NETFX_CORE
            threadId = Environment.CurrentManagedThreadId;
#else
            threadId = Thread.CurrentThread.ManagedThreadId;
#endif

            message = string.Format(message, args);
            string textEntry = string.Format("{0:MM-dd-yyyy HH:mm:ss:ffff} :{1}: [{2}] {3}", DateTime.Now, threadId, messageType, message);

            // throw all logging events to subscriber if there is subscriber(s)
            LogEvent(new LogEventArgs { LogLevel = messageType, Message = message, Exception = exception, });

            AppendText(LogFileName, textEntry);
        }

        /// <summary>
        /// Appends the specified <see cref="Exception"/> to the current log file.
        /// </summary>
        /// <param name="ex">The exception to append to the current log file.</param>
        /// <param name="messageType">The type of log message.  If this is less than the current logging level, the exception will not be appended.</param>
        public virtual void AppendLog(Exception ex, LogMessageType messageType)
        {
            AppendLog(LogHelper.BuildExceptionMessage(ex, messageType), messageType, ex);
        }

        #endregion

        #region File Helper Methods

        /// <summary>
        /// Appends the specified text to the file at the specified path.
        /// </summary>
        /// <param name="filename">A <see cref="String"/> representing the path of the file.</param>
        /// <param name="value">A <see cref="String"/> representing the text to append.</param>
        protected virtual void AppendText(string filename, string value)
        {
            lock (PadLock)
            {
#if NETFX_CORE
                var folderTask = Windows.Storage.StorageFolder.GetFolderFromPathAsync(Device.File.DirectoryName(filename)).AsTask();
                folderTask.Wait();
                var fileTask = folderTask.Result.CreateFileAsync(Path.GetFileName(filename), Windows.Storage.CreationCollisionOption.OpenIfExists).AsTask();
                fileTask.Wait();
                Windows.Storage.FileIO.AppendTextAsync(fileTask.Result, value + Environment.NewLine).AsTask().Wait();
#elif SILVERLIGHT
                var store = ((FileSystem.SLFile)Device.File)._store;
                if (value.Length > store.AvailableFreeSpace && !((FileSystem.SLFile)Device.File).IncreaseStorage(value.Length + (store.Quota - store.AvailableFreeSpace)))
                    return;

                System.IO.IsolatedStorage.IsolatedStorageFileStream fileStream = null;
                try
                {
                    fileStream = store.OpenFile(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fileStream.Seek(0, SeekOrigin.End);
                    using (var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                    {
                        writer.WriteLine(value);
                        writer.Close();
                    }
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
#else
                StreamWriter sw = null;
                try
                {
                    FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Seek(0, SeekOrigin.End);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(value);
                }
                catch (Exception)
                {
                    //Swallow any errors here... If we tried logging our error, we might end up back here.
                }
                finally
                {
                    if (sw != null)
                        try { sw.Close(); }
                        catch { }
                    //if (fs != null)
                    //{
                    //    //fs.Close();
                    //    fs.Dispose();
                    //}

                }
#endif
            }
        }

        #endregion
    }
}
