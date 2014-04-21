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
        /// Gets the parameters added to the controller.
        /// </summary>
        Dictionary<string, string> Parameters { get; }

        /// <summary>
        /// Gets the URI that was used to navigate to this instance.
        /// </summary>
        string Uri { get; }

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
    }

    /// <summary>
    /// Represents the base implementation of a navigation controller. This class is abstract.
    /// </summary>
    public abstract class MXController<T> : IMXController
    {
        /// <summary>
        /// Gets the URI that was used to navigate to this instance.
        /// </summary>
        public string Uri { get; protected set; }

        /// <summary>
        /// Gets the parameters added to the controller.
        /// </summary>
        public Dictionary<string, string> Parameters { get; protected set; }

        /// <summary>
        /// Gets or sets the model for the controller.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Gets the type of the model used by this controller.
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        public object GetModel() { return Model; }

        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="parameters">A <see cref="Dictionary{TKey,TValue}"/> representing any parameters such as submitted values.</param>
        public abstract string Load(Dictionary<string, string> parameters);
    }
}