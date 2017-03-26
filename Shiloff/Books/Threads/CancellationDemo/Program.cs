using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var token = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    token.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(10);
                    Console.Write(".");
                }
            }, token.Token);


            token.Token.Register(() => { Console.WriteLine("Canceled"); }, true);
            token.Token.Register(() => { Console.WriteLine("Canceled2"); }, true);
            token.Token.Register(() => { Console.WriteLine(""); });
            while (Console.ReadKey().Key != ConsoleKey.Backspace)
            {
            }
            token.Cancel(false);
            Console.ReadKey();
        }
    }
}