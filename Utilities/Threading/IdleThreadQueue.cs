using System;
using System.Collections.Generic;
using System.Threading;

namespace MonoCross.Utilities.Threading
{
    /// <summary>
    /// Represents a method sitting in the idle queue and waiting to be invoked.
    /// </summary>
    public class IdleQueueDelegateCall
    {
        /// <summary>
        /// Gets or sets the method to invoke when no longer idle.
        /// </summary>
        public ParameterizedThreadStart Delegate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parameter for the method.
        /// </summary>
        public object Item
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Queues and executes <see cref="IdleQueueDelegateCall"/>s.
    /// </summary>
    public class IdleThreadQueue : Queue<IdleQueueDelegateCall>
    {
        /// <summary>
        /// The maximum number of threads.
        /// </summary>
        private const int MAXTHREADS = 10;

        // An AutoReset event that allows the main thread to block until an exiting thread has decremented the count.
        private static readonly EventWaitHandle ClearCount = new AutoResetEvent(false);
        private static readonly EventWaitHandle ExecutionWaitHandle = new AutoResetEvent(false);
        private readonly object _syncLock = new object();
        //private bool firstEnabled = false;
        private Timer _timer;

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The IdleThreadQueue instance.</value>
        public static IdleThreadQueue Instance
        {
            get { return _queue ?? (_queue = new IdleThreadQueue()); }
        }
        private static IdleThreadQueue _queue;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IdleThreadQueue"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                if (value)
                {
                    TriggerTimer();
                }
            }
        }
        private bool _enabled = true;

        /// <summary>
        /// Gets the number of active threads, in a thread-safe manner.
        /// </summary>
        /// <value>A counter to make sure all threads are started and
        /// blocked before any are released. An int is used to 
        /// remain WinCE compatible, which prevents use of the 
        /// 64-bit Interlocked methods.</value>
        public static long ThreadCountSafeRead
        {
            get { lock (_queue) { return _threadCount; } }
        }

        private static long _threadCount;

        private bool IsIdle
        {
            get
            {
                return DateTime.Now.Subtract(Device.LastActivityDate).TotalSeconds > 10;
            }
        }

        #endregion

        private IdleThreadQueue() { }

        /// <summary>
        /// Enqueues the specified idle queue delegate.
        /// </summary>
        /// <param name="idleQueueDelegate">The idle queue delegate.</param>
        /// <param name="item">The item.</param>
        public void Enqueue(ParameterizedThreadStart idleQueueDelegate, object item)
        {
            var idleDelegateCall = new IdleQueueDelegateCall
            {
                Delegate = idleQueueDelegate,
                Item = item
            };

            lock (_syncLock)
            {
                Enqueue(idleDelegateCall);
            }
            TriggerTimer();
        }

        private void TriggerTimer()
        {
            if (!Enabled)
                return;

            if (_timer != null)
            {
                // execute again in 10 seconds.
                _timer.Change(10 * 1000, Timeout.Infinite);
            }
            else
            {
                _timer = new Timer(new TimerCallback((o) =>
                {
                    ProcessQueue();
                }), null, 1 * 1000, Timeout.Infinite);
            }
        }

        private void ProcessQueue()
        {
            if (!IsIdle)
            {
                TriggerTimer();
                return;
            }

            //iApp.Log.Info("ProcessQueue:  Staging delegates.");
            long lng = ThreadCountSafeRead;

            for (int i = 0; i < MAXTHREADS - lng; i++)
            {
                StageNext();
            }

            // Process queue until all threads have been released or activity occurs.
            while (ThreadCountSafeRead > 0 && IsIdle)
            {
                //iApp.Log.Debug(string.Format("ProcessQueue:  start of loop. staged count {0}  queue count {1}", Interlocked.Read(ref threadCount), this.Count));

                // Signal all ExecuteOnSignal threads to run concurrently.
                ExecutionWaitHandle.Set();
                // The loop continues when clearCount is signalled by any thread.
                ClearCount.WaitOne();
            }

            //iApp.Log.Info( "ProcessQueue:  Calling trigger for next round." );
            TriggerTimer();
        }

        private void ExecuteOnSignal(object obj)
        {
            var call = (IdleQueueDelegateCall)obj;

            // Increment the count of blocked threads.
            Interlocked.Increment(ref _threadCount);

            // Wait on the EventWaitHandle.
            ExecutionWaitHandle.WaitOne();

            // invoke command now.
            call.Delegate.DynamicInvoke(call.Item);

            // alternatively, we could release to thread pool
            //Device.Thread.QueueWorker(call.Delegate, call.Item);

            // refill stage slot from queue
            StageNext();

            // Decrement the count of blocked threads.
            Interlocked.Decrement(ref _threadCount);

            // After signaling ewh, the main thread blocks on
            // clearCount until the signaled thread has 
            // decremented the count. Signal it now.
            ClearCount.Set();
        }

        private void StageNext()
        {
            lock (_syncLock)
            {
                if (Count > 0 && IsIdle)
                {
                    //iApp.Log.Debug(string.Format("StageNext:  Adding delegate to thread. staged count {0}  queue count {1}", ThreadCountSafeRead, this.Count));
                    Device.Thread.Start(ExecuteOnSignal, Dequeue());
                }
            }
        }
    }
}
