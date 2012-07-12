using System;
using System.Threading;
using Android.Content;
using Android.Hardware;

namespace DeviceAccess
{
  public class CompassSensor : ISensorEventListener
  {
    protected string ReturnHeading()
    {
      SensorManager sm = _context.GetSystemService(Context.SensorService) as SensorManager;

      Sensor sensor = sm.GetDefaultSensor(SensorType.Orientation);
      if (sensor != null)
      {
        sm.RegisterListener(this, sensor, SensorDelay.Ui);
      }

      // block on the search until the async result return
      string heading;
      lock (_locker)
      {
        while (location == null)
        {
          Monitor.Pulse(_locker);
        }

        heading = location;
        location = null;
      }
      return heading;
    }

    protected void CompassCancel()
    {
      SensorManager sm = _context.GetSystemService(Context.SensorService) as SensorManager;
      Sensor sensor = sm.GetDefaultSensor(SensorType.Orientation);
      sm.UnregisterListener(this, sensor);
    }

    public void OnSensorChanged(SensorEvent e)
    {
      if (e.Sensor.Type == SensorType.Proximity)
      {
        lock (_locker)
        {

          location = e.ToString();
          Monitor.Pulse(_locker);
        }
      }
    }
    static readonly object _locker = new object();
    string location = null;

    Context _context;


    public void OnAccuracyChanged(Sensor sensor, int accuracy)
    {
      return;
    }

    public IntPtr Handle
    {
      get { throw new NotImplementedException(); }
    }
  }
}