using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonoCross.Utilities.Threading
{
    public delegate void TimerCallback(object state);

    public sealed class Timer : CancellationTokenSource, IDisposable
    {
        TimerCallback _callback;
        object _state;

        public Timer(TimerCallback callback, object state, int dueTime, int period)
        {
            Contract.Assert(period == -1, "This stub implementation only supports dueTime.");
            _callback = callback;
            _state = state;
            Change(dueTime, period);
        }
        public bool Change(int dueTime, int period)
        {
            try
            {
                Task.Delay(dueTime, Token).ContinueWith((t, s) =>
                {
                    var tuple = (Tuple<TimerCallback, object>)s;
                    tuple.Item1(tuple.Item2);
                }, Tuple.Create(_callback, _state), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public new void Dispose() { base.Cancel(); }
    }
}
