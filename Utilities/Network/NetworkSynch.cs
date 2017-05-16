using MonoCross.Navigation;
using System.Collections.Generic;

namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Represents a networking utility that performs synchronously.
    /// </summary>
    public class NetworkSynch : INetwork
    {
        /// <summary>
        /// Gets the network fetcher.
        /// </summary>
        /// <value>The fetcher as an <see cref="IFetcher"/> instance.</value>
        public IFetcher Fetcher
        {
            get
            {
                return new FetcherSynch();
            }
        }

        /// <summary>
        /// Gets the network poster.
        /// </summary>
        /// <value>The poster as an <see cref="IPoster"/> instance.</value>
        public IPoster Poster
        {
            get
            {
                return new PosterSynch();
            }
        }

        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        public string Get(string uri)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, (Dictionary<string, string>)null, 60000);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        /// <param name="headers">The headers for the request.</param>
        public string Get(string uri, Dictionary<string, string> headers)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, headers, 60000);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        public string Get(string uri, int timeout)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, (Dictionary<string, string>)null, timeout);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        public string Get(string uri, Dictionary<string, string> headers, int timeout)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, headers, timeout);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        /// <returns></returns>
        public byte[] GetBytes(string uri)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, (Dictionary<string, string>)null, 60000);

            return networkResponse.ResponseBytes;
        }

        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <returns>
        /// A byte array representing the response.
        /// </returns>
        public byte[] GetBytes(string uri, int timeout)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, (Dictionary<string, string>)null, timeout);

            return networkResponse.ResponseBytes;
        }

        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns>
        /// A byte array representing the response.
        /// </returns>
        public byte[] GetBytes(string uri, Dictionary<string, string> headers)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, headers, 60000);

            return networkResponse.ResponseBytes;
        }

        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <returns>
        /// A byte array representing the response.
        /// </returns>
        public byte[] GetBytes(string uri, Dictionary<string, string> headers, int timeout)
        {
            IFetcher fetcher = new FetcherSynch();
            NetworkResponse networkResponse = fetcher.Fetch(uri, headers, timeout);

            return networkResponse.ResponseBytes;
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        public string PostBytes(string uri, byte[] postBytes, string contentType)
        {
            IPoster poster = new PosterSynch();
            NetworkResponse networkResponse = poster.PostBytes(uri, postBytes, contentType, (Dictionary<string, string>)null);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="headers">The headers for the request.</param>
        public string PostBytes(string uri, byte[] postBytes, string contentType, Dictionary<string, string> headers)
        {
            IPoster poster = new PosterSynch();
            NetworkResponse networkResponse = poster.PostBytes(uri, postBytes, contentType, headers);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        public string PostObject(string uri, object postObject)
        {
            IPoster poster = new PosterSynch();
            NetworkResponse networkResponse = poster.PostObject(uri, postObject, (Dictionary<string, string>)null);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="headers">The headers for the request.</param>
        public string PostObject(string uri, object postObject, Dictionary<string, string> headers)
        {
            IPoster poster = new PosterSynch();
            NetworkResponse networkResponse = poster.PostObject(uri, postObject, headers);
            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        public string PostString(string uri, string postString)
        {
            IPoster poster = new PosterSynch();
            NetworkResponse networkResponse = poster.PostString(uri, postString, (Dictionary<string, string>)null);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="headers">The headers for the request.</param>
        public string PostString(string uri, string postString, Dictionary<string, string> headers)
        {
            IPoster poster = new PosterSynch();
            NetworkResponse networkResponse = poster.PostString(uri, postString, headers);

            return networkResponse.ResponseString;
        }

        /// <summary>
        /// Gets or sets the request injection headers within the current session.
        /// </summary>
        /// <value>A <see cref="SerializableDictionary{TKey, TValue}"/> representing the request injection headers.</value>
        public SerializableDictionary<string, string> RequestInjectionHeaders 
        {
            get
            {
                if (Device.Session == null)
                    return null;
                if (!Device.Session.ContainsKey("MonoCross_RequestInjectionHeaders"))
                    Device.Session["MonoCross_RequestInjectionHeaders"] = new SerializableDictionary<string, string>();

                return (SerializableDictionary<string, string>)Device.Session["MonoCross_RequestInjectionHeaders"];
            }
            set
            {
                Device.Session["MonoCross_RequestInjectionHeaders"] = value;
            }
        }
    }
}
