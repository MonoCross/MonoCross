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

#if NETCF

        /// <summary>
        /// Gets an object that represents the method represented by the specified delegate.
        /// </summary>
        /// <param name="method">The delegate to examine.</param>
        /// <returns>An object that represents the method.</returns>
        public static MethodInfo GetMethodInfo(this Delegate method)
        {
            return method.Method;
        }

#endif
    }
}
