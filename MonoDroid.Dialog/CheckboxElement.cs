using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class CheckboxElement : Element
    {
        private readonly Context _context;
        private CheckBox _cb;
        public bool Value;
        public string Group;

        public CheckboxElement(Context context, string caption)
            : base(caption)
        {
            _context = context;
        }

        public CheckboxElement(Context context, string caption, bool value)
            : this(context, caption)
        {
            Value = value;
        }

        public CheckboxElement(Context context, string caption, bool value, string group)
            : this(context, caption, value)
        {
            Group = group;
        }

        public override View GetView()
        {
            _cb = new CheckBox(_context) {Checked = Value, Text = Caption};
            return _cb;
        }

        public override void Selected()
        {
            Value = !Value;
            _cb.Checked = Value;
        }
    }
}