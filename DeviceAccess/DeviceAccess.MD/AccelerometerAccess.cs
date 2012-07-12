using System;
using Android.Content;
using Android.Hardware;

namespace DeviceAccess
{
  class AccelerometerSensor : ISensorEventListener
  {
    Context _context;

    protected void AccelerometerStart()
    {
      SensorManager sm = _context.GetSystemService(Context.SensorService) as SensorManager;
      Sensor sensor = sm.GetDefaultSensor(SensorType.Accelerometer);
      if (sensor != null)
      {
        sm.RegisterListener(this, sensor, SensorDelay.Ui);
      }
      else
      {
        throw new Exception("Could not load Accelerometer sensor");
      }
    }

    void AccelerometerCancel()
    {
      SensorManager sm = _context.GetSystemService(Context.SensorService) as SensorManager;
      Sensor sensor = sm.GetDefaultSensor(SensorType.Accelerometer);
      sm.UnregisterListener(this);
    }

    public void OnAccuracyChanged(Sensor sensor, int accuracy)
    {
    }

    public void OnSensorChanged(SensorEvent e)
    {
      string js = string.Empty;
      switch (e.Sensor.Type)
      {
        case SensorType.Accelerometer:
          js = string.Format(
            "javascript:accelerometer.onAccelerometerSuccess({0:0.00})",
              e.Values[0], e.Values[1], e.Values[2]);
          break;
      }
      if (js.Length > 0)
      {

      }
    }

    public IntPtr Handle
    {
      get { throw new NotImplementedException(); }
    }
  }
}