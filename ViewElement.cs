using System;
using System.Globalization;
using Android.Content;
using Android.Util;
using Android.Views;

namespace Android.Dialog
{
    public class ViewElement : Element
    {
        public ViewElement(int layout)
            : base(string.Empty, layout)
        {

        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            var view = convertView ?? LayoutInflater.FromContext(context).Inflate(LayoutId, parent, false);
            if (view == null)
            {
                Log.Error("Android.Dialog", "ViewElement: Failed to load resource: " + LayoutId.ToString(CultureInfo.InvariantCulture));
            }
            else if (Populate != null)
                Populate(view);
            return view;
        }

        /// <summary>
        /// Gets or sets the <see cref="Action{T}"/> that populates the <see cref="View"/> that was inflated from the Layout ID passed in on the constructor.
        /// </summary>
        /// <value>
        /// The <see cref="Action{T}"/> that hydrates the inflated View with data.
        /// </value>
        public Action<View> Populate { get; set; }
    }
}