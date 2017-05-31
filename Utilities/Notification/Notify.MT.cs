using System;
using MonoTouch.Foundation;
using MonoTouch.AudioToolbox;

namespace MonoCross.Utilities.Notification
{
    public static class Notify
    {
        static Notify()
        {
            // Setup your session  
            AudioSession.Initialize();
            AudioSession.Category = AudioSessionCategory.MediaPlayback;
            AudioSession.SetActive(true);
        }

        public static void PlaySound(string uri)
        {
            // Play the file
            var sound = SystemSound.FromFile(uri);
            sound.PlaySystemSound();
        }

        public static void Vibrate()
        {
            SystemSound.Vibrate.PlaySystemSound();
        }
    }
}
