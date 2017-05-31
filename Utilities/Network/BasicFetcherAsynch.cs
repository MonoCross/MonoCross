using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace MonoCross.Utilities.Networking
{
    /// <summary>
    /// Represents a network fetch utility that performs asynchronously.
    /// </summary>
    public class BasicFetcherAsynch : IFetcher
    {
        const int DefaultTimeout = 180 * 1000;  // default to 180 seconds

        private ManualResetEvent allDone = new ManualResetEvent(false);
        private AutoResetEvent autoEvent = new AutoResetEvent(false);

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
        public delegate void NetworkingEventHandler(FetcherAsynch.RequestState state);

        /// <summary>
        /// Represents the method that will handle network error events.
        /// </summary>
        /// <param name="state"></param>
        public delegate void NetworkingErrorHandler(FetcherAsynch.RequestState state);

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
                    Device.Log.Warn("Exception occurred in WindowsFetcherAsynch", e);
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
#if NETCF
            if ( !autoEvent.WaitOne( timeout, false ) )
#else
            if (!autoEvent.WaitOne(timeout))
#endif
            {
                string message = "BasicFetcherAsynch call to FetchAsynch timed out. uri " + fetchParameters.Uri;
                Device.Log.Metric(string.Format("FetchAsynch timed out: Uri: {0} Time: {1} milliseconds ", uri, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

                NetworkResponse networkResponse = new NetworkResponse()
                {
                    Message = message,
                    URI = fetchParameters.Uri,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    WebExceptionStatusCode = (WebExceptionStatus)(14),  // 14 = timeout, not available in silverlight
                    ResponseString = string.Empty,
                    Expiration = DateTime.MinValue.ToUniversalTime(),
                    AttemptToRefresh = DateTime.MinValue.ToUniversalTime(),
                    Downloaded = DateTime.UtcNow,
                    Exception = threadExc,
                };

                Device.PostNetworkResponse(networkResponse);
                return networkResponse;
            }
            else if (threadExc != null && PostNetworkResponse.Exception == null)
            {
                PostNetworkResponse.Exception = threadExc;
                PostNetworkResponse.Message = "FetchAsync threw an exception: " + threadExc.Message;
                PostNetworkResponse.StatusCode = (HttpStatusCode)(-1);
            }

            Device.Log.Metric(string.Format("FetchAsynch Completed: Uri: {0} Time: {1} milliseconds  Size: {2} ", uri, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds, (PostNetworkResponse.ResponseBytes != null ? PostNetworkResponse.ResponseBytes.Length : -1)));

            return PostNetworkResponse;
        }

        private void FetcherAsynch_OnError(FetcherAsynch.RequestState state)
        {
            Exception exc = new Exception("BasicFetcherAsynch call to FetchAsynch threw an exception", state.Exception);
            Device.Log.Error(exc);

            PostNetworkResponse.StatusCode = state.StatusCode;
            PostNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            PostNetworkResponse.Message = exc.Message;
            PostNetworkResponse.Exception = exc;
            PostNetworkResponse.URI = state.AbsoluteUri;
            PostNetworkResponse.Verb = state.Verb;
            PostNetworkResponse.ResponseString = null;
            PostNetworkResponse.ResponseBytes = null;
            PostNetworkResponse.Downloaded = state.Downloaded;

            Device.PostNetworkResponse(PostNetworkResponse);

            allDone.Set(); // release the Fetch(...) call
        }

        private void FetcherAsynch_OnDownloadComplete(FetcherAsynch.RequestState state)
        {
            PostNetworkResponse.StatusCode = state.StatusCode;
            PostNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            PostNetworkResponse.URI = state.AbsoluteUri;
            PostNetworkResponse.Verb = state.Verb;
            PostNetworkResponse.ResponseString = state.ResponseString;
            PostNetworkResponse.ResponseBytes = state.ResponseBytes;
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

            if (fetchParameters.Headers != null && fetchParameters.Headers.Any())
            {
                foreach (string key in fetchParameters.Headers.Keys)
                {
                    var keyValue = key.ToLower();
                    if (keyValue == "accept")
                    {
                        request.Accept = fetchParameters.Headers[key];
                    }
                    else if (keyValue == "content-type")
                    {
                        request.ContentType = fetchParameters.Headers[key];
                    }
                    else if (keyValue == "host")
                    {
                        request.Host = fetchParameters.Headers[key];
                    }
                    else if (keyValue == "accept-encoding")
                    {
                        string encodingValue = fetchParameters.Headers[key];
                        if (encodingValue.ToLower().Contains("gzip"))
                        {
                            request.AutomaticDecompression = DecompressionMethods.GZip;
                        }
                    }
                    else
                    {
                        request.Headers[key] = fetchParameters.Headers[key];
                    }
                }
            }

            FetcherAsynch.RequestState state = new FetcherAsynch.RequestState()
            {
                Request = request,
                AbsoluteUri = fetchParameters.Uri,
                FileName = fetchParameters.FileName,
                Downloaded = DateTime.UtcNow
            };

            try
            {
                // Start the asynchronous request.
                IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ResponseCallback), state);
#if NETCF
                if (!allDone.WaitOne(fetchParameters.Timeout, false))
#else
                if (!allDone.WaitOne(fetchParameters.Timeout))
#endif
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
            FetcherAsynch.RequestState state = (FetcherAsynch.RequestState)result.AsyncState;

            try
            {
                HttpWebRequest request = state.Request;

                // End the Asynchronous response and get the actual response object
                state.Response = (HttpWebResponse)request.EndGetResponse(result);
#if !NETCF
                state.Expiration = state.Response.Headers["Expires"].TryParseDateTimeUtc();
                state.AttemptToRefresh = state.Response.Headers["MonoCross-Attempt-Refresh"].TryParseDateTimeUtc();
#endif
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
                        state.ErrorMessage = String.Format("No Content returned: Result {0} for {1}", state.StatusCode, request.RequestUri);
                        Device.Log.Warn(state.ErrorMessage);
                        state.Expiration = DateTime.UtcNow;
                        state.AttemptToRefresh = DateTime.UtcNow;
                        state.Downloaded = DateTime.UtcNow;
                        OnDownloadComplete(state);
                        return;
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

                OnDownloadComplete(state);

            }
            catch (WebException ex)
            {
                string StatusDescription = string.Empty;
#if !NETCF
                ex.Data.Add("Uri", state.Request.RequestUri);
                ex.Data.Add("Verb", state.Request.Method);
#endif
                if (ex.Response != null)
                {
                    state.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    StatusDescription = ((HttpWebResponse)ex.Response).StatusDescription;
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
#if !NETCF
                ex.Data.Add("StatusCode", state.StatusCode);
                ex.Data.Add("WebException.Status", ex.Status);
                ex.Data.Add("StatusDescription", StatusDescription);
#endif
                state.ErrorMessage = string.Format("Call to {0} had a Webexception. {1}   Status: {2}   Desc: {3}", state.Request.RequestUri, ex.Message, ex.Status, StatusDescription);
                state.Exception = ex;
                state.Expiration = DateTime.UtcNow;
                state.AttemptToRefresh = DateTime.UtcNow;

                OnError(state);
            }
            catch (Exception ex)
            {
#if !NETCF
                ex.Data.Add("Uri", state.Request.RequestUri);
                ex.Data.Add("Verb", state.Request.Method);
#endif
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

    }

    internal class WindowsNetworkAsync : NetworkAsynch
    {
        public override IFetcher Fetcher
        {
            get { return new BasicFetcherAsynch(); }
        }
    }
}

