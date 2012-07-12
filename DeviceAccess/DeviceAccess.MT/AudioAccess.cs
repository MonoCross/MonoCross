using System;
using MonoTouch.Foundation;
using MonoTouch.AVFoundation;

namespace DeviceAccess
{
  public class AudioAccess
  {
    public void StartAudioPlayback(string AudioFilePath)
    {
      NSError error = null;
      NSUrl audioFileUrl = NSUrl.FromFilename(AudioFilePath);
      _player = AVAudioPlayer.FromUrl(audioFileUrl, out error);
      if (_player != null)
      {
        _player.PrepareToPlay();
        _player.Play();
      }
      else
      {
        throw new Exception("Could not load Accelerometer sensor");
      }
    }

    public void StopAudioPlayback()
    {
      if (_player != null)
      {
        _player.Stop();
        _player.Dispose();
        _player = null;
      }
    }

    AVAudioPlayer Player
    {
      get
      {
        return _player;
      }
      set
      {
        _player = value;
      }
    }
    AVAudioPlayer _player;
  }
}
