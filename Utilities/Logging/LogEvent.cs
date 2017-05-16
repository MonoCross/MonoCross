using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace MonoCross.Utilities.Logging
{
	[XmlRootAttribute("LogEvent", Namespace="api.tgt.com", IsNullable = false)]
	public class LoggerEvent
	{	
		private LogMessageType messageType;
		
		public LoggerEvent()
		{
		}
		
		public LoggerEvent(string[] logEntryArray)
		{
			Type = logEntryArray[0];
			Message = logEntryArray[1];
			//Timestamp = DateTime.UtcNow;
		}
		
		public string Type { get; set; }
		
		public string Application { get; set; }
		
		public string Module { get; set; }
		
		public string Message { get; set; }
		
		public string Platform { get; set; }
		
		public string User { get; set; }
		
		public string URI { get; set; }
		
		public string IPAddress { get; set; }
		
		public string StackTrace { get; set; }
		
		public DateTime Timestamp { get; set; }
		
		[System.Xml.Serialization.XmlIgnoreAttribute]
		public LogMessageType MessageType
		{
			get
			{
				return messageType;
			}
			
			set
			{
				switch(Type)
				{
				case "info":
					messageType = LogMessageType.Info;
					break;
				case "debug":
					messageType = LogMessageType.Debug;
					break;
				case "warn":
					messageType = LogMessageType.Warn;
					break;
				case "error":
					messageType = LogMessageType.Error;
					break;
				case "fatal":
					messageType = LogMessageType.Fatal;
					break;
				case "metric":
					messageType = LogMessageType.Metric;
					break;
				default:
					messageType = LogMessageType.Debug;
					break;
				}
			}
		}
		
	}
}

