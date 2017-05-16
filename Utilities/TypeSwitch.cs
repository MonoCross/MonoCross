using System;

namespace MonoCross.Utilities
{
    /// <summary>
    /// A switch statement for Types
    /// </summary>
    public class TypeSwitch
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeSwitch"/>
        /// </summary>
        public TypeSwitch()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeSwitch"/>
        /// </summary>
        /// <param name="o">The object to check against cases.</param>
        public TypeSwitch(object o)
        {
            Object = o;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeSwitch"/>
        /// </summary>
        /// <param name="type">The type to check against cases.</param>
        public TypeSwitch(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeSwitch"/>
        /// </summary>
        /// <param name="o">The object to check against cases.</param>
        /// <param name="type">The specific type to check against cases.</param>
        public TypeSwitch(object o, Type type)
        {
            Object = o;
            Type = type;
        }

        /// <summary>
        /// The object to check for types.
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// The type filter to apply.
        /// </summary>
        public Type Type { get; set; }
    }

    public static class TypeSwitchExtensions
    {
        /// <summary>
        /// Adds a case label to a <see cref="TypeSwitch"/>.
        /// </summary>
        /// <typeparam name="T">The case type.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/>.</param>
        /// <param name="a">The action to perform if <see cref="TypeSwitch.Object"/> matches the case type.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases.</returns>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Action<T> a)
        {
            return Case(typeSwitch, o => true, a, false);
        }

        /// <summary>
        /// Adds a case label to a <see cref="TypeSwitch"/>.
        /// </summary>
        /// <typeparam name="T">The case type.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/>.</param>
        /// <param name="a">The action to perform if <see cref="TypeSwitch.Object"/> matches the case type.</param>
        /// <param name="fallThrough"><c>true</c> to fall-through to any following cases; otherwise <c>false</c>.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases.</returns>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Action<T> a, bool fallThrough)
        {
            return Case(typeSwitch, o => true, a, fallThrough);
        }

        /// <summary>
        /// Adds a case label to a <see cref="TypeSwitch"/>.
        /// </summary>
        /// <typeparam name="T">The case type.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/>.</param>
        /// <param name="c">A filter function that returns <c>true</c> if the object should run the case; otherwise <c>false</c>.</param>
        /// <param name="a">The action to perform if <see cref="TypeSwitch.Object"/> matches the case type.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases.</returns>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Func<T, bool> c, Action<T> a)
        {
            return Case(typeSwitch, c, a, false);
        }

        /// <summary>
        /// Adds a case label to a <see cref="TypeSwitch"/>.
        /// </summary>
        /// <typeparam name="T">The case type.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/>.</param>
        /// <param name="c">A filter function that returns <c>true</c> if the object should run the case; otherwise <c>false</c>.</param>
        /// <param name="a">The action to perform if <see cref="TypeSwitch.Object"/> matches the case type.</param>
        /// <param name="fallThrough"><c>true</c> to fall-through to any following cases; otherwise <c>false</c>.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases.</returns>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Func<T, bool> c, Action<T> a, bool fallThrough)
        {
            if (typeSwitch == null)
            {
                return null;
            }

            var typedObject = typeSwitch.Object is T ? (T)typeSwitch.Object : default(T);
            var type = typeSwitch.Type;
            var caseType = typeof(T);

            if ((typedObject == null || typedObject.Equals(default(T)) || !c(typedObject) || type != null && !Device.Reflector.IsAssignableFrom(caseType, type)) &&
                (type == null || !Device.Reflector.IsAssignableFrom(caseType, type) || !c(default(T)))) return typeSwitch;
            a(typedObject);
            return fallThrough ? typeSwitch : null;
        }

        /// <summary>
        /// Adds a default label to a <see cref="TypeSwitch"/>.
        /// </summary>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/>.</param>
        /// <param name="a">The action to perform if <see cref="TypeSwitch.Object"/> reaches the Default case.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases.</returns>
        public static void Default(this TypeSwitch typeSwitch, Action<object> a)
        {
            if (typeSwitch != null)
                a(typeSwitch.Object);
        }
    }
}