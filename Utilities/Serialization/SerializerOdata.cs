using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using MonoCross.Utilities;
using MonoCross.Utilities.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MonoCross.Utilities.Serialization
{
    /// <summary>
    /// The JSON serializer utility.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerializerOdata<T> : BaseSerializer<T>, ISerializer<T>
    {
        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType
        {
            get
            {
                return "application/json";
            }
        }

        #region Serialize Object Methods

        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="filename">The filename for the serialized object.</param>
        public void SerializeObjectToFile(T obj, string filename)
        {
            DateTime dtMetric = DateTime.UtcNow;
            Device.File.Save(filename, ToJson(obj));
            Device.Log.Metric(string.Format("SerializerOdata.SerializeObjectToFile(1): File {0}  Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Serializes the object to file.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="filename">The filename of the serialized object.</param>
        /// <param name="mode">The encryption mode for the serialization.</param>
        public void SerializeObjectToFile(T obj, string filename, EncryptionMode mode)
        {
            DateTime dtMetric = DateTime.UtcNow;
            Device.File.Save(filename, ToJson(obj), mode);
            Device.Log.Metric(string.Format("SerializerOdata.SerializeObjectToFile(2): File {0}  Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
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
            DateTime dtMetric = DateTime.UtcNow;
            Device.File.Save(filename, ToJson(obj), key, salt);
            Device.Log.Metric(string.Format("SerializerOdata.SerializeObjectToFile(3): File {0}  Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Serializes the object clear.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        protected override string SerializeObjectClear(T obj)
        {
            DateTime dtMetric = DateTime.UtcNow;
            string retval = ToJson(obj);
            Device.Log.Metric(string.Format("SerializerOdata.SerializeObjectClear: Type: {0} Size: {1} Time: {2:0} milliseconds", obj.GetType(), (string.IsNullOrEmpty(retval) ? 0 : retval.Length), DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
            return retval;
        }

        /// <summary>
        /// Serializes the object to bytes clear.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        protected override byte[] SerializeObjectToBytesClear(T obj)
        {
            DateTime dtMetric = DateTime.UtcNow;
            byte[] bytes = NetworkUtils.StrToByteArray(ToJson(obj));
            Device.Log.Metric(string.Format("SerializerOdata.SerializeObjectToBytesClear: Time: {0} milliseconds", DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
            return bytes;
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
        /// <param name="key">The encrtyption key.</param>
        /// <param name="salt">The enctryption salt.</param>
        /// <returns></returns>
        public T DeserializeObjectFromFile(string filename, string key, byte[] salt)
        {
            return DeserializeObject(Device.File.Read(filename, key, salt));
        }

        /// <summary>
        /// Deserializes the object clear.
        /// </summary>
        /// <param name="value">The serialized object value.</param>
        /// <returns></returns>
        protected override T DeserializeObjectClear(string value)
        {
            DateTime dtMetric = DateTime.UtcNow;

            T obj = default(T);
            if (string.IsNullOrEmpty(value))
                return obj;

            //obj = FromJson(Encoding.UTF8.GetBytes(value));
            obj = FromJson(value);

            Device.Log.Metric(string.Format("SerializerOdata.DeserializeObject: Type: {0} Size: {1} Time: {2:0} milliseconds", obj.GetType(), value.Length, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return obj;
        }

        #endregion

        #region Serialize List Methods

        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="filename">The filename.</param>
        public void SerializeListToFile(List<T> list, string filename)
        {
            DateTime dtMetric = DateTime.UtcNow;
            Device.File.Save(filename, ListToJson(list, string.Empty));
            Device.Log.Metric(string.Format("SerializerOdata.SerializeListToFile(1): File: {0}  Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <param name="filename">The filename of the serialized list.</param>
        /// <param name="mode">The encryption mode.</param>
        public void SerializeListToFile(List<T> list, string filename, EncryptionMode mode)
        {
            DateTime dtMetric = DateTime.UtcNow;
            Device.File.Save(filename, ListToJson(list, string.Empty), mode);
            Device.Log.Metric(string.Format("SerializerOdata.SerializeListToFile(2): File: {0}  Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Serializes the list to file.
        /// </summary>
        /// <param name="list">The list to serialzed.</param>
        /// <param name="filename">The filename of the serialized list.</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="salt">The encryption salt.</param>
        public void SerializeListToFile(List<T> list, string filename, string key, byte[] salt)
        {
            DateTime dtMetric = DateTime.UtcNow;
            Device.File.Save(filename, ListToJson(list, string.Empty), key, salt);
            Device.Log.Metric(string.Format("SerializerOdata.SerializeListToFile(3): File: {0}  Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Serializes the list to bytes clear.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        protected override byte[] SerializeListToBytesClear(List<T> list)
        {
            DateTime dtMetric = DateTime.UtcNow;
            byte[] bytes = Encoding.UTF8.GetBytes(ListToJson(list, string.Empty));
            Device.Log.Metric(string.Format("SerializerOdata.SerializeListToBytes: Time: {0} milliseconds", DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
            return bytes;
        }

        /// <summary>
        /// Serializes the list clear.
        /// </summary>
        /// <param name="list">The list to serialize.</param>
        /// <returns></returns>
        protected override string SerializeListClear(List<T> list)
        {
            DateTime dtMetric = DateTime.UtcNow;
            string retval = ListToJson(list, string.Empty);
            Device.Log.Metric(string.Format("SerializerOdata.SerializeList: Type: {0} Size: {1} Time: {2:0} milliseconds", list.GetType(), (string.IsNullOrEmpty(retval) ? 0 : retval.Length), DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
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
            DateTime dtMetric = DateTime.UtcNow;

            if (string.IsNullOrEmpty(value))
                return null;

            List<T> list = ListFromJson(value);

            Device.Log.Metric(string.Format("SerializerJson.DeserializeListClear: Type: {0} Size: {1} Time: {2:0} milliseconds", list.GetType(), value.Length, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return list;
        }

        #endregion

        /// <summary>
        /// Serializes the specified object into a JSON string.
        /// </summary>
        /// <param name="model">The <typeparamref name="T"/> to serialize.</param>
        /// <returns>A JSON <see cref="string"/> that contains the serialized <typeparamref name="T"/>.</returns>
        protected virtual string ToJson(T model)
        {
            string json = null;

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IsoDateTimeConverter());
            using (TextWriter writer = new StringWriter())
            {
                JsonSerializer.Create(settings).Serialize(writer, model);
                json = writer.ToString();
            }
            Device.Log.Debug(json);
            return json;
        }

        /// <summary>
        /// Deserializes an object from the specified JSON string.
        /// </summary>
        /// <param name="json">A JSON <see cref="string"/> that contains the serialized object.</param>
        /// <returns>The deserialized <typeparamref name="T"/>.</returns>
        protected virtual T FromJson(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Serializes the specified list into a JSON string.
        /// </summary>
        /// <param name="model">The <see cref="List&lt;T&gt;"/> to serialize.</param>
        /// <param name="meta">The ODATA metadata string to include in the serialized object.</param>
        /// <returns>A JSON <see cref="string"/> that contains the serialized list.</returns>
        protected virtual string ListToJson(List<T> model, string meta)
        {
            string json = null;

            // serialize the object to xml string.
            var odmodel = new ODataList<T> { Value = model.ToArray(), Metadata = meta };

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IsoDateTimeConverter());
            using (TextWriter writer = new StringWriter())
            {
                JsonSerializer.Create(settings).Serialize(writer, odmodel);
                json = writer.ToString();
            }
            return json;
        }

        /// <summary>
        /// Deserializes a list from the specified JSON string.
        /// </summary>
        /// <param name="json">A JSON <see cref="string"/> that contains the serialized list.</param>
        /// <returns>The deserialized <see cref="List&lt;T&gt;"/>.</returns>
        protected virtual List<T> ListFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ODataList<T>>(json).List;
        }
    }

    [DataContract]
    internal class ODataList<T>
    {
        [DataMember(Name = "odata.metadata")]
        internal string Metadata { get; set; }

        [DataMember(Name = "value")]
        internal T[] Value { get; set; }

        internal List<T> List
        {
            get
            {
                var retval = new List<T>();
                retval.AddRange(Value);
                return retval;
            }
        }
    }
}
