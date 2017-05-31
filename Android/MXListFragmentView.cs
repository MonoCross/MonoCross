using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Views;
using MonoCross.Navigation;
using Android.App;

namespace MonoCross.Droid
{
    /// <summary>
    /// Renders an <see cref="ListFragment"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMXView.ModelType"/>.</typeparam>
    public abstract class MXListFragmentView<T> : ListFragment, IMXView
    {
        /// <summary>
        /// Called to do initial creation of a fragment.
        /// </summary>
        /// <param name="savedInstanceState">
        /// If the fragment is being re-created from a previous saved state, this is the state.
        /// </param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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

        /// <summary>
        /// Called to have the fragment instantiate its user interface view.
        /// </summary>
        /// <param name="inflater">The LayoutInflater object that can be used to inflate any views in the fragment.</param>
        /// <param name="container">If non-null, this is the parent view that the fragment's UI should be attached to.  The fragment should not add the view itself,
        ///  but this can be used to generate the LayoutParams of the view.</param>
        /// <param name="savedInstanceState">If non-null, this fragment is being re-constructed from a previous saved state as given here.</param>
        /// <returns>
        /// The created <see cref="View"/>.
        /// </returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Render();
            return base.OnCreateView(inflater, container, savedInstanceState);
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
        /// Displays the view according to the state of the model.
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
        /// Get the model for the view.
        /// </summary>
		public object GetModel()
		{
			return Model;
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