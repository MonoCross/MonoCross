using Android.OS;
using Android.Support.V4.App;
using MonoCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCross.Droid
{
    public abstract class MXFragmentView<T> : Fragment, IMXView
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
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public virtual void Render() { /* Override OnCreateView() in your subclass to render the view. */ }
        public void SetModel(object model)
        {
            Model = (T)model;
        }
    }
}