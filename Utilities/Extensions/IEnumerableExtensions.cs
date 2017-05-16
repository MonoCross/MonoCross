using System.Linq;

namespace System.Collections
{
    /// <summary>
    /// Provides methods for querying information from an <see cref="IEnumerable"/> object.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether the <see cref="IEnumerable"/> contains any elements.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <returns><c>true</c> if the <see cref="IEnumerable"/> contains any elements; otherwise, <c>false</c>.</returns>
        public static bool Any(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return false;

            foreach (var item in enumerable)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether any element in the <see cref="IEnumerable"/> satisfies a condition.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if any element in the <see cref="IEnumerable"/> satisfies the condition; otherwise, <c>false</c>.</returns>
        public static bool Any(this IEnumerable enumerable, Func<object, bool> predicate)
        {
            if (enumerable == null)
                return false;

            foreach (var item in enumerable)
            {
                if (predicate == null || predicate.Invoke(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the number of elements contained within the <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <returns>The number of elements in the <see cref="IEnumerable"/>.</returns>
        public static int Count(this IEnumerable enumerable)
        {
            return enumerable == null ? 0 : Enumerable.Count(enumerable.Cast<object>());
        }

        /// <summary>
        /// Returns the element at the specified index within the <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <param name="index">The index of the element to return.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index equals or exceeds the
        /// number of elements in the <see cref="IEnumerable"/> -or- when the index is less than 0.</exception>
        public static object ElementAt(this IEnumerable enumerable, int index)
        {
            if (enumerable == null)
                return null;

            int i = 0;
            foreach (var item in enumerable)
            {
                if (i++ == index)
                {
                    return item;
                }
            }

            throw new ArgumentOutOfRangeException("index");
        }

        /// <summary>
        /// Returns the first element in the <see cref="IEnumerable"/> or <c>null</c> if there are no elements.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <returns>The first element in the <see cref="IEnumerable"/> -or- <c>null</c> if there are no elements.</returns>
        public static object FirstOrDefault(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return null;

            foreach (var item in enumerable)
            {
                return item;
            }

            return null;
        }

        /// <summary>
        /// Returns the first element in the <see cref="IEnumerable"/> that satisfies a condition or <c>null</c> if no such element is found.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The first element in the <see cref="IEnumerable"/> that satisfies the condition -or- <c>null</c> if no such element is found.</returns>
        public static object FirstOrDefault(this IEnumerable enumerable, Func<object, bool> predicate)
        {
            if (enumerable == null)
                return null;

            foreach (var item in enumerable)
            {
                if (predicate == null || predicate.Invoke(item))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the index at which the specified element resides within the <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <param name="element">The element to return the index of.</param>
        /// <returns>The index of the element -or- -1 if the element is not found in the collection.</returns>
        public static int IndexOf(this IEnumerable enumerable, object element)
        {
            if (enumerable == null)
                return -1;

            int i = 0;
            foreach (var item in enumerable)
            {
                if ((item == null && element == null) || (item != null && item.Equals(element)))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the last element in the <see cref="IEnumerable"/> or <c>null</c> if there are no elements.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <returns>The last element in the <see cref="IEnumerable"/> -or- <c>null</c> if there are no elements.</returns>
        public static object LastOrDefault(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return null;

            object obj = null;
            foreach (var item in enumerable)
            {
                obj = item;
            }

            return obj;
        }

        /// <summary>
        /// Returns the last element in the <see cref="IEnumerable"/> that satisfies a condition or <c>null</c> if no such element is found.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> object.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The last element in the <see cref="IEnumerable"/> that satisfies the condition -or- <c>null</c> if no such element is found.</returns>
        public static object LastOrDefault(this IEnumerable enumerable, Func<object, bool> predicate)
        {
            if (enumerable == null)
                return null;

            object obj = null;
            foreach (var item in enumerable)
            {
                if (predicate == null || predicate.Invoke(item))
                {
                    obj = item;
                }
            }

            return obj;
        }
    }
}

namespace System.Collections.Generic
{
    /// <summary>
    /// Provides methods for querying information from an <see cref="IEnumerable"/> object.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the index of the first element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable&lt;T&gt;"/> object.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The index of the first element that satisfies the condition.</returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (enumerable == null)
                return -1;

            int i = 0;
            foreach (var item in enumerable)
            {
                if (predicate == null || predicate.Invoke(item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the first element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable&lt;T&gt;"/> object.</param>
        /// <param name="startIndex">The zero-based starting index of the search.  The search proceeds from the start index to the end of the sequence.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The index of the first element that satisfies the condition.</returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, int startIndex, Func<T, bool> predicate)
        {
            if (enumerable == null)
                return -1;

            int i = 0;
            foreach (var item in enumerable)
            {
                if (i >= startIndex && (predicate == null || predicate.Invoke(item)))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the last element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable&lt;T&gt;"/> object.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The index of the last element that satisfies the condition.</returns>
        public static int LastIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (enumerable == null)
                return -1;

            int i = 0;
            int lastIndex = -1;
            foreach (var item in enumerable)
            {
                if (predicate == null || predicate.Invoke(item))
                {
                    lastIndex = i;
                }

                i++;
            }

            return lastIndex;
        }

        /// <summary>
        /// Returns the index of the last element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable&lt;T&gt;"/> object.</param>
        /// <param name="startIndex">The zero-based starting index of the search.  The search proceeds from the start index to the beginning of the sequence.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The index of the last element that satisfies the condition.</returns>
        public static int LastIndexOf<T>(this IEnumerable<T> enumerable, int startIndex, Func<T, bool> predicate)
        {
            if (enumerable == null)
                return -1;

            int i = 0;
            int lastIndex = -1;
            foreach (var item in enumerable)
            {
                if (i <= startIndex && (predicate == null || predicate.Invoke(item)))
                {
                    lastIndex = i;
                }

                i++;
            }

            return lastIndex;
        }

        /// <summary>
        /// Determines whether two <see cref="IEnumerable&lt;T&gt;"/> objects have equivalent contents.
        /// </summary>
        /// <typeparam name="T">The enumerable type parameter.</typeparam>
        /// <param name="first">The first enumerable to check.</param>
        /// <param name="second">The second enumerable to check.</param>
        /// <param name="orderMatters"><c>true</c> to compare elements by order and contents; otherwise <c>false</c> to compare just contents</param>
        /// <returns><c>true</c> if the enumerables are equivalent; otherwise <c>false</c>.</returns>
        public static bool Equivalent<T>(this IEnumerable<T> first, IEnumerable<T> second, bool orderMatters)
        {
            if (first == null)
                return second == null;

            if (second == null)
                return false;

            if (ReferenceEquals(first, second))
                return true;

            var firstCollection = first as ICollection<T>;
            var secondCollection = second as ICollection<T>;
            if (firstCollection != null && secondCollection != null)
            {
                if (firstCollection.Count != secondCollection.Count)
                    return false;

                if (firstCollection.Count == 0)
                    return true;
            }

            int firstCount;
            int secondCount;
            var firstElementCounts = GetElementCounts(first, out firstCount);
            var secondElementCounts = GetElementCounts(second, out secondCount);

            if (firstCount != secondCount)
                return false;

            for (int i = 0; i < firstElementCounts.Count; i++)
            {
                var kvp = (KeyValuePair<T, int>)firstElementCounts.ElementAt(i);
                firstCount = kvp.Value;
                if (orderMatters)
                {
                    var secondKvp = (KeyValuePair<T, int>)secondElementCounts.ElementAt(i);
                    secondCount = secondKvp.Key.Equals(kvp.Key) ? secondKvp.Value : -1;
                }
                else
                {
                    secondElementCounts.TryGetValue(kvp.Key, out secondCount);
                }

                if (firstCount != secondCount)
                    return false;
            }

            return firstElementCounts.Count > 0 || secondElementCounts.Count == 0;
        }

        private static Dictionary<T, int> GetElementCounts<T>(IEnumerable<T> enumerable, out int nullCount)
        {
            var dictionary = new Dictionary<T, int>();
            nullCount = 0;

            foreach (T element in enumerable)
            {
                if (element == null)
                {
                    nullCount++;
                }
                else
                {
                    int num;
                    dictionary.TryGetValue(element, out num);
                    num++;
                    dictionary[element] = num;
                }
            }

            return dictionary;
        }
    }
}