using System;
using Android.App;

namespace Android.Dialog
{
    public class DialogActivity : ListActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            ReloadData(); 
        }

        public RootElement Root
        {
            get { return _dialogAdapter == null ? null : _dialogAdapter.Root; }
            set
            {
                value.Context = this;
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    _dialogAdapter.DeregisterListView();
                }

                ListAdapter = _dialogAdapter = new DialogAdapter(this, value, ListView);
            }
        }
        private DialogAdapter _dialogAdapter;

        public event EventHandler ValueChanged;
        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            return null;
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}