using System;

namespace MonoCross.Utilities.Serialization
{
    /// <summary>
    /// Represents a factory for creating object serializers.
    /// </summary>
    public static class SerializerFactory
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <returns></returns>
        public static ISerializer<T> Create<T>()
        {
            // default ISerializer to XML
            return Create<T>(SerializationFormat.XML, null, null);
        }

        /// <summary>
        /// Creates an ISerializer instance using the specified aux types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="auxTypes">The aux types.</param>
        /// <returns></returns>
        public static ISerializer<T> Create<T>(Type[] auxTypes)
        {
            // default ISerializer to XML
            return Create<T>(SerializationFormat.XML, auxTypes);
        }

        /// <summary>
        /// Creates an ISerializer instance using the specified format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static ISerializer<T> Create<T>(SerializationFormat format)
        {
            return Create<T>(format, null, null);
        }
        /// <summary>
        /// Creates an ISerializer instance using the specified format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format">The format.</param>
        /// <param name="customSerializerType">Type of the custom serializer.</param>
        /// <returns></returns>
        public static ISerializer<T> Create<T>(SerializationFormat format, Type customSerializerType)
        {
            // default ISerializer to XML
            return Create<T>(format, null, customSerializerType);
        }

        /// <summary>
        /// Creates an ISerializer instance using the specified format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format">The format.</param>
        /// <param name="auxTypes">The aux types.</param>
        /// <returns></returns>
        public static ISerializer<T> Create<T>(SerializationFormat format, Type[] auxTypes)
        {
            return Create<T>(format, auxTypes, null);
        }
        /// <summary>
        /// Creates an ISerializer instance using the specified format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format">The format.</param>
        /// <param name="auxTypes">The aux types.</param>
        /// <param name="customSerializerType">Type of the custom serializer.</param>
        /// <returns></returns>
        public static ISerializer<T> Create<T>(SerializationFormat format, Type[] auxTypes, Type customSerializerType)
        {
            //TODO: consider adding an identity map to cache serializers by type T.
            switch (format)
            {
                case SerializationFormat.CUSTOM:
                    if (customSerializerType == null)
                        throw new ArgumentException("CustomSerializerType must be provided when SerializationFormat is CUSTOM");
                    var instance = Activator.CreateInstance(customSerializerType);
                    if (instance is ISerializer<T>)
                        return (ISerializer<T>)instance;
                    throw new ArgumentException(customSerializerType.FullName + " does not implement iSerializer<T>");
                case SerializationFormat.XML:
                    return auxTypes != null ? new SerializerXml<T>(auxTypes) : new SerializerXml<T>();
                case SerializationFormat.JSON:
                    return new SerializerJson<T>();
                case SerializationFormat.ODATA:
                    return new SerializerOdata<T>();
            }
            return null;
        }
    }
}