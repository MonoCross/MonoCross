using System;

using UIKit;

namespace MonoCross.Utilities.Threading
{
    public class TouchThread : TaskThread
    {
        public override void ExecuteOnMainThread(Delegate method)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() => method.DynamicInvoke());
        }

        public override void ExecuteOnMainThread(Delegate method, object parameter)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() => method.DynamicInvoke(parameter));
        }

		public override void ExecuteOnMainThread(Action action)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() => action.Invoke());
		}

		public override void ExecuteOnMainThread(Action<object> action, object parameter)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() => action.Invoke(parameter));
		}
    }
}
