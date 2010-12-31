using Android.Content;
using Android.Util;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class DialogView : LinearLayout
    {
        public DialogView(Context context, Element element) : base(context)
        {
            Initialize(element);
        }

        public DialogView(Context context, IAttributeSet attrs, Element element) :
            base(context, attrs)
        {
            Initialize(element);
        }

        private void Initialize(Element element)
        {
            SetMinimumHeight(44);
            AddView(element.GetView());
        }
    }
}