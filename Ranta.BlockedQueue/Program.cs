using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ranta.BlockedQueue
{
    class Program
    {
        static BQueue<string> queue;

        static void Main(string[] args)
        {
            queue = new BQueue<string>();

            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(new Task(() =>
                {
                    Request();
                }));
                tasks.Add(new Task(() =>
                {
                    Process();
                }));
            }

            foreach (var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("Press any key to continue.");
            Console.Read();
        }

        static int requestIndex = 1;
        static int processIndex = 1;

        static Random random = new Random();

        static void Request()
        {
            var current = requestIndex++;

            var message = string.Format("This is test {0}.", current);

            Console.WriteLine("正在请求:{0}", message);

            Thread.Sleep(50 * random.Next(5, 10));

            queue.Enqueue(message);

            Console.WriteLine("请求成功");
        }

        static void Process()
        {
            var current = processIndex++;

            Console.WriteLine("即将处理");

            Thread.Sleep(300 * random.Next(5, 10));

            var t = queue.Dequeue();

            Console.WriteLine("正在处理{0}", t);

            Console.WriteLine("处理成功");
        }
    }
}
