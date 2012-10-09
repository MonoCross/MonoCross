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

                if (_dialogAdapter == null)
                    _dialogAdapter = new DialogAdapter(Activity, value);
                else
                    _dialogAdapter.Root = value;

            }
        }

        public override Views.View OnCreateView(Views.LayoutInflater p0, Views.ViewGroup p1, OS.Bundle p2)
        {
            ListAdapter = _dialogAdapter;
            return base.OnCreateView(p0, p1, p2);
        }

        public override void OnViewCreated(Views.View p0, OS.Bundle p1)
        {
            if (_dialogAdapter == null) return;
            _dialogAdapter.List = ListView;
            _dialogAdapter.RegisterListView();
            base.OnViewCreated(p0, p1);
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