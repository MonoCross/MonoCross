using System;
using Windows.UI.Core;

namespace MonoCross.Utilities.Threading
{
    public class WindowsThread : TaskThread
    {
        public CoreDispatcher Dispatcher { get; set; }

        public async override void ExecuteOnMainThread(Delegate method)
        {
            if (Dispatcher == null)
            {
                method.DynamicInvoke();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => method.DynamicInvoke());
            }
        }

        public async override void ExecuteOnMainThread(Delegate method, object parameter)
        {
            if (Dispatcher == null)
            {
                method.DynamicInvoke(parameter);
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => method.DynamicInvoke(parameter));
            }
        }

        public async override void ExecuteOnMainThread(Action action)
        {
            if (Dispatcher == null)
            {
                action();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            }
        }

        public async override void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            if (Dispatcher == null)
            {
                action(parameter);
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action(parameter));
            }
        }
    }
}
