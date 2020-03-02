using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System;
using MonoCross.Utilities;

namespace MonoCross.Utilities.Serialization
{
    /// <summary>
    /// The XML serializer utility.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerializerXml<T> : BaseSerializer<T>, ISerializer<T>
    {
        XmlSerializer _objSerializer;
        XmlSerializer _listSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerXml&lt;T&gt;"/> class.
        /// </summary>
        public SerializerXml()
        {
            _objSerializer = new XmlSerializer(typeof(T));
            _listSerializer = new XmlSerializer(typeof(List<T>));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerXml&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="auxTypes">The aux types.</param>
        public SerializerXml(Type[] auxTypes)
        {
            _objSerializer = new XmlSerializer(typeof(T), auxTypes);
            _listSerializer = new XmlSerializer(typeof(List<T>), auxTypes);
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType
        {
            get
            {
                return "application/xml";
            }
        }

        #region Serialize Object Methods

        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="filename">The filename of the serialized object.</param>
        public void SerializeObjectToFile(T obj, string filename)
        {
            Device.File.Save(filename, SerializeObjectToBytesClear(obj));
        }

        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="filename">The filename of the serialized object.</param>
        /// <param name="mode">The encryption mode of the serialization.</param>
        public void SerializeObjectToFile(T obj, string filename, EncryptionMode mode)
        {
            Device.File.Save(filename, SerializeObjectToBytesClear(obj), mode);
        }

        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="filename">The filename of the serialized object.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        public void SerializeObjectToFile(T obj, string filename, string key, byte[] salt)
        {
            Device.File.Save(filename, SerializeObjectToBytesClear(obj), key, salt);
        }

        /// <summary>
        /// Serializes the object to bytes clear.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        protected override byte[] SerializeObjectToBytesClear(T obj)
        {
            DateTime dtMetric = DateTime.UtcNow;

            byte[] byteData = null;

            MemoryStream stream = new MemoryStream();

            //			Device.Log.Debug( "SerializeObjectToBytesClear() 1  type: "  + typeof(T).ToString() + " ToString: " + obj.ToString() );

            //XmlSerializer ser = new XmlSerializer( typeof( T ) );
            XmlWriter writer = null;

            //			Device.Log.Debug( "SerializeObjectToBytesClear() 2  type: "  +  typeof(T).ToString() + " ToString: " + obj.ToString() );

            try
            {
                writer = XmlWriter.Create(stream, new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8
                });

                //				Device.Log.Debug( "SerializeObjectToBytesClear() 3  type: "  +  typeof(T).ToString() + " ToString: " + obj.ToString() );

                _objSerializer.Serialize(writer, obj);

                //				Device.Log.Debug( "SerializeObjectToBytesClear() 4  type: "  +  typeof(T).ToString() + " ToString: " + obj.ToString() );

                byteData = stream.ToArray();

                //				Device.Log.Debug( "SerializeObjectToBytesClear() 5  type: "  +  typeof(T).ToString() + " ToString: " + obj.ToString() );
                //Encoding enc = Encoding.GetEncoding( "utf-8" );
                //string a = enc.GetString( byteData, 0, byteData.Length );
                //Device.Log.Info( a );
            }
            catch (Exception exc)
            {
                Device.Log.Error("SerializeObjectToBytesClear XML error: type " + typeof(T).ToString() + " ToString: " + obj.ToString(), exc);
                throw;
            }
            finally
            {
                //				Device.Log.Debug( "SerializeObjectToBytesClear() 6  type: "  +  typeof(T).ToString() + " ToString: " + obj.ToString() );

                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }

            Device.Log.Metric(string.Format("SerializerXml.SerializeObjectToBytes: Type: {0} Size: {1} Time: {2:0} milliseconds", obj.GetType().ToString(), byteData.Length, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return byteData;
        }

        /// <summary>
        /// Serializes the object clear.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        protected override string SerializeObjectClear(T obj)
        {
            //			Device.Log.Debug( "SerializeObjectClear() 1  type: "  +  typeof(T).ToString() + " ToString: " + obj.ToString() );
            DateTime dtMetric = DateTime.UtcNow;

            byte[] byteData = SerializeObjectToBytesClear(obj);
            //			Device.Log.Debug( "SerializeObjectClear() 2  type: "  +  typeof(T).ToString() + " ToString: " + obj.ToString() );
            string retval = Encoding.GetEncoding("utf-8").GetString(byteData, 0, byteData.Length);

            Device.Log.Metric(string.Format("SerializeObject: Type: {0} Size: {1} Time: {2:0} milliseconds", obj.GetType().ToString(), (string.IsNullOrEmpty(retval) ? 0 : retval.Length), DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return retval;
        }

        #endregion

        #region Deserialize Object Methods

        /// <summary>
        /// Deserializes the object from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public T DeserializeObjectFromFile(string filename)
        {
            return DeserializeObjectClear(Device.File.Read(filename));
        }

        /// <summary>
        /// Deserializes the object from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns></returns>
        public T DeserializeObjectFromFile(string filename, EncryptionMode mode)
        {
            return DeserializeObjectClear(Device.File.Read(filename, mode));
        }

        /// <summary>
        /// Deserializes the object from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The enryption salt.</param>
        /// <returns></returns>
        public T DeserializeObjectFromFile(string filename, string key, byte[] salt)
        {
            return DeserializeObjectClear(Device.File.Read(filename, key, salt));
        }

        /// <summary>
        /// Deserializes the object clear.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected override T DeserializeObjectClear(string value)
        {
            //			Device.Log.Debug( "DeserializeObjectClear() 1  type: "  + typeof(T).ToString() );

            DateTime dtMetric = DateTime.UtcNow;

            T obj = default(T);
            if (string.IsNullOrEmpty(value))
                return obj;

            if (value[0] != '<')
                value = value.Substring(1);

            //			Device.Log.Debug( "DeserializeObjectClear() 2  type: "  + typeof(T).ToString() );

            //XmlSerializer xmlsr = new XmlSerializer( typeof( T ) );

            StringReader reader = null;
            try
            {
                //			Device.Log.Debug( "DeserializeObjectClear() 3  type: "  + typeof(T).ToString() );
                reader = new StringReader(value);

                //			Device.Log.Debug( "DeserializeObjectClear() 4  type: "  + typeof(T).ToString() );
                obj = (T)_objSerializer.Deserialize(reader);

                //			Device.Log.Debug( "DeserializeObjectClear() 5  type: "  + typeof(T).ToString() );
            }
            catch (Exception exc)
            {
                Device.Log.Error("DeserializeObjectClear XML error: type " + typeof(T).ToString(), exc);
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    //			Device.Log.Debug( "DeserializeObjectClear() 6  type: "  + typeof(T).ToString() );
                    reader.Close();
                    reader.Dispose();
                }
            }

            //            Device.Log.Metric( string.Format( "SerializerXml.DeserializeObject: Type: {0} Size: {1} Time: {2} milliseconds", obj.GetType().ToString(), value.Length, DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );

            return obj;
        }

        #endregion

        #region Serialize List Methods

        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="filename">The filename of the serialized list.</param>
        public void SerializeListToFile(List<T> list, string filename)
        {
            Device.File.Save(filename, SerializeListToBytesClear(list));
        }

        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="filename">The filename of the serialized list.</param>
        /// <param name="mode">The encryption mode.</param>
        public void SerializeListToFile(List<T> list, string filename, EncryptionMode mode)
        {
            Device.File.Save(filename, SerializeListToBytesClear(list), mode);
        }

        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="filename">The filename of the serialized list.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        public void SerializeListToFile(List<T> list, string filename, string key, byte[] salt)
        {
            Device.File.Save(filename, SerializeListToBytesClear(list), key, salt);
        }

        /// <summary>
        /// Serializes the list to bytes clear.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        protected override byte[] SerializeListToBytesClear(List<T> list)
        {

            //			Device.Log.Debug( "SerializeListToBytesClear() 1  type: "  + typeof(T).ToString() );

            DateTime dtMetric = DateTime.UtcNow;

            byte[] byteData = null;

            MemoryStream stream = new MemoryStream();

            //			Device.Log.Debug( "SerializeListToBytesClear() 2  type: "  + typeof(T).ToString() );
            //XmlSerializer ser = new XmlSerializer( typeof( List<T> ) );
            XmlWriter writer = null;
            try
            {

                //			Device.Log.Debug( "SerializeListToBytesClear() 3  type: "  + typeof(T).ToString() );
                writer = XmlWriter.Create(stream, new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8
                });

                //			Device.Log.Debug( "SerializeListToBytesClear() 4  type: "  + typeof(T).ToString() );
                _listSerializer.Serialize(writer, list);


                //			Device.Log.Debug( "SerializeListToBytesClear() 5  type: "  + typeof(T).ToString() );
                byteData = stream.ToArray();
                //			Device.Log.Debug( "SerializeListToBytesClear() 6  type: "  + typeof(T).ToString() );
            }
            catch (Exception exc)
            {
                Device.Log.Error("SerializeListToBytesClear XML error: type " + typeof(List<T>), exc);
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }

            Device.Log.Metric(string.Format("SerializerXml.SerializeListToBytes: Type: {0} Size: {1} Time: {2:0} milliseconds", list.GetType(), byteData.Length, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return byteData;
        }

        /// <summary>
        /// Serializes the list clear.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        protected override string SerializeListClear(List<T> list)
        {

            //			Device.Log.Debug( "SerializeListClear() 1  type: "  + typeof(T).ToString() );
            DateTime dtMetric = DateTime.UtcNow;

            //			Device.Log.Debug( "SerializeListClear() 2  type: "  + typeof(T).ToString() );
            byte[] byteData = SerializeListToBytesClear(list);


            //			Device.Log.Debug( "SerializeListClear() 3  type: "  + typeof(T).ToString() );
            string retval = Encoding.GetEncoding("utf-8").GetString(byteData, 0, byteData.Length);

            Device.Log.Metric(string.Format("SerializerXml.SerializeList: Type: {0} Size: {1} Time: {2:0,0} milliseconds", list.GetType().ToString(), (string.IsNullOrEmpty(retval) ? 0 : retval.Length), DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
            return retval;
        }

        #endregion

        #region Deserialize List Methods

        /// <summary>
        /// Deserializes the list from file.
        /// </summary>
        /// <param name="filename">The filename of the serialized list.</param>
        /// <returns></returns>
        public List<T> DeserializeListFromFile(string filename)
        {
            return DeserializeListClear(Device.File.Read(filename));
        }

        /// <summary>
        /// Deserializes the list from file.
        /// </summary>
        /// <param name="filename">The filename of the serialized list.</param>
        /// <param name="mode">The encryption mode.</param>
        /// <returns></returns>
        public List<T> DeserializeListFromFile(string filename, EncryptionMode mode)
        {
            return DeserializeListClear(Device.File.Read(filename, mode));
        }

        /// <summary>
        /// Deserializes the list from file.
        /// </summary>
        /// <param name="filename">The filename of the serialized list.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        /// <returns></returns>
        public List<T> DeserializeListFromFile(string filename, string key, byte[] salt)
        {
            return DeserializeListClear(Device.File.Read(filename, key, salt));
        }

        /// <summary>
        /// Deserializes the list clear.
        /// </summary>
        /// <param name="value">The serialized list value.</param>
        /// <returns></returns>
        protected override List<T> DeserializeListClear(string value)
        {
            //			Device.Log.Debug( "DeserializeListClear() 1  type: "  + typeof(T).ToString() );

            DateTime dtMetric = DateTime.UtcNow;

            List<T> list = default(List<T>);
            if (string.IsNullOrEmpty(value))
                return list;

            if (value[0] != '<')
                value = value.Substring(1);

            //			Device.Log.Debug( "DeserializeListClear() 2  type: "  + typeof(T).ToString() );
            // XmlSerializer xmlsr = new XmlSerializer( typeof( List<T> ) );
            StringReader reader = null;
            try
            {
                //			Device.Log.Debug( "DeserializeListClear() 3  type: "  + typeof(T).ToString() );
                reader = new StringReader(value);

                //			Device.Log.Debug( "DeserializeListClear() 4  type: "  + typeof(T).ToString() );
                list = (List<T>)_listSerializer.Deserialize(reader);

                //			Device.Log.Debug( "DeserializeListClear() 5  type: "  + typeof(T).ToString() );
            }
            catch (Exception exc)
            {
                Device.Log.Error("DeserializeListClear XML error: type " + typeof(List<T>).ToString(), exc);
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    //Device.Log.Debug( "DeserializeListClear() 6  type: "  + typeof(T).ToString() );
                    reader.Close();
                    reader.Dispose();
                }
            }

            Device.Log.Metric(string.Format("SerializerXml.DeserializeList: Type: {0} Size: {1} Time: {2:0} milliseconds", list.GetType(), value.Length, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return list;
        }

        #endregion
    }
}
