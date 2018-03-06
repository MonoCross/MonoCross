#if TOUCH
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;

using LineaSDK;

namespace MonoCross.Utilities.Barcode
{
	public class LineaPro : BarcodeScanner, IBarcodeScanner
    {
        private static LineaPro _instance = null;
		
		LineaSDK.Linea Linea;
		LineaProDelegate LineaDelegate;
		
		public static LineaPro GetInstance()
        {
            if (_instance == null)
                _instance = new LineaPro();
            return _instance;
        }		
		public static LineaPro GetInstance(UIWebView webView)
        {
            if (_instance == null)
                _instance = new LineaPro(webView);
            return _instance;
        }
		
		public LineaPro() : base()
		{			
			Linea = new LineaSDK.Linea();
		    LineaDelegate = new LineaProDelegate();
		    Linea.Delegate = LineaDelegate;
			Linea.Connect();			
		}
		
		public LineaPro(UIWebView webView) : base()
		{			
			Linea = new LineaSDK.Linea();
		    LineaDelegate = new LineaProDelegate(webView);
		    Linea.Delegate = LineaDelegate;
			Linea.Connect();
			
		}
    }
}
#endif