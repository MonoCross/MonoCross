using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

using Microsoft.Devices;

namespace MonoCross.Utilities.Notification
{
    public static class Notify
    {
        public static void PlaySound(string uri)
        {
            Stream stream = TitleContainer.OpenStream(uri);
            SoundEffect effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();
        }

        public static void Vibrate()
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(1000));
        }
    }
}
