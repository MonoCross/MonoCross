using System;
using System.Threading;

namespace MonoCross.Utilities.Threading
{
#if NETCF
    public class SynchronizationContext
    {
        public static SynchronizationContext Current
        {
            get
            {
                return new SynchronizationContext();
            }
        }
        public static void SetSynchronizationContext(SynchronizationContext current)
        { }
        public void Post(ParameterizedThreadStart threadStart, object parameter)
        {
            Device.Thread.ExecuteOnMainThread(threadStart, parameter);
        }
    }

    public static class DelegateExtensions
    {
        public static void DynamicInvoke(this Delegate dlg, params object[] args)
        {
            var list = dlg.GetInvocationList();
            foreach (var del in list)
            {
                del.Method.Invoke(del.Target, System.Reflection.BindingFlags.Default, null, args, null);
            }
        }
    }
#endif

    /// <summary>
    /// Represents a basic threading utility that works across multiple platforms.
    /// </summary>
    public class BasicThread : IThread
    {
        /// <summary>
        /// Starts a new thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void Start(ThreadDelegate method)
        {
            SynchronizationContext context = SynchronizationContext.Current;

            // if there is no synchronization, don't launch a new thread
            if (context != null)
            {
                // new thread to execute the Load() method for the layer
                new Thread(() => method.DynamicInvoke()).Start();
            }
            else
            {
                method.DynamicInvoke();
            }
        }

        /// <summary>
        /// Starts a new thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void Start(ParameterDelegate method, object parameter)
        {
            SynchronizationContext context = SynchronizationContext.Current;

            // if there is no synchronization, don't launch a new thread
            if (context != null)
            {
                // new thread to execute the Load() method for the layer
                new Thread(() => method.DynamicInvoke(parameter)).Start();
            }
            else
            {
                method.DynamicInvoke(parameter);
            }
        }

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void QueueWorker(ParameterDelegate method)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(method));
        }

        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void QueueWorker(ParameterDelegate method, object parameter)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(method), parameter);
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

        /// <summary>
        /// Invokes the specified method on the main thread.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public virtual void ExecuteOnMainThread(Delegate method)
        {
            method.DynamicInvoke();
        }

        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public virtual void ExecuteOnMainThread(Delegate method, object parameter)
        {
            method.DynamicInvoke(parameter);
        }

        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        public virtual void ExecuteOnMainThread(Action action)
        {
            action();
        }

        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public virtual void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            action(parameter);
        }
    }
}
