using MonoTouch.UIKit;
using System.Threading;

namespace DeviceAccess
{
  public class AccelerometerAccess
  {
    public string ReturnXyz()
    {
      if (UIAccelerometer.SharedAccelerometer != null)
      {
        UIAccelerometer.SharedAccelerometer.UpdateInterval = 0.05;
        UIAccelerometer.SharedAccelerometer.Acceleration += XyzUpdateDelegate;
      }

      // block on the search until the async result return
      string result;
      lock (_locker)
      {
        while (_xyz == null)
        {
          Monitor.Pulse(_locker);
        }

        result = _xyz;
        _xyz = null;
      }
      return result;
    }
    static readonly object _locker = new object();
    string _xyz = null;

    private void XyzUpdateDelegate(object sender, UIAccelerometerEventArgs e)
    {
      lock (_locker)
      {
        _xyz =
            string.Format("{0:0.00}, {1:0.00}, {2:0.00})", e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z);

        Monitor.Pulse(_locker);
      }
    }

    void AccelerometerCancel()
    {
      UIAccelerometer.SharedAccelerometer.UpdateInterval = 0.0;
    }

  }
}
