using System;
using System.Net;
using System.Threading;

namespace BestSellers
{
    public static class HttpExtensions
    {
        public static WebResponse GetResponse(this HttpWebRequest request)
        {
            var autoResetEvent = new AutoResetEvent(false);
            IAsyncResult asyncResult = request.BeginGetResponse(r => autoResetEvent.Set(), null);
            autoResetEvent.WaitOne();

            return request.EndGetResponse(asyncResult);
        }
    }
}