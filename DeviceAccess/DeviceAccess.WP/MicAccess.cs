using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace DeviceAccess
{
  public class MicAccess
  {
    public void StartRecordingFroMic(string FilePath)
    {
      // close memory steam if left open
      if (_memoryStream != null)
      {
        _memoryStream.Close();
      }

      // start a new memory stream
      _memoryStream = new MemoryStream();

      // start recording
      _mic.Start();
    }

    public void StopRecording()
    {
      // stop recording
      if (_mic.State != MicrophoneState.Stopped)
      {
        _mic.Stop();
      }

      // reset memory buffer
      _memoryStream.Position = 0;
    }

    void ProcessingBuffer(object sender, EventArgs e)
    {

      // read in read values
      byte[] buffer = new byte[4096];
      int bytesRead = _mic.GetData(buffer, 0, buffer.Length);

      // write and read mic buffer contents
      while (bytesRead > 0)
      {
        _memoryStream.Write(buffer, 0, bytesRead);
        bytesRead = _mic.GetData(buffer, 0, buffer.Length);
      }
    }

    public MicAccess()
    {
      _mic.BufferReady += ProcessingBuffer;
    }
    MemoryStream _memoryStream;
    Microphone _mic = Microphone.Default;
  }
}
