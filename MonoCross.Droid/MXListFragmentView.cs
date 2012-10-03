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
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// Required for interface. Use OnCreateView to build Fragment UI.
        /// </summary>
        public virtual void Render() { }

        public void SetModel(object model)
        {
            Model = (T)model;
        }

        public event ModelEventHandler ViewModelChanged;
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(Model); }
    }
}