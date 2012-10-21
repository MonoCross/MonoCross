using System;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace Android.Dialog
{
    public class DialogListView : ListView
    {
        public RootElement Root
        {
            get { return DialogAdapter == null ? null : DialogAdapter.Root; }
            set
            {
                value.Context = Context;
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (DialogAdapter == null)
                    Adapter = DialogAdapter = new DialogAdapter(Context, value, this);
                else
                    DialogAdapter.Root = value;
            }
        }

        public DialogAdapter DialogAdapter { get; set; }

        public DialogListView(Context context) :
            base(context, null)
        {
        }

        public DialogListView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
        }

        public DialogListView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
        }

        public event EventHandler ValueChanged;

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            DialogAdapter.ReloadData();
        }
    }
}