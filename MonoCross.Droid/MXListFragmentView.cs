using System;
using Android.OS;
using Android.Support.V4.App;
using MonoCross.Navigation;

namespace MonoCross.Droid
{
    public abstract class MXListFragmentView<T> : ListFragment, IMXView
    {
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // fetch the model before rendering!!!
            Model = (T)MXDroidContainer.ViewModels[typeof(T)];
            ViewModelChanged += OnViewModelChanged;
        }

        private T _model;
        public T Model
        {
            get { return _model; }
            set { _model = value; NotifyModelChanged(); }
        }
        public Type ModelType { get { return typeof(T); } }

        public abstract void Render();

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater p0, Android.Views.ViewGroup p1, Bundle p2)
        {
            Render();
            return base.OnCreateView(p0, p1, p2);
        }

        public void SetModel(object model)
        {
            Model = (T)model;
        }

        public event ModelEventHandler ViewModelChanged;
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(Model); }
    }
}