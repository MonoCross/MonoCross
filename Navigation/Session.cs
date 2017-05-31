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

        /// <summary>
        /// Gets a collection of entries to persist through a <see cref="ICollection&lt;T&gt;.Clear()"/>
        /// </summary>
        List<string> SafeKeys { get; }
    }

    /// <summary>
    /// Represents a <see cref="SerializableDictionary&lt;TKey,TValue&gt;"/> that stores an application's session settings.
    /// </summary>
    public class SessionDictionary : SerializableDictionary<string, object>, ISession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionDictionary"/> class.
        /// </summary>
        public SessionDictionary()
        {
            SafeKeys = new List<string>();
            SafeKeys.Add(ContainerKey);
            SafeKeys.Add(NavKey);
            SafeKeys.Add(ViewsKey);
        }

        /// <summary>
        /// The key used to keep the <see cref="MXContainer"/> instance in session.
        /// </summary>
        public const string ContainerKey = "theContainer";

        /// <summary>
        /// The key used to keep the <see cref="NavigationList"/> instance in session.
        /// </summary>
        public const string NavKey = "navMap";

        /// <summary>
        /// The key used to keep the <see cref="MXViewMap"/> instance in session.
        /// </summary>
        public const string ViewsKey = "Views";

        /// <summary>
        /// Removes all keys and values from the <see cref="SessionDictionary"/> with the exception of the application and navigation map objects.
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
        /// Gets a collection of entries to persist through a <see cref="Clear"/>
        /// </summary>
        public List<string> SafeKeys { get; private set; }

        /// <summary>
        /// Removes or resets all session settings.
        /// </summary>
        public virtual void Abandon()
        {
            Clear();
        }
    }
}