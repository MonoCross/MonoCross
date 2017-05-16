using System;

namespace MonoCross.Utilities.Threading
{
    /// <summary>
    /// Represents a threading utility with no implementation.  This is compatible with all platforms and targets, and it is useful
    /// for when a concrete class is required but no implementation is necessary.
    /// </summary>
    public class NullThread : IThread
    {
        /// <summary>
        /// Starts a new thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void Start(ThreadDelegate method) { }

        /// <summary>
        /// Starts a new thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void Start(ParameterDelegate method, object parameter) { }

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void QueueWorker(ParameterDelegate method) { }

        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void QueueWorker(ParameterDelegate method, object parameter) { }

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void QueueIdle( ParameterizedThreadStart method )
        {
        }

        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke..</param>
        /// <param name="parameter">The method parameter.</param>
        public void QueueIdle( ParameterizedThreadStart method, object parameter )
        {
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
			get 
			{
				return false;
			}
			set
			{ }
		}

        /// <summary>
        /// Invokes the specified method on the main thread.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public void ExecuteOnMainThread(Delegate method)
        {
        }

        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void ExecuteOnMainThread(Delegate method, object parameter)
        {
        }

		/// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        public void ExecuteOnMainThread(Action action) { }

		/// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        public void ExecuteOnMainThread(Action<object> action, object parameter) { }
    }
}
