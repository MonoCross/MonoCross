using System.Collections.Generic;
using MonoCross.Utilities;

namespace MonoCross.Utilities.Serialization
{
    /// <summary>
    /// Defines the MonoCross abstract serialization utility.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISerializer<T>
    {
        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="filename">The filename.</param>
        void SerializeObjectToFile( T obj, string filename );
        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The mode.</param>
        void SerializeObjectToFile( T obj, string filename, EncryptionMode mode );
        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        void SerializeObjectToFile( T obj, string filename, string key, byte[] salt );

        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="filename">The filename.</param>
        void SerializeListToFile( List<T> list, string filename );
        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The mode.</param>
        void SerializeListToFile( List<T> list, string filename, EncryptionMode mode );
        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        void SerializeListToFile( List<T> list, string filename, string key, byte[] salt );

        /// <summary>
        /// Serializes the object to bytes.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        byte[] SerializeObjectToBytes( T obj );
        /// <summary>
        /// Serializes the object to bytes.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        byte[] SerializeObjectToBytes( T obj, EncryptionMode mode );
        /// <summary>
        /// Serializes the object to bytes.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        byte[] SerializeObjectToBytes( T obj, string key, byte[] salt );

        /// <summary>
        /// Serializes the list to bytes.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        byte[] SerializeListToBytes( List<T> list );
        /// <summary>
        /// Serializes the list to bytes.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        byte[] SerializeListToBytes( List<T> list, EncryptionMode mode );
        /// <summary>
        /// Serializes the list to bytes.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        byte[] SerializeListToBytes( List<T> list, string key, byte[] salt );

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        string SerializeObject( T obj );
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        string SerializeObject( T obj, EncryptionMode mode );
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        string SerializeObject( T obj, string key, byte[] salt );

        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        string SerializeList( List<T> list );
        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        string SerializeList( List<T> list, EncryptionMode mode );
        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        string SerializeList( List<T> list, string key, byte[] salt );

        /// <summary>
        /// Deserializes the object from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        T DeserializeObjectFromFile( string filename );
        /// <summary>
        /// Deserializes the object from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        T DeserializeObjectFromFile( string filename, EncryptionMode mode );
        /// <summary>
        /// Deserializes the object from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        T DeserializeObjectFromFile( string filename, string key, byte[] salt );

        /// <summary>
        /// Deserializes the list from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        List<T> DeserializeListFromFile( string filename );
        /// <summary>
        /// Deserializes the list from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        List<T> DeserializeListFromFile( string filename, EncryptionMode mode );
        /// <summary>
        /// Deserializes the list from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        List<T> DeserializeListFromFile( string filename, string key, byte[] salt );

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        T DeserializeObject( string value );
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        T DeserializeObject( string value, EncryptionMode mode );
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        T DeserializeObject( string value, string key, byte[] salt );

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        T DeserializeObject( byte[] value );
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        T DeserializeObject( byte[] value, EncryptionMode mode );
        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        T DeserializeObject( byte[] value, string key, byte[] salt );

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        List<T> DeserializeList( string value );
        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        List<T> DeserializeList( string value, EncryptionMode mode );
        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        List<T> DeserializeList( string value, string key, byte[] salt );

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        List<T> DeserializeList( byte[] value );
        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        List<T> DeserializeList( byte[] value, EncryptionMode mode );
        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        List<T> DeserializeList( byte[] value, string key, byte[] salt );

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        string ContentType { get; }
    }
}
