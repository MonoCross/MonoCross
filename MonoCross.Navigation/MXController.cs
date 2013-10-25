using System;
using System.Collections.Generic;

namespace MonoCross.Navigation
{
    public interface IMXController
    {
        /// <summary>
        /// Gets or sets the parameters added to the controller.
        /// </summary>
        /// <value>
        /// The parameters as a <see cref="Dictionary{TKey,TValue}"/> instance.
        /// </value>
        Dictionary<string, string> Parameters { get; set; }

        String Uri { get; set; }
        IMXView View { get; set; }

        /// <summary>
        /// The type of the model used by this controller.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        object GetModel();

        string Load(Dictionary<string, string> parameters);

        /// <summary>
        /// Calls <see cref="IMXView.Render"/> on the <see cref="View"/>.
        /// </summary>
        void RenderView();
    }

    public abstract class MXController<T> : IMXController
    {
        public string Uri { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the model for the controller.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// The type of the model used by this controller.
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        public virtual IMXView View { get; set; }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        public object GetModel() { return Model; }
        public abstract string Load(Dictionary<string, string> parameters);

        /// <summary>
        /// Calls <see cref="IMXView.Render"/> on the <see cref="View"/>.
        /// </summary>
        public virtual void RenderView() { if (View != null) View.Render(); }
    }
}