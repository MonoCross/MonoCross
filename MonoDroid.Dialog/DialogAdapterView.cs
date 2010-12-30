using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace MonoDroid.Dialog
{
   public class DialogAdapterView : LinearLayout
    {
        public DialogAdapterView(Context context, Element element) : base(context)
        {
            Initialize(element);
        }

        public DialogAdapterView(Context context, IAttributeSet attrs, Element element) :
            base(context, attrs)
        {
            Initialize(element);
        }

        private void Initialize(Element element)
        {
            this.Orientation = Orientation.Horizontal;
            this.AddView(element.GetView());
        }
    }
}
