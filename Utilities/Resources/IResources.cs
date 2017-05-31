using System.Globalization;
using System.Reflection;

namespace MonoCross.Utilities.Resources
{
    /// <summary>
    /// Describes the functionality of a Resources provider.
    /// </summary>
    public interface IResources
    {
        /// <summary>
        /// Gets the number of resource providers contained in this instance.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Removes all resource providers from this instance.
        /// </summary>
        void RemoveAllResources();

        /// <summary>
        /// Adds a resource manager that provides convenient access to culture-specific resources at run time.
        /// </summary>
        /// <param name="baseName">The root name of the resource file without its extension but including any fully qualified namespace name. For example, the root name for the resource file named MyApplication.MyResource.en-US.resources is MyApplication.MyResource. </param>
        /// <param name="assembly">The main assembly for the resources.</param>
        void AddResources(string baseName, Assembly assembly);

        /// <summary>
        /// Adds a resource manager that provides convenient access to culture-specific resources at run time.
        /// </summary>
        /// <param name="assembly">The main assembly for the resources.</param>
        void AddResources(Assembly assembly);

        /// <summary>
        /// Gets the value of the specified non-string resource localized for the current culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <returns>The value of the resource localized for the current culture, or <c>null</c> if <paramref name="key"/> cannot be found in a resource set.</returns>
        object GetObject(string key);

        /// <summary>
        /// Gets the value of the specified non-string resource localized for the specified culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <param name="culture">An object that represents the culture for which the resource is localized.</param>
        /// <returns>The value of the resource localized for the specified culture, or <c>null</c> if <paramref name="key"/> cannot be found in a resource set.</returns>
        object GetObject(string key, CultureInfo culture);

        /// <summary>
        /// Returns the value of the string resource localized for the current culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <returns>The value of the resource localized for the current culture, or <c>null</c> if <paramref name="key"/> cannot be found in a resource set.</returns>
        string GetString(string key);

        /// <summary>
        /// Returns the value of the string resource localized for the specified culture.
        /// </summary>
        /// <param name="key">The name of the resource to retrieve.</param>
        /// <param name="culture">An object that represents the culture for which the resource is localized.</param>
        /// <returns>The value of the resource localized for the specified culture, or <c>null</c> if <paramref name="key"/> cannot be found in a resource set.</returns>
        string GetString(string key, CultureInfo culture);
    }
}
