using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MonoCross.Utilities.Threading
{
    public class WinPhoneThread : BasicThread
    {
        public override void ExecuteOnMainThread(Delegate method)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => method.DynamicInvoke());
        }

        public override void ExecuteOnMainThread(Delegate method, object parameter)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => method.DynamicInvoke(parameter));
        }

        public override void ExecuteOnMainThread(Action action)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }

        public override void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action, parameter);
        }
    }
}
