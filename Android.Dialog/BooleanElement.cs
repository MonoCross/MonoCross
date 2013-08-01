using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public abstract class BoolElement : Element
    {
        private bool _val;

        public string TextOff { get; set; }
        public string TextOn { get; set; }

        public bool Value
        {
            get { return _val; }
            set
            {
                if (_val != value)
                {
                    _val = value;
                    if (Changed != null)
                        Changed(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler Changed;

        public BoolElement(string caption, bool value)
            : base(caption)
        {
            _val = value;
        }

        public BoolElement(string caption, bool value, int layoutId)
            : base(caption, layoutId)
        {
            _val = value;
        }

        public override string Summary()
        {
            return _val ? TextOn : TextOff;
        }
    }

    /// <summary>
    /// Used to display toggle button on the screen.
    /// </summary>
    public class BooleanElement : BoolElement, CompoundButton.IOnCheckedChangeListener
    {
        protected ToggleButton _toggleButton;
        protected TextView _caption;
        protected TextView _subCaption;

        public BooleanElement(string caption, bool value)
            : base(caption, value, Resource.Layout.dialog_onofffieldright)
        {
        }

        public BooleanElement(string caption, bool value, int layoutId)
            : base(caption, value, layoutId)
        {
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            View toggleButtonView;
            View view = DroidResources.LoadBooleanElementLayout(context, convertView, parent, LayoutId, out _caption, out _subCaption, out toggleButtonView);

            if (view != null)
            {
                _caption.Text = Caption;
                _toggleButton = (ToggleButton)toggleButtonView;
                _toggleButton.SetOnCheckedChangeListener(null);
                _toggleButton.Checked = Value;
                _toggleButton.SetOnCheckedChangeListener(this);

                if (TextOff != null)
                {
                    _toggleButton.TextOff = TextOff;
                    if (!Value)
                        _toggleButton.Text = TextOff;
                }

                if (TextOn != null)
                {
                    _toggleButton.TextOn = TextOn;
                    if (Value)
                        _toggleButton.Text = TextOn;
                }
            }
            return view;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            //_toggleButton.Dispose();
            _toggleButton = null;
            //_caption.Dispose();
            _caption = null;
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            Value = isChecked;
        }

        public override void Selected()
        {
            if (_toggleButton != null)
                _toggleButton.Toggle();
        }
    }
}