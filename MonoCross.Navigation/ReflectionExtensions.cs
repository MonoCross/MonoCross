using System;
using System.Reflection;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Provides helper extensions for reflection compatibility.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Returns the <see cref="TypeInfo"/> representation of the specified type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to convert.</param>
        /// <returns>The <see cref="TypeInfo"/> representation of the specified type.</returns>
        public static TypeInfo GetTypeInfo(this Type type)
        {
            return new TypeInfo(type);
        }
    }

    /// <summary>
    /// A compatibility wrapper for .NET 4.5
    /// </summary>
    public class TypeInfo
    {
        private Type _type;

        /// <summary>
        /// Initializes a new <see cref="TypeInfo"/> instance.
        /// </summary>
        /// <param name="type">The type being wrapped.</param>
        public TypeInfo(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Public constructors for the given <see cref="Type"/>.
        /// </summary>
        public ConstructorInfo[] DeclaredConstructors { get { return _type.GetConstructors(); } }

        /// <summary>
        /// The interfaces implemented or inherited by the given <see cref="Type"/>.
        /// </summary>
        public Type[] ImplementedInterfaces { get { return _type.GetInterfaces(); } }

        /// <summary>
        /// Determines whether an instance of a specified type can be assigned to the current type instance. 
        /// </summary>
        /// <param name="typeInfo">The type info to compare with the current instance. </param>
        /// <returns><c>true</c> if the given type can be assigned to this instance's type; otherwise <c>false</c>.</returns>
        public bool IsAssignableFrom(TypeInfo typeInfo)
        {
            return _type.IsAssignableFrom(typeInfo._type);
        }
    }
}