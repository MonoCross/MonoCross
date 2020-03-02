using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Represents a network fetch utility that performs asynchronously.
    /// </summary>
    public class FetcherAsynch : IFetcher, IDisposable
    {
        const int DefaultTimeout = 180 * 1000;  // default to 180 seconds

        private ManualResetEvent allDone = new ManualResetEvent(false);
        private AutoResetEvent autoEvent = new AutoResetEvent(false);

        #region IDisposable implementation
        public void Dispose()
        {
            //TODO: should we call abort on open HttpWebRequest.BeginGetResponse() calls?
        }
        #endregion

        //private object padLock = new object();
        //NetworkResponse _networkResponse = new NetworkResponse();
        private NetworkResponse PostNetworkResponse
        {
            get;
            //{
            //lock ( padLock )
            //{
            //    return _networkResponse;
            //}
            //}
            set;
            // {
            //lock ( padLock )
            //{
            //    _networkResponse = value;
            //}
            //}
        }

        /// <summary>
        /// Represents the method that will handle networking events.
        /// </summary>
        public delegate void NetworkingEventHandler(RequestState state);

        /// <summary>
        /// Represents the method that will handle network error events.
        /// </summary>
        /// <param name="state"></param>
        public delegate void NetworkingErrorHandler(RequestState state);

        /// <summary>
        /// Occurs when an asynch download completes.
        /// </summary>
        public event NetworkingEventHandler OnDownloadComplete;

        /// <summary>
        /// Occurs when an error happens.
        /// </summary>
        public event NetworkingErrorHandler OnError;

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
            return Fetch(uri, null, null, timeout);
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
            return Fetch(uri, null, headers, timeout);
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
            return Fetch(uri, filename, null, timeout);
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
        public NetworkResponse Fetch(string uri, string filename, IDictionary<string, string> headers, int timeout)
        {
            PostNetworkResponse = new NetworkResponse();
            FetchParameters fetchParameters = new FetchParameters()
            {
                Uri = uri,
                Headers = headers,
                FileName = filename,
                Timeout = timeout
            };

            DateTime dtMetric = DateTime.UtcNow;

            // set callback and error handler
            OnDownloadComplete -= FetcherAsynch_OnDownloadComplete;
            OnDownloadComplete += FetcherAsynch_OnDownloadComplete;
            OnError -= FetcherAsynch_OnError;
            OnError += FetcherAsynch_OnError;

            Exception threadExc = null;
            Device.Thread.QueueWorker(parameters =>
            {
                try
                {
                    FetchAsynch(parameters);
                }
                catch (Exception e)
                {
                    Device.Log.Info("This exception occurred in FetchingAsynch:", e);
                    // You could react or save the exception to an 'outside' variable 6`
                    threadExc = e;    
                }
                finally
                {
                    autoEvent.Set(); // if you're firing and not forgetting ;)    
                }
            }, fetchParameters);

            // WaitOne returns true if autoEvent were signaled (i.e. process completed before timeout expired)
            // WaitOne returns false it the timeout expired before the process completed.
            if (!autoEvent.WaitOne(timeout))
            {
                string message = "FetcherAsynch call to FetchAsynch timed out. uri " + fetchParameters.Uri;
                Device.Log.Metric("FetchAsynch timed out: Uri: {0} Time: {1:F0} milliseconds ", uri, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds);

                NetworkResponse networkResponse = new NetworkResponse()
                {
                    Message = message,
                    URI = fetchParameters.Uri,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    WebExceptionStatusCode = WebExceptionStatus.Timeout,
                    ResponseString = string.Empty,
                    Expiration = DateTime.MinValue.ToUniversalTime(),
                    AttemptToRefresh = DateTime.MinValue.ToUniversalTime(),
                    Downloaded = DateTime.UtcNow,
                    Exception = threadExc
                };

                Device.PostNetworkResponse(networkResponse);
                return networkResponse;
            }
            else if (threadExc != null)
            {
                PostNetworkResponse.Exception = threadExc;
                PostNetworkResponse.Message = "FetchAsync threw an exception";
                PostNetworkResponse.StatusCode = (HttpStatusCode)(-1);
            }    

			Device.Log.Metric(string.Format("FetchAsynch Completed: Uri: {0} Time: {1:F0} milliseconds  Size: {2} ", uri, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds, (PostNetworkResponse.ResponseBytes != null ? PostNetworkResponse.ResponseBytes.Length : -1)));

            return PostNetworkResponse;
        }

        private void FetcherAsynch_OnError(RequestState state)
        {
            Exception exc = new Exception("FetcherAsynch call to FetchAsynch threw an exception: " + state.ErrorMessage, state.Exception);

            var webEx = state.Exception as WebException;
            if (webEx != null)
            {
                if (state != null && state.StatusCode == HttpStatusCode.NotModified)
                {
                    Device.Log.Info("Not Modified returned in FetcherAsynch for: {0}", state.Request.RequestUri);
                }
                else if (webEx.Message.Contains("Network is unreachable") ||
                    webEx.Message.Contains("Error: ConnectFailure") || // iOS Message when in Airplane mode
                    webEx.Message.Contains("The remote name could not be resolved:") || // Windows message when no network access
                    webEx.Message.Contains("(304) Not Modified")) // HttpWebResponse.EndGetResponse throws exception on HttpStatusCode.NotModified response
                {
                    Device.Log.Info(exc);
                }
                else { Device.Log.Error(exc);  }
            }
            else if (state.Exception != null && state.Exception.Message != null &&
                     state.Exception.Message.Contains("Error: NameResolutionFailure"))
            {
                Device.Log.Info(exc);
            }
            else if (state.StatusCode == HttpStatusCode.RequestTimeout)
            {
                Device.Log.Info(exc);
            }
            else { Device.Log.Error(exc); }

            PostNetworkResponse.StatusCode = state.StatusCode;
            PostNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            PostNetworkResponse.Message = exc.Message;
            PostNetworkResponse.Exception = exc;
            PostNetworkResponse.URI = state.AbsoluteUri;
            PostNetworkResponse.Verb = state.Verb;
            PostNetworkResponse.ResponseString = null;
            PostNetworkResponse.ResponseBytes = null;
            PostNetworkResponse.ResponseHeaders = state.ResponseHeaders;
            PostNetworkResponse.Downloaded = state.Downloaded;

            Device.PostNetworkResponse(PostNetworkResponse);

            autoEvent.Set(); // release the Fetch(...) call
        }

        private void FetcherAsynch_OnDownloadComplete(RequestState state)
        {
            PostNetworkResponse.StatusCode = state.StatusCode;
            PostNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            PostNetworkResponse.URI = state.AbsoluteUri;
            PostNetworkResponse.Verb = state.Verb;
            PostNetworkResponse.ResponseString = state.ResponseString;
            PostNetworkResponse.ResponseBytes = state.ResponseBytes;
            PostNetworkResponse.ResponseHeaders = state.ResponseHeaders;
            PostNetworkResponse.Expiration = state.Expiration;
            PostNetworkResponse.Downloaded = state.Downloaded;
            PostNetworkResponse.AttemptToRefresh = state.AttemptToRefresh;
            PostNetworkResponse.Message = state.ErrorMessage;

            switch (PostNetworkResponse.StatusCode)
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
                    PostNetworkResponse.Message = String.Format("Network Service responded with status code {0}", state.StatusCode);
                    Device.PostNetworkResponse(PostNetworkResponse);
                    break;
                default:
                    PostNetworkResponse.Message = String.Format("FetcherAsynch completed but received HTTP {0}", state.StatusCode);
                    Device.Log.Error(PostNetworkResponse.Message);
                    Device.PostNetworkResponse(PostNetworkResponse);
                    return;
            }
        }

        private void FetchAsynch(Object parameters)
        {
            FetchParameters fetchParameters = (FetchParameters)parameters;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fetchParameters.Uri);

            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            if (fetchParameters.Headers != null && fetchParameters.Headers.Any())
            {
                foreach (string key in fetchParameters.Headers.Keys)
                {
                    if (key.ToLower() == "accept")
                    {
                        request.Accept = fetchParameters.Headers[key];
                    }
                    else if (key.ToLower() == "content-type")
                    {
                        request.ContentType = fetchParameters.Headers[key];
                    }
                    else if (key.ToLower() == "host")
                    {
                        Exception ex = new ArgumentException("Host header value cannot be set in PCL libraries.");
                        Device.Log.Error(ex);
                        throw ex;
                    }
                    else
                    {
                        request.Headers[key] = fetchParameters.Headers[key];
                    }
                }
            }

            RequestState state = new RequestState()
            {
                Request = request,
                AbsoluteUri = fetchParameters.Uri,
                FileName = fetchParameters.FileName,
                Downloaded = DateTime.UtcNow,
                StatusCode = HttpStatusCode.RequestTimeout,
            };

            try
            {
                // Start the asynchronous request.
                IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ResponseCallback), state);
                if (!allDone.WaitOne(fetchParameters.Timeout))
                {
                    try { request.Abort(); } catch (Exception) { } // .Abort() always throws exception
                    return;
                }
            }
            catch (Exception exc)
            {
                Device.Log.Error("FetcherAsynch.FetchAsynch encountered exception", exc);
                autoEvent.Set();
            }
        }

        // Define other methods and classes here
        private void ResponseCallback(IAsyncResult result)
        {
            // Get and fill the RequestState
            RequestState state = (RequestState)result.AsyncState;

            try
            {
                HttpWebRequest request = state.Request;

                // End the Asynchronous response and get the actual response object
                state.Response = (HttpWebResponse)request.EndGetResponse(result);
                state.Expiration = state.Response.Headers["Expires"].TryParseDateTimeUtc();
                state.AttemptToRefresh = state.Response.Headers["MonoCross-Attempt-Refresh"].TryParseDateTimeUtc();
                // apply web response headers to data collection.
                // TODO: evaluate which headers are actually needed and skip those that aren't. So, what's our logic for "needed headers" ?
                foreach (string key in state.Response.Headers.AllKeys)
                {
                    state.Data[key] = state.Response.Headers[key];
                }

                state.StatusCode = state.Response.StatusCode;

                switch (state.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.Accepted:
                        break;
                    case HttpStatusCode.NoContent:
                        Device.Log.Info("Empty payload returned in FetcherAsynch: Result {0} for {1}", state.StatusCode, request.RequestUri);
                        break;
                    default:
                        state.ErrorMessage = String.Format("Get failed. Received HTTP {0} for {1}", state.StatusCode, request.RequestUri);
                        Device.Log.Error(state.ErrorMessage);
                        state.Expiration = DateTime.UtcNow;
                        state.AttemptToRefresh = DateTime.UtcNow;
                        state.Downloaded = DateTime.UtcNow;
                        OnDownloadComplete(state);

                        return;
                }

                // extract response into bytes and string.
                WebResponse webResponse = NetworkUtils.ExtractResponse(state.Response, state.FileName);
                state.ResponseBytes = webResponse.ResponseBytes;
                state.ResponseString = webResponse.ResponseString;
                state.ResponseHeaders = webResponse.ResponseHeaders;

                OnDownloadComplete(state);

            }
            catch (WebException ex)
            {
                string StatusDescription = string.Empty;
                ex.Data.Add("Uri", state.Request.RequestUri);
                ex.Data.Add("Verb", state.Request.Method);
                if (ex.Response != null)
                {
                    state.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    StatusDescription = ((HttpWebResponse)ex.Response).StatusDescription;
                    state.ResponseHeaders = ex.Response.Headers;
                }
                else if (ex.Message.ToLower().Contains("request was aborted"))
                {
                    state.StatusCode = HttpStatusCode.RequestTimeout;
                    StatusDescription = "Request cancelled by client because the server did not respond within timeout";
                }
                else
                {
                    state.StatusCode = (HttpStatusCode)(-2);
                }
                state.WebExceptionStatusCode = ex.Status;
                ex.Data.Add("StatusCode", state.StatusCode);
                ex.Data.Add("WebException.Status", ex.Status);
                ex.Data.Add("StatusDescription", StatusDescription);
                state.ErrorMessage = string.Format("Call to {0} had a Webexception. {1}   Status: {2}   Desc: {3}", state.Request.RequestUri, ex.Message, ex.Status, StatusDescription);
                state.Exception = ex;
                state.Expiration = DateTime.UtcNow;
                state.AttemptToRefresh = DateTime.UtcNow;

                OnError(state);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Uri", state.Request.RequestUri);
                ex.Data.Add("Verb", state.Request.Method);
                state.ErrorMessage = string.Format("Call to {0} had an Exception. {1}", state.Request.RequestUri, ex.Message);
                state.Exception = ex;
                state.StatusCode = (HttpStatusCode)(-1);
                state.WebExceptionStatusCode = (WebExceptionStatus)(-1);
                state.Expiration = DateTime.UtcNow;
                state.AttemptToRefresh = DateTime.UtcNow;

                OnError(state);
            }
            finally
            {
                if (state.Response != null)
                    ((IDisposable)state.Response).Dispose();
                state.Request = null;

                allDone.Set();
            }
        }

        /// <summary>
        /// Represents a collection of parameters for network fetch calls.
        /// </summary>
        public class FetchParameters
        {
            /// <summary>
            /// Gets or sets the URI of the resource.
            /// </summary>
            public string Uri
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the headers added to the request.
            /// </summary>
            public IDictionary<string, string> Headers
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the file name of the resource. 
            /// </summary>
            public string FileName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the timeout value in milliseconds.
            /// </summary>
            public int Timeout
            {
                get;
                set;
            }
        }

        /// <summary>
    /// Represents the state of a network fetch request.
    /// </summary>
        public class RequestState
        {
            /// <summary>
            /// Gets or sets the file name of the resource.
            /// </summary>
            public string FileName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the web request.
            /// </summary>
            public HttpWebRequest Request
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the web response.
            /// </summary>
            public HttpWebResponse Response
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
            /// Gets or sets the URI of the request.
            /// </summary>
            public string AbsoluteUri
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
            /// Gets or sets the exception that occurred during the request.
            /// </summary>
            public Exception Exception
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a message describing the error that occurred during the request.
            /// </summary>
            public string ErrorMessage
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the headers that are associated with the response.
            /// </summary>
            public Dictionary<string, string> Data
            {
                get;
                set;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RequestState"/> class.
            /// </summary>
            public RequestState()
            {
                Request = null;
                Response = null;
                Data = new Dictionary<string, string>();
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
}

