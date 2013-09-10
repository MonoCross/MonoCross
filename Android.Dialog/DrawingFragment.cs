using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace Android.Dialog
{
    public class DrawingFragment : DialogFragment, IDialogInterfaceOnClickListener, IDialogInterface
    {
        public static readonly string DRAWING_LOCATION_INTENT = "DrawingLocation";
        public static readonly string DRAWING_COLOR_INTENT = "StrokeColor";
        public static string BACKGROUND_FILE_PATH = Environment.ExternalStorageDirectory + File.Separator + "drawing_image_reserved_location.png";
        private DrawingView _signatureDrawingView;
        private string drawingLocation;

        public override void OnCreate(Bundle savedInstanceState)
        {
            drawingLocation = Arguments.GetString(DRAWING_LOCATION_INTENT);
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.drawing_field, null);
            _signatureDrawingView = view.FindViewById<DrawingView>(Resource.Id.drawingfield_drawingview);
            _signatureDrawingView.StrokeColor = new Color(Arguments.GetInt(DRAWING_COLOR_INTENT));

            Button saveButton = view.FindViewById<Button>(Resource.Id.drawingfield_save);
            Button clearButton = view.FindViewById<Button>(Resource.Id.drawingfield_clear);

            saveButton.Click += delegate
            {
                OnClick(this, 0);
            };

            clearButton.Click += delegate
            {
                OnClick(this, 1);
            };

            return view;
        }

        //public override App.Dialog OnCreateDialog(Bundle savedInstanceState)
        //{
        //    LayoutInflater factory = LayoutInflater.From(Activity);
        //    View textEntryView = OnCreateView(factory, null, null);

        //    return new AlertDialog.Builder(Activity)
        //            .SetView(textEntryView)
        //            .SetPositiveButton(Android.Resource.String.Ok, this)
        //            .SetNegativeButton(Android.Resource.String.Cancel, this)
        //            .Create();
        //}

        public void OnClick(IDialogInterface dialog, int which)
        {
            switch (which)
            {
                case 0:
                    _signatureDrawingView.SaveImage(drawingLocation);
                    if (ShowsDialog) Dismiss();
                    else Activity.Finish();
                    break;
                default:
                    _signatureDrawingView.ClearImage();
                    break;
            }
        }

        public void Cancel()
        {
            _signatureDrawingView.ClearImage();
        }
    }
}