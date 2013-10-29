using System;
using System.Collections.Generic;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Describes a navigation controller.
    /// </summary>
    public interface IMXController
    {
        /// <summary>
        /// Gets or sets the parameters added to the controller.
        /// </summary>
        Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the URI that was used to navigate to this instance.
        /// </summary>
        string Uri { get; set; }

        /// <summary>
        /// Thw <see cref="IMXView"/> that is loaded into the controller from a <see cref="ViewPerspective"/>.
        /// </summary>
        IMXView View { get; set; }

        /// <summary>
        /// The type of the model used by this controller.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        object GetModel();

        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="parameters">A <see cref="Dictionary{TKey,TValue}"/> representing any parameters such as submitted values.</param>
        string Load(Dictionary<string, string> parameters);

        /// <summary>
        /// Calls <see cref="IMXView.Render"/> on the <see cref="View"/>.
        /// </summary>
        void RenderView();
    }

    /// <summary>
    /// Represents the base implementation of a navigation controller. This class is abstract.
    /// </summary>
    public abstract class MXController<T> : IMXController
    {
        /// <summary>
        /// Gets or sets the URI that was used to navigate to this instance.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the parameters added to the controller.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the model for the controller.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// The type of the model used by this controller.
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// Thw <see cref="IMXView"/> that is loaded into the controller from a <see cref="ViewPerspective"/>.
        /// </summary>
        public virtual IMXView View { get; set; }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        public object GetModel() { return Model; }

        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="parameters">A <see cref="Dictionary{TKey,TValue}"/> representing any parameters such as submitted values.</param>
        public abstract string Load(Dictionary<string, string> parameters);

        /// <summary>
        /// Calls <see cref="IMXView.Render"/> on the <see cref="View"/>.
        /// </summary>
        public virtual void RenderView() { if (View != null) View.Render(); }
    }
}