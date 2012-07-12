using System;
using Microsoft.Phone.Tasks;

namespace DeviceAccess
{
  public class VideoPlayer
  {
    public void StartAudioPlayback(string AudioFilePath)
    {
      MediaPlayerLauncher objMediaPlayerLauncher = new MediaPlayerLauncher();
      objMediaPlayerLauncher.Media = new Uri(AudioFilePath, UriKind.Relative);
      objMediaPlayerLauncher.Location = MediaLocationType.Install;
      objMediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop | MediaPlaybackControls.All;
      objMediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
      objMediaPlayerLauncher.Show();
    }
  }
}
