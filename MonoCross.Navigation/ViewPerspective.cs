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
}