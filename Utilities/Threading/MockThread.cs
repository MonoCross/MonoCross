using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MonoCross.Utilities.Threading
{
    /// <summary>
    /// Represents a threading utility for platforms that do not support threading.
    /// </summary>
    public class MockThread : IThread
    {
        /// <summary>
        /// Starts a new thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void Start(ThreadDelegate method) { method.DynamicInvoke(); }

        /// <summary>
        /// Starts a new thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void Start(ParameterDelegate method, object parameter) { method.DynamicInvoke(parameter); }

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void QueueWorker(ParameterDelegate method) { method.DynamicInvoke(); }

        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void QueueWorker(ParameterDelegate method, object parameter) { method.DynamicInvoke(parameter); }

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void QueueIdle(ParameterizedThreadStart method)
        {
            method.DynamicInvoke((object)null);
        }

        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke..</param>
        /// <param name="parameter">The method parameter.</param>
        public void QueueIdle(ParameterizedThreadStart method, object parameter)
        {
            method.DynamicInvoke(parameter);
        }

        /// <summary>
        /// Discards any idle thread.
        /// </summary>
        public void DiscardIdleThread()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the idle queue is enabled.
        /// </summary>
        /// <value><c>true</c> if the idle queue is enabled; otherwise <c>false</c>.</value>
        public bool IdleQueueEnabled
        {
            get { return _idleQueueEnabled; }
            set { _idleQueueEnabled = value; }
        }
        bool _idleQueueEnabled = true;

        /// <summary>
        /// Invokes the specified method on the main thread.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void ExecuteOnMainThread(Delegate method)
        {
            method.DynamicInvoke((object)null);
        }

        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void ExecuteOnMainThread(Delegate method, object parameter)
        {
            method.DynamicInvoke(parameter);
        }

        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        public void ExecuteOnMainThread(Action action)
        {
            action.Invoke();
        }

        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void ExecuteOnMainThread(Action<object> action, object parameter)
        {
            action.Invoke(parameter);
        }
    }
}
