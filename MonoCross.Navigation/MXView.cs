using System;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Delegate type for handling model events from the view
    /// </summary>
    public delegate void ModelEventHandler(object model);

    #region IMXView interface
    /// <summary>
    /// Interface that marks a class as being a View
    /// </summary>
    public interface IMXView
    {
        /// <summary>
        /// The type of the model displayed by this view
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// Sets the model for the view.
        /// </summary>
        /// <param name="model">The model to associate with the view.</param>
        /// <exception cref="InvalidCastException">Thrown if a model of the wrong type is set.</exception>
        void SetModel(object model);

        /// <summary>
        /// Displays the view according to the state of the model.
        /// </summary>
        void Render();
    }

    /// <summary>
    /// Interface that marks an <see cref="IMXView" /> that displays models of type T.
    /// </summary>
    /// <typeparam name="T">The type of the Model.</typeparam>
    public interface IMXView<T> : IMXView
    {
        /// <summary>
        /// Gets or sets the model for the view.
        /// </summary>
        T Model { get; set; }
    }
    #endregion

    /// <summary>
    /// Base class for helping to implement Views that display models of type T. 
    /// You can chooses to either inherit from this base class or re-implement the IMXView from scratch. 
    /// Alternatively your view class can have a member that inherits from this class and is delegated to
    /// for the IMXView implementation (Bridge Pattern).
    /// </summary>
    /// <typeparam name="T">The type of Model that the view displays</typeparam>
    public abstract class MXView<T> : IMXView<T>
    {
        /// <summary>
        /// The type of the model displayed by this view
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// Sets the model for the view. An InvalidCastException may be thrown if a model of the wrong type is set.
        /// </summary>
        public virtual void SetModel(object model)
        {
            Model = (T)model;
        }

        /// <summary>
        /// This implementaion does nothing but fire the event.
        /// If you plan to override this method you should call the base implementation after
        /// you have done your Render().
        /// </summary>
        public virtual void Render() { }

        /// <summary>
        /// Gets or sets the model for the view.
        /// </summary>
        public virtual T Model { get; set; }
    }
}