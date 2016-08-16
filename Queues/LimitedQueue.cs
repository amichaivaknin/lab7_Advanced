using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queues
{
    class LimitedQueue<T>
    {
        private readonly Queue<T> _queue;
        private readonly SemaphoreSlim _semaphore;
        private readonly object _lock;

        public LimitedQueue(int limit)
        {
            _queue=new Queue<T>(limit);
            _semaphore= new SemaphoreSlim(limit);
            _lock = new object();
        }

        public void Enqueue(T insert)
        {
            _semaphore.Wait();
            lock (_lock)
            {
                _queue.Enqueue(insert);
            }
        }

        public T Dequeue()
        {
            T res;
            lock (_lock)
            {
                 res = _queue.Dequeue();
            }
            _semaphore.Release(1);
            return res;
        }
    }
}
