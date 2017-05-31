using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using MonoCross;
using MonoCross.Utilities;

namespace MonoCross.Scanning
{

    /// <summary>
    /// An IScanner implementation for testing applications in simulators, or in unit tests
    /// </summary>
    public class MockScanner : Scanner, IDisposable
    {
        private int _delay;
        private string _barcode;

        /// <summary>
        /// This event occurs when a valid scan occurs
        /// </summary>
        public override event ScanEventDelegate ScanOccurred;

        /// <summary>
        /// Initialize the scanning device</summary>
        /// <returns>Indicates if the device initiallized successfully</returns>
        public override bool InitScanner() { return true; }

        /// <summary>
        /// Terminate the scanning device
        /// </summary>
        public override void TermScanner() { }

        /// <summary>
        /// Signal the scanning device to actively scan
        /// </summary>
        public override void StartScan()
        {
            StartScan(false);
        }

        /// <summary>
        /// Signal the scanning device to actively scan</summary>
        /// <param name="triggerSoftAlways">Require hardware trigger to active scanning</param>
        public override void StartScan(bool triggerSoftAlways)
        {
            if (_delay > 0)
                Device.Thread.QueueWorker((o) => { SimulateScan(_barcode, _delay); });
        }

        private void SimulateScan(string code, int delay)
        {
            new ManualResetEvent(false).WaitOne(delay);

            var scanEvent = ScanOccurred;
            if (scanEvent != null)
                scanEvent(code);
        }

        /// <summary>
        /// Signal the scanning device to stop actively scanning
        /// </summary>
        public override void StopScan() { }

        /// <summary>
        /// Signal the scanning device to stop actively scanning</summary>
        /// <param name="scanDelegate">Delegate to unregister from the ScanOccurred event</param>
        public override void StopScan(ScanEventDelegate scanDelegate)
        {
            ScanOccurred -= scanDelegate;
        }

        /// <summary>
        /// Unregister all ScanEventDelegate delegates registered to the ScanOccurred Event
        /// </summary>
        public override void NullifyScanOccurredEvent()
        {
            ScanOccurred = null;
        }

        /// <summary>
        /// Create a MockScanner for testing applications in simulators, or in unit tests
        /// </summary>
        public MockScanner() 
        { 
            _delay = -1;
            _barcode = null;
        }

        /// <summary>
        /// Create a MockScanner for testing applications in simulators, or in unit tests
        /// </summary>
        /// <param name="delay">
        /// Number of milliseconds after calling StartScan() that a simulated scan will occur
        /// </param>
        /// <param name="barcode">Barcode value the simulated scan will produce</param>
        public MockScanner(int delay, string barcode) 
        { 
            _delay = delay;
            _barcode = barcode;
        }
    
        /// <summary>
        /// Clean up resources: Stop Scanning, Terminate Scanner, Nullify ScanOccurred Event handler
        /// </summary>
        public void Dispose()
        {
            try { StopScan(); }
            catch (Exception) { }
 	        finally { NullifyScanOccurredEvent(); }
        }
    }
}
