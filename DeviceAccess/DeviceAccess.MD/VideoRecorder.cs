using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Media;

namespace DeviceAccess
{

  public class RecordVideo
  {
    public void RecordVideoToPath(SurfaceView Sv, string VideoPath)
    {
      // setup and configure recorder
      _mediaRecorder = new MediaRecorder();

      // set the input source
      _mediaRecorder.SetAudioSource(Android.Media.AudioSource.Mic);
      _mediaRecorder.SetVideoSource(Android.Media.VideoSource.Camera);

      // set encoding values 
      _mediaRecorder.SetAudioEncoder(Android.Media.AudioEncoder.Default);
      _mediaRecorder.SetVideoEncoder(Android.Media.VideoEncoder.Default);

      // set the desirable preview display 
      _mediaRecorder.SetPreviewDisplay(Sv.Holder.Surface);

      // set output file locationa and format
      _mediaRecorder.SetOutputFormat(Android.Media.OutputFormat.Default);
      _mediaRecorder.SetOutputFile(VideoPath);

      _mediaRecorder.Prepare();

    }

    public void StopRecording()
    {
      if (_mediaRecorder != null)
      {
        _mediaRecorder.Stop();
        _mediaRecorder.Release();
        _mediaRecorder = null;
      }
    }

    MediaRecorder _mediaRecorder;
  }

  [Activity(Label = "Record Video")]
  public class RecordVideoActivity : Activity, ISurfaceHolderCallback
  {
    RecordVideo rv;
    SurfaceView sv;
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);
      //SetContentView(Resource.Layout.RecordVideo);
      //var btnStart = FindViewById<Button>(Resource.Id.StartVid);
      //btnStart.Click += HandleBtnStartClick;
      //var btnStop = FindViewById<Button>(Resource.Id.StopVid);
      //btnStop.Click += HandleBtnStopClick;
      //sv = FindViewById<SurfaceView>(Resource.Id.displayVid);

      SetContentView(null);
      sv = FindViewById<SurfaceView>(0);


      rv = new RecordVideo();
      rv.RecordVideoToPath(sv, "file.mp4");


      var holder = sv.Holder;
      holder.AddCallback(this);
      holder.SetType(Android.Views.SurfaceType.PushBuffers);
      holder.SetFixedSize(400, 300);
    }

    void HandleBtnStopClick(object sender, EventArgs e)
    {
      rv.StopRecording();
    }

    void HandleBtnStartClick(object sender, EventArgs e)
    {
      try
      {
        //rv.RecordVideoToPath(
      }
      catch (System.Exception sysExc)
      {
        Android.Util.Log.Error("Record Video", sysExc.Message);
      }
    }

    #region ISurfaceHolderCallback implementation
    void ISurfaceHolderCallback.SurfaceChanged(ISurfaceHolder holder, int format, int width, int height)
    {

    }

    void ISurfaceHolderCallback.SurfaceCreated(ISurfaceHolder holder)
    {

    }

    void ISurfaceHolderCallback.SurfaceDestroyed(ISurfaceHolder holder)
    {

    }
    #endregion
  }
}