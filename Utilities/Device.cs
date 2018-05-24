using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Network;
using MonoCross.Utilities.Threading;
using MonoCross.Navigation;
using System;

namespace MonoCross.Utilities
{
    /// <summary>
    /// Represents a method that handles network response events.
    /// </summary>
    /// <param name="networkResponse">Contains information about the network response.</param>
    public delegate void NetworkResponseEvent(NetworkResponse networkResponse);

    /// <summary>
    /// Represents a collection of platform-specific utilities in the abstract.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Initializes a static instance of the <see cref="Device"/> class.
        /// </summary>
        static Device()
        {
            DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
            DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        #region Singleton initializer

        /// <summary>
        /// Initializes the singleton device instance.
        /// </summary>
        /// <param name="newInstance">The singleton device instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newInstance"/> is <c>null</c>.</exception>
        public static void Initialize(Device newInstance)
        {
            if (newInstance == null)
                throw new ArgumentNullException();

            MXContainer.RegisterSingleton<IThread>(typeof(BasicThread));
            MXContainer.RegisterSingleton<IThread>(typeof(MockThread), ThreadType.MockThread.ToString());

            MXContainer.RegisterSingleton<INetwork>(typeof(NetworkAsynch));
            MXContainer.RegisterSingleton<INetwork>(typeof(NetworkSynch), NetworkType.NetworkSynch.ToString());

            MXContainer.RegisterSingleton<IImageCache>(typeof(ImageCache));
            MXContainer.RegisterSingleton<IFile>(typeof(BasicFile));
            MXContainer.RegisterSingleton<Resources.IResources>(typeof(Resources.BasicResources));
            MXContainer.RegisterSingleton<IReflector>(typeof(BasicReflector));
            MXContainer.RegisterSingleton<ILog>(typeof(BasicLogger), args =>
            {
                var path = args.Length > 0 ? args[0] as string : null;
                return new BasicLogger(path ?? SessionDataPath.AppendPath("Log"));
            });

            MXContainer.RegisterSingleton<IEncryption>(typeof(MockEncryption));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.NullCompositor));

            Instance = newInstance;
            Instance.Initialize();
            File.EnsureDirectoryExistsForFile(SessionDataPath.AppendPath("Log"));
        }
        #endregion

        private string _applicationPath;
        private string _sessionDataRoot;
        private string _sessionDataAppend = string.Empty;

        /// <summary>
        /// Initializes this instance with platform-specific implementations.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Gets or sets the Device instance stored on the session.
        /// </summary>
        public static Device Instance
        {
            get
            {
                object instance;
                Session.TryGetValue(SessionDeviceId, out instance);
                return (instance as Device);
            }
            set
            {
                Session[SessionDeviceId] = value;
                if (!Session.SafeKeys.Contains(SessionDeviceId))
                    Session.SafeKeys.Add(SessionDeviceId);
            }
        }

        /// <summary>
        /// Gets the current session settings.
        /// </summary>
        public static ISession Session
        {
            get { return MXContainer.Session; }
            set { MXContainer.Session = value; }
        }

        static string SessionDeviceId
        {
            get
            {
                return string.Format("{0}.Device", MXContainer.GetSessionId == null ? string.Empty : MXContainer.GetSessionId());
            }
        }

        /// <summary>
        /// Occurs when a network response is received.
        /// </summary>
        public static event NetworkResponseEvent OnNetworkResponse;

        /// <summary>
        /// Gets the date and time of the last attempted controller navigation.
        /// </summary>
        public static DateTime LastActivityDate { get; set; }

        static NetworkGetMethod _networkGetMethod = NetworkGetMethod.Any;

        /// <summary>
        /// Gets or sets the restrictions to impose on network GET methods.
        /// </summary>
        public static NetworkGetMethod NetworkGetMethod
        {
            get
            {
                return _networkGetMethod;
            }
            set
            {
                _networkGetMethod = value;
            }
        }

        static NetworkPostMethod _networkPostMethod = NetworkPostMethod.QueuedAsynchronous;

        /// <summary>
        /// Gets or sets the restrictions to impose on network POST methods.
        /// </summary>
        public static NetworkPostMethod NetworkPostMethod
        {
            get
            {
                return _networkPostMethod;
            }
            set
            {
                _networkPostMethod = value;
            }
        }

        /// <summary>
        /// Gets the session-scoped root path for application data.
        /// </summary>
        /// <value>The data path as a <see cref="String"/> instance.</value>
        public virtual string SessionDataRoot
        {
            get
            {
                if (string.IsNullOrEmpty(_sessionDataRoot))
                    _sessionDataRoot = DataPath.AppendPath("session");
                return _sessionDataRoot;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && File != null)
                    File.EnsureDirectoryExists(value);
                _sessionDataRoot = value;
            }
        }

        /// <summary>
        /// Gets or sets the session-scoped appended path for application data.
        /// </summary>
        /// <value>The data path as a <see cref="String"/> instance.</value>
        public virtual string SessionDataAppend
        {
            get
            {
                return _sessionDataAppend;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    _sessionDataAppend = string.Empty;
                _sessionDataAppend = value;
            }
        }

        /// <summary>
        /// Gets the session-scoped path for application data.
        /// </summary>
        /// <value>The data path as a <see cref="String"/> instance.</value>
        public static string SessionDataPath
        {
            get
            {
                string _sessionDataPath = Instance.SessionDataRoot.AppendPath(Instance.SessionDataAppend);
                File.EnsureDirectoryExists(_sessionDataPath);
                return _sessionDataPath;
            }
        }

        /// <summary>
        /// Gets or sets the request injection headers within the current session.
        /// </summary>
        /// <value>A <see cref="SerializableDictionary&lt;TKey,TValue&gt;"/> representing the request injection headers.</value>
        public static SerializableDictionary<string, string> RequestInjectionHeaders
        {
            get
            {
                if (!MXContainer.Session.ContainsKey("MonoCross_RequestInjectionHeaders"))
                    MXContainer.Session["MonoCross_RequestInjectionHeaders"] = new SerializableDictionary<string, string>();

                return (SerializableDictionary<string, string>)MXContainer.Session["MonoCross_RequestInjectionHeaders"];
            }
            set
            {
                MXContainer.Session["MonoCross_RequestInjectionHeaders"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the data encryptor.
        /// </summary>
        /// <value>The encryptor as an <see cref="IEncryption"/> instance.</value>
        public static IEncryption Encryption
        {
            get { return MXContainer.Resolve<IEncryption>(); }
            set { MXContainer.RegisterSingleton<IEncryption>(value); }
        }

        /// <summary>
        /// Gets or sets the file system interface.
        /// </summary>
        /// <value>File system access as an <see cref="IFile"/> instance.</value>
        public static IFile File
        {
            get { return MXContainer.Resolve<IFile>(); }
            set { MXContainer.RegisterSingleton<IFile>(value); }
        }

        /// <summary>
        /// Gets or sets the image cache.
        /// </summary>
        /// <value>The image cache as an <see cref="IImageCache"/> instance.</value>
        public static IImageCache ImageCache
        {
            get { return MXContainer.Resolve<IImageCache>(); }
            set { MXContainer.RegisterSingleton<IImageCache>(value); }
        }

        /// <summary>
        /// Gets or sets the logging utility.
        /// </summary>
        /// <value>The logger as an <see cref="ILog"/> instance.</value>
        public static ILog Log
        {
            get { return LoggerFactory.Create(SessionDataPath.AppendPath("Log")); }
            set { MXContainer.RegisterSingleton<ILog>(value); }
        }

        /// <summary>
        /// Gets or sets the reflector utility.
        /// </summary>
        public static IReflector Reflector
        {
            get { return MXContainer.Resolve<IReflector>(); }
            set { MXContainer.RegisterSingleton<IReflector>(value); }
        }

        /// <summary>
        /// Gets or sets the resources utility.
        /// </summary>
        /// <value>The resrouces utility as an <see cref="Resources.IResources"/> instance.</value>
        public static Resources.IResources Resources
        {
            get { return MXContainer.Resolve<Resources.IResources>(); }
            set { MXContainer.RegisterSingleton<Resources.IResources>(value); }
        }

        /// <summary>
        /// Gets or sets the threading utility.
        /// </summary>
        /// <value>The threading utility as an <see cref="IThread"/> instance.</value>
        public static IThread Thread
        {
            get { return ThreadFactory.Create(); }
            set { MXContainer.RegisterSingleton<IThread>(value); }
        }

        /// <summary>
        /// Gets the networking utility.
        /// </summary>
        /// <value>The networking utility as an <see cref="INetwork"/> instance.</value>
        public static INetwork Network
        {
            get { return NetworkFactory.Create(); }
            set { MXContainer.RegisterSingleton<INetwork>(value); }
        }

        /// <summary>
        /// Gets the read-only path for application assets.
        /// </summary>
        /// <value>The application path as a <see cref="string"/> instance.</value>
        public static string ApplicationPath
        {
            get { return Instance._applicationPath; }
            set { Instance._applicationPath = value; }
        }

        /// <summary>
        /// Gets the path for read/write global data.
        /// </summary>
        /// <value>The data path as a <see cref="string"/> instance.</value>
        public static string DataPath { get; set; }

        /// <summary>
        /// Gets the appropriate directory separator character for the platform.
        /// </summary>
        public static char DirectorySeparatorChar
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the device platform.
        /// </summary>
        public static MobilePlatform Platform
        {
            get { return _platform; }
            set { _platform = value; }
        }
        private static MobilePlatform _platform = MobilePlatform.Unknown;

        /// <summary>
        /// Invokes the <see cref="OnNetworkResponse"/> event using the specified network response.
        /// </summary>
        /// <param name="networkResponse">The network response from a previous request.</param>
        public static void PostNetworkResponse(NetworkResponse networkResponse)
        {
            if (networkResponse == null)
                return;

            var responseHandler = OnNetworkResponse;
            if (responseHandler != null)
                responseHandler(networkResponse);
        }
    }
}