using MonoTouch.AVFoundation;
using MonoTouch.CoreMedia;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreVideo;

namespace DeviceAccess
{
  class VideoRecorder
  {
    public void RecordVideoToPath(UIViewController ViewController, string VideoPath)
    {
      // setup capture device 
      AVCaptureDevice videoRecordingDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
      NSError error;
      AVCaptureDeviceInput videoInput = new AVCaptureDeviceInput(videoRecordingDevice, out error);

      // create and assign a capture session
      AVCaptureSession captureSession = new AVCaptureSession();
      captureSession.SessionPreset = AVCaptureSession.Preset1280x720;
      captureSession.AddInput(videoInput);

      // Create capture device output
      AVCaptureVideoDataOutput videoOutput = new AVCaptureVideoDataOutput();
      captureSession.AddOutput(videoOutput);
      videoOutput.VideoSettings.PixelFormat = CVPixelFormatType.CV32BGRA;
      videoOutput.MinFrameDuration = new CMTime(1, 30);
      videoOutput.SetSampleBufferDelegatequeue(captureVideoDelegate, System.IntPtr.Zero);

      // create a delegate class for handling capture
      captureVideoDelegate = new CaptureVideoDelegate(ViewController);

      // Start capture session
      captureSession.StartRunning();
    }
    CaptureVideoDelegate captureVideoDelegate;

    public class CaptureVideoDelegate : AVCaptureVideoDataOutputSampleBufferDelegate
    {
      private UIViewController _viewController;

      public CaptureVideoDelegate(UIViewController viewController)
      {
        _viewController = viewController;
      }

      public override void DidOutputSampleBuffer(AVCaptureOutput output, CMSampleBuffer buffer, AVCaptureConnection con)
      {
        //  Implement
        //  - see: http://go-mono.com/docs/index.aspx?link=T%3aMonoTouch.Foundation.ModelAttribute
        //
      }
    }
  }
}
