namespace MonoCross.Navigation
{
    /// <summary>
    /// Represents a cross-platform MonoCross application.  This class is abstract.
    /// </summary>
    public abstract class MXApplication
    {
        /// <summary>
        /// Gets or sets the URI to navigate to once the application has loaded.
        /// </summary>
        /// <value>The URL as a <see cref="string"/> value.</value>
        public string NavigateOnLoad { get; set; }

        /// <summary>
        /// Gets or sets the application's title.
        /// </summary>
        /// <value>The title as a <see cref="string"/> value.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets the navigation map that associates the application's controllers to their respective URIs.
        /// </summary>
        /// <value>The navigation map as a <see cref="NavigationList"/> instance.</value>
        public NavigationList NavigationMap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MXApplication"/> class.
        /// </summary>
        protected MXApplication()
        {
            NavigationMap = new NavigationList();
            NavigateOnLoad = string.Empty;
        }

        /// <summary>
        /// Called when the application instance is loaded. This method is meant to be overridden in consuming applications 
        /// for application-level initialization code.
        /// </summary>
        public virtual void OnAppLoad() { }

        /// <summary>
        /// Called by the container when the application load is complete. This method is meant to be overridden in consuming applications 
        /// for application-level initialization code.
        /// </summary>
        public virtual void OnAppLoadComplete() { }
    }
}