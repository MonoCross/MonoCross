using System;
using System.Collections.Generic;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Extension methods for <see cref="IMXView"/> that add navigation.
    /// </summary>
    public static class MXNavigationExtensions
    {
        /// <summary>
        /// Initiates a navigation to the specified URL.
        /// </summary>
        /// <param name="view">The <see cref="IMXView"/> that kicked off the navigation.</param>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        public static void Navigate(this IMXView view, string url)
        {
            MXContainer.Navigate(view, url);
        }

        /// <summary>
        /// Initiates a navigation to the specified URL.
        /// </summary>
        /// <param name="view">The <see cref="IMXView"/> that kicked off the navigation.</param>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        public static void Navigate(this IMXView view, string url, Dictionary<string, string> parameters)
        {
            MXContainer.Navigate(view, url, parameters);
        }
    }

    /// <summary>
    /// Represents the platform-specific instance of the MonoCross container.
    /// </summary>
    public abstract class MXContainer
    {
        /// <summary>
        /// Gets the date and time of the last navigation that occurred.
        /// </summary>
        /// <value>The last navigation date.</value>
        public DateTime LastNavigationDate { get; set; }

        /// <summary>
        /// Gets or sets the URL of last navigation that occurred.
        /// </summary>
        /// <value>The last navigation URL.</value>
        public string LastNavigationUrl { get; set; }

        /// <summary>
        /// The cancel load.
        /// </summary>
        protected bool CancelLoad = false;

        /// <summary>
        /// Load containers on a separate thread.
        /// </summary>
        public bool ThreadedLoad = true;

        /// <summary>
        /// Raises the controller load begin event.
        /// </summary>
        [Obsolete("Use OnControllerLoadBegin(IMXController, IMXView) instead")]
        protected virtual void OnControllerLoadBegin(IMXController controller)
        {

        }

        /// <summary>
        /// Called when a controller is about to be loaded.
        /// </summary>
        /// <param name="controller">The controller to be loaded.</param>
        /// <param name="fromView">The view that initiated the navigation that resulted in the controller being loaded.</param>
        protected virtual void OnControllerLoadBegin(IMXController controller, IMXView fromView)
        {
#pragma warning disable 618
            OnControllerLoadBegin(controller);
#pragma warning restore 618
        }

        /// <summary>
        /// Raises the controller load failed event.
        /// </summary>
        /// <param name="controller">The controller that failed to load.</param>
        /// <param name="ex">The exception that caused the load to fail.</param>
        protected virtual void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
        }

        /// <summary>
        /// Called when the IoC container is ready to be populated with its default entries.
        /// </summary>
        protected internal abstract void OnSetDefinitions();

        /// <summary>
        /// Raises the load complete event after the Controller has completed loading its Model. The View may be populated,
        /// and the derived class should check if it exists and do something with it if needed for the platform: either free it,
        /// pop off the views in a stack above it or whatever makes sense to the platform.
        /// </summary>
        /// <param name="fromView">
        /// The view that raised the navigation.
        /// </param>
        /// <param name='controller'>
        /// The newly loaded controller.
        /// </param>
        /// <param name='perspective'>
        /// The view perspective returned by the controller load.
        /// </param>
        /// <param name="navigatedUri">
        /// A <see cref="String"/> that represents the uri used to navigate to the controller.
        /// </param>
        protected abstract void OnControllerLoadComplete(IMXView fromView, IMXController controller, string perspective, string navigatedUri);
        private readonly MXViewMap _views = new MXViewMap();

        /// <summary>
        /// Gets the view map.
        /// </summary>
        public virtual MXViewMap Views
        {
            get { return _views; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MXContainer"/> class.
        /// </summary>
        /// <param name="theApp">The application to contain.</param>
        protected MXContainer(MXApplication theApp)
        {
            App = theApp;
        }

        /// <summary>
        /// Gets the MonoCross application in the container.
        /// </summary>
        /// <value>The application as a <see cref="MXApplication"/> instance.</value>
        public MXApplication App { get; private set; }

        /// <summary>
        /// Sets the MonoCross application in the container.
        /// </summary>
        protected static void SetApp(MXApplication app)
        {
            Instance.App = app;
            Instance.App.OnAppLoad();
            Instance.App.OnAppLoadComplete();
        }

        /// <summary>
        /// A delegate for retrieving a container session identifier.
        /// </summary>
        /// <returns>A <see cref="string"/> that uniquely identifies the container's session.</returns>
        public delegate string SessionIdDelegate();

        /// <summary>
        /// Gets the container session identifier
        /// </summary>
        static public SessionIdDelegate GetSessionId;

        /// <summary>
        /// Initializes the <see cref="Instance"/>.
        /// </summary>
        /// <param name="theContainer">The container instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="theContainer"/> is <c>null</c>.</exception>
        protected static void InitializeContainer(MXContainer theContainer)
        {
            Instance = theContainer;
        }

        /// <summary>
        /// Gets or sets the application instance.
        /// </summary>
        /// <value>The application instance.</value>
        public static MXContainer Instance
        {
            get
            {
                object instance;
                Session.TryGetValue(GetSessionId == null ? SessionDictionary.ContainerKey : GetSessionId(), out instance);
                return (instance as MXContainer);
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Cannot have a null MXContainer instance.");
                }

                Session[GetSessionId == null ? SessionDictionary.ContainerKey : GetSessionId()] = value;
                Instance.OnSetDefinitions();
                if (value.App == null) return;
                Instance.App.OnAppLoad();
                Instance.App.OnAppLoadComplete();
            }
        }

        /// <summary>
        /// Gets the current session settings.
        /// </summary>
        public static ISession Session
        {
            get { return _session ?? (_session = new SessionDictionary()); }
        }
        static SessionDictionary _session;

        // Model to View associations

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="view">The initialized view value.</param>
        public static void AddView<TModel>(IMXView view)
        {
            Instance.AddView(typeof(TModel), view.GetType(), ViewPerspective.Default, view);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="perspective">The view's perspective.</param>
        /// <param name="view">The initialized view value.</param>
        public static void AddView<TModel>(IMXView view, string perspective)
        {
            Instance.AddView(typeof(TModel), view.GetType(), perspective, view);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="viewType">The view's type.</param>
        public static void AddView<TModel>(Type viewType)
        {
            Instance.AddView(typeof(TModel), viewType, ViewPerspective.Default, null);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="viewType">The view's type.</param>
        /// <param name="perspective">The view's perspective.</param>
        public static void AddView<TModel>(Type viewType, string perspective)
        {
            Instance.AddView(typeof(TModel), viewType, perspective, null);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="modelType">The type of the view's model.</param>
        /// <param name="viewType">The view's type.</param>
        /// <param name="perspective">The view's perspective.</param>
        protected virtual void AddView(Type modelType, Type viewType, string perspective)
        {
            Instance.AddView(modelType, viewType, perspective, null);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="modelType">The type of the view's model.</param>
        /// <param name="viewType">The view's type.</param>
        /// <param name="perspective">The view perspective.</param>
        /// <param name="view">The initialized view value.</param>
        protected virtual void AddView(Type modelType, Type viewType, string perspective, IMXView view)
        {
            if (view == null)
                Views.Add(perspective, modelType, viewType);
            else
                Views.Add(perspective, view);
        }

        /// <summary>
        /// Initiates a navigation to the specified URL.
        /// </summary>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        public static void Navigate(string url)
        {
            InternalNavigate(null, url, new Dictionary<string, string>());
        }

        /// <summary>
        /// Initiates a navigation to the specified URL.
        /// </summary>
        /// <param name="view">The <see cref="IMXView"/> that kicked off the navigation.</param>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        public static void Navigate(IMXView view, string url)
        {
            InternalNavigate(view, url, new Dictionary<string, string>());
        }

        /// <summary>
        /// Initiates a navigation to the specified URL.
        /// </summary>
        /// <param name="view">The <see cref="IMXView"/> that kicked off the navigation.</param>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        public static void Navigate(IMXView view, string url, Dictionary<string, string> parameters)
        {
            InternalNavigate(view, url, parameters);
        }

        private static void InternalNavigate(IMXView fromView, string url, Dictionary<string, string> parameters)
        {
            MXContainer container = Instance; // optimization for the server side; property reference is a hashed lookup

            // fetch and allocate a viable controller
            var controller = container.GetController(url, ref parameters);
            if (controller != null)
            {
                // Initiate load for the associated controller passing all parameters
                TryLoadController(container, fromView, controller, url, parameters);
            }
        }

        /// <summary>
        /// Tries to execute the Load method of the specified controller using eventing.
        /// </summary>
        /// <param name="container">The container that loads the controller.</param>
        /// <param name="fromView">The view that activated the navigation.</param>
        /// <param name="controller">The controller to load.</param>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        /// <param name="parameters">The parameters to use with the controller's Load method.</param>
        protected static void TryLoadController(MXContainer container, IMXView fromView, IMXController controller, string navigatedUri, Dictionary<string, string> parameters)
        {
            container.OnControllerLoadBegin(controller, fromView);
            container.CancelLoad = false;

            // synchronize load layer to prevent collisions on web-based targets.
            lock (container)
            {
                // Console.WriteLine("InternalNavigate: Locked");

                Action<object> load = (o) =>
                {
                    try
                    {
                        container.LoadController(fromView, controller, navigatedUri, parameters);
                    }
                    catch (Exception ex)
                    {
                        container.OnControllerLoadFailed(controller, ex);
                    }
                };

                // if there is no synchronization, don't launch a new thread
                if (container.ThreadedLoad)
                {
#if NETCF
                    // new thread to execute the Load() method for the layer
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(load));
#else
                    System.Threading.Tasks.Task.Factory.StartNew(() => load(null));
#endif
                }
                else
                {
                    load(null);
                }

                // Console.WriteLine("InternalNavigate: Unlocking");
            }
        }

        private void LoadController(IMXView fromView, IMXController controller, string uri, Dictionary<string, string> parameters)
        {
            string perspective = controller.Load(uri, parameters);
            if (!CancelLoad && perspective != null) // done if failed
            {
                // give the derived container the ability to do something
                // with the fromView if it exists or to create it if it doesn't
                OnControllerLoadComplete(fromView, controller, perspective, uri);
            }
            // clear CancelLoad, we're done
            CancelLoad = false;
        }

        /// <summary>
        /// Cancels loading of the current controller and navigates to the specified url.
        /// </summary>
        /// <param name="url">The url of the controller to navigate to.</param>
        public abstract void Redirect(string url);

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <param name="url">The URL pattern of the controller.</param>
        /// <param name="parameters">The parameters to load into the controller.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">url</exception>
        public virtual IMXController GetController(string url, ref Dictionary<string, string> parameters)
        {
            IMXController controller = null;

            // return if no url provided
            if (url == null)
                throw new ArgumentNullException("url");

            // set last navigation
            LastNavigationDate = DateTime.Now;
            LastNavigationUrl = url;

            // initialize parameter dictionary if not provided
            parameters = parameters ?? new Dictionary<string, string>();

            // for debug
            // Console.WriteLine("Navigating to: " + url);

            // get map object
            MXNavigation navigation = App.NavigationMap.MatchUrl(url);

            // If there is no result, assume the URL is external and create a new Browser View
            if (navigation != null)
            {
                controller = navigation.Controller;
                navigation.ExtractParameters(url, parameters);

                //Add default view parameters without overwriting current ones
                if (navigation.Parameters != null)
                {
                    foreach (var param in navigation.Parameters)
                    {
                        if (!parameters.ContainsKey(param.Key))
                        {
                            parameters.Add(param.Key, param.Value);
                        }
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No controller loaded for URI: " + url);
            }

            return controller;
        }

        /// <summary>
        /// Renders the view described by the perspective.
        /// </summary>
        /// <param name="controller">The controller requesting the view.</param>
        /// <param name="perspective">The perspective describing the view.</param>
        public IMXView RenderViewFromPerspective(IMXController controller, string perspective)
        {
            return RenderViewFromPerspective(controller.ModelType, perspective, controller.GetModel());
        }

        /// <summary>
        /// Renders the view described by the perspective.
        /// </summary>
        /// <param name="modelType">The type of the view's model.</param>
        /// <param name="perspective">The perspective describing the view.</param>
        /// <param name="model">The model for the view.</param>
        public IMXView RenderViewFromPerspective(Type modelType, string perspective, object model)
        {
            IMXView view = Views.GetOrCreateView(modelType, perspective);
            if (view == null)
            {
                // No view perspective found for model
                throw new ArgumentException("No View found for: " + perspective, "perspective");
            }
            view.SetModel(model);
            view.Render();

            return view;
        }

        #region Register/resolve

        private static readonly NamedTypeMap NativeDefinitions = new NamedTypeMap();

        /// <summary>
        /// Registers the specified abstract type and class type for <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        public static void Register<T>(Type nativeType)
        {
            Register<T>(nativeType, null, null);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="namedInstance">An optional unique identifier for the abstract type.</param>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        public static void Register<T>(Type nativeType, string namedInstance)
        {
            Register<T>(nativeType, namedInstance, null);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public static void Register<T>(Type nativeType, Func<object> initialization)
        {
            Register<T>(nativeType, null, initialization);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="namedInstance">An optional unique identifier for the abstract type.</param>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public static void Register<T>(Type nativeType, string namedInstance, Func<object> initialization)
        {
            NativeDefinitions[typeof(T), namedInstance] = new TypeLoader(nativeType, initialization);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for a singleton <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        public static void RegisterSingleton<T>(Type nativeType)
        {
            RegisterSingleton<T>(nativeType, null, null);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for a singleton <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="namedInstance">An optional unique identifier for the abstract type.</param>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        public static void RegisterSingleton<T>(Type nativeType, string namedInstance)
        {
            RegisterSingleton<T>(nativeType, namedInstance, null);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for a singleton <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public static void RegisterSingleton<T>(Type nativeType, Func<object> initialization)
        {
            RegisterSingleton<T>(nativeType, null, initialization);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for a singleton <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="namedInstance">An optional unique identifier for the abstract type.</param>
        /// <param name="nativeType">The type of the class to associate with the abstract type.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public static void RegisterSingleton<T>(Type nativeType, string namedInstance, Func<object> initialization)
        {
            NativeDefinitions[typeof(T), namedInstance] = new TypeLoader(nativeType, true, initialization);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for a singleton <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="instance">The object to associate with the abstract type.</param>
        public static void RegisterSingleton<T>(object instance)
        {
            RegisterSingleton<T>(instance, null);
        }

        /// <summary>
        /// Registers the specified abstract type and class type for a singleton <see cref="Resolve"/>.
        /// </summary>
        /// <typeparam name="T">The type of the abstract to associate with the class type.</typeparam>
        /// <param name="instance">The object to associate with the abstract type.</param>
        /// <param name="namedInstance">An optional unique identifier for the abstract type.</param>
        public static void RegisterSingleton<T>(object instance, string namedInstance)
        {
            NativeDefinitions.Add(namedInstance, typeof(T), new TypeLoader(instance));
        }

        /// <summary>
        /// Resolves the specified abstract type as a concrete instance.
        /// </summary>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        public static T Resolve<T>(params object[] parameters)
        {
            return Resolve<T>(null, parameters);
        }

        /// <summary>
        /// Resolves the specified abstract type as a concrete instance.
        /// </summary>
        /// <param name="name">An optional unique identifier for the abstract type.</param>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        public static T Resolve<T>(string name, params object[] parameters)
        {
            return (T)Resolve(typeof(T), name, parameters);
        }

        /// <summary>
        /// Resolves the specified abstract type as a concrete instance.
        /// </summary>
        /// <param name="type">The abstract type to resolve.</param>
        /// <param name="name">An optional unique identifier for the abstract type.</param>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        public static object Resolve(Type type, string name, params object[] parameters)
        {
            return !NativeDefinitions.ContainsKey(type, name) ? null :
                NativeDefinitions.Resolve(type, name, parameters);
        }

        #endregion
    }
}