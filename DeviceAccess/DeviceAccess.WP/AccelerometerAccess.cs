using System;
using System.Threading;
using Microsoft.Devices.Sensors;

namespace DeviceAccess
{
  public class AccelerometerAccess
  {
    Accelerometer _accelerometer = new Accelerometer();

    public string GetAccelerometerXyz()
    {
      if (!Accelerometer.IsSupported)
      {
        throw new Exception("Not supported on this device");
      }
      try
      {
        // Start accelerometer for detecting compass axis
        _accelerometer = new Accelerometer();
        _accelerometer.CurrentValueChanged +=
          new EventHandler<SensorReadingEventArgs<AccelerometerReading>>
                                      (_accelerometer_CurrentValueChanged);
        _accelerometer.Start();
      }
      catch (InvalidOperationException e)
      {
        throw new Exception("Error starting accelerometer", e);
      }

      string xyz = null;
      if (_accelerometer != null)
      {
        // block on the search until the async result return
        lock (_locker)
        {
          while (_xyz == null)
          {
            Monitor.Pulse(_locker);
          }

          xyz = _xyz;
          _xyz = null;
        }

      }
      return xyz;
    }
    static readonly object _locker = new object();
    string _xyz = null;

    private void _accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
    {
      lock (_locker)
      {
        _xyz = string.Format("{0:0.00}, {1:0.00}, {2:0.00}", e.SensorReading.Acceleration.X,
                                                                         e.SensorReading.Acceleration.Y,
                                                                            e.SensorReading.Acceleration.Z);
        Monitor.Pulse(_locker);
      }
    }

    public void AccelerometerCancel()
    {
      if (_accelerometer != null)
      {
        _accelerometer.Stop();
        _accelerometer.Dispose();
        _accelerometer = null;
      }
    }
  }
}
