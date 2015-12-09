using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Represents a generic collection of key/value pairs with serialization logic.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IXmlSerializable
    {
        private readonly IDictionary<TKey, TValue> _internalDictionary;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class with the default equality comparer.
        /// </summary>
        public SerializableDictionary()
        {
            _internalDictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class that contains elements
        /// copied from the specified <see cref="IDictionary&lt;TKey, TValue&gt;"/> and uses the default equality comparer.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> whose elements are copied to the new <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/>.</param>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _internalDictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class that uses the specified <see cref="IEqualityComparer&lt;TKey&gt;"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer&lt;TKey&gt;"/> to use when comparing keys.</param>
        public SerializableDictionary(IEqualityComparer<TKey> comparer)
        {
            _internalDictionary = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> can contain.</param>
        public SerializableDictionary(int capacity)
        {
            _internalDictionary = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class that contains elements
        /// copied from the specified <see cref="IDictionary&lt;TKey, TValue&gt;"/> and uses the specified equality comparer.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> whose elements are copied to the new <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer&lt;TKey&gt;"/> to use when comparing keys.</param>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _internalDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class with the specified capacity
        /// and that uses the specified equality comparer.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> can contain.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer&lt;TKey&gt;"/> to use when comparing keys.</param>
        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _internalDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }
        #endregion

        #region IEnumerable implementation
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }
        #endregion

        #region IEnumerable implementation
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }
        #endregion

        #region ICollection implementation
        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            _internalDictionary.Add(item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public virtual void Clear()
        {
            _internalDictionary.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The item to locate in the collection.</param>
        /// <returns><c>true</c> if the item is found in the collection; otherwise <c>false</c>.</returns>
        public virtual bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _internalDictionary.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the collection to an <see cref="System.Array"/>, starting at a particular index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from the collection.</param>
        /// <param name="arrayIndex">The index in the array at which copying begins.</param>
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _internalDictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was successfully removed from the collection; otherwise <c>false</c>.</returns>
        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _internalDictionary.Remove(item);
        }

        /// <summary>
        /// Copies the elements of the collection to an <see cref="System.Array"/>, starting at a particular index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from the collection.</param>
        /// <param name="index">The index in the array at which copying begins.</param>
        public virtual void CopyTo(Array array, int index)
        {
            ((ICollection)_internalDictionary).CopyTo(array, index);
        }

        /// <summary>
        /// Gets whether this instance is synchronized across threads.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets an object that can be used for synchronization.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the number of elements contained within the collection.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return _internalDictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public virtual bool IsReadOnly
        {
            get
            {
                return _internalDictionary.IsReadOnly;
            }
        }
        #endregion

        #region IDictionary implementation
        /// <summary>
        /// Adds an element with the provided key and value to the dictionary.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public virtual void Add(TKey key, TValue value)
        {
            _internalDictionary.Add(key, value);
        }

        /// <summary>
        /// Determines whether the dictionary contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns><c>true</c> if the dictionary contains an element with the key; otherwise <c>false</c>.</returns>
        public virtual bool ContainsKey(TKey key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the element with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element was successfully removed from the dictionary; otherwise <c>false</c>.</returns>
        public virtual bool Remove(TKey key)
        {
            return _internalDictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When the method returns, the value of the specified key.</param>
        /// <returns><c>true</c> if the dictionary contains an element with the key; otherwise <c>false</c>.</returns>
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return _internalDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        public virtual TValue this[TKey key]
        {
            get
            {
                return _internalDictionary[key];
            }
            set
            {
                _internalDictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets a collection containing the keys of the dictionary.
        /// </summary>
        public virtual ICollection<TKey> Keys
        {
            get
            {
                return _internalDictionary.Keys;
            }
        }

        /// <summary>
        /// Gets a collection containing the values of the dictionary.
        /// </summary>
        public virtual ICollection<TValue> Values
        {
            get
            {
                return _internalDictionary.Values;
            }
        }
        #endregion

        #region IDictionary implementation
        /// <summary>
        /// Adds an element with the provided key and value to the dictionary.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public virtual void Add(object key, object value)
        {
            _internalDictionary.Add((TKey)key, (TValue)value);
        }

        /// <summary>
        /// Determines whether the dictionary contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns><c>true</c> if the dictionary contains an element with the key; otherwise <c>false</c>.</returns>
        public virtual bool Contains(object key)
        {
            return _internalDictionary.ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_internalDictionary).GetEnumerator();
        }

        /// <summary>
        /// Removes the element with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public virtual void Remove(object key)
        {
            _internalDictionary.Remove((TKey)key);
        }

        /// <summary>
        /// Gets whether this instance is of a fixed size and cannot be resized to fit more elements.
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (key is TKey && _internalDictionary.ContainsKey((TKey)key))
                {
                    return _internalDictionary[(TKey)key];
                }
                return null;
            }
            set
            {
                if (key is TKey && value is TValue)
                    _internalDictionary[(TKey)key] = (TValue)value;
            }
        }

        ICollection IDictionary.Keys
        {
            get { return (ICollection)_internalDictionary.Keys; }
        }

        ICollection IDictionary.Values
        {
            get { return (ICollection)_internalDictionary.Values; }
        }

        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates a dictionary element from the XML that is at the current position of the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the element is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmptyElement = reader.IsEmptyElement;
            reader.Read();
            if (wasEmptyElement)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                var key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                var obj = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadEndElement(); // item

                Add(key, obj);
            }

            reader.Read(); //closing tag for serialized property name
        }

        /// <summary>
        /// Converts an element into its XML representation and writes it to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey index in Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, index);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                TValue obj = this[index];
                valueSerializer.Serialize(writer, obj);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        #endregion

        /// <summary>
        /// Implicitly converts a <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> instance to a <see cref="Dictionary&lt;TKey, TValue&gt;"/> instance.
        /// </summary>
        /// <param name="d">The <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> instance to convert.</param>
        public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> d)
        {
            return (Dictionary<TKey, TValue>)d._internalDictionary;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Dictionary&lt;TKey, TValue&gt;"/> instance to a <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> instance.
        /// </summary>
        /// <param name="d">The <see cref="Dictionary&lt;TKey, TValue&gt;"/> instance to convert.</param>
        public static implicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> d)
        {
            return d == null ? null : new SerializableDictionary<TKey, TValue>(d);
        }
    }
}