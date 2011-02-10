using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class DialogView : LinearLayout
    {
        public DialogView(Context context, Element element) : base(context)
        {
			Initialize(context, element);
        }

        public DialogView(Context context, IAttributeSet attrs, Element element) :
            base(context, attrs)
        {
			Initialize(context, element);
        }

        private void Initialize(Context context, Element element)
        {
            AddView(element.GetView(context));
        }
    }
}