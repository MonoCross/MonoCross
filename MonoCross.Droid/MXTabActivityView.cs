using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.OS;

using MonoCross.Navigation;

namespace MonoCross.Droid
{
    /// <summary>
    /// Renders tabs for an application.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMXView.ModelType"/>.</typeparam>
    public abstract class MXTabActivityView<T> : TabActivity, IMXView
    {
        /// <summary>
        /// Called when the activity is starting.
        /// </summary>
        /// <param name="savedInstanceState">If the activity is being re-initialized after previously being shut down then this Bundle contains the data it most
        ///   recently supplied in <c><see cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)"/></c>.  <format type="text/html"><b><i>Note: Otherwise it is null.</i></b></format></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // fetch the model before rendering!!!
            Type t = typeof(T);
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
            // render the model within the view
            Render();
        }

        /// <summary>
        /// Gets or sets the model for the view.
        /// </summary>
        public T Model
        {
            get { return _model; }
            set { _model = value; NotifyModelChanged(); }
        }
        private T _model;

        /// <summary>
        /// The type of the model displayed by this view
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// Performs a layout according to the state of the <see cref="Model"/>.
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Sets the model for the view. An InvalidCastException may be thrown if a model of the wrong type is set.
        /// </summary>
        public void SetModel(object model)
        {
            Model = (T)model;
        }

        /// <summary>
        /// Event fired from <see cref="NotifyModelChanged"/>
        /// </summary>
        public event ModelEventHandler ViewModelChanged;

        /// <summary>
        /// Method activated by <see cref="ViewModelChanged"/>
        /// </summary>
        /// <param name="model">The new object returned from a <see cref="ViewModelChanged"/> event.</param>
        protected virtual void OnViewModelChanged(object model) { }
        private void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(Model); }
    }
}