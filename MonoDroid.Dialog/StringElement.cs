using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class StringElement : Element
    {
        public string Value;
        private Context _context;
        private TextView _text;
        private TextView _caption;

        public StringElement(Context context, string caption) : base(caption)
        {
            _context = context;
        }

        public StringElement(Context context, string caption, string value)
            : base(caption)
        {
            _context = context;
            this.Value = value;
        }

        public StringElement(string caption, Action tapped)
            : base(caption)
        {
            Tapped += tapped;
        }

        public event Action Tapped;

        public override View GetView()
        {
            var view = new LinearLayout(_context) {Orientation = Orientation.Horizontal};

            _caption = new TextView(_context) { Text = Caption };
            view.AddView(_caption);

            if (string.IsNullOrEmpty(Value))
            {
                _text = new TextView(_context) { Text = Value };
                view.AddView(_text);
            }

            return view;
        }

        public override string Summary()
        {
            return Caption;
        }

        public override void Selected()
        {
            if (Tapped != null)
                Tapped();
        }

        public override bool Matches(string text)
        {
            return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) || base.Matches(text);
        }
    }
}