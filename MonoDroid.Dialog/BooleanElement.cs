using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public abstract class BoolElement : Element
    {
        private bool val;

        public bool Value
        {
            get { return val; }
            set
            {
                bool emit = val != value;
                val = value;
                if (emit && ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler ValueChanged;

        public BoolElement(string caption, bool value)
            : base(caption)
        {
            val = value;
        }

        public override string Summary()
        {
            return val ? "On" : "Off";
        }
    }

    /// <summary>
    /// Used to display switch on the screen.
    /// </summary>
    public class BooleanElement : BoolElement
    {
        readonly Context _context;
        static string bkey = "BooleanElement";
        ToggleButton sw;
        TextView tv;

        public BooleanElement(Context context, string caption, bool value)
            : base(caption, value)
        {
            _context = context;
        }

        public BooleanElement(Context context, string caption, bool value, string key)
            : this(context, caption, value)
        { }

        public override View GetView()
        {
            var view = new LinearLayout(_context)
                           {
                               Orientation = Orientation.Horizontal,
                               LayoutParameters =
                                   new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                              ViewGroup.LayoutParams.WrapContent)
                           };

            sw = new ToggleButton(_context) {Tag = 1, Checked = true};
            //sw.SetBackgroundColor(Android.Graphics.Color.Transparent);
            view.AddView(sw);

            tv = new TextView(_context) {Text = Caption};
            view.AddView(tv);

            return view;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sw.Dispose();
                sw = null;
                tv.Dispose();
                tv = null;
            }
        }
    }
}