using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Android.Dialog
{
    public class DialogInstanceData : Java.Lang.Object
    {
        public DialogInstanceData()
        {
            _dialogState = new Dictionary<string, string>();
        }

        private Dictionary<String, String> _dialogState;
    }

    public class DialogActivity : ListActivity
    {
        public RootElement Root
        {
            get { return _root; }
            set
            {
                _root = value;
                _root.Context = this;
                _root.ValueChanged += HandleValueChangedEvent;

                this.ListAdapter = new DialogAdapter(this, _root);
            }
        }
        RootElement _root;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ListView.ItemClick += ListView_ItemClick;
            ListView.ItemLongClick += ListView_ItemLongClick;

            if (LastNonConfigurationInstance != null)
            {
                // apply value changes that are saved
            }
        }

        void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var dialogAdapter = (DialogAdapter)ListAdapter;
            var elem = dialogAdapter.ElementAtIndex(e.Position);
            if (elem != null && elem.LongClick != null)
                elem.LongClick(sender, e);
        }

        void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var dialogAdapter = (DialogAdapter)ListAdapter;
            var elem = dialogAdapter.ElementAtIndex(e.Position);
            if (elem != null && elem.Click != null)
                elem.Click(sender, e);
        }

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
    }
}