#if DROID || TOUCH
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCross.Utilities.Barcode
{
    public partial class RedLaser : BarcodeScanner
    {
        private static RedLaser _instance = null;

        private RedLaser()
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

        public override bool NeedsStartToScan
        {
            get { return true; }
        }
    }
}
#endif