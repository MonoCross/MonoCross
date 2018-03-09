using System;
using System.Windows.Threading;

namespace MonoCross.Utilities.Threading
{
    public class DispatcherThread : TaskThread
    {
        public Dispatcher Dispatcher { get; set; }

        public override void ExecuteOnMainThread(Delegate d)
        {
            if (Dispatcher == null)
            {
                d.DynamicInvoke();
            }
            else
            {
                Dispatcher.BeginInvoke(d);
            }
        }

        public override void ExecuteOnMainThread(Delegate d, object parameter)
        {
            if (Dispatcher == null)
            {
                d.DynamicInvoke(parameter);
            }
            else
            {
                Dispatcher.BeginInvoke(d, parameter);
            }
        }

        public override void ExecuteOnMainThread(Action action)
        {
            if (Dispatcher == null)
            {
                action();
            }
            else
            {
                Dispatcher.BeginInvoke(action);
            }
        }

        public override void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            if (Dispatcher == null)
            {
                action(parameter);
            }
            else
            {
                Dispatcher.BeginInvoke(action, parameter);
            }
        }
    }
}