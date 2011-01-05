using Android.Content;

namespace MonoDroid.Dialog
{
    public class MultilineElement : StringElement, IElementSizing
    {
        public MultilineElement(Context context, string caption, string value) : base(context, caption)
        {
        }

        public float GetHeight()
        {
            return 0.0f;
        }
    }
}