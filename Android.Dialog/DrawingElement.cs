using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class DrawingElement : Element
    {
        Bitmap backgroundBitmap;
        Bitmap drawingBitmap;
        string fieldLabel;

        public DrawingElement(string fieldLabel, Bitmap backgroundBitmap, string drawingLocation)
            : base(string.Empty)
        {
            this.fieldLabel = fieldLabel;
            this.backgroundBitmap = backgroundBitmap;
            DrawingLocation = drawingLocation;

            Click = delegate
            {
                DrawImage();
            };
        }

        public int StrokeColor { get; set; }
        public string DrawingLocation { get; set; }

        private View MakeEmpty(Context context)
        {
            LayoutInflater inflater = LayoutInflater.FromContext(context);
            View curView = inflater.Inflate(Resource.Layout.drawing_element, null);

            return curView;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (backgroundBitmap != null)
                    backgroundBitmap.Dispose();
                if (drawingBitmap != null)
                    drawingBitmap.Dispose();
            }
            base.Dispose(disposing);
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView labelTV;
            public ImageView drawingIV;
        }

        public void InitializeView(View row)
        {
            row.Tag = new ViewHolder
            {
                labelTV = (TextView)row.FindViewById(Resource.Id.dialog_LabelField),
                drawingIV = (ImageView)row.FindViewById(Resource.Id.drawing_element_imageview),
            };
        }

        public void SetValues(View row)
        {
            TextView labelTV = ((ViewHolder)row.Tag).labelTV;
            ImageView drawingIV = ((ViewHolder)row.Tag).drawingIV;

            labelTV.SetText(fieldLabel, TextView.BufferType.Normal);

            /* TODO: should only be loaded when it is changed */
            drawingBitmap = ImageUtility.LoadImage(DrawingLocation);

            drawingIV.SetImageBitmap(drawingBitmap ?? backgroundBitmap);

            ViewHolder vh = (ViewHolder)row.Tag;
            vh.labelTV.SetText(fieldLabel, TextView.BufferType.Normal);
            drawingBitmap = ImageUtility.LoadImage(DrawingLocation);

            if (drawingBitmap != null)
            {
                vh.drawingIV.SetImageBitmap(drawingBitmap);
                drawingBitmap.Recycle();
                drawingBitmap = null;
            }
            else
            {
                vh.drawingIV.SetImageBitmap(backgroundBitmap);
            }
        }

        public override bool IsSelectable
        {
            get { return true; }
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            if (convertView != null && convertView.Id != LayoutId) convertView = null;
            View row = convertView;
            if (row == null)
            {
                row = MakeEmpty(context);
                InitializeView(row);
                SetValues(row);
            }
            else
            {
                SetValues(row);
            }


            return row;
        }

        public void DrawImage()
        {
            if (UseFragment && GetContext() is FragmentActivity)
            {
                var fragment = new DrawingFragment { Arguments = new Bundle(), };
                fragment.Arguments.PutString(DrawingFragment.DRAWING_LOCATION_INTENT, DrawingLocation);
                fragment.Arguments.PutInt(DrawingFragment.DRAWING_COLOR_INTENT, StrokeColor);
                fragment.Show(((FragmentActivity)GetContext()).SupportFragmentManager, fieldLabel);
                return;
            }
            ImageUtility.SaveImage(backgroundBitmap, DrawingFragment.BACKGROUND_FILE_PATH);
            Intent drawImageIntent = new Intent(GetContext(), typeof(DrawingActivity));
            drawImageIntent.PutExtra(DrawingFragment.DRAWING_LOCATION_INTENT, DrawingLocation);
            drawImageIntent.PutExtra(DrawingFragment.DRAWING_COLOR_INTENT, StrokeColor);
            GetContext().StartActivity(drawImageIntent);
        }

        public bool UseFragment { get; set; }
    }
}