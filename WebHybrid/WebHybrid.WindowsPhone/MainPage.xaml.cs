using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WebHybrid.WindowsPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.webBrowser1.IsScriptEnabled = true;
            this.webBrowser1.Base = "www";
            this.webBrowser1.Navigate(new Uri("container.html", UriKind.Relative));
            this.webBrowser1.Navigating += new EventHandler<NavigatingEventArgs>(webBrowser1_Navigating);
            this.webBrowser1.NavigationFailed += new System.Windows.Navigation.NavigationFailedEventHandler(webBrowser1_NavigationFailed);
            this.webBrowser1.ScriptNotify += new EventHandler<NotifyEventArgs>(webBrowser1_ScriptNotify);
        }

        void webBrowser1_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value.ToLower().StartsWith("hybrid://")) {
                string actionUri = e.Value.Substring(9);
                CallNativeMethod(actionUri);
            }
        }

        void CallNativeMethod(string actionUri)
        {
            string[] paramArray = actionUri.Split(new Char[] { '/' });
            string action = paramArray[0];
            string[] itemArray = paramArray.Skip(1).Take(paramArray.Length - 1).ToArray();

            switch (action)
            {
                case "compass":
                    if (itemArray[0].Equals("start"))
                        CompassStart();
                    else if (itemArray[0].Equals("cancel"))
                        CompassCancel();
                    break;

                case "accelerometer":
                    if (itemArray[0].Equals("start"))
                        AccelerometerStart();
                    else if (itemArray[0].Equals("cancel"))
                        AccelerometerCancel();
                    break;

                case "media":
                    MonoCross.Utilities.Notification.Notify.PlaySound(itemArray[0]);
                    break;

                case "notify":
                    if (itemArray.Length >= 1) {
                        if (itemArray[0].Equals("vibrate"))
                            MonoCross.Utilities.Notification.Notify.Vibrate();
                        else if (itemArray[0].Equals("playSound"))
                            MonoCross.Utilities.Notification.Notify.PlaySound(itemArray[1]);
                    }
                    break;

                default:
                    break;
            }
        }

        static Compass _compass;

        void CompassStart()
        {
            if (!Compass.IsSupported) {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    this.webBrowser1.InvokeScript("eval", "compass.onCompassFail('Compass not available.')");
                });
                return;
            }
            if (_compass == null) {
                _compass = new Compass();
                _compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(100);
                _compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(_compass_CurrentValueChanged);
                try {
                    _compass.Start();
                }
                catch (InvalidOperationException) {
                    this.webBrowser1.InvokeScript("eval", "compass.onCompassFail('Could not start the compass.')");
                }
            }
        }

        void _compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            string callBack = string.Format("compass.onCompassSuccess({0:0.00})", e.SensorReading.TrueHeading);
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                this.webBrowser1.InvokeScript("eval", callBack);
            });
        }

        void CompassCancel()
        {
            if (_compass != null) {
                _compass.Stop();
                _compass.Dispose();
                _compass = null;
            }
        }

        Accelerometer _accelerometer = new Accelerometer();

        void AccelerometerStart()
        {
            if (!Accelerometer.IsSupported) {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    this.webBrowser1.InvokeScript("eval", "compass.onCompassFail('Accelerometer not available.')");
                });
                return;
            }
            try {
                // Start accelerometer for detecting compass axis
                _accelerometer = new Accelerometer();
                _accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>
                    (_accelerometer_CurrentValueChanged);
                _accelerometer.Start();
            } catch (InvalidOperationException) {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    this.webBrowser1.InvokeScript("eval", "compass.onCompassFail('Could not start the accelerometer.')");
                });
            }
        }

        void _accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            string callBack = string.Format("accelerometer.onAccelerometerSuccess({0:0.00}, {1:0.00}, {2:0.00})",
                e.SensorReading.Acceleration.X, e.SensorReading.Acceleration.Y, e.SensorReading.Acceleration.Z);

            Deployment.Current.Dispatcher.BeginInvoke(() => {
                this.webBrowser1.InvokeScript("eval", callBack);
            });
        }

        void AccelerometerCancel()
        {
            if (_accelerometer != null) {
                _accelerometer.Stop();
                _accelerometer.Dispose();
                _accelerometer = null;
            }
        }

        void webBrowser1_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
        }

        void webBrowser1_Navigating(object sender, NavigatingEventArgs e)
        {
        }
    }
}