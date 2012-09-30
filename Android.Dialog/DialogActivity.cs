using System;
using System.Linq;
using Android.App;

namespace Android.Dialog
{
    public class DialogActivity : ListActivity
    {
        public RootElement Root
        {
            get { return _root; }
            set
            {
                _root = value;
                _root.Context = this;
                _root.ValueChanged -= HandleValueChangedEvent;
                _root.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    ListView.ItemClick -= _dialogAdapter.ListView_ItemClick;
                    ListView.ItemLongClick -= _dialogAdapter.ListView_ItemLongClick;
                }

                ListAdapter = _dialogAdapter = new DialogAdapter(this, _root);
                ListView.ItemClick += _dialogAdapter.ListView_ItemClick;
                ListView.ItemLongClick += _dialogAdapter.ListView_ItemLongClick;
            }
        }
        private RootElement _root;
        private DialogAdapter _dialogAdapter;

        public void HandleValueChangedEvents(EventHandler eventHandler)
        {
            foreach (var element in _root.Sections.SelectMany(section => section))
            {
                if (element is EntryElement)
                    (element as EntryElement).ValueChanged += eventHandler;
                if (element is BooleanElement)
                    (element as BooleanElement).ValueChanged += eventHandler;
                if (element is CheckboxElement)
                    (element as CheckboxElement).ValueChanged += eventHandler;
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