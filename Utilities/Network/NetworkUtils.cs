using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Net;
using System;

namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Provides methods for handling HTTP web responses.
    /// </summary>
    public static class NetworkUtils
    {
        /// <summary>
        /// Converts the specified object into a serialized XML string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The byte array from the object.</returns>
        public static byte[] XmlSerializeObjectToBytes(object obj)
        {
            byte[] byteData = null;

            MemoryStream stream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(obj.GetType());
            XmlWriter writer = null;
            try
            {
                writer = XmlWriter.Create(stream, new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8
                });

                ser.Serialize(stream, obj);
                byteData = stream.ToArray();

                //Encoding enc = Encoding.GetEncoding( "utf-8" );
                //string a = enc.GetString( byteData, 0, byteData.Length );

            }
            finally
            {
                if (writer != null)
                    ((IDisposable)writer).Dispose();
                if (stream != null)
                {
#if NETCF
                    stream.Close();
#endif
                    stream.Dispose();
                }
            }

            return byteData;
        }

        /// <summary>
        /// Converts the specified string into a byte array using UTF8 encoding.
        /// </summary>
        /// <param name="str">The string to convert to a byte array.</param>
        /// <returns>The byte array from the string.</returns>
        public static byte[] StrToByteArray(string str)
        {
            if (str == null)
                return null;

            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// Converts the specified byte array into a string using UTF8 encoding.  If the conversion fails, null is returned.
        /// </summary>
        /// <param name="byteData">The byte array to convert to a string.</param>
        /// <returns>The string from the byte array, or null if the conversion fails.</returns>
        public static string ByteArrayToStr(byte[] byteData)
        {
            if (byteData == null)
                return null;

            try
            {
                Encoding enc = Encoding.GetEncoding("utf-8");
                return enc.GetString(byteData, 0, byteData.Length);
            }
            catch
            {
                // swallow exception if cannot convert to UTF8 string.
            }

            return null;
        }

        /// <summary>
        /// Reads data from the specified stream until the end is reached and returns the data as a byte array.
        /// An IOException is thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from.</param>
        public static byte[] StreamToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                try
                {
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        ms.Write(buffer, 0, read);
                    return ms.ToArray();
                }
                finally
                {
                    if (ms != null)
                    {
#if NETCF
                        ms.Close();
#endif
                        ms.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="WebResponse"/> object from the specified <see cref="HttpWebResponse"/>.
        /// </summary>
        /// <param name="response">The <see cref="HttpWebResponse"/> from which to create the <see cref="WebResponse"/>.</param>
        /// <returns></returns>
        public static WebResponse ExtractResponse(HttpWebResponse response)
        {
            return ExtractResponse(response, null);
        }

        /// <summary>
        /// Returns a <see cref="WebResponse"/> object from the specified <see cref="HttpWebResponse"/> and optionally saves the response bytes to a file.
        /// </summary>
        /// <param name="response">The <see cref="HttpWebResponse"/> from which to create the <see cref="WebResponse"/>.</param>
        /// <param name="filename">The name of the file to save the response bytes to, or null if saving is not desired.</param>
        /// <returns></returns>
        public static WebResponse ExtractResponse(HttpWebResponse response, string filename)
        {
            WebResponse webResponse = null;
            Stream streamResponse = response.GetResponseStream();

            StreamReader streamRead = null;
            try
            {
                webResponse = new WebResponse();
                webResponse.ResponseBytes = StreamToByteArray(streamResponse);
                webResponse.ResponseString = ByteArrayToStr(webResponse.ResponseBytes);
                webResponse.ResponseHeaders = response.Headers;

                if (!string.IsNullOrEmpty(filename))
                    Device.File.Save(filename, webResponse.ResponseBytes);
            }
            finally
            {
                // Close the stream object
                if (streamResponse != null)
                    streamResponse.Dispose();
                if (streamRead != null)
                    streamRead.Dispose();
            }

            return webResponse;
        }
    }

    /// <summary>
    /// Represents a network response from a web server.
    /// </summary>
    public class WebResponse
    {
        /// <summary>
        /// Gets or sets the body of the response from the server as an array of <see cref="System.Byte"/>s.
        /// </summary>
        public byte[] ResponseBytes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body of the response from the server as a <see cref="System.String"/>.
        /// </summary>
        public string ResponseString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the headers contained in the server's response.
        /// </summary>
        /// <value>The response headers.</value>
        public WebHeaderCollection ResponseHeaders
        {
            get;
            internal set;
        }
    }
}
