using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class StringElement : Element
    {
        private readonly Context _context;
        public string Value;
        private TextView _caption;
        private TextView _text;

        public StringElement(Context context, string caption) : base(caption)
        {
            _context = context;
        }

        public StringElement(Context context, string caption, string value)
            : base(caption)
        {
            _context = context;
            Value = value;
        }

        public StringElement(string caption, Action tapped)
            : base(caption)
        {
            Tapped += tapped;
        }

        public event Action Tapped;

        public override View GetView()
        {
            var view = new RelativeLayout(_context);

            var parms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                        ViewGroup.LayoutParams.WrapContent);
            parms.SetMargins(5, 3, 5, 0);
            parms.AddRule((int) LayoutRules.AlignParentLeft);

            _caption = new TextView(_context) {Text = Caption, TextSize = 16f};
            view.AddView(_caption, parms);

            if (!string.IsNullOrEmpty(Value))
            {
                var tparms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                             ViewGroup.LayoutParams.WrapContent);
                tparms.SetMargins(5, 3, 5, 0);
                tparms.AddRule((int) LayoutRules.AlignParentRight);

                _text = new TextView(_context) {Text = Value, TextSize = 16f};
                view.AddView(_text, tparms);
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
            return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) ||
                   base.Matches(text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _caption.Dispose();
                _caption = null;
                _text.Dispose();
                _text = null;
            }
        }
    }
}