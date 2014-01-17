using System;
using System.Collections.Generic;

namespace MonoCross.Navigation
{
    /// <summary>
    /// The key for a MXViewPerspective+ID to a <see cref="IMXView"/> value in a <see cref="MXContainer.MXViewMap"/>
    /// </summary>
    public struct MXViewEntry : IComparable, IEqualityComparer<MXViewEntry>
    {
        /// <summary>
        /// Initializes a new <see cref="MXViewEntry"/>instance.
        /// </summary>
        /// <param name="perspective">The perspective describing the mapped <see cref="IMXView"/>.</param>
        /// <param name="id">The identifier of the viewport that displays the <see cref="IMXView"/>.</param>
        public MXViewEntry(MXViewPerspective perspective, string id)
        {
            if (perspective.ModelType == null) throw new TypeInitializationException("MXViewEntry", new ArgumentNullException("perspective"));
            if (id == null) throw new TypeInitializationException("MXViewEntry", new ArgumentNullException("id"));

            Perspective = perspective;
            ID = id;
            _parameters = new Dictionary<string, string>();
            _uri = null;
        }

        /// <summary>
        /// The identifier of the viewport that displays the <see cref="IMXView"/>.
        /// </summary>
        public readonly MXViewPerspective Perspective;

        /// <summary>
        /// The identifier of the viewport that displays the <see cref="IMXView"/>.
        /// </summary>
        public readonly string ID;

        /// <summary>
        /// Gets or sets the URI that was last used to navigate to the <see cref="IMXView"/> that this instance describes.
        /// </summary>
        public string Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        private string _uri;

        /// <summary>
        /// Gets or sets the parameters used to navigate to the view.
        /// </summary>
        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        private Dictionary<string, string> _parameters;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} in viewport {1}", Perspective, ID);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            var p = ((MXViewEntry)obj);
            return GetHashCode() == p.GetHashCode() ? 0 : -1;
        }

        /// <summary>
        /// Checks the specified MXViewPerspectives for equality.
        /// </summary>
        /// <param name="a">The first <see cref="MXViewEntry"/> to check.</param>
        /// <param name="b">The second <see cref="MXViewEntry"/> to check.</param>
        /// <returns><c>true</c> if the parameters are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(MXViewEntry a, MXViewEntry b)
        {
            return a.CompareTo(b) == 0;
        }

        /// <summary>
        /// Checks the specified MXViewPerspectives for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="MXViewEntry"/> to check.</param>
        /// <param name="b">The second <see cref="MXViewEntry"/> to check.</param>
        /// <returns><c>true</c> if the parameters are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(MXViewEntry a, MXViewEntry b)
        {
            return a.CompareTo(b) != 0;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <see name="MXViewEntry" /> to compare.</param>
        /// <param name="y">The second object of type <see name="MXViewEntry" /> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(MXViewEntry x, MXViewEntry y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(MXViewEntry obj)
        {
            return Perspective.GetHashCode() ^ ID.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, (MXViewEntry)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }
}