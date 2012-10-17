using System;
using System.Collections.Generic;
using System.Linq;
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
            var t = typeof(T);
            if (MXDroidContainer.ViewModels.ContainsKey(t))
            {
                SetModel(MXDroidContainer.ViewModels[t]);
            }
            else
            {
                var mapping = MXContainer.Instance.App.NavigationMap.FirstOrDefault(layer => layer.Controller.ModelType == t);
                if (mapping == null)
                {
                    throw new ApplicationException("The navigation map does not contain any controllers for type " + t);
                }
                mapping.Controller.Load(new Dictionary<string, string>());
                SetModel(mapping.Controller.GetModel());
            }

            ViewModelChanged += OnViewModelChanged;
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater p0, Android.Views.ViewGroup p1, Bundle p2)
        {
            Render();
            return base.OnCreateView(p0, p1, p2);
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
        protected virtual void OnViewModelChanged(object model) { }
        private void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(Model); }
    }
}