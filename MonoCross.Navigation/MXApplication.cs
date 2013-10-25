namespace MonoCross.Navigation
{
    public abstract class MXApplication
    {
        public string NavigateOnLoad { get; set; }
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