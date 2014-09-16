using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        /// <param name='viewPerspective'>
        /// The view perspective returned by the controller load.
        /// </param>
        /// <param name="navigatedUri">
        /// A <see cref="String"/> that represents the uri used to navigate to the controller.
        /// </param>
        protected abstract void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective, string navigatedUri);
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
                Session.TryGetValue(GetSessionId == null ? string.Empty : GetSessionId(), out instance);
                return (instance as MXContainer);
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Cannot have a null MXContainer instance.");
                }

                Session[GetSessionId == null ? string.Empty : GetSessionId()] = value;
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
            Instance.AddView(new MXViewPerspective(typeof(TModel), ViewPerspective.Default), view.GetType(), view);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="perspective">The view's perspective.</param>
        /// <param name="view">The initialized view value.</param>
        public static void AddView<TModel>(IMXView view, string perspective)
        {
            Instance.AddView(new MXViewPerspective(typeof(TModel), perspective), view.GetType(), view);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="viewType">The view's type.</param>
        public static void AddView<TModel>(Type viewType)
        {
            Instance.AddView(new MXViewPerspective(typeof(TModel), ViewPerspective.Default), viewType, null);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="viewType">The view's type.</param>
        /// <param name="perspective">The view's perspective.</param>
        public static void AddView<TModel>(Type viewType, string perspective)
        {
            Instance.AddView(new MXViewPerspective(typeof(TModel), perspective), viewType, null);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="modelType">The type of the view's model.</param>
        /// <param name="viewType">The view's type.</param>
        /// <param name="perspective">The view's perspective.</param>
        protected virtual void AddView(Type modelType, Type viewType, string perspective)
        {
            Instance.AddView(new MXViewPerspective(modelType, perspective), viewType, null);
        }

        /// <summary>
        /// Adds the specified view to the view map.
        /// </summary>
        /// <param name="viewPerspective">The view perspective key.</param>
        /// <param name="viewType">The view's type value.</param>
        /// <param name="view">The initialized view value.</param>
        protected virtual void AddView(MXViewPerspective viewPerspective, Type viewType, IMXView view)
        {
            if (view == null)
                Views.Add(viewPerspective, viewType);
            else
                Views.Add(viewPerspective, view);
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
                    ThreadPool.QueueUserWorkItem(new WaitCallback(load));
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
                var viewPerspective = new MXViewPerspective(controller.ModelType, perspective);

                // give the derived container the ability to do something
                // with the fromView if it exists or to create it if it doesn't
                OnControllerLoadComplete(fromView, controller, viewPerspective, uri);
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
        /// <param name="model">The model for the view.</param>
        /// <param name="perspective">The perspective describing the view.</param>
        public IMXView RenderViewFromPerspective(MXViewPerspective perspective, object model)
        {
            IMXView view = Views.GetOrCreateView(perspective);
            if (view == null)
            {
                // No view perspective found for model
                throw new ArgumentException("No View found for: " + perspective, "perspective");
            }
            view.SetModel(model);
            view.Render();

            return view;
        }


        /// <summary>
        /// Represents a mapping of <see cref="MXViewPerspective"/>s to <see cref="IMXView"/>s in a container.
        /// </summary>
        public class MXViewMap
        {
            readonly Dictionary<MXViewPerspective, object> _viewMap = new Dictionary<MXViewPerspective, object>();

            /// <summary>
            /// Adds the specified view to the view map.
            /// </summary>
            /// <param name="viewPerspective">The view perspective key.</param>
            /// <param name="viewType">The view's type value.</param>
            public void Add(MXViewPerspective viewPerspective, Type viewType)
            {
                if (viewType == null) { throw new ArgumentNullException("viewType"); }

#if NETCF
                if (!viewType.GetInterfaces().Contains(typeof(IMXView)))
#else
                if (!viewType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IMXView)))
#endif
                {
                    throw new ArgumentException("Type provided does not implement IMXView interface.", "viewType");
                }

                _viewMap[viewPerspective] = viewType;
            }

            /// <summary>
            /// Adds the specified view to the view map.
            /// </summary>
            /// <param name="viewPerspective">The view perspective key.</param>
            /// <param name="view">The initialized view value.</param>
            public void Add(MXViewPerspective viewPerspective, IMXView view)
            {
                if (view == null) { throw new ArgumentNullException("view"); }
                _viewMap[viewPerspective] = view;
            }
            /// <summary>
            /// Gets the type of the view described by a view perspective.
            /// </summary>
            /// <param name="viewPerspective">The view perspective.</param>
            /// <returns>The type associated with the view perspective.</returns>
            public Type GetViewType(MXViewPerspective viewPerspective)
            {
                object type;
                _viewMap.TryGetValue(viewPerspective, out type);
                return type as Type;
            }

            /// <summary>
            /// Gets the view described by a view perspective.
            /// </summary>
            /// <param name="viewPerspective">The view perspective.</param>
            /// <returns>The view associated with the view perspective.</returns>
            public IMXView GetView(MXViewPerspective viewPerspective)
            {
                object o;
                _viewMap.TryGetValue(viewPerspective, out o);
                return o as IMXView;
            }

            /// <summary>
            /// Gets the view or creates it if it has not been created.
            /// </summary>
            /// <param name="viewPerspective">The view perspective.</param>
            /// <returns></returns>
            /// <exception cref="System.ArgumentException">Thrown when no view or view type is found in the view map</exception>
            public IMXView GetOrCreateView(MXViewPerspective viewPerspective)
            {
                object o;
                if (!_viewMap.TryGetValue(viewPerspective, out o))
                {
                    // No view
                    throw new ArgumentException("No View for ViewPerspective: " + viewPerspective, "viewPerspective");
                }

                // if we have a type registered and haven't yet created an instance, view will be null
                var view = o as IMXView;
                var viewType = o as Type;

                if (viewType == null)
                    return view;

                // Instantiate an instance of the view from its type
                view = (IMXView)Activator.CreateInstance(viewType);

                return view;
            }

            /// <summary>
            /// Determines whether the view map contains the specified view perspective.
            /// </summary>
            /// <param name="viewPerspective">The view perspective.</param>
            /// <returns><c>true</c> if the view perspective exists; otherwise <c>false</c>.</returns>
            public bool ContainsKey(MXViewPerspective viewPerspective)
            {
                return _viewMap.ContainsKey(viewPerspective);
            }

            /// <summary>
            /// Gets a view perspective from a view's model type.
            /// </summary>
            /// <param name="viewType">Type of the view's Model.</param>
            /// <returns>A <see cref="MXViewPerspective"/> that maps to a view.</returns>
            public MXViewPerspective GetViewPerspectiveForViewType(Type viewType)
            {
                // Check typemap values for either a concrete type or an interface
                var kvp = _viewMap.FirstOrDefault(keyValuePair => viewType == keyValuePair.Value);

                return kvp.Key;
            }
        }
    }
}