using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCross
{
    /// <summary>
    /// Defines an object for reflecting on a <see cref="System.Type"/> in real-time.
    /// </summary>
    public interface IReflector
    {
        /// <summary>
        /// Gets the assembly in which the specified <see cref="Type"/> is declared.
        /// </summary>
        /// <param name="type">The type for which to get the declaring assembly.</param>
        /// <returns>The <see cref="Assembly"/> in which the specified <see cref="Type"/> is declared.</returns>
        Assembly GetAssembly(Type type);

        /// <summary>
        /// Gets all types for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>An enumeration of <see cref="Type"/> objects in the specified <see cref="Assembly"/>.</returns>
        IEnumerable<Type> GetTypes(Assembly assembly);

        /// <summary>
        /// Gets the type from which the specified <see cref="Type"/> directly inherits.
        /// </summary>
        /// <param name="type">The child type.</param>
        /// <returns>The <see cref="Type"/> from which the current <see cref="Type"/> directly inherits, or <c>null</c> if the current <c>Type</c> represents the <see cref="Object"/> class or an interface.</returns>
        Type GetBaseType(Type type);

        /// <summary>
        /// Searches for the event with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the event.</param>
        /// <returns>
        /// An object that represents the event with the specified name, if found; otherwise, <c>null</c>.
        /// </returns>
        EventInfo GetEvent(Type type, string name);

        /// <summary>
        /// Gets all events of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>An enumeration of <see cref="EventInfo"/> objects representing all events of the specified <see cref="Type"/>.</returns>
        IEnumerable<EventInfo> GetEvents(Type type);

        /// <summary>
        /// Searches for the field with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the field.</param>
        /// <returns>
        /// An object that represents the field with the specified name, if found; otherwise, <c>null</c>.
        /// </returns>
        FieldInfo GetField(Type type, string name);

        /// <summary>
        /// Gets all fields of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>An enumeration of <see cref="FieldInfo"/> objects representing all fields of the specified <see cref="Type"/>.</returns>
        IEnumerable<FieldInfo> GetFields(Type type);

        /// <summary>
        /// Gets all members of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>An enumeration of <see cref="MemberInfo"/> objects representing all members of the specified <see cref="Type"/>.</returns>
        IEnumerable<MemberInfo> GetMembers(Type type);

        /// <summary>
        /// Searches for the method with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the method.</param>
        /// <returns>An object that represents the method with the specified name, if found; otherwise, <c>null</c>.</returns>
        MethodInfo GetMethod(Type type, string name);

        /// <summary>
        /// Searches for the method with the specified name and parameter types.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The types of the parameters in the order in which they appear in the method signature.</param>
        /// <returns>An object that represents the method with the specified name and parameter types, if found; otherwise, <c>null</c>.</returns>
        MethodInfo GetMethod(Type type, string name, params Type[] parameterTypes);

        /// <summary>
        /// Gets all methods of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>An enumeration of <see cref="MethodInfo" /> objects representing all methods of the specified <see cref="Type" />.</returns>
        IEnumerable<MethodInfo> GetMethods(Type type);

        /// <summary>
        /// Searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore the case of that part of name that specifies the simple interface name (the part that specifies the namespace must be correctly cased); otherwise <c>false</c> to perform a case-sensitive search for all parts of name.</param>
        /// <returns>An object representing the interface with the specified name, implemented or inherited by <paramref name="type"/>, if found; otherwise <c>null</c>.</returns>
        Type GetInterface(Type type, string name, bool ignoreCase);

        /// <summary>
        /// Gets all the interfaces implemented or inherited by the specified Type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>An enumeration of Type objects representing all the interfaces implemented or inherited by the specified <see cref="Type"/>.</returns>
        IEnumerable<Type> GetInterfaces(Type type);

        /// <summary>
        /// Searches for the property with the specified name.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>An object representing the property with the specified name, if found; otherwise, <c>null</c>.</returns>
        PropertyInfo GetProperty(Type type, string name);

        /// <summary>
        /// Gets all properties of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>An enumeration of <see cref="PropertyInfo"/> objects representing all properties of the specified <see cref="Type"/>.</returns>
        IEnumerable<PropertyInfo> GetProperties(Type type);

        /// <summary>
        /// Gets the first instance of a custom attribute applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type"/>'s inheritance chain to find the attribute; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>The first attribute found that is assignable to <typeparamref name="T"/>.</returns>
        T GetCustomAttribute<T>(Type type, bool inherit) where T : Attribute;

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>An enumeration of custom attributes applied to the <paramref name="type"/>.</returns>
        IEnumerable<Attribute> GetCustomAttributes(Type type, bool inherit);

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/> and identified by <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>An enumeration of custom attributes applied to the <paramref name="type"/>.</returns>
        IEnumerable<Attribute> GetCustomAttributes(Type type, Type attributeType, bool inherit);
        /// <summary>
        /// Gets the first instance of a custom attribute applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="member">The type to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member"/>'s inheritance chain to find the attribute; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>The first attribute found that is assignable to <typeparamref name="T"/>.</returns>
        T GetCustomAttribute<T>(MemberInfo member, bool inherit) where T : Attribute;

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="member">The type to check.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>An enumeration of custom attributes applied to the <paramref name="member"/>.</returns>
        IEnumerable<Attribute> GetCustomAttributes(MemberInfo member, bool inherit);

        /// <summary>
        /// Gets the custom attributes applied to the specified <see cref="Type"/> and identified by <see cref="Type"/>.
        /// </summary>
        /// <param name="member">The type to check.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member"/>'s inheritance chain to find the attributes; otherwise <c>false</c>. This parameter is ignored for properties and events.</param>
        /// <returns>An enumeration of custom attributes applied to the <paramref name="member"/>.</returns>
        IEnumerable<Attribute> GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> has an attribute defined.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="attributeType">The type of the attribute to look for.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="type"/>'s inheritance chain to find the attributes; otherwise <c>false</c>.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> has the attribute defined; otherwise <c>false</c>.</returns>
        bool HasAttribute(Type type, Type attributeType, bool inherit);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> has an attribute defined.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <param name="attributeType">The type of the attribute to look for.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="member"/>'s inheritance chain to find the attributes; otherwise <c>false</c>.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> has the attribute defined; otherwise <c>false</c>.</returns>
        bool HasAttribute(MemberInfo member, Type attributeType, bool inherit);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> implements an interface.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="interfaceType">The type of the interface to look for.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> implements the interface; otherwise <c>false</c>.</returns>
        bool HasInterface(Type type, Type interfaceType);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is abstract and must be overridden.
        /// </summary>
        /// <param name="type">The type to check for abstractness.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> is abstract; otherwise <c>false</c>.</returns>
        bool IsAbstract(Type type);

        /// <summary>
        /// Determines whether another type is assignable from the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="baseType">The type to be assigned.</param>
        /// <param name="type">The type to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="baseType"/> and <paramref name="type"/> represent the same type,
        /// or if <paramref name="baseType"/> is in the inheritance hierarchy of <paramref name="type"/>,
        /// or if <paramref name="baseType"/> is an interface that <paramref name="type"/> implements,
        /// or if <paramref name="type"/> is a generic type parameter and <paramref name="baseType"/> represents one of type's constraints,
        /// or if <paramref name="type"/> represents a value type and <paramref name="baseType"/> represents <see cref="Nullable&lt;type&gt;"/>.
        /// <c>false</c> if none of these conditions are true, or if <paramref name="type"/> is <c>null</c>.
        /// </returns>
        bool IsAssignableFrom(Type baseType, Type type);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is a class; that is, not a value type or interface.
        /// </summary>
        /// <param name="type">The type to check for classhood.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> is a class; otherwise, <c>false</c>.</returns>
        bool IsClass(Type type);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is an enum.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> is an enum; otherwise <c>false</c>.</returns>
        bool IsEnum(Type type);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is interface; that is, not a class or a value type.
        /// </summary>
        /// <param name="type">The type to check as an interface.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> is an interface; otherwise, <c>false</c>.</returns>
        bool IsInterface(Type type);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is a primitive.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> is a primitive; otherwise <c>false</c>.</returns>
        bool IsPrimitive(Type type);

        /// <summary>
        /// Determines whether the class represented by the specified <see cref="Type"/> derives from the class represented by the another <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="baseType">Type of the base.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> or <paramref name="baseType"/> parameter is <c>null</c>.</exception>
        /// <returns>
        /// <c>true</c> if the <c>Type</c> represented by the <paramref name="type"/> parameter and the <c>Type</c> represented by the <paramref name="baseType"/> parameter represent classes, and <paramref name="type"/>'s class derives from <paramref name="baseType"/>; otherwise <c>false</c>.
        /// This method also returns <c>false</c> if <paramref name="type"/> and <paramref name="baseType"/> represent the same class.
        /// </returns>
        bool IsSubclassOf(Type type, Type baseType);

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is a value type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> is a value type; otherwise <c>false</c>.</returns>
        bool IsValueType(Type type);
    }
}
