using System;
using System.Linq;
using System.Reflection;

namespace MonoCross.Navigation
{
    /// <summary>
    /// A wrapper for initializing and managing objects by type.
    /// </summary>
    public struct TypeLoader
    {
        private readonly bool _singletonInstance;
        private object _instance;
        private readonly Type _instanceType;
        private readonly Func<object> _initialize;

        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        public TypeLoader(Type instanceType) : this(instanceType, false, null) { }

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        /// <param name="isSingleton"><c>true</c> to create and cache the instance; otherwise <c>false</c> to create every time.</param>
        public TypeLoader(Type instanceType, bool isSingleton) : this(instanceType, isSingleton, null) { }

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public TypeLoader(Type instanceType, Func<object> initialization) : this(instanceType, false, initialization) { }

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        /// <param name="isSingleton"><c>true</c> to create and cache the instance; otherwise <c>false</c> to create every time.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public TypeLoader(Type instanceType, bool isSingleton, Func<object> initialization)
        {
            _singletonInstance = isSingleton;
            _instanceType = instanceType;
            _initialize = initialization;
            _instance = null;

            if (instanceType == null) throw new ArgumentNullException("instanceType");
        }

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instance">The singleton object to manage.</param>
        public TypeLoader(object instance)
        {
            _singletonInstance = true;
            _instance = instance;
            _instanceType = instance.GetType();
            _initialize = null;
        }

        #endregion

        /// <summary>
        /// The initialized object of type <see cref="Type"/>, if it exists.
        /// </summary>
        public object Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// The type managed by this instance.
        /// </summary>
        public Type Type
        {
            get { return _instanceType; }
        }

        /// <summary>
        /// Returns the managed instance.
        /// </summary>
        /// <typeparam name="T">The type for casting the result.</typeparam>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        /// <returns>The object instance cast to a <typeparamref name="T"/>.</returns>
        public T Load<T>(params object[] parameters)
        {
            return (T)Load(parameters);
        }

        /// <summary>
        /// Returns the managed instance.
        /// </summary>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        /// <returns>The object instance.</returns>
        public object Load(params object[] parameters)
        {
            if (_instanceType == null)
            {
                return null;
            }

            if (Instance != null)
            {
                return Instance;
            }

            object retval;

            if (_initialize == null)
            {
                try
                {
                    var ctors = _instanceType.GetTypeInfo().DeclaredConstructors;
                    if (parameters == null || parameters.Length == 0)
                    {
                        var info = ctors.Select(c => new { ctor = c, param = c.GetParameters() })
                            .OrderByDescending(i => i.param.Length)
                            .Select(p => new { p.ctor, param = p.param.Select(res => MXContainer.Resolve(res.ParameterType, null, null)) })
                            .FirstOrDefault(v => v.param.All(o => o != null));

                        retval = info != null ? info.ctor.Invoke(info.param.ToArray()) : Activator.CreateInstance(_instanceType);
                    }
                    else
                    {
                        var ctor = ctors.FirstOrDefault(c =>
                        {
                            var p = c.GetParameters();
                            return p.Length == parameters.Length && !p.Where((t, i) => parameters[i] == null || !parameters[i].GetType().GetTypeInfo().IsAssignableFrom(t.ParameterType.GetTypeInfo())).Any();
                        });
                        retval = ctor == null ? Activator.CreateInstance(_instanceType) : ctor.Invoke(parameters);
                    }
                }
                catch (MissingMemberException)
                {
                    retval = Activator.CreateInstance(_instanceType);
                }
            }
            else
            {
                retval = _initialize();
            }

            if (_singletonInstance)
            {
                _instance = retval;
            }

            return retval;
        }
    }
}