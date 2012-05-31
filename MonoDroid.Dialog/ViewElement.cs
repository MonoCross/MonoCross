using System;
using Android.Content;
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
            var view = LayoutInflater.FromContext(context).Inflate(LayoutId, parent, false);
            if (Populate != null)
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