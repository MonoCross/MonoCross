using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Network;
using MonoCross.Utilities.Serialization;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Threading;

namespace MonoCross.Utilities
{
    public class Device
    {
        public static event NetworkResponseEvent OnNetworkResponse;
        public static void PostNetworkResponse(NetworkResponse networkResponse)
        {
            if (networkResponse == null)
                return;

            if (OnNetworkResponse != null)
                Device.OnNetworkResponse(networkResponse);
        }

        static Device()
        {
            Thread = ThreadFactory.Create();
            Network = NetworkFactory.Create();
			      DataPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);			
			
            //Session = new SerializableDictionary<string, object>();
            //Settings = new SerializableDictionary<string, string>();
        }

        #region utilities
		/// <summary>Gets the data path.</summary>
        /// <value>The data path.</value>
        public static string DataPath
        {
            get { return  _dataPath; }
			set { _dataPath = value; }
        }
		
		private static string _dataPath = string.Empty;		

        public static ILog Log
        {
            get
            {
                if ( _logger == null )
                    _logger = new BasicLogger( Environment.GetFolderPath (Environment.SpecialFolder.Personal) );
                return _logger;
            }
        }

		public static IFile File
        {
            get
            {
#if SILVERLIGHT
                if (_file == null)
                    _file = (MXDevice.Encryption.Required ? new SLFileEncrypted() : new SLFile());
#else
                if ( _file == null )
					_file = (Device.Encryption.Required ? new BasicFileEncrypted() : new BasicFile());
#endif
                return _file;
            }
        }
		
		public static IEncryption Encryption 
		{
			get {
				if( _encryption == null )
				{
#if SILVERLIGHT
                    _encryption = new SLEncryption();
#else
                    _encryption = new AesEncryption();
#endif
			    }
				return _encryption;
			}
		}
		
		protected static ILog _logger = null;

        protected static IFile _file = null;

        protected static IEncryption _encryption = null;
		
        public static IThread Thread { get; set; }

        public static INetwork Network { get; set; }

        //public static SerializableDictionary<string, object> Session { get; set; }

        //public static SerializableDictionary<string, string> Settings { get; set; }

        #endregion
    }
}
