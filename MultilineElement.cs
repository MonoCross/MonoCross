using Android.Content;
using Android.Views;

namespace Android.Dialog
{
    public class MultilineEntryElement : EntryElement
    {
        public int MaxLength { get; set; }
        public MultilineEntryElement(string caption, string value)
            : base(caption, value, (int)DroidResources.ElementLayout.dialog_textfieldbelow)
        {
            Lines = 3;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            var view = base.GetView(context, convertView, parent);
            if (_entry != null)
            {
                _entry.TextChanged += delegate
                {
                    if (MaxLength > 0 && _entry.Text.Length > MaxLength)
                        _entry.Text = _entry.Text.Substring(0, MaxLength);
                };
            }
            return view;
        }
    }
}