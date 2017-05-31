using System;

namespace MonoCross.Utilities.Threading
{
    /// <summary>
    /// Represents the method that will be invoked on a new thread.
    /// </summary>
    public delegate void ThreadDelegate();

    /// <summary>
    /// Represents the method that will be invoked on a new thread with a parameter.
    /// </summary>
    /// <param name="parameter">The method parameter.</param>
    public delegate void ParameterDelegate(object parameter);

    /// <summary>
    /// Represents the method that will be invoked on a new thread with a parameter.
    /// </summary>
    /// <param name="obj">The method parameter.</param>
    public delegate void ParameterizedThreadStart(Object obj);

    /// <summary>
    /// Defines an abstract threading utility.
    /// </summary>
    public interface IThread
    {
        /// <summary>
        /// Starts a new thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        void Start(ThreadDelegate method);
        /// <summary>
        /// Starts a new thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        void Start(ParameterDelegate method, object parameter);

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        void QueueWorker(ParameterDelegate method);
        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        void QueueWorker(ParameterDelegate method, object parameter);

        /// <summary>
        /// Queues a new worker thread invoking the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        void QueueIdle(ParameterizedThreadStart method );
        /// <summary>
        /// Queues a new worker thread invoking the specified method with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke..</param>
        /// <param name="parameter">The method parameter.</param>
        void QueueIdle(ParameterizedThreadStart method, object parameter );

        /// <summary>
        /// Discards any idle thread.
        /// </summary>
        void DiscardIdleThread();
        /// <summary>
        /// Gets or sets a value indicating whether the idle queue is enabled.
        /// </summary>
        /// <value><c>true</c> if the idle queue is enabled; otherwise <c>false</c>.</value>
		bool IdleQueueEnabled { get; set; }

        // TODO: implement for ALL environments, unimplemented versions throw exceptions, only Android for now!
        /// <summary>
        /// Invokes the specified method on the main thread.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        void ExecuteOnMainThread(Delegate method);
        /// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        void ExecuteOnMainThread(Delegate method, object parameter);
		/// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        void ExecuteOnMainThread(Action action);
		/// <summary>
        /// Invokes the specified method on the main thread with the parameter provided.
        /// </summary>
        /// <param name="action">The method to invoke.</param>
        /// <param name="parameter">The method parameter.</param>
        void ExecuteOnMainThread(Action<object> action, object parameter);
    }

}
