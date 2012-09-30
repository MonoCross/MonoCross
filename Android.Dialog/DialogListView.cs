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
                _root.Context = Context;
                _root.ValueChanged -= HandleValueChangedEvent;
                _root.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    ItemClick -= _dialogAdapter.ListView_ItemClick;
                    ItemLongClick -= _dialogAdapter.ListView_ItemLongClick;
                }

                Adapter = _dialogAdapter = new DialogAdapter(Context, _root);
                ItemClick += _dialogAdapter.ListView_ItemClick;
                ItemLongClick += _dialogAdapter.ListView_ItemLongClick;
            }
        }
        private RootElement _root;
        private DialogAdapter _dialogAdapter;

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
            _dialogAdapter.ReloadData();
        }
    }
}