using System.Collections.Generic;

namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Defines an abstract network post utility.
    /// </summary>
    public interface IPoster
    {
        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <returns></returns>
        NetworkResponse PostBytes( string uri, byte[] postBytes, string contentType );
        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <returns></returns>
        NetworkResponse PostBytes( string uri, byte[] postBytes, string contentType, string verb );
        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        NetworkResponse PostBytes( string uri, byte[] postBytes, string contentType, IDictionary<string, string> headers );
        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        NetworkResponse PostBytes( string uri, byte[] postBytes, string contentType, string verb, IDictionary<string, string> headers );
        /// <summary>
        /// Posts the specified bytes to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postBytes">The bytes to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="postObject">The object to post.</param>
        /// <returns></returns>
        NetworkResponse PostBytes( string uri, byte[] postBytes, string contentType, string verb, IDictionary<string, string> headers, object postObject );
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
		/// <returns></returns>
		NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, string verb, IDictionary<string, string> headers, object postObject, int timeout);

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <returns></returns>
        NetworkResponse PostObject( string uri, object postObject );
        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <returns></returns>
        NetworkResponse PostObject( string uri, object postObject, string verb );
        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        NetworkResponse PostObject( string uri, object postObject, IDictionary<string, string> headers );
        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        NetworkResponse PostObject( string uri, object postObject, string verb, IDictionary<string, string> headers );
        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="timeout">The timeout value in milliseconds.</param>
        /// <returns></returns>
        NetworkResponse PostObject(string uri, object postObject, string verb, IDictionary<string, string> headers, int timeout);

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <returns></returns>
        NetworkResponse PostString( string uri, string postString );
        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <returns></returns>
        NetworkResponse PostString( string uri, string postString, string contentType );
        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <returns></returns>
        NetworkResponse PostString( string uri, string postString, string contentType, string verb );
        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        NetworkResponse PostString( string uri, string postString, IDictionary<string, string> headers );
        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        NetworkResponse PostString( string uri, string postString, string contentType, IDictionary<string, string> headers );
        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        /// <param name="verb">The HTTP verb for the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <returns></returns>
        NetworkResponse PostString( string uri, string postString, string contentType, string verb, IDictionary<string, string> headers );
    }
}
