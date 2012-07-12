using Android.Media;

namespace DeviceAccess
{
  class MicAccess
  {
    public void StartRecordingFroMic(string FileName)
    {
      // set some default values for recording settings
      _mic.SetAudioSource(AudioSource.Mic);
      _mic.SetOutputFormat(OutputFormat.Default);
      _mic.SetAudioEncoder(AudioEncoder.Default);

      // define a filename and location for the output file
      _mic.SetOutputFile(FileName);

      // prepare and start recording
      _mic.Prepare();
      _mic.Start();
    }

    public void StopRecording()
    {
      // stop recording
      _mic.Stop();

      // prepare object for GC by calling dispose
      _mic.Release();
      _mic = null;
    }

    public MicAccess()
    {
      _mic = new MediaRecorder();
    }
    MediaRecorder _mic;
  }
}