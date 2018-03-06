using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
 
namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Represents a network post utility that performs synchronously.
    /// </summary>
    public class PosterSynch : IPoster
    {
        const int DefaultTimeout = 60 * 1000;  // default to 60 seconds

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        public NetworkResponse PostString(string uri, string postString)
        {
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostString(uri, postString, "application/x-www-form-urlencoded", "POST", null);
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        public NetworkResponse PostString(string uri, string postString, string contentType)
        {
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostString(uri, postString, contentType, "POST", null);
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        public NetworkResponse PostString(string uri, string postString, string contentType, string verb)
        {
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostString(uri, postString, contentType, verb, null);
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostString(string uri, string postString, IDictionary<string, string> headers)
        {
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostString(uri, postString, "application/x-www-form-urlencoded", "POST", headers);
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostString(string uri, string postString, string contentType, IDictionary<string, string> headers)
        {
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, contentType, "POST", headers, null);
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostString(string uri, string postString, string contentType, string verb, IDictionary<string, string> headers)
        {
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, contentType, verb, headers, null);
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        public NetworkResponse PostObject(string uri, object postObject)
        {
            // convert object to bytes via an XML serializer, JSON not supported in MonoCross
            byte[] postBytes = NetworkUtils.XmlSerializeObjectToBytes(postObject);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, "application/xml", "POST", null, postObject);
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        public NetworkResponse PostObject(string uri, object postObject, string verb)
        {
            // convert object to bytes via an XML serializer, JSON not supported in MonoCross
            byte[] postBytes = NetworkUtils.XmlSerializeObjectToBytes(postObject);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, "application/xml", verb, null, postObject);
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostObject(string uri, object postObject, IDictionary<string, string> headers)
        {
            // convert object to bytes via an XML serializer, JSON not supported in MonoCross
            byte[] postBytes = NetworkUtils.XmlSerializeObjectToBytes(postObject);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, "application/xml", "POST", headers, postObject);
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostObject(string uri, object postObject, string verb, IDictionary<string, string> headers)
        {
            return PostObject(uri, postObject, verb, headers, -1);
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        public NetworkResponse PostObject(string uri, object postObject, string verb, IDictionary<string, string> headers, int timeout)
        {
            // convert object to bytes via an XML serializer, JSON not supported in MonoCross
            byte[] postBytes = NetworkUtils.XmlSerializeObjectToBytes(postObject);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, "application/xml", verb, headers, postObject, timeout);
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType)
        {
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, contentType, "POST", null, null);
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, string verb)
        {
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, contentType, verb, null, null);
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, IDictionary<string, string> headers)
        {
            return PostBytes(uri, postBytes, contentType, "POST", headers, null);
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, string verb, IDictionary<string, string> headers)
        {
            return PostBytes(uri, postBytes, contentType, verb, headers, null);
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="postObject">The object to post.</param>
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, string verb,
                    IDictionary<string, string> headers, object postObject)
        {
            return PostBytes(uri, postBytes, contentType, verb,
                    headers, postObject, DefaultTimeout);
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <exception cref="NotSupportedException">Thrown on platforms that do not support <see cref="PosterSynch"/>.</exception>
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, string verb,
                    IDictionary<string, string> headers, object postObject, int timeout)
        {
            using (var poster = new PosterAsynch())
            {
               return poster.PostBytes(uri, postBytes, contentType, verb, headers, postObject, timeout);
            }
        }

        private static NetworkResponse ExtractState(string uri, string verb, object postObject, PosterAsynch.RequestState state)
        {
            NetworkResponse response = new NetworkResponse() {
                URI = uri,
                Verb = verb,
                Downloaded = DateTime.UtcNow,
                PostObject = postObject,

                StatusCode = state.StatusCode,
                ResponseBytes = state.ResponseBytes,
                ResponseString = state.ResponseString,
            };

            switch (response.StatusCode)
            {
            case System.Net.HttpStatusCode.OK:
            case System.Net.HttpStatusCode.Created:
            case System.Net.HttpStatusCode.Accepted:
                break;
            case System.Net.HttpStatusCode.NoContent:
                response.Message = String.Format("No Content returned: Result {0} for {1}", response.StatusCode, uri);
                response.Expiration = DateTime.Now;
                Device.Log.Warn(response.Message);
                break;
            default:
                response.Expiration = DateTime.UtcNow;
                response.AttemptToRefresh = DateTime.UtcNow;
                response.Message = String.Format("{0} failed. Received HTTP {1} for {2}", verb, response.StatusCode, uri);
                Device.Log.Error(response.Message);
                break;
            }
            return response;
        }

        void poster_OnComplete(PosterAsynch.RequestState state)
        {
            throw new NotImplementedException();
        }

    }
}
