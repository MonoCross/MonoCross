using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonoCross.Navigation;

namespace MonoCross.Droid
{
    /// <summary>
    /// Droid Specific version of ObservingMXView that invokes the re-render on the Gui thread
    /// </summary>
    /// <typeparam name="T">A model that implements IMXNotifying</typeparam>
    public class MxDroidObservingView<T> : ObservingMXView<T>
        where T : IMXNotifying
    {
        /// <summary>
        /// Reference to the ActivityView that owns this view
        /// </summary>
        private Activity OwningActivityView { get; set; }

        /// <summary>
        /// Must create this view with a reference back to the Activity that owns
        /// </summary>
        private MxDroidObservingView() { }

        /// <summary>
        /// Create the droid specific IMXView implementation passing in the owning activity/view (must inherit from Activity and IMXView). This should normally be "this"
        /// </summary>
        /// <param name="owningActivity">Normally owningActivityView should be passed as "this"</param>
        public MxDroidObservingView(Activity owningActivityView)
        {
            OwningActivityView = owningActivityView;
        }

        /// <summary>
        /// This is the method that gets run when the model changes. In the Android case any UI stuff must happen on the UI thread so we call render there.
        /// </summary>
        protected override void ModelCollectionChangedHandler(object sender, EventArgs args)
        {
            OwningActivityView.RunOnUiThread(() => Render());
        }
    }
}

