using System;
using System.Net;

namespace MonoCross
{
    /// <summary>
    /// Represents the state of a network response.
    /// </summary>
    public class NetworkResponse
    {
        /// <summary>
        /// Gets or sets the expiration date and time of the request.
        /// </summary>
        public DateTime Expiration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date and time that the content of the request was downloaded.
        /// </summary>
        public DateTime Downloaded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date and time of the last attempt to refresh.
        /// </summary>
        public DateTime AttemptToRefresh
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status of the response when an exception has occurred during the request.
        /// </summary>
        public WebExceptionStatus WebExceptionStatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message describing the state of the response.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exception that occurred during the request.
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI of the request.
        /// </summary>
        public string URI
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file to output.
        /// </summary>
        public string OutputFile
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
        /// Gets or sets the body of the response from the server as an array of <see cref="System.Byte"/>s.
        /// </summary>
        public byte[] ResponseBytes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object being posted.
        /// </summary>
        public object PostObject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP verb for the request.
        /// </summary>
        public string Verb
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkResponse"/> class.
        /// </summary>
        public NetworkResponse()
        {
        }
    }
}
