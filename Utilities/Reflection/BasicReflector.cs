using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoCross.Utilities
{
    /// <summary>
    /// Represents an object that is able to perform real-time reflection on a <see cref="System.Type"/>.
    /// </summary>
    public class BasicReflector : IReflector
    {
        /// <summary>
        /// Gets the assembly in which the specified <see cref="Type" /> is declared.
        /// </summary>
        /// <param name="type">The type for which to get the declaring assembly.</param>
        /// <returns>
        /// The <see cref="Assembly" /> in which the specified <see cref="Type" /> is declared.
        /// </returns>
        public Assembly GetAssembly(Type type)
        {
            return type.Assembly;
        }

        /// <summary>
        /// Gets all types for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>An enumeration of <see cref="Type"/> objects in the specified <see cref="Assembly"/>.</returns>
        public IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        /// <summary>
        /// Gets the type from which the specified Type directly inherits.
        /// </summary>
        /// <param name="type">The child type.</param>
        /// <returns>
        /// The <see cref="Type" /> from which the current <see cref="Type" /> directly inherits, or <c>null</c> if the current <c>Type</c> represents the <see cref="Object" /> class or an interface.
        /// </returns>
        public Type GetBaseType(Type type)
        {
            return type.BaseType;
        }

        /// <summary>
        /// Searches for the event with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the event.</param>
        /// <returns>
        /// An object that represents the event with the specified name, if found; otherwise, <c>null</c>.
        /// </returns>
        public EventInfo GetEvent(Type type, string name)
        {
            return type.GetEvent(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets all events of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// An enumeration of <see cref="EventInfo" /> objects representing all events of the specified <see cref="Type" />.
        /// </returns>
        public IEnumerable<EventInfo> GetEvents(Type type)
        {
            return type.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Searches for the field with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the field.</param>
        /// <returns>
        /// An object that represents the field with the specified name, if found; otherwise, <c>null</c>.
        /// </returns>
        public FieldInfo GetField(Type type, string name)
        {
            return type.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets all fields of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// An enumeration of <see cref="FieldInfo" /> objects representing all fields of the specified <see cref="Type" />.
        /// </returns>
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets all members of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// An enumeration of <see cref="MemberInfo" /> objects representing all members of the specified <see cref="Type" />.
        /// </returns>
        public IEnumerable<MemberInfo> GetMembers(Type type)
        {
            return type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Searches for the method with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the method.</param>
        /// <returns>
        /// An object that represents the method with the specified name, if found; otherwise, <c>null</c>.
        /// </returns>
        public MethodInfo GetMethod(Type type, string name)
        {
            return type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        /// <summary>
        /// Searches for the method with the specified name and parameter types.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The types of the parameters in the order in which they appear in the method signature.</param>
        /// <returns>
        /// An object that represents the method with the specified name and parameter types, if found; otherwise, <c>null</c>.
        /// </returns>
        public MethodInfo GetMethod(Type type, string name, params Type[] parameterTypes)
        {
            return type.GetMethod(name, parameterTypes);
        }

        /// <summary>
        /// Gets all methods of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// An enumeration of <see cref="MethodInfo" /> objects representing all methods of the specified <see cref="Type" />.
        /// </returns>
        public IEnumerable<MethodInfo> GetMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        /// <summary>
        /// Searches for method overloads with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the method.</param>
        /// <returns>
        /// A collection of objects that represent methods with the specified name.
        /// </returns>
        public IEnumerable<MethodInfo> GetMethods(Type type, string name)
        {
            return GetMethods(type).Where(mi => mi.Name == name);
        }

        /// <summary>
        /// Searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore the case of that part of name that specifies the simple interface name (the part that specifies the namespace must be correctly cased); otherwise <c>false</c> to perform a case-sensitive search for all parts of name.</param>
        /// <returns>
        /// An object representing the interface with the specified name, implemented or inherited by <paramref name="type" />, if found; otherwise <c>null</c>.
        /// </returns>
        public Type GetInterface(Type type, string name, bool ignoreCase)
        {
            return type.GetInterfaces().FirstOrDefault(i => string.Equals(i.Name, name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }

        /// <summary>
        /// Gets all the interfaces implemented or inherited by the specified Type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// An enumeration of Type objects representing all the interfaces implemented or inherited by the specified <see cref="Type" />.
        /// </returns>
        public IEnumerable<Type> GetInterfaces(Type type)
        {
            return type.GetInterfaces();
        }

        /// <summary>
        /// Searches for the property with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>
        /// An object representing the property with the specified name, if found; otherwise, <c>null</c>.
        /// </returns>
        public PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets all properties of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// An enumeration of <see cref="PropertyInfo" /> objects representing all properties of the specified <see cref="Type" />.
        /// </returns>
        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets the first instance of a custom attribute applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type"/>'s inheritance chain to find the attribute; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>The first attribute found that is assignable to <typeparamref name="T"/>.</returns>
        public T GetCustomAttribute<T>(Type type, bool inherit)
            where T : Attribute
        {
            return (T)type.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>
        /// An enumeration of custom attributes applied to the <paramref name="type"/>.
        /// </returns>
        public IEnumerable<Attribute> GetCustomAttributes(Type type, bool inherit)
        {
            return type.GetCustomAttributes(inherit).OfType<Attribute>();
        }

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/> and identified by <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>
        /// An enumeration of custom attributes applied to the <paramref name="type"/>.
        /// </returns>
        public IEnumerable<Attribute> GetCustomAttributes(Type type, Type attributeType, bool inherit)
        {
            return type.GetCustomAttributes(attributeType, inherit).OfType<Attribute>();
        }

        /// <summary>
        /// Gets the first instance of a custom attribute applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="member">The member to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member"/>'s inheritance chain to find the attribute; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>The first attribute found that is assignable to <typeparamref name="T"/>.</returns>
        public T GetCustomAttribute<T>(MemberInfo member, bool inherit)
            where T : Attribute
        {
            return (T)member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>
        /// An enumeration of custom attributes applied to the <paramref name="member"/>.
        /// </returns>
        public IEnumerable<Attribute> GetCustomAttributes(MemberInfo member, bool inherit)
        {
            return member.GetCustomAttributes(inherit).OfType<Attribute>();
        }

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/> and identified by <see cref="Type"/>.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>
        /// An enumeration of custom attributes applied to the <paramref name="member"/>.
        /// </returns>
        public IEnumerable<Attribute> GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit)
        {
            return member.GetCustomAttributes(attributeType, inherit).OfType<Attribute>();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> has an attribute defined.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="attributeType">The type of the attribute to look for.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type" />'s inheritance chain to find the attributes; otherwise <c>false</c>.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> has the attribute defined; otherwise <c>false</c>.
        /// </returns>
        public bool HasAttribute(Type type, Type attributeType, bool inherit)
        {
            return type.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> has an attribute defined.
        /// </summary>
        /// <param name="member">The type to check.</param>
        /// <param name="attributeType">The type of the attribute to look for.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member" />'s inheritance chain to find the attributes; otherwise <c>false</c>.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> has the attribute defined; otherwise <c>false</c>.
        /// </returns>
        public bool HasAttribute(MemberInfo member, Type attributeType, bool inherit)
        {
            return member.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> implements an interface.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="interfaceType">The type of the interface to look for.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> implements the interface; otherwise <c>false</c>.
        /// </returns>
        public bool HasInterface(Type type, Type interfaceType)
        {
            return type == interfaceType || type.GetInterfaces().Any(i => i == interfaceType);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> is abstract and must be overridden.
        /// </summary>
        /// <param name="type">The type to check for abstractness.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> is abstract; otherwise <c>false</c>.
        /// </returns>
        public bool IsAbstract(Type type)
        {
            return type.IsAbstract;
        }

        /// <summary>
        /// Determines whether another type is assignable from the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="c">The type to compare.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="type" /> and <paramref name="c" /> represent the same type,
        /// or if <paramref name="type" /> is in the inheritance hierarchy of <paramref name="c" />,
        /// or if <paramref name="type" /> is an interface that <paramref name="c" /> implements,
        /// or if <paramref name="c" /> is a generic type parameter and <paramref name="type" /> represents one of the constraints of c,
        /// or if <paramref name="c" /> represents a value type and <paramref name="type" /> represents <see cref="Nullable&lt;c&gt;" />.
        /// <c>false</c> if none of these conditions are true, or if <paramref name="c" /> is <c>null</c>.
        /// </returns>
        public bool IsAssignableFrom(Type type, Type c)
        {
            return type.IsAssignableFrom(c);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> is a class; that is, not a value type or interface.
        /// </summary>
        /// <param name="type">The type to check for classhood.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> is a class; otherwise, <c>false</c>.
        /// </returns>
        public bool IsClass(Type type)
        {
            return type.IsClass;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> is an enum.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> is an enum; otherwise <c>false</c>.
        /// </returns>
        public bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> is interface; that is, not a class or a value type.
        /// </summary>
        /// <param name="type">The type to check as an interface.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> is an interface; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInterface(Type type)
        {
            return type.IsInterface;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> is a primitive.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> is a primitive; otherwise <c>false</c>.
        /// </returns>
        public bool IsPrimitive(Type type)
        {
            return type.IsPrimitive;
        }

        /// <summary>
        /// Determines whether the class represented by the current <see cref="Type" /> derives from the class represented by the specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="baseType">Type of the base.</param>
        /// <returns>
        ///   <c>true</c> if the <c>Type</c> represented by the <paramref name="type" /> parameter and the <c>Type</c> represented by the <paramref name="baseType" /> parameter represent classes, and <paramref name="type" />'s class derives from <paramref name="baseType" />; otherwise <c>false</c>.
        /// This method also returns <c>false</c> if <paramref name="type" /> and <paramref name="baseType" /> represent the same class.
        /// </returns>
        public bool IsSubclassOf(Type type, Type baseType)
        {
            return type.IsSubclassOf(baseType);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type" /> is a value type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Type" /> is a value type; otherwise <c>false</c>.
        /// </returns>
        public bool IsValueType(Type type)
        {
            return type.IsValueType;
        }
    }
}
