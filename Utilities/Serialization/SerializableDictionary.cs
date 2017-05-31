using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MonoCross.Utilities.Serialization
{
    /// <summary>
    /// Represents a serializable dictionary of values
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [XmlRoot( "dictionary" )]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public SerializableDictionary()

            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dictionary">A <see cref="IDictionary&lt;TKey,TValue&gt;"/> representing the Dictionary value.</param>
        public SerializableDictionary( IDictionary<TKey, TValue> dictionary )

            : base( dictionary )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="comparer">A <see cref="IEqualityComparer&lt;TKey&gt;"/> representing the Comparer value.</param>
        public SerializableDictionary( IEqualityComparer<TKey> comparer )

            : base( comparer )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">A <see cref="Int32"/> representing the Capacity value.</param>
        public SerializableDictionary( int capacity )

            : base( capacity )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dictionary">A <see cref="IDictionary&lt;TKey,TValue&gt;"/> representing the Dictionary value.</param>
        /// <param name="comparer">A <see cref="IEqualityComparer&lt;TKey&gt;"/> representing the Comparer value.</param>
        public SerializableDictionary( IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer )

            : base( dictionary, comparer )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">A <see cref="Int32"/> representing the Capacity value.</param>
        /// <param name="comparer">A <see cref="IEqualityComparer&lt;TKey&gt;"/> representing the Comparer value.</param>
        public SerializableDictionary( int capacity, IEqualityComparer<TKey> comparer )

            : base( capacity, comparer )
        {

        }

        //public SerializableDictionary( SerializationInfo info, StreamingContext context )

        //    : base( info, context )
        //{

        //}

        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml( System.Xml.XmlReader reader )
        {
            XmlSerializer keySerializer = new XmlSerializer( typeof( TKey ) );
            XmlSerializer valueSerializer = new XmlSerializer( typeof( TValue ) );

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if ( wasEmpty )
                return;

            reader.ReadStartElement( "root" );
            while ( reader.NodeType != System.Xml.XmlNodeType.EndElement )
            {
                reader.ReadStartElement( "item" );

                reader.ReadStartElement( "key" );
                TKey key = (TKey) keySerializer.Deserialize( reader );
                reader.ReadEndElement();

                reader.ReadStartElement( "value" );
                TValue value = (TValue) valueSerializer.Deserialize( reader );
                reader.ReadEndElement();

                this.Add( key, value );

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml( System.Xml.XmlWriter writer )
        {
            XmlSerializer keySerializer = new XmlSerializer( typeof( TKey ) );
            XmlSerializer valueSerializer = new XmlSerializer( typeof( TValue ) );

            writer.WriteStartElement( "root" );
            foreach ( TKey key in this.Keys )
            {
                writer.WriteStartElement( "item" );

                writer.WriteStartElement( "key" );
                keySerializer.Serialize( writer, key );
                writer.WriteEndElement();

                writer.WriteStartElement( "value" );
                TValue value = this[key];
                valueSerializer.Serialize( writer, value );

                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Creates a new dictionary from XML.
        /// </summary>
        /// <param name="filename">A <see cref="String"/> representing the Filename value.</param>
        /// <returns></returns>
        public static SerializableDictionary<TKey, TValue> CreateFromXml( string filename )
        {
            SerializableDictionary<TKey, TValue> dictionary = new SerializableDictionary<TKey, TValue>();

            FileInfo fileInfo = new FileInfo( filename );

            if ( !File.Exists( fileInfo.FullName ) )
                return null;

            using ( Stream fileStream = new FileStream( filename, FileMode.Open, FileAccess.Read ) )
            using ( XmlReader reader = XmlReader.Create( fileStream, new XmlReaderSettings()
            {
                //Encoding = Encoding.Unicode,
                ConformanceLevel = ConformanceLevel.Document
            } ) )
            {
                dictionary.ReadXml( reader );

                reader.Close();
                fileStream.Close();
            }

            return dictionary;
        }

        #endregion
    }
}
