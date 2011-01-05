using Android.Widget;

namespace MonoDroid.Dialog
{
    public class ImageElement : Element
    {
        public ImageView Value;

        public ImageElement(ImageView image) : base("")
        {
            Value = image;
        }
    }
}