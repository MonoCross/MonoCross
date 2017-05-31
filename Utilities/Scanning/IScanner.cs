using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MonoCross;

namespace MonoCross.Scanning
{
    /// <summary>
    /// A Delegate for capturing data when a scan successfully occurs on an IScanner implementation
    /// </summary>
    /// <param name="scannedValue"></param>
    public delegate void ScanEventDelegate(string scannedValue);

    /// <summary>Interface for interacting with data collection devices</summary>
    public interface IScanner 
    {
        /// <summary>This event occurs when a valid scan occurs</summary>
        event ScanEventDelegate ScanOccurred;

        /// <summary>
        /// Initialize the scanning device</summary>
        /// <returns>Indicates if the device initiallized successfully</returns>
        bool InitScanner();

        /// <summary>Terminate the scanning device</summary>
        void TermScanner();

        /// <summary>Signal the scanning device to actively scan</summary>
        void StartScan();

        /// <summary>
        /// Signal the scanning device to actively scan</summary>
        /// <param name="triggerSoftAlways">Require hardware trigger to active scanning</param>
        void StartScan(bool triggerSoftAlways);

        /// <summary>Signal the scanning device to stop actively scanning</summary>
        void StopScan();

        /// <summary>
        /// Signal the scanning device to stop actively scanning</summary>
        /// <param name="scanDelegate">Delegate to unregister from the ScanOccurred event</param>
        void StopScan(ScanEventDelegate scanDelegate);

        /// <summary>
        /// Raise the ScanOccurred event</summary>
        /// <param name="code">Scanned value to pass to the event</param>
        void RaiseScanOccurred(string code);

        /// <summary>Unregister all ScanEventDelegate delegates registered to the ScanOccurred Event</summary>
        void NullifyScanOccurredEvent();
    }
}
