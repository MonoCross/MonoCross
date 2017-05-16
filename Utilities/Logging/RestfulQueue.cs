using System;
using System.Net;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using MonoCross.Utilities.Serialization;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Network;

//using MonoTouch.SystemConfiguration;
//using MonoTouch.CoreFoundation;
//using Android.Util;

namespace MonoCross.Utilities.Logging
{
    public class RestfulQueue<T> : Queue<T>
    {

        /// <summary>
        /// Returns true if the device has an internet connection.
        /// </summary>
        public bool IsOnline()
        {
//            ReachabilityStatus status = new ReachabilityStatus();
//            Thread thread = new Thread(ThreadedIsOnline);
//            thread.IsBackground = true;
//            thread.Start(status);
//
//            bool terminated = thread.Join(2000);
//
//            return terminated && status.IsOnline;
			return true;
        }

        private void ThreadedIsOnline(object state)
        {
            //using (NSAutoreleasePool pool = new NSAutoreleasePool())
            //{
            //	((ReachabilityStatus)state).IsOnline = Reachability.RemoteHostStatus() != NetworkStatus.NotReachable;
            //}
        }

        private class ReachabilityStatus
        {
            public bool IsOnline = true;
        }

        private string _endpointAddress;
        private Timer timer;
        private const int timerDelay = 5000;

        #region using extension and Restful Object

        //			/// <summary>
        //	        /// Defines the delegate for factory events
        //	        /// </summary>
        //	        public delegate void RequestComplete( T obj );
        //		
        //		    /// <summary>
        //		    /// Occurs when asynch download completes.
        //		    /// </summary>
        //		    public event RequestComplete OnRequestComplete;
        //	        public string BaseUri
        //	        {
        //	            get;
        //	            set;
        //	        }
        //	        public string RelativeUri
        //	        {
        //	            get;
        //	            set;
        //	        }
        //	        public string AbsoluteUri
        //	        {
        //	            get
        //	            {
        //	                return BaseUri.AppendPath( RelativeUri );
        //	                // return BaseUri.Insert( BaseUri.Length, RelativeUri );
        //	            }
        //	        }
        //	
        //	        public SerializationFormat Format
        //	        {
        //	            get;
        //	            set;
        //	        }
        #endregion

        private bool _enabled = true;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                if (value && this.Count() > 0)
                    TriggerTimer();
            }
        }

        internal RestfulQueue()
        {
        }

        public RestfulQueue(string endpointAddress)
        {
            _endpointAddress = endpointAddress;
        }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            TriggerTimer();
        }

        public void AttemptNextTransaction()
        {
            bool continuePost = false;
            continuePost = IsOnline();

            while (this.Count > 0 && continuePost)
            {
                T nextItem = Peek();
                if (nextItem == null)
                    continue;

                try
                {
                    ISerializer<T> serializer = SerializerFactory.Create<T>(SerializationFormat.XML);
                    if (null != serializer)
                    {
                        string objString = serializer.SerializeObject(nextItem);
                        PostObject(_endpointAddress, objString);
                        Dequeue();
                    }
                    else
                    {
                        continuePost = false;
                    }
                }
                catch
                {
                    //Application.Log.Error( exc );
                    //Log.Error("RestfulQueue.AttemptNextTransaction", ex.Message + " " + ex.StackTrace);
                    continuePost = false;
                }
            }

        }

        public void UpdateNetworkStatus()
        {
            /*
            var remoteHostStatus = Reachability.RemoteHostStatus ();			
            switch (remoteHostStatus)
            {
                case NetworkStatus.NotReachable:						
                    break;

                case NetworkStatus.ReachableViaCarrierDataNetwork:
                    break;

                case NetworkStatus.ReachableViaWiFiNetwork:
                    break;

            }
            */
        }

        private string PostObject(string url, string data)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/xml; charset=utf-8";
#if !WINDOWS_PHONE
            request.KeepAlive = false;
#endif
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data);
#if !WINDOWS_PHONE
            request.ContentLength = byteData.Length;
#endif
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }

        public void TriggerTimer()
        {
            if (!Enabled)
                return;

            if (timer != null)
                timer.Change(timerDelay, Timeout.Infinite);
            else
            {
                timer = new Timer(new TimerCallback((o) =>
                {
                    AttemptNextTransaction();
                }), null, timerDelay, Timeout.Infinite);
            }
        }

        #region Queue Serialization Methods
        //	
        //	        public void DeserializeQueue()
        //	        {
        //	            ISerializer<RestfulObject<T>> iSerializer = SerializerFactory.Create<RestfulObject<T>>( Format );
        //	            List<RestfulObject<T>> queuelist = iSerializer.DeserializeListFromFile( QueueFileName );
        //	
        //	            if ( queuelist == null )
        //	                return;
        //	
        //	            foreach ( RestfulObject<T> item in queuelist )
        //	            {
        //	                this.Enqueue( item );
        //	            }
        //	        }
        //	
        //	        public void SerializeQueue()
        //	        {
        //	            ISerializer<RestfulObject<T>> iSerializer = SerializerFactory.Create<T>( Format );
        //	            iSerializer.SerializeListToFile( this.ToList(), QueueFileName );
        //	        }
        //	
        //	        private string _queueFileName;
        //	        private string QueueFileName
        //	        {
        //	            get
        //	            {
        //	                if ( string.IsNullOrEmpty( _queueFileName ) )
        //	                {
        //	                    //_queueFileName = Application.Factory.SessionDataPath.AppendPath( "Queue" ).AppendPath( typeof( T ).Name + ".xml" );
        //	                    _queueFileName = "";
        //					//// ensure file's directory exists
        //	                    //Application.File.EnsureDirectoryExists( _queueFileName );
        //	                }
        //	
        //	                return _queueFileName;
        //	            }
        //	            set
        //	            {
        //	                _queueFileName = value;
        //	            }
        //	        }
        //	
        //	
        //	        /// <summary>
        //	        /// Removes serialized file for queue and discards all queue contents
        //	        /// </summary>
        //	        public void DiscardQueue()
        //	        {
        //	            if ( File.Exists( QueueFileName ) )
        //	            {
        //	                File.Delete( QueueFileName );
        //	            }
        //	            lock ( syncLock )
        //	            {
        //	                while ( this.Count() > 0 )
        //	                {
        //	                    this.Dequeue();
        //	                }
        //	            }
        //	        }


        #endregion
    }
}

