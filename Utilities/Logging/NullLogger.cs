using System;

namespace MonoCross.Utilities.Logging
{
    internal class NullLogger : ILog
    {
        #region ILog Members
        public void Info(string message, params object[] args) { }
        public void Info(Exception ex) { }
        public void Info(string message, Exception ex, params object[] args) { }

        public void Warn(string message, params object[] args) { }
        public void Warn(Exception ex) { }
        public void Warn(string message, Exception ex, params object[] args) { }

        public void Debug(string message, params object[] args) { }
        public void Debug(Exception ex) { }
        public void Debug(string message, Exception ex, params object[] args) { }

        public void Error(string message, params object[] args) { }
        public void Error(Exception ex) { }
        public void Error(string message, Exception ex, params object[] args) { }

        public void Fatal(string message, params object[] args) { }
        public void Fatal(Exception ex) { }
        public void Fatal(string message, Exception ex, params object[] args) { }

        public void Metric(string message, params object[] args) { }
        public void Metric(string message, double milliseconds, params object[] args) { }
        #endregion
    }
}