using System;
using Android.App;

namespace Android.Dialog
{
    public class DialogActivity : ListActivity, IDialogView
    {
        protected override void OnResume()
        {
            base.OnResume();
            this.ReloadData();
        }

        public RootElement Root
        {
            get { return DialogAdapter == null ? null : DialogAdapter.Root; }
            set
            {
                value.ValueChanged += HandleValueChangedEvent;
                if (Root == null) DialogAdapter = new DialogAdapter(this, value, ListView);
                else
                {
                    Root.ValueChanged -= HandleValueChangedEvent;
                    value.Context = this;
                    DialogAdapter.Root = value;
                }
            }
        }

        public DialogAdapter DialogAdapter
        {
            get { return ListAdapter as DialogAdapter; }
            set { ListAdapter = value; }
        }

        public event EventHandler ValueChanged;
        protected void HandleValueChangedEvent(object sender, EventArgs args)
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