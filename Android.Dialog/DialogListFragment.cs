using System;
using Android.Support.V4.App;

namespace Android.Dialog
{
    public class DialogListFragment : ListFragment, IDialogView
    {
        private RootElement _root;

        public RootElement Root
        {
            get { return DialogAdapter == null ? null : DialogAdapter.Root; }
            set
            {
                value.ValueChanged += HandleValueChangedEvent;
                if (Root == null) _root = value;
                else
                {
                    Root.ValueChanged -= HandleValueChangedEvent;
                    value.Context = Activity;
                    DialogAdapter.Root = value;
                }
            }
        }

        public override void OnViewCreated(Views.View p0, OS.Bundle p1)
        {
            if (DialogAdapter != null && DialogAdapter.List != null)
                DialogAdapter.DeregisterListView();

            DialogAdapter = new DialogAdapter(Activity, _root, ListView);
            base.OnViewCreated(p0, p1);
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
    }
}