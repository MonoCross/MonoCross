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
        /// The <see cref="IMXView"/> described by this controller's current <see cref="ViewEntry"/>.
        /// </summary>
        IMXView View { get; }

        /// <summary>
        /// Gets or sets the location of the <see cref="IMXView"/> in cache.
        /// </summary>
        MXViewEntry ViewEntry { get; set; }

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
        /// Calls <see cref="IMXView.Render"/> on the <see cref="IMXView"/> in cache.
        /// </summary>
        void RenderView();
    }

    /// <summary>
    /// Represents the base implementation of a navigation controller. This class is abstract.
    /// </summary>
    public abstract class MXController<T> : IMXController
    {
        /// <summary>
        /// Gets the URI that was used to navigate to this instance.
        /// </summary>
        public string Uri { get { return ViewEntry.Uri; } }

        /// <summary>
        /// Gets the parameters added to the controller.
        /// </summary>
        public Dictionary<string, string> Parameters { get { return ViewEntry.Parameters; } }

        /// <summary>
        /// Gets or sets the model for the controller.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Gets the type of the model used by this controller.
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// Gets the <see cref="IMXView"/> described by this controller's current <see cref="ViewEntry"/>.
        /// </summary>
        public IMXView View { get { return MXContainer.Instance.Views.GetView(ViewEntry); } }

        /// <summary>
        /// Gets or sets the location of the view in cache.
        /// </summary>
        public MXViewEntry ViewEntry { get; set; }

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
        /// Calls <see cref="IMXView.Render"/> on the <see cref="IMXView"/> that the controller loaded.
        /// </summary>
        public virtual void RenderView()
        {
            var view = MXContainer.Instance.Views.GetView(ViewEntry);
            if (view != null) view.Render();
        }
    }
}