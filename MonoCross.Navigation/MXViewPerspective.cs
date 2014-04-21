using System;
using System.Collections.Generic;

namespace MonoCross.Navigation
{
    /// <summary>
    /// Static class containing common perspectives returned by a <see cref="IMXController"/>
    /// </summary>
    public static class ViewPerspective
    {
        /// <summary>
        /// The default ViewPerspective for a model when none is specified.
        /// </summary>
        /// <remarks>This perspective is usually associated with lists or dashboards that describe collections of a model.</remarks>
        public const string Default = "";
        /// <summary>
        /// The ViewPerspective for reading from a model.
        /// </summary>
        public const string Read = "GET";
        /// <summary>
        /// The ViewPerspective for creating a model.
        /// </summary>
        public const string Create = "POST";
        /// <summary>
        /// The ViewPerspective for modifying a model.
        /// </summary>
        public const string Update = "PUT";
        /// <summary>
        /// The ViewPerspective for deleting a model.
        /// </summary>
        public const string Delete = "DELETE";
    }

    /// <summary>
    /// The key for a Perspective+ModelType to a <see cref="IMXView"/> value in a <see cref="MXContainer.Views"/>
    /// </summary>
    public struct MXViewPerspective : IComparable, IEqualityComparer<MXViewPerspective>
    {
        /// <summary>
        /// Initializes a new <see cref="MXViewPerspective"/>instance.
        /// </summary>
        /// <param name="modelType">Type of the model on the mapped <see cref="IMXView"/>.</param>
        /// <param name="perspective">The perspective describing the mapped <see cref="IMXView"/>.</param>
        public MXViewPerspective(Type modelType, string perspective)
        {
            if (modelType == null) throw new TypeInitializationException("MXViewPerspective", new ArgumentNullException("modelType"));
            if (perspective == null) throw new TypeInitializationException("MXViewPerspective", new ArgumentNullException("perspective"));

            Perspective = perspective;
            ModelType = modelType;
        }

        /// <summary>
        /// The perspective describing the mapped <see cref="IMXView"/>. This field is readonly.
        /// </summary>
        public readonly string Perspective;

        /// <summary>
        /// The model type of the mapped <see cref="IMXView"/>. This field is readonly.
        /// </summary>
        public readonly Type ModelType;

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.</returns>
        public int CompareTo(object obj)
        {
            var p = (MXViewPerspective)obj;
            return GetHashCode() == p.GetHashCode() ? 0 : -1;
        }

        /// <summary>
        /// Checks the specified MXViewPerspectives for equality.
        /// </summary>
        /// <param name="a">The first <see cref="MXViewPerspective"/> to check.</param>
        /// <param name="b">The second <see cref="MXViewPerspective"/> to check.</param>
        /// <returns><c>true</c> if the parameters are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(MXViewPerspective a, MXViewPerspective b)
        {
            return a.CompareTo(b) == 0;
        }

        /// <summary>
        /// Checks the specified MXViewPerspectives for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="MXViewPerspective"/> to check.</param>
        /// <param name="b">The second <see cref="MXViewPerspective"/> to check.</param>
        /// <returns><c>true</c> if the parameters are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(MXViewPerspective a, MXViewPerspective b)
        {
            return a.CompareTo(b) != 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>. </returns>
        public override bool Equals(object obj)
        {
            return this == (MXViewPerspective)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return ModelType == null ? -1 : ModelType.GetHashCode() ^ Perspective.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that displays the <see cref="ModelType"/> and <see cref="Perspective"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Model \"{0}\" with perspective  \"{1}\"", ModelType, Perspective);
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <see cref="MXViewPerspective"/> to compare.</param>
        /// <param name="y">The second object of type <see cref="MXViewPerspective"/> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(MXViewPerspective x, MXViewPerspective y)
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
        public int GetHashCode(MXViewPerspective obj)
        {
            return obj.GetHashCode();
        }
    }
}

#if NETCF
namespace System
{
    /// <summary>
    /// .NET Compact Framework replacement for TypeInitializationException
    /// </summary>
    public class TypeInitializationException : TypeLoadException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeInitializationException"/> class.
        /// </summary>
        /// <param name="message">The message associated with this exception.</param>
        /// <param name="exception">The exception that prevented the type from initializing.</param>
        public TypeInitializationException(string message, Exception exception) : base(message, exception) { }
    }
}
#endif