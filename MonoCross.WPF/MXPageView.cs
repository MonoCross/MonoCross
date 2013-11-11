using System;
using System.Windows.Controls;
using MonoCross.Navigation;

namespace MonoCross.WPF
{
    /// <summary>
    /// A <see cref="Page"/> for rendering MonoCross views.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MXPageView<T> : Page, IMXView
    {
        /// <summary>
        /// Gets or sets the model for this view.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// The type of the model displayed by this view
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// Displays the view according to the state of the model.
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Sets the model for the view.
        /// </summary>
        /// <param name="model">The model to associate with the view.</param>
        /// <exception cref="InvalidCastException">Thrown if a model of the wrong type is set.</exception>
        public void SetModel(object model)
        {
            Model = (T)model;
        }
    }
}