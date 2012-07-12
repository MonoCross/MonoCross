using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Android.Util;
using Android.Media;

using Android.Hardware;

namespace WebHybrid.Droid
{
    [Activity(Label = "Web Hybrid", MainLauncher = true)]
    public class WebViewActivity : Activity
    {
        WebView _webView;

        public MediaPlayer Player
        {
            get {
                if (_player == null) {
                    _player = new MediaPlayer();
                }
                return _player;
            }
        }
        MediaPlayer _player;

		protected class WebViewClientOverride : WebViewClient, ISensorEventListener
        {
            Context _context;
			WebView _webView;

            public WebViewClientOverride(Context parent, WebView webView)
            {
                _context = parent;
				_webView = webView;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                if (url.ToLower().StartsWith("hybrid://")) {
                    string actionUri = url.Substring(9);
                    CallNativeMethod(actionUri);
                    return true;
                }
                return base.ShouldOverrideUrlLoading(view, url);
            }

            void CallNativeMethod(string actionUri)
            {
                string[] paramArray = actionUri.Split(new Char[] { '/' });
                string action = paramArray[0].ToString();
                string[] itemArray = paramArray.Skip(1).Take(paramArray.Length - 1).ToArray();

                Log.Info("WebViewActivity", "CallNativeMethod: " + action);

                switch (action) {
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
                    MonoCross.Utilities.Notification.Notify.PlaySound(_context, itemArray[0]);
                    break;
                case "notify":
                    if (itemArray.Length >= 1) {
                        if (itemArray[0].Equals("vibrate")) {
                            MonoCross.Utilities.Notification.Notify.Vibrate(_context, 500);
                        } else if (itemArray[0].Equals("playSound")) {
                            MonoCross.Utilities.Notification.Notify.PlaySound(_context, itemArray[1]);
                        }
                    }
                    break;
                }
            }
			
			protected void CompassStart()
			{
				SensorManager sensorManager = _context.GetSystemService(Context.SensorService) as SensorManager;
				Sensor sensor = sensorManager.GetDefaultSensor(SensorType.Orientation);
				if (sensor != null) {
					sensorManager.RegisterListener(this, sensor, SensorDelay.Ui);
				} else {
					_webView.LoadUrl("javascript:compass.onCompassFail('No Compass Found');");
				}
			}

			static void CompassCancel() {
			}		

			protected void AccelerometerStart()
			{
				SensorManager sensorManager = _context.GetSystemService(Context.SensorService) as SensorManager;
				Sensor sensor = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
				if (sensor != null) {
					sensorManager.RegisterListener(this, sensor, SensorDelay.Ui);
				} else {
					_webView.LoadUrl("javascript:accelerometer.onAccelerometerFail('No Accelerometer Found');");
				}
			}

			static void AccelerometerCancel()
            {
			}		

			public void OnAccuracyChanged (Sensor sensor, int accuracy)
			{
			}

			public void OnSensorChanged (SensorEvent e) {
				string callBack = "";
				switch (e.Sensor.Type) {
				case SensorType.Orientation:
					callBack = string.Format("javascript:compass.onCompassSuccess({0:0.00})", e.Values[0]);
					break;
				case SensorType.Accelerometer:
					callBack = string.Format("javascript:accelerometer.onAccelerometerSuccess({0:0.00})", e.Values[0], e.Values[1], e.Values[2]);
					break;
				}
				if (callBack.Length > 0) {
					_webView.LoadUrl(callBack);
				}
			}
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our web view, enable javascript and override the container to enable integration
            _webView = FindViewById<WebView>(Resource.Id.WebView);
            _webView.ScrollBarStyle = ScrollbarStyles.InsideOverlay;
            _webView.Settings.JavaScriptEnabled = true;
            _webView.SetWebViewClient(new WebViewClientOverride(this, _webView));
            _webView.LoadUrl(@"file:///android_asset/container.html");
        }

		protected override void OnPause ()
		{
			base.OnPause ();
			// release all pending resources
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();
			// resume any activities we were do
		}
    }
}