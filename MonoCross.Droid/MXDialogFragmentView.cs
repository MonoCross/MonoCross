using System;
using Android.Dialog;
using Android.OS;
using MonoCross.Navigation;

namespace MonoCross.Droid
{
    public abstract class MXDialogFragmentView<T> : DialogListFragment, IMXView
    {
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // fetch the model before rendering!!!
            _model = (T)MXDroidContainer.ViewModels[typeof(T)];
            ViewModelChanged += OnViewModelChanged;
            // render the model within the view
            Render();
        }

        private T _model;
        public T Model
        {
            get { return _model; }
            set { _model = value; NotifyModelChanged(); }
        }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();

        public void SetModel(object model)
        {
            Model = (T)model;
        }

        public event ModelEventHandler ViewModelChanged;
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged()
        {
            if (ViewModelChanged != null) ViewModelChanged(Model);
            ReloadData();
        }
    }
}