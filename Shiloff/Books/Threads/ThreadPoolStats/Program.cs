using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolStats
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int worker, io;
            ThreadPool.GetAvailableThreads(out worker, out io);
            ThreadPool.QueueUserWorkItem((state) =>
            {
                Thread.Sleep(1000);
                Console.WriteLine((int) state);

            }, 5);
            Thread.Sleep(1000);
            Console.WriteLine($"{worker:N0}, {io:N0}");
            Console.ReadKey();
        }
    }
}