using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace DeviceAccess
{
  public class VideoAccess
  {
    // Source and device for capturing video.
    CaptureSource captureSource;
    VideoCaptureDevice videoCaptureDevice;

    // File details for storing the recording.        
    IsolatedStorageFileStream isoVideoFile;
    FileSink fileSink;

    // Viewfinder for capturing video.
    VideoBrush videoRecorderBrush;

    public void StartRecording(Rectangle viewfinderRectangle, string filePath)
    {
      InitializeVideoRecorder(viewfinderRectangle);

      // Connect fileSink to captureSource.
      if (captureSource.VideoCaptureDevice != null
          && captureSource.State == CaptureState.Started)
      {
        captureSource.Stop();

        // Connect the input and output of fileSink.
        fileSink.CaptureSource = captureSource;
        fileSink.IsolatedStorageFileName = filePath;
      }

      // Begin recording.
      if (captureSource.VideoCaptureDevice != null
          && captureSource.State == CaptureState.Stopped)
      {
        captureSource.Start();
      }
    }

    public void StopRecording()
    {
      if (captureSource != null)
      {
        // Stop captureSource if it is running.
        if (captureSource.VideoCaptureDevice != null
            && captureSource.State == CaptureState.Started)
        {
          captureSource.Stop();
        }

        // Remove the event handlers for capturesource and the shutter button.
        captureSource.CaptureFailed -= OnCaptureFailed;

        // Remove the video recording objects.
        captureSource = null;
        videoCaptureDevice = null;
        fileSink = null;
        videoRecorderBrush = null;
      }
    }

    void InitializeVideoRecorder(Rectangle viewfinderRectangle)
    {
      if (captureSource == null)
      {
        // Create the VideoRecorder objects.
        captureSource = new CaptureSource();
        fileSink = new FileSink();

        videoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();

        // Add eventhandlers for captureSource.
        captureSource.CaptureFailed += new EventHandler<ExceptionRoutedEventArgs>(OnCaptureFailed);

        // Initialize the camera if it exists on the device.
        if (videoCaptureDevice != null)
        {
          // Create the VideoBrush for the viewfinder.
          videoRecorderBrush = new VideoBrush();
          videoRecorderBrush.SetSource(captureSource);

          // Display the viewfinder image on the rectangle.
          viewfinderRectangle.Fill = videoRecorderBrush;

          // Start video capture and display it on the viewfinder.
          captureSource.Start();
        }
        else
        {
          // A camera is not supported on this device
        }
      }
    }

    void OnCaptureFailed(object sender, ExceptionRoutedEventArgs e)
    {
    }
  }
}
