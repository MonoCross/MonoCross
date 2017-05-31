namespace MonoCross.Utilities.Serialization
{
    /// <summary>
    /// Specifies the valid serialization formats.
    /// </summary>
    public enum SerializationFormat
    {
        /// <summary>
        /// Formats serialized objects in JSON
        /// </summary>
        JSON,
        /// <summary>
        /// Formats serialized objects in XML.
        /// </summary>
        XML,
        /// <summary>
        /// Formats serialized objects in a custom defined format.
        /// </summary>
        CUSTOM,
        /// <summary>
        /// Formats serialized objects in a OData JSON light.
        /// </summary>
        ODATA,
    }
}