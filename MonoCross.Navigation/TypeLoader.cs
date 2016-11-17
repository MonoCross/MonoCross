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
        private readonly Func<object[], object> _initialize;

        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        public TypeLoader(Type instanceType) : this(instanceType, false) { }

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        /// <param name="isSingleton"><c>true</c> to create and cache the instance; otherwise <c>false</c> to create every time.</param>
        public TypeLoader(Type instanceType, bool isSingleton) : this(instanceType, isSingleton, (Func<object[], object>)null) { }

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
        /// <param name="initialization">A method that initializes the object.</param>
        public TypeLoader(Type instanceType, Func<object[], object> initialization) : this(instanceType, false, initialization) { }

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        /// <param name="isSingleton"><c>true</c> to create and cache the instance; otherwise <c>false</c> to create every time.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public TypeLoader(Type instanceType, bool isSingleton, Func<object> initialization)
        {
            if (instanceType == null) throw new ArgumentNullException("instanceType");

            _singletonInstance = isSingleton;
            _instanceType = instanceType;
            _initialize = p => initialization();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new <see cref="TypeLoader"/> instance.
        /// </summary>
        /// <param name="instanceType">The <see cref="System.Type"/> to manage.</param>
        /// <param name="isSingleton"><c>true</c> to create and cache the instance; otherwise <c>false</c> to create every time.</param>
        /// <param name="initialization">A method that initializes the object.</param>
        public TypeLoader(Type instanceType, bool isSingleton, Func<object[], object> initialization)
        {
            if (instanceType == null) throw new ArgumentNullException("instanceType");

            _singletonInstance = isSingleton;
            _instanceType = instanceType;
            _initialize = initialization;
            _instance = null;
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
                            .OrderBy(i => i.param.Length)
                            .Select(p => new { p.ctor, param = p.param.Select(res => MXContainer.Resolve(res.ParameterType, null, null)) })
                            .FirstOrDefault(v => v.param.All(o => o != null));

                        retval = info != null ? info.ctor.Invoke(info.param.ToArray()) : Activator.CreateInstance(_instanceType);
                    }
                    else
                    {
                        var ctor = ctors.FirstOrDefault(c =>
                        {
                            var p = c.GetParameters();
                            return p.Length >= parameters.Length && !p.Any(t => t.Position < parameters.Length && parameters[t.Position] != null &&
                                    !t.ParameterType.GetTypeInfo().IsAssignableFrom(parameters[t.Position].GetType().GetTypeInfo()));
                        });
                        if (ctor == null)
                        {
                            retval = Activator.CreateInstance(_instanceType);
                        }
                        else
                        {
                            var paramTypes = ctor.GetParameters();
                            if (paramTypes.Length > parameters.Length)
                            {
                                var index = parameters.Length;
                                Array.Resize(ref parameters, paramTypes.Length);
                                for (; index < parameters.Length; index++)
                                {
                                    parameters[index] = paramTypes[index].DefaultValue;
                                }
                            }
                            retval = ctor.Invoke(parameters);
                        }
                    }
                }
                catch (MissingMemberException)
                {
                    retval = Activator.CreateInstance(_instanceType);
                }
            }
            else
            {
                retval = _initialize(parameters);
            }

            if (_singletonInstance)
            {
                _instance = retval;
            }

            return retval;
        }
    }
}