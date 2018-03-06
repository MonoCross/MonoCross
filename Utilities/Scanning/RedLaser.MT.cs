using System;

namespace MonoCross.Utilities.Barcode
{
    public partial class RedLaser
    {
        public static RedLaser GetInstance()
        {
            if (_instance == null)
                _instance = new RedLaser();
            return _instance;
        }
    }
}
