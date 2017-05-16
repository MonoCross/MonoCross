using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Android.OS;
using Java.Net;
using System.Threading.Tasks;
using Android.Runtime;

namespace MonoCross.Utilities.Networking
{
    /// <summary>
    /// Represents a network fetch utility that performs asynchronously.
    /// </summary>
    public class AndroidFetcher : IFetcher
    {
        private const int DefaultTimeout = 180 * 1000; // default to 180 seconds

        [Preserve]
        public AndroidFetcher() { }

        private void DisableConnectionReuseIfNecessary()
        {
            // Work around pre-Gingerbread bugs in HTTP connection reuse.
            if ((int)Build.VERSION.SdkInt < 9)
            {
                Java.Lang.JavaSystem.SetProperty("http.keepAlive", "false");
            }
        }

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

        private DateTime _dtMetric;

        /// <summary>
        /// Fetches the specified URI.
        /// </summary>
        /// <param name="uri">The URI of the resource to fetch.</param>
        /// <param name="filename">The name of the file to be fetched.</param>
        /// <param name="headers">The headers to be added to the request.</param>
        /// <param name="timeout">The request timeout value in milliseconds.</param>
        public NetworkResponse Fetch(string uri, string filename, IDictionary<string, string> headers, int timeout)
        {
            var fetchParameters = new DroidFetchParameters
            {
                NetworkResponse = new NetworkResponse(),
                Uri = uri.Replace(' ', '+'),
                Headers = headers,
                FileName = filename,
                Timeout = timeout <= 0 ? DefaultTimeout : timeout,
            };

            FetchSync(fetchParameters);
            return fetchParameters.NetworkResponse;
        }

        public void FetchSync(object param)
        {
            DisableConnectionReuseIfNecessary();

            _dtMetric = DateTime.UtcNow;
            var fetchParameters = (DroidFetchParameters)param;
            var state = new DroidRequestState
            {
                AbsoluteUri = fetchParameters.Uri,
                FileName = fetchParameters.FileName,
                Downloaded = DateTime.UtcNow,
                Verb = "GET",
            };

            var tokenSource2 = new CancellationTokenSource();
            CancellationToken token = tokenSource2.Token;
            var t = Task.Factory.StartNew(() =>
            {
                token.ThrowIfCancellationRequested();
                var currThread = Thread.CurrentThread;
                using (token.Register(currThread.Abort))
                {
                    var url = new URL(fetchParameters.Uri);
                    state.Connection = (HttpURLConnection)url.OpenConnection();
                    url.Dispose();
                    state.Connection.ConnectTimeout = fetchParameters.Timeout;
                    state.Connection.ReadTimeout = fetchParameters.Timeout;

                    if (fetchParameters.Headers != null && fetchParameters.Headers.Any())
                    {
                        foreach (var key in fetchParameters.Headers.Keys)
                        {
                            state.Connection.SetRequestProperty(key, fetchParameters.Headers[key]);
                        }
                    }

                    // End the Asynchronous response and get the actual response object
                    state.Expiration = state.Connection.GetHeaderField("Expires").TryParseDateTimeUtc();
                    state.AttemptToRefresh = state.Connection.GetHeaderField("MonoCross-Attempt-Refresh").TryParseDateTimeUtc();

                    // apply web response headers to data collection.
                    foreach (string key in state.Connection.HeaderFields.Keys.Where(k => k != null))
                    {
                        state.Data[key] = state.Connection.GetHeaderField(key);
                    }

                    state.StatusCode = (HttpStatusCode)state.Connection.ResponseCode;
                    switch (state.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Created:
                        case HttpStatusCode.Accepted:
                            try
                            {
                                // extract response into bytes and string.
                                byte[] bytes;
                                var webResponse = new WebResponse
                                {
                                    ResponseBytes = bytes = NetworkUtils.StreamToByteArray(state.Connection.InputStream),
                                    ResponseString = NetworkUtils.ByteArrayToStr(bytes),
                                };
                                if (!string.IsNullOrEmpty(state.FileName))
                                    Device.File.Save(state.FileName, bytes);
                                state.ResponseBytes = bytes;
                                state.ResponseString = webResponse.ResponseString;
                                FetcherAsynch_OnDownloadComplete(state, fetchParameters.NetworkResponse);
                            }
                            catch (Exception ex)
                            {
                                string StatusDescription = string.Empty;
                                ex.Data.Add("Uri", state.Connection.URL.ToString());
                                ex.Data.Add("Verb", state.Connection.RequestMethod);
                                if (state.Connection != null)
                                {
                                    state.StatusCode = (HttpStatusCode)state.Connection.ResponseCode;
                                    StatusDescription = state.Connection.ResponseMessage;
                                }
                                else
                                {
                                    state.StatusCode = (HttpStatusCode)(-2);
                                }

                                if (string.IsNullOrEmpty(StatusDescription))
                                {
                                    ex.Data.Add("StatusDescription", StatusDescription);
                                    state.ErrorMessage = string.Format("Call to {0} had an Exception. {1}", state.Connection.URL, ex.Message);
                                    state.WebExceptionStatusCode = (WebExceptionStatus)(-1);
                                }
                                else
                                {
                                    state.ErrorMessage = string.Format("Call to {0} had a web exception. {1}  Desc: {2}", state.Connection.URL, ex.Message, StatusDescription);
                                    state.StatusCode = (HttpStatusCode)(-1);
                                }

                                ex.Data.Add("StatusCode", state.StatusCode);
                                state.Exception = ex;
                                state.Expiration = DateTime.UtcNow;
                                state.AttemptToRefresh = DateTime.UtcNow;

                                Device.Log.Error(state.ErrorMessage);
                                Device.Log.Error(ex);

                                FetcherAsynch_OnError(state, fetchParameters.NetworkResponse);
                            }
                            finally
                            {
                                //if (state.Response != null)
                                //    ((IDisposable)state.Response).Dispose();
                                state.Connection.Disconnect();
                                state.Connection = null;
                            }
                            break;
                        case HttpStatusCode.NoContent:
                            state.ErrorMessage = string.Format("No Content returned: Result {0} for {1}", state.StatusCode, state.AbsoluteUri);
                            Device.Log.Warn(state.ErrorMessage);
                            state.Expiration = DateTime.UtcNow;
                            state.AttemptToRefresh = DateTime.UtcNow;
                            state.Downloaded = DateTime.UtcNow;
                            FetcherAsynch_OnDownloadComplete(state, fetchParameters.NetworkResponse);
                            break;
                        default:
                            state.Expiration = DateTime.UtcNow;
                            state.AttemptToRefresh = DateTime.UtcNow;
                            state.Downloaded = DateTime.UtcNow;
                            FetcherAsynch_OnError(state, fetchParameters.NetworkResponse);
                            break;
                    }
                }
            }, token);

            try
            {
                t.Wait(fetchParameters.Timeout + 5);
            }
            catch (Exception e)
            {
                state.Exception = e;
                state.Expiration = DateTime.UtcNow;
                state.AttemptToRefresh = DateTime.UtcNow;
                state.Downloaded = DateTime.UtcNow;
                FetcherAsynch_OnError(state, fetchParameters.NetworkResponse);
            }
            finally
            {
                if (state.Connection != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        state.Connection.Disconnect();
                        state.Connection = null;
                    });
                }
            }

            if (tokenSource2 != null && !t.IsCompleted)
            {
                tokenSource2.Cancel();
                state.StatusCode = HttpStatusCode.RequestTimeout;
                state.Expiration = DateTime.UtcNow;
                state.AttemptToRefresh = DateTime.UtcNow;
                state.Downloaded = DateTime.UtcNow;
                FetcherAsynch_OnError(state, fetchParameters.NetworkResponse);
            }

            Device.Log.Metric(string.Format("FetchAsynch Completed: Uri: {0} Time: {1} milliseconds  Size: {2} ", state.AbsoluteUri, DateTime.UtcNow.Subtract(_dtMetric).TotalMilliseconds, (fetchParameters.NetworkResponse.ResponseBytes != null ? fetchParameters.NetworkResponse.ResponseBytes.Length : -1)));
        }

        private void FetcherAsynch_OnError(DroidRequestState state, NetworkResponse postNetworkResponse)
        {
            string message;
            switch (state.StatusCode)
            {
                case HttpStatusCode.RequestTimeout:
                    message = "FetcherAsynch call to FetchAsynch timed out. uri " + state.AbsoluteUri;

                    Device.Log.Metric(string.Format("FetchAsynch timed out: Uri: {0} Time: {1} milliseconds ", state.AbsoluteUri, DateTime.UtcNow.Subtract(_dtMetric).TotalMilliseconds));

                    postNetworkResponse.URI = state.AbsoluteUri;
                    postNetworkResponse.StatusCode = HttpStatusCode.RequestTimeout;
                    postNetworkResponse.ResponseString = string.Empty;
                    postNetworkResponse.Expiration = DateTime.MinValue.ToUniversalTime();
                    postNetworkResponse.AttemptToRefresh = DateTime.MinValue.ToUniversalTime();
                    state.WebExceptionStatusCode = WebExceptionStatus.Timeout;
                    state.Downloaded = DateTime.UtcNow;
                    break;
                default:
                    message = "FetcherAsynch call to FetchAsynch threw an exception";
                    state.WebExceptionStatusCode = WebExceptionStatus.ProtocolError;
                    postNetworkResponse.StatusCode = state.StatusCode;
                    postNetworkResponse.Message = message;
                    postNetworkResponse.ResponseString = null;
                    postNetworkResponse.ResponseBytes = null;
                    postNetworkResponse.Verb = state.Verb;
                    break;
            }

            var exc = new Exception(message, state.Exception);
            Device.Log.Error(exc);

            postNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            postNetworkResponse.Exception = exc;
            postNetworkResponse.Downloaded = state.Downloaded;

            Device.PostNetworkResponse(postNetworkResponse);
        }

        private void FetcherAsynch_OnDownloadComplete(DroidRequestState state, NetworkResponse postNetworkResponse)
        {
            postNetworkResponse.StatusCode = state.StatusCode;
            postNetworkResponse.WebExceptionStatusCode = state.WebExceptionStatusCode;
            postNetworkResponse.URI = state.AbsoluteUri;
            postNetworkResponse.Verb = state.Verb;
            postNetworkResponse.ResponseString = state.ResponseString;
            postNetworkResponse.ResponseBytes = state.ResponseBytes;
            postNetworkResponse.Expiration = state.Expiration;
            postNetworkResponse.Downloaded = state.Downloaded;
            postNetworkResponse.AttemptToRefresh = state.AttemptToRefresh;
            postNetworkResponse.Message = state.ErrorMessage;

            switch (postNetworkResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                    // things are ok, no event required
                    break;
                case HttpStatusCode.NoContent: // return when an object is not found
                case HttpStatusCode.Unauthorized: // return when session expires
                case HttpStatusCode.InternalServerError: // return when an exception happens
                case HttpStatusCode.ServiceUnavailable: // return when the database or siteminder are unavailable
                    postNetworkResponse.Message = string.Format("Network Service responded with status code {0}",
                        state.StatusCode);
                    Device.PostNetworkResponse(postNetworkResponse);
                    break;
                default:
                    postNetworkResponse.Message = string.Format("FetcherAsynch completed but received HTTP {0}",
                        state.StatusCode);
                    Device.Log.Error(postNetworkResponse.Message);
                    Device.PostNetworkResponse(postNetworkResponse);
                    return;
            }
        }

        /// <summary>
        /// Represents a collection of parameters for network fetch calls.
        /// </summary>
        public class DroidFetchParameters
        {
            /// <summary>
            /// Gets or sets the URI of the resource.
            /// </summary>
            public string Uri { get; set; }

            /// <summary>
            /// Gets or sets the headers added to the request.
            /// </summary>
            public IDictionary<string, string> Headers { get; set; }

            /// <summary>
            /// Gets or sets the file name of the resource. 
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// Gets or sets the timeout value in milliseconds.
            /// </summary>
            public int Timeout { get; set; }

            /// <summary>
            /// Gets or sets the network response.
            /// </summary>
            public NetworkResponse NetworkResponse { get; set; }
        }
    }

    /// <summary>
    /// Represents the state of a network fetch request.
    /// </summary>
    public class DroidRequestState
    {
        /// <summary>
        /// Gets or sets the file name of the resource.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the web connection.
        /// </summary>
        public HttpURLConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the body of the response from the server as a <see cref="System.String"/>.
        /// </summary>
        public string ResponseString { get; set; }

        /// <summary>
        /// Gets or sets the body of the response from the server as an array of <see cref="System.Byte"/>s.
        /// </summary>
        public byte[] ResponseBytes { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the request.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the content of the request was downloaded.
        /// </summary>
        public DateTime Downloaded { get; set; }

        /// <summary>
        /// Gets or sets the URI of the request.
        /// </summary>
        public string AbsoluteUri { get; set; }

        /// <summary>
        /// Gets or sets the HTTP verb for the request.
        /// </summary>
        public string Verb { get; set; }

        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status of the response when an exception has occurred during the request.
        /// </summary>
        public WebExceptionStatus WebExceptionStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the exception that occurred during the request.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets a message describing the error that occurred during the request.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the headers that are associated with the response.
        /// </summary>
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DroidRequestState"/> class.
        /// </summary>
        public DroidRequestState()
        {
            Connection = null;
            //Response = null;
            Data = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the date and time of the last attempt to refresh.
        /// </summary>
        public DateTime AttemptToRefresh { get; set; }
    }
}
