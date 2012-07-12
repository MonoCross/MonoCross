using System.Threading;
using MonoTouch.CoreLocation;

namespace DeviceAccess
{
  public class GpsAccess
  {
    public string ReturnHeading()
    {
      if (_lm != null)
      {
        _lm = new CLLocationManager();
        _lm.UpdatedHeading += HeadingUpdateDelegate;
        _lm.StartUpdatingHeading();
      }

      // block on the search until the async result return
      string result;
      lock (_locker)
      {
        while (location == null)
        {
          Monitor.Pulse(_locker);
        }

        result = location;
        location = null;
      }
      return result;
    }

    void CompassCancel()
    {
      if (_lm != null)
      {
        _lm.StopUpdatingHeading();
        _lm.Dispose();
        _lm = null;
      }
    }
    static CLLocationManager _lm;

    private void HeadingUpdateDelegate(object sender, CLHeadingUpdatedEventArgs e)
    {
      lock (_locker)
      {
        // build new result list
        if (_lm != null && _lm.Heading != null)
        {
          location = _lm.Heading.MagneticHeading.ToString();
        }
        Monitor.Pulse(_locker);
      }
    }
    static readonly object _locker = new object();
    string location = null;
  }
}
