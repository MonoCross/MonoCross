using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class CheckboxElement : Element, CompoundButton.IOnCheckedChangeListener
    {
        public bool Value
        {
            get { return _val; }
            set
            {
                bool emit = _val != value;
                _val = value;
                if (_checkbox != null && _checkbox.Checked != _val)
                    _checkbox.Checked = _val;
                else if (emit && Changed != null)
                    Changed(this, EventArgs.Empty);
            }
        }
        private bool _val;

        public string SubCaption { get; set; }

        public bool ReadOnly
        {
            get;
            set;
        }

        public event EventHandler Changed;

        private CheckBox _checkbox;
        private TextView _caption;
        private TextView _subCaption;

        public string Group;

        public CheckboxElement(string caption)
            : base(caption, Resource.Layout.dialog_boolfieldright)
        {

        }

        public CheckboxElement(string caption, bool value)
            : base(caption, Resource.Layout.dialog_boolfieldright)
        {
            Value = value;
        }

        public CheckboxElement(string caption, bool value, string subCaption, string group)
            : base(caption, Resource.Layout.dialog_boolfieldsubright)
        {
            Value = value;
            Group = group;
            SubCaption = subCaption;
        }

        public CheckboxElement(string caption, bool value, string group)
            : base(caption, Resource.Layout.dialog_boolfieldright)
        {
            Value = value;
            Group = group;
        }

        public CheckboxElement(string caption, bool value, string group, int layoutId)
            : base(caption, layoutId)
        {
            Value = value;
            Group = group;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            View checkboxView;
            View view = DroidResources.LoadBooleanElementLayout(context, convertView, parent, LayoutId, out _caption, out _subCaption, out checkboxView);
            if (view != null)
            {
                _caption.Text = Caption;

                _checkbox = (CheckBox)checkboxView;
                _checkbox.SetOnCheckedChangeListener(null);
                _checkbox.Checked = Value;
                _checkbox.SetOnCheckedChangeListener(this);
                _checkbox.Clickable = !ReadOnly;

                if (_subCaption != null)
                {
                    _subCaption.Text = SubCaption;
                }
            }
            return view;
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            Value = isChecked;
        }

        public override void Selected()
        {
            if (!ReadOnly)
                _checkbox.Toggle();
        }

        public override string Summary()
        {
            return Value ? "On" : "Off"; //Because iOS, that's why.
        }
    }
}