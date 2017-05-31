using System;
using System.Linq;
using System.Reflection;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Represents a mapping of a model type and perspective to <see cref="IMXView&lt;T&gt;"/>s in a container.
    /// </summary>
    public class MXViewMap : NamedTypeMap
    {
        /// <summary>
        /// Performs type constraints on new entries.
        /// </summary>
        /// <param name="name">The entry name passed by reference so that it may be changed.</param>
        /// <param name="key">The key <see cref="Type"/>.</param>
        /// <param name="value">the value <see cref="Type"/>.</param>
        protected override void CheckEntry(ref string name, Type key, Type value)
        {
            if (name == null) name = ViewPerspective.Default;
            if (value == null) { throw new ArgumentNullException("value"); }
            if (!value.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IMXView)))
            {
                throw new ArgumentException("Type provided does not implement IMXView interface.", "value");
            }
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="perspective">The view perspective key.</param>
        /// <param name="view">The initialized view value.</param>
        public void Add(string perspective, IMXView view)
        {
            if (view == null) { throw new ArgumentNullException("view"); }
            Add(perspective, view.ModelType, new TypeLoader(view));
        }

        /// <summary>
        /// Gets the type of the view described by a view perspective.
        /// </summary>
        /// <param name="modelType">The view's model type.</param>
        /// <param name="perspective">The view perspective.</param>
        /// <returns>The type associated with the view perspective.</returns>
        public Type GetViewType(Type modelType, string perspective)
        {
            Type type;
            TryGetValue(modelType, perspective ?? ViewPerspective.Default, out type);
            return type;
        }

        /// <summary>
        /// Gets the view described by a view perspective.
        /// </summary>
        /// <param name="modelType">The view's model type.</param>
        /// <param name="perspective">The view perspective.</param>
        /// <returns>The view associated with the view perspective.</returns>
        public IMXView GetView(Type modelType, string perspective)
        {
            object o;
            TryGetInstance(modelType, perspective ?? ViewPerspective.Default, out o);
            return o as IMXView;
        }

        /// <summary>
        /// Gets the view or creates it if it has not been created.
        /// </summary>
        /// <param name="modelType">The view's model type.</param>
        /// <param name="perspective">The view perspective.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Thrown when no view or view type is found in the view map</exception>
        public IMXView GetOrCreateView(Type modelType, string perspective)
        {
            if (perspective == null) perspective = ViewPerspective.Default;
            if (!ContainsKey(modelType, perspective))
            {
                // No view
                throw new ArgumentException(string.Format("No View for model type: {0} with perspective: {1}", modelType.Name, perspective == string.Empty ? "Default" : perspective), "perspective");
            }

            return Resolve(modelType, perspective) as IMXView;
        }

        /// <summary>
        /// Determines whether a view has been registered for a model and perspective.
        /// </summary>
        /// <param name="modelType">The view's model type.</param>
        /// <param name="perspective">The view perspective.</param>
        /// <returns><c>true</c> if a view has been registered to the model type and perspective; otherwise <c>false</c>.</returns>
        public bool ContainsView(Type modelType, string perspective)
        {
            return ContainsKey(modelType, perspective ?? ViewPerspective.Default);
        }
    }
}