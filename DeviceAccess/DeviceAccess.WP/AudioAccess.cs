using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DeviceAccess
{
  public static class AudioAccess
  {
    public static void StartAudioPlayback(string AudioFilePath)
    {
      Stream stream = TitleContainer.OpenStream(AudioFilePath);
      SoundEffect effect = SoundEffect.FromStream(stream);
      FrameworkDispatcher.Update();
      effect.Play();
    }
  }
}
