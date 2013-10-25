namespace MonoCross.Navigation
{
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
        /// Gets the navigation map containing all of the application's view and their respective URIs.
        /// </summary>
        /// <value>The navigation map as a <see cref="NavigationList"/> instance.</value>
        public NavigationList NavigationMap { get; set; }

        protected MXApplication()
        {
            NavigationMap = new NavigationList();
            NavigateOnLoad = string.Empty;
            OnAppLoad();
        }

        public virtual void OnAppLoad() { }
        public virtual void OnAppLoadComplete() { }
    }
}