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
            get { return _root; }
            set
            {
                _root = value;
                _root.ValueChanged += HandleValueChangedEvent;
                Adapter = _dialogAdapter = new DialogAdapter(Context, _root);
            }
        }
        private RootElement _root;
        private DialogAdapter _dialogAdapter;

        public DialogListView(Context context) :
            base(context, null)
        {
            Initialize();
        }

        public DialogListView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public DialogListView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        public event EventHandler ValueChanged;

        private void Initialize()
        {
            ItemClick += (sender, eventArgs) =>
            {
                var elem = _dialogAdapter.ElementAtIndex(eventArgs.Position);
                if (elem != null && elem.Click != null)
                    elem.Click(sender, eventArgs);
            };

            ItemLongClick += (sender, eventArgs) =>
            {
                var elem = _dialogAdapter.ElementAtIndex(eventArgs.Position);
                if (elem != null && elem.LongClick != null)
                    elem.LongClick(sender, eventArgs);
            };
        }

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}