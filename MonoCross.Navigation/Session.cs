using System.Collections;
using System.Collections.Generic;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Defines an application's session settings.
    /// </summary>
    public interface ISession : IDictionary<string, object>, ICollection
    {
        /// <summary>
        /// Removes or resets all session settings.
        /// </summary>
        void Abandon();
    }

    /// <summary>
    /// Represents a <see cref="SerializableDictionary{TKey,TValue}"/> that stores an application's session settings.
    /// </summary>
    public class SessionDictionary : SerializableDictionary<string, object>, ISession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionDictionary"/> class.
        /// </summary>
        public SessionDictionary()
        {
            SafeKeys.Add(ContainerKey);
            SafeKeys.Add(NavKey);
        }

        /// <summary>
        /// The key used for the container object.
        /// </summary>
        public const string ContainerKey = "theContainer";

        /// <summary>
        /// The key used for the navigation map object.
        /// </summary>
        public const string NavKey = "navMap";

        /// <summary>
        /// Removes all keys and values from the <see cref="SessionDictionary"/> with the except of the application and navigation map objects.
        /// </summary>
        public override void Clear()
        {
            var safeEntries = new Dictionary<string, object>();
            foreach (var key in SafeKeys)
            {
                object entry;
                TryGetValue(key, out entry);
                if (entry != null) safeEntries.Add(key, entry);
            }

            base.Clear();
            foreach (var pair in safeEntries)
            {
                Add(pair);
            }
        }

        /// <summary>
        /// Keys of entries to persist through a <see cref="Clear"/>
        /// </summary>
        public readonly List<string> SafeKeys = new List<string>();

        /// <summary>
        /// Removes or resets all session settings.
        /// </summary>
        public virtual void Abandon()
        {
            Clear();
        }
    }
}