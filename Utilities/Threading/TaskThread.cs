using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonoCross.Utilities.Threading
{
    public class TaskThread : IThread
    {
        public void Start(ThreadDelegate method)
        {
            Task.Factory.StartNew(() => method.DynamicInvoke(), TaskCreationOptions.LongRunning);
        }

        public void Start(ParameterDelegate method, object parameter)
        {
            Task.Factory.StartNew(() => method(parameter), TaskCreationOptions.LongRunning);
        }

        public void QueueWorker(ParameterDelegate method)
        {
            Task.Factory.StartNew(() => method(null));
        }

        public void QueueWorker(ParameterDelegate method, object parameter)
        {
            Task.Factory.StartNew(() => method(parameter));
        }

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void QueueIdle(ParameterizedThreadStart method)
        {
            IdleThreadQueue.Instance.Enqueue(method, null);
        }

        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke..</param>
        /// <param name="parameter">The method parameter.</param>
        public void QueueIdle(ParameterizedThreadStart method, object parameter)
        {
            IdleThreadQueue.Instance.Enqueue(method, parameter);
        }

        /// <summary>
        /// Discards any idle thread.
        /// </summary>
        public void DiscardIdleThread()
        {
            IdleThreadQueue.Instance.Clear();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the idle queue is enabled.
        /// </summary>
        /// <value><c>true</c> if the idle queue is enabled; otherwise <c>false</c>.</value>
        public bool IdleQueueEnabled
        {
            get
            {
                return IdleThreadQueue.Instance.Enabled;
            }
            set
            {
                IdleThreadQueue.Instance.Enabled = value;
            }
        }

        public virtual void ExecuteOnMainThread(Delegate method)
        {
            if (UiSynchronizationContext != null)
            {
                UiSynchronizationContext.Post(o => method.DynamicInvoke(), null);
            }
            else
            {
                method.DynamicInvoke();
            }
        }

        public virtual void ExecuteOnMainThread(Delegate method, object parameter)
        {
            if (UiSynchronizationContext != null)
            {
                UiSynchronizationContext.Post(o => method.DynamicInvoke(o), parameter);
            }
            else
            {
                method.DynamicInvoke(parameter);
            }
        }

        public virtual void ExecuteOnMainThread(Action action)
        {
            if (UiSynchronizationContext != null)
            {
                UiSynchronizationContext.Post(o => action.DynamicInvoke(), null);
            }
            else
            {
                action.DynamicInvoke();
            }
        }

        public virtual void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            if (UiSynchronizationContext != null)
            {
                UiSynchronizationContext.Post(o => action.DynamicInvoke(o), parameter);
            }
            else
            {
                action.DynamicInvoke(parameter);
            }
        }

        /// <summary>
        /// A <see cref="SynchronizationContext"/> for returning to the UI thread.
        /// </summary>
        public SynchronizationContext UiSynchronizationContext { get; set; }
    }
}
