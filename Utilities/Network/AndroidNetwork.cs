using Android.Runtime;
using MonoCross.Navigation;

namespace MonoCross.Utilities.Networking
{
    class AndroidNetwork : NetworkAsynch
    {
        private readonly IFetcher _fetcher;

        [Preserve]
        public AndroidNetwork()
        {
            _fetcher = MXContainer.Resolve<IFetcher>();
        }

        [Preserve]
        public AndroidNetwork(IFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public override IFetcher Fetcher
        {
            get { return _fetcher; }
        }

        public override IPoster Poster
        {
            get
            {
                return new PosterAsynch();
            }
        }
    }
}
