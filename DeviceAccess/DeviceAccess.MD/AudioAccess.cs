using System;
using System.IO;
using Android.Media;

namespace DeviceAccess
{
  public class AudioAccess
  {
    public void StartAudioPlayback(string AudioFilePath)
    {
      // first stop any active audio on MediaPlayer instance
      if (_player != null) { StopAudioPlayback(); }

      _player = new MediaPlayer();
      if (_player != null)
      {
        _player.SetDataSource(AudioFilePath);
        _player.Prepare();
        _player.Start();
      }
      else
      {
        throw new Exception("Could not load MediaPlayer");
      }
    }

    public void StopAudioPlayback()
    {
      if (_player != null)
      {
        if (_player.IsPlaying)
        {
          _player.Stop();
        }

        _player.Release();
        _player = null;
      }
    }

    public void MultiMediaPlayer(String path, String fileName)
    {
      //set up MediaPlayer    
      MediaPlayer mp = new MediaPlayer();

      try
      {
        mp.SetDataSource(Path.Combine(path, fileName));
      }
      catch (Exception e)
      {
        Console.WriteLine("");
      }
      try
      {
        mp.Prepare();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      mp.Start();
    }

    MediaPlayer _player;
  }
}
