using System;
using Android.Runtime;

namespace MonoCross.Utilities.Threading
{
    public class AndroidThread : TaskThread
    {
        [Preserve]
        public AndroidThread() { }

        public override void ExecuteOnMainThread(Delegate method)
        {
            AndroidDevice.Instance.Context.RunOnUiThread(() => method.DynamicInvoke());
        }

        public override void ExecuteOnMainThread(Delegate method, object parameter)
        {
            AndroidDevice.Instance.Context.RunOnUiThread(() => method.DynamicInvoke(parameter));
        }

        public override void ExecuteOnMainThread(Action action)
        {
            AndroidDevice.Instance.Context.RunOnUiThread(() => action());
        }

        public override void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            AndroidDevice.Instance.Context.RunOnUiThread(() => action(parameter));
        }
    }
}
