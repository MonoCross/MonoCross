using System;
using System.Windows.Forms;

namespace MonoCross.Utilities.Threading
{
    public class CompactFrameworkThread : BasicThread
    {
        public Control DispatcherSource
        {
            get;
            set;
        }

        public override void ExecuteOnMainThread(Delegate method)
        {
            DispatcherSource.Invoke(method);
        }

        public override void ExecuteOnMainThread(Delegate method, object parameter)
        {
            DispatcherSource.Invoke(method, parameter);
        }

        public override void ExecuteOnMainThread(Action action)
        {
            DispatcherSource.Invoke(action);
        }

        public override void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            DispatcherSource.Invoke(action, parameter);
        }
    }
}
