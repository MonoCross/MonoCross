using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCross.Utilities.Barcode
{
    public abstract class BarcodeScanner : IBarcodeScanner
    {
        public event EventHandler BarcodeScannerReady;
        public event EventHandler<BarcodeScanEvent> BarcodeScan;

        protected List<Symbology> _available;
        protected List<Symbology> _enabled;

        protected const String DO_UPCE = "do_upce";
        protected const String DO_EAN8 = "do_ean8";
        protected const String DO_EAN13 = "do_ean13";
        protected const String DO_CODE128 = "do_code128";
        protected const String DO_CODE39 = "do_code39";
        protected const String DO_CODE93 = "do_code93";
        protected const String DO_STICKY = "do_sticky";
        protected const String DO_BROADCAST_TO = "do_broadcast_to";

        public const int NONE = 0;
        public const int EAN13 = 1;
        public const int UPCE = 2;
        public const int EAN8 = 4;
        public const int STICKY = 8;
        public const int QRCODE = 16;
        public const int CODE128 = 32;
        public const int CODE39 = 64;
        public const int DATAMATRIX = 128;
        public const int ITF = 256;
        public const int CODE93 = 512;
        public const int RSS14 = 1024;

        protected BarcodeScanner()
        {
            _available = new List<Symbology>();
            _available.Add(Symbology.UPCA);
            _available.Add(Symbology.UPCE);
            _available.Add(Symbology.EAN8);
            _available.Add(Symbology.EAN13);
            _available.Add(Symbology.Code39);
            _available.Add(Symbology.Code93);
            _available.Add(Symbology.Code128);
            _available.Add(Symbology.Sticky);

            _enabled = _available;
        }

        public void TriggerScan(String data, Symbology symbology)
        {
            if (BarcodeScan != null)
                BarcodeScan(this, new BarcodeScanEvent(data, DateTime.Now, symbology));
        }

        public bool Initialize()
        {
            if (BarcodeScannerReady != null)
                BarcodeScannerReady(this, EventArgs.Empty);
            return true;
        }

        public virtual void Start() { }
        public void Stop() { }
        public void Terminate() { }
        public abstract bool NeedsStartToScan { get; }
        public bool IsSymbologyAvailable(Symbology symbology)
        {
            return _available.Contains(symbology);
        }
        public bool EnableSymbology(Symbology symbology)
        {
            if (_available.Contains(symbology))
            {
                _enabled.Add(symbology);
                return true;
            }

            return false;
        }
        public bool DisableSymbology(Symbology symbology)
        {
            if (_available.Contains(symbology))
            {
                _enabled.Remove(symbology);
                return true;
            }

            return false;
        }
        public bool IsSymbologyEnabled(Symbology symbology)
        {
            return _enabled.Contains(symbology);
        }
        public bool EnableSymbologies(List<Symbology> symbologies)
        {
            if (symbologies.Count == 0)
            {
                throw new ArgumentException("You haven't specified any symbologies", "symbologies");
            }

            _enabled.Clear();
            foreach (Symbology symbology in symbologies)
            {
                if (_available.Contains(symbology))
                    _enabled.Add(symbology);
            }

            return true;
        }
    }
}
