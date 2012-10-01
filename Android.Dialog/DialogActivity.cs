using System;
using System.Linq;
using Android.App;

namespace Android.Dialog
{
    public class DialogActivity : ListActivity
    {
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

        public void HandleValueChangedEvents(EventHandler eventHandler)
        {
            foreach (var element in Root.Sections.SelectMany(section => section))
            {
                if (element is EntryElement)
                    (element as EntryElement).Changed += eventHandler;
                if (element is BooleanElement)
                    (element as BooleanElement).Changed += eventHandler;
                if (element is CheckboxElement)
                    (element as CheckboxElement).Changed += eventHandler;
            }
        }

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