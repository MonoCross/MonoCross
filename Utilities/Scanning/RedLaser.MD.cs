#if Droid
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace MonoCross.Utilities.Barcode
{
    public partial class RedLaser : BarcodeScanner, IBarcodeScanner
    {
        private Context _context;
        public static RedLaser GetInstance(Context context)
        {
            if (_instance == null)
                _instance = new RedLaser(context);
            return _instance;
        }

        private RedLaser(Context context) : base()
        {
            _context = context;
        }

        public override void Start()
        {
            Bundle hints = new Bundle();
            foreach (Symbology symbology in _enabled)
            {
                switch (symbology)
                {
                    case Symbology.UPCE: hints.PutBoolean(DO_UPCE, true); break;
                    case Symbology.EAN8: hints.PutBoolean(DO_EAN8, true); break;
                    case Symbology.EAN13: hints.PutBoolean(DO_EAN13, true); break;
                    case Symbology.Code39: hints.PutBoolean(DO_CODE93, true); break;
                    case Symbology.Code93: hints.PutBoolean(DO_CODE39, true); break;
                    case Symbology.Code128: hints.PutBoolean(DO_CODE128, true); break;
                    case Symbology.Sticky: hints.PutBoolean(DO_STICKY, true); break;

                    case Symbology.UPCA: break;
                    default: break;
                }
            }
            hints.PutString(DO_BROADCAST_TO, RedLaserScanReceiver.BROADCAST_ACTION);

            Intent i = new Intent();
            i.SetAction("com.target.redlasercontainer.SCAN");
            i.PutExtras(hints);

            Log.Info("BarcodeScanning", "broadcast intent with action com.target.redlasercontainter.SCAN sent");
            try
            {
                _context.SendBroadcast(i);
            }
            catch (Exception ex)
            {
                Log.Error("BarcodeScanning", ex.Message);
            }
        }
    }

    [BroadcastReceiver()]
    [IntentFilter(new string[] { BROADCAST_ACTION })]
    public class RedLaserScanReceiver : BroadcastReceiver
    {
        private const string BARCODE_VALUE = "BARCODE";
        private const string BARCODE_TYPE = "BARCODETYPE";

        public const string BROADCAST_ACTION = "com.target.targetpoc.SCAN";

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent != null)
            {
                String data = intent.GetStringExtra(BARCODE_VALUE);
                String typeString = intent.GetStringExtra(BARCODE_TYPE);
                Symbology symbology = Symbology.UNKNOWN;
                switch (typeString.ToUpper())
                {
                    case "UPCE":
                        symbology = Symbology.UPCE;
                        break;
                    case "EAN13":
                        symbology = Symbology.EAN13;
                        break;
                    case "EAN8":
                        symbology = Symbology.EAN8;
                        break;
                    default:
                        symbology = Symbology.UNKNOWN;
                        break;
                }

                RedLaser.Instance.TriggerScan(data, symbology);
                Log.Info("MonoCross", "response received from scan " + data + " : " + typeString);
            }
            else
            {
                Log.Warn("MonoCross", "weird response received");
            }
        }
    }
}
#endif