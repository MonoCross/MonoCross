using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace MonoCross.Utilities
{
    public class SyncQueue<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        // based on information contained in http://yacsharpblog.blogspot.com/2008/07/thread-synchronized-queing.html

        private WaitHandle[] handles = { new AutoResetEvent(false), new ManualResetEvent(false), };
        private Queue<T> _q = new Queue<T>();

        // To-Do: replace lock(_q) with lock(queueLock) or something similar.
        // to avoid lock(this) problems http://bytes.com/topic/c-sharp/answers/242087-whats-wrong-lock
        //object queueLock = new object();

        public int Count { get { lock (_q) { return _q.Count; } } }
        public T Peek()
        {
            lock (_q)
            {
                if (_q.Count > 0)
                    return _q.Peek();
            }
            return default(T);
        }

        public void Enqueue(T element)
        {
            lock (_q)
            {
                _q.Enqueue(element);
                ((AutoResetEvent)handles[0]).Set();
            }
        }

        public T Dequeue(int timeout_milliseconds)
        {
            T element;
            try
            {
                if (WaitHandle.WaitAny(handles, timeout_milliseconds) == 0)
                {
                    lock (_q)
                    {
                        if (_q.Count > 0)
                        {
                            element = _q.Dequeue();
                            if (_q.Count > 0)
                                ((AutoResetEvent)handles[0]).Set();
                            return element;
                        }
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }


        public T Dequeue()
        {
            return Dequeue(-1);
        }

        public void Interrupt()
        {
            ((ManualResetEvent)handles[1]).Set();
        }
        public void Uninterrupt()
        {
            // for completeness, lets the queue be used again  
            ((ManualResetEvent)handles[1]).Reset();
        }

        #region IEnumerable

        public IEnumerator GetEnumerator()
        {
            return _q.GetEnumerator();
        }

        #endregion

        #region ICollection

        public void CopyTo(Array array, int index)
        {
            _q.CopyTo((T[])array, index);
        }

        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IEnumerable<T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _q.GetEnumerator();
        }

        #endregion

    }
}
