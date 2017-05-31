namespace System.Collections.Generic
{
	/// <summary>
	/// Provides methods for easily retrieving values from an <see cref="IDictionary&lt;TKey, TValue&gt;"/> object.
	/// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Merges the contents of the given dictionary into the current dictionary, overriding any current values.
        /// </summary>
        /// <param name="current">The current dictionary.</param>
        /// <param name="dictionary">The dictionary to be merged.</param>
        /// <exception cref="ArgumentNullException">Thrown when either dictionary is <c>null</c>.</exception>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> current, IDictionary<TKey, TValue> dictionary)
        {
            if (current == null)
            {
                throw new ArgumentNullException("current");
            }

            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            foreach (var key in dictionary.Keys)
            {
                current[key] = dictionary[key];
            }
        }

		/// <summary>
		/// Returns the value associated with the specified key,
		/// or returns the default value of TValue if the key was not found.
		/// </summary>
		/// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> object.</param>
		/// <param name="key">The key for the value to return.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		/// <typeparam name="TValue">The 2nd type parameter.</typeparam>
		/// <returns>The value associated with the key -or- the default value of TValue.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (key == null)
                return default(TValue);

            TValue value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

		/// <summary>
		///	Returns the value associated with the specified key,
		/// or returns the specified default value if the key was not found.
		/// </summary>
		/// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> object.</param>
		/// <param name="key">The key for the value to return.</param>
		/// <param name="defaultValue">The value to return if the key was not found.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		/// <typeparam name="TValue">The 2nd type parameter.</typeparam>
		/// <returns>The value associated with the key -or- the specified default value.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (key == null)
                return defaultValue;

            TValue value;
            if (dictionary.TryGetValue(key, out value))
                return value;

            return defaultValue;
        }
    }
}
