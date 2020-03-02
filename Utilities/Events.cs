using System;
using System.Linq;
using System.Reflection;

namespace MonoCross.Utilities
{
    /// <summary>
    /// Provides methods for event invocation.
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Adds a handler to an event using reflection.
        /// </summary>
        /// <param name="eventObject">The object instance with the event to modify.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handlerObject">The object instance that contains the event handler.</param>
        /// <param name="handlerName">The name of the event handler method.</param>
        public static void AddEventHandler(object eventObject, string eventName, object handlerObject, string handlerName)
        {
            var eventInfo = Device.Reflector.GetEvent(eventObject.GetType(), eventName);
            var eventHandler = Device.Reflector.GetMethod(handlerObject.GetType(), handlerName)
                .CreateDelegate(eventInfo.EventHandlerType, handlerObject);
            eventInfo.AddEventHandler(eventObject, eventHandler);
        }

        /// <summary>
        /// Adds a handler to an event using reflection.
        /// </summary>
        /// <param name="eventObject">The object instance with the event to modify.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The handler to add to the event.</param>
        public static void AddEventHandler(object eventObject, string eventName, Delegate eventHandler)
        {
            var eventInfo = Device.Reflector.GetEvent(eventObject.GetType(), eventName);
            eventInfo.AddEventHandler(eventObject, eventHandler);
        }

        /// <summary>
        /// Removes a handler from an event using reflection.
        /// </summary>
        /// <param name="eventObject">The object instance with the event to modify.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handlerObject">The object instance that contains the event handler.</param>
        /// <param name="handlerName">The name of the event handler method.</param>
        public static void RemoveEventHandler(object eventObject, string eventName, object handlerObject, string handlerName)
        {
            var eventInfo = Device.Reflector.GetEvent(eventObject.GetType(), eventName);
            var eventHandler = Device.Reflector.GetMethod(handlerObject.GetType(), handlerName)
                .CreateDelegate(eventInfo.EventHandlerType, handlerObject);
            eventInfo.RemoveEventHandler(eventObject, eventHandler);
        }

        /// <summary>
        /// Removes a handler from an event using reflection.
        /// </summary>
        /// <param name="eventObject">The object instance with the event to modify.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The handler to remove from the event.</param>
        public static void RemoveEventHandler(object eventObject, string eventName, Delegate eventHandler)
        {
            var eventInfo = Device.Reflector.GetEvent(eventObject.GetType(), eventName);
            eventInfo.RemoveEventHandler(eventObject, eventHandler);
        }

        /// <summary>
        /// Attempts to raise the event with the specified name and <see cref="EventArgs"/> on the current instance.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <param name="eventName">The name of the event to be raised.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event is successfully raised, otherwise <c>false</c>.</returns>
        public static bool RaiseEvent(object obj, string eventName, EventArgs args)
        {
            var invocationList = GetInvocationList(obj, eventName);
            if (invocationList == null) return false;
            foreach (var method in invocationList)
            {
                method.GetMethodInfo().Invoke(method.Target, new[] { obj, args });
            }
            return true;
        }

        /// <summary>
        /// Determines whether an object has a delegate wired up to an event.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns><c>true</c> if the event is present, otherwise <c>false</c>.</returns>
        public static bool HasEvent(object obj, string eventName)
        {
            var invocationList = GetInvocationList(obj, eventName);
            return invocationList != null && invocationList.Any();
        }

        private static Delegate[] GetInvocationList(object obj, string eventName)
        {
            if (obj == null) return null;
            var type = obj.GetType();
            var evt = Device.Reflector.GetEvent(type, eventName);
            if (evt == null) return null;

            FieldInfo info;
            do
            {
                info = Device.Reflector.GetField(type, eventName);
                type = Device.Reflector.GetBaseType(type);
            }
            while (info == null && type != null);

            if (info == null) return null;
            var del = info.GetValue(obj) as MulticastDelegate;
            if (del == null) return null;
            return del.GetInvocationList();
        }

        /// <summary>
        /// Gets an object that represents the method represented by the specified delegate.
        /// </summary>
        /// <param name="method">The delegate to examine.</param>
        /// <returns>An object that represents the method.</returns>
        public static MethodInfo GetMethodInfo(this Delegate method)
        {
            return method.Method;
        }

        /// <summary>
        /// Creates a delegate of the specified type that represents the specified static or instance method, with the specified first argument.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo"/> describing the static or instance method the delegate is to represent.</param>
        /// <param name="type">The <see cref="Type"/> of delegate to create. </param>
        /// <param name="firstArgument">The object to which the delegate is bound, or <c>null</c> to treat <paramref name="method"/> as <c>static</c>.</param>
        /// <returns>A delegate of the specified type that represents the specified static or instance method.</returns>
        public static Delegate CreateDelegate(this MethodInfo method, Type type, object firstArgument)
        {
            return Delegate.CreateDelegate(type, firstArgument, method);
        }
    }
}