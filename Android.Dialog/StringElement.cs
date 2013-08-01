using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class StringElement : Element
    {
        public int FontSize { get; set; }
        public string Value
        {
            get { return _value; }
            set { _value = value; if (_text != null) _text.Text = _value; }
        }
        private string _value;

        public object Alignment;

        public StringElement(string caption)
            : base(caption, Resource.Layout.dialog_multiline_labelfieldbelow)
        {
        }

        public StringElement(string caption, int layoutId)
            : base(caption, layoutId)
        {
        }

        public StringElement(string caption, string value)
            : base(caption, Resource.Layout.dialog_multiline_labelfieldbelow)
        {
            Value = value;
        }

        public StringElement(string caption, string value, int layoutId)
            : base(caption, layoutId)
        {
            Value = value;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            var view = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _text);
            if (view != null && _caption != null && _text != null)
            {
                _caption.Text = Caption;
                _caption.Visibility = Caption == null ? ViewStates.Gone : ViewStates.Visible;
                _text.Text = Value;
                if (FontSize > 0)
                {
                    _caption.TextSize = FontSize;
                    _text.TextSize = FontSize;
                }
            }
            return view;
        }

        public override string Summary()
        {
            return Value;
        }

        public override bool IsSelectable
        {
            get { return true; }
        }

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 || base.Matches(text);
        }

        protected TextView _caption;
        protected TextView _text;

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            //_caption.Dispose();
            _caption = null;
            //_text.Dispose();
            _text = null;
        }
    }

    #region Compatibility classes
    public class MultilineElement : StringElement
    {
        public MultilineElement(string caption) : base(caption) { }
        public MultilineElement(string caption, int layoutId) : base(caption, layoutId) { }
        public MultilineElement(string caption, string value) : base(caption, value) { }
        public MultilineElement(string caption, string value, int layoutId) : base(caption, value, layoutId) { }
    }

    public class StringMultilineElement : StringElement
    {
        public StringMultilineElement(string caption) : base(caption) { }
        public StringMultilineElement(string caption, int layoutId) : base(caption, layoutId) { }
        public StringMultilineElement(string caption, string value) : base(caption, value) { }
        public StringMultilineElement(string caption, string value, int layoutId) : base(caption, value, layoutId) { }
    }

    public class StyledMultilineElement : StringElement
    {
        public StyledMultilineElement(string caption) : base(caption) { }
        public StyledMultilineElement(string caption, int layoutId) : base(caption, layoutId) { }
        public StyledMultilineElement(string caption, string value) : base(caption, value) { }
        public StyledMultilineElement(string caption, string value, int layoutId) : base(caption, value, layoutId) { }
    }
    #endregion
}