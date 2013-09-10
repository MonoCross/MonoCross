using System;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace Android.Dialog
{
    public class DialogListView : ListView, IDialogView
    {
        public RootElement Root
        {
            get { return DialogAdapter == null ? null : DialogAdapter.Root; }
            set
            {
                value.ValueChanged += HandleValueChangedEvent;
                if (Root == null) DialogAdapter = new DialogAdapter(Context, value, this);
                else
                {
                    Root.ValueChanged -= HandleValueChangedEvent;
                    value.Context = Context;
                    DialogAdapter.Root = value;
                }
            }
        }

        public DialogAdapter DialogAdapter
        {
            get { return Adapter as DialogAdapter; }
            set { Adapter = value; }
        }

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

        protected void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }
    }
}