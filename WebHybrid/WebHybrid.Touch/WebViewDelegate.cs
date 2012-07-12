using System;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using MonoTouch.CoreLocation;

namespace WebHybrid.Touch
{
	public class WebViewDelegate : UIWebViewDelegate
	{
		UIWebView _webView = null;
			
		public WebViewDelegate(UIWebView webView)
		{
			_webView = webView;
		}
		
		public override void LoadFailed (UIWebView webView, NSError error)
		{
			
		}
		
		public override bool ShouldStartLoad (UIWebView webView, MonoTouch.Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
		{	
			string actionUri = string.Empty;
			string uri = request.Url.AbsoluteUrl.ToString();
			if(uri.ToLower().Contains("hybrid://"))
			{
				actionUri = uri.Substring(9);
				CallNativeMethod(actionUri);   
				return false;
			}
			return true;
		}
		
		public override void LoadingFinished (UIWebView webView)
		{
		}
		
        void CallNativeMethod(string actionUri)
        {
            string[] paramArray = actionUri.Split(new Char[] { '/' });
            string action = paramArray[0].ToString();
            string[] itemArray = paramArray.Skip(1).Take(paramArray.Length - 1).ToArray();

            System.Console.WriteLine("WebViewActivity: CallNativeMethod: " + action);

            switch (action)
            {
			case "compass":
                if (itemArray[0].Equals("start")) {
					CompassStart();
				} else if (itemArray[0].Equals("cancel")) {
					CompassCancel();
				}
				break;

			case "accelerometer":
                if (itemArray[0].Equals("start")) {
					AccelerometerStart();
				} else if (itemArray[0].Equals("cancel")) {
					AccelerometerCancel();
				}
				break;
				
            case "media":
                MonoCross.Utilities.Notification.Notify.PlaySound(itemArray[0]);
                break;

            case "notify":
                if (itemArray.Length >= 1)
                {
                    if (itemArray[0].Equals("vibrate"))
                    {
                        MonoCross.Utilities.Notification.Notify.Vibrate();
                    }
                    else if (itemArray[0].Equals("playSound"))
                    {
                        MonoCross.Utilities.Notification.Notify.PlaySound(itemArray[1]);
                    }
                }
                break;

            default:
                break;
            }
        }
		
		static CLLocationManager _locationManager;
		
		void CompassStart()
		{
			if (_locationManager != null) {
				_locationManager = new CLLocationManager();
				//_locationManager.Delegate = new CLLocationManagerDelegate();
				_locationManager.UpdatedHeading += delegate(object sender, CLHeadingUpdatedEventArgs e) {
					string javascript = string.Format("compass.onCompassSuccess({0:0.00})", _locationManager.Heading.MagneticHeading);
					_webView.EvaluateJavascript(javascript);
				};
				_locationManager.StartUpdatingHeading();
			}
		}

		void CompassCancel()
		{
			_locationManager.StopUpdatingHeading();
		}		

		void AccelerometerStart()
		{
			if (UIAccelerometer.SharedAccelerometer != null) {
				UIAccelerometer.SharedAccelerometer.Acceleration += delegate(object sender, UIAccelerometerEventArgs e) {
					string javascript = string.Format("compass.onCAccelerometerSuccess({0:0.00}, {1:0.00}, {2:0.00})",
						e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z);
					_webView.EvaluateJavascript(javascript);
				};
				UIAccelerometer.SharedAccelerometer.UpdateInterval = 0.05;
			}
			
		}

		void AccelerometerCancel()
		{
			UIAccelerometer.SharedAccelerometer.UpdateInterval = 0.0;
		}
	}
}

