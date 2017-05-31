using System.Collections.Generic;

namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Defines an abstract network fetch utility.
    /// </summary>
    public interface IFetcher
    {
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        NetworkResponse Fetch(string uri);
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        NetworkResponse Fetch(string uri, int timeout);
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        NetworkResponse Fetch(string uri, IDictionary<string, string> headers);
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        NetworkResponse Fetch(string uri, IDictionary<string, string> headers, int timeout);
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        NetworkResponse Fetch(string uri, string filename);
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        NetworkResponse Fetch(string uri, string filename, int timeout);
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        NetworkResponse Fetch(string uri, string filename, IDictionary<string, string> headers);
        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        NetworkResponse Fetch(string uri, string filename, IDictionary<string, string> headers, int timeout);
    }
}
