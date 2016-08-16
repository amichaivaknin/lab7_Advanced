using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queues
{
    class Program
    {

        static void Main(string[] args)
        {
            var queue = new LimitedQueue<int>(3);
            for (var i = 1; i <= 5; i++)
            {
                var i1 = i;
                var t = new Thread(()=>queue.Enqueue(i1));
                t.Start();
            }
           
            Thread.Sleep(500);

            for (var i = 1; i <= 5; i++)
            {
                var t = new Thread(() => Console.WriteLine(queue.Dequeue()));
                t.Start();
            }
          Thread.Sleep(10000);  
        }
    }
}

