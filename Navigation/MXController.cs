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
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        [Obsolete("Deprecated in favor of the new Load(string, Dictionary<string, string>) method")]
        string Load(Dictionary<string, string> parameters);


        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="uri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        string Load(string uri, Dictionary<string, string> parameters);
    }

    /// <summary>
    /// Represents the base implementation of a navigation controller. This class is abstract.
    /// </summary>
    public abstract class MXController<T> : IMXController
    {
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
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        [Obsolete("Deprecated in favor of the new Load(string, Dictionary<string, string>) method")]
        public virtual string Load(Dictionary<string, string> parameters) { throw new NotImplementedException("You must override a Load method"); }

        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="uri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        public virtual string Load(string uri, Dictionary<string, string> parameters) 
        { 
#pragma warning disable 618
            return Load(parameters); 
#pragma warning restore 618
        }
    }
}