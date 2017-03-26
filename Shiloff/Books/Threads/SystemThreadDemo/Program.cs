using System;
using System.Threading;

namespace SystemThreadDemo
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("Main thread: starting a dedicated thread " +
                              "to do an asynchronous operation");
            var dedicatedThread = new Thread(state =>
            {
                Console.WriteLine("In ComputeBoundOp: state={0}", state);
                Thread.Sleep(100000);
                Console.WriteLine("Finished state={0}", state);
            });
            dedicatedThread.Start(5);
            Console.WriteLine("Main thread: Doing other work here...");
            Thread.Sleep(10000); // Имитация другой работы (10 секунд)
            dedicatedThread.Join(); // Ожидание завершения потока
            Console.WriteLine("Hit <Enter> to end this program...");
            Console.ReadLine();
        }
    }
}