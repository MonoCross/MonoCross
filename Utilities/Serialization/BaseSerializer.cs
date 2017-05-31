using MonoCross.Utilities;
using System.Collections.Generic;
using MonoCross.Utilities.Network;

namespace MonoCross.Utilities.Serialization
{
    /// <summary>
    /// Represents a base serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseSerializer<T>
    {
        #region Base SerializeObject Methods

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        public virtual string SerializeObject(T obj)
        {
            return SerializeObject(obj, EncryptionMode.Default);
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns></returns>
        public virtual string SerializeObject(T obj, EncryptionMode mode)
        {
            string retval = null;

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    retval = SerializeObjectClear(obj);
                    break;
                case EncryptionMode.Encryption:
                    retval = SerializeObject(obj, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    retval = Device.Encryption.Required ?
                        SerializeObject(obj, Device.Encryption.Key, Device.Encryption.Salt) :
                        SerializeObjectClear(obj);
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The obj to serialize.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns></returns>
        public virtual string SerializeObject(T obj, string key, byte[] salt)
        {
            string retval = SerializeObjectClear(obj);
            return Device.Encryption.EncryptString(retval, key, salt);
        }

        /// <summary>
        /// Serializes the object clear.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        protected abstract string SerializeObjectClear(T obj);

        #endregion

        #region Base SerializeObjectToBytes Methods

        /// <summary>
        /// Serializes the object to bytes.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        public virtual byte[] SerializeObjectToBytes(T obj)
        {
            return SerializeObjectToBytes(obj, EncryptionMode.Default);
        }

        /// <summary>
        /// Serializes the object to bytes.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="mode">The encription mode of the serialization.</param>
        /// <returns></returns>
        public virtual byte[] SerializeObjectToBytes(T obj, EncryptionMode mode)
        {
            byte[] bytes = null;

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    bytes = SerializeObjectToBytesClear(obj);
                    break;
                case EncryptionMode.Encryption:
                    bytes = SerializeObjectToBytes(obj, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    bytes = Device.Encryption.Required ?
                        SerializeObjectToBytes(obj, Device.Encryption.Key, Device.Encryption.Salt) :
                        SerializeObjectToBytesClear(obj);
                    break;
            }

            return bytes;
        }

        /// <summary>
        /// Serializes the object to bytes.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns></returns>
        public virtual byte[] SerializeObjectToBytes(T obj, string key, byte[] salt)
        {
            return Device.Encryption.EncryptBytes(SerializeObjectToBytesClear(obj), key, salt);
        }

        /// <summary>
        /// Serializes the object to bytes clear.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        protected abstract byte[] SerializeObjectToBytesClear(T obj);

        #endregion

        #region Base SerializeList Methods

        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        public virtual string SerializeList(List<T> list)
        {
            return SerializeList(list, EncryptionMode.Default);
        }

        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="mode">The encryption mode of the serialization.</param>
        /// <returns></returns>
        public virtual string SerializeList(List<T> list, EncryptionMode mode)
        {
            string retval = null;

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    retval = SerializeListClear(list);
                    break;
                case EncryptionMode.Encryption:
                    retval = SerializeList(list, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    retval = Device.Encryption.Required ?
                        SerializeList(list, Device.Encryption.Key, Device.Encryption.Salt) :
                        SerializeListClear(list);
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns></returns>
        public virtual string SerializeList(List<T> list, string key, byte[] salt)
        {
            string retval = SerializeListClear(list);
            return Device.Encryption.EncryptString(retval, key, salt);
        }

        /// <summary>
        /// Serializes the list clear.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        protected abstract string SerializeListClear(List<T> list);

        #endregion

        #region Base SerializeListToBytes Methods

        /// <summary>
        /// Serializes the list to bytes.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        public virtual byte[] SerializeListToBytes(List<T> list)
        {
            return SerializeListToBytes(list, EncryptionMode.Default);
        }

        /// <summary>
        /// Serializes the list to bytes.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns></returns>
        public virtual byte[] SerializeListToBytes(List<T> list, EncryptionMode mode)
        {
            byte[] bytes = null;

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    bytes = SerializeListToBytesClear(list);
                    break;
                case EncryptionMode.Encryption:
                    bytes = SerializeListToBytes(list, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    bytes = Device.Encryption.Required ?
                        SerializeListToBytes(list, Device.Encryption.Key, Device.Encryption.Salt) :
                        SerializeListToBytesClear(list);
                    break;
            }

            return bytes;
        }

        /// <summary>
        /// Serializes the list to bytes.
        /// </summary>
        /// <param name="list">The list to serialized.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns>The serialized list as an encrypted byte array.</returns>
        public virtual byte[] SerializeListToBytes(List<T> list, string key, byte[] salt)
        {
            byte[] bytes = SerializeListToBytesClear(list);
            return Device.Encryption.EncryptBytes(bytes, key, salt);
        }

        /// <summary>
        /// Serializes the list to bytes clear.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns>The serialized list as a byte array.</returns>
        protected abstract byte[] SerializeListToBytesClear(List<T> list);

        #endregion

        #region Base DeserializeObject Methods

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        public virtual T DeserializeObject(string value)
        {
            return DeserializeObject(value, EncryptionMode.Default);
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        public virtual T DeserializeObject(string value, EncryptionMode mode)
        {
            T retval = default(T);

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    retval = DeserializeObjectClear(value);
                    break;
                case EncryptionMode.Encryption:
                    retval = DeserializeObject(value, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    retval = Device.Encryption.Required ?
                        DeserializeObject(value, Device.Encryption.Key, Device.Encryption.Salt) :
                        DeserializeObjectClear(value);
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        public virtual T DeserializeObject(string value, string key, byte[] salt)
        {
            string retval = Device.Encryption.DecryptString(value, key, salt);
            return DeserializeObjectClear(retval);
        }

        /// <summary>
        /// Deserializes the object clear.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        protected abstract T DeserializeObjectClear(string value);

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        public virtual T DeserializeObject(byte[] value)
        {
            return DeserializeObject(value, EncryptionMode.Default);
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        public virtual T DeserializeObject(byte[] value, EncryptionMode mode)
        {
            T retval = default(T);

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    retval = DeserializeObjectClear(value);
                    break;
                case EncryptionMode.Encryption:
                    retval = DeserializeObject(value, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    retval = Device.Encryption.Required ?
                        DeserializeObject(value, Device.Encryption.Key, Device.Encryption.Salt) :
                        DeserializeObjectClear(value);
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        public virtual T DeserializeObject(byte[] value, string key, byte[] salt)
        {
            byte[] retval = Device.Encryption.DecryptBytes(value, key, salt);
            return DeserializeObjectClear(retval);
        }

        /// <summary>
        /// Deserializes the object clear.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        protected virtual T DeserializeObjectClear(byte[] value)
        {
            if (value == null)
                return default(T);

            string retval = NetworkUtils.ByteArrayToStr(value);

            return DeserializeObjectClear(retval);
        }

        #endregion

        #region Base DeserializeList Methods

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        public virtual List<T> DeserializeList(string value)
        {
            return DeserializeList(value, EncryptionMode.Default);
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        public virtual List<T> DeserializeList(string value, EncryptionMode mode)
        {
            List<T> retval = default(List<T>);

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    retval = DeserializeListClear(value);
                    break;
                case EncryptionMode.Encryption:
                    retval = DeserializeList(value, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    retval = Device.Encryption.Required ?
                        DeserializeList(value, Device.Encryption.Key, Device.Encryption.Salt) :
                        DeserializeListClear(value);
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        public virtual List<T> DeserializeList(string value, string key, byte[] salt)
        {
            string retval = Device.Encryption.DecryptString(value, key, salt);
            return DeserializeListClear(retval);
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        public virtual List<T> DeserializeList(byte[] value)
        {
            return DeserializeList(value, EncryptionMode.Default);
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        public virtual List<T> DeserializeList(byte[] value, EncryptionMode mode)
        {
            List<T> retval = default(List<T>);

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    retval = DeserializeListClear(value);
                    break;
                case EncryptionMode.Encryption:
                    retval = DeserializeList(value, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    retval = Device.Encryption.Required ?
                        DeserializeList(value, Device.Encryption.Key, Device.Encryption.Salt) :
                        DeserializeListClear(value);
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        public virtual List<T> DeserializeList(byte[] value, string key, byte[] salt)
        {
            byte[] retval = Device.Encryption.DecryptBytes(value, key, salt);
            return DeserializeListClear(retval);
        }

        /// <summary>
        /// Deserializes the list clear.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        protected abstract List<T> DeserializeListClear(string value);

        /// <summary>
        /// Deserializes the list clear.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <returns>A deserialized <see cref="List&lt;T&gt;"/> instance.</returns>
        protected virtual List<T> DeserializeListClear(byte[] value)
        {
            if (value == null)
                return default(List<T>);

            string retval = NetworkUtils.ByteArrayToStr(value);

            return DeserializeListClear(retval);
        }

        #endregion
    }
}
