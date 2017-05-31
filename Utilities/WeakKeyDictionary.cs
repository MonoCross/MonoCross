using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoCross.Utilities
{
    /// <summary>
    /// Represents a collection of keys and values.  The keys are weakly referenced, thereby allowing
    /// them to be reclaimed by the garbage collector if no other references to them exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
    public sealed class WeakKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly Dictionary<WeakKeyReference, TValue> items;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private long lastMemoryMarker = 0;

        /// <summary>
        /// Gets the number of key/value pairs contained in the <see cref="T:WeakKeyDictionary&lt;TKey, TValue&gt;"/>.
        /// This number may include dead references; to get a count of live references only, call the
        /// <see cref="M:PurgeDeadKeys"/> method prior to calling <see cref="P:Count"/>.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets a collection containing the live keys in the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                var deadKeys = new List<WeakKeyReference>();
                foreach (var key in items.Keys)
                {
                    if (key.IsAlive)
                    {
                        yield return key.Target;
                    }
                    else
                    {
                        deadKeys.Add(key);
                    }
                }

                try
                {
                    foreach (var key in deadKeys)
                    {
                        items.Remove(key);
                    }
                }
                // if called inside of another enumeration
                catch (InvalidOperationException) { }
            }
        }

        /// <summary>
        /// Gets a collection containing the values in the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.
        /// This may include values associated with dead keys; to get values from live keys only, call the
        /// <see cref="M:PurgeDeadKeys"/> method prior to calling <see cref="P:Values"/>.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return items.Values; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the value is retrieved and the <paramref name="key"/> is not present in the dictionary.</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                return items[new WeakKeyReference(key)];
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                CheckMemory();
                items[new WeakKeyReference(key)] = value;
            }
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public WeakKeyDictionary()
            : this(EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of the elements that the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="capacity"/> is less than zero.</exception>
        public WeakKeyDictionary(int capacity)
            : this(capacity, EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dictionary">A <see cref="IDictionary&lt;TKey, TValue&gt;"/> whose elements are copied to the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <c>null</c>.</exception>
        public WeakKeyDictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer&lt;TKey&gt;"/> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of key.</param>
        public WeakKeyDictionary(IEqualityComparer<TKey> comparer)
        {
            items = new Dictionary<WeakKeyReference, TValue>(new WeakReferenceComparer(comparer));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of the elements that the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> can contain.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer&lt;TKey&gt;"/> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of key.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="capacity"/> is less than zero.</exception>
        public WeakKeyDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }

            items = new Dictionary<WeakKeyReference, TValue>(capacity, new WeakReferenceComparer(comparer));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dictionary">A <see cref="IDictionary&lt;TKey, TValue&gt;"/> whose elements are copied to the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer&lt;TKey&gt;"/> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <c>null</c>.</exception>
        public WeakKeyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            items = new Dictionary<WeakKeyReference, TValue>(new WeakReferenceComparer(comparer));
            foreach (var key in dictionary.Keys)
            {
                items[new WeakKeyReference(key)] = dictionary[key];
            }
        }
        #endregion

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when an element with the same <paramref name="key"/> already exists in the dictionary.</exception>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            CheckMemory();
            items.Add(new WeakKeyReference(key), value);
        }

        /// <summary>
        /// Determines whether the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> contains the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.</param>
        /// <returns><c>true</c> if the key was located in the dictionary; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is <c>null</c>.</exception>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return items.ContainsKey(new WeakKeyReference(key));
        }

        /// <summary>
        /// Removes the value with the specified <paramref name="key"/> from the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>.
        /// This method returns false if the key is not found in the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return items.Remove(new WeakKeyReference(key));
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key was found;
        /// otherwise, the default value for the type of the value parameter.  This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> contains an element with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is <c>null</c>.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return items.TryGetValue(new WeakKeyReference(key), out value);
        }

        /// <summary>
        /// Removes all keys and values from the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Scans the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/> for dead references and removes any that are found.
        /// </summary>
        public void PurgeDeadKeys()
        {
            var deadKeys = items.Keys.Where(k => !k.IsAlive).ToArray();
            foreach (var key in deadKeys)
            {
                items.Remove(key);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="WeakKeyDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(items);
        }

        #region Extra Interface Members
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                var deadKeys = new List<WeakKeyReference>();
                var liveKeys = new List<TKey>();

                foreach (var key in items.Keys)
                {
                    if (key.IsAlive)
                    {
                        liveKeys.Add(key.Target);
                    }
                    else
                    {
                        deadKeys.Add(key);
                    }
                }

                try
                {
                    foreach (var key in deadKeys)
                    {
                        items.Remove(key);
                    }
                }
                // if this was called during an enumeration
                catch (InvalidOperationException) { }

                return liveKeys;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException("key");
            }

            CheckMemory();
            items.Add(new WeakKeyReference(item.Key), item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException("key");
            }

            TValue value;
            items.TryGetValue(new WeakKeyReference(item.Key), out value);
            return value.Equals(item.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            // only purging keys we happen to come across
            var deadKeys = new List<WeakKeyReference>(items.Count - arrayIndex);

            int i = 0;
            foreach (var key in items.Keys)
            {
                if (i >= array.Length)
                {
                    break;
                }

                if (!key.IsAlive)
                {
                    deadKeys[i] = key;
                }
                else
                {
                    if (i >= arrayIndex)
                    {
                        array[i - arrayIndex] = new KeyValuePair<TKey, TValue>(key.Target, items[key]);
                    }
                    i++;
                }
            }

            try
            {
                foreach (var key in deadKeys)
                {
                    items.Remove(key);
                }
            }
            // if this method was called during an enumeration
            catch (InvalidOperationException) { }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException("key");
            }

            return items.Remove(new WeakKeyReference(item.Key));
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(items);
        }
        #endregion

        private void CheckMemory()
        {
            long memory = GC.GetTotalMemory(false);
            if (memory < lastMemoryMarker)
            {
                // a garbage collection has occurred.  we may have dead keys to purge
                PurgeDeadKeys();
            }
            lastMemoryMarker = memory;
        }

        private struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator, IEnumerator, IDisposable
        {
            private readonly IEnumerator<KeyValuePair<WeakKeyReference, TValue>> enumerator;
            private readonly IDictionary<WeakKeyReference, TValue> dictionary;
            private readonly List<WeakKeyReference> deadKeys;

            public Enumerator(IDictionary<WeakKeyReference, TValue> dictionary)
            {
                this.dictionary = dictionary;
                this.enumerator = dictionary.GetEnumerator();
                this.deadKeys = new List<WeakKeyReference>();
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get { return new KeyValuePair<TKey, TValue>(enumerator.Current.Key.Target, enumerator.Current.Value); }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        return false;
                    }

                    if (enumerator.Current.Key.IsAlive)
                    {
                        break;
                    }

                    // only purging keys we happen to come across
                    deadKeys.Add(enumerator.Current.Key);
                }

                return true;
            }

            public void Reset()
            {
                enumerator.Reset();
            }

            public void Dispose()
            {
                enumerator.Dispose();
                foreach (var deadKey in deadKeys)
                {
                    dictionary.Remove(deadKey);
                }
                deadKeys.Clear();
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get { return new DictionaryEntry(enumerator.Current.Key.Target, enumerator.Current.Value); }
            }

            object IDictionaryEnumerator.Key
            {
                get { return enumerator.Current.Key.Target; }
            }

            object IDictionaryEnumerator.Value
            {
                get { return enumerator.Current.Value; }
            }
        }

        private class WeakKeyReference : WeakReference
        {
            public int HashCode;

            public new TKey Target
            {
                get { return (TKey)base.Target; }
                set { base.Target = value; }
            }

            public WeakKeyReference(TKey key)
                : base(key)
            {
                HashCode = key == null ? 0 : key.GetHashCode();
            }

            public override int GetHashCode()
            {
                if (IsAlive)
                {
                    HashCode = Target.GetHashCode();
                }

                return HashCode;
            }

            public override bool Equals(object obj)
            {
                var weak = obj as WeakReference;
                if (Target == null)
                {
                    return object.Equals(Target, weak == null ? obj : weak.Target);
                }
                return Target.Equals(weak == null ? obj : weak.Target);
            }

            public override string ToString()
            {
                return Target == null ? "<null>" : Target.ToString();
            }
        }

        private class WeakReferenceComparer : IEqualityComparer<WeakKeyReference>
        {
            private IEqualityComparer<TKey> comparer;

            public WeakReferenceComparer(IEqualityComparer<TKey> comparer)
            {
                this.comparer = comparer ?? EqualityComparer<TKey>.Default;
            }

            public bool Equals(WeakKeyReference x, WeakKeyReference y)
            {
                if (x == null || y == null)
                {
                    return x == y;
                }

                if (!x.IsAlive)
                {
                    return y.IsAlive ? false : comparer.Equals(x.Target, x.Target);
                }

                if (!y.IsAlive)
                {
                    return false;
                }

                return comparer.Equals(x.Target, y.Target);
            }

            public int GetHashCode(WeakKeyReference obj)
            {
                if (obj == null)
                {
                    return comparer.GetHashCode(default(TKey));
                }

                if (!obj.IsAlive) return obj.HashCode;

                try
                {
                    obj.HashCode = comparer.GetHashCode(obj.Target);
                }
                catch (Exception)
                {
                    return comparer.GetHashCode(default(TKey));
                }

                return obj.HashCode;
            }
        }
    }
}
