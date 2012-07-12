using System;
using System.Threading;
using Microsoft.Devices.Sensors;

namespace DeviceAccess
{
  public class GpsAccess
  {
    static Compass _compass;

    public string ReturnHeading()
    {
      if (!Compass.IsSupported)
      {
        throw new Exception("Could not load Compass");
      }
      if (_compass == null)
      {
        _compass = new Compass();
        _compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(100);
        _compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(_compass_CurrentValueChanged);
        try
        {
          _compass.Start();
        }
        catch (InvalidOperationException e)
        {
          if (_compass != null)
          {
            _compass.Dispose();
            _compass = null;
          }

          throw new Exception("Could not initiate compass readings", e);
        }
      }

      string trueHeading = null;
      if (_compass != null)
      {
        // block on the search until the async result return
        lock (_locker)
        {
          while (_trueHeading == null)
          {
            Monitor.Pulse(_locker);
          }

          trueHeading = _trueHeading;
          _trueHeading = null;
        }
      }
      return trueHeading;
    }
    static readonly object _locker = new object();
    string _trueHeading = null;

    void _compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
    {
      lock (_locker)
      {

        _trueHeading = e.SensorReading.TrueHeading.ToString();
        Monitor.Pulse(_locker);
      }
    }

    void CompassCancel()
    {
      if (_compass != null)
      {
        _compass.Stop();
        _compass.Dispose();
        _compass = null;
      }
    }
  }
}
