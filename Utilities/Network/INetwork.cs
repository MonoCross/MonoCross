using MonoCross.Navigation;
using System.Collections.Generic;

namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Defines an abstract networking interface.
    /// </summary>
    public interface INetwork
    {
        /// <summary>
        /// Gets the network fetcher.
        /// </summary>
        /// <value>The fetcher as an <see cref="IFetcher"/> instance.</value>
        IFetcher Fetcher { get; }
        /// <summary>
        /// Gets the network poster.
        /// </summary>
        /// <value>The poster as an <see cref="IPoster"/> instance.</value>
        IPoster Poster { get; }

        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        string Get(string uri);
        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <returns></returns>
        string Get(string uri, int timeout);
        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        string Get(string uri, Dictionary<string, string> headers);
        /// <summary>
        /// Returns the response string of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response string of.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <returns></returns>
        string Get(string uri, Dictionary<string, string> headers, int timeout);

        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        byte[] GetBytes(string uri);
        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <returns>A byte array representing the response.</returns>
        byte[] GetBytes(string uri, int timeout);
        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns>A byte array representing the response.</returns>
        byte[] GetBytes(string uri, Dictionary<string, string> headers);
        /// <summary>
        /// Returns the response bytes of the specified URI.
        /// </summary>
        /// <param name="uri">The URI to get the response bytes of.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <returns>A byte array representing the response.</returns>
        byte[] GetBytes(string uri, Dictionary<string, string> headers, int timeout);

        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <returns></returns>
        string PostBytes(string uri, byte[] postBytes, string contentType);
        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        string PostBytes(string uri, byte[] postBytes, string contentType, Dictionary<string, string> headers);

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <returns></returns>
        string PostObject( string uri, object postObject );
        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        string PostObject( string uri, object postObject, Dictionary<string, string> headers );

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <returns></returns>
        string PostString( string uri, string postString );
        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        string PostString( string uri, string postString, Dictionary<string, string> headers );

        /// <summary>
        /// Gets or sets the request injection headers within the current session.
        /// </summary>
        /// <value>A <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> representing the request injection headers.</value>
        SerializableDictionary<string, string> RequestInjectionHeaders { get; set; }

        //static string Request( string uri );
        //static string Request( string uri, params string[] args );

        //static string Post( string uri, Dictionary<string, string> parameters );
        //static string Post( string uri, Dictionary<string, string> parameters, params string[] args );
        //static string Post( string uri, object obj );
        //static string Post( string uri, object obj, params string[] args );

        //static XDocument LoadXml( string uri );
        //static XDocument LoadXml( string uri, params string[] args );

    }
}
