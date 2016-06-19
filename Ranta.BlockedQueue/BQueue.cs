using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ranta.BlockedQueue
{
    /// <summary>
    /// 提供线程安全的阻塞队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class BQueue<T>
    {
        private Queue<T> queue;

        private object lockObject;

        public const int MaxSize = 5;

        public BQueue()
        {
            this.queue = new Queue<T>();

            this.lockObject = new object();
        }

        public void Enqueue(T t)
        {
            lock (lockObject)
            {
                while (this.queue.Count == 5)
                {
                    Console.WriteLine("队列已满，等待中");

                    Monitor.Wait(lockObject, 500);
                }
                this.queue.Enqueue(t);
                Monitor.PulseAll(lockObject);
            }
        }

        public T Dequeue()
        {
            lock (lockObject)
            {
                while (this.Count == 0)
                {
                    Console.WriteLine("队列为空，等待中");

                    Monitor.Wait(lockObject, 500);
                }
                var t = this.queue.Dequeue();
                Monitor.PulseAll(lockObject);
                return t;
            }
        }

        public int Count
        {
            get
            {
                return this.queue.Count;
            }
        }

    }
}
