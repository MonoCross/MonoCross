using Android.Views;
using Android.Media;

namespace DeviceAccess
{
  class VideoPlayer
  {
    public void StartVideoPlayback(SurfaceView surface, string FilePath)
    {
      if (_player != null)
      {
        StopVideoPlayback();
      }
      _player = new MediaPlayer();

      ISurfaceHolder holder = surface.Holder;
      holder.SetType(Android.Views.SurfaceType.PushBuffers);
      holder.SetFixedSize(400, 300);

      _player.SetDisplay(holder);
      _player.SetDataSource(FilePath);
      _player.Prepare();
      _player.Start();
    }

    public void StopVideoPlayback()
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
    MediaPlayer _player;
  }
}