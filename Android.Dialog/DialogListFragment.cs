using System;
using Android.Support.V4.App;

namespace Android.Dialog
{
    public class DialogListFragment : ListFragment
    {
        public RootElement Root
        {
            get { return _dialogAdapter == null ? null : _dialogAdapter.Root; }
            set
            {
                value.Context = Activity;
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    _dialogAdapter.DeregisterListView();
                }

                ListAdapter = _dialogAdapter = new DialogAdapter(Activity, value, ListView);
            }
        }

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