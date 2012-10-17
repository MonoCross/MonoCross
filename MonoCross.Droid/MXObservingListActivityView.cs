using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MonoCross.Navigation;

namespace MonoCross.Droid
{
    /// <summary>
    /// Abstract class for implementing a ListActivity that observes it's Model
    /// </summary>
    /// <typeparam name="T">T is the type of the Model we are displaying. T must me a notifying model (implementing IMXNotifying)</typeparam>
    public abstract class MXObservingListActivityView<T> : ListActivity, IMXView
    where T : IMXNotifying
    {
        /// <summary>
        /// Local IMXView implementation to delegate IMXView to
        /// </summary>
        protected MXView<T> LocalView { set; get; }

        public MXObservingListActivityView()
        {
            LocalView = new MxDroidObservingView<T>(this);

            // Sign up for the AfterRender event on the LocalView so we can do our implementation of render when it fires
            LocalView.AfterRender += (sender, args) => DoRender();

        }

        protected override void OnCreate(Bundle bundle)
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

            // render the model within the view
            Render();
        }

        /// <summary>
        /// This method should be overridden to do the rendering rather than Render.
        /// This is because Render is delegated. This method is called after the delegated Render runs.
        /// </summary>
        protected abstract void DoRender();

        #region IMXView Members

        public Type ModelType
        {
            get { return LocalView.ModelType; }
        }

        public void SetModel(object model)
        {
            LocalView.SetModel(model);
        }

        /// <summary>
        /// Do not re-implement this method. To do your rendering override DoRender
        /// </summary>
        public void Render()
        {
            LocalView.Render();
        }

        #endregion
    }

}
