using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;


namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Represents a network post utility that performs asynchronously.
    /// </summary>
    public class PosterAsynch : IPoster, IDisposable
    {
        #region IDisposable implementation

        public void Dispose()
        {
            //TODO: should we call abort on open HttpWebRequest.BeginGetResponse() calls?
        }

        #endregion

        #region class fields and properties

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


        const int DefaultTimeout = 60 * 1000;

        /// <summary>
        /// Represents the method that will handle network post events.
        /// </summary>
        public delegate void PostRequestEventHandler(RequestState state);

        /// <summary>
        /// Represents the method that will handle network post error events.
        /// </summary>
        /// <param name="state"></param>
        public delegate void PostRequestErrorHandler(RequestState state);

        /// <summary>
        /// Occurs when an asynch download completes.
        /// </summary>
        public event PostRequestEventHandler OnComplete;

        /// <summary>
        /// Occurs when an error occurs.
        /// </summary>
        public event PostRequestErrorHandler OnError;

        //private RequestState _state = null;

        #endregion

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        public NetworkResponse PostString(string uri, string postString)
        {
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, "application/x-www-form-urlencoded", "POST", null);
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="contentType">The type of the content being posted.</param>
        public NetworkResponse PostString(string uri, string postString, string contentType)
        {
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, contentType, "POST", null);
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
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, contentType, verb, null);
        }

        /// <summary>
        /// Posts the specified string to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postString">The string to post.</param>
        /// <param name="headers">The headers for the request.</param>
        public NetworkResponse PostString(string uri, string postString, IDictionary<string, string> headers)
        {
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            return PostBytes(uri, postBytes, "application/x-www-form-urlencoded", "POST", headers);
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
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            return PostBytes(uri, postBytes, contentType, headers);
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
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.StrToByteArray(postString);

            return PostBytes(uri, postBytes, contentType, verb, headers);
        }

        /// <summary>
        /// Posts the specified object to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to post to.</param>
        /// <param name="postObject">The object to post.</param>
        public NetworkResponse PostObject(string uri, object postObject)
        {
            // convert object to bytes via an XML
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
            // convert object to bytes via an XML
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
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.XmlSerializeObjectToBytes(postObject);

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
            return PostObject(uri, postObject, verb, headers, DefaultTimeout);
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
            // convert object to bytes via an XML
            byte[] postBytes = NetworkUtils.XmlSerializeObjectToBytes(postObject);

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
            return PostBytes(uri, postBytes, contentType, "POST", null);
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
            return PostBytes(uri, postBytes, contentType, verb, null);
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
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
            return PostBytes(uri, postBytes, contentType, "POST", headers);
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
            // default headers to null rather than Device.RequestInjectionHeaders as we cannot guarantee the 
            // default headers are relevant to the specified URI
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
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, string verb, IDictionary<string, string> headers, object postObject)
        {
            return PostBytes(uri, postBytes, contentType, verb, headers, postObject, DefaultTimeout);
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
        public NetworkResponse PostBytes(string uri, byte[] postBytes, string contentType, string verb, IDictionary<string, string> headers,
                        object postObject, int timeout)
        {
            string strheaders = string.Empty;
            if (headers != null)
            {
                foreach (var kvp in headers)
                {
                    strheaders += string.Format("{0}: {1}\r\n", kvp.Key, kvp.Value);
                }
            }
            Device.Log.Debug(string.Format("Processing Request: {0} {1}\r\n \r\n{2}", verb, uri, strheaders));
            PostNetworkResponse = new NetworkResponse();
            if (postObject != null)
                PostNetworkResponse.PostObject = postObject;

            RequestParameters requestParameters = new RequestParameters()
            {
                PostBytes = postBytes,
                Uri = uri,
                Headers = headers,
                ContentType = contentType,
                Verb = verb,
                Timeout = timeout
            };
            DateTime dtMetric = DateTime.UtcNow;

            // set callback and error handler
            OnComplete -= PostRequest_OnComplete;
            OnComplete += PostRequest_OnComplete;
            OnError -= PostRequest_OnError;
            OnError += PostRequest_OnError;

            Exception threadExc = null;
            Device.Thread.QueueWorker(parameters =>
            {
                try
                {
                    PostAsynch(parameters);
                }
                catch (Exception e)
                {
                    // You could react or save the exception to an 'outside' variable 
                    threadExc = e;
                }
                finally
                {
                    autoEvent.Set(); // if you're firing and not forgetting ;)    
                }
            }, requestParameters);

            // WaitOne returns true if autoEvent were signaled (i.e. process completed before timeout expired)
            // WaitOne returns false it the timeout expired before the process completed.
#if NETCF
            //if (!autoEvent.WaitOne(DefaultTimeout, false))
            if (!autoEvent.WaitOne(timeout, false))
#else
            //if (!autoEvent.WaitOne(DefaultTimeout))
            if (!autoEvent.WaitOne(requestParameters.Timeout))
#endif
            {
                string message = "PosterAsynch call to RequestAsynch timed out. uri " + requestParameters.Uri;
                Device.Log.Metric(string.Format("PosterAsynch timed out: Uri: {0} Time: {1:F0} milliseconds ", uri, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

                NetworkResponse networkResponse = new NetworkResponse()
                {
                    Message = message,
                    URI = requestParameters.Uri,
                    StatusCode = HttpStatusCode.RequestTimeout,
                    WebExceptionStatusCode = (WebExceptionStatus)(14), // 14 = timeout, not available in silverlight
                    ResponseString = string.Empty,
                    Expiration = DateTime.MinValue.ToUniversalTime(),
                    AttemptToRefresh = DateTime.MinValue.ToUniversalTime(),
                    Downloaded = DateTime.UtcNow,
                    Exception = threadExc,
                };

                Device.PostNetworkResponse(networkResponse);
                return networkResponse;
            }
            else if (threadExc != null)
            {
                PostNetworkResponse.Exception = threadExc;
                PostNetworkResponse.Message = "PostAsynch threw an exception";
                PostNetworkResponse.StatusCode = (HttpStatusCode)(-1);
            }

            Device.Log.Metric(string.Format("PosterAsynch Completed: Uri: {0} Time: {1:F0} milliseconds  Size: {2} ", uri, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds, (PostNetworkResponse.ResponseBytes != null ? PostNetworkResponse.ResponseBytes.Length : -1)));

            return PostNetworkResponse;
        }

        private void PostRequest_OnError(RequestState state)
        {
            Exception exc = new Exception("PosterAsynch call to RequestAsynch threw an exception", state.Exception);

            var webEx = state.Exception as WebException;
            if (webEx != null)
            {
                if (webEx.Message.Contains("Network is unreachable") ||
                    webEx.Message.Contains("Error: ConnectFailure") || // iOS Message when in Airplane mode
                    webEx.Message.Contains("The remote name could not be resolved:")) // Windows message when no network access
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
            //Device.Log.Error (string.Format ("Request to endpoint {0} failed.", state.Uri));
            else { Device.Log.Error(exc); }

            PostNetworkResponse.StatusCode = state.StatusCode;
            PostNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            PostNetworkResponse.Message = exc.Message;
            PostNetworkResponse.Exception = exc;
            PostNetworkResponse.URI = state.Uri;
            PostNetworkResponse.Verb = state.Verb;
            PostNetworkResponse.ResponseHeaders = state.ResponseHeaders;
            PostNetworkResponse.Downloaded = state.Downloaded;

            Device.PostNetworkResponse(PostNetworkResponse);

            autoEvent.Set(); // release the PostBytes(...) call
        }

        private void PostRequest_OnComplete(RequestState state)
        {
            PostNetworkResponse.StatusCode = state.StatusCode;
            PostNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            PostNetworkResponse.URI = state.Uri;
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
                case HttpStatusCode.NoContent:           // return request is fulfilled but no return body
                    // things are ok, no event required
                    break;
                case HttpStatusCode.Unauthorized:        // return when session expires
                case HttpStatusCode.InternalServerError: // return when an exception happens
                case HttpStatusCode.ServiceUnavailable:  // return when the database or siteminder are unavailable
                    PostNetworkResponse.Message = String.Format("Network Service responded with status code {0}", state.StatusCode);
                    Device.PostNetworkResponse(PostNetworkResponse);
                    break;
                default:
                    PostNetworkResponse.Message = String.Format("PosterAsynch completed but received HTTP {0}", state.StatusCode);
                    // Device.Log.Error( PostNetworkResponse.Message );
                    Device.PostNetworkResponse(PostNetworkResponse);
                    return;
            }
        }

        private void PostAsynch(Object parameters)
        {
            RequestParameters requestParameters = (RequestParameters)parameters;

            RequestState state = new RequestState
            {
                PostBytes = requestParameters.PostBytes,
                Uri = requestParameters.Uri,
                Verb = requestParameters.Verb,
                Downloaded = DateTime.UtcNow
            };

            DateTime dtMetric = DateTime.UtcNow;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(state.Uri);

            request.Method = requestParameters.Verb; // Post, Put, Delete
            request.ContentType = requestParameters.ContentType;
#if NETCF
            request.Timeout = requestParameters.Timeout;
            request.ContentLength = requestParameters.PostBytes.Length;
            request.AutomaticDecompression = DecompressionMethods.Deflate;
            request.KeepAlive = false;
#endif

            if (requestParameters.Headers != null && requestParameters.Headers.Any())
            {
                foreach (string key in requestParameters.Headers.Keys)
                {
                    if (key.ToLower() == "accept")
                    {
                        request.Accept = requestParameters.Headers[key];
                    }
                    else if (key.ToLower() == "content-type")
                    {
                        request.ContentType = requestParameters.Headers[key];
                    }
                    else if (key.ToLower() == "host")
                    {
                        Exception ex;
#if NETCF
                        ex = new ArgumentException("Host header value cannot be set in Compact Frameword libraries.");
#else
                        //TODO: add the URL explaining PCL incompatibility
                        ex = new ArgumentException("Host header value cannot be set in PCL libraries.");
#endif
                        Device.Log.Error(ex);
                        throw ex;
                    }
                    else
                    {
                        request.Headers[key] = requestParameters.Headers[key];
                    }
                }
            }

            state.Request = request;

            try
            {
                // Start the asynchronous request.
                IAsyncResult result = request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), state);
#if NETCF
                if (!allDone.WaitOne(requestParameters.Timeout, false))
#else
                if (!allDone.WaitOne(requestParameters.Timeout))
#endif
                {
                    try { request.Abort(); } catch (Exception) { } // .Abort() always throws exception
                    return;
                }
            }
            catch (Exception exc)
            {
                Device.Log.Error("PosterAsynch.PostAsynch encountered exception", exc);
                autoEvent.Set();
            }
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            // Get and fill the RequestState
            RequestState state = (RequestState)asynchronousResult.AsyncState;
            HttpWebRequest request = state.Request;

            string errorMsg = "PosterAsynch.GetRequestStreamCallback encountered WebException"; 
            try
            {
                // End the operation
                Stream postStream = request.EndGetRequestStream(asynchronousResult);

                // Write to the request stream.
                postStream.Write(state.PostBytes, 0, state.PostBytes.Length);
#if NETCF
                postStream.Close();
#endif
                postStream.Dispose();

                // Start the asynchronous operation to get the response
                IAsyncResult result = request.BeginGetResponse(new AsyncCallback(GetResponseCallback), state);
            }
            catch (WebException ex)
            {
                string StatusDescription = string.Empty;
#if !NETCF
                ex.Data.Add("Uri", state.Uri);
                ex.Data.Add("Verb", state.Verb);
                ex.Data.Add("WebException.Status", ex.Status);
#endif
                state.ErrorMessage = errorMsg;
                state.Exception = ex;
                state.WebExceptionStatusCode = ex.Status;

                if (ex.Response != null)
                {
                    state.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
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
#if !NETCF
                ex.Data.Add("StatusCode", state.StatusCode);
                ex.Data.Add("StatusDescription", StatusDescription);
#endif
                OnError(state);
                allDone.Set();
            }
            catch (Exception ex)
            {
#if !NETCF
                ex.Data.Add("Uri", state.Uri);
                ex.Data.Add("Verb", state.Verb);
#endif
                state.ErrorMessage = errorMsg;
                state.Exception = ex;
                state.StatusCode = (HttpStatusCode)(-2);
                OnError(state);
                allDone.Set();
            }
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            // Get and fill the RequestState
            RequestState state = (RequestState)asynchronousResult.AsyncState;

            HttpWebRequest request = state.Request;
            HttpWebResponse response = null;

            try
            {
                // End the Asynchronous response and get the actual response object
                response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

                state.StatusCode = response.StatusCode;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.Accepted:
                        break;
                    case HttpStatusCode.NoContent:
                        Device.Log.Info("Empty payload returned in PosterAsynch: Result {0} for {1}", state.StatusCode, request.RequestUri);
                        break;
                    default:
                        state.ErrorMessage = String.Format("{0} failed. Received HTTP {1} for {2}", state.Verb, response.StatusCode, state.Uri);
                        Device.Log.Error(state.ErrorMessage);
                        state.Expiration = DateTime.UtcNow;
                        state.AttemptToRefresh = DateTime.UtcNow;
                        OnComplete(state);

                        return;
                }

                // extract response into bytes and string.
                WebResponse webResponse = NetworkUtils.ExtractResponse(response);
                state.ResponseBytes = webResponse.ResponseBytes;
                state.ResponseString = webResponse.ResponseString;
                state.ResponseHeaders = webResponse.ResponseHeaders;
#if !NETCF
                state.Expiration = response.Headers["Expires"].TryParseDateTimeUtc();
                state.AttemptToRefresh = response.Headers["MonoCross-Attempt-Refresh"].TryParseDateTimeUtc();
#endif
                OnComplete(state);
            }
            catch (WebException ex)
            {
                string StatusDescription = string.Empty;
                string message = string.Format("Call to {0} had a Webexception. {1}   Status: {2}", state.Request.RequestUri, ex.Message, ex.Status);
#if !NETCF
                ex.Data.Add("WebException.StatusCode", ex.Status);
                ex.Data.Add("Uri", state.Uri);
                ex.Data.Add("Verb", state.Verb);

                ex.Data.Add("StatusCode", state.StatusCode);
#endif
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
#if !NETCF
                ex.Data.Add("WebException.Status", ex.Status);
                ex.Data.Add("StatusDescription", StatusDescription);
#endif
                state.ErrorMessage = message;
                state.Exception = ex;
                state.Expiration = DateTime.UtcNow;
                state.AttemptToRefresh = DateTime.UtcNow;

                OnError(state);
            }
            catch (Exception ex)
            {
                // TODO: Consider adding custom post exceptions...
                string message = string.Format("Call to {0} had an Exception. {1}", state.Request.RequestUri, ex.Message);
                Exception qexc = new Exception(message, ex);
#if !NETCF
                qexc.Data.Add("Uri", state.Uri);
                qexc.Data.Add("Verb", state.Verb);
#endif
                state.ErrorMessage = message;
                state.Exception = qexc;
                state.Expiration = DateTime.UtcNow;
                state.AttemptToRefresh = DateTime.UtcNow;
                state.StatusCode = (HttpStatusCode)(-1);
                state.WebExceptionStatusCode = (WebExceptionStatus)(-1);

                OnError(state);
            }
            finally
            {
                // Release the HttpWebResponse
                if (response != null)
                    ((IDisposable)response).Dispose();
                state.Request = null;

                allDone.Set();
            }
        }

        /// <summary>
        /// Represents a collection of parameters for network post calls.
        /// </summary>
        public class RequestParameters
        {
            /// <summary>
            /// Gets or sets the bytes being posted.
            /// </summary>
            public byte[] PostBytes
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the URI being posted to.
            /// </summary>
            public string Uri
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the type of the content being posted.
            /// </summary>
            public string ContentType
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
            /// Gets or sets the headers added to the request.
            /// </summary>
            public IDictionary<string, string> Headers
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
        /// Represents the state of a network post request.
        /// </summary>
        public class RequestState
        {
            //public int BufferSize
            //{
            //    get;
            //    private set;
            //}
            /// <summary>
            /// Gets or sets the web request.
            /// </summary>
            public HttpWebRequest Request
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the URI of the request.
            /// </summary>
            public string Uri
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the bytes being posted.
            /// </summary>
            public byte[] PostBytes
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
            ///  Represents a handle that has been registered when calling
            ///  System.Threading.ThreadPool.RegisterWaitForSingleObject(System.Threading.WaitHandle,System.Threading.WaitOrTimerCallback,System.Object,System.UInt32,System.Boolean).
            /// </summary>
            public class RegisteredWaitHandle { }

            /// <summary>
            /// The handle to register when calling RegisterWaitForSingleObject.
            /// </summary>
            public RegisteredWaitHandle Handle = null;

            /// <summary>
            /// Gets or sets a message describing the error that occurred during the request.
            /// </summary>
            public string ErrorMessage
            {
                get;
                set;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RequestState"/> class.
            /// </summary>
            public RequestState()
            {
                // BufferSize = 6 * 1024;
                Request = null;
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
