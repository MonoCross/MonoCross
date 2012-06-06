using System;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace Android.Dialog
{
    public class EntryElement : Element, ITextWatcher
    {
        public string Value
        {
            get { return _val; }
            set
            {
                if (_entry != null && _val != value)
                {
                    if (_entry.Text != value)
                        _entry.Text = value;
                    if (ValueChanged != null)
                        ValueChanged(this, EventArgs.Empty);
                }
                _val = value;
            }
        }

        public event EventHandler ValueChanged;

        public EntryElement(string caption, string value)
            : this(caption, value, (int)DroidResources.ElementLayout.dialog_textfieldright)
        {
        }

        public EntryElement(string caption, string hint, string value)
            : this(caption, value)
        {
            Hint = hint;
        }

        public EntryElement(string caption, string value, int layoutId)
            : base(caption, layoutId)
        {
            _val = value;
            Lines = 1;
        }

        public override string Summary()
        {
            return _val;
        }

        public bool Password { get; set; }
        public bool IsEmail { get; set; }
        public bool Numeric { get; set; }
        public string Hint { get; set; }
        public int Lines { get; set; }

        /// <summary>
        /// An action to perform when Enter is hit
        /// </summary>
        /// <remarks>This is only meant to be set if this is the last field in your RootElement, to allow the Enter button to be used for submitting the form data.<br>
        /// If you want to perform an action when the text changes, consider hooking into ValueChanged instead.</remarks>
        public Action Send { get; set; }

        protected EditText _entry;
        private string _val;

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            TextView label;
            var view = DroidResources.LoadStringEntryLayout(context, convertView, parent, LayoutId, out label, out _entry);
            if (view != null)
            {
                // Warning! Crazy ass hack ahead!
                // since we can't know when out convertedView was was swapped from inside us, we store the
                // old textwatcher in the tag element so it can be removed!!!! (barf, rech, yucky!)
                if (_entry.Tag != null)
                    _entry.RemoveTextChangedListener((ITextWatcher)_entry.Tag);

                _entry.Text = Value;
                _entry.Hint = Hint;
                //_entry.EditorAction += new EventHandler<TextView.EditorActionEventArgs>(_entry_EditorAction);
                _entry.ImeOptions = ImeAction.Unspecified;

                if (Numeric)
                    _entry.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned;
                else if (IsEmail)
                    _entry.InputType = InputTypes.TextVariationEmailAddress | InputTypes.ClassText;
                else
                    _entry.InputType = InputTypes.ClassText;

                if (Password)
                    _entry.InputType |= InputTypes.TextVariationPassword;

                if (Lines > 1)
                {
                    _entry.InputType |= InputTypes.TextFlagMultiLine;
                    _entry.SetLines(Lines);
                }
                else if (Send != null)
                {
                    _entry.ImeOptions = ImeAction.Go;
                    _entry.SetImeActionLabel("Go", ImeAction.Go);
                    _entry.EditorAction += _entry_EditorAction;
                }

                // continuation of crazy ass hack, stash away the listener value so we can look it up later
                _entry.Tag = this;
                _entry.AddTextChangedListener(this);
                if (label == null)
                {
                    _entry.Hint = Caption;
                }
                else
                {
                    label.Text = Caption;
                }
            }

            return view;
        }

        protected void _entry_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == ImeAction.Go)
            {
                Send();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_entry.Dispose();
                _entry = null;
            }
        }

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 || base.Matches(text);
        }

        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            Value = s.ToString();
        }

        public void AfterTextChanged(IEditable s)
        {
            // nothing needed
        }

        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
            // nothing needed
        }
    }
}