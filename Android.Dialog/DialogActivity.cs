using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;

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

                this.ListAdapter = this.ListAdapter = new DialogAdapter(this, _root);
            }
        }
        RootElement _root;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.ListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(ListView_ItemClick);
            this.ListView.ItemLongClick +=new EventHandler<AdapterView.ItemLongClickEventArgs>(ListView_ItemLongClick);
                
            //this.ListView.Tag = Root;

            if (this.LastNonConfigurationInstance != null)
            {
                // apply value changes that are saved
            }
        }

        void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            DialogAdapter dialogAdapter = this.ListAdapter as DialogAdapter;
            Element elem = dialogAdapter.ElementAtIndex(e.Position);
            if (elem != null && elem.LongClick != null)
                elem.LongClick(sender, e);
        }

        void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            DialogAdapter dialogAdapter = this.ListAdapter as DialogAdapter;
            Element elem = dialogAdapter.ElementAtIndex(e.Position);
            if (elem != null && elem.Click != null)
                elem.Click(sender, e);
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            return null;
        }
    }
}