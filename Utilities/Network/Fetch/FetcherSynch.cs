using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Represents a network fetch utility that performs synchronously.
    /// </summary>
    public class FetcherSynch : IFetcher
    {
        const int DefaultTimeout = 180 * 1000;  // default to 180 seconds

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        public NetworkResponse Fetch(string uri)
        {
            return Fetch(uri, null, null, DefaultTimeout);
        }

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        public NetworkResponse Fetch(string uri, int timeout)
        {
            return Fetch(uri, null, null, (timeout < 0) ? DefaultTimeout : timeout);
        }

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        public NetworkResponse Fetch(string uri, IDictionary<string, string> headers)
        {
            return Fetch(uri, null, headers, DefaultTimeout);
        }

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        public NetworkResponse Fetch(string uri, IDictionary<string, string> headers, int timeout)
        {
            return Fetch(uri, null, headers, (timeout < 0) ? DefaultTimeout : timeout);
        }

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        public NetworkResponse Fetch(string uri, string filename)
        {
            return Fetch(uri, filename, null, DefaultTimeout);
        }

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        public NetworkResponse Fetch(string uri, string filename, int timeout)
        {
            return Fetch(uri, filename, null, (timeout < 0) ? DefaultTimeout : timeout);
        }

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        public NetworkResponse Fetch(string uri, string filename, IDictionary<string, string> headers)
        {
            return Fetch(uri, filename, headers, DefaultTimeout);
        }

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        /// <exception cref="NotSupportedException">Thrown on platforms that do not support <see cref="FetcherSynch"/>.</exception>
        public virtual NetworkResponse Fetch(string uri, string filename, IDictionary<string, string> headers, int timeout)
        {
            using (var fetcher = new FetcherAsynch())
            {
                return fetcher.Fetch(uri, filename, headers, timeout);
            }
        }


        private static NetworkResponse ExtractState(string uri, FetcherAsynch.RequestState state)
        {
            NetworkResponse response = new NetworkResponse() {
                StatusCode = state.StatusCode,
                WebExceptionStatusCode = state.WebExceptionStatusCode,
                URI = uri,

                Verb = state.Verb,
                ResponseString = state.ResponseString,
                ResponseBytes = state.ResponseBytes,
                Expiration = state.Expiration,
                Downloaded = state.Downloaded,
                AttemptToRefresh = state.AttemptToRefresh,
                Message = state.ErrorMessage,
            };

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                    // things are ok, no event required
                    break;
                case HttpStatusCode.NoContent:           // return when an object is not found
                case HttpStatusCode.Unauthorized:        // return when session expires
                case HttpStatusCode.InternalServerError: // return when an exception happens
                case HttpStatusCode.ServiceUnavailable:  // return when the database or siteminder are unavailable
                    response.Message = String.Format("Network Service responded with status code {0}", state.StatusCode);
                    Device.PostNetworkResponse(response);
                    break;
                default:
                    response.Message = String.Format("FetcherAsynch completed but received HTTP {0}", state.StatusCode);
                    Device.Log.Error(response.Message);
                    Device.PostNetworkResponse(response);
                    break;
            }
            return response;
        }
    
    }
}
