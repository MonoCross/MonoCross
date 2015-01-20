using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Represents a mapping of abstract types to native class types to each platform that the abstract types represent.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
    public class NamedTypeMap : IDictionary<Type, Type>
    {
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif

        private readonly IDictionary<NamedType, TypeLoader> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedTypeMap"/> class.
        /// </summary>
        public NamedTypeMap()
        {
            _items = new Dictionary<NamedType, TypeLoader>();
        }

        /// <summary>
        /// Adds an entry with the specified abstract type and class type to the collection.
        /// </summary>
        /// <param name="abstractType">The type of the abstract to associate with the class type.</param>
        /// <param name="concreteType">The type of the class to associate with the abstract type.</param>
        public void Add(Type abstractType, Type concreteType)
        {
            Add(null, abstractType, concreteType);
        }

        /// <summary>
        /// Adds an entry with the specified abstract type and class type to the collection.
        /// </summary>
        /// <param name="name">A unique identifier for the abstract type.</param>
        /// <param name="abstractType">The type of the abstract to associate with the class type.</param>
        /// <param name="concreteType">The type of the class to associate with the abstract type.</param>
        public void Add(string name, Type abstractType, Type concreteType)
        {
            Add(name, abstractType, new TypeLoader(concreteType));
        }

        /// <summary>
        /// Adds an entry with the specified abstract type and class type to the collection.
        /// </summary>
        /// <param name="name">A unique identifier for the abstract type.</param>
        /// <param name="abstractType">The type of the abstract to associate with the class type.</param>
        /// <param name="typeLoader">The type loader to associate with the abstract type.</param>
        public void Add(string name, Type abstractType, TypeLoader typeLoader)
        {
            CheckEntry(ref name, abstractType, typeLoader.Type);
            _items.Add(new NamedType(abstractType, name), typeLoader);
        }

        /// <summary>
        /// Gets the number of entries contained in the collection.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Determines whether the collection contains an entry with the specified abstract type.
        /// </summary>
        /// <param name="abstractType">The abstract type to locate in the collection.</param>
        /// <returns><c>true</c> if the abstract type was located in the collection; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(Type abstractType)
        {
            return ContainsKey(abstractType, null);
        }

        /// <summary>
        /// Determines whether the collection contains an entry with the specified abstract type.
        /// </summary>
        /// <param name="abstractType">The abstract type to locate in the collection.</param>
        /// <param name="name">A unique identifier for the abstract type.</param>
        /// <returns><c>true</c> if the abstract type was located in the collection; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(Type abstractType, string name)
        {
            return _items.ContainsKey(new NamedType(abstractType, name));
        }

        /// <summary>
        /// Removes all custom entries from the collection.
        /// </summary>
        public virtual void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Removes the entry with the specified abstract type from the collection.
        /// </summary>
        /// <param name="abstractType">The abstract type of the entry to remove.</param>
        /// <returns><c>true</c> if the entry was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(Type abstractType)
        {
            return Remove(abstractType, null);
        }

        /// <summary>
        /// Removes the entry with the specified abstract type from the collection.
        /// </summary>
        /// <param name="abstractType">The abstract type of the entry to remove.</param>
        /// <param name="name">A unique identifier for the abstract type.</param>
        /// <returns><c>true</c> if the entry was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(Type abstractType, string name)
        {
            return _items.Remove(new NamedType(abstractType, name));
        }

        /// <summary>
        /// Gets the class type associated with the specified abstract type.
        /// </summary>
        /// <param name="abstractType">The abstract type whose associated class type to get.</param>
        /// <param name="classType">When the method returns, the class type associated with the specified abstract type, if the abstract type was found; otherwise, <c>null</c>."/></param>
        /// <returns><c>true</c> if the class type was successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(Type abstractType, out Type classType)
        {
            return TryGetValue(abstractType, null, out classType);
        }

        /// <summary>
        /// Gets the class type associated with the specified abstract type.
        /// </summary>
        /// <param name="abstractType">The abstract type whose associated class type to get.</param>
        /// <param name="name">A unique identifier for the abstract type.</param>
        /// <param name="classType">When the method returns, the class type associated with the specified abstract type, if the abstract type was found; otherwise, <c>null</c>."/></param>
        /// <returns><c>true</c> if the class type was successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(Type abstractType, string name, out Type classType)
        {
            TypeLoader loader;
            var retval = _items.TryGetValue(new NamedType(abstractType, name), out loader);
            classType = loader.Type;
            return retval;
        }

        /// <summary>
        /// Gets the class type associated with the specified abstract type.
        /// </summary>
        /// <param name="abstractType">The abstract type whose associated class type to get.</param>
        /// <param name="instance">When the method returns, the instance associated with the specified abstract type, if the abstract type was found; otherwise, <c>null</c>."/></param>
        /// <returns><c>true</c> if the class type was successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetInstance(Type abstractType, out object instance)
        {
            return TryGetInstance(abstractType, null, out instance);
        }

        /// <summary>
        /// Gets the class type associated with the specified abstract type.
        /// </summary>
        /// <param name="abstractType">The abstract type whose associated class type to get.</param>
        /// <param name="name">A unique identifier for the abstract type.</param>
        /// <param name="instance">When the method returns, the instance associated with the specified abstract type, if the abstract type was found; otherwise, <c>null</c>."/></param>
        /// <returns><c>true</c> if the class type was successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetInstance(Type abstractType, string name, out object instance)
        {
            TypeLoader loader;
            var retval = _items.TryGetValue(new NamedType(abstractType, name), out loader);
            instance = loader.Instance;
            return retval;
        }

        /// <summary>
        /// Gets or sets the class type associated with the specified interface type.
        /// </summary>
        /// <param name="abstractType">The interface type associated with the class type to get or set.</param>
        /// <returns>The class type associated with the <paramref name="abstractType"/>.</returns>
        public Type this[Type abstractType]
        {
            get { return this[abstractType, null].Type; }
            set { this[abstractType, null] = new TypeLoader(value); }
        }

        /// <summary>
        /// Gets or sets the class type associated with the specified interface type.
        /// </summary>
        /// <param name="abstractType">The interface type associated with the class type to get or set.</param>
        /// <param name="name">A unique identifier for the abstract type.</param>
        /// <returns>The class type associated with the <paramref name="abstractType"/>.</returns>
        public TypeLoader this[Type abstractType, string name]
        {
            get { return GetTypeLoader(abstractType, name); }
            set
            {
                CheckEntry(ref name, abstractType, value.Type);
                _items[new NamedType(abstractType, name)] = value;
            }
        }

        /// <summary>
        /// Performs type constraints on new entries.
        /// </summary>
        /// <param name="name">The entry name passed by reference so that it may be changed.</param>
        /// <param name="key">The key <see cref="Type"/>.</param>
        /// <param name="value">the value <see cref="Type"/>.</param>
        protected virtual void CheckEntry(ref string name, Type key, Type value) { }

        void ICollection<KeyValuePair<Type, Type>>.Add(KeyValuePair<Type, Type> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<Type, Type>>.Contains(KeyValuePair<Type, Type> item)
        {
            Type type;
            if (TryGetValue(item.Key, out type))
            {
                return type == item.Value;
            }
            return false;
        }

        void ICollection<KeyValuePair<Type, Type>>.CopyTo(KeyValuePair<Type, Type>[] array, int arrayIndex)
        {
            _items.Select(i => new KeyValuePair<Type, Type>(i.Key.Type, i.Value.Type)).ToList().CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<Type, Type>>.Remove(KeyValuePair<Type, Type> item)
        {
            Type type;
            return TryGetValue(item.Key, out type) &&
                type == item.Value &&
                Remove(item.Key);
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif
        bool ICollection<KeyValuePair<Type, Type>>.IsReadOnly
        {
            get { return false; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif
        ICollection<Type> IDictionary<Type, Type>.Keys
        {
            get { return _items.Keys.Select(k => k.Type).ToList(); }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif
        ICollection<Type> IDictionary<Type, Type>.Values
        {
            get { return _items.Values.Select(k => k.Type).ToList(); }
        }

        IEnumerator<KeyValuePair<Type, Type>> IEnumerable<KeyValuePair<Type, Type>>.GetEnumerator()
        {
            return _items.Select(i => new KeyValuePair<Type, Type>(i.Key.Type, i.Value.Type)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Type, Type>>)this).GetEnumerator();
        }

        private TypeLoader GetTypeLoader(Type type, string name)
        {
            TypeLoader initer;
            if (ContainsKey(type, name) && (initer = _items[new NamedType(type, name)]).Type != null)
                return initer;
            if (name != null)
            {
                if (ContainsKey(type) && (initer = _items[new NamedType(type)]).Type != null)
                    return initer;
            }

            var abstracts = type.GetTypeInfo().ImplementedInterfaces;
            initer = abstracts
                .Select(inter => GetTypeLoader(inter, name))
                .FirstOrDefault(i => i.Type != null);
            return initer;
        }

        /// <summary>
        /// Resolves the specified abstract type as a concrete instance.
        /// </summary>
        /// <param name="type">The abstract type to resolve.</param>
        /// <param name="name">An optional unique identifier for the abstract type.</param>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        /// <exception cref="TypeLoadException">Thrown if the type cannot be found in the map.</exception>
        /// <returns>The object instance.</returns>
        public object Resolve(Type type, string name, params object[] parameters)
        {
            var initer = GetTypeLoader(type, name);
            if (initer.Type == null)
            {
                throw new TypeLoadException();
            }
            var retval = initer.Load(parameters);
            _items[new NamedType(type, name)] = initer;
            return retval;
        }

        /// <summary>
        /// A key containing a type and optional name.
        /// </summary>
        protected struct NamedType : IComparable, IEqualityComparer<NamedType>
        {
            /// <summary>
            /// Initializes a new <see cref="NamedType"/> instance.
            /// </summary>
            /// <param name="type">The mapped <see cref="Type"/>.</param>
            public NamedType(Type type) : this(type, null) { }

            /// <summary>
            /// Initializes a new <see cref="NamedType"/> instance.
            /// </summary>
            /// <param name="type">The mapped <see cref="Type"/>.</param>
            /// <param name="id">An optional name describing the type.</param>
            public NamedType(Type type, string id)
            {
                if (type == null) throw new ArgumentNullException("type");

                Type = type;
                Id = id;
            }

            /// <summary>
            /// The type for this instance. This field is readonly.
            /// </summary>
            public readonly Type Type;

            /// <summary>
            /// An optional identifier. This field is readonly.
            /// </summary>
            public readonly string Id;

            #region Equality members

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                return Type == null ? -1 : Type.GetHashCode() ^ (Id == null ? 0 : Id.GetHashCode());
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>. </returns>
            public override bool Equals(object obj)
            {
                return this == (NamedType)obj;
            }

            /// <summary>
            /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
            /// </summary>
            /// <param name="obj">An object to compare with this instance.</param>
            /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.</returns>
            public int CompareTo(object obj)
            {
                var p = (NamedType)obj;
                return GetHashCode() == p.GetHashCode() ? 0 : -1;
            }

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="NamedType"/> to compare.</param>
            /// <param name="y">The second object of type <see cref="NamedType"/> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(NamedType x, NamedType y)
            {
                return x.GetHashCode() == y.GetHashCode();
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public int GetHashCode(NamedType obj)
            {
                return obj.GetHashCode();
            }

            /// <summary>
            /// Checks the specified NamedTypes for equality.
            /// </summary>
            /// <param name="a">The first <see cref="NamedType"/> to check.</param>
            /// <param name="b">The second <see cref="NamedType"/> to check.</param>
            /// <returns><c>true</c> if the parameters are equal; otherwise, <c>false</c>.</returns>
            public static bool operator ==(NamedType a, NamedType b)
            {
                return a.CompareTo(b) == 0;
            }

            /// <summary>
            /// Checks the specified NamedTypes for inequality.
            /// </summary>
            /// <param name="a">The first <see cref="NamedType"/> to check.</param>
            /// <param name="b">The second <see cref="NamedType"/> to check.</param>
            /// <returns><c>true</c> if the parameters are not equal; otherwise, <c>false</c>.</returns>
            public static bool operator !=(NamedType a, NamedType b)
            {
                return a.CompareTo(b) != 0;
            }

            #endregion
        }
    }
}