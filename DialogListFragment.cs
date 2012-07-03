using System;
using Android.Support.V4.App;

namespace Android.Dialog
{
    public class DialogListFragment : ListFragment
    {
        public RootElement Root
        {
            get { return _root; }
            set
            {
                _root = value;
                _root.Context = Activity;
                _root.ValueChanged -= HandleValueChangedEvent;
                _root.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    ListView.ItemClick -= _dialogAdapter.ListView_ItemClick;
                    ListView.ItemLongClick -= _dialogAdapter.ListView_ItemLongClick;
                }

                ListAdapter = _dialogAdapter = new DialogAdapter(Activity, _root);
                ListView.ItemClick += _dialogAdapter.ListView_ItemClick;
                ListView.ItemLongClick += _dialogAdapter.ListView_ItemLongClick;
            }
        }

        private RootElement _root;
        private DialogAdapter _dialogAdapter;

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