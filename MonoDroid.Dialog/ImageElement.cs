using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class ImageElement : Element
    {
        // Height for rows
        const int dimx = 48;
        const int dimy = 44;

        // radius for rounding
        const int roundPx = 12;

        public readonly ImageView Value;
        ImageView scaled;

        public ImageElement(ImageView image)
            : base(string.Empty)
        {
            if (image == null)
            {
                Value = MakeEmpty();
                scaled = Value;
            }
            else
            {
                Value = image;
                if (image.Drawable != null)
                    scaled = Scale(Value);
            }
        }

        static ImageView MakeEmpty()
        {
            return new ImageView(null);
        }

        ImageView Scale(ImageView source)
        {
            var drawable = (BitmapDrawable)source.Drawable;
            var bitmap = drawable.Bitmap;
            var bMapScaled = Bitmap.CreateScaledBitmap(bitmap, dimx, dimy, true);
            source.SetImageBitmap(bMapScaled);
            return source;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (scaled != null)
                    scaled.Dispose();
                if (Value != null)
                    Value.Dispose();
            }
            base.Dispose(disposing);
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            if (scaled == null)
                scaled = Scale(Value);

            Click = delegate { SelectImage(); };

            var view = convertView as RelativeLayout ?? new RelativeLayout(context);

            var parms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                        ViewGroup.LayoutParams.WrapContent);
            parms.SetMargins(5, 2, 5, 2);
            parms.AddRule(LayoutRules.AlignParentLeft);

            // SEC bug fix, not yet submitted to Kenny.  Getting exception "specified view already has a parent"
            if (scaled.Parent != view)
                view.AddView(scaled, parms);

            return view;
        }

        private void SelectImage()
        {
            var activity = (Activity)GetContext();
            var intent = new Intent(Intent.ActionPick, Provider.MediaStore.Images.Media.InternalContentUri);
            activity.StartActivity(intent);
        }
    }
}