using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rnd = new Random();
            var watcher = new Stopwatch();
            watcher.Start();

            var test = new TaskTest();
            test.StartSleep(20);
            test.StartDelay(20);
            var tasks = new List<Task>();
            //for (var i = 0; i < 10000; i++)
            //{
            //    var closure = i;
            //    Task.Run(
            //        async () =>
            //        {
            //            Console.WriteLine($"task{closure} start {watcher.ElapsedMilliseconds}");
            //            await Task.Delay(rnd.Next(10000));
            //            Console.WriteLine($"task{closure} finished {watcher.ElapsedMilliseconds}");
            //        });
            //}
            Thread.Sleep(500);
            Console.WriteLine("finished");
            Console.ReadKey();
        }


        public class TaskTest
        {
            public void StartSleep(int nbr)
            {
                for (var i = 0; i < nbr; i++)
                {
                    var closure = i;
                    Task.Run(() =>
                    {
                        for (var j = 0; j < 100; j++)
                        {
                            Console.WriteLine($"{closure}");
                            Thread.Sleep(100);
                        }
                    });
                }
            }

            public void StartDelay(int nbr)
            {
                for (var i = 0; i < nbr; i++)
                {
                    var closure = i;
                    Task.Run(async () =>
                    {
                        for (var j = 0; j < 100; j++)
                        {
                            Console.WriteLine($"{closure}");
                            await Task.Delay(100);
                        }
                    });
                }
            }
        }
    }
}