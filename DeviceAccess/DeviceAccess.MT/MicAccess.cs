using System;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
using MonoTouch.AudioToolbox;
using System.IO;

namespace DeviceAccess
{
  class MicAccess
  {
    public MicAccess()
    {
      _mic = new AVAudioRecorder();
    }

    public void StartRecordingFroMic(string FilePath)
    {
      // set some default values for recording settings
      NSObject[] keys = new NSObject[]
            {
                AVAudioSettings.AVSampleRateKey,
                AVAudioSettings.AVFormatIDKey,
                AVAudioSettings.AVNumberOfChannelsKey,
                AVAudioSettings.AVEncoderAudioQualityKey,
            };
      NSObject[] values = new NSObject[]
            {    
                NSNumber.FromFloat(44100.0f),
                NSNumber.FromInt32((int)AudioFileType.WAVE),
                NSNumber.FromInt32(1),
                NSNumber.FromInt32((int)AVAudioQuality.Max),
            };
      NSDictionary settings = NSDictionary.FromObjectsAndKeys(values, keys);

      // define a filename and location for the output file
      string fileName = FilePath;
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
      outputFileUrl = NSUrl.FromFilename(path);

      // pass the configured url to the AVAudioRecorder object 
      NSError error = new NSError();
      _mic = AVAudioRecorder.ToUrl(outputFileUrl, settings, out error);

      if (error != null)
      {
        // prepare and start recording
        _mic.PrepareToRecord();
        _mic.Record();
      }
      else
      {
        throw new Exception("Error loading mic: " + error.ToString());
      }
    }

    public void StopRecording()
    {
      _mic.Stop();

      // prepare object for GC by calling dispose
      _mic.FinishedRecording += delegate
      {
        _mic.Dispose();
        _mic = null;
      };
    }

    AVAudioRecorder _mic;
    private NSUrl outputFileUrl;
  }
}
