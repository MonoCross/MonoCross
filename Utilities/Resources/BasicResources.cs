using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace MonoCross.Utilities.Resources
{
    /// <summary>
    /// An <see cref="IResources"/> implementation that provides support for getting strings.
    /// </summary>
    public class BasicResources : IResources
    {
        /// <summary>
        /// The resources managed by this instance.
        /// </summary>
        protected readonly List<ResourceManager> Resources;

        /// <summary>
        /// Gets the number of resource providers contained in this instance.
        /// </summary>
        public int Count { get { return Resources.Count; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicResources"/> class.
        /// </summary>
        public BasicResources()
        {
            Resources = new List<ResourceManager>();
        }

        /// <summary>
        /// Removes all resource providers from this instance.
        /// </summary>
        public virtual void RemoveAllResources()
        {
            Resources.Clear();
        }

        /// <summary>
        /// Adds a resource manager that provides convenient access to culture-specific resources at run time.
        /// </summary>
        /// <param name="baseName">The root name of the resource file without its extension but including any fully qualified namespace name. For example, the root name for the resource file named MyApplication.MyResource.en-US.resources is MyApplication.MyResource.</param>
        /// <param name="assembly">The main assembly for the resources.</param>
        public virtual void AddResources(string baseName, Assembly assembly)
        {
            Resources.Add(new ResourceManager(baseName, assembly));
        }

        /// <summary>
        /// Adds resource managers that provide convenient access to culture-specific resources at run time.
        /// </summary>
        /// <param name="assembly">The main assembly for the resources.</param>
        public virtual void AddResources(Assembly assembly)
        {
            Resources.AddRange(assembly.GetManifestResourceNames()
                .Where(n => n.EndsWith(".resources"))
                .Select(n => new ResourceManager(n.Remove(n.LastIndexOf('.')), assembly)));
        }

        /// <summary>
        /// Gets the value of the specified non-string resource localized for the current culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <returns>
        /// The value of the resource localized for the current culture, or <c>null</c> if <paramref name="key" /> cannot be found in a resource set.
        /// </returns>
        /// <exception cref="System.NotSupportedException">GetObject is not supported in BasicResources</exception>
        public virtual object GetObject(string key)
        {
            throw new NotSupportedException("GetObject is not supported in BasicResources");
        }

        /// <summary>
        /// Gets the value of the specified non-string resource localized for the specified culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <param name="culture">An object that represents the culture for which the resource is localized.</param>
        /// <returns>
        /// The value of the resource localized for the specified culture, or <c>null</c> if <paramref name="key" /> cannot be found in a resource set.
        /// </returns>
        /// <exception cref="System.NotSupportedException">GetObject is not supported in BasicResources</exception>
        public virtual object GetObject(string key, CultureInfo culture)
        {
            throw new NotSupportedException("GetObject is not supported in BasicResources");
        }

        /// <summary>
        /// Returns the value of the string resource localized for the current culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <returns>
        /// The value of the resource localized for the current culture, or <c>null</c> if <paramref name="key" /> cannot be found in a resource set.
        /// </returns>
        public virtual string GetString(string key)
        {
            return GetString(key, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Returns the value of the string resource localized for the specified culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <param name="culture">An object that represents the culture for which the resource is localized.</param>
        /// <returns>
        /// The value of the resource localized for the specified culture, or <c>null</c> if <paramref name="key" /> cannot be found in a resource set.
        /// </returns>
        public virtual string GetString(string key, CultureInfo culture)
        {
            foreach (var resource in Resources.Reverse<ResourceManager>())
            {
                try
                {
                    var retval = resource.GetString(key, culture);
                    if (retval != null)
                        return retval;
                }
                catch (Exception e)
                {
                    Device.Log.Debug(string.Format("String \"{0}\" not found for culture: {1}", key, culture), e);
                }
            }
            return null;
        }
    }
}
