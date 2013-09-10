using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Defines an application's session settings.
    /// </summary>
    public interface ISession : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, ICollection, IEnumerable
    {
        /// <summary>
        /// Removes or resets all session settings.
        /// </summary>
        void Abandon();
    }

    /// <summary>
    /// Represents a <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> that stores an application's session settings.
    /// </summary>
    public class SessionDictionary : SerializableDictionary<string, object>, ISession
    {
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
            object theApp;
            object navMap;
            this.TryGetValue(ContainerKey, out theApp);
            this.TryGetValue(NavKey, out navMap);

            base.Clear();
            if (theApp != null)
                Add(ContainerKey, theApp);
            if (navMap != null)
                Add(NavKey, navMap);
        }

        /// <summary>
        /// Removes or resets all session settings.
        /// </summary>
        public virtual void Abandon()
        {
            this.Clear();
        }
    }
}
