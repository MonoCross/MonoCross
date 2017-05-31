using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCross.Scanning
{
    /// <summary>
    /// Default singleton instance for IScanner implementations
    /// </summary>
    public class Scanner : IScanner
    {
        /// <summary>
        /// Singleton instance of an IScanner implementation
        /// </summary>
        public static IScanner Instance
        {
            get
            {
                if (!IsInitialized)
                {
                    var explaination = new NullReferenceException("Must call Scanner.Initialize first");
                    throw new Exception("MonoCross.Scanning.Scanner", explaination);
                }
                return _instance;
            }

            //TODO: Create a lock object for threading safety.
            protected set { _instance = value; }
        }
        private static IScanner _instance;

        /// <summary>
        /// Indicates whether <see cref="Instance"/> has been initialized using <see cref="Initialize"/>
        /// </summary>
        public static bool IsInitialized
        {
            get { return _instance != null; }
        }

        /// <summary>
        /// Initialize the singleton object with an instance of an IScanner implementation
        /// </summary>
        /// <param name="scanner">Instance of an IScanner implementation</param>
        public static void Initialize(IScanner scanner)
        {
            if (scanner == null)
                throw new ArgumentNullException("scanner");
            else
                Scanner.Instance = scanner;
        }

        /// <summary>
        /// Obsolete scanner initialization method
        /// </summary>
        /// <param name="scanner">Instance of an IScanner implementation</param>
        [Obsolete("Please use the Initialize method.")]
        public static void Initalize(IScanner scanner)
        {
            Initialize(scanner);
        }

        /// <summary>
        /// This event occurs when a valid scan occurs
        /// </summary>
        public virtual event ScanEventDelegate ScanOccurred;

        /// <summary>
        /// Initialize the scanning device</summary>
        /// <returns>Indicates if the device initialized successfully</returns>
        public virtual bool InitScanner()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Terminate the scanning device
        /// </summary>
        public virtual void TermScanner()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Signal the scanning device to actively scan
        /// </summary>
        public virtual void StartScan()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Signal the scanning device to actively scan</summary>
        /// <param name="triggerSoftAlways">Require hardware trigger to active scanning</param>
        public virtual void StartScan(bool triggerSoftAlways)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Signal the scanning device to stop actively scanning
        /// </summary>
        public virtual void StopScan() { throw new NotImplementedException(); }

        /// <summary>
        /// Signal the scanning device to stop actively scanning</summary>
        /// <param name="scanDelegate">Delegate to unregister from the ScanOccurred event</param>
        public virtual void StopScan(ScanEventDelegate scanDelegate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Raise the ScanOccurred event</summary>
        /// <param name="code">Scanned value to pass to the event</param>
        public virtual void RaiseScanOccurred(string code)
        {
            var scanOccurred = ScanOccurred;
            if (scanOccurred != null)
                scanOccurred(code);
        }

        /// <summary>
        /// Unregister all ScanEventDelegate delegates attached to the ScanOccurred Event
        /// </summary>
        public virtual void NullifyScanOccurredEvent()
        {
            throw new NotImplementedException();
        }
    }
}
