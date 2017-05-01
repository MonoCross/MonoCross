using System;
using System.Collections.Generic;
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

            object retval = null;

            if (_initialize == null)
            {
                var ctors = _instanceType.GetTypeInfo().DeclaredConstructors.ToDictionary(c => c, i => i.GetParameters());

                // Try to find constructor by exact type first, then assignable types.
                for (var i = 1; i >= 0 && retval == null; i--)
                {
                    // Try to match exact parameter number first, then try to match default parameters
                    retval = AttemptConstruction(parameters, ctors.Where(c => c.Value.Length == parameters.Length), Convert.ToBoolean(i)) ??
                             AttemptConstruction(parameters, ctors.Where(c => c.Value.Length > parameters.Length)
                                 .OrderBy(c => c.Value.Length), Convert.ToBoolean(i));
                }

                // If all else fails, throw out provided parameters until something matches.
                if (retval == null)
                {
                    // Try to find constructor by exact type first, then assignable types.
                    for (var i = 1; i >= 0 && retval == null; i--)
                    {
                        retval = AttemptConstruction(parameters, ctors.Where(c => c.Value.Length < parameters.Length)
                                     .OrderByDescending(c => c.Value.Length), Convert.ToBoolean(i));
                    }
                }

                if (retval == null)
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

        private static object AttemptConstruction(object[] parameters, IEnumerable<KeyValuePair<ConstructorInfo, ParameterInfo[]>> ctors, bool matchExactType)
        {
            foreach (var info in ctors)
            {
                var parametersCopy = new object[info.Value.Length];
                for (var i = 0; i < info.Value.Length; i++)
                {
                    var param = info.Value[i];
                    if (parameters != null && parameters.Length > i)
                    {
                        var item = parameters[i];
                        if (item != null && (matchExactType ? param.ParameterType != item.GetType()
                            : !param.ParameterType.GetTypeInfo().IsAssignableFrom(item.GetType().GetTypeInfo())))
                        {
                            parametersCopy = null;
                            break;
                        }
                        parametersCopy[i] = item;
                    }
                    else if ((param.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault)
                    {
                        parametersCopy[i] = param.DefaultValue;
                    }
                    else
                    {
                        parametersCopy[i] = MXContainer.Resolve(param.ParameterType, null, null);
                        if (parametersCopy[i] != null) continue;
                        parametersCopy = null;
                        break;
                    }
                }
                if (parametersCopy != null) return info.Key.Invoke(parametersCopy);
            }
            return null;
        }
    }
}