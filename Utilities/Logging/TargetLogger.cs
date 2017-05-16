using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using MonoCross.Utilities.Serialization;
using MonoCross.Utilities.Storage;
using System.Net;

//using Android.Util;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class TargetLogger : ILog
    {
        const string _fileExt = ".log";
        const string _fileType = "_TargetLogger";
		
		public List<LogEvent> LogEvents = new List<LogEvent>();
		public static RestfulQueue<LogEvent> LogEventQueue = new RestfulQueue<LogEvent>("http://www.postbin.org/16u5rjh");
		
        internal string LogPath
        {
            get;
            private set;
        }
        static readonly object padlock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetLogger"/> class.
        /// </summary>
        /// <param name="logPath">A <see cref="String"/> representing the Log path value.</param>
        internal TargetLogger( string logPath )
        {
            LogPath = logPath;
        }

        #region ILog Members

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Info( string message )
        {
            AppendLog( message, LogMessageType.Info );
        }
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        public void Info( Exception exc )
        {
            AppendLog( exc, LogMessageType.Info );
        }        
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Info( string message, Exception exc )
        {
            AppendLog( message, LogMessageType.Info );
            AppendLog( exc, LogMessageType.Info );
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Warn( string message )
        {
            AppendLog( message, LogMessageType.Warn );
        }        
        /// <summary>
        /// Logs an warning message.
        /// </summary>
        public void Warn( Exception exc )
        {
            AppendLog( exc, LogMessageType.Warn );
        }
        /// <summary>
        /// Logs an warning message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Warn( string message, Exception exc )
        {
            AppendLog( message, LogMessageType.Warn );
            AppendLog( exc, LogMessageType.Warn );
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Debug( string message )
        {
            AppendLog( message, LogMessageType.Debug );
        }
        /// <summary>
        /// Logs an debug message.
        /// </summary>
        public void Debug( Exception exc )
        {
            AppendLog( exc, LogMessageType.Debug );
        }
        /// <summary>
        /// Logs an debug message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Debug( string message, Exception exc )
        {
            AppendLog( message, LogMessageType.Debug );
            AppendLog( exc, LogMessageType.Debug );
        }


        /// <summary>
        /// logs an error message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Error( string message )
        {
            AppendLog( message, LogMessageType.Error );
        }

        /// <summary>
        /// Logs an error message from the specified exception.
        /// </summary>
        /// <param name="ex">A <see cref="Exception"/> representing the Ex value.</param>
        public void Error( Exception ex )
        {
            AppendLog( ex, LogMessageType.Error );
        }        
        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Error( string message, Exception exc )
        {
            AppendLog( message, LogMessageType.Error );
            AppendLog( exc, LogMessageType.Error );
        }


        /// <summary>
        /// Logs a fatal error message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Fatal( string message )
        {
            AppendLog( message, LogMessageType.Fatal );
        }

        /// <summary>
        /// Logs a fatal error message from the specified exception.
        /// </summary>
        /// <param name="ex">A <see cref="Exception"/> representing the Ex value.</param>
        public void Fatal( Exception ex )
        {
            AppendLog( ex, LogMessageType.Fatal );
        }
        /// <summary>
        /// Logs an fatal message.
        /// </summary>
        /// <param name="message">A <see cref="String"/> representing the Message value.</param>
        public void Fatal( string message, Exception exc )
        {
            AppendLog( message, LogMessageType.Fatal );
            AppendLog( exc, LogMessageType.Fatal );
        }


        #endregion

        #region Private Help Methods

        // Help methods that use our FileSystem abstraction
        public void AppendLog( String message, LogMessageType messageType )
        {
            string textEntry = string.Format( "{0:MM-dd-yyyy HH:mm:ss:ffff} :{1}: [{2}] {3}", DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, messageType.ToString(), message );
            Console.WriteLine( textEntry );

            string fileName = LogPath + LogHelper.GetFileNameYYYMMDD( _fileType, _fileExt );
            FileHelper.AppendText( fileName, textEntry );
        }

        public void AppendLog( Exception ex, LogMessageType messageType )
        {
            string fileName = LogPath + LogHelper.GetFileNameYYYMMDD( _fileType, _fileExt );

            string textEntry = LogHelper.BuildExceptionMessage( ex, messageType );
            Console.WriteLine( textEntry );

            FileHelper.AppendText( fileName, textEntry );
        }
		
		public void AppendLog(LogEvent logEvent)
        {
			LogEventQueue.Enqueue(logEvent);
        }
		
		public void SendLog()
        {
            //send batch via web service
//			while(LogEventQueue.Count > 0)
//			{
//				LogEventQueue.AttemptNextTransaction();
//			}
        }
		
		private string PostObject(string url, string data)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/xml; charset=utf-8";
            request.KeepAlive = false;

            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteData.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }
        #endregion
    }
}
